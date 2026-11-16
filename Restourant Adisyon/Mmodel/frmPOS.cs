using System;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.Core.DTOs;
using Restourant_Adisyon.Core.Entities;
using Restourant_Adisyon.UI.Controls;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        public int    MainID = 0;
        public string OrderType = "Masada";

        // Barkod tamponu (klavye ile hızlı giriş algılar)
        private string _barcodeBuffer = "";
        private DateTime _lastKeyTime = DateTime.Now;

        private FlowLayoutPanel _flpQuickCash;
        private Label _lblSubtotal;
        private Label _lblTax;

        // Modern UI Rozetleri
        private Label _lblBrand;
        private Label _lblBrandSub;
        private Guna2Button _badgeTable;
        private Guna2Button _badgeWaiter;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            if (lblTable != null)
            {
                if (lblTable.Text == "Table" || string.IsNullOrWhiteSpace(lblTable.Text))
                    lblTable.Text = "";
                lblTable.Visible = false;
            }
            if (lblWaiter != null)
            {
                if (lblWaiter.Text == "Waiter" || string.IsNullOrWhiteSpace(lblWaiter.Text))
                    lblWaiter.Text = "";
                lblWaiter.Visible = false;
            }

            SetupModernHeader();
            ApplyThemeAndStyling();

            this.Resize += (s, ev) => LayoutPosHeader();
            if (guna2Panel1 != null)
                guna2Panel1.Resize += (s, ev) => LayoutPosHeader();

            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyThemeAndStyling();

            ApplyLocalization();
            SetupReceiptPanel();

            ProductPanel.Controls.Clear();
            LoadProducts();

            // Barkod desteği: form-level KeyPreview etkinleştir
            this.KeyPreview = true;

            MainClass.EnableEnterKeyConfirmation(
                this,
                () =>
                {
                    if (txtSearch != null && (txtSearch.Focused || this.ActiveControl == txtSearch))
                    {
                        string q = txtSearch.Text.Trim();
                        if (!string.IsNullOrEmpty(q))
                        {
                            var byBarcode = PosManager.Instance.FindProductByBarcode(q);
                            if (byBarcode != null)
                            {
                                AddToCart(byBarcode.Id, byBarcode.Name, byBarcode.Price.ToString("0.00"));
                                txtSearch.Clear();
                                return;
                            }
                            foreach (Control c in ProductPanel.Controls)
                            {
                                if (c is ucProduct prod && prod.Visible)
                                {
                                    AddToCart(prod.id, prod.PName, prod.PPrice);
                                    return;
                                }
                            }
                        }
                    }
                    else if (guna2DataGridView1 != null && guna2DataGridView1.Rows.Count > 0)
                    {
                        btnCheckout_Click(btnCheckout, EventArgs.Empty);
                    }
                }
            );
        }

        private void SetupModernHeader()
        {
            if (guna2Panel1 == null) return;

            guna2Panel1.FillColor = AppTheme.SidebarBackground;
            guna2Panel1.BackColor = AppTheme.SidebarBackground;
            guna2Panel1.Height = 82;

            // Eski pikselli resmi ve labelleri gizle
            if (pictureBox1 != null) pictureBox1.Visible = false;
            if (lblTable != null) lblTable.Visible = false;
            if (lblWaiter != null) lblWaiter.Visible = false;

            // 1. Marka Başlığı
            if (_lblBrand == null)
            {
                _lblBrand = new Label
                {
                    Text = "⚡ RESTO POS",
                    Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(16, 16),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                guna2Panel1.Controls.Add(_lblBrand);

                _lblBrandSub = new Label
                {
                    Text = "POS & Satış Terminali",
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    Location = new Point(18, 44),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                guna2Panel1.Controls.Add(_lblBrandSub);
            }

            // 2. Masa ve Garson Rozetleri (Dinamik Gösterim)
            if (_badgeTable == null)
            {
                _badgeTable = new Guna2Button
                {
                    Text = "🪑 Masa Seç",
                    Size = new Size(115, 36),
                    BorderRadius = 18,
                    FillColor = Color.FromArgb(30, 41, 59),
                    ForeColor = Color.FromArgb(226, 232, 240),
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                _badgeTable.Click += (s, e) => btnDin_Click(s, e);
                guna2Panel1.Controls.Add(_badgeTable);

                _badgeWaiter = new Guna2Button
                {
                    Text = "👤 Garson Seç",
                    Size = new Size(115, 36),
                    BorderRadius = 18,
                    FillColor = Color.FromArgb(30, 41, 59),
                    ForeColor = Color.FromArgb(226, 232, 240),
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                _badgeWaiter.Click += (s, e) =>
                {
                    frmWaiterSelect frmW = new frmWaiterSelect();
                    MainClass.BlurBackground(frmW);
                    if (!string.IsNullOrEmpty(frmW.waiterName))
                    {
                        lblWaiter.Text = frmW.waiterName;
                        UpdateBadges();
                    }
                };
                guna2Panel1.Controls.Add(_badgeWaiter);
            }

            // 3. Servis Tipi Butonları
            StyleServiceButton(btnDin, "🍽️ Masada");
            StyleServiceButton(btnTake, "🥡 Gel Al");
            StyleServiceButton(btnDelivery, "🛵 Paket");

            // 4. İşlem Butonları (New, Bill, KOT, Checkout)
            StyleActionButton(btnNew, "➕ Yeni", Color.FromArgb(51, 65, 85), Color.White);
            StyleActionButton(btnBill, "📋 Adisyonlar", Color.FromArgb(71, 85, 105), Color.White);
            StyleActionButton(btnKot, "🍳 Mutfak (KOT)", AppTheme.BrandPrimary, Color.White);
            StyleActionButton(btnCheckout, "💳 Ödeme Al", AppTheme.Success, Color.White);

            // Eski çakışan daire güç butonunu gizle
            if (btnExit != null)
            {
                btnExit.Visible = false;
            }

            LayoutPosHeader();
            UpdateServiceTypeButtons();
            UpdateBadges();
        }

        private void LayoutPosHeader()
        {
            if (guna2Panel1 == null) return;

            // 1. Sağdaki İşlem Butonları (Sağ kenara hizalı dinamik yerleşim - Asla Ekrandan Taşamaz)
            int rightEdge = guna2Panel1.Width - 16;

            if (btnCheckout != null)
            {
                btnCheckout.Size = new Size(106, 38);
                rightEdge -= btnCheckout.Width;
                btnCheckout.Location = new Point(rightEdge, 22);
                rightEdge -= 8;
            }

            if (btnKot != null)
            {
                btnKot.Size = new Size(110, 38);
                rightEdge -= btnKot.Width;
                btnKot.Location = new Point(rightEdge, 22);
                rightEdge -= 8;
            }

            if (btnBill != null)
            {
                btnBill.Size = new Size(96, 38);
                rightEdge -= btnBill.Width;
                btnBill.Location = new Point(rightEdge, 22);
                rightEdge -= 8;
            }

            if (btnNew != null)
            {
                btnNew.Size = new Size(76, 38);
                rightEdge -= btnNew.Width;
                btnNew.Location = new Point(rightEdge, 22);
            }

            // 2. Soldaki Rozetler ve Servis Seçenekleri (Markadan ve alt yazıdan sonra dinamik başlar)
            int brandRight = 155;
            if (_lblBrand != null) brandRight = Math.Max(brandRight, _lblBrand.Right);
            if (_lblBrandSub != null) brandRight = Math.Max(brandRight, _lblBrandSub.Right);
            int leftEdge = brandRight + 18;

            if (_badgeTable != null)
            {
                _badgeTable.Size = new Size(115, 36);
                _badgeTable.Location = new Point(leftEdge, 23);
                leftEdge += _badgeTable.Width + 8;
            }

            if (_badgeWaiter != null)
            {
                _badgeWaiter.Size = new Size(115, 36);
                _badgeWaiter.Location = new Point(leftEdge, 23);
                leftEdge += _badgeWaiter.Width + 12;
            }

            if (btnDin != null)
            {
                btnDin.Size = new Size(82, 38);
                btnDin.Location = new Point(leftEdge, 22);
                leftEdge += btnDin.Width + 6;
            }

            if (btnTake != null)
            {
                btnTake.Size = new Size(78, 38);
                btnTake.Location = new Point(leftEdge, 22);
                leftEdge += btnTake.Width + 6;
            }

            if (btnDelivery != null)
            {
                btnDelivery.Size = new Size(78, 38);
                btnDelivery.Location = new Point(leftEdge, 22);
            }
        }

        private void StyleServiceButton(Button btn, string text)
        {
            if (btn == null) return;
            btn.Text = text;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void StyleActionButton(Button btn, string text, Color bgColor, Color fgColor)
        {
            if (btn == null) return;
            btn.Text = text;
            btn.BackColor = bgColor;
            btn.ForeColor = fgColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void UpdateServiceTypeButtons()
        {
            Color activeBg = AppTheme.BrandPrimary; // Rose Red
            Color inactiveBg = Color.FromArgb(51, 65, 85); // Slate 700

            if (btnDin != null)      btnDin.BackColor      = OrderType == "Masada" ? activeBg : inactiveBg;
            if (btnTake != null)     btnTake.BackColor     = OrderType == "Gel-Al" || OrderType == "Gel Al" ? activeBg : inactiveBg;
            if (btnDelivery != null) btnDelivery.BackColor = OrderType == "Paket" ? activeBg : inactiveBg;
        }

        private void UpdateBadges()
        {
            bool hasTable = !string.IsNullOrWhiteSpace(lblTable?.Text) && lblTable.Text != "Table";
            if (_badgeTable != null)
            {
                if (hasTable)
                {
                    _badgeTable.Text = "🪑 " + lblTable.Text;
                    _badgeTable.FillColor = Color.FromArgb(16, 185, 129); // Emerald
                    _badgeTable.ForeColor = Color.White;
                }
                else
                {
                    _badgeTable.Text = "🪑 Masa Seç";
                    _badgeTable.FillColor = Color.FromArgb(30, 41, 59);
                    _badgeTable.ForeColor = Color.FromArgb(203, 213, 225);
                }
            }

            bool hasWaiter = !string.IsNullOrWhiteSpace(lblWaiter?.Text) && lblWaiter.Text != "Waiter";
            if (_badgeWaiter != null)
            {
                if (hasWaiter)
                {
                    _badgeWaiter.Text = "👤 " + lblWaiter.Text;
                    _badgeWaiter.FillColor = Color.FromArgb(37, 99, 235); // Blue
                    _badgeWaiter.ForeColor = Color.White;
                }
                else
                {
                    _badgeWaiter.Text = "👤 Garson Seç";
                    _badgeWaiter.FillColor = Color.FromArgb(30, 41, 59);
                    _badgeWaiter.ForeColor = Color.FromArgb(203, 213, 225);
                }
            }
        }

        private void ApplyThemeAndStyling()
        {
            this.BackColor = AppTheme.Background;
            if (guna2Panel1 != null)
            {
                guna2Panel1.FillColor = AppTheme.SidebarBackground;
                guna2Panel1.BackColor = AppTheme.SidebarBackground;
            }
            if (guna2Panel2 != null)
            {
                guna2Panel2.FillColor = AppTheme.CardBackground;
                guna2Panel2.BackColor = AppTheme.CardBackground;
                guna2Panel2.BorderColor = AppTheme.BorderColor;
                guna2Panel2.BorderThickness = 1;
            }

            if (guna2DataGridView1 != null)
                GridStyler.Apply(guna2DataGridView1, "Sepetiniz boş. Menüden ürün seçin veya barkod okutun.");

            if (lblTotal != null)
                lblTotal.ForeColor = AppTheme.BrandPrimary;

            if (txtSearch != null)
            {
                txtSearch.FillColor = AppTheme.CardBackground;
                txtSearch.ForeColor = AppTheme.TextPrimary;
                txtSearch.BorderColor = AppTheme.BorderColor;
                txtSearch.BorderRadius = 8;
                txtSearch.PlaceholderText = "🔍 Ürün ara veya barkod okut...";
            }

            UpdateServiceTypeButtons();
            UpdateBadges();
        }

        private void SetupReceiptPanel()
        {
            if (guna2Panel2 == null || _flpQuickCash != null) return;

            // Fiş döküm etiketleri (Ara Toplam & KDV)
            _lblSubtotal = new Label
            {
                Text = "Ara Toplam: 0.00 ₺",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = AppTheme.TextSecondary,
                Location = new Point(14, 18),
                AutoSize = true
            };
            guna2Panel2.Controls.Add(_lblSubtotal);

            _lblTax = new Label
            {
                Text = "KDV (%10): 0.00 ₺",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = AppTheme.TextSecondary,
                Location = new Point(14, 38),
                AutoSize = true
            };
            guna2Panel2.Controls.Add(_lblTax);

            // Hızlı Nakit Tuşları (50₺, 100₺, 200₺, 500₺, Tam Tutar)
            _flpQuickCash = new FlowLayoutPanel
            {
                Size = new Size(guna2Panel2.Width - 28, 42),
                Location = new Point(14, 62),
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent
            };
            guna2Panel2.Controls.Add(_flpQuickCash);

            int[] cashAmounts = new int[] { 50, 100, 200, 500 };
            foreach (int amt in cashAmounts)
            {
                Guna2Button btnCash = new Guna2Button
                {
                    Text = amt + " ₺",
                    Size = new Size(55, 32),
                    Margin = new Padding(0, 0, 5, 0),
                    AutoRoundedCorners = true,
                    FillColor = AppTheme.Secondary,
                    ForeColor = AppTheme.TextPrimary,
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnCash.Click += (s, e) =>
                {
                    double total = double.TryParse(lblTotal.Text, out double t) ? t : 0;
                    double change = amt - total;
                    if (change >= 0)
                        guna2MessageDialog1.Show($"Nakit Alındı: {amt:N2} ₺\nPara Üstü: {change:N2} ₺");
                    else
                        guna2MessageDialog1.Show($"Eksik Tutar: {Math.Abs(change):N2} ₺");
                };
                _flpQuickCash.Controls.Add(btnCash);
            }

            // Tam Tutar Butonu
            Guna2Button btnExact = new Guna2Button
            {
                Text = "Tam",
                Size = new Size(52, 32),
                AutoRoundedCorners = true,
                FillColor = AppTheme.Success,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                UseTransparentBackground = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExact.Click += (s, e) =>
            {
                guna2MessageDialog1.Show($"Tam Tutar Alındı ({lblTotal.Text} ₺). Para Üstü: 0.00 ₺");
            };
            _flpQuickCash.Controls.Add(btnExact);
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (txtSearch != null) txtSearch.PlaceholderText = loc.GetString("Search") ?? "🔍 Ürün ara veya barkod okut...";
            AddCategory();
        }

        // ── Kategori Hap Butonları ───────────────────────────────────────────────
        private void AddCategory()
        {
            CategoryPanel.Controls.Clear();

            string allText = LocalizationService.Instance.GetString("All_Categories") ?? "🌟 Tümü";
            Guna2Button b2 = new Guna2Button
            {
                FillColor = AppTheme.Secondary,
                Size = new Size(130, 40),
                ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                Text = allText,
                AutoRoundedCorners = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                BackColor = Color.Transparent,
                UseTransparentBackground = true,
                Checked = true,
                Margin = new Padding(0, 0, 8, 0),
                Cursor = Cursors.Hand
            };
            b2.CheckedState.FillColor = AppTheme.BrandPrimary;
            b2.CheckedState.ForeColor = Color.White;
            b2.Click += new EventHandler(b_click);
            CategoryPanel.Controls.Add(b2);

            var categories = PosManager.Instance.GetCategories();
            foreach (var cat in categories)
            {
                Guna2Button b = new Guna2Button
                {
                    FillColor = AppTheme.Secondary,
                    Size = new Size(140, 40),
                    ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                    Text = cat.Name,
                    AutoRoundedCorners = true,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = AppTheme.TextPrimary,
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
                    Margin = new Padding(0, 0, 8, 0),
                    Cursor = Cursors.Hand
                };
                b.CheckedState.FillColor = AppTheme.BrandPrimary;
                b.CheckedState.ForeColor = Color.White;
                b.Click += new EventHandler(b_click);
                CategoryPanel.Controls.Add(b);
            }
        }

        private void b_click(object sender, EventArgs e)
        {
            string allText = LocalizationService.Instance.GetString("All_Categories") ?? "🌟 Tümü";
            Guna2Button b = (Guna2Button)sender;
            if (b.Text == allText || b.Text == "Tümü" || b.Text == "All" || b.Text.Contains("Tümü"))
            {
                foreach (var item in ProductPanel.Controls)
                    ((ucProduct)item).Visible = true;
                return;
            }
            foreach (var tem in ProductPanel.Controls)
            {
                var pro = (ucProduct)tem;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        // ── Ürün Yükleme (Konsept 1 İştah Açıcı Yemek Kartları) ───────────────────
        private void LoadProducts()
        {
            var products = PosManager.Instance.GetProducts();

            foreach (var p in products)
            {
                AddItem(p.Id.ToString(), p.Name, p.CategoryName, p.Price.ToString("0.00"), p.Barcode, p.ProductImage);
            }
        }

        private void AddItem(string proID, string name, string cat, string price, string barcode, Image pimage)
        {
            var w = new ucProduct()
            {
                PName     = name,
                PPrice    = price,
                PCategory = cat,
                PImage    = pimage,
                id        = Convert.ToInt32(proID)
            };

            ProductPanel.Controls.Add(w);
            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;
                AddToCart(wdg.id, wdg.PName, wdg.PPrice);
            };
        }

        // ── Sepete Ekleme ────────────────────────────────────────────────────────
        private void AddToCart(int proID, string proName, string proPrice)
        {
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                if (Convert.ToInt32(item.Cells["dgvproID"].Value) == proID)
                {
                    int currentQty = Convert.ToInt32(item.Cells["dgvQty"].Value);
                    currentQty++;
                    item.Cells["dgvQty"].Value    = currentQty;
                    item.Cells["dgvAmount"].Value = currentQty * double.Parse(item.Cells["dgvPrice"].Value.ToString());
                    GetTotal();
                    return;
                }
            }
            guna2DataGridView1.Rows.Add(new object[] { 0, 0, proID, proName, 1, proPrice, proPrice });
            GetTotal();
        }

        // ── Barkod Okuyucu Algılama ──────────────────────────────────────────────
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (txtSearch.Focused) return;

            double elapsed = (DateTime.Now - _lastKeyTime).TotalMilliseconds;
            _lastKeyTime = DateTime.Now;

            if (elapsed < 80)
                _barcodeBuffer += e.KeyChar;
            else
                _barcodeBuffer = e.KeyChar.ToString();

            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                string barcode = _barcodeBuffer.Trim().Replace("\r", "").Replace("\n", "");
                _barcodeBuffer = "";
                if (!string.IsNullOrEmpty(barcode))
                    ProcessBarcode(barcode);
            }
        }

        private void ProcessBarcode(string barcode)
        {
            var p = PosManager.Instance.FindProductByBarcode(barcode);
            if (p != null)
            {
                AddToCart(p.Id, p.Name, p.Price.ToString("0.00"));
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                guna2MessageDialog1.Show("Ürün bulunamadı: " + barcode);
            }
        }

        // ── Arama ───────────────────────────────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        // ── Satır biçimlendirme & Toplam ────────────────────────────────────────
        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void GetTotal()
        {
            double tot = 0;
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                if (item.Cells["dgvAmount"].Value != null)
                    tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }
            lblTotal.Text = tot.ToString("N2");

            // KDV ve Ara Toplamı güncelle
            double subtotal = tot / 1.10;
            double tax = tot - subtotal;
            if (_lblSubtotal != null) _lblSubtotal.Text = $"Ara Toplam: {subtotal:N2} ₺";
            if (_lblTax != null)      _lblTax.Text      = $"KDV (%10): {tax:N2} ₺";
        }

        // ── Dokunmatik Adet Arttır/Azalt & Sil ──────────────────────────────────
        private void guna2DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && guna2DataGridView1.CurrentRow != null)
            {
                guna2DataGridView1.Rows.RemoveAt(guna2DataGridView1.CurrentRow.Index);
                GetTotal();
            }
            else if (e.KeyCode == Keys.Add && guna2DataGridView1.CurrentRow != null)
            {
                int qty = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvQty"].Value) + 1;
                guna2DataGridView1.CurrentRow.Cells["dgvQty"].Value = qty;
                guna2DataGridView1.CurrentRow.Cells["dgvAmount"].Value = qty * double.Parse(guna2DataGridView1.CurrentRow.Cells["dgvPrice"].Value.ToString());
                GetTotal();
            }
            else if (e.KeyCode == Keys.Subtract && guna2DataGridView1.CurrentRow != null)
            {
                int qty = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvQty"].Value) - 1;
                if (qty <= 0)
                    guna2DataGridView1.Rows.RemoveAt(guna2DataGridView1.CurrentRow.Index);
                else
                {
                    guna2DataGridView1.CurrentRow.Cells["dgvQty"].Value = qty;
                    guna2DataGridView1.CurrentRow.Cells["dgvAmount"].Value = qty * double.Parse(guna2DataGridView1.CurrentRow.Cells["dgvPrice"].Value.ToString());
                }
                GetTotal();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";  lblWaiter.Text = "";
            lblTable.Visible = false; lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "0.00";
            OrderType = "Masada";
            UpdateServiceTypeButtons();
            UpdateBadges();
            GetTotal();
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTable.Visible = false; lblWaiter.Visible = false;
            OrderType = "Paket";
            UpdateServiceTypeButtons();
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            lblTable.Visible = false; lblWaiter.Visible = false;
            OrderType = "Gel-Al";
            UpdateServiceTypeButtons();
        }

        private void btnDin_Click(object sender, EventArgs e)
        {
            OrderType = "Masada";
            UpdateServiceTypeButtons();

            frmTableSelect frm = new frmTableSelect();
            MainClass.BlurBackground(frm);
            lblTable.Text    = frm.TableName ?? "";
            lblTable.Visible = false;

            if (!string.IsNullOrEmpty(frm.TableName))
            {
                frmWaiterSelect frm2 = new frmWaiterSelect();
                MainClass.BlurBackground(frm2);
                lblWaiter.Text    = frm2.waiterName ?? "";
                lblWaiter.Visible = false;
            }

            UpdateBadges();
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(OrderType))
            {
                guna2MessageDialog1.Show("Lütfen sipariş tipini seçin (Masada/Paket/Gel-Al).");
                return;
            }
            if (guna2DataGridView1.Rows.Count == 0)
            {
                guna2MessageDialog1.Show("Lütfen önce ürün ekleyin.");
                return;
            }
            if (string.IsNullOrEmpty(lblTotal.Text) || lblTotal.Text == "0.00")
            {
                guna2MessageDialog1.Show("Toplam tutar 0 olamaz.");
                return;
            }

            try
            {
                using (var con = new System.Data.SQLite.SQLiteConnection(MainClass.con_string))
                {
                    con.Open();
                    using (var tr = con.BeginTransaction())
                    {
                        try
                        {
                            if (MainID == 0)
                            {
                                string qry1 = @"INSERT INTO tblMain (aDate,aTime,TableName,WaiterName,status,orderType,total,received,change)
                                                VALUES (@aDate,@aTime,@TableName,@WaiterName,@status,@orderType,@total,0,0);
                                                SELECT last_insert_rowid();";
                                using (var cmd = new System.Data.SQLite.SQLiteCommand(qry1, con, tr))
                                {
                                    cmd.Parameters.AddWithValue("@aDate",      DateTime.Now.ToString("yyyy-MM-dd"));
                                    cmd.Parameters.AddWithValue("@aTime",      DateTime.Now.ToString("HH:mm"));
                                    cmd.Parameters.AddWithValue("@TableName",  lblTable.Text);
                                    cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text);
                                    cmd.Parameters.AddWithValue("@status",     "Pending");
                                    cmd.Parameters.AddWithValue("@orderType",  OrderType);
                                    cmd.Parameters.AddWithValue("@total",      Convert.ToDouble(lblTotal.Text));
                                    MainID = Convert.ToInt32(cmd.ExecuteScalar());
                                }
                            }
                            else
                            {
                                string qry1 = @"UPDATE tblMain SET status='Pending', total=@total WHERE MainID=@ID";
                                using (var cmd = new System.Data.SQLite.SQLiteCommand(qry1, con, tr))
                                {
                                    cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text));
                                    cmd.Parameters.AddWithValue("@ID",    MainID);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                            {
                                int detailID = Convert.ToInt32(row.Cells["dgvid"].Value);
                                string qry2;
                                if (detailID == 0)
                                    qry2 = "INSERT INTO tblDetails (MainID,proID,qty,price,amount) VALUES (@MainID,@proID,@qty,@price,@amount)";
                                else
                                    qry2 = "UPDATE tblDetails SET qty=@qty,price=@price,amount=@amount WHERE DetailID=@ID";

                                using (var cmd2 = new System.Data.SQLite.SQLiteCommand(qry2, con, tr))
                                {
                                    cmd2.Parameters.AddWithValue("@ID",     detailID);
                                    cmd2.Parameters.AddWithValue("@MainID", MainID);
                                    cmd2.Parameters.AddWithValue("@proID",  Convert.ToInt32(row.Cells["dgvproID"].Value));
                                    cmd2.Parameters.AddWithValue("@qty",    Convert.ToInt32(row.Cells["dgvQty"].Value));
                                    cmd2.Parameters.AddWithValue("@price",  Convert.ToDouble(row.Cells["dgvPrice"].Value));
                                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));
                                    cmd2.ExecuteNonQuery();
                                }

                                using (var cmdStock = new System.Data.SQLite.SQLiteCommand(
                                    @"UPDATE tblMaterials SET mQty = mQty - (r.qtyNeeded * @qty)
                                      FROM tblMaterials m
                                      INNER JOIN tblRecipe r ON m.mID = r.mID
                                      WHERE r.proID = @proID", con, tr))
                                {
                                    cmdStock.Parameters.AddWithValue("@qty",   Convert.ToInt32(row.Cells["dgvQty"].Value));
                                    cmdStock.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                                    try { cmdStock.ExecuteNonQuery(); } catch { /* tblRecipe yoksa atla */ }
                                }
                            }

                            tr.Commit();
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            throw ex;
                        }
                    }
                }

                guna2MessageDialog1.Show("Sipariş mutfağa gönderildi!");
                btnNew_Click(null, null);
            }
            catch (Exception ex)
            {
                MainClass.LogError("frmPOS.btnKot_Click", ex);
                MessageBox.Show("Sipariş kaydedilemedi: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frm = new frmBillList();
            MainClass.BlurBackground(frm);

            if (frm.MainID > 0)
            {
                MainID = frm.MainID;
                LoadEntries();
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (MainID == 0)
            {
                if (guna2DataGridView1.Rows.Count == 0)
                {
                    guna2MessageDialog1.Show("Lütfen önce sepete ürün ekleyin.");
                    return;
                }
                btnKot_Click(null, null);
            }

            if (MainID == 0) return;

            frmCheckout frm = new frmCheckout();
            frm.MainID = MainID;
            frm.amt    = Convert.ToDouble(lblTotal.Text);
            MainClass.BlurBackground(frm);

            if (frm.isSuccess)
                btnNew_Click(null, null);
        }

        private void LoadEntries()
        {
            string qry = @"SELECT m.TableName, m.WaiterName, d.DetailID, d.proID, p.pName, d.qty, d.price, d.amount
                           FROM tblMain m
                           INNER JOIN tblDetails d ON m.MainID = d.MainID
                           INNER JOIN products p ON p.pID = d.proID
                           WHERE m.MainID = @ID";
            Hashtable ht = new Hashtable();
            ht.Add("@ID", MainID);
            DataTable dt = MainClass.GetDataTable(qry, ht);

            guna2DataGridView1.Rows.Clear();

            foreach (DataRow item in dt.Rows)
            {
                lblTable.Text    = item["TableName"].ToString();
                lblWaiter.Text   = item["WaiterName"].ToString();
                lblTable.Visible = false;
                lblWaiter.Visible = false;

                guna2DataGridView1.Rows.Add(new object[]
                {
                    0,
                    item["DetailID"],
                    item["proID"],
                    item["pName"],
                    item["qty"],
                    item["price"],
                    item["amount"]
                });
            }
            UpdateBadges();
            GetTotal();
        }
    }
}
