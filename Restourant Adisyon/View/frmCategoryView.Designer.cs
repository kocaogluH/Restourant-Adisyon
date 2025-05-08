namespace Restourant_Adisyon.View
{
	partial class frmCategoryView
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.guna2DataGridView1 = new Guna.UI2.WinForms.Guna2DataGridView();
			this.dgvSno = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvid = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvedit = new System.Windows.Forms.DataGridViewImageColumn();
			this.dgvdel = new System.Windows.Forms.DataGridViewImageColumn();
			((System.ComponentModel.ISupportInitialize)(this.guna2DataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblHeader
			// 
			this.lblHeader.Size = new System.Drawing.Size(121, 25);
			this.lblHeader.Text = "Category List";
			// 
			// btnAdd
			// 
			this.btnAdd.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
			this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.None;
			this.btnAdd.HoverState.ImageSize = new System.Drawing.Size(57, 57);
			this.btnAdd.ImageFlip = Guna.UI2.WinForms.Enums.FlipOrientation.Normal;
			this.btnAdd.PressedState.ImageSize = new System.Drawing.Size(55, 55);
			// 
			// guna2DataGridView1
			// 
			this.guna2DataGridView1.AllowUserToAddRows = false;
			this.guna2DataGridView1.AllowUserToDeleteRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
			this.guna2DataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.DimGray;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.GrayText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.guna2DataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.guna2DataGridView1.ColumnHeadersHeight = 40;
			this.guna2DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			this.guna2DataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvSno,
            this.dgvid,
            this.dgvName,
            this.dgvedit,
            this.dgvdel});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.guna2DataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
			this.guna2DataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
			this.guna2DataGridView1.Location = new System.Drawing.Point(12, 220);
			this.guna2DataGridView1.Name = "guna2DataGridView1";
			this.guna2DataGridView1.ReadOnly = true;
			this.guna2DataGridView1.RowHeadersVisible = false;
			this.guna2DataGridView1.RowTemplate.Height = 40;
			this.guna2DataGridView1.Size = new System.Drawing.Size(901, 288);
			this.guna2DataGridView1.TabIndex = 8;
			this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
			this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = null;
			this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
			this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
			this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
			this.guna2DataGridView1.ThemeStyle.BackColor = System.Drawing.Color.White;
			this.guna2DataGridView1.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
			this.guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.WhiteSmoke;
			this.guna2DataGridView1.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.guna2DataGridView1.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
			this.guna2DataGridView1.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			this.guna2DataGridView1.ThemeStyle.HeaderStyle.Height = 40;
			this.guna2DataGridView1.ThemeStyle.ReadOnly = true;
			this.guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
			this.guna2DataGridView1.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.guna2DataGridView1.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
			this.guna2DataGridView1.ThemeStyle.RowsStyle.Height = 40;
			this.guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
			this.guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
			// 
			// dgvSno
			// 
			this.dgvSno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dgvSno.FillWeight = 70F;
			this.dgvSno.HeaderText = "sr#";
			this.dgvSno.MinimumWidth = 70;
			this.dgvSno.Name = "dgvSno";
			this.dgvSno.ReadOnly = true;
			this.dgvSno.Visible = false;
			this.dgvSno.Width = 70;
			// 
			// dgvid
			// 
			this.dgvid.HeaderText = "textid";
			this.dgvid.Name = "dgvid";
			this.dgvid.ReadOnly = true;
			this.dgvid.Visible = false;
			// 
			// dgvName
			// 
			this.dgvName.HeaderText = "Name";
			this.dgvName.Name = "dgvName";
			this.dgvName.ReadOnly = true;
			// 
			// dgvedit
			// 
			this.dgvedit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dgvedit.FillWeight = 50F;
			this.dgvedit.HeaderText = "";
			this.dgvedit.Image = global::Restourant_Adisyon.Properties.Resources.png_clipart_black_fountain_pen_nib_pen_detail_objects_pen;
			this.dgvedit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
			this.dgvedit.MinimumWidth = 50;
			this.dgvedit.Name = "dgvedit";
			this.dgvedit.ReadOnly = true;
			this.dgvedit.Width = 50;
			// 
			// dgvdel
			// 
			this.dgvdel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dgvdel.FillWeight = 50F;
			this.dgvdel.HeaderText = "";
			this.dgvdel.Image = global::Restourant_Adisyon.Properties.Resources.png_transparent_green_trash_bin_illustration_recycling_bin_waste_container_recycle_bin_rectangle_recycling_waste;
			this.dgvdel.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
			this.dgvdel.MinimumWidth = 50;
			this.dgvdel.Name = "dgvdel";
			this.dgvdel.ReadOnly = true;
			this.dgvdel.Width = 50;
			// 
			// frmCategoryView
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(925, 538);
			this.Controls.Add(this.guna2DataGridView1);
			this.Name = "frmCategoryView";
			this.Text = "frmCategoryView";
			this.Controls.SetChildIndex(this.btnAdd, 0);
			this.Controls.SetChildIndex(this.lblSearch, 0);
			this.Controls.SetChildIndex(this.lblHeader, 0);
			this.Controls.SetChildIndex(this.guna2DataGridView1, 0);
			((System.ComponentModel.ISupportInitialize)(this.guna2DataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Guna.UI2.WinForms.Guna2DataGridView guna2DataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvSno;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvid;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvName;
		private System.Windows.Forms.DataGridViewImageColumn dgvedit;
		private System.Windows.Forms.DataGridViewImageColumn dgvdel;
	}
}