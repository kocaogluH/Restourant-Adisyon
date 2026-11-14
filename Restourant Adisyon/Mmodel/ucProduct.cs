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

        public ucProduct()
        {
            InitializeComponent();
            SetupModernCard();
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
                    _lblPriceBadge.Text = value + " ₺";
                }
            }
        }

        public string PCategory { get; set; }

        public string PName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public Image PImage
        {
            get { return txtImage.Image; }
            set { txtImage.Image = value; }
        }

        private void SetupModernCard()
        {
            this.Cursor = Cursors.Hand;
            this.BackColor = Color.Transparent;

            if (guna2ShadowPanel1 != null)
            {
                guna2ShadowPanel1.FillColor = AppTheme.CardBackground;
                guna2ShadowPanel1.Radius = 12;
                guna2ShadowPanel1.ShadowColor = Color.FromArgb(10, 10, 20);
                guna2ShadowPanel1.ShadowShift = 3;
            }

            if (lblName != null)
            {
                lblName.ForeColor = AppTheme.TextPrimary;
                lblName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }

            // Fiyat Rozeti (Sağ üst köşe)
            _lblPriceBadge = new Label
            {
                Text = "0 ₺",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = AppTheme.BrandPrimary,
                AutoSize = true,
                Padding = new Padding(6, 3, 6, 3),
                Location = new Point(115, 12)
            };
            guna2ShadowPanel1.Controls.Add(_lblPriceBadge);
            guna2ShadowPanel1.Controls.SetChildIndex(_lblPriceBadge, 0);

            // Tüm kart bileşenlerine tıklama bağla
            this.Click += TriggerSelect;
            if (guna2ShadowPanel1 != null) guna2ShadowPanel1.Click += TriggerSelect;
            if (panel1 != null) panel1.Click += TriggerSelect;
            if (lblName != null) lblName.Click += TriggerSelect;
            if (_lblPriceBadge != null) _lblPriceBadge.Click += TriggerSelect;
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
