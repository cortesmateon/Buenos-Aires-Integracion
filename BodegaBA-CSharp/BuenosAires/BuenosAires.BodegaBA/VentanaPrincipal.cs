using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuenosAires.BodegaBA
{
    public partial class VentanaPrincipal : Form

        

    {
        public VentanaReservarAnwo VentanaANWO = new VentanaReservarAnwo();
        public VentanaGuiasDespacho VentanaDespachoGuia = new VentanaGuiasDespacho();
        public VentanaStockProducto VentanaStock = new VentanaStockProducto(); 
        public VentanaPrincipal()
        {
            InitializeComponent();
        }

        private void VentanaPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void BtnConsultaProductos_Click(object sender, EventArgs e)
        {
            if(VentanaStock.IsDisposed) VentanaStock = new VentanaStockProducto();
            VentanaStock.Show();
        }

        private void BtnAdministrarGuias_Click(object sender, EventArgs e)
        {
            if (VentanaDespachoGuia.IsDisposed) VentanaDespachoGuia = new VentanaGuiasDespacho();
            VentanaDespachoGuia.Show();
        }

        private void BtnANWO_Click(object sender, EventArgs e)
        {
            if (VentanaANWO.IsDisposed) VentanaANWO = new VentanaReservarAnwo();
            VentanaANWO.Show();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
