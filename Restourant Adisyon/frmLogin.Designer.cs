namespace Restourant_Adisyon
{
	partial class frmLogin
	{
		/// <summary>
		///Gerekli tasarımcı değişkeni.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///Kullanılan tüm kaynakları temizleyin.
		/// </summary>
		///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer üretilen kod

		/// <summary>
		/// Tasarımcı desteği için gerekli metot - bu metodun 
		///içeriğini kod düzenleyici ile değiştirmeyin.
		/// </summary>
		private void InitializeComponent()
		{
			this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
			this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtuser = new Guna.UI2.WinForms.Guna2TextBox();
			this.txtpass = new Guna.UI2.WinForms.Guna2TextBox();
			this.btnlogin = new Guna.UI2.WinForms.Guna2Button();
			this.btnexit = new Guna.UI2.WinForms.Guna2Button();
			this.guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
			this.guna2Panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// guna2Panel1
			// 
			this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
			this.guna2Panel1.Controls.Add(this.guna2PictureBox1);
			this.guna2Panel1.Controls.Add(this.label3);
			this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.guna2Panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
			this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
			this.guna2Panel1.Name = "guna2Panel1";
			this.guna2Panel1.Size = new System.Drawing.Size(339, 231);
			this.guna2Panel1.TabIndex = 4;
			// 
			// guna2PictureBox1
			// 
			this.guna2PictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.guna2PictureBox1.FillColor = System.Drawing.Color.Transparent;
			this.guna2PictureBox1.Image = global::Restourant_Adisyon.Properties.Resources._6681204;
			this.guna2PictureBox1.ImageRotate = 0F;
			this.guna2PictureBox1.Location = new System.Drawing.Point(108, 40);
			this.guna2PictureBox1.Name = "guna2PictureBox1";
			this.guna2PictureBox1.Size = new System.Drawing.Size(128, 123);
			this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.guna2PictureBox1.TabIndex = 4;
			this.guna2PictureBox1.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.ForeColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(12, 198);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(162, 13);
			this.label3.TabIndex = 0;
			this.label3.Tag = "";
			this.label3.Text = "Plase Eneter User İnformation.";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(42, 276);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 5;
			this.label1.Tag = "";
			this.label1.Text = "Username";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(42, 362);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Password";
			// 
			// txtuser
			// 
			this.txtuser.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtuser.DefaultText = "";
			this.txtuser.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.txtuser.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
			this.txtuser.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
			this.txtuser.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
			this.txtuser.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
			this.txtuser.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.txtuser.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
			this.txtuser.Location = new System.Drawing.Point(37, 301);
			this.txtuser.Name = "txtuser";
			this.txtuser.PlaceholderText = "";
			this.txtuser.SelectedText = "";
			this.txtuser.Size = new System.Drawing.Size(247, 36);
			this.txtuser.TabIndex = 0;
			// 
			// txtpass
			// 
			this.txtpass.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtpass.DefaultText = "";
			this.txtpass.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.txtpass.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
			this.txtpass.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
			this.txtpass.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
			this.txtpass.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
			this.txtpass.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.txtpass.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
			this.txtpass.Location = new System.Drawing.Point(37, 382);
			this.txtpass.Name = "txtpass";
			this.txtpass.PlaceholderText = "";
			this.txtpass.SelectedText = "";
			this.txtpass.Size = new System.Drawing.Size(246, 36);
			this.txtpass.TabIndex = 1;
			this.txtpass.UseSystemPasswordChar = true;
			// 
			// btnlogin
			// 
			this.btnlogin.AutoRoundedCorners = true;
			this.btnlogin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
			this.btnlogin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
			this.btnlogin.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
			this.btnlogin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
			this.btnlogin.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(123)))));
			this.btnlogin.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.btnlogin.ForeColor = System.Drawing.Color.White;
			this.btnlogin.Location = new System.Drawing.Point(45, 438);
			this.btnlogin.Name = "btnlogin";
			this.btnlogin.Size = new System.Drawing.Size(110, 45);
			this.btnlogin.TabIndex = 2;
			this.btnlogin.Text = "Login";
			this.btnlogin.Click += new System.EventHandler(this.btnlogin_Click);
			// 
			// btnexit
			// 
			this.btnexit.AutoRoundedCorners = true;
			this.btnexit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
			this.btnexit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
			this.btnexit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
			this.btnexit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
			this.btnexit.FillColor = System.Drawing.Color.Red;
			this.btnexit.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.btnexit.ForeColor = System.Drawing.Color.White;
			this.btnexit.Location = new System.Drawing.Point(166, 438);
			this.btnexit.Name = "btnexit";
			this.btnexit.Size = new System.Drawing.Size(108, 45);
			this.btnexit.TabIndex = 1;
			this.btnexit.Text = "Exit";
			this.btnexit.Click += new System.EventHandler(this.btnexit_Click);
			// 
			// guna2MessageDialog1
			// 
			this.guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
			this.guna2MessageDialog1.Caption = "   ";
			this.guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
			this.guna2MessageDialog1.Parent = this;
			this.guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
			this.guna2MessageDialog1.Text = null;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(339, 495);
			this.Controls.Add(this.btnlogin);
			this.Controls.Add(this.btnexit);
			this.Controls.Add(this.txtpass);
			this.Controls.Add(this.txtuser);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.guna2Panel1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.ForeColor = System.Drawing.SystemColors.ButtonShadow;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "0,1";
			this.guna2Panel1.ResumeLayout(false);
			this.guna2Panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private Guna.UI2.WinForms.Guna2TextBox txtuser;
		private Guna.UI2.WinForms.Guna2TextBox txtpass;
		private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
		private Guna.UI2.WinForms.Guna2Button btnlogin;
		private Guna.UI2.WinForms.Guna2Button btnexit;
		private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
	}
}

