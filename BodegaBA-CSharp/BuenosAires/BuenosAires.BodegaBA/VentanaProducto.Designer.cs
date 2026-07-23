namespace BuenosAires.BodegaBA
{
    partial class VentanaProducto
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCargarProductos = new System.Windows.Forms.Button();
            this.dvgProducto = new System.Windows.Forms.DataGridView();
            this.lblIdProd = new System.Windows.Forms.Label();
            this.lblNomProd = new System.Windows.Forms.Label();
            this.lblDescProd = new System.Windows.Forms.Label();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.lblImagen = new System.Windows.Forms.Label();
            this.txtIdProd = new System.Windows.Forms.TextBox();
            this.txtNomProd = new System.Windows.Forms.TextBox();
            this.txtDescProd = new System.Windows.Forms.TextBox();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.txtImagen = new System.Windows.Forms.TextBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.panSuperior = new System.Windows.Forms.Panel();
            this.productoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.idprodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomprodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descprodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precioDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imagenDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dvgProducto)).BeginInit();
            this.panSuperior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCargarProductos
            // 
            this.btnCargarProductos.Location = new System.Drawing.Point(344, 154);
            this.btnCargarProductos.Name = "btnCargarProductos";
            this.btnCargarProductos.Size = new System.Drawing.Size(104, 23);
            this.btnCargarProductos.TabIndex = 10;
            this.btnCargarProductos.Text = "Cargar productos";
            this.btnCargarProductos.UseVisualStyleBackColor = true;
            // 
            // dvgProducto
            // 
            this.dvgProducto.AutoGenerateColumns = false;
            this.dvgProducto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idprodDataGridViewTextBoxColumn,
            this.nomprodDataGridViewTextBoxColumn,
            this.descprodDataGridViewTextBoxColumn,
            this.precioDataGridViewTextBoxColumn,
            this.imagenDataGridViewTextBoxColumn});
            this.dvgProducto.DataSource = this.productoBindingSource;
            this.dvgProducto.Location = new System.Drawing.Point(16, 195);
            this.dvgProducto.Name = "dvgProducto";
            this.dvgProducto.Size = new System.Drawing.Size(544, 288);
            this.dvgProducto.TabIndex = 11;
            // 
            // lblIdProd
            // 
            this.lblIdProd.AutoSize = true;
            this.lblIdProd.Location = new System.Drawing.Point(13, 12);
            this.lblIdProd.Name = "lblIdProd";
            this.lblIdProd.Size = new System.Drawing.Size(18, 13);
            this.lblIdProd.TabIndex = 2;
            this.lblIdProd.Text = "ID";
            // 
            // lblNomProd
            // 
            this.lblNomProd.AutoSize = true;
            this.lblNomProd.Location = new System.Drawing.Point(13, 40);
            this.lblNomProd.Name = "lblNomProd";
            this.lblNomProd.Size = new System.Drawing.Size(44, 13);
            this.lblNomProd.TabIndex = 3;
            this.lblNomProd.Text = "Nombre";
            // 
            // lblDescProd
            // 
            this.lblDescProd.AutoSize = true;
            this.lblDescProd.Location = new System.Drawing.Point(13, 67);
            this.lblDescProd.Name = "lblDescProd";
            this.lblDescProd.Size = new System.Drawing.Size(63, 13);
            this.lblDescProd.TabIndex = 4;
            this.lblDescProd.Text = "Descripción";
            // 
            // lblPrecio
            // 
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Location = new System.Drawing.Point(13, 95);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(37, 13);
            this.lblPrecio.TabIndex = 5;
            this.lblPrecio.Text = "Precio";
            // 
            // lblImagen
            // 
            this.lblImagen.AutoSize = true;
            this.lblImagen.Location = new System.Drawing.Point(13, 122);
            this.lblImagen.Name = "lblImagen";
            this.lblImagen.Size = new System.Drawing.Size(42, 13);
            this.lblImagen.TabIndex = 6;
            this.lblImagen.Text = "Imagen";
            // 
            // txtIdProd
            // 
            this.txtIdProd.Enabled = false;
            this.txtIdProd.Location = new System.Drawing.Point(83, 9);
            this.txtIdProd.Name = "txtIdProd";
            this.txtIdProd.ReadOnly = true;
            this.txtIdProd.Size = new System.Drawing.Size(51, 20);
            this.txtIdProd.TabIndex = 1;
            // 
            // txtNomProd
            // 
            this.txtNomProd.Location = new System.Drawing.Point(83, 37);
            this.txtNomProd.Name = "txtNomProd";
            this.txtNomProd.Size = new System.Drawing.Size(214, 20);
            this.txtNomProd.TabIndex = 3;
            // 
            // txtDescProd
            // 
            this.txtDescProd.Location = new System.Drawing.Point(82, 64);
            this.txtDescProd.Name = "txtDescProd";
            this.txtDescProd.Size = new System.Drawing.Size(215, 20);
            this.txtDescProd.TabIndex = 4;
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(83, 92);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(75, 20);
            this.txtPrecio.TabIndex = 5;
            // 
            // txtImagen
            // 
            this.txtImagen.Location = new System.Drawing.Point(82, 119);
            this.txtImagen.Name = "txtImagen";
            this.txtImagen.Size = new System.Drawing.Size(215, 20);
            this.txtImagen.TabIndex = 6;
            // 
            // btnNuevo
            // 
            this.btnNuevo.Location = new System.Drawing.Point(16, 154);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(104, 23);
            this.btnNuevo.TabIndex = 7;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(126, 154);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(104, 23);
            this.btnGuardar.TabIndex = 8;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(236, 154);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(102, 23);
            this.btnEliminar.TabIndex = 9;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(140, 7);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            // 
            // panSuperior
            // 
            this.panSuperior.Controls.Add(this.lblIdProd);
            this.panSuperior.Controls.Add(this.btnBuscar);
            this.panSuperior.Controls.Add(this.btnCargarProductos);
            this.panSuperior.Controls.Add(this.btnEliminar);
            this.panSuperior.Controls.Add(this.lblNomProd);
            this.panSuperior.Controls.Add(this.btnGuardar);
            this.panSuperior.Controls.Add(this.lblDescProd);
            this.panSuperior.Controls.Add(this.btnNuevo);
            this.panSuperior.Controls.Add(this.lblPrecio);
            this.panSuperior.Controls.Add(this.txtImagen);
            this.panSuperior.Controls.Add(this.lblImagen);
            this.panSuperior.Controls.Add(this.txtPrecio);
            this.panSuperior.Controls.Add(this.txtIdProd);
            this.panSuperior.Controls.Add(this.txtDescProd);
            this.panSuperior.Controls.Add(this.txtNomProd);
            this.panSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSuperior.Location = new System.Drawing.Point(0, 0);
            this.panSuperior.Name = "panSuperior";
            this.panSuperior.Size = new System.Drawing.Size(595, 189);
            this.panSuperior.TabIndex = 17;
            // 
            // productoBindingSource
            // 
            this.productoBindingSource.DataSource = typeof(BuenosAires.Model.Producto);
            // 
            // idprodDataGridViewTextBoxColumn
            // 
            this.idprodDataGridViewTextBoxColumn.DataPropertyName = "idprod";
            this.idprodDataGridViewTextBoxColumn.HeaderText = "idprod";
            this.idprodDataGridViewTextBoxColumn.Name = "idprodDataGridViewTextBoxColumn";
            // 
            // nomprodDataGridViewTextBoxColumn
            // 
            this.nomprodDataGridViewTextBoxColumn.DataPropertyName = "nomprod";
            this.nomprodDataGridViewTextBoxColumn.HeaderText = "nomprod";
            this.nomprodDataGridViewTextBoxColumn.Name = "nomprodDataGridViewTextBoxColumn";
            // 
            // descprodDataGridViewTextBoxColumn
            // 
            this.descprodDataGridViewTextBoxColumn.DataPropertyName = "descprod";
            this.descprodDataGridViewTextBoxColumn.HeaderText = "descprod";
            this.descprodDataGridViewTextBoxColumn.Name = "descprodDataGridViewTextBoxColumn";
            // 
            // precioDataGridViewTextBoxColumn
            // 
            this.precioDataGridViewTextBoxColumn.DataPropertyName = "precio";
            this.precioDataGridViewTextBoxColumn.HeaderText = "precio";
            this.precioDataGridViewTextBoxColumn.Name = "precioDataGridViewTextBoxColumn";
            // 
            // imagenDataGridViewTextBoxColumn
            // 
            this.imagenDataGridViewTextBoxColumn.DataPropertyName = "imagen";
            this.imagenDataGridViewTextBoxColumn.HeaderText = "imagen";
            this.imagenDataGridViewTextBoxColumn.Name = "imagenDataGridViewTextBoxColumn";
            // 
            // VentanaProducto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 495);
            this.Controls.Add(this.panSuperior);
            this.Controls.Add(this.dvgProducto);
            this.Name = "VentanaProducto";
            this.Text = "Mantenedor de Productos";
            ((System.ComponentModel.ISupportInitialize)(this.dvgProducto)).EndInit();
            this.panSuperior.ResumeLayout(false);
            this.panSuperior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCargarProductos;
        private System.Windows.Forms.DataGridView dvgProducto;
        private System.Windows.Forms.Label lblIdProd;
        private System.Windows.Forms.Label lblNomProd;
        private System.Windows.Forms.Label lblDescProd;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.Label lblImagen;
        private System.Windows.Forms.TextBox txtIdProd;
        private System.Windows.Forms.TextBox txtNomProd;
        private System.Windows.Forms.TextBox txtDescProd;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.TextBox txtImagen;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Panel panSuperior;
        private System.Windows.Forms.DataGridViewTextBoxColumn idprodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nomprodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descprodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn precioDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imagenDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource productoBindingSource;
    }
}

