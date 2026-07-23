# core/bch_api.py
import requests
import os
from dotenv import load_dotenv

# Load environment variables from the .env file
load_dotenv()

# Get API credentials from environment variables
BCCH_USER = os.getenv("BCCH_USER")
BCCH_PASS = os.getenv("BCCH_PASS")

# Correct BASE_URL based on documentation: just the base path
BASE_URL = "https://si3.bcentral.cl/SieteRestWS/SieteRestWS.ashx"

def get_series(series_id, first_date, last_date):
    """
    Fetches economic series data from the Banco Central de Chile API.
    Handles JSON response.
    """
    if not BCCH_USER or not BCCH_PASS:
        print("ERROR: BCCH_USER or BCCH_PASS environment variables are not set.")
        return {
            "Codigo": -1,
            "Descripcion": "Credenciales de la API del Banco Central no configuradas.",
            "Series": {"Obs": []} # Maintain consistent structure for error
        }

    params = {
        "user": BCCH_USER,
        "pass": BCCH_PASS,
        "function": "GetSeries",
        "timeseries": series_id,   # --- CRITICAL CHANGE: 'timeseries' instead of 'timeseriesId' ---
        "firstdate": first_date,   # Use 'firstdate' (lowercase 'd')
        "lastdate": last_date      # Use 'lastdate' (lowercase 'd')
    }

    try:
        response = requests.get(BASE_URL, params=params, timeout=10)
        response.raise_for_status() # Raise an exception for HTTP errors (4xx or 5xx)

        # --- IMPORTANT DEBUGGING STEP ---
        print("\n--- API Endpoint URL (actual URL sent) ---")
        print(response.url)
        print("--- RAW BCCH API JSON RESPONSE ---")
        print(response.text) # Print raw text for JSON response
        print("--- END RAW BCCH API JSON RESPONSE ---\n")
        # --- END DEBUGGING STEP ---

        # --- CRITICAL CHANGE: Parse JSON response ---
        data = response.json()

        # Assuming JSON structure like:
        # {
        #   "Series": {
        #     "Obs": [
        #       {"value": "920.50", "date": "2025-06-03"},
        #       ...
        #     ]
        #   }
        # }
        # You might need to adjust this parsing if the actual JSON structure is different.
        
        series_data = []
        if 'Series' in data and 'Obs' in data['Series']:
            series_data = data['Series']['Obs']
        
        # The BCCH API's JSON response often has a "Codigo" and "Descripcion" at the top level
        # so we can use those directly from the 'data' dictionary if available.
        # Otherwise, default to OK if data was found.
        api_codigo = data.get("Codigo", 0)
        api_descripcion = data.get("Descripcion", "OK")

        return {
            "Codigo": api_codigo,
            "Descripcion": api_descripcion,
            "Series": {
                "Obs": series_data
            }
        }

    except requests.exceptions.HTTPError as http_err:
        print(f"HTTP error occurred: {http_err} - Response: {response.text}")
        return {
            "Codigo": response.status_code,
            "Descripcion": f"Error HTTP al comunicarse con el Banco Central: {http_err}",
            "Series": {"Obs": []}
        }
    except requests.exceptions.ConnectionError as conn_err:
        print(f"Connection error occurred: {conn_err}")
        return {
            "Codigo": -2,
            "Descripcion": f"Error de conexión al Banco Central: {conn_err}",
            "Series": {"Obs": []}
        }
    except requests.exceptions.Timeout as timeout_err:
        print(f"Timeout error occurred: {timeout_err}")
        return {
            "Codigo": -3,
            "Descripcion": f"Tiempo de espera agotado al conectar con el Banco Central: {timeout_err}",
            "Series": {"Obs": []}
        }
    except requests.exceptions.RequestException as req_err:
        print(f"An unexpected request error occurred: {req_err}")
        return {
            "Codigo": -4,
            "Descripcion": f"Error desconocido al solicitar datos del Banco Central: {req_err}",
            "Series": {"Obs": []}
        }
    except ValueError as json_err: # Catch JSON decoding errors
        print(f"JSON decode error: {json_err} - Response content: {response.text[:500]}...")
        return {
            "Codigo": -5,
            "Descripcion": f"Error al decodificar la respuesta JSON de la API del Banco Central: {json_err}",
            "Series": {"Obs": []}
        }
    except Exception as e:
        print(f"An unknown error occurred in get_series: {e}")
        return {
            "Codigo": -6,
            "Descripcion": f"Error inesperado en la API del Banco Central: {e}",
            "Series": {"Obs": []}
        }