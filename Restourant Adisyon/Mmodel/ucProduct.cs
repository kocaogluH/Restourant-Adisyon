using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon.Mmodel
{
    public partial class ucProduct : UserControl
    {
        private Label _lblPriceBadge;
        private Label _lblCategory;

        public ucProduct()
        {
            InitializeComponent();
            SetupConcept1FoodCard();
        }

        public event EventHandler onSelect = null;

        public int id { get; set; }

        private string _pPrice;
        public string PPrice
        {
            get { return _pPrice; }
            set
            {
                _pPrice = value;
                if (_lblPriceBadge != null)
                {
                    if (decimal.TryParse(value, out decimal val))
                        _lblPriceBadge.Text = val.ToString("N2") + " ₺";
                    else
                        _lblPriceBadge.Text = value + " ₺";
                }
            }
        }

        private string _pCategory;
        public string PCategory
        {
            get { return _pCategory; }
            set
            {
                _pCategory = value;
                if (_lblCategory != null)
                    _lblCategory.Text = value?.ToUpper();
            }
        }

        public string PName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public Image PImage
        {
            get { return txtImage.Image; }
            set
            {
                txtImage.Image = value ?? global::Restourant_Adisyon.Properties.Resources.biryani;
            }
        }

        private void SetupConcept1FoodCard()
        {
            this.Cursor = Cursors.Hand;
            this.BackColor = Color.Transparent;
            this.Size = new Size(196, 215);

            if (guna2ShadowPanel1 != null)
            {
                guna2ShadowPanel1.Size = new Size(190, 205);
                guna2ShadowPanel1.FillColor = AppTheme.CardBackground;
                guna2ShadowPanel1.Radius = 14;
                guna2ShadowPanel1.ShadowColor = Color.FromArgb(20, 20, 35);
                guna2ShadowPanel1.ShadowShift = 4;
            }

            // Yemek Görseli (Büyük, İştah Açıcı, Konsept 1 Tarzı)
            if (txtImage != null)
            {
                txtImage.Size = new Size(166, 110);
                txtImage.Location = new Point(12, 12);
                txtImage.SizeMode = PictureBoxSizeMode.Zoom;
                txtImage.ShadowDecoration.Enabled = false;
            }

            // Kategori Alt Başlığı
            _lblCategory = new Label
            {
                Text = "GENEL",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = AppTheme.TextSecondary,
                Location = new Point(12, 130),
                Size = new Size(166, 14),
                AutoEllipsis = true
            };
            guna2ShadowPanel1.Controls.Add(_lblCategory);
            guna2ShadowPanel1.Controls.SetChildIndex(_lblCategory, 0);

            // Ürün Adı
            if (panel1 != null)
            {
                panel1.Location = new Point(12, 145);
                panel1.Size = new Size(166, 48);
            }

            if (lblName != null)
            {
                lblName.ForeColor = AppTheme.TextPrimary;
                lblName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                lblName.Location = new Point(0, 0);
                lblName.Size = new Size(166, 24);
                lblName.TextAlign = ContentAlignment.MiddleLeft;
                lblName.AutoEllipsis = true;
            }

            // Fiyat Rozeti (Sağ üst köşe parlayan Konsept 1 stili)
            _lblPriceBadge = new Label
            {
                Text = "0.00 ₺",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = AppTheme.BrandPrimary,
                AutoSize = true,
                Padding = new Padding(6, 3, 6, 3),
                Location = new Point(102, 16)
            };
            guna2ShadowPanel1.Controls.Add(_lblPriceBadge);
            guna2ShadowPanel1.Controls.SetChildIndex(_lblPriceBadge, 0);

            if (guna2Separator1 != null)
                guna2Separator1.Visible = false;

            // Kartın her noktasına dokunarak sepete ekleme
            this.Click += TriggerSelect;
            if (guna2ShadowPanel1 != null) guna2ShadowPanel1.Click += TriggerSelect;
            if (panel1 != null) panel1.Click += TriggerSelect;
            if (lblName != null) lblName.Click += TriggerSelect;
            if (_lblCategory != null) _lblCategory.Click += TriggerSelect;
            if (_lblPriceBadge != null) _lblPriceBadge.Click += TriggerSelect;
            if (txtImage != null) txtImage.Click += TriggerSelect;
        }

        private void TriggerSelect(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
        }

        private void txtImage_Click(object sender, EventArgs e)
        {
            TriggerSelect(this, e);
        }
    }
}
