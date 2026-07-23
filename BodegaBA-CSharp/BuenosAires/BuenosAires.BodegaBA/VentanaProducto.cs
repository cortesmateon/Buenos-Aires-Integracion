using System;
using System.Windows.Forms;
using System.Collections.Generic;
using BuenosAires.Model.Utiles;
using BuenosAires.Model;
using BuenosAires.ServiceProxy;


namespace BuenosAires.BodegaBA
{
    public partial class VentanaProducto : Form
    {
        private void VentanaProductos_Load(object sender, EventArgs e)
        {
            RefrescarDataGridView();
        }
        public void RefrescarDataGridView()
        {
            var bc = new ScProducto();
            bc.LeerTodos();
            if (bc.Mensaje != "") Util.MostrarMensaje(bc.Mensaje, bc.HayErrores);
            dvgProducto.DataSource = bc.Lista;
            dvgProducto.Refresh();
            dvgProducto.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        public VentanaProducto(string nomusu, string tipousu)
        {
            InitializeComponent();
            this.Text = $"Mantenedor de Productos - Usuario: {nomusu} ({tipousu})";
            btnBuscar.Click += (sender, e) => Buscar();
            btnNuevo.Click += (sender, e) => Nuevo();
            btnGuardar.Click += (sender, e) => Guardar();
            btnEliminar.Click += (sender, e) => Eliminar();
            btnCargarProductos.Click += (sender, e) => CargarProductos();
            dvgProducto.SelectionChanged += (sender, e) => Seleccionar();
            dvgProducto.ConfigurarDataGridView("idprod:ID, nomprod:Nombre, "
                + "descprod:Descripción, precio:Precio, imagen:Imagen");
            CargarProductos();
            this.CentrarVentana();
        }


        private void Nuevo()
        {
            this.Limpiar(new TextBox[] {txtIdProd, txtNomProd, txtDescProd, txtPrecio, txtImagen });
        }

        public bool ValidarCamposNumericos()
        {
            if (txtIdProd.Text != "" && !txtIdProd.EsNumero()) return this.ErrEntero("ID");
            if (!txtPrecio.EsNumero()) return this.ErrEntero("Precio");
            return true;
        }

        private void Seleccionar()
        {
            
            if (dvgProducto.SelectedRows.Count <= 0) return;

            
            DataGridViewRow row = dvgProducto.SelectedRows[0];
            if (row.GetString("idprod") != "0") this.AsignarValoresTextBox(row);
            txtNomProd.FocusToEnd();
        }

        private bool Buscar()
        {
            int id = new VentanaBuscarID().MostrarVentanaModal();
            if (id == -1) return false;

            var bc = new ScProducto();
            bc.Leer(id);

            if (bc.Producto == null) return this.MensajeInfo(bc.Mensaje);

            CargarProductos();
            this.AsignarValoresTextBox(bc.Producto);

            dvgProducto.SeleccionarId("idprod", txtIdProd.ToInt());
            txtNomProd.FocusToEnd();
            return true;
        }

        private void Guardar()
        {
            if (!ValidarCamposNumericos()) return;

            var prod = new Producto();
            prod.idprod = txtIdProd.ToIntOrDefault();
            prod.nomprod = txtNomProd.Text;
            prod.descprod = txtDescProd.Text;
            prod.precio = txtPrecio.ToInt();
            prod.imagen = txtImagen.Text;

            var bc = new ScProducto();

            if (txtIdProd.Text == "") bc.Crear(prod); else bc.Actualizar(prod);

            if (!bc.HayErrores)
            {
                txtIdProd.SetText(bc.Producto.idprod);
                CargarProductos();
                dvgProducto.SeleccionarId("idprod", bc.Producto.idprod);
                txtNomProd.FocusToEnd();
            }

            this.MensajeInfo(bc.Mensaje);
        }

        private bool Eliminar()
        {
            var bc = new ScProducto();
            if (txtIdProd.Text == "") return this.ErrAccionID("ID", "eliminar");
            bc.Eliminar(txtIdProd.ToInt());
            CargarProductos();
            this.MensajeInfo(bc.Mensaje);
            return true;
        }

        public void CargarProductos()
        {
            var bc = new ScProducto();
            bc.LeerTodos();
            dvgProducto.DataSource = bc.Lista;
            dvgProducto.RefrescarYajustar();
            if (bc.HayErrores == true) this.MensajeInfo(bc.Mensaje);
        }
    }
}
