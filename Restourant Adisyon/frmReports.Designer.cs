namespace Restourant_Adisyon
{
    partial class frmReports
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
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalOrders = new System.Windows.Forms.Label();
            this.dtStart = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtEnd = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.btnFilter = new Guna.UI2.WinForms.Guna2Button();
            this.dgvTopProducts = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dgvDailySales = new Guna.UI2.WinForms.Guna2DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailySales)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.BorderRadius = 15;
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Controls.Add(this.lblTotalRevenue);
            this.guna2Panel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2Panel1.Location = new System.Drawing.Point(30, 90);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(250, 100);
            this.guna2Panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Total Revenue";
            // 
            // lblTotalRevenue
            // 
            this.lblTotalRevenue.AutoSize = true;
            this.lblTotalRevenue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalRevenue.ForeColor = System.Drawing.Color.White;
            this.lblTotalRevenue.Location = new System.Drawing.Point(15, 45);
            this.lblTotalRevenue.Name = "lblTotalRevenue";
            this.lblTotalRevenue.Size = new System.Drawing.Size(89, 37);
            this.lblTotalRevenue.TabIndex = 0;
            this.lblTotalRevenue.Text = "0.00 ₺";
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.BorderRadius = 15;
            this.guna2Panel2.Controls.Add(this.label3);
            this.guna2Panel2.Controls.Add(this.lblTotalOrders);
            this.guna2Panel2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.guna2Panel2.Location = new System.Drawing.Point(300, 90);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(250, 100);
            this.guna2Panel2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 21);
            this.label3.TabIndex = 1;
            this.label3.Text = "Total Orders";
            // 
            // lblTotalOrders
            // 
            this.lblTotalOrders.AutoSize = true;
            this.lblTotalOrders.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalOrders.ForeColor = System.Drawing.Color.White;
            this.lblTotalOrders.Location = new System.Drawing.Point(15, 45);
            this.lblTotalOrders.Name = "lblTotalOrders";
            this.lblTotalOrders.Size = new System.Drawing.Size(33, 37);
            this.lblTotalOrders.TabIndex = 0;
            this.lblTotalOrders.Text = "0";
            // 
            // dtStart
            // 
            this.dtStart.Checked = true;
            this.dtStart.FillColor = System.Drawing.Color.White;
            this.dtStart.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtStart.Location = new System.Drawing.Point(30, 30);
            this.dtStart.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtStart.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(150, 36);
            this.dtStart.TabIndex = 2;
            this.dtStart.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // dtEnd
            // 
            this.dtEnd.Checked = true;
            this.dtEnd.FillColor = System.Drawing.Color.White;
            this.dtEnd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtEnd.Location = new System.Drawing.Point(190, 30);
            this.dtEnd.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtEnd.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(150, 36);
            this.dtEnd.TabIndex = 3;
            this.dtEnd.Value = new System.DateTime(2024, 12, 31, 0, 0, 0, 0);
            // 
            // btnFilter
            // 
            this.btnFilter.AutoRoundedCorners = true;
            this.btnFilter.BorderRadius = 17;
            this.btnFilter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFilter.ForeColor = System.Drawing.Color.White;
            this.btnFilter.Location = new System.Drawing.Point(350, 30);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(100, 36);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "Filter";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // dgvTopProducts
            // 
            this.dgvTopProducts.AllowUserToAddRows = false;
            this.dgvTopProducts.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvTopProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTopProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTopProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvTopProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTopProducts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvTopProducts.ColumnHeadersHeight = 40;
            this.dgvTopProducts.Location = new System.Drawing.Point(30, 240);
            this.dgvTopProducts.Name = "dgvTopProducts";
            this.dgvTopProducts.ReadOnly = true;
            this.dgvTopProducts.RowHeadersVisible = false;
            this.dgvTopProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTopProducts.Size = new System.Drawing.Size(400, 250);
            this.dgvTopProducts.TabIndex = 5;
            // 
            // dgvDailySales
            // 
            this.dgvDailySales.AllowUserToAddRows = false;
            this.dgvDailySales.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.dgvDailySales.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDailySales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDailySales.BackgroundColor = System.Drawing.Color.White;
            this.dgvDailySales.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDailySales.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDailySales.ColumnHeadersHeight = 40;
            this.dgvDailySales.Location = new System.Drawing.Point(460, 240);
            this.dgvDailySales.Name = "dgvDailySales";
            this.dgvDailySales.ReadOnly = true;
            this.dgvDailySales.RowHeadersVisible = false;
            this.dgvDailySales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDailySales.Size = new System.Drawing.Size(400, 250);
            this.dgvDailySales.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(30, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(201, 25);
            this.label5.TabIndex = 7;
            this.label5.Text = "Top Selling Products";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(460, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 25);
            this.label6.TabIndex = 8;
            this.label6.Text = "Daily Sales";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Start Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(190, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "End Date";
            // 
            // frmReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvDailySales);
            this.Controls.Add(this.dgvTopProducts);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.dtEnd);
            this.Controls.Add(this.dtStart);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmReports";
            this.Text = "frmReports";
            this.Load += new System.EventHandler(this.frmReports_Load);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailySales)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalRevenue;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTotalOrders;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtStart;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtEnd;
        private Guna.UI2.WinForms.Guna2Button btnFilter;
        private Guna.UI2.WinForms.Guna2DataGridView dgvTopProducts;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDailySales;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}
