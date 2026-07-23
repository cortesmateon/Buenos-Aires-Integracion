using System;
using System.Net.Http;
using BuenosAires.Model.Utiles;
using BuenosAires.Model;
using BuenosAires.BusinessLayer;

namespace BuenosAires.ServiceLayer
{
    public class WsGuiaDespacho : IWsGuiaDespacho
    {
        public Respuesta ConsultarGuiaDespacho()
        {
            var resp = new Respuesta();
            resp.Accion = "obtener Guia de Despacho";
            resp.Mensaje = "";
            resp.HayErrores = false;
            resp.JsonGuiaDespacho = "";

            string apiUrl = "http://127.0.0.1:8000/BuenosAiresApiRest/consultar_guias_despacho";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        resp.JsonGuiaDespacho = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        resp.HayErrores = true;
                        resp.Mensaje = "No fue posible " + resp.Accion + ", intente nuevamente mas tarde "
                            + "o comuníquese con el Administrador del Sistema";
                    }
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.HayErrores = true;
                resp.Mensaje = Util.MensajeError("WsGuiaDespacho.ConsultarGuiaDespacho", ex);
                return resp;
            }
        }

        public Respuesta ActualizarEstadoGuiaDespacho(int nrogd, string estadogd)
        {
            var resp = new Respuesta();
            resp.Accion = "Actualizar Guia Despacho";
            resp.Mensaje = "";
            resp.HayErrores = false;

            string apiUrl = "http://127.0.0.1:8000/BuenosAiresApiRest/actualizar_estado_guia_despacho/" + nrogd.ToString() + "/" + estadogd;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        resp.Mensaje = "La Guia de Despacho Fue Actualizada Correctamente";
                    }
                    else
                    {
                        resp.HayErrores = true;
                        resp.Mensaje = "No fue posible " + resp.Accion + ", intente nuevamente mas tarde "
                            + "o comuníquese con el Administrador del Sistema";
                    }
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.HayErrores = true;
                resp.Mensaje = Util.MensajeError("WsGuiaDespacho.ActualizarEstadoGuiaDespacho", ex);
                return resp;
            }
        }


    }
}