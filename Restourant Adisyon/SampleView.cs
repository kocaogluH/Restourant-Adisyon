using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon
{
    public partial class SampleView : Form
    {
        public SampleView()
        {
            InitializeComponent();
            this.Load += SampleView_Load;
        }

        private void SampleView_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyTheme();
            ApplyLocalization();

            ApplyModernSampleViewStyle();
        }

        public virtual void ApplyTheme()
        {
            this.BackColor = AppTheme.Background;
            if (label2 != null) label2.ForeColor = AppTheme.TextPrimary;
            if (label1 != null) label1.ForeColor = AppTheme.TextSecondary;
            if (txtSearch != null)
            {
                txtSearch.FillColor = AppTheme.CardBackground;
                txtSearch.ForeColor = AppTheme.TextPrimary;
                txtSearch.BorderColor = AppTheme.BorderColor;
            }

            ApplyModernSampleViewStyle();
        }

        public virtual void ApplyLocalization()
        {
            if (txtSearch != null)
                txtSearch.PlaceholderText = LocalizationService.Instance.GetString("Search") ?? "🔍 Arayın...";
            if (label2 != null && !string.IsNullOrEmpty(label2.Text))
                label2.Text = LocalizationService.Instance.GetString(label2.Text);
        }

        protected virtual void ApplyModernSampleViewStyle()
        {
            this.BackColor = AppTheme.Background;

            // 1. Sayfa Başlığı (label2)
            if (label2 != null)
            {
                label2.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                label2.ForeColor = AppTheme.TextPrimary;
                label2.Location = new Point(32, 22);
            }

            // 2. Alt Açıklama / Arama Bilgisi (label1)
            if (label1 != null)
            {
                label1.Font = new Font("Segoe UI", 9.5F);
                label1.ForeColor = AppTheme.TextSecondary;
                label1.Text = "Kayıtları filtrelemek için arama kutusunu kullanın.";
                label1.Location = new Point(34, 56);
            }

            // 3. Arama Çubuğu (txtSearch)
            if (txtSearch != null)
            {
                txtSearch.BorderRadius = 8;
                txtSearch.BorderColor = AppTheme.BorderColor;
                txtSearch.FocusedState.BorderColor = AppTheme.BrandPrimary;
                txtSearch.FillColor = AppTheme.CardBackground;
                txtSearch.ForeColor = AppTheme.TextPrimary;
                txtSearch.Font = new Font("Segoe UI", 10F);
                txtSearch.Height = 40;
                txtSearch.PlaceholderText = "🔍 Listede ara...";
            }

            // 4. Yeni Ekle Butonu (btnAdd)
            if (btnAdd != null)
            {
                btnAdd.Image = null; // Eski pikselli resmi kaldır
                btnAdd.BackColor = Color.Transparent;

                // Dinamik olarak şık bir Guna2Button ekle (eğer henüz eklenmediyse)
                if (this.Controls.Find("btnModernAdd", false).Length == 0)
                {
                    Guna2Button btnModern = new Guna2Button
                    {
                        Name = "btnModernAdd",
                        Text = "➕ Yeni Ekle",
                        Size = new Size(130, 40),
                        Location = new Point(34, 95),
                        BorderRadius = 8,
                        FillColor = AppTheme.BrandPrimary,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        Animated = true
                    };
                    btnModern.HoverState.FillColor = Color.FromArgb(190, 18, 60); // Darker Rose
                    btnModern.Click += (s, ev) => btnAdd_Click(s, ev);
                    this.Controls.Add(btnModern);
                    btnAdd.Visible = false; // Eski butonu gizle
                }
            }

            // 5. Ayraç Çizgisi
            if (guna2Separator1 != null)
            {
                guna2Separator1.FillColor = AppTheme.BorderColor;
                guna2Separator1.Location = new Point(32, 148);
            }
        }

        public virtual void btnAdd_Click(object sender, EventArgs e)
        {
            // Base implementation
        }

        public virtual void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Base implementation
        }

        protected Guna.UI2.WinForms.Guna2TextBox SearchTextBox => txtSearch;
    }
}
