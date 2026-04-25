namespace Restourant_Adisyon
{
    partial class frmInventory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMaterials = new Guna.UI2.WinForms.Guna2DataGridView();
            this.txtMName = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMQty = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMUnit = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnAddMaterial = new Guna.UI2.WinForms.Guna2Button();
            this.cbProduct = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dgvRecipe = new Guna.UI2.WinForms.Guna2DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaterials)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecipe)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMaterials
            // 
            this.dgvMaterials.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvMaterials.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMaterials.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMaterials.BackgroundColor = System.Drawing.Color.White;
            this.dgvMaterials.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMaterials.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvMaterials.ColumnHeadersHeight = 40;
            this.dgvMaterials.Location = new System.Drawing.Point(30, 220);
            this.dgvMaterials.Name = "dgvMaterials";
            this.dgvMaterials.ReadOnly = true;
            this.dgvMaterials.RowHeadersVisible = false;
            this.dgvMaterials.Size = new System.Drawing.Size(400, 300);
            this.dgvMaterials.TabIndex = 0;
            // 
            // txtMName
            // 
            this.txtMName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMName.DefaultText = "";
            this.txtMName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMName.Location = new System.Drawing.Point(30, 50);
            this.txtMName.Name = "txtMName";
            this.txtMName.Size = new System.Drawing.Size(200, 36);
            this.txtMName.TabIndex = 1;
            // 
            // txtMQty
            // 
            this.txtMQty.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMQty.DefaultText = "0";
            this.txtMQty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMQty.Location = new System.Drawing.Point(30, 110);
            this.txtMQty.Name = "txtMQty";
            this.txtMQty.Size = new System.Drawing.Size(90, 36);
            this.txtMQty.TabIndex = 2;
            // 
            // txtMUnit
            // 
            this.txtMUnit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMUnit.DefaultText = "kg";
            this.txtMUnit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMUnit.Location = new System.Drawing.Point(140, 110);
            this.txtMUnit.Name = "txtMUnit";
            this.txtMUnit.Size = new System.Drawing.Size(90, 36);
            this.txtMUnit.TabIndex = 3;
            // 
            // btnAddMaterial
            // 
            this.btnAddMaterial.AutoRoundedCorners = true;
            this.btnAddMaterial.BorderRadius = 17;
            this.btnAddMaterial.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnAddMaterial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAddMaterial.ForeColor = System.Drawing.Color.White;
            this.btnAddMaterial.Location = new System.Drawing.Point(30, 160);
            this.btnAddMaterial.Name = "btnAddMaterial";
            this.btnAddMaterial.Size = new System.Drawing.Size(200, 36);
            this.btnAddMaterial.TabIndex = 4;
            this.btnAddMaterial.Text = "Add Material";
            this.btnAddMaterial.Click += new System.EventHandler(this.btnAddMaterial_Click);
            // 
            // cbProduct
            // 
            this.cbProduct.BackColor = System.Drawing.Color.Transparent;
            this.cbProduct.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProduct.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbProduct.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbProduct.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbProduct.ItemHeight = 30;
            this.cbProduct.Location = new System.Drawing.Point(460, 50);
            this.cbProduct.Name = "cbProduct";
            this.cbProduct.Size = new System.Drawing.Size(400, 36);
            this.cbProduct.TabIndex = 5;
            this.cbProduct.SelectedIndexChanged += new System.EventHandler(this.cbProduct_SelectedIndexChanged);
            // 
            // dgvRecipe
            // 
            this.dgvRecipe.AllowUserToAddRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.dgvRecipe.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecipe.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecipe.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecipe.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRecipe.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRecipe.ColumnHeadersHeight = 40;
            this.dgvRecipe.Location = new System.Drawing.Point(460, 110);
            this.dgvRecipe.Name = "dgvRecipe";
            this.dgvRecipe.ReadOnly = true;
            this.dgvRecipe.RowHeadersVisible = false;
            this.dgvRecipe.Size = new System.Drawing.Size(400, 410);
            this.dgvRecipe.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Material Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Quantity";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(140, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Unit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(460, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Product Recipe";
            // 
            // frmInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvRecipe);
            this.Controls.Add(this.cbProduct);
            this.Controls.Add(this.btnAddMaterial);
            this.Controls.Add(this.txtMUnit);
            this.Controls.Add(this.txtMQty);
            this.Controls.Add(this.txtMName);
            this.Controls.Add(this.dgvMaterials);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInventory";
            this.Text = "frmInventory";
            this.Load += new System.EventHandler(this.frmInventory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaterials)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecipe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Guna.UI2.WinForms.Guna2DataGridView dgvMaterials;
        private Guna.UI2.WinForms.Guna2TextBox txtMName;
        private Guna.UI2.WinForms.Guna2TextBox txtMQty;
        private Guna.UI2.WinForms.Guna2TextBox txtMUnit;
        private Guna.UI2.WinForms.Guna2Button btnAddMaterial;
        private Guna.UI2.WinForms.Guna2ComboBox cbProduct;
        private Guna.UI2.WinForms.Guna2DataGridView dgvRecipe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
