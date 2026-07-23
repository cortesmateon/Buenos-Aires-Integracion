using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuenosAires.Model;
using BuenosAires.Model.Utiles;
using BuenosAires.DataLayer;

namespace BuenosAires.DataLayer
{
    public class DcProducto
    {
        public string Accion = "";
        public string Mensaje = "";
        public bool HayErrores = false;
        public Producto Producto = null;
        public List<Producto> Lista = null;


        public DcProducto()
        {
            Inicializar("");
        }
        public void Inicializar(string accion)
        {
            this.Accion = accion;
            this.Mensaje = "";
            this.HayErrores = false;
            this.Lista = null;
        }
        public int ObtenerSiguienteId()
        {
            Inicializar("Obtener un nuevo id");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    int siguienteID = 1;
                    if (bd.Producto.Count() > 0) siguienteID = bd.Producto.Max(p => p.idprod) + 1;

                    this.Mensaje = "Se ha logrado crear el nuevo id";

                    return siguienteID;
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError("DcPRoducto.ObtenerSiguienteID", ex);
                return -1;
            }
        }
        public bool Crear(Producto producto)
        {
            this.Inicializar($"cear el producto '{producto.nomprod}");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    int siguienteID = this.ObtenerSiguienteId();
                    if (this.HayErrores)
                    {

                    }
                    ;
                    producto.idprod = siguienteID;
                    bd.Producto.Add(producto);
                    bd.SaveChanges();
                    this.Producto = producto;
                    this.Mensaje = $"El producto '{producto.nomprod}' fue creado correctamente ";
                    

                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError("DcProducto.Crear", ex);
            }
            return true;
        }

        public void LeerTodos()
        {
            this.Accion = "Obtener la lista de productos";
            try
            {
                using (var bd = new base_datosEntities())
                {
                    this.Lista = bd.Producto.ToList();
                    if (this.Lista.Count == 0) this.Mensaje = "La lista de productos esta vacía.";


                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError("DcProducto.LeerTodos", ex);

            }

        }
        public void Leer(int idprod)
        {

            this.Inicializar($"Obtener producto con id '{idprod}' ");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    this.Producto = bd.Producto.FirstOrDefault(r => r.idprod == idprod);
                    if (this.Producto == null) this.Mensaje = $"No fue posible '{this.Accion}' no existe en la base de datos";
                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError("DcProducto.Leer", ex);
            }
        }
        public bool Actualizar(Producto producto)
        {
            this.Inicializar($"Actualizar producto  '{producto.nomprod}' ");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    Producto encontrado = bd.Producto.FirstOrDefault(x => x.idprod == producto.idprod);
                    if (encontrado == null)
                    {
                        this.Mensaje = $"No se pudo actualizar '{this.Accion}' no existe en la base de datos";
                        return false;
                    }
                    else
                    {
                        encontrado.idprod = producto.idprod;
                        encontrado.nomprod = producto.nomprod;
                        encontrado.descprod = producto.descprod;
                        encontrado.precio = producto.precio;
                        encontrado.imagen = producto.imagen;
                        bd.SaveChanges();
                        this.Mensaje = $"El producto '{producto.nomprod}' fue actualizado correctamente ";
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError("DcProducto.Actualizar", ex);
                return false;
            }
            
        }
        public void Eliminar(int idprod)
        {
            this.Inicializar($"Eliminar producto con '{idprod}' ");
            try
            {
                using (var bd = new base_datosEntities())
                {
                    var encontrado = bd.Producto.FirstOrDefault(x => x.idprod == idprod);
                    if (encontrado == null)
                    {
                        this.Mensaje = $"No se pudo eliminar '{this.Accion}' no existe en la base de datos";
                    }
                    else
                    {
                        bd.Producto.Remove(encontrado);
                        bd.SaveChanges();
                        this.Mensaje = $"El producto '{encontrado.nomprod}' fue eliminado";
                    }

                }
            }
            catch (Exception ex)
            {
                this.HayErrores = true;
                this.Mensaje = Util.MensajeError("DcProducto.Actualizar", ex);
            }
        }


    }
}

