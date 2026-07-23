using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BuenosAires.BodegaBA.WsAnwoReference;
using BuenosAires.ServiceProxy;
using BuenosAires.Model.Utiles;
using static System.Net.Mime. MediaTypeNames;

namespace BuenosAires.BodegaBA
{
    public partial class VentanaReservarAnwo : Form
    {
        public VentanaReservarAnwo()
        {
            InitializeComponent();
            BtnCargarProducto.Click += (sender, e) => CargarProductos();
            Grid.ConfigurarDataGridView("nroserieanwo: Nro Serie,nomprodanwo: Nombre Producto,  "
                + "precioanwo: Precio, reservado: Reservado, ReservadoSi[button]: Reserva Si, NoReservado[button]: Reserva No");
            CargarProductos();
            this.CentrarVentana();
        }

        private void CargarProductos()
            {
            var bc = new ScAnwo();
            bc.ObtenerStockProductoAnwo();
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
            if (e.ColumnIndex == Grid.Columns["ReservadoSi[button]"].Index && e.RowIndex >= 0)
            {
                var valorFila = Grid.Rows[e.RowIndex].Cells[0].Value.ToString();
                var bc = new ScAnwo();
                bc.ReservarEquipoAnwo(valorFila, "S");
                CargarProductos();
            }
            if (e.ColumnIndex == Grid.Columns["NoReservado[button]"].Index && e.RowIndex >= 0)
            {
                var valorFila = Grid.Rows[e.RowIndex].Cells[0].Value.ToString();
                var bc = new ScAnwo();
                bc.ReservarEquipoAnwo(valorFila, "N");
                CargarProductos();
            }

        }
    }
}
