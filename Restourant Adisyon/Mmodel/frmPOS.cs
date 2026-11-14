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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            ApplyThemeAndStyling();

            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyThemeAndStyling();

            ApplyLocalization();

            // Buton Hiyerarşisi (Button Roles)
            if (btnCheckout != null) AppTheme.StyleButton(btnCheckout, ButtonRole.Success); // Yeşil
            if (btnKot != null)      AppTheme.StyleButton(btnKot, ButtonRole.Primary);      // Pembe
            if (btnNew != null)      AppTheme.StyleButton(btnNew, ButtonRole.Info);         // Mavi
            if (btnBill != null)     AppTheme.StyleButton(btnBill, ButtonRole.Secondary);

            SetupQuickCashButtons();

            ProductPanel.Controls.Clear();
            LoadProducts();

            // Barkod desteği: form-level KeyPreview etkinleştir
            this.KeyPreview = true;
        }

        private void ApplyThemeAndStyling()
        {
            this.BackColor = AppTheme.Background;
            if (guna2Panel1 != null) guna2Panel1.FillColor = AppTheme.SidebarBackground;
            if (guna2Panel2 != null)
            {
                guna2Panel2.FillColor = AppTheme.CardBackground;
                guna2Panel2.BorderColor = AppTheme.BorderColor;
                guna2Panel2.BorderThickness = 1;
            }

            if (guna2DataGridView1 != null)
                GridStyler.Apply(guna2DataGridView1, "Cart_Empty");

            if (lblTotal != null)
                lblTotal.ForeColor = AppTheme.BrandPrimary;

            if (txtSearch != null)
            {
                txtSearch.FillColor = AppTheme.CardBackground;
                txtSearch.ForeColor = AppTheme.TextPrimary;
                txtSearch.BorderColor = AppTheme.BorderColor;
            }
        }

        private void SetupQuickCashButtons()
        {
            if (guna2Panel2 == null || _flpQuickCash != null) return;

            // Fiş döküm etiketleri (Ara Toplam & KDV)
            _lblSubtotal = new Label
            {
                Text = "Ara Toplam: 0.00 ₺",
                Font = new Font("Segoe UI", 9F),
                ForeColor = AppTheme.TextSecondary,
                Location = new Point(14, 20),
                AutoSize = true
            };
            guna2Panel2.Controls.Add(_lblSubtotal);

            _lblTax = new Label
            {
                Text = "KDV (%10): 0.00 ₺",
                Font = new Font("Segoe UI", 9F),
                ForeColor = AppTheme.TextSecondary,
                Location = new Point(14, 40),
                AutoSize = true
            };
            guna2Panel2.Controls.Add(_lblTax);

            // Hızlı Nakit Tuşları (50₺, 100₺, 200₺, 500₺, Tam Tutar)
            _flpQuickCash = new FlowLayoutPanel
            {
                Size = new Size(guna2Panel2.Width - 28, 40),
                Location = new Point(14, 65),
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
                    Size = new Size(54, 30),
                    Margin = new Padding(0, 0, 4, 0),
                    AutoRoundedCorners = true,
                    FillColor = AppTheme.Secondary,
                    ForeColor = AppTheme.TextPrimary,
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
                Size = new Size(50, 30),
                AutoRoundedCorners = true,
                FillColor = AppTheme.Success,
                ForeColor = Color.White,
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
            if (btnNew != null)      btnNew.Text      = loc.GetString("POS_NewOrder");
            if (btnDin != null)      btnDin.Text      = loc.GetString("POS_DineIn");
            if (btnTake != null)     btnTake.Text     = loc.GetString("POS_TakeAway");
            if (btnDelivery != null) btnDelivery.Text = loc.GetString("POS_Delivery");
            if (btnKot != null)      btnKot.Text      = loc.GetString("POS_KOT");
            if (btnBill != null)     btnBill.Text     = loc.GetString("POS_BillList");
            if (btnCheckout != null) btnCheckout.Text = loc.GetString("POS_Checkout");
            if (txtSearch != null)   txtSearch.PlaceholderText = loc.GetString("Search");
            AddCategory();
        }

        // ── Kategori Butonları ───────────────────────────────────────────────────
        private void AddCategory()
        {
            CategoryPanel.Controls.Clear();

            // "Tümü" / "All" butonu
            string allText = LocalizationService.Instance.GetString("All_Categories");
            Guna2Button b2 = new Guna2Button
            {
                FillColor = AppTheme.Secondary,
                Size = new Size(130, 42),
                ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                Text = allText,
                AutoRoundedCorners = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                Checked = true
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
                    Size = new Size(140, 42),
                    ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                    Text = cat.Name,
                    AutoRoundedCorners = true,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = AppTheme.TextPrimary
                };
                b.CheckedState.FillColor = AppTheme.BrandPrimary;
                b.CheckedState.ForeColor = Color.White;
                b.Click += new EventHandler(b_click);
                CategoryPanel.Controls.Add(b);
            }
        }

        private void b_click(object sender, EventArgs e)
        {
            string allText = LocalizationService.Instance.GetString("All_Categories");
            Guna2Button b = (Guna2Button)sender;
            if (b.Text == allText || b.Text == "Tümü" || b.Text == "All")
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

        // ── Ürün Yükleme ────────────────────────────────────────────────────────
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

        // ── Sepete Ekleme (Barkod veya tıklama) ─────────────────────────────────
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

        // ── Sepetten Ürün Kaldır (Delete) & Miktar Değiştirme ─────────────────────
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

        // ── Sipariş Tipi Butonları ───────────────────────────────────────────────
        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";  lblWaiter.Text = "";
            lblTable.Visible = false; lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "0.00";
            OrderType = "Masada";
            GetTotal();
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTable.Visible = false; lblWaiter.Visible = false;
            OrderType = "Paket";
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            lblTable.Visible = false; lblWaiter.Visible = false;
            OrderType = "Gel-Al";
        }

        private void btnDin_Click(object sender, EventArgs e)
        {
            OrderType = "Masada";

            frmTableSelect frm = new frmTableSelect();
            MainClass.BlurBackground(frm);
            lblTable.Text    = frm.TableName ?? "";
            lblTable.Visible = !string.IsNullOrEmpty(frm.TableName);

            frmWaiterSelect frm2 = new frmWaiterSelect();
            MainClass.BlurBackground(frm2);
            lblWaiter.Text    = frm2.waiterName ?? "";
            lblWaiter.Visible = !string.IsNullOrEmpty(frm2.waiterName);
        }

        // ── Sipariş Kaydet (KOT) ────────────────────────────────────────────────
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
                                // Yeni kayıt
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
                                // Güncelle
                                string qry1 = @"UPDATE tblMain SET status='Pending', total=@total WHERE MainID=@ID";
                                using (var cmd = new System.Data.SQLite.SQLiteCommand(qry1, con, tr))
                                {
                                    cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text));
                                    cmd.Parameters.AddWithValue("@ID",    MainID);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Detaylar
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

                                // Stok düş
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

        // ── Fatura Listesi ───────────────────────────────────────────────────────
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

        // ── Ödeme Al (Checkout) ──────────────────────────────────────────────────
        private void btnCheckout_Click(object sender, EventArgs e)
        {
            // Eğer sipariş henüz kaydedilmemişse otomatik kaydet ve ödemeye geç
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

        // ── Mevcut Siparişi Yükle ────────────────────────────────────────────────
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
                lblTable.Visible = true;
                lblWaiter.Visible = true;

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
            GetTotal();
        }
    }
}
