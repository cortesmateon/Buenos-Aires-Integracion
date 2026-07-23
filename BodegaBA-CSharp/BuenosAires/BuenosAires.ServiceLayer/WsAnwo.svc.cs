using System;
using System.Net.Http;
using BuenosAires.Model.Utiles;
using BuenosAires.Model;
using BuenosAires.BusinessLayer;
using Newtonsoft.Json;
using System.Text;

namespace BuenosAires.ServiceLayer
{


    public class WsAnwo : IWsAnwo
    {
        public Respuesta LeerTodoAnwoStockProducto()
        {
            var resp = new Respuesta();
            resp.Accion = "Obtener Productos en Anwo";
            resp.Mensaje = "";
            resp.HayErrores = false;
            resp.JsonAnwo = "";

            string apiUrl = "http://127.0.0.1:5000/leer_todos_anwo_stock_producto";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        resp.JsonAnwo = response.Content.ReadAsStringAsync().Result;
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
                resp.Mensaje = Util.MensajeError("WsAnwo.LeerTodoAnwoStockProducto", ex);
                return resp;
            }
        }

        public Respuesta ReservarEquipoAnwo(string nroserieanwo, string reservar_si_o_no)
        {
            var resp = new Respuesta();
            resp.Accion = "Reservar Equipo";
            resp.Mensaje = "";
            resp.HayErrores = false;

            string apiUrl = "http://127.0.0.1:5000/reservar_equipo_anwo";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var requestData = new
                    {
                        nroserieanwo = nroserieanwo,
                        reservado = reservar_si_o_no
                    };

                    string jsonData = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        resp.Mensaje = response.Content.ReadAsStringAsync().Result;
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
                resp.Mensaje = Util.MensajeError("WsAnwo.ReservarEquipoAnwo", ex);
                return resp;
            }
        }


    }
}