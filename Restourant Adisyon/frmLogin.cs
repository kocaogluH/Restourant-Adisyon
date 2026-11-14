using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _authService = new AuthService();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse
        );

        private Guna2ControlBox _ctrlClose;
        private Label _lblHint;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
            }
            catch { }

            ApplyTheme();
            SetupHeaderControls();

            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyTheme();

            ApplyLocalization();
        }

        private void SetupHeaderControls()
        {
            // Sağ Üst Kapatma Butonu
            if (_ctrlClose == null && guna2Panel1 != null)
            {
                _ctrlClose = new Guna2ControlBox
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(guna2Panel1.Width - 38, 10),
                    Size = new Size(28, 28),
                    FillColor = Color.Transparent,
                    IconColor = Color.FromArgb(203, 213, 225),
                    Cursor = Cursors.Hand
                };
                _ctrlClose.HoverState.FillColor = Color.FromArgb(225, 29, 72);
                _ctrlClose.Click += (s, e) => Application.Exit();
                guna2Panel1.Controls.Add(_ctrlClose);
            }

            // Demo Hızlı Giriş İpucu (Tıklayınca doldurur)
            if (_lblHint == null)
            {
                _lblHint = new Label
                {
                    Text = "💡 Hızlı Test: admin / 123  veya  PIN: 5555\n(Tıklayarak otomatik doldurabilirsiniz)",
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(308, 36),
                    Location = new Point(36, 468),
                    Cursor = Cursors.Hand
                };
                _lblHint.Click += (s, e) =>
                {
                    txtuser.Text = "admin";
                    txtpass.Text = "123";
                };
                this.Controls.Add(_lblHint);
            }
        }

        private void ApplyTheme()
        {
            this.BackColor = AppTheme.CardBackground;

            if (guna2Panel1 != null)
            {
                guna2Panel1.BackColor = AppTheme.SidebarBackground;
                guna2Panel1.FillColor = AppTheme.SidebarBackground;
            }

            if (label1 != null) label1.ForeColor = AppTheme.TextSecondary;
            if (label2 != null) label2.ForeColor = AppTheme.TextSecondary;

            if (txtuser != null)
            {
                txtuser.FillColor = AppTheme.CardBackground;
                txtuser.ForeColor = AppTheme.TextPrimary;
                txtuser.BorderColor = AppTheme.BorderColor;
                txtuser.FocusedState.BorderColor = AppTheme.BrandPrimary;
            }

            if (txtpass != null)
            {
                txtpass.FillColor = AppTheme.CardBackground;
                txtpass.ForeColor = AppTheme.TextPrimary;
                txtpass.BorderColor = AppTheme.BorderColor;
                txtpass.FocusedState.BorderColor = AppTheme.BrandPrimary;
            }

            if (btnlogin != null)
            {
                btnlogin.FillColor = AppTheme.BrandPrimary;
                btnlogin.HoverState.FillColor = Color.FromArgb(190, 18, 60);
                btnlogin.ForeColor = Color.White;
            }

            if (btnexit != null)
            {
                btnexit.FillColor = Color.FromArgb(241, 245, 249);
                btnexit.BorderColor = AppTheme.BorderColor;
                btnexit.ForeColor = Color.FromArgb(71, 85, 105);
            }
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label3 != null)   label3.Text   = loc.GetString("Login_Subtitle") ?? "Lütfen giriş bilgilerinizi yazınız.";
            if (label1 != null)   label1.Text   = loc.GetString("Username") ?? "Kullanıcı Adı veya PIN";
            if (label2 != null)   label2.Text   = loc.GetString("Password") ?? "Şifre";
            if (btnlogin != null) btnlogin.Text = (loc.GetString("Login_Btn") ?? "Giriş Yap") + "  ➔";
            if (btnexit != null)  btnexit.Text  = loc.GetString("Exit_Btn") ?? "Uygulamadan Çık";
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtuser.Text))
            {
                MessageBox.Show(LocalizationService.Instance.GetString("Username") ?? "Lütfen kullanıcı adı veya PIN giriniz.", "Uyarı",
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
