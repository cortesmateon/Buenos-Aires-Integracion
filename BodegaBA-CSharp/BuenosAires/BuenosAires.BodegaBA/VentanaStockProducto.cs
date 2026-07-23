using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BuenosAires.BodegaBA.WsStockProductoReference;
using BuenosAires.ServiceProxy;
using BuenosAires.Model.Utiles;
using static System.Net.Mime. MediaTypeNames;


namespace BuenosAires.BodegaBA
{
    public partial class VentanaStockProducto : Form
    {
        public VentanaStockProducto()
        {
            InitializeComponent();
            BtnCargarProducto.Click += (sender, e) => CargarProductos();
            Grid.ConfigurarDataGridView("idprod: ID, nomprod: Nombre, "
                + "descprod: Descripción, precio: Precio, imagen: Imagen, cantidad: Cantidad, disponibilidad:Disponibilidad");
            CargarProductos();
            this.CentrarVentana();
        }

        private void CargarProductos()
            {
            var bc = new ScStockProducto();
            bc.ObtenerStockProducto();
            Grid.DataSource = bc.Lista;
            Grid.RefrescarYajustar();
            if (bc.HayErrores == true) this.MensajeInfo(bc.Mensaje);
            }

        private void BtnCargarProducto_Click(object sender, EventArgs e)
        {

        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnVolver_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
