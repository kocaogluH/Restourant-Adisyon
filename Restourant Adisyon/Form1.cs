using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Properties;

namespace Restourant_Adisyon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            try
            {
                InitializeComponent();
                ConfigureForm();
                LoadUserSettings();
            }
            catch (Exception ex)
            {
                ShowError("Form başlatılırken hata oluştu", ex);
            }
        }

        private void ConfigureForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            
            txtuser.PlaceholderText = "Kullanıcı Adı";
            txtpass.PlaceholderText = "Şifre";
            txtpass.UseSystemPasswordChar = true;
            
            btnlogin.Text = "Giriş Yap";
            btnexit.Text = "Çıkış";

            // Enter tuşu ile giriş yapma
            this.AcceptButton = btnlogin;
        }

        private void LoadUserSettings()
        {
            try
            {
                if (MainClass.GetRememberMe())
                {
                    txtuser.Text = MainClass.GetLastUser();
                    txtpass.Text = string.Empty;
                    txtpass.Focus();
                }
                else
                {
                    txtuser.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError("Ayarlar yüklenirken hata oluştu", ex);
            }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2MessageDialog1.Show("Çıkış yapmak istediğinize emin misiniz?", "Çıkış Onayı") == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                ShowError("Çıkış yapılırken hata oluştu", ex);
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtuser.Text) || string.IsNullOrWhiteSpace(txtpass.Text))
                {
                    guna2MessageDialog1.Show("Lütfen kullanıcı adı ve şifre giriniz.");
                    return;
                }

                if (MainClass.IsValidUser(txtuser.Text, txtpass.Text))
                {
                    this.Hide();
                    using (var form = new formMain())
                    {
                        form.ShowDialog();
                    }
                    this.Close();
                }
                else
                {
                    guna2MessageDialog1.Show("Geçersiz kullanıcı adı veya şifre!");
                    txtpass.Clear();
                    txtpass.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError("Giriş yapılırken hata oluştu", ex);
            }
        }

        private void ShowError(string message, Exception ex)
        {
            guna2MessageDialog1.Show($"{message}: {ex.Message}");
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                if (string.IsNullOrEmpty(txtuser.Text))
                {
                    txtuser.Focus();
                }
                else
                {
                    txtpass.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError("Form yüklenirken hata oluştu", ex);
            }
        }
    }
}
