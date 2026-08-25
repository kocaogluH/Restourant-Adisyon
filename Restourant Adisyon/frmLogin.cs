using System;
using System.Windows.Forms;

namespace Restourant_Adisyon
{
    public partial class frmLogin : Form
    {
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
                MessageBox.Show("Lütfen kullanıcı adı girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtuser.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtpass.Text))
            {
                MessageBox.Show("Lütfen şifre girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtpass.Focus();
                return;
            }

            if (MainClass.IsValidUser(txtuser.Text.Trim(), txtpass.Text.Trim()))
            {
                this.Hide();
                formMain form = new formMain();
                form.FormClosed += (s, args) => this.Close();
                form.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Giriş Başarısız",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtpass.Text = "";
                txtpass.Focus();
            }
        }
    }
}
