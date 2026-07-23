using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuenosAires.Model;
using BuenosAires.DataLayer;

namespace BuenosAires.BusinessLayer
{
    public class BcProducto
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public Producto Producto = null;
        public List<Producto> Lista = null;


        public BcProducto()
        {
            Inicializar("");
        }
        public static void MostrarMensajeError(string mensaje)
        {

        }
        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.Lista = null;
        }
        public void CopiarPropiedades(DcProducto dc)
        {
            this.Accion = dc.Accion;
            this.Mensaje = dc.Mensaje;
            this.HayErrores = dc.HayErrores;
            this.Producto = dc.Producto;
            this.Lista = dc.Lista;
        }
        public bool RetornarError(string mensaje)
        {
            this.HayErrores = true;
            this.Mensaje = mensaje;
            return false;
        }
        public int ObtenerSiguienteId()
        {
            var dc = new DcProducto();
            int siguienteId = dc.ObtenerSiguienteId();
            this.CopiarPropiedades(dc);
            return siguienteId;
        }
        public bool ValidarProducto(Producto producto)
        {
            
            if (producto.nomprod.Trim() == "") return RetornarError($"El campo nombre de producto debe tener un valor");
            producto.nomprod = producto.nomprod.Trim();
            if (producto.descprod.Trim() == "") return RetornarError($"El campo descripcion de producto debe tener un valor");
            producto.descprod = producto.descprod.Trim();
            if (producto.precio <= 0) return RetornarError($"El campo precio debe ser mayor a 0");
            if (producto.imagen.Trim() == "") return RetornarError($"El campo imagen de producto debe tener un valor");
            producto.imagen = producto.imagen.Trim();
            return true;
        }
        public void Crear(Producto producto)
        {
            if (ValidarProducto(producto) == false) return;
            var dc = new DcProducto();            
            dc.Crear(producto);           
            this.CopiarPropiedades(dc);
        }
        public void LeerTodos()
        {
            var dc = new DcProducto();
            dc.LeerTodos();
            this.CopiarPropiedades(dc);

        }
        public void Leer(int id)
        {
            var dc = new DcProducto();
            dc.Leer(id);
            this.CopiarPropiedades(dc);
        }
        public void Actualizar(Producto producto)
        {
            if (ValidarProducto(producto) == false) return;
            var dc = new DcProducto();
            dc.Actualizar(producto);
            this.CopiarPropiedades(dc);
        }
        public bool Eliminar(int id)
        {
            var dcprod = new DcProducto();
            dcprod.Eliminar(id);
            if (dcprod.HayErrores) return RetornarError(dcprod.Mensaje);
            this.CopiarPropiedades(dcprod);
            return true;

        }
    }
}

