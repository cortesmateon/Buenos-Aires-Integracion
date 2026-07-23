using BuenosAires.ServiceProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BuenosAires.BodegaBA.WsGuiaDespachoReference;
using BuenosAires.Model.Utiles;
using static System.Net.Mime.MediaTypeNames;

namespace BuenosAires.BodegaBA
{
    public partial class VentanaGuiasDespacho : Form
    {
        public VentanaGuiasDespacho()
        {
            InitializeComponent();
            BtnCargarGuiaDespacho.Click += (sender, e) => CargarProductos();
            Grid.ConfigurarDataGridView("nrogd: Nro GD, descprod: Producto, estadogd: Estado GD, nrofac: Nro Factura, Cliente: Cliente,"
                + "Despachado[button]:Despachado, Imprimir[button]:Imprimir, Entregado[button]:Entregado");
            CargarProductos();
            this.CentrarVentana();
        }

        private void CargarProductos()
        {
            var bc = new ScGuiaDespacho();
            bc.ObtenerGuiaDespacho();
            Grid.DataSource = bc.Lista;
            Grid.RefrescarYajustar();
            if (bc.HayErrores == true) this.MensajeInfo(bc.Mensaje);
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == Grid.Columns["Despachado[button]"].Index && e.RowIndex >= 0)
            {
                var valorFila = Grid.Rows[e.RowIndex].Cells[0].Value.ToString();
                var bc = new ScGuiaDespacho();
                bc.ActualizarEstadoGuiaDespacho(int.Parse(valorFila), "Despachado");
                CargarProductos();
            }
            if (e.ColumnIndex == Grid.Columns["Entregado[button]"].Index && e.RowIndex >= 0)
            {
                var valorFila = Grid.Rows[e.RowIndex].Cells[0].Value.ToString();
                var bc = new ScGuiaDespacho();
                bc.ActualizarEstadoGuiaDespacho(int.Parse(valorFila), "Entregado");
                CargarProductos();
            }

        }
    }
}
