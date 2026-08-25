using System;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        public int    MainID = 0;
        public string OrderType;

        // Barkod tamponu (klavye ile hızlı giriş algılar)
        private string _barcodeBuffer = "";
        private DateTime _lastKeyTime = DateTime.Now;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();
            ProductPanel.Controls.Clear();
            LoadProducts();

            // Barkod desteği: form-level KeyPreview etkinleştir
            this.KeyPreview = true;
        }

        // ── Kategori Butonları ───────────────────────────────────────────────────
        private void AddCategory()
        {
            string qry = "SELECT * FROM category";
            DataTable dt = MainClass.GetDataTable(qry);

            CategoryPanel.Controls.Clear();

            // "Tümü" butonu
            Guna.UI2.WinForms.Guna2Button b2 = new Guna.UI2.WinForms.Guna2Button();
            b2.FillColor = Color.FromArgb(50, 55, 89);
            b2.Size      = new Size(134, 45);
            b2.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            b2.Text      = "Tümü";
            b2.CheckedState.FillColor = Color.FromArgb(241, 85, 126);
            b2.Click += new EventHandler(b_click);
            CategoryPanel.Controls.Add(b2);

            foreach (DataRow row in dt.Rows)
            {
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.FillColor = Color.FromArgb(94, 148, 255);
                b.Size      = new Size(180, 45);
                b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                b.Text      = row["catName"].ToString();
                b.CheckedState.FillColor = Color.FromArgb(241, 85, 126);
                b.Click += new EventHandler(b_click);
                CategoryPanel.Controls.Add(b);
            }
        }

        private void b_click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            if (b.Text == "Tümü")
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
            string qry = @"SELECT p.*, c.catName FROM products p
                           INNER JOIN category c ON c.catID = p.CategoryID";
            DataTable dt = MainClass.GetDataTable(qry);

            foreach (DataRow item in dt.Rows)
            {
                Image pImage = null;
                try
                {
                    if (item["pImage"] != DBNull.Value && item["pImage"] is byte[] imgBytes && imgBytes.Length > 0)
                        pImage = Image.FromStream(new MemoryStream(imgBytes));
                }
                catch { /* Görsel yoksa null bırak */ }

                AddItem(item["pID"].ToString(), item["pName"].ToString(),
                        item["catName"].ToString(), item["pPrice"].ToString(),
                        item["pBarcode"] != DBNull.Value ? item["pBarcode"].ToString() : "",
                        pImage);
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
        // Barkod okuyucular klavye gibi hızlı karakter gönderir ve Enter ile bitirir.
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // Arama kutusu odaklanmışsa normal yazma devam etsin
            if (txtSearch.Focused) return;

            double elapsed = (DateTime.Now - _lastKeyTime).TotalMilliseconds;
            _lastKeyTime = DateTime.Now;

            // Çok hızlı karakter geliyorsa barkod tamponu doldur
            if (elapsed < 80)
                _barcodeBuffer += e.KeyChar;
            else
                _barcodeBuffer = e.KeyChar.ToString();

            // Enter = barkod tamamlandı
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
            DataRow row = MainClass.FindProductByBarcode(barcode);
            if (row != null)
            {
                AddToCart(Convert.ToInt32(row["pID"]), row["pName"].ToString(), row["pPrice"].ToString());
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
        }

        // ── Sepetten Ürün Kaldır (Delete) ───────────────────────────────────────
        private void guna2DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && guna2DataGridView1.CurrentRow != null)
            {
                guna2DataGridView1.Rows.RemoveAt(guna2DataGridView1.CurrentRow.Index);
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
            OrderType = "";
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

                guna2MessageDialog1.Show("Sipariş kaydedildi!");
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

        // ── Ödeme Al ────────────────────────────────────────────────────────────
        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (MainID == 0)
            {
                guna2MessageDialog1.Show("Lütfen önce bir fatura seçin.");
                return;
            }

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
