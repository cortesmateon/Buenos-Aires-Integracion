using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;

namespace BuenosAires.DataLayer
{
    public class DcStockProducto
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public StockProducto StockProducto = null;
        public List<StockProducto> Lista = null;

		public DcStockProducto() 
        {
            Inicializar("");
        }

        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.StockProducto = null;
            this.Lista = null;
        }

		public int ContarStockProductoPorProducto(int idprod)
		{
			this.Inicializar($"Contar productos en bodega asociados al producto con ID '{idprod}'");
			try
			{
				using (var bd = new base_datosEntities())
				{
					int cantidad = bd.StockProducto.Count(x => x.idprod == idprod);
					return cantidad;
				}
			}
			catch (Exception ex)
			{
				this.HayErrores = true;
				this.Mensaje = Util.MensajeError("DcStockProducto.ContarStockProductoPorProducto", ex);
				return -1;
			}
		}
	}
}
