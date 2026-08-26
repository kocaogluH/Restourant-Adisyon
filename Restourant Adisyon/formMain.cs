using Guna.UI2.WinForms;
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

            // Dil değişikliği event'ini dinle
            LocalizationService.Instance.OnLanguageChanged += Instance_OnLanguageChanged;

            // Üst bar dil değiştirme butonunu ekle
            SetupLanguageButton();

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

        private void SetupLanguageButton()
        {
            if (guna2CustomGradientPanel1 == null) return;

            _btnLangToggle = new Guna2Button
            {
                Text = LocalizationService.Instance.CurrentLanguage == Language.TR ? "🇹🇷 TR" : "🇬🇧 EN",
                Size = new Size(80, 32),
                Location = new Point(guna2CustomGradientPanel1.Width - 230, 13),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoRoundedCorners = true,
                FillColor = Color.FromArgb(241, 85, 126),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Animated = true
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

        private void Instance_OnLanguageChanged(object sender, EventArgs e)
        {
            if (_btnLangToggle != null)
            {
                _btnLangToggle.Text = LocalizationService.Instance.CurrentLanguage == Language.TR ? "🇹🇷 TR" : "🇬🇧 EN";
            }
            UpdateUserLabel();
            ApplyModernSidebarStyle();
        }

        private void UpdateUserLabel()
        {
            if (lblUser != null)
            {
                string activeText = LocalizationService.Instance.GetString("Active_User");
                lblUser.Text = $"{activeText}: {MainClass.USER} ({MainClass.ROLE})";
                lblUser.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                lblUser.ForeColor = Color.FromArgb(50, 55, 89);
            }
        }

        private void ApplyModernSidebarStyle()
        {
            // Sol panel rengi (Şık koyu lacivert/indigo)
            guna2Panel1.FillColor = Color.FromArgb(34, 38, 58);
            guna2Panel1.Width = 215;

            // Üst başlık paneli rengi
            if (guna2CustomGradientPanel1 != null)
            {
                guna2CustomGradientPanel1.FillColor  = Color.FromArgb(245, 247, 250);
                guna2CustomGradientPanel1.FillColor2 = Color.FromArgb(245, 247, 250);
                guna2CustomGradientPanel1.FillColor3 = Color.FromArgb(245, 247, 250);
                guna2CustomGradientPanel1.FillColor4 = Color.FromArgb(245, 247, 250);
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
                label1.ForeColor = Color.FromArgb(241, 85, 126);
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
                btn.CustomBorderThickness = new Padding(0);
                btn.CustomBorderColor = Color.Transparent;

                if (btn.CustomizableEdges != null)
                {
                    btn.CustomizableEdges.TopLeft     = true;
                    btn.CustomizableEdges.TopRight    = true;
                    btn.CustomizableEdges.BottomLeft  = true;
                    btn.CustomizableEdges.BottomRight = true;
                }

                btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btn.ImageAlign = HorizontalAlignment.Left;
                btn.ImageOffset = new Point(10, 0);
                btn.TextOffset = new Point(8, 0);

                // Normal Durum
                btn.FillColor = Color.Transparent;
                btn.ForeColor = Color.FromArgb(185, 195, 220);

                // Hover Durum
                btn.HoverState.FillColor = Color.FromArgb(55, 62, 92);
                btn.HoverState.ForeColor = Color.White;
                btn.HoverState.CustomBorderColor = Color.Transparent;

                // Checked Durum (Oval Pembe Kapsül)
                btn.CheckedState.FillColor = Color.FromArgb(241, 85, 126);
                btn.CheckedState.ForeColor = Color.White;
                btn.CheckedState.CustomBorderColor = Color.Transparent;

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
            frmPOS frm = new frmPOS();
            frm.Show();
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
