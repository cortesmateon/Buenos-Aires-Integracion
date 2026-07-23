namespace BuenosAires.BodegaBA
{
    partial class VentanaStockProducto
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
            this.Grid = new System.Windows.Forms.DataGridView();
            this.BtnCargarProducto = new System.Windows.Forms.Button();
            this.BtnVolver = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(92, 123);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(627, 212);
            this.Grid.TabIndex = 0;
            // 
            // BtnCargarProducto
            // 
            this.BtnCargarProducto.Location = new System.Drawing.Point(190, 357);
            this.BtnCargarProducto.Name = "BtnCargarProducto";
            this.BtnCargarProducto.Size = new System.Drawing.Size(151, 28);
            this.BtnCargarProducto.TabIndex = 1;
            this.BtnCargarProducto.Text = "Cargar";
            this.BtnCargarProducto.UseVisualStyleBackColor = true;
            this.BtnCargarProducto.Click += new System.EventHandler(this.BtnCargarProducto_Click);
            // 
            // BtnVolver
            // 
            this.BtnVolver.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnVolver.Location = new System.Drawing.Point(445, 354);
            this.BtnVolver.Name = "BtnVolver";
            this.BtnVolver.Size = new System.Drawing.Size(189, 31);
            this.BtnVolver.TabIndex = 4;
            this.BtnVolver.Text = "Volver al menu Principal";
            this.BtnVolver.UseVisualStyleBackColor = true;
            this.BtnVolver.Click += new System.EventHandler(this.BtnVolver_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(231, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 48);
            this.label1.TabIndex = 6;
            this.label1.Text = "   Productos disponible en bodega\r\n(Vista de Bodeguero y Administrador)";
            // 
            // VentanaStockProducto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnVolver);
            this.Controls.Add(this.BtnCargarProducto);
            this.Controls.Add(this.Grid);
            this.Name = "VentanaStockProducto";
            this.Text = "VentanaStockProducto";
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Grid;
        private System.Windows.Forms.Button BtnCargarProducto;
        private System.Windows.Forms.Button BtnVolver;
        private System.Windows.Forms.Label label1;
    }
}