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
	public partial class SampleView : Form
	{
		public SampleView()
		{
			InitializeComponent();
			ConfigureForm();
		}

		private void ConfigureForm()
		{
			// Form özelliklerini yapılandır
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.None;
			
			// Etiketleri yapılandır
			lblHeader.Text = "Başlık";
			lblSearch.Text = "Ara";
			
			// Arama kutusunu yapılandır
			txtSearch.PlaceholderText = "Arama yapmak için yazın...";
			
			// Butonları yapılandır
			btnAdd.ImageSize = new Size(55, 55);
			btnAdd.HoverState.ImageSize = new Size(57, 57);
			btnAdd.PressedState.ImageSize = new Size(55, 55);
		}

		public virtual void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				// Ekleme işlemleri burada yapılacak
				MessageBox.Show("Ekleme işlemi başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ekleme işlemi sırasında bir hata oluştu: {ex.Message}", 
					"Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public virtual void txtSearch_TextChanged(object sender, EventArgs e)
		{
			try
			{
				// Arama işlemleri burada yapılacak
				string searchText = txtSearch.Text.Trim();
				if (!string.IsNullOrEmpty(searchText))
				{
					// Arama işlemlerini gerçekleştir
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Arama işlemi sırasında bir hata oluştu: {ex.Message}", 
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
