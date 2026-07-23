# core/views.py

# --- Standard Django and Python Imports ---
from django.contrib.auth import login, logout, authenticate
from django.contrib.auth.models import User
from django.shortcuts import render, redirect, get_object_or_404
from django.views.decorators.csrf import csrf_exempt
from django.contrib.auth.decorators import login_required
import datetime
import random
import traceback # For debugging exceptions

# --- Transbank Imports ---
from transbank.webpay.webpay_plus.transaction import Transaction, WebpayOptions

# --- Local App Imports (Models, Forms, Custom API Client) ---
from .models import Producto, PerfilUsuario, SolicitudServicio
from .forms import ProductoForm, IniciarSesionForm, RegistrarUsuarioForm, PerfilUsuarioForm, ContactForm
from .bch_api import get_series # Crucial: Ensure core/bch_api.py exists and is accessible

# --- General Views ---
def home(request):
    return render(request, "core/home.html")

def tienda(request):
    data = {"list": Producto.objects.all().order_by('idprod')}
    return render(request, "core/tienda.html", data)

# --- Authentication Views ---
def iniciar_sesion(request):
    if request.method == 'POST':
        form = IniciarSesionForm(request.POST)
        if form.is_valid():
            username = form.cleaned_data['username']
            password = form.cleaned_data['password']
            user = authenticate(request, username=username, password=password)
            if user is not None:
                login(request, user)
                return redirect('home') # Or wherever you redirect after login
            else:
                mesg = "Usuario o contraseña inválidos."
                return render(request, "core/iniciar_sesion.html", {'form': form, 'mesg': mesg})
        else:
            mesg = "Por favor, corrige los errores del formulario."
            return render(request, "core/iniciar_sesion.html", {'form': form, 'mesg': mesg})
    else:
        form = IniciarSesionForm()
    return render(request, "core/iniciar_sesion.html", {'form': form, 'mesg': None})

def cerrar_sesion(request):
    logout(request)
    return redirect(home)

def registrar_usuario(request):
    print("--- registrar_usuario view called ---") # Debug print
    mesg = None # Initialize mesg for general messages

    if request.method == 'POST':
        print("Request method is POST.") # Debug print
        form = RegistrarUsuarioForm(request.POST)
        print("RegistrarUsuarioForm initialized with POST data.") # Debug print

        if form.is_valid():
            print("Form is VALID! Attempting to save...") # Debug print
            try:
                user = form.save()
                print(f"User '{user.username}' (ID: {user.id}) created successfully.") # Debug print
                if hasattr(user, 'perfilusuario'):
                    print(f"PerfilUsuario for user '{user.username}' created/linked. RUT: {user.perfilusuario.rut}")
                else:
                    print(f"WARNING: PerfilUsuario for user '{user.username}' NOT found after save.")
                print("Redirecting to 'iniciar_sesion'...") # Debug print
                return redirect('iniciar_sesion')
            except Exception as e:
                print(f"ERROR: Exception caught during form.save(): {e}") # Debug print
                traceback.print_exc()
                mesg = f"Ocurrió un error inesperado al intentar registrar el usuario: {e}"
        else:
            print("Form is NOT valid.") # Debug print
            print("Form errors (as_json):", form.errors.as_json())
            print("Form errors (as_text):", form.errors.as_text())
            if form.non_field_errors():
                mesg = "Por favor, corrige los errores generales del formulario: " + str(form.non_field_errors())
            else:
                mesg = "Por favor, corrige los errores específicos en el formulario."
            print("Rendering form again with validation errors.") # Debug print
            return render(request, "core/registrar_usuario.html", context={'form': form, 'mesg': mesg})
    else:
        print("Request method is GET. Initializing empty form.") # Debug print
        form = RegistrarUsuarioForm()

    print("Rendering registrar_usuario.html") # Debug print
    return render(request, "core/registrar_usuario.html", context={'form': form, 'mesg': mesg})

@login_required
def perfil_usuario(request):
    user = request.user
    perfil = None
    mesg = None # Initialize mesg

    try:
        perfil = user.perfilusuario
    except PerfilUsuario.DoesNotExist:
        pass

    if request.method == 'POST':
        form = PerfilUsuarioForm(request.POST, instance=perfil)
        
        if form.is_valid():
            user.first_name = form.cleaned_data.get("first_name", user.first_name)
            user.last_name = form.cleaned_data.get("last_name", user.last_name)
            user.email = form.cleaned_data.get("email", user.email)
            user.save()

            perfil_instance = form.save(commit=False)
            perfil_instance.user = user
            perfil_instance.save()
            
            perfil = perfil_instance # Update the 'perfil' variable with the new/updated instance
            
            mesg = "¡Sus datos fueron actualizados correctamente!"
        else:
            mesg = "Por favor, corrige los errores del formulario."
    else:
        form = PerfilUsuarioForm(instance=perfil)
        # mesg remains None for GET request if not set during POST
        
    return render(request, "core/perfil_usuario.html", {'form': form, 'mesg': mesg})

# --- Product Detail and Payment Views ---
@csrf_exempt
def ficha_view(request, idprod):
    producto = get_object_or_404(Producto, idprod=idprod)

    precio_usd = None
    error_api = None
    mesg = None

    # Calculate dates for the API request
    today_str = datetime.date.today().strftime('%Y-%m-%d')
    # Request data from the beginning of last year to ensure we capture available historical data
    first_date_for_usd_check = "2024-01-01" 

    # --- Revert to the correct USD series ID ---
    usd_series_id = "F049.OCU.PMT.INE4.39K.M" # This is the Observed Dollar series ID
    # --- End important change ---

    if request.method == "POST":
        if request.user.is_authenticated and not request.user.is_staff:
            return redirect('iniciar_pago', idprod=idprod)
        else:
            mesg = "¡Para poder comprar debe iniciar sesión como cliente!"
            context = {
                'producto': producto,
                'precio_usd': precio_usd, # Still include even if POST
                'error_api': error_api,
                'mesg': mesg
            }
            return render(request, "core/ficha.html", context)

    # Logic for GET requests (and POST requests that don't redirect)
    try:
        # Request data for a wide range to ensure we get the latest observed value
        usd_response = get_series(usd_series_id, first_date=first_date_for_usd_check, last_date=today_str)

        if usd_response and usd_response.get("Codigo") == 0 and usd_response.get("Series") and usd_response["Series"].get("Obs"):
            if usd_response["Series"]["Obs"]: # Check if the 'Obs' list is not empty
                # Get the latest observation (usually the last item in the list for a date range)
                usd_exchange_rate = float(usd_response["Series"]["Obs"][-1]["value"])
                
                if producto.precio is not None and usd_exchange_rate > 0: # Ensure producto.precio is not None
                    precio_usd = round(producto.precio / usd_exchange_rate, 2)
                    error_api = None # Clear any previous error if successful
                else:
                    error_api = "Tipo de cambio de USD inválido o precio del producto no definido/cero."
            else:
                error_api = f"No se encontraron observaciones de USD para la serie '{usd_series_id}' entre {first_date_for_usd_check} y {today_str}."
                print(f"API Warning: {error_api}. Response: {usd_response}")
        else:
            # If API call returned an error or no data structure (e.g., Codigo: -50)
            api_desc = usd_response.get("Descripcion", "Error desconocido en la API.")
            api_code = usd_response.get("Codigo", "N/A")
            
            if api_code == -50:
                error_api = f"Error del Banco Central (Código {api_code}): Sus credenciales tienen acceso a la API, pero la serie '{usd_series_id}' no está disponible o no tiene datos para su cuenta. Verifique sus permisos en el portal del Banco Central."
            else:
                error_api = f"Error de la API del Banco Central (Código {api_code}): {api_desc}"
            
            print(f"API Error: {error_api}. Full Response: {usd_response}")

    except Exception as e:
        error_api = f"Error inesperado al obtener el tipo de cambio del USD: {e}"
        print(f"Unexpected Error in ficha_view: {e}")

    context = {
        'producto': producto,
        'precio_usd': precio_usd,
        'error_api': error_api,
        'mesg': mesg
    }
    return render(request, 'core/ficha.html', context)
@csrf_exempt
@login_required # Ensure user is logged in to initiate payment
def iniciar_pago(request, idprod): # Change 'id' to 'idprod' to match product ID

    print("Webpay Plus Transaction.create")
    buy_order = str(random.randrange(1000000, 99999999))
    session_id = request.user.username
    
    # Use get_object_or_404 to ensure product exists before trying to get its price
    producto = get_object_or_404(Producto, idprod=idprod)
    amount = producto.precio

    return_url = request.build_absolute_uri('/pago_exitoso/')

    commercecode = "597055555532"
    apikey = "579B532A7440BB0C9079DED94D31EA1615BACEB56610332264630D42D0A36B1C"

    tx = Transaction(options=WebpayOptions(commerce_code=commercecode, api_key=apikey, integration_type="TEST"))
    response = tx.create(buy_order, session_id, amount, return_url)
    print(response['token'])

    # Safely get PerfilUsuario
    try:
        perfil = PerfilUsuario.objects.get(user=request.user)
    except PerfilUsuario.DoesNotExist:
        # If profile doesn't exist, redirect to profile setup
        return redirect('perfil_usuario') # Assuming you have a URL named 'perfil_usuario' for profile setup

    context = {
        "buy_order": buy_order,
        "session_id": session_id,
        "amount": amount,
        "return_url": return_url,
        "response": response,
        "token_ws": response.get('token'), # Use .get() for safety
        "url_tbk": response.get('url'),     # Use .get() for safety
        "first_name": request.user.first_name,
        "last_name": request.user.last_name,
        "email": request.user.email,
        "rut": perfil.rut,
        "dirusu": perfil.dirusu,
    }

    return render(request, "core/iniciar_pago.html", context)

@csrf_exempt
def pago_exitoso(request):
    if request.method == "GET":
        token = request.GET.get("token_ws")
        if not token: # Handle case where token is not in GET params
            print("Error: No token_ws received in GET request for pago_exitoso.")
            # Maybe redirect to an error page or home
            return redirect('home')

        print("commit for token_ws: {}".format(token))
        commercecode = "597055555532"
        apikey = "579B532A7440BB0C9079DED94D31EA1615BACEB56610332264630D42D0A36B1C"
        tx = Transaction(options=WebpayOptions(commerce_code=commercecode, api_key=apikey, integration_type="TEST"))
        
        try:
            response = tx.commit(token=token)
            print("response: {}".format(response))

            # Check if commit was successful and session_id is available
            if response and response.get('session_id'):
                user = get_object_or_404(User, username=response['session_id']) # Use get_object_or_404
                perfil = get_object_or_404(PerfilUsuario, user=user) # Use get_object_or_404

                context = {
                    "buy_order": response.get('buy_order'),
                    "session_id": response.get('session_id'),
                    "amount": response.get('amount'),
                    "response": response,
                    "token_ws": token,
                    "first_name": user.first_name,
                    "last_name": user.last_name,
                    "email": user.email,
                    "rut": perfil.rut,
                    "dirusu": perfil.dirusu,
                    "response_code": response.get('response_code')
                }
                return render(request, "core/pago_exitoso.html", context)
            else:
                # Handle cases where Transbank commit failed or didn't return expected data
                error_mesg = "Error al confirmar la transacción con Transbank o datos de sesión incompletos."
                print(f"Transbank commit error: {response}")
                return render(request, "core/pago_fallido.html", {'mesg': error_mesg, 'response': response}) # Render a failed page

        except Exception as e:
            error_mesg = f"Ocurrió un error al procesar el pago: {e}"
            print(f"Exception during Transbank commit: {e}")
            traceback.print_exc()
            return render(request, "core/pago_fallido.html", {'mesg': error_mesg}) # Render a failed page

    else: # If method is not GET (e.g., POST directly to /pago_exitoso/)
        return redirect('home') # Redirect to home or an appropriate page

@csrf_exempt
@login_required # Only staff can administer products, so they must be logged in
def administrar_productos(request, action, id): # 'id' here is product ID for upd/del
    if not (request.user.is_staff): # Simplified condition
        return redirect('home') # Use URL name for redirect

    data = {"mesg": "", "form": None, "action": action, "id": id, "formsesion": IniciarSesionForm} # Initialize form to None

    if action == 'ins':
        if request.method == "POST":
            form = ProductoForm(request.POST, request.FILES)
            if form.is_valid(): # Corrected: Added parentheses
                try:
                    form.save()
                    data["mesg"] = "¡El producto fue creado correctamente!"
                except Exception as e: # Catch specific exception if possible, or general Exception
                    # Check for unique constraint violation on idprod
                    if 'idprod' in str(e).lower(): # Generic check for common DB error messages
                        data["mesg"] = "¡No se puede crear dos productos con el mismo ID!"
                    else:
                        data["mesg"] = f"¡Ocurrió un error al crear el producto: {e}!"
                        traceback.print_exc() # Print full traceback
            else:
                data["mesg"] = "¡Por favor, corrige los errores del formulario!"
        else: # GET request for 'ins'
            data["form"] = ProductoForm() # Provide empty form for GET
            
    elif action == 'upd':
        objeto = get_object_or_404(Producto, idprod=id) # Use get_object_or_404
        if request.method == "POST":
            form = ProductoForm(data=request.POST, files=request.FILES, instance=objeto)
            if form.is_valid(): # Corrected: Added parentheses
                form.save()
                data["mesg"] = "¡El producto fue actualizado correctamente!"
            else:
                data["mesg"] = "¡Por favor, corrige los errores del formulario!"
        data["form"] = ProductoForm(instance=objeto) # Always provide form for GET or invalid POST

    elif action == 'del':
        try:
            Producto.objects.get(idprod=id).delete()
            data["mesg"] = "¡El producto fue eliminado correctamente!"
            return redirect('administrar_productos', action='ins', id='-1') # Use URL name and id as string
        except Producto.DoesNotExist: # More specific exception handling
            data["mesg"] = "¡El producto ya estaba eliminado o no existe!"
        except Exception as e: # Catch other potential errors
            data["mesg"] = f"¡Ocurrió un error al eliminar el producto: {e}!"
            traceback.print_exc() # Print full traceback
        
    data["list"] = Producto.objects.all().order_by('idprod')
    return render(request, "core/administrar_productos.html", data)

@login_required
def obtener_solicitudes_de_servicio(request):
    user = request.user
    tipousu = None

    try:
        perfil = PerfilUsuario.objects.get(user=user)
        tipousu = perfil.tipousu
    except PerfilUsuario.DoesNotExist:
        print(f"User {user.username} does not have a PerfilUsuario. Redirecting to profile setup.")
        return redirect('perfil_usuario')

    # Example of fetching solicitudes if needed, replace with your actual logic
    # solicitudes = SolicitudServicio.objects.filter(...) # Filter based on user/tipousu

    data = {
        'tipousu': tipousu,
        # 'solicitudes': solicitudes # If you fetch them
    }
    return render(request, "core/obtener_solicitudes_de_servicio.html", data)

def contact_view(request):
    mesg = ''
    if request.method == 'POST':
        form = ContactForm(request.POST)
        if form.is_valid():
            name = form.cleaned_data['name']
            email = form.cleaned_data['email']
            message = form.cleaned_data['message']

            print("\n--- EMULATING EMAIL SENDING ---")
            print(f"To: contact@yourdomain.com (or your admin email)")
            print(f"Subject: Mensaje de contacto de {name}")
            print(f"From: {email}")
            print("--- Message Body ---")
            print(message)
            print("-------------------------------\n")
            
            mesg = '¡Gracias por tu mensaje! Hemos recibido tu información.'
            form = ContactForm()
        else:
            mesg = 'Por favor, corrige los errores en el formulario.'
    else:
        form = ContactForm()
    
    context = {
        'form': form,
        'mesg': mesg
    }
    return render(request, 'core/contact.html', context)