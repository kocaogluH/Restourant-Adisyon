using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon
{
    public partial class SampleAdd : Form
    {
        public SampleAdd()
        {
            InitializeComponent();
            this.Load += SampleAdd_Load;
        }

        private void SampleAdd_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyTheme();
            ApplyLocalization();

            ApplyModernSampleAddStyle();
        }

        public virtual void ApplyTheme()
        {
            this.BackColor = AppTheme.CardBackground;
            this.ForeColor = AppTheme.TextPrimary;
            if (label1 != null) label1.ForeColor = Color.White;

            ApplyModernSampleAddStyle();
        }

        public virtual void ApplyLocalization()
        {
            if (btnSave != null)
                btnSave.Text = LocalizationService.Instance.GetString("Save");
            if (btnClose != null)
                btnClose.Text = LocalizationService.Instance.GetString("Close");
        }

        protected virtual void ApplyModernSampleAddStyle()
        {
            // Form Temel Özellikleri
            this.BackColor = AppTheme.Background;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // 1. Üst Başlık Paneli (guna2Panel1)
            if (guna2Panel1 != null)
            {
                guna2Panel1.FillColor = AppTheme.SidebarBackground;
                guna2Panel1.BackColor = AppTheme.SidebarBackground;
                guna2Panel1.Height = 75;

                // Eski pikselli ikon gizlenip minimalist başlık oluşturulur
                if (pictureBox1 != null)
                {
                    pictureBox1.Visible = false;
                }

                if (label1 != null)
                {
                    label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                    label1.ForeColor = Color.White;
                    label1.Location = new Point(24, 25);
                }

                // Sağ üst köşeye modern kapatma butonu
                if (guna2Panel1.Controls.Find("modernCloseBox", false).Length == 0)
                {
                    Guna2ControlBox ctrlClose = new Guna2ControlBox
                    {
                        Name = "modernCloseBox",
                        Anchor = AnchorStyles.Top | AnchorStyles.Right,
                        Location = new Point(guna2Panel1.Width - 45, 15),
                        Size = new Size(32, 32),
                        FillColor = Color.Transparent,
                        IconColor = Color.White,
                        Cursor = Cursors.Hand
                    };
                    ctrlClose.HoverState.FillColor = Color.FromArgb(225, 29, 72); // Rose Red
                    guna2Panel1.Controls.Add(ctrlClose);
                }
            }

            // 2. Alt İşlem Paneli (guna2Panel2)
            if (guna2Panel2 != null)
            {
                guna2Panel2.FillColor = AppTheme.CardBackground;
                guna2Panel2.Height = 70;

                // Butonları modernize et
                if (btnSave != null)
                {
                    btnSave.Size = new Size(130, 42);
                    btnSave.Location = new Point(24, 14);
                    btnSave.BackColor = Color.Transparent;
                    btnSave.FlatStyle = FlatStyle.Flat;
                    btnSave.FlatAppearance.BorderSize = 0;
                    AppTheme.StyleButton(btnSave, ButtonRole.Success);
                    btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    btnSave.Cursor = Cursors.Hand;
                }

                if (btnClose != null)
                {
                    btnClose.Size = new Size(110, 42);
                    btnClose.Location = new Point(165, 14);
                    btnClose.BackColor = Color.Transparent;
                    btnClose.FlatStyle = FlatStyle.Flat;
                    btnClose.FlatAppearance.BorderSize = 0;
                    AppTheme.StyleButton(btnClose, ButtonRole.Secondary);
                    btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    btnClose.Cursor = Cursors.Hand;
                }
            }

            // 3. İç Kontrolleri Otomatik Stilize Et
            StyleChildControls(this);
        }

        private void StyleChildControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c == guna2Panel1 || c == guna2Panel2) continue;

                if (c is Guna2TextBox tb)
                {
                    tb.BorderRadius = 8;
                    tb.BorderColor = AppTheme.BorderColor;
                    tb.FocusedState.BorderColor = AppTheme.BrandPrimary;
                    tb.FillColor = AppTheme.CardBackground;
                    tb.ForeColor = AppTheme.TextPrimary;
                    tb.Font = new Font("Segoe UI", 10F);
                }
                else if (c is Guna2ComboBox cb)
                {
                    cb.BorderRadius = 8;
                    cb.BorderColor = AppTheme.BorderColor;
                    cb.FillColor = AppTheme.CardBackground;
                    cb.ForeColor = AppTheme.TextPrimary;
                    cb.Font = new Font("Segoe UI", 10F);
                }
                else if (c is Label lbl && lbl != label1)
                {
                    lbl.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    lbl.ForeColor = AppTheme.TextSecondary;
                }
                else if (c is Guna2Button btn)
                {
                    btn.BorderRadius = 8;
                    btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    btn.Cursor = Cursors.Hand;
                }

                if (c.HasChildren)
                {
                    StyleChildControls(c);
                }
            }
        }

        public virtual void btnSave_Click(object sender, EventArgs e)
        {
        }

        public virtual void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
