using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmCheckout : Form
    {
        public frmCheckout()
        {
            InitializeComponent();
        }

        public double amt;
        public int    MainID;
        public bool   isSuccess = false;

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            txtBillAmount.Text = amt.ToString("N2");
        }

        private void txtReceived_TextChanged(object sender, EventArgs e)
        {
            double amtVal = 0, received = 0;
            double.TryParse(txtBillAmount.Text, out amtVal);
            double.TryParse(txtReceived.Text,   out received);

            if (received >= 0)
            {
                double change = received - amtVal;
                txtChange.Text = change.ToString("N2");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReceived.Text))
            {
                MessageBox.Show("Lütfen alınan tutarı girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReceived.Focus();
                return;
            }

            double received = 0;
            if (!double.TryParse(txtReceived.Text, out received) || received < 0)
            {
                MessageBox.Show("Geçerli bir tutar girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double change = received - amt;

            string qry = @"UPDATE tblMain 
                           SET status='Paid', received=@received, change=@change
                           WHERE MainID=@ID";

            Hashtable ht = new Hashtable();
            ht.Add("@ID",       MainID);
            ht.Add("@received", received);
            ht.Add("@change",   change);

            if (MainClass.Sql(qry, ht) > 0)
            {
                MessageBox.Show("Ödeme alındı. Para üstü: " + change.ToString("N2") + " ₺",
                    "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isSuccess = true;

                // Fiş yazdır
                PrintReceipt(received, change);
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
