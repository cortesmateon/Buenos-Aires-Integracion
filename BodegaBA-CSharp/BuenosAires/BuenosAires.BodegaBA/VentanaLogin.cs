using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BuenosAires.BodegaBA;
using BuenosAires.Model.Utiles;
using BuenosAires.ServiceProxy;

namespace BuenosAires.VentaBA
{
    public partial class VentanaLogin : Form
    {   
        public VentanaPrincipal Inicio = new VentanaPrincipal();
        public VentanaLogin()
        {
            InitializeComponent();
            this.AcceptButton = btnIngresar;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            var sc = new ScAutenticacion();
            sc.Autenticar("Bodeguero", txtCuenta.Text, txtPassword.Text);
            if (sc.Autenticado)
            {
                if (Inicio.IsDisposed) Inicio = new VentanaPrincipal();
                this.Hide();
                Inicio.ShowDialog();
            }
            else
            {
                this.MensajeInfo(sc.Mensaje);
            }
        }
    }
}
