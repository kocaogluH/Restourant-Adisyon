using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Restourant_Adisyon.Model
{
	public partial class frmCategoriesAdd : Form
	{
		private readonly string con_string = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
		private SqlConnection con;
		private SqlCommand cmd;

		public frmCategoriesAdd()
		{
			InitializeComponent();
			ConfigureForm();
		}

		private void ConfigureForm()
		{
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.None;
			
			// Butonları yapılandır
			btnSave.Text = "Kaydet";
			btnClose.Text = "Kapat";
			
			// TextBox'ı yapılandır
			txtName.PlaceholderText = "Kategori Adı";
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(txtName.Text))
				{
					guna2MessageDialog1.Show("Lütfen kategori adını giriniz.");
					return;
				}

				using (con = new SqlConnection(con_string))
				{
					string query = "INSERT INTO categories (catName) VALUES (@name)";
					using (cmd = new SqlCommand(query, con))
					{
						cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
						con.Open();
						cmd.ExecuteNonQuery();
						guna2MessageDialog1.Show("Kategori başarıyla eklendi.");
						this.Close();
					}
				}
			}
			catch (Exception ex)
			{
				guna2MessageDialog1.Show($"Hata oluştu: {ex.Message}");
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				if (guna2MessageDialog1.Show("Kategori ekleme işlemini iptal etmek istediğinize emin misiniz?", "İptal Onayı") == DialogResult.OK)
				{
					this.Close();
				}
			}
			catch (Exception ex)
			{
				guna2MessageDialog1.Show($"Hata oluştu: {ex.Message}");
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			txtName.Focus();
		}
	}
}
