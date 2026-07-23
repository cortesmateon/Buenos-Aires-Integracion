using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAires.DataLayer
{
    public class DcFactura
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public Factura Factura = null;
        public List<Factura> Lista = null;

        public DcFactura()
        {
            Inicializar("");
        }

        private void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.Factura = null;
            this.Lista = null;
        }

        public int ContarFacturasPorProducto(int idprod)
        {
            this.Inicializar($"contar las facturas asociadas al Factura con el ID '{idprod}'");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    int cantidad = bd.Factura.Count(s => s.idprod == idprod);
                    return cantidad;
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError( "DcFactura.ContarFacturasPorProducto", ex);
                return -1;
            }
        }

        public void LeerFacturasPorProducto(int idprod)
        {
            this.Inicializar($"obtener la lista de facturas asociadas al producto con el ID de Producto '{idprod}'");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    this.Lista = bd.Factura.Where(x => x.idprod == idprod).ToList();
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError( "DcFactura.LeerFacturasPorProducto", ex);
            }
        }
    }
}
