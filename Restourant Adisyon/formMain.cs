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
        private Label _lblPageTitle;
        private Guna2Panel _pnlUserChip;
        private Guna2Button _btnLogout;
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

            UpdatePageTitle(f);
        }

        private void UpdatePageTitle(Form f)
        {
            if (_lblPageTitle == null) return;

            string title = "📊 Ana Sayfa & Dashboard";
            if (f is frmPOS) title = "⚡ POS Satış & Hızlı Adisyon";
            else if (f is frmTableView) title = "🪑 Masalar & Kroki Haritası";
            else if (f is frmKitchenView) title = "👨‍🍳 Mutfak Ekranı (KDS)";
            else if (f is frmProductView) title = "🍽️ Ürün Yönetimi";
            else if (f is frmCategoryview) title = "📂 Kategori Yönetimi";
            else if (f is frmStaffView) title = "👥 Personel & Yetkiler";
            else if (f is frmWaiterView) title = "🛎️ Garson Servis Masaları";
            else if (f is frmInventory) title = "📦 Stok & Reçete Takibi";
            else if (f is frmReports) title = "📈 Ciro & Satış Raporları";

            _lblPageTitle.Text = title;
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            _obj = this;

            this.WindowState = FormWindowState.Maximized;

            // Dil ve Tema dinleyicileri
            LocalizationService.Instance.OnLanguageChanged += Instance_OnLanguageChanged;
            ThemeManager.Instance.OnThemeChanged += Instance_OnThemeChanged;

            // Tüm eski çakışan pikselli resimleri gizle ve temizle
            CleanUpLegacyControls();

            // Üst Bar & Sol Menü Yenilemesi
            SetupTopBarControls();
            ApplyModernSidebarStyle();

            // Ekran yeniden boyutlandığında veya DPI değişimlerinde üst bar çakışmasını önle
            this.Resize += (s, ev) => LayoutTopBar();
            this.SizeChanged += (s, ev) => LayoutTopBar();
            this.Shown += (s, ev) => LayoutTopBar();
            if (guna2CustomGradientPanel1 != null)
                guna2CustomGradientPanel1.Resize += (s, ev) => LayoutTopBar();
            if (guna2ControlBox2 != null)
                guna2ControlBox2.LocationChanged += (s, ev) => LayoutTopBar();

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

            MainClass.EnableEnterKeyConfirmation(
                this,
                () =>
                {
                    if (_currentChildForm != null && !_currentChildForm.IsDisposed)
                        MainClass.TriggerFormEnter(_currentChildForm);
                }
            );
        }

        private void CleanUpLegacyControls()
        {
            // Eski pikselli ikonları ve daire resim kutularını gizle
            Guna2CirclePictureBox[] circles = new Guna2CirclePictureBox[]
            {
                guna2CirclePictureBox1,
                guna2CirclePictureBox2, guna2CirclePictureBox3, guna2CirclePictureBox4,
                guna2CirclePictureBox5, guna2CirclePictureBox6, guna2CirclePictureBox7,
                guna2CirclePictureBox8, guna2CirclePictureBox9
            };

            foreach (var c in circles)
            {
                if (c != null)
                {
                    c.Visible = false;
                    c.Image = null;
                }
            }

            if (label1 != null)
                label1.Visible = false;
        }

        private void SetupTopBarControls()
        {
            if (guna2CustomGradientPanel1 == null) return;

            guna2CustomGradientPanel1.Height = 60;
            guna2CustomGradientPanel1.Padding = new Padding(16, 8, 16, 8);

            // Sayfa Başlığı (Sol üst)
            if (_lblPageTitle == null)
            {
                _lblPageTitle = new Label
                {
                    Text = "📊 Ana Sayfa & Dashboard",
                    Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    ForeColor = ThemeManager.Instance.TextPrimary,
                    Location = new Point(16, 17),
                    AutoSize = true
                };
                guna2CustomGradientPanel1.Controls.Add(_lblPageTitle);
            }

            // Eski lblUser'ı gizle
            if (lblUser != null) lblUser.Visible = false;

            // Modern Kullanıcı Rozeti
            if (_pnlUserChip == null)
            {
                _pnlUserChip = new Guna2Panel
                {
                    Size = new Size(175, 36),
                    BorderRadius = 18,
                    BorderThickness = 1,
                    BorderColor = ThemeManager.Instance.BorderColor,
                    FillColor = ThemeManager.Instance.SurfaceCard,
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true
                };

                Label lblUserInfo = new Label
                {
                    Text = $"👤 {MainClass.USER} ({MainClass.ROLE})",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = ThemeManager.Instance.TextPrimary,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                _pnlUserChip.Controls.Add(lblUserInfo);
                guna2CustomGradientPanel1.Controls.Add(_pnlUserChip);
            }

            // Tema Değiştirme Butonu (🌙 Cyber Dark / 👑 Luxury Amber / ☀️ Clean Light)
            if (_btnThemeToggle == null)
            {
                _btnThemeToggle = new Guna2Button
                {
                    Text = ThemeManager.Instance.GetThemeIcon() + " " + ThemeManager.Instance.GetThemeName(),
                    Size = new Size(145, 36),
                    AutoRoundedCorners = true,
                    FillColor = ThemeManager.Instance.SurfaceElevated,
                    ForeColor = ThemeManager.Instance.TextPrimary,
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Animated = true,
                    Cursor = Cursors.Hand
                };
                _btnThemeToggle.Click += (s, e) => ThemeManager.Instance.CycleNextTheme();
                guna2CustomGradientPanel1.Controls.Add(_btnThemeToggle);
            }

            // Dil Butonu (TR / EN)
            if (_btnLangToggle == null)
            {
                _btnLangToggle = new Guna2Button
                {
                    Text = LocalizationService.Instance.CurrentLanguage == Language.TR ? "🇹🇷 TR" : "🇬🇧 EN",
                    Size = new Size(72, 36),
                    AutoRoundedCorners = true,
                    FillColor = ThemeManager.Instance.BrandPrimary,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
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
                guna2CustomGradientPanel1.Controls.Add(_btnLangToggle);
            }

            LayoutTopBar();
        }

        private void LayoutTopBar()
        {
            if (guna2CustomGradientPanel1 == null) return;

            // ControlBox'ların (Simge, Büyüt, Kapat) en solundaki butonun X koordinatını dinamik tespit et.
            // DPI scaling (100%, 125%, 150%) ve çözünürlük fark etmeksizin asla çakışma yaşanmaz.
            int minControlBoxLeft = guna2CustomGradientPanel1.Width;
            if (guna2ControlBox2 != null && guna2ControlBox2.Visible && guna2ControlBox2.Left > 100)
                minControlBoxLeft = Math.Min(minControlBoxLeft, guna2ControlBox2.Left);
            if (guna2ControlBox3 != null && guna2ControlBox3.Visible && guna2ControlBox3.Left > 100)
                minControlBoxLeft = Math.Min(minControlBoxLeft, guna2ControlBox3.Left);
            if (btnexit != null && btnexit.Visible && btnexit.Left > 100)
                minControlBoxLeft = Math.Min(minControlBoxLeft, btnexit.Left);

            int safeRight = (minControlBoxLeft < guna2CustomGradientPanel1.Width)
                ? minControlBoxLeft - 18
                : guna2CustomGradientPanel1.Width - 250;

            int currentRight = safeRight;

            if (_btnLangToggle != null)
            {
                currentRight -= _btnLangToggle.Width;
                _btnLangToggle.Location = new Point(currentRight, 11);
                currentRight -= 10;
            }

            if (_btnThemeToggle != null)
            {
                currentRight -= _btnThemeToggle.Width;
                _btnThemeToggle.Location = new Point(currentRight, 11);
                currentRight -= 12;
            }

            if (_pnlUserChip != null)
            {
                currentRight -= _pnlUserChip.Width;
                _pnlUserChip.Location = new Point(currentRight, 11);
            }
        }

        private void Instance_OnLanguageChanged(object sender, EventArgs e)
        {
            if (_btnLangToggle != null)
                _btnLangToggle.Text = LocalizationService.Instance.CurrentLanguage == Language.TR ? "🇹🇷 TR" : "🇬🇧 EN";

            ApplyModernSidebarStyle();
            if (_currentChildForm != null)
                UpdatePageTitle(_currentChildForm);
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
                _btnLangToggle.FillColor = ThemeManager.Instance.BrandPrimary;

            if (_lblPageTitle != null)
                _lblPageTitle.ForeColor = ThemeManager.Instance.TextPrimary;

            if (_pnlUserChip != null)
            {
                _pnlUserChip.FillColor = ThemeManager.Instance.SurfaceCard;
                _pnlUserChip.BorderColor = ThemeManager.Instance.BorderColor;
            }

            ApplyModernSidebarStyle();

            // Mevcut alt formu yenile
            if (_currentChildForm != null && !_currentChildForm.IsDisposed)
            {
                Type formType = _currentChildForm.GetType();
                Form newInstance = (Form)Activator.CreateInstance(formType);
                AddControls(newInstance);
            }
        }

        private void ApplyModernSidebarStyle()
        {
            // Panel & Arka Plan
            guna2Panel1.FillColor = ThemeManager.Instance.SidebarBackground;
            guna2Panel1.BackColor = ThemeManager.Instance.SidebarBackground;
            guna2Panel1.Width = 224;

            if (ControlsPanel != null)
                ControlsPanel.BackColor = ThemeManager.Instance.Background;

            if (guna2CustomGradientPanel1 != null)
            {
                Color headerCol = ThemeManager.Instance.HeaderBackground;
                guna2CustomGradientPanel1.BackColor  = headerCol;
                guna2CustomGradientPanel1.FillColor  = headerCol;
                guna2CustomGradientPanel1.FillColor2 = headerCol;
                guna2CustomGradientPanel1.FillColor3 = headerCol;
                guna2CustomGradientPanel1.FillColor4 = headerCol;
            }

            // Modern Marka Başlığı Ekle (Sidebar Üst Alanı)
            SetupBrandHeader();

            // Menü Öğeleri Listesi
            Tuple<Guna2Button, string, string>[] menuItems = new Tuple<Guna2Button, string, string>[]
            {
                Tuple.Create(btnHome,       "📊", LocalizationService.Instance.GetString("Nav_Home")),
                Tuple.Create(btnPos,        "⚡", LocalizationService.Instance.GetString("Nav_POS")),
                Tuple.Create(btnTables,     "🪑", LocalizationService.Instance.GetString("Nav_Tables")),
                Tuple.Create(btnKitchen,    "👨‍🍳", LocalizationService.Instance.GetString("Nav_Kitchen")),
                Tuple.Create(btnProducts,   "🍽️", LocalizationService.Instance.GetString("Nav_Products")),
                Tuple.Create(btnCatagories, "📂", LocalizationService.Instance.GetString("Nav_Categories")),
                Tuple.Create(btnStaff,      "👥", LocalizationService.Instance.GetString("Nav_Staff")),
                Tuple.Create(btnService,    "🛎️", LocalizationService.Instance.GetString("Nav_Waiter")),
                Tuple.Create(btnInventory,   "📦", LocalizationService.Instance.GetString("Nav_Inventory")),
                Tuple.Create(btnReports,    "📈", LocalizationService.Instance.GetString("Nav_Reports")),
                Tuple.Create(btnSettings,   "⚙️", LocalizationService.Instance.GetString("Nav_Settings"))
            };

            int startY = 95;
            int btnHeight = 44;
            int spacing = 5;
            int currentY = startY;

            foreach (var item in menuItems)
            {
                var btn = item.Item1;
                var icon = item.Item2;
                var title = item.Item3;
                if (btn == null) continue;

                // Eski pikselli resimleri kaldır
                btn.Image = null;
                btn.Text = $"  {icon}   {title}";
                btn.Animated = true;
                btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                btn.Size = new Size(204, btnHeight);
                btn.Location = new Point(10, currentY);

                // Modern Oval Kapsül
                btn.AutoRoundedCorners = true;
                btn.UseTransparentBackground = true;
                btn.BorderThickness = 0;
                btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btn.TextAlign = HorizontalAlignment.Left;
                btn.TextOffset = new Point(10, 0);

                // Normal Durum (Göz yormayan şık slate)
                btn.FillColor = Color.Transparent;
                btn.ForeColor = Color.FromArgb(148, 163, 184);

                // Hover Durumu
                btn.HoverState.FillColor = Color.FromArgb(30, 41, 59);
                btn.HoverState.ForeColor = Color.White;

                // Seçili / Aktif Durum (Canlı Rose / Vurgulu Kapsül - Beyaz Üstüne Beyaz HATASI YOK!)
                btn.CheckedState.FillColor = ThemeManager.Instance.BrandPrimary;
                btn.CheckedState.ForeColor = Color.White;

                currentY += btnHeight + spacing;
            }

            // Alt Oturumu Kapat Butonu
            if (_btnLogout == null)
            {
                _btnLogout = new Guna2Button
                {
                    Text = "  🚪   Çıkış Yap",
                    Size = new Size(204, 40),
                    Location = new Point(10, currentY + 15),
                    AutoRoundedCorners = true,
                    FillColor = Color.Transparent,
                    ForeColor = Color.FromArgb(239, 68, 68),
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    TextAlign = HorizontalAlignment.Left,
                    TextOffset = new Point(10, 0),
                    Cursor = Cursors.Hand
                };
                _btnLogout.HoverState.FillColor = Color.FromArgb(239, 68, 68);
                _btnLogout.HoverState.ForeColor = Color.White;
                _btnLogout.Click += (s, e) => Application.Exit();
                guna2Panel1.Controls.Add(_btnLogout);
            }
            else
            {
                _btnLogout.Location = new Point(10, currentY + 15);
            }
        }

        private Panel _pnlBrandHeader;
        private void SetupBrandHeader()
        {
            if (_pnlBrandHeader != null) return;

            _pnlBrandHeader = new Panel
            {
                Size = new Size(204, 75),
                Location = new Point(10, 12),
                BackColor = Color.Transparent
            };

            Label lblLogo = new Label
            {
                Text = "🍽️",
                Font = new Font("Segoe UI", 24F),
                Location = new Point(4, 8),
                Size = new Size(48, 55),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblTitle = new Label
            {
                Text = "RESTO POS",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(54, 14),
                Size = new Size(145, 24)
            };

            Label lblSub = new Label
            {
                Text = "Yönetim & Adisyon",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(56, 38),
                Size = new Size(145, 20)
            };

            _pnlBrandHeader.Controls.Add(lblLogo);
            _pnlBrandHeader.Controls.Add(lblTitle);
            _pnlBrandHeader.Controls.Add(lblSub);

            guna2Panel1.Controls.Add(_pnlBrandHeader);
        }

        private void ReorderVisibleButtons()
        {
            Guna2Button[] navButtons = new Guna2Button[]
            {
                btnHome, btnPos, btnTables, btnKitchen, btnProducts,
                btnCatagories, btnStaff, btnService, btnInventory, btnReports, btnSettings
            };

            int startY = 95;
            int btnHeight = 44;
            int spacing = 5;
            int currentY = startY;

            foreach (var btn in navButtons)
            {
                if (btn != null && btn.Visible)
                {
                    btn.Location = new Point(10, currentY);
                    currentY += btnHeight + spacing;
                }
            }

            if (_btnLogout != null)
                _btnLogout.Location = new Point(10, currentY + 15);
        }

        // ── Menü Buton Tıklamaları ──────────────────────────────────────────────
        private void btnHome_Click(object sender, EventArgs e)
        {
            SetChecked(btnHome);
            AddControls(new frmHome());
        }

        private void btnPos_Click(object sender, EventArgs e)
        {
            SetChecked(btnPos);
            AddControls(new frmPOS());
        }

        private void btnTables_Click(object sender, EventArgs e)
        {
            SetChecked(btnTables);
            AddControls(new frmTableView());
        }

        private void btnKitchen_Click(object sender, EventArgs e)
        {
            SetChecked(btnKitchen);
            AddControls(new frmKitchenView());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            SetChecked(btnProducts);
            AddControls(new frmProductView());
        }

        private void btnCatagories_Click(object sender, EventArgs e)
        {
            SetChecked(btnCatagories);
            AddControls(new frmCategoryview());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            SetChecked(btnStaff);
            AddControls(new frmStaffView());
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
