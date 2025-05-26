namespace Restourant_Adisyon.Model
{
	partial class frmCategoriesAdd
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
			this.components = new System.ComponentModel.Container();
			this.txtName = new Guna.UI2.WinForms.Guna2TextBox();
			this.btnSave = new Guna.UI2.WinForms.Guna2Button();
			this.btnClose = new Guna.UI2.WinForms.Guna2Button();
			this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
			this.guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.BorderRadius = 5;
			this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtName.DefaultText = "";
			this.txtName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.txtName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
			this.txtName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
			this.txtName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
			this.txtName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
			this.txtName.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.txtName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
			this.txtName.Location = new System.Drawing.Point(12, 12);
			this.txtName.Name = "txtName";
			this.txtName.PasswordChar = '\0';
			this.txtName.PlaceholderText = "Kategori Adı";
			this.txtName.SelectedText = "";
			this.txtName.Size = new System.Drawing.Size(250, 36);
			this.txtName.TabIndex = 0;
			// 
			// btnSave
			// 
			this.btnSave.BorderRadius = 5;
			this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
			this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
			this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
			this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
			this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.btnSave.ForeColor = System.Drawing.Color.White;
			this.btnSave.Location = new System.Drawing.Point(12, 54);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(120, 36);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Kaydet";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.BorderRadius = 5;
			this.btnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
			this.btnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
			this.btnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
			this.btnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
			this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.btnClose.ForeColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(142, 54);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(120, 36);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Kapat";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// guna2Elipse1
			// 
			this.guna2Elipse1.BorderRadius = 10;
			this.guna2Elipse1.TargetControl = this;
			// 
			// guna2MessageDialog1
			// 
			this.guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
			this.guna2MessageDialog1.Caption = "Bilgi";
			this.guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
			this.guna2MessageDialog1.Parent = null;
			this.guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Light;
			this.guna2MessageDialog1.Text = "İşlem başarılı";
			// 
			// frmCategoriesAdd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(274, 102);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmCategoriesAdd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Kategori Ekle";
			this.ResumeLayout(false);
		}

		#endregion

		private Guna.UI2.WinForms.Guna2TextBox txtName;
		private Guna.UI2.WinForms.Guna2Button btnSave;
		private Guna.UI2.WinForms.Guna2Button btnClose;
		private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
		private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
	}
}