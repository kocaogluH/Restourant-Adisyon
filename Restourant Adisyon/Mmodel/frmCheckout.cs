using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.Core.Entities;
using Restourant_Adisyon.Core.Enums;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmCheckout : Form
    {
        private readonly OrderService _orderService = new OrderService();
        public double amt;
        public int    MainID;
        public bool   isSuccess = false;
        private PaymentMethod _selectedMethod = PaymentMethod.Nakit;

        private FlowLayoutPanel _flpQuickCash;


        public frmCheckout()
        {
            InitializeComponent();
        }

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            SetupModernCheckoutUI();
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ApplyLocalization();

            Order order = _orderService.GetOrderById(MainID);
            if (order != null)
            {
                txtBillAmount.Text = order.RemainingAmount > 0 ? order.RemainingAmount.ToString("N2") : order.TotalAmount.ToString("N2");
            }
            else
            {
                txtBillAmount.Text = amt.ToString("N2");
            }

            txtReceived.Focus();
        }

        private void SetupModernCheckoutUI()
        {
            this.BackColor = AppTheme.CardBackground;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // 1. Üst Başlık Paneli
            if (guna2Panel1 != null)
            {
                guna2Panel1.FillColor = AppTheme.SidebarBackground;
                guna2Panel1.BackColor = AppTheme.SidebarBackground;
                guna2Panel1.Height = 70;

                if (label1 != null)
                {
                    label1.Text = "💳 Hesap Kapatma & Ödeme";
                    label1.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
                    label1.ForeColor = Color.White;
                    label1.Location = new Point(20, 22);
                }
            }

            // 2. Metin Kutuları ve Etiketler
            if (txtBillAmount != null)
            {
                txtBillAmount.BorderRadius = 8;
                txtBillAmount.BorderColor = AppTheme.BorderColor;
                txtBillAmount.FillColor = Color.FromArgb(248, 250, 252);
                txtBillAmount.ForeColor = AppTheme.BrandPrimary;
                txtBillAmount.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
                txtBillAmount.ReadOnly = true;
            }

            if (txtReceived != null)
            {
                txtReceived.BorderRadius = 8;
                txtReceived.BorderColor = AppTheme.BrandPrimary;
                txtReceived.FillColor = Color.White;
                txtReceived.ForeColor = AppTheme.TextPrimary;
                txtReceived.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            }

            if (txtChange != null)
            {
                txtChange.BorderRadius = 8;
                txtChange.BorderColor = AppTheme.BorderColor;
                txtChange.FillColor = Color.FromArgb(248, 250, 252);
                txtChange.ForeColor = AppTheme.Success;
                txtChange.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
                txtChange.ReadOnly = true;
            }

            // 3. Ödeme Butonu
            if (btnSave != null)
            {
                btnSave.BorderRadius = 8;
                btnSave.FillColor = AppTheme.Success;
                btnSave.ForeColor = Color.White;
                btnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                btnSave.Cursor = Cursors.Hand;
            }

            // 4. Hızlı Nakit Tuşları (Eğer henüz eklenmediyse)
            if (_flpQuickCash == null)
            {
                _flpQuickCash = new FlowLayoutPanel
                {
                    Location = new Point(txtReceived.Left, txtReceived.Bottom + 8),
                    Size = new Size(txtReceived.Width, 38),
                    FlowDirection = FlowDirection.LeftToRight,
                    BackColor = Color.Transparent
                };
                this.Controls.Add(_flpQuickCash);

                int[] presets = new int[] { 50, 100, 200, 500 };
                foreach (int p in presets)
                {
                    Guna2Button btnP = new Guna2Button
                    {
                        Text = p + " ₺",
                        Size = new Size(58, 30),
                        BorderRadius = 6,
                        FillColor = Color.FromArgb(241, 245, 249),
                        ForeColor = Color.FromArgb(30, 41, 59),
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        Margin = new Padding(0, 0, 4, 0),
                        Cursor = Cursors.Hand
                    };
                    btnP.Click += (s, ev) =>
                    {
                        txtReceived.Text = p.ToString();
                        txtReceived_TextChanged(null, null);
                    };
                    _flpQuickCash.Controls.Add(btnP);
                }

                Guna2Button btnExact = new Guna2Button
                {
                    Text = "Tam",
                    Size = new Size(50, 30),
                    BorderRadius = 6,
                    FillColor = AppTheme.BrandPrimary,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnExact.Click += (s, ev) =>
                {
                    txtReceived.Text = txtBillAmount.Text;
                    txtReceived_TextChanged(null, null);
                };
                _flpQuickCash.Controls.Add(btnExact);
            }
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label1 != null) label1.Text = loc.GetString("Checkout_Title") ?? "💳 Hesap Kapatma & Ödeme";
            if (label2 != null) label2.Text = loc.GetString("Bill_Amount") ?? "Hesap Tutarı";
            if (label3 != null) label3.Text = loc.GetString("Received_Amount") ?? "Alınan Tutar";
            if (label4 != null) label4.Text = loc.GetString("Change_Amount") ?? "Para Üstü";
            if (btnSave != null) btnSave.Text = loc.GetString("Pay_Bill") ?? "✅ Ödemeyi Tamamla";
        }

        private void txtReceived_TextChanged(object sender, EventArgs e)
        {
            double amtVal = 0, received = 0;
            double.TryParse(txtBillAmount.Text, out amtVal);
            double.TryParse(txtReceived.Text,   out received);

            if (received >= 0)
            {
                double change = received - amtVal;
                txtChange.Text = change >= 0 ? change.ToString("N2") : "0.00";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReceived.Text))
            {
                MessageBox.Show("Lütfen alınan ödeme tutarını girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReceived.Focus();
                return;
            }

            decimal received = 0m;
            if (!decimal.TryParse(txtReceived.Text, out received) || received <= 0m)
            {
                MessageBox.Show("Geçerli bir ödeme tutarı girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Split Bill / Parçalı Ödeme İşleme
            decimal remainingAmount;
            bool success = _orderService.AddPayment(MainID, received, _selectedMethod, out remainingAmount);

            if (success)
            {
                if (remainingAmount <= 0)
                {
                    MessageBox.Show("Hesabın tamamı kapatıldı! Fiş yazdırılıyor...",
                        "Ödeme Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isSuccess = true;
                    PrintReceipt((double)received, 0);
                }
                else
                {
                    MessageBox.Show($"Parçalı ödeme alındı! Kalan Hesap Tutarı: {remainingAmount:N2} ₺",
                        "Parçalı Ödeme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBillAmount.Text = remainingAmount.ToString("N2");
                    txtReceived.Clear();
                    txtChange.Clear();
                }
            }
            else
            {
                MessageBox.Show("Ödeme işlenirken bir hata oluştu veya kalan tutardan fazla miktar girildi.", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReceipt(double received, double change)
        {
            try
            {
                DataTable dt = GetOrderDetails();

                using (PrintDocument pd = new PrintDocument())
                {
                    pd.PrintPage += (s, ev) =>
                    {
                        Graphics g = ev.Graphics;
                        var fntBold   = new Font("Courier New", 10, FontStyle.Bold);
                        var fntNormal = new Font("Courier New", 9);
                        int y = 10, x = 10;

                        g.DrawString("ADİSYON / FİŞ", fntBold, Brushes.Black, x + 50, y); y += 20;
                        g.DrawString($"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm}", fntNormal, Brushes.Black, x, y); y += 15;
                        g.DrawString($"Kasiyer: {MainClass.USER}", fntNormal, Brushes.Black, x, y); y += 15;
                        g.DrawString(new string('-', 38), fntNormal, Brushes.Black, x, y); y += 12;

                        foreach (DataRow row in dt.Rows)
                        {
                            string line = $"{row["pName"],-18} x{row["qty"]} {Convert.ToDouble(row["amount"]):N2} TL";
                            g.DrawString(line, fntNormal, Brushes.Black, x, y); y += 13;
                        }

                        g.DrawString(new string('-', 38), fntNormal, Brushes.Black, x, y); y += 12;
                        g.DrawString($"TOPLAM    : {amt,12:N2} TL", fntBold, Brushes.Black, x, y); y += 15;
                        g.DrawString($"ALINDI    : {received,12:N2} TL", fntNormal, Brushes.Black, x, y); y += 13;
                        g.DrawString($"PARA ÜSTÜ : {change,12:N2} TL", fntBold, Brushes.Black, x, y); y += 20;
                        g.DrawString("   Teşekkür ederiz!   ", fntBold, Brushes.Black, x + 30, y);
                    };

                    PrintDialog dlg = new PrintDialog { Document = pd };
                    if (dlg.ShowDialog() == DialogResult.OK)
                        pd.Print();
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("PrintReceipt", ex);
            }
            finally
            {
                this.Close();
            }
        }

        private DataTable GetOrderDetails()
        {
            string qry = @"SELECT p.pName, d.qty, d.price, d.amount
                           FROM tblDetails d
                           INNER JOIN products p ON p.pID = d.proID
                           WHERE d.MainID = @ID";
            Hashtable ht = new Hashtable();
            ht.Add("@ID", MainID);
            return MainClass.GetDataTable(qry, ht);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
