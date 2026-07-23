# core/urls.py
from django.urls import path
# Make sure to import all your views from .views
from .views import (
    autenticar, 
    obtener_equipos_en_bodega, 
    obtener_productos, 
    ficha_view, # Assuming ficha_view is still in this file
    consultar_guias_despacho,       # <-- Add this
    actualizar_estado_guia_despacho # <-- Add this
)

urlpatterns = [
    path('autenticar/<tipousu>/<username>/<password>', autenticar, name="autenticar"),

    path('obtener_equipos_en_bodega', obtener_equipos_en_bodega, name='obtener_equipos_en_bodega'),
    path('obtener_productos', obtener_productos, name='obtener_productos'),

    path('ficha/<int:idprod>', ficha_view, name='ficha'),
    
    # --- New URL paths for your new views ---
    path('consultar_guias_despacho', consultar_guias_despacho, name='consultar_guias_despacho'),
    path('actualizar_estado_guia_despacho/<int:nrogd>/<str:estadogd>', actualizar_estado_guia_despacho, name='actualizar_estado_guia_despacho'),
    # --- End new URL paths ---

    # ... other URL patterns
]