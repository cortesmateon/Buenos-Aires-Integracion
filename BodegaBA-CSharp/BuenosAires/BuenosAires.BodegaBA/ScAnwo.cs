using System;
using BuenosAires.BodegaBA.WsAnwoReference;
using Newtonsoft.Json;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Collections.Generic;
using BuenosAires.Model;

namespace BuenosAires.ServiceProxy
{
    public class ScAnwo
    {
        public class StockProductoAnwo
        {
            public string nroserieanwo { get; set; }
            public string nomprodanwo { get; set; }
            public int precioanwo { get; set; }
            public string reservado { get; set; }
        }
        
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public string JsonAnwo= "";
        public List<StockProductoAnwo> Lista = new List<StockProductoAnwo>();

        public void CopiarPropiedades(Respuesta resp)
        {
            this.Accion = resp.Accion;
            this.Mensaje = resp.Mensaje;
            this.HayErrores = resp.HayErrores;
            this.JsonAnwo = resp.JsonAnwo;

            if (resp.JsonAnwo != "")
            {
                if(resp.JsonAnwo != null)
                {
                    this.Lista = 
                        JsonConvert.DeserializeObject<List<StockProductoAnwo>>(resp.JsonAnwo);
                    
                }
                this.Mensaje = resp.Mensaje;
            }
        }
       
        public WsAnwoClient getWs()
        {
            var ws = new WsAnwoClient();
            ws.InnerChannel.OperationTimeout = new TimeSpan(1, 0, 0);
            return ws;
        }

        public void ObtenerStockProductoAnwo()
        {
            CopiarPropiedades(getWs().LeerTodoAnwoStockProducto());
        }
        
        public void ReservarEquipoAnwo(string nroserieanwo, string reservar_si_o_no)
        {
            CopiarPropiedades(getWs().ReservarEquipoAnwo(nroserieanwo, reservar_si_o_no));
        }
    }
}
