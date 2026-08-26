using Guna.UI2.WinForms;
using Restourant_Adisyon.Mmodel;
using Restourant_Adisyon.Vview;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon
{
    public partial class formMain : Form
    {
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

            // Aktif Kullanıcı ve Rol gösterimi
            if (lblUser != null)
            {
                lblUser.Text = $"👤 {MainClass.USER} ({MainClass.ROLE})";
                lblUser.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                lblUser.ForeColor = Color.FromArgb(50, 55, 89);
            }

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

        /// <summary>
        /// Sol menü butonlarını oval/dairesel (capsule pill) kapsül formuna dönüştürür,
        /// taşma yapan beyaz dikdörtgen kenarları temizler ve yumuşak animasyonlar ekler.
        /// </summary>
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
                label1.Text = "RESTORAN ADİSYON\nYönetim Sistemi";
                label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                label1.ForeColor = Color.FromArgb(241, 85, 126);
                label1.TextAlign = ContentAlignment.MiddleCenter;
            }

            // Buton listesi ve Türkçe isimleri
            Tuple<Guna2Button, string>[] menuItems = new Tuple<Guna2Button, string>[]
            {
                Tuple.Create(btnHome,       "Ana Sayfa"),
                Tuple.Create(btnCatagories, "Kategoriler"),
                Tuple.Create(btnProducts,   "Ürünler"),
                Tuple.Create(btnTables,     "Masalar"),
                Tuple.Create(btnStaff,      "Personel"),
                Tuple.Create(btnPos,        "POS Satış"),
                Tuple.Create(btnKitchen,    "Mutfak Ekranı"),
                Tuple.Create(btnService,    "Garson Servis"),
                Tuple.Create(btnInventory,   "Stok Takibi"),
                Tuple.Create(btnReports,    "Raporlar"),
                Tuple.Create(btnSettings,   "Ayarlar")
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
                btn.Animated = true; // Guna2 yumuşak renk geçiş animasyonu
                btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                btn.Size = new Size(190, btnHeight);
                btn.Location = new Point(12, currentY);

                // ── OVAL / DAİRESEL KAPSÜL ŞEKİLLENDİRME ─────────────────────────
                btn.AutoRoundedCorners = true; // Tam dairesel/oval kapsül yapar
                btn.UseTransparentBackground = true;
                btn.BorderThickness = 0;
                btn.BorderColor = Color.Transparent;
                btn.CustomBorderThickness = new Padding(0);
                btn.CustomBorderColor = Color.Transparent;

                // Tüm köşeleri serbest bırak (kareleştirme kısıtını kaldır)
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

                // Normal Durum: Tamamen şeffaf arka plan (beyaz kare kutu kalmaz)
                btn.FillColor = Color.Transparent;
                btn.ForeColor = Color.FromArgb(185, 195, 220);

                // Hover (Üzerine Gelince) Durum: Şık oval kapsül vurgusu
                btn.HoverState.FillColor = Color.FromArgb(55, 62, 92);
                btn.HoverState.ForeColor = Color.White;
                btn.HoverState.CustomBorderColor = Color.Transparent;

                // Checked (Seçili/Aktif) Durum: Canlı Pembe/Kırmızı Oval Kapsül
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
