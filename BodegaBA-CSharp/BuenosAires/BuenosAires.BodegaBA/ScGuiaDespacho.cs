using System;
using BuenosAires.BodegaBA.WsGuiaDespachoReference;
using Newtonsoft.Json;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Collections.Generic;
using BuenosAires.Model;

namespace BuenosAires.ServiceProxy
{
    public class ScGuiaDespacho
    {
        public class GuiaDespacho
        {
            public int nrogd { get; set; }
            public string descprod { get; set; }
            public string estadogd { get; set; }
            public int nrofac { get; set; }
            public string Cliente { get; set; }
        }
        
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public string JsonGuiaDespacho= "";
        public List<GuiaDespacho> Lista = new List<GuiaDespacho>();

        public void CopiarPropiedades(Respuesta resp)
        {
            this.Accion = resp.Accion;
            this.Mensaje = resp.Mensaje;
            this.HayErrores = resp.HayErrores;
            this.JsonGuiaDespacho = resp.JsonGuiaDespacho;

            if (resp.JsonGuiaDespacho != "")
            {
                if (resp.JsonGuiaDespacho != null)
                {
                    this.Lista = 
                        JsonConvert.DeserializeObject<List<GuiaDespacho>>(resp.JsonGuiaDespacho);
                }
                
                this.Mensaje = resp.Mensaje;
            }
        }
       
        public WsGuiaDespachoClient getWs()
        {
            var ws = new WsGuiaDespachoClient();
            ws.InnerChannel.OperationTimeout = new TimeSpan(1, 0, 0);
            return ws;
        }

        public void ObtenerGuiaDespacho()
        {
            CopiarPropiedades(getWs().ConsultarGuiaDespacho());
        }

        public void ActualizarEstadoGuiaDespacho(int nrogd, string estadogd)
        {
           CopiarPropiedades(getWs().ActualizarEstadoGuiaDespacho(nrogd, estadogd));
        }
        
    }
}
