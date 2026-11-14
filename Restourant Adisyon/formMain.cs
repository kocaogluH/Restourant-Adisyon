using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.Core.Enums;
using Restourant_Adisyon.Mmodel;
using Restourant_Adisyon.Vview;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon
{
    public partial class formMain : Form
    {
        private Guna2Button _btnLangToggle;
        private Guna2Button _btnThemeToggle;
        private Form _currentChildForm;

        public formMain()
        {
            InitializeComponent();
        }

        static formMain _obj;
        public static formMain Instance
        {
            get
            {
                if (_obj == null) { _obj = new formMain(); }
                return _obj;
            }
        }

        public void AddControls(Form f)
        {
            _currentChildForm = f;
            ControlsPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            ControlsPanel.Controls.Add(f);
            f.Show();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            _obj = this;

            // Dil ve Tema değişikliklerini dinle
            LocalizationService.Instance.OnLanguageChanged += Instance_OnLanguageChanged;
            ThemeManager.Instance.OnThemeChanged += Instance_OnThemeChanged;

            // Üst bar butonlarını ekle (Dil + Tema Değiştirici)
            SetupTopBarControls();

            // Aktif Kullanıcı ve Rol gösterimi
            UpdateUserLabel();

            // Sol menü tasarımını, oval butonları ve animasyonları uygula
            ApplyModernSidebarStyle();

            // Varsayılan sayfa: Ana Sayfa
            btnHome.Checked = true;
            btnHome_Click(btnHome, null);

            // Rol Tabanlı Erişim Kontrolü
            if (MainClass.ROLE != "Admin")
            {
                if (btnCatagories != null) btnCatagories.Visible = false;
                if (btnProducts != null)   btnProducts.Visible   = false;
                if (btnStaff != null)      btnStaff.Visible      = false;
                if (btnReports != null)    btnReports.Visible    = false;
                if (btnSettings != null)   btnSettings.Visible   = false;
                ReorderVisibleButtons();
            }
        }

        private void SetupTopBarControls()
        {
            if (guna2CustomGradientPanel1 == null) return;

            // Dil Butonu (TR / EN)
            _btnLangToggle = new Guna2Button
            {
                Text = LocalizationService.Instance.CurrentLanguage == Language.TR ? "🇹🇷 TR" : "🇬🇧 EN",
                Size = new Size(82, 34),
                Location = new Point(guna2CustomGradientPanel1.Width - 190, 13),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoRoundedCorners = true,
                FillColor = ThemeManager.Instance.BrandPrimary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Animated = true,
                Cursor = Cursors.Hand
            };

            _btnLangToggle.Click += (s, e) =>
            {
                Language nextLang = LocalizationService.Instance.CurrentLanguage == Language.TR
                    ? Language.EN
                    : Language.TR;
                LocalizationService.Instance.ChangeLanguage(nextLang);
            };

            // Tema Değiştirme Butonu (🌙 Cyber Dark / 👑 Luxury Amber / ☀️ Clean Light)
            _btnThemeToggle = new Guna2Button
            {
                Text = ThemeManager.Instance.GetThemeIcon() + " " + ThemeManager.Instance.GetThemeName(),
                Size = new Size(160, 34),
                Location = new Point(guna2CustomGradientPanel1.Width - 360, 13),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoRoundedCorners = true,
                FillColor = ThemeManager.Instance.SurfaceElevated,
                ForeColor = ThemeManager.Instance.TextPrimary,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Animated = true,
                Cursor = Cursors.Hand
            };

            _btnThemeToggle.Click += (s, e) =>
            {
                ThemeManager.Instance.CycleNextTheme();
            };

            guna2CustomGradientPanel1.Controls.Add(_btnLangToggle);
            guna2CustomGradientPanel1.Controls.Add(_btnThemeToggle);
        }

        private void Instance_OnLanguageChanged(object sender, EventArgs e)
        {
            if (_btnLangToggle != null)
                _btnLangToggle.Text = LocalizationService.Instance.CurrentLanguage == Language.TR ? "🇹🇷 TR" : "🇬🇧 EN";

            UpdateUserLabel();
            ApplyModernSidebarStyle();
        }

        private void Instance_OnThemeChanged(object sender, ThemeMode e)
        {
            if (_btnThemeToggle != null)
            {
                _btnThemeToggle.Text = ThemeManager.Instance.GetThemeIcon() + " " + ThemeManager.Instance.GetThemeName();
                _btnThemeToggle.FillColor = ThemeManager.Instance.SurfaceElevated;
                _btnThemeToggle.ForeColor = ThemeManager.Instance.TextPrimary;
            }

            if (_btnLangToggle != null)
            {
                _btnLangToggle.FillColor = ThemeManager.Instance.BrandPrimary;
            }

            UpdateUserLabel();
            ApplyModernSidebarStyle();

            // Açık olan mevcut formu yeniden yükle
            if (_currentChildForm != null && !_currentChildForm.IsDisposed)
            {
                Type formType = _currentChildForm.GetType();
                Form newInstance = (Form)Activator.CreateInstance(formType);
                AddControls(newInstance);
            }
        }

        private void UpdateUserLabel()
        {
            if (lblUser != null)
            {
                string activeText = LocalizationService.Instance.GetString("Active_User");
                lblUser.Text = $"{activeText}: {MainClass.USER} ({MainClass.ROLE})";
                lblUser.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                lblUser.ForeColor = ThemeManager.Instance.TextPrimary;
            }
        }

        private void ApplyModernSidebarStyle()
        {
            // Panel & Arka Plan Renkleri
            guna2Panel1.FillColor = ThemeManager.Instance.SidebarBackground;
            guna2Panel1.Width = 215;

            if (ControlsPanel != null)
                ControlsPanel.BackColor = ThemeManager.Instance.Background;

            // Üst başlık paneli rengi
            if (guna2CustomGradientPanel1 != null)
            {
                Color headerCol = ThemeManager.Instance.HeaderBackground;
                guna2CustomGradientPanel1.FillColor  = headerCol;
                guna2CustomGradientPanel1.FillColor2 = headerCol;
                guna2CustomGradientPanel1.FillColor3 = headerCol;
                guna2CustomGradientPanel1.FillColor4 = headerCol;
            }

            // Çakışan ekstra resim kutularını gizle
            Guna2CirclePictureBox[] circles = new Guna2CirclePictureBox[]
            {
                guna2CirclePictureBox2, guna2CirclePictureBox3, guna2CirclePictureBox4,
                guna2CirclePictureBox5, guna2CirclePictureBox6, guna2CirclePictureBox7,
                guna2CirclePictureBox8, guna2CirclePictureBox9
            };
            foreach (var c in circles)
            {
                if (c != null) c.Visible = false;
            }

            // Başlık yazısı
            if (label1 != null)
            {
                label1.Text = LocalizationService.Instance.GetString("App_Title");
                label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                label1.ForeColor = ThemeManager.Instance.BrandPrimary;
                label1.TextAlign = ContentAlignment.MiddleCenter;
            }

            // Dinamik Dil Çevirili Buton Listesi
            Tuple<Guna2Button, string>[] menuItems = new Tuple<Guna2Button, string>[]
            {
                Tuple.Create(btnHome,       LocalizationService.Instance.GetString("Nav_Home")),
                Tuple.Create(btnCatagories, LocalizationService.Instance.GetString("Nav_Categories")),
                Tuple.Create(btnProducts,   LocalizationService.Instance.GetString("Nav_Products")),
                Tuple.Create(btnTables,     LocalizationService.Instance.GetString("Nav_Tables")),
                Tuple.Create(btnStaff,      LocalizationService.Instance.GetString("Nav_Staff")),
                Tuple.Create(btnPos,        LocalizationService.Instance.GetString("Nav_POS")),
                Tuple.Create(btnKitchen,    LocalizationService.Instance.GetString("Nav_Kitchen")),
                Tuple.Create(btnService,    LocalizationService.Instance.GetString("Nav_Waiter")),
                Tuple.Create(btnInventory,   LocalizationService.Instance.GetString("Nav_Inventory")),
                Tuple.Create(btnReports,    LocalizationService.Instance.GetString("Nav_Reports")),
                Tuple.Create(btnSettings,   LocalizationService.Instance.GetString("Nav_Settings"))
            };

            int startY = 155;
            int btnHeight = 42;
            int spacing = 6;
            int currentY = startY;

            foreach (var item in menuItems)
            {
                var btn = item.Item1;
                var title = item.Item2;
                if (btn == null) continue;

                btn.Text = "  " + title;
                btn.Animated = true;
                btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                btn.Size = new Size(190, btnHeight);
                btn.Location = new Point(12, currentY);

                // OVAL / DAİRESEL KAPSÜL ŞEKİLLENDİRME
                btn.AutoRoundedCorners = true;
                btn.UseTransparentBackground = true;
                btn.BorderThickness = 0;
                btn.BorderColor = Color.Transparent;

                btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btn.ImageAlign = HorizontalAlignment.Left;
                btn.ImageOffset = new Point(10, 0);
                btn.TextOffset = new Point(8, 0);

                // Normal Durum
                btn.FillColor = Color.Transparent;
                btn.ForeColor = ThemeManager.Instance.TextSecondary;

                // Hover Durum
                btn.HoverState.FillColor = ThemeManager.Instance.SurfaceElevated;
                btn.HoverState.ForeColor = Color.White;

                // Checked Durum (Tema Renginde Kapsül)
                btn.CheckedState.FillColor = ThemeManager.Instance.BrandPrimary;
                btn.CheckedState.ForeColor = Color.White;

                currentY += btnHeight + spacing;
            }
        }

        private void ReorderVisibleButtons()
        {
            Guna2Button[] navButtons = new Guna2Button[]
            {
                btnHome, btnCatagories, btnProducts, btnTables, btnStaff,
                btnPos, btnKitchen, btnService, btnInventory, btnReports, btnSettings
            };

            int startY = 155;
            int btnHeight = 42;
            int spacing = 6;
            int currentY = startY;

            foreach (var btn in navButtons)
            {
                if (btn != null && btn.Visible)
                {
                    btn.Location = new Point(12, currentY);
                    currentY += btnHeight + spacing;
                }
            }
        }

        // ── Buton Tıklama İşleyicileri ──────────────────────────────────────────
        private void btnHome_Click(object sender, EventArgs e)
        {
            SetChecked(btnHome);
            AddControls(new frmHome());
        }

        private void btnCatagories_Click(object sender, EventArgs e)
        {
            SetChecked(btnCatagories);
            AddControls(new frmCategoryview());
        }

        private void btnTables_Click(object sender, EventArgs e)
        {
            SetChecked(btnTables);
            AddControls(new frmTableView());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            SetChecked(btnStaff);
            AddControls(new frmStaffView());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            SetChecked(btnProducts);
            AddControls(new frmProductView());
        }

        private void btnPos_Click(object sender, EventArgs e)
        {
            SetChecked(btnPos);
            AddControls(new frmPOS());
        }

        private void btnKitchen_Click(object sender, EventArgs e)
        {
            SetChecked(btnKitchen);
            AddControls(new frmKitchenView());
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            SetChecked(btnService);
            AddControls(new frmWaiterView());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            SetChecked(btnReports);
            AddControls(new frmReports());
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            SetChecked(btnInventory);
            AddControls(new frmInventory());
        }

        private void SetChecked(Guna2Button target)
        {
            if (target != null) target.Checked = true;
        }
    }
}
