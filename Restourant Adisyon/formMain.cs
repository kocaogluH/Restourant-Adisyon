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

            // Sol menü tasarımını ve animasyonlarını uygula
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
        /// Sol menü butonlarına modern görünüm, yumuşak geçiş animasyonları (Animated=true)
        /// ve aktif/hover renk stilleri uygular.
        /// </summary>
        private void ApplyModernSidebarStyle()
        {
            // Sol panel rengi (Modern koyu lacivert/indigo)
            guna2Panel1.FillColor = Color.FromArgb(36, 40, 62);
            guna2Panel1.Width = 210;

            // Üst başlık paneli rengi
            if (guna2CustomGradientPanel1 != null)
            {
                guna2CustomGradientPanel1.FillColor  = Color.FromArgb(245, 247, 250);
                guna2CustomGradientPanel1.FillColor2 = Color.FromArgb(245, 247, 250);
                guna2CustomGradientPanel1.FillColor3 = Color.FromArgb(245, 247, 250);
                guna2CustomGradientPanel1.FillColor4 = Color.FromArgb(245, 247, 250);
            }

            // Çakışan ekstra resim kutularını gizle (daha temiz görünüm)
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

            // Başlık yazısını Türkçeleştir ve şıklaştır
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

            int startY = 160;
            int btnHeight = 42;
            int spacing = 5;
            int currentY = startY;

            foreach (var item in menuItems)
            {
                var btn = item.Item1;
                var title = item.Item2;
                if (btn == null) continue;

                btn.Text = "  " + title;
                btn.Animated = true; // Guna2 donanım hızlandırmalı yumuşak animasyon
                btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                btn.Size = new Size(190, btnHeight);
                btn.Location = new Point(10, currentY);
                btn.BorderRadius = 8;
                btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btn.ImageAlign = HorizontalAlignment.Left;
                btn.ImageOffset = new Point(8, 0);
                btn.TextOffset = new Point(8, 0);

                // Normal Durum
                btn.FillColor = Color.Transparent;
                btn.ForeColor = Color.FromArgb(175, 185, 210);

                // Üzerine Gelince (Hover) Animasyon Rengi
                btn.HoverState.FillColor = Color.FromArgb(52, 58, 88);
                btn.HoverState.ForeColor = Color.White;

                // Seçili / Aktif Durum (Checked) Rengi
                btn.CheckedState.FillColor = Color.FromArgb(241, 85, 126); // Canlı Pembe/Kırmızı Accent
                btn.CheckedState.ForeColor = Color.White;

                currentY += btnHeight + spacing;
            }
        }

        /// <summary>
        /// Personel girişi yapıldığında gizlenen menü öğelerinden sonra
        /// kalan butonları düzgün aralıklarla yeniden hizalar.
        /// </summary>
        private void ReorderVisibleButtons()
        {
            Guna2Button[] navButtons = new Guna2Button[]
            {
                btnHome, btnCatagories, btnProducts, btnTables, btnStaff,
                btnPos, btnKitchen, btnService, btnInventory, btnReports, btnSettings
            };

            int startY = 160;
            int btnHeight = 42;
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
