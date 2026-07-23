using System;
using System.Net.Http;
using BuenosAires.Model.Utiles;
using BuenosAires.Model;
using BuenosAires.BusinessLayer;
using BuenosAires.ServiceLayer;

namespace BuenosAires.ServiceLayer
{


    public class WsStockProducto : IWsStockProducto
    {
        public Respuesta ObtenerEquiposEnBodega()
        {
            var res = new Respuesta();
            res.Accion = "obtener equipos en bodega";
            res.Mensaje = "";
            res.HayErrores = false;
            res.JsonStockProducto = "";

            string apiUrl = "http://127.0.0.1:8000/BuenosAiresApiRest/obtener_equipos_en_bodega";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        res.JsonStockProducto = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        res.HayErrores = true;
                        res.Mensaje = "No fue posible " + res.Accion + ", intente nuevamente mas tarde "
                            + "o comuníquese con el Administrador del Sistema";
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.HayErrores = true;
                res.Mensaje = Util.MensajeError("WsStockProducto.ObtenerEquiposEnBodega", ex);
                return res;
            }
        }
    }
}
