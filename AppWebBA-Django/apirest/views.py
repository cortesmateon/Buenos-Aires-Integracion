# Make sure these imports are at the top of your core/views.py file
import datetime
from django.shortcuts import render, get_object_or_404, redirect
from django.views.decorators.csrf import csrf_exempt
from django.http import JsonResponse # For API views
from django.db import connection # For direct SQL execution (if used elsewhere)
from django.contrib.auth import authenticate # Specifically for 'autenticar' view
from django.conf import settings # Needed if you construct MEDIA_URL for images
from typing import cast # For type hinting with PerfilUsuario's __str__ method

from rest_framework.decorators import api_view # For API views
from core.bch_api import get_series

from core.models import Producto, PerfilUsuario # Your models

@csrf_exempt
@api_view(['GET'])
def obtener_equipos_en_bodega(request):
    if request.method == 'GET':
        cursor = connection.cursor()

        cursor.execute("EXEC SP_OBTENER_EQUIPOS_EN_BODEGA")

        results = cursor.fetchall()

        data = []
        for row in results:
            idprod = row[0]
            nomprod = row[1]
            descprod = row[2]
            precio = row[3]
            imagen = row[4]
            cantidad = row[5]
            disponibilidad = row[6]

            data.append({
            'idprod': idprod,
            'nomprod': nomprod,
            'descprod': descprod,
            'precio': precio,
            'imagen': imagen,
            'cantidad': cantidad,
            'disponibilidad': disponibilidad,
            })

        
        return JsonResponse(data, safe=False)

@csrf_exempt
@api_view(['GET'])
def obtener_productos(request):
    if request.method == 'GET':
        cursor = connection.cursor()
        cursor.execute("EXEC SP_OBTENER_PRODUCTOS")
        results = cursor.fetchall()
        data = []
        for row in results:
            data.append({
                'idprod': row[0],
                'nomprod': row[1],
                'descprod': row[2],
                'precio': row[3],
                'imagen': row[4]
            })

      
        return JsonResponse(data, safe=False)
    
@csrf_exempt
@api_view(['GET'])
def consultar_guias_despacho (request):
    if request.method == 'GET':
        cursor = connection.cursor()

        cursor.execute("EXEC SP_OBTENER_GUIA_DESPACHO_API")

        results = cursor.fetchall()

        data = []
        for row in results:
            nrogd = row[0]
            descprod = row[1]
            estadogd= row[2]
            nrofac= row[3]
            Cliente = row[4]
            

            data.append({
            'nrogd': nrogd,
            'descprod': descprod,
            'estadogd': estadogd,
            'nrofac' : nrofac,
            'Cliente' : Cliente,
            })

        
        return JsonResponse(data, safe=False)
@csrf_exempt
@api_view(['GET'])
def actualizar_estado_guia_despacho(request, nrogd, estadogd):
    if request.method == 'GET':
        cursor = connection.cursor()
        cursor.execute(f"EXEC SP_ACTUALIZAR_ESTADO_GUIA_DESPACHO {nrogd}, '{estadogd}'")
        respuesta = {'Completado': nrogd}
        return JsonResponse(respuesta, safe=False)
    
# --- The 'autenticar' API View ---
@csrf_exempt
@api_view(['GET'])
def autenticar(request, tipousu, username, password):
    user = authenticate(username=username, password=password)
    if user:
        try:
            # Use type: ignore to tell Pyright to suppress this specific warning
            perfil = PerfilUsuario.objects.get(user=user) # type: ignore [reportAttributeAccessIssue]
        except PerfilUsuario.DoesNotExist: # type: ignore [reportAttributeAccessIssue]
            # Handle case where a User exists but no PerfilUsuario is linked
            return JsonResponse({'Autenticado': False, 'NombreUsuario': '', 'TipoUsuario': '', 'Mensaje': 'Usuario válido pero sin perfil asociado.'})

        nombre = f'{user.first_name} {user.last_name}'
        tipo = perfil.tipousu
        
        if tipo in [tipousu, 'Administrador']:
            return JsonResponse({'Autenticado': True, 'NombreUsuario': nombre, 'TipoUsuario': tipo, 'Mensaje': ''})
        msg = f'La cuenta de usuario {nombre} es del perfil {tipo}, por lo que no puede ingresar al sistema'
    else:
        nombre, tipo, msg = '', '', 'La cuenta o la contraseña no coinciden con un usuario válido'
    return JsonResponse({'Autenticado': False, 'NombreUsuario': nombre, 'TipoUsuario': tipo, 'Mensaje': msg})



# --- The 'ficha_view' (Product Detail Page View) ---
@csrf_exempt # Use if you're not using standard Django CSRF tokens for this view/form
def ficha_view(request, idprod):
    producto = get_object_or_404(Producto, idprod=idprod)

    precio_usd = None
    error_api = None
    mesg = None

    # Calculate dates for the API request
    today_str = datetime.date.today().strftime('%Y-%m-%d')
    # Request data from the beginning of last year to ensure we capture available historical data
    first_date_for_usd_check = "2024-01-01" 

    # --- USD series ID ---
    usd_series_id = "F073.DOL.OBS.L.ZM.D" # This is the Observed Dollar series ID
    # --- End USD series ID ---

    if request.method == "POST":
        # Check if user is authenticated and not a staff user (assuming staff can't 'buy')
        if request.user.is_authenticated and not request.user.is_staff:
            return redirect('iniciar_pago', idprod=idprod)
        else:
            mesg = "¡Para poder comprar debe iniciar sesión como cliente!"
            # Render the page again with the message if conditions aren't met
            context = {
                'producto': producto,
                'precio_usd': precio_usd, # Still include even if POST
                'error_api': error_api,
                'mesg': mesg
            }
            return render(request, "core/ficha.html", context)

    # Logic for GET requests (and POST requests that fall through without redirecting)
    try:
        # Request data for a wide range to ensure we get the latest observed value
        usd_response = get_series(usd_series_id, first_date=first_date_for_usd_check, last_date=today_str)

        if usd_response and usd_response.get("Codigo") == 0 and usd_response.get("Series") and usd_response["Series"].get("Obs"):
            if usd_response["Series"]["Obs"]: # Check if the 'Obs' list is not empty
                # Get the latest observation (usually the last item in the list for a date range)
                usd_exchange_rate = float(usd_response["Series"]["Obs"][-1]["value"])
                
                # --- DEBUG PRINT STATEMENTS (KEEP FOR NOW TO DIAGNOSE USD PRICE) ---
                print(f"DEBUG: Producto Precio CLP: {producto.precio}")
                print(f"DEBUG: USD Exchange Rate (raw from API): {usd_exchange_rate}")
                print(f"DEBUG: Expected division: {producto.precio} / {usd_exchange_rate}")
                # --- END DEBUG ---

                if producto.precio is not None and usd_exchange_rate > 0: # Ensure producto.precio is not None or zero
                    precio_usd = round(producto.precio / usd_exchange_rate, 2)
                    error_api = None # Clear any previous error if successful
                else:
                    error_api = "Tipo de cambio de USD inválido o precio del producto no definido/cero."
            else:
                error_api = f"No se encontraron observaciones de USD para la serie '{usd_series_id}' entre {first_date_for_usd_check} y {today_str}."
                print(f"API Warning: {error_api}. Response: {usd_response}")
        else:
            # If API call returned an error or no data structure (e.g., Codigo: -50 or -2)
            api_desc = usd_response.get("Descripcion", "Error desconocido en la API.")
            api_code = usd_response.get("Codigo", "N/A")
            
            if api_code == -50:
                error_api = f"Error del Banco Central (Código {api_code}): Sus credenciales tienen acceso a la API, pero la serie '{usd_series_id}' no está disponible o no tiene datos para su cuenta. Verifique sus permisos en el portal del Banco Central."
            elif api_code == -2:
                error_api = f"Error de conexión al Banco Central (Código {api_code}): {api_desc}. Esto puede ser un problema temporal del servidor del Banco Central o una restricción de conexión desde su red/IP."
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