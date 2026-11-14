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

        private Label _lblBrandTitle;
        private Label _lblBrandSub;
        private Guna2Button _badgeIcon;
        private Label _lblHint;
        private Guna2ControlBox _ctrlClose;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            ApplyModernLoginDesign();
            ApplyTheme();

            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyTheme();

            ApplyLocalization();
        }

        private void ApplyModernLoginDesign()
        {
            // 1. Form Geometrisi & Köşe Yuvarlama
            this.Size = new Size(380, 540);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = AppTheme.CardBackground;
            try
            {
                this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
            }
            catch { }

            // 2. Üst Header Paneli (guna2Panel1)
            if (guna2Panel1 != null)
            {
                guna2Panel1.Height = 175;
                guna2Panel1.FillColor = AppTheme.SidebarBackground;
                guna2Panel1.BackColor = AppTheme.SidebarBackground;

                // Eski pikselli resmi gizle
                if (guna2PictureBox1 != null)
                {
                    guna2PictureBox1.Visible = false;
                }

                // Şık Marka İkonu (Rose Red Badge)
                if (_badgeIcon == null)
                {
                    _badgeIcon = new Guna2Button
                    {
                        Text = "⚡",
                        Size = new Size(46, 46),
                        Location = new Point((guna2Panel1.Width - 46) / 2, 22),
                        BorderRadius = 23,
                        FillColor = AppTheme.BrandPrimary,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                        Cursor = Cursors.Default
                    };
                    guna2Panel1.Controls.Add(_badgeIcon);

                    // Başlık
                    _lblBrandTitle = new Label
                    {
                        Text = "RESTO POS",
                        Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                        ForeColor = Color.White,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(guna2Panel1.Width, 26),
                        Location = new Point(0, 74),
                        BackColor = Color.Transparent
                    };
                    guna2Panel1.Controls.Add(_lblBrandTitle);

                    // Alt Başlık
                    _lblBrandSub = new Label
                    {
                        Text = "Akıllı Restoran Yönetim Sistemi",
                        Font = new Font("Segoe UI", 8.5F),
                        ForeColor = Color.FromArgb(148, 163, 184),
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(guna2Panel1.Width, 20),
                        Location = new Point(0, 102),
                        BackColor = Color.Transparent
                    };
                    guna2Panel1.Controls.Add(_lblBrandSub);

                    // Sağ Üst Kapatma Butonu
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

                // Yazım hatası olan eski label3'ü düzelt ve konumlandır
                if (label3 != null)
                {
                    label3.Text = "Lütfen giriş bilgilerinizi yazınız.";
                    label3.Font = new Font("Segoe UI", 8.5F);
                    label3.ForeColor = Color.FromArgb(203, 213, 225);
                    label3.AutoSize = false;
                    label3.TextAlign = ContentAlignment.MiddleCenter;
                    label3.Size = new Size(guna2Panel1.Width, 22);
                    label3.Location = new Point(0, 132);
                    label3.BackColor = Color.Transparent;
                }
            }

            // 3. Giriş Kontrolleri ve Hizalama
            int contentX = 36;
            int contentWidth = 308;

            if (label1 != null)
            {
                label1.Text = "👤 Kullanıcı Adı veya PIN";
                label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                label1.ForeColor = AppTheme.TextSecondary;
                label1.Location = new Point(contentX, 195);
                label1.AutoSize = true;
            }

            if (txtuser != null)
            {
                txtuser.Location = new Point(contentX, 218);
                txtuser.Size = new Size(contentWidth, 42);
                txtuser.BorderRadius = 8;
                txtuser.BorderColor = AppTheme.BorderColor;
                txtuser.FocusedState.BorderColor = AppTheme.BrandPrimary;
                txtuser.FillColor = AppTheme.CardBackground;
                txtuser.ForeColor = AppTheme.TextPrimary;
                txtuser.Font = new Font("Segoe UI", 10F);
                txtuser.PlaceholderText = "admin veya 4 haneli PIN";
            }

            if (label2 != null)
            {
                label2.Text = "🔒 Şifre";
                label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                label2.ForeColor = AppTheme.TextSecondary;
                label2.Location = new Point(contentX, 274);
                label2.AutoSize = true;
            }

            if (txtpass != null)
            {
                txtpass.Location = new Point(contentX, 297);
                txtpass.Size = new Size(contentWidth, 42);
                txtpass.BorderRadius = 8;
                txtpass.BorderColor = AppTheme.BorderColor;
                txtpass.FocusedState.BorderColor = AppTheme.BrandPrimary;
                txtpass.FillColor = AppTheme.CardBackground;
                txtpass.ForeColor = AppTheme.TextPrimary;
                txtpass.Font = new Font("Segoe UI", 10F);
                txtpass.PlaceholderText = "•••••••• (PIN ile giriş için boş bırakın)";
            }

            if (btnlogin != null)
            {
                btnlogin.Location = new Point(contentX, 360);
                btnlogin.Size = new Size(contentWidth, 44);
                btnlogin.BorderRadius = 8;
                btnlogin.FillColor = AppTheme.BrandPrimary;
                btnlogin.HoverState.FillColor = Color.FromArgb(190, 18, 60);
                btnlogin.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                btnlogin.ForeColor = Color.White;
                btnlogin.Text = "Giriş Yap  ➔";
                btnlogin.Cursor = Cursors.Hand;
            }

            if (btnexit != null)
            {
                btnexit.Location = new Point(contentX, 414);
                btnexit.Size = new Size(contentWidth, 38);
                btnexit.BorderRadius = 8;
                btnexit.FillColor = Color.FromArgb(241, 245, 249);
                btnexit.HoverState.FillColor = Color.FromArgb(226, 232, 240);
                btnexit.BorderColor = AppTheme.BorderColor;
                btnexit.BorderThickness = 1;
                btnexit.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btnexit.ForeColor = Color.FromArgb(71, 85, 105);
                btnexit.Text = "Uygulamadan Çık";
                btnexit.Cursor = Cursors.Hand;
            }

            // 4. Demo Hızlı Giriş İpucu (Tıklayınca doldurur)
            if (_lblHint == null)
            {
                _lblHint = new Label
                {
                    Text = "💡 Hızlı Test: admin / 123  veya  PIN: 5555\n(Tıklayarak otomatik doldurabilirsiniz)",
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(contentWidth, 36),
                    Location = new Point(contentX, 470),
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
            }

            if (txtpass != null)
            {
                txtpass.FillColor = AppTheme.CardBackground;
                txtpass.ForeColor = AppTheme.TextPrimary;
                txtpass.BorderColor = AppTheme.BorderColor;
            }
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label3 != null)   label3.Text   = loc.GetString("Login_Subtitle") ?? "Lütfen giriş bilgilerinizi giriniz";
            if (label1 != null)   label1.Text   = loc.GetString("Username") ?? "👤 Kullanıcı Adı veya PIN";
            if (label2 != null)   label2.Text   = loc.GetString("Password") ?? "🔒 Şifre";
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
