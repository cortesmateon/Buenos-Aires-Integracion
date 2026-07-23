using BuenosAires.Model.Utiles;
using BuenosAires.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAires.DataLayer
{
    public class DcSolicitudServicio
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public SolicitudServicio SolicitudServicio = null;
        public List<SolicitudServicio> Lista = null;

        public DcSolicitudServicio()
        {
            Inicializar("");
        }

        private void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.SolicitudServicio = null;
            this.Lista = null;
        }

        public int ContarSolicitudServiciosPorFactura(int nrofac)
        {
            this.Inicializar($"Contar solicitudes de servicios asociadas a la factura con el número '{nrofac}'");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    int cantidad = bd.SolicitudServicio.Count(x => x.nrofac == nrofac);
                    return cantidad;
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError("DcSolicitudServicio.ContarSolicitudServiciosPorFactura", ex);
                return -1;
            }
        }
    }
}
