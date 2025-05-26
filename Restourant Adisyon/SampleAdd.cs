using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Restourant_Adisyon
{
	public partial class SampleAdd : Form
	{
		public SampleAdd()
		{
			InitializeComponent();
			ConfigureForm();
		}

		private void ConfigureForm()
		{
			// Form özelliklerini yapılandır
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.None;
			
			// Butonları yapılandır
			btnSave.Text = "Kaydet";
			btnClose.Text = "Kapat";
			
			// Panel renklerini ayarla
			guna2Panel1.FillColor = Color.MidnightBlue;
			guna2Panel2.FillColor = Color.Gainsboro;
			
			// Buton renklerini ayarla
			btnSave.FillColor = Color.HotPink;
			btnClose.FillColor = Color.DarkBlue;
		}

		public virtual void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				// Kaydetme işlemleri burada yapılacak
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Kaydetme işlemi sırasında bir hata oluştu: {ex.Message}", 
					"Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public virtual void btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show("Değişiklikleri kaydetmeden çıkmak istediğinize emin misiniz?", 
					"Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					this.DialogResult = DialogResult.Cancel;
					this.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Çıkış işlemi sırasında bir hata oluştu: {ex.Message}", 
					"Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			// Form yüklendiğinde yapılacak işlemler
		}
	}
}
