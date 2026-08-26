using System;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;

namespace Restourant_Adisyon
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _authService = new AuthService();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtuser.Text))
            {
                MessageBox.Show("Lütfen kullanıcı adı veya PIN kodunuzu girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtuser.Focus();
                return;
            }

            string inputUser = txtuser.Text.Trim();
            string inputPass = txtpass.Text.Trim();

            // 1. PIN ile Hızlı Giriş Kontrolü
            if (inputUser.Length <= 4 && string.IsNullOrEmpty(inputPass) && _authService.QuickLoginWithPin(inputUser))
            {
                OpenMainForm();
                return;
            }

            // 2. Kullanıcı Adı ve Şifre ile Doğrulama
            if (_authService.LoginWithPassword(inputUser, inputPass))
            {
                OpenMainForm();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı, şifre veya PIN kodu hatalı!", "Giriş Başarısız",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtpass.Text = "";
                txtpass.Focus();
            }
        }

        private void OpenMainForm()
        {
            this.Hide();
            formMain form = new formMain();
            form.FormClosed += (s, args) => this.Close();
            form.Show();
        }
    }
}
