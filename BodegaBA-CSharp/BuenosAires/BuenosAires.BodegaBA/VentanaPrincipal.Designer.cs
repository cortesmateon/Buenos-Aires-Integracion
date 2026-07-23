namespace BuenosAires.BodegaBA
{
    partial class VentanaPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnConsultaProductos = new System.Windows.Forms.Button();
            this.BtnAdministrarGuias = new System.Windows.Forms.Button();
            this.BtnANWO = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(222, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sistema de Bodega - Menu Principal\r\n(Vista de Bodeguero y Administrador)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Buenos Aires";
            // 
            // BtnConsultaProductos
            // 
            this.BtnConsultaProductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnConsultaProductos.Location = new System.Drawing.Point(256, 172);
            this.BtnConsultaProductos.Name = "BtnConsultaProductos";
            this.BtnConsultaProductos.Size = new System.Drawing.Size(279, 68);
            this.BtnConsultaProductos.TabIndex = 2;
            this.BtnConsultaProductos.Text = "Consultar productos en bodega";
            this.BtnConsultaProductos.UseVisualStyleBackColor = true;
            this.BtnConsultaProductos.Click += new System.EventHandler(this.BtnConsultaProductos_Click);
            // 
            // BtnAdministrarGuias
            // 
            this.BtnAdministrarGuias.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdministrarGuias.Location = new System.Drawing.Point(256, 264);
            this.BtnAdministrarGuias.Name = "BtnAdministrarGuias";
            this.BtnAdministrarGuias.Size = new System.Drawing.Size(279, 62);
            this.BtnAdministrarGuias.TabIndex = 3;
            this.BtnAdministrarGuias.Text = "Administrar Guias de Despacho";
            this.BtnAdministrarGuias.UseVisualStyleBackColor = true;
            this.BtnAdministrarGuias.Click += new System.EventHandler(this.BtnAdministrarGuias_Click);
            // 
            // BtnANWO
            // 
            this.BtnANWO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnANWO.Location = new System.Drawing.Point(256, 343);
            this.BtnANWO.Name = "BtnANWO";
            this.BtnANWO.Size = new System.Drawing.Size(279, 62);
            this.BtnANWO.TabIndex = 4;
            this.BtnANWO.Text = "Reservar Equipos de ANWO";
            this.BtnANWO.UseVisualStyleBackColor = true;
            this.BtnANWO.Click += new System.EventHandler(this.BtnANWO_Click);
            // 
            // BtnSalir
            // 
            this.BtnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSalir.Location = new System.Drawing.Point(256, 425);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(279, 58);
            this.BtnSalir.TabIndex = 5;
            this.BtnSalir.Text = "Salir";
            this.BtnSalir.UseVisualStyleBackColor = true;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // VentanaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 574);
            this.Controls.Add(this.BtnSalir);
            this.Controls.Add(this.BtnANWO);
            this.Controls.Add(this.BtnAdministrarGuias);
            this.Controls.Add(this.BtnConsultaProductos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "VentanaPrincipal";
            this.Text = "VentanaPrincipal";
            this.Load += new System.EventHandler(this.VentanaPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnConsultaProductos;
        private System.Windows.Forms.Button BtnAdministrarGuias;
        private System.Windows.Forms.Button BtnANWO;
        private System.Windows.Forms.Button BtnSalir;
    }
}