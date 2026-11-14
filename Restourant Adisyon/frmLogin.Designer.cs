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
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelSub = new System.Windows.Forms.Label();
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
			this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.guna2Panel1.Controls.Add(this.labelTitle);
			this.guna2Panel1.Controls.Add(this.labelSub);
			this.guna2Panel1.Controls.Add(this.label3);
			this.guna2Panel1.Controls.Add(this.guna2PictureBox1);
			this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.guna2Panel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.guna2Panel1.ForeColor = System.Drawing.Color.White;
			this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
			this.guna2Panel1.Name = "guna2Panel1";
			this.guna2Panel1.Size = new System.Drawing.Size(380, 175);
			this.guna2Panel1.TabIndex = 4;
			// 
			// guna2PictureBox1
			// 
			this.guna2PictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.guna2PictureBox1.FillColor = System.Drawing.Color.Transparent;
			this.guna2PictureBox1.ImageRotate = 0F;
			this.guna2PictureBox1.Location = new System.Drawing.Point(0, 0);
			this.guna2PictureBox1.Name = "guna2PictureBox1";
			this.guna2PictureBox1.Size = new System.Drawing.Size(1, 1);
			this.guna2PictureBox1.TabIndex = 4;
			this.guna2PictureBox1.TabStop = false;
			this.guna2PictureBox1.Visible = false;
			// 
			// labelTitle
			// 
			this.labelTitle.BackColor = System.Drawing.Color.Transparent;
			this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.labelTitle.ForeColor = System.Drawing.Color.White;
			this.labelTitle.Location = new System.Drawing.Point(0, 38);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(380, 32);
			this.labelTitle.TabIndex = 1;
			this.labelTitle.Text = "⚡ RESTO POS";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelSub
			// 
			this.labelSub.BackColor = System.Drawing.Color.Transparent;
			this.labelSub.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.labelSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
			this.labelSub.Location = new System.Drawing.Point(0, 75);
			this.labelSub.Name = "labelSub";
			this.labelSub.Size = new System.Drawing.Size(380, 20);
			this.labelSub.TabIndex = 2;
			this.labelSub.Text = "Akıllı Restoran Yönetim Sistemi";
			this.labelSub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 8.5F);
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
			this.label3.Location = new System.Drawing.Point(0, 125);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(380, 22);
			this.label3.TabIndex = 0;
			this.label3.Text = "Lütfen giriş bilgilerinizi yazınız.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
			this.label1.Location = new System.Drawing.Point(36, 195);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(147, 17);
			this.label1.TabIndex = 5;
			this.label1.Text = "Kullanıcı Adı veya PIN";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
			this.label2.Location = new System.Drawing.Point(36, 274);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 17);
			this.label2.TabIndex = 6;
			this.label2.Text = "Şifre";
			// 
			// txtuser
			// 
			this.txtuser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
			this.txtuser.BorderRadius = 8;
			this.txtuser.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtuser.DefaultText = "";
			this.txtuser.FillColor = System.Drawing.Color.White;
			this.txtuser.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(29)))), ((int)(((byte)(72)))));
			this.txtuser.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtuser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.txtuser.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(29)))), ((int)(((byte)(72)))));
			this.txtuser.Location = new System.Drawing.Point(36, 220);
			this.txtuser.Name = "txtuser";
			this.txtuser.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
			this.txtuser.PlaceholderText = "admin veya 4 haneli PIN";
			this.txtuser.SelectedText = "";
			this.txtuser.Size = new System.Drawing.Size(308, 42);
			this.txtuser.TabIndex = 0;
			// 
			// txtpass
			// 
			this.txtpass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
			this.txtpass.BorderRadius = 8;
			this.txtpass.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtpass.DefaultText = "";
			this.txtpass.FillColor = System.Drawing.Color.White;
			this.txtpass.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(29)))), ((int)(((byte)(72)))));
			this.txtpass.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtpass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.txtpass.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(29)))), ((int)(((byte)(72)))));
			this.txtpass.Location = new System.Drawing.Point(36, 299);
			this.txtpass.Name = "txtpass";
			this.txtpass.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
			this.txtpass.PlaceholderText = "••••••••";
			this.txtpass.SelectedText = "";
			this.txtpass.Size = new System.Drawing.Size(308, 42);
			this.txtpass.TabIndex = 1;
			this.txtpass.UseSystemPasswordChar = true;
			// 
			// btnlogin
			// 
			this.btnlogin.BorderRadius = 8;
			this.btnlogin.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnlogin.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(29)))), ((int)(((byte)(72)))));
			this.btnlogin.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
			this.btnlogin.ForeColor = System.Drawing.Color.White;
			this.btnlogin.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(18)))), ((int)(((byte)(60)))));
			this.btnlogin.Location = new System.Drawing.Point(36, 365);
			this.btnlogin.Name = "btnlogin";
			this.btnlogin.Size = new System.Drawing.Size(308, 44);
			this.btnlogin.TabIndex = 2;
			this.btnlogin.Text = "Giriş Yap  ➔";
			this.btnlogin.Click += new System.EventHandler(this.btnlogin_Click);
			// 
			// btnexit
			// 
			this.btnexit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
			this.btnexit.BorderRadius = 8;
			this.btnexit.BorderThickness = 1;
			this.btnexit.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnexit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
			this.btnexit.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
			this.btnexit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
			this.btnexit.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
			this.btnexit.Location = new System.Drawing.Point(36, 420);
			this.btnexit.Name = "btnexit";
			this.btnexit.Size = new System.Drawing.Size(308, 38);
			this.btnexit.TabIndex = 3;
			this.btnexit.Text = "Uygulamadan Çık";
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
			// frmLogin
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(380, 520);
			this.Controls.Add(this.btnlogin);
			this.Controls.Add(this.btnexit);
			this.Controls.Add(this.txtpass);
			this.Controls.Add(this.txtuser);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.guna2Panel1);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmLogin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "RESTO POS - Giriş";
			this.Load += new System.EventHandler(this.frmLogin_Load);
			this.guna2Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Label labelSub;
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
