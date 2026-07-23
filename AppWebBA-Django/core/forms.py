# your_app/forms.py
from django import forms
from django.forms import ModelForm, fields, Form
from django.contrib.auth.models import User
from django.contrib.auth.forms import UserCreationForm
from .models import Producto, PerfilUsuario

class ProductoForm(ModelForm):
    class Meta:
        model = Producto
        fields = ['idprod', 'nomprod', 'descprod', 'precio', 'imagen']

class IniciarSesionForm(Form):
    username = forms.CharField(widget=forms.TextInput(), label="Correo")
    password = forms.CharField(widget=forms.PasswordInput(), label="Contraseña")
    # No Meta class for a non-ModelForm unless it's just for field order,
    # but not standard practice for plain Form. Keep it simple.
    # class Meta:
    #     fields = ['username', 'password']

class RegistrarUsuarioForm(UserCreationForm):
    # Overriding the email field to ensure it's required and includes common validators
    email = forms.EmailField(
        label="Correo Electrónico",
        max_length=254,
        required=True,
        help_text="Por favor, ingresa una dirección de correo válida."
    )
    first_name = forms.CharField(max_length=150, required=True, label="Nombres")
    last_name = forms.CharField(max_length=150, required=True, label="Apellidos")
    rut = forms.CharField(max_length=20, required=True, label="Rut")
    # For tipousu, using a ChoiceField and your model's choices is best practice
    # Ensure this initial value ('Cliente') matches one of your choices
    tipousu = forms.ChoiceField(
        choices=PerfilUsuario.TIPOUSU_CHOICES,
        label="Tipo de usuario",
        initial='Cliente', # Set a sensible default for new registrations
        required=True
    )
    dirusu = forms.CharField(max_length=300, required=True, label="Dirección")

    class Meta(UserCreationForm.Meta): # Inherit Meta from UserCreationForm
        model = User
        # Include UserCreationForm's default fields (username, password, password2)
        # AND your custom fields for the User model, then add PerfilUsuario fields.
        # Ensure 'email', 'first_name', 'last_name' are in 'fields' if you want them on User.
        fields = UserCreationForm.Meta.fields + ('first_name', 'last_name', 'email', 'rut', 'tipousu', 'dirusu',)

    # Custom validation for RUT (Chilean RUT format and verifier digit)
    def clean_rut(self):
        rut = self.cleaned_data['rut']
        # Remove dots and hyphens for validation
        rut_cleaned = rut.replace('.', '').replace('-', '')

        if not rut_cleaned.isalnum() or len(rut_cleaned) < 2:
            raise forms.ValidationError("Formato de RUT inválido. Ej: 12345678-9")

        body = rut_cleaned[:-1]
        dv = rut_cleaned[-1].lower() # Verifier digit

        if not body.isdigit():
            raise forms.ValidationError("Formato de RUT inválido. El cuerpo debe ser numérico.")

        # Basic RUT validation algorithm (checksum)
        revertido = map(int, reversed(str(body)))
        factors = [2, 3, 4, 5, 6, 7]
        s = sum(f * d for f, d in zip(factors * (len(body) // len(factors) + 1), revertido))

        calculated_dv = 11 - (s % 11)
        if calculated_dv == 11:
            calculated_dv = '0'
        elif calculated_dv == 10:
            calculated_dv = 'k'
        else:
            calculated_dv = str(calculated_dv)

        if dv != calculated_dv:
            raise forms.ValidationError("RUT inválido. El dígito verificador no coincide.")

        # Also check if RUT already exists in PerfilUsuario
        if PerfilUsuario.objects.filter(rut=rut).exists():
            raise forms.ValidationError("Este RUT ya está registrado.")

        return rut # Return the original rut for storage


    # Custom validation for email format and uniqueness
    def clean_email(self):
        email = self.cleaned_data['email']
        if not '@' in email or '.' not in email:
            raise forms.ValidationError("Por favor, ingresa una dirección de correo válida (debe contener '@' y '.').")
        # Ensure email is unique across User model
        if User.objects.filter(email=email).exists():
            raise forms.ValidationError("Este correo electrónico ya está registrado.")
        return email

    # Override the save method to handle both User and PerfilUsuario creation
    def save(self, commit=True):
        # 1. Save the User object first (this handles username, password, password2, email, first_name, last_name)
        user = super().save(commit=False) # Get the User instance but don't save yet

        # Update User fields that come from the form but are not default UserCreationForm fields
        user.first_name = self.cleaned_data['first_name']
        user.last_name = self.cleaned_data['last_name']
        user.email = self.cleaned_data['email'] # Ensure email is set on User model

        if commit:
            user.save() # Save the User instance to the database

            # 2. Create the PerfilUsuario instance and link it to the new User
            perfil_usuario = PerfilUsuario.objects.create(
                user=user,
                rut=self.cleaned_data['rut'],
                tipousu=self.cleaned_data['tipousu'],
                dirusu=self.cleaned_data['dirusu']
            )

        return user # Return the user object

class PerfilUsuarioForm(ModelForm): # <--- THIS MUST BE ModelForm, NOT Form
    first_name = forms.CharField(max_length=150, required=True, label="Nombres")
    last_name = forms.CharField(max_length=150, required=True, label="Apellidos")
    email = forms.EmailField(max_length=254, required=True, label="Correo")
    
    # Keeping 'rut', 'tipousu', and 'dirusu' as explicitly defined fields.
    # Note: If 'rut' is your primary_key, you might want it read-only on update.
    rut = forms.CharField(max_length=20, required=True, label="Rut")
    tipousu = forms.CharField(max_length=50, required=True, label="Tipo de usuario")
    dirusu = forms.CharField(max_length=300, required=True, label="Dirección") # Make required=True as per model

    class Meta:
        model = PerfilUsuario
        # Specify the fields from the PerfilUsuario model that the form should manage.
        # Since you've explicitly defined rut, tipousu, and dirusu above,
        # you actually don't *need* to list them here in 'fields' if you want full control
        # via the explicit fields. However, listing them here is harmless and often
        # clearer for ModelForm behavior.
        # It's best practice to let ModelForm handle the fields that directly map
        # to the model, and then add custom fields for the User model data.
        # Let's keep it simple:
        fields = ['rut', 'tipousu', 'dirusu']

    def __init__(self, *args, **kwargs):
        # Extract the PerfilUsuario instance if it's passed
        instance = kwargs.get('instance')
        super().__init__(*args, **kwargs)

        # If an instance (PerfilUsuario object) exists, populate the form's User-related fields.
        if instance and instance.user:
            self.fields['first_name'].initial = instance.user.first_name
            self.fields['last_name'].initial = instance.user.last_name
            self.fields['email'].initial = instance.user.email
            # If RUT should be read-only for existing profiles, set it here:
            # self.fields['rut'].widget.attrs['readonly'] = True
            # self.fields['rut'].required = False # No longer required on update

    # Add custom clean methods if you need specific validation beyond basic field validation
    # or model-level validation for these custom fields.
    def clean_tipousu(self):
        tipousu = self.cleaned_data['tipousu']
        valid_choices = [choice[0] for choice in PerfilUsuario.TIPOUSU_CHOICES]
        if tipousu not in valid_choices:
            raise forms.ValidationError(f"Tipo de usuario inválido. Debe ser uno de: {', '.join(valid_choices)}")
        return tipousu
    

class ContactForm(forms.Form):
    name = forms.CharField(
        max_length=100, 
        label="Tu Nombre",
        widget=forms.TextInput(attrs={'placeholder': 'Escribe tu nombre'})
    )
    email = forms.EmailField(
        label="Tu Correo Electrónico",
        widget=forms.EmailInput(attrs={'placeholder': 'ejemplo@correo.com'})
    )
    message = forms.CharField(
        label="Tu Mensaje",
        widget=forms.Textarea(attrs={'rows': 5, 'placeholder': 'Escribe tu mensaje aquí...'})
    )