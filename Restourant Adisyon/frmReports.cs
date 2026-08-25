using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace Restourant_Adisyon
{
    public partial class frmReports : Form
    {
        public frmReports()
        {
            InitializeComponent();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            dtStart.Value = DateTime.Now.AddDays(-7);
            dtEnd.Value   = DateTime.Now;
            LoadReports();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void LoadReports()
        {
            LoadStats();
            LoadTopProducts();
            LoadSalesGrid();
        }

        // ─── İstatistikler ──────────────────────────────────────────────────────
        private void LoadStats()
        {
            string start = dtStart.Value.ToString("yyyy-MM-dd");
            string end   = dtEnd.Value.ToString("yyyy-MM-dd");

            // Toplam Ciro
            Hashtable ht = new Hashtable();
            ht.Add("@start", start);
            ht.Add("@end",   end);

            object res = MainClass.SqlScalar(
                "SELECT IFNULL(SUM(total),0) FROM tblMain WHERE status='Paid' AND aDate BETWEEN @start AND @end", ht);
            lblTotalRevenue.Text = Convert.ToDouble(res).ToString("N2") + " ₺";

            // Toplam Sipariş
            object cnt = MainClass.SqlScalar(
                "SELECT COUNT(MainID) FROM tblMain WHERE status='Paid' AND aDate BETWEEN @start AND @end", ht);
            lblTotalOrders.Text = Convert.ToInt64(cnt).ToString();

            // Ortalama Sipariş Tutarı (kontrol varsa)
            // Gelecekte lblAvgOrder eklendikten sonra etkinleştirilebilir
        }

        // ─── En Çok Satılan Ürünler ─────────────────────────────────────────────
        private void LoadTopProducts()
        {
            string qry = @"SELECT p.pName AS 'Ürün', 
                                  SUM(d.qty) AS 'Adet', 
                                  ROUND(SUM(d.amount),2) AS 'Ciro (₺)'
                           FROM tblDetails d
                           INNER JOIN products p ON p.pID = d.proID
                           INNER JOIN tblMain m ON m.MainID = d.MainID
                           WHERE m.status = 'Paid'
                             AND m.aDate BETWEEN @start AND @end
                           GROUP BY p.pName
                           ORDER BY SUM(d.qty) DESC
                           LIMIT 10";

            Hashtable ht = new Hashtable();
            ht.Add("@start", dtStart.Value.ToString("yyyy-MM-dd"));
            ht.Add("@end",   dtEnd.Value.ToString("yyyy-MM-dd"));

            dgvTopProducts.DataSource = MainClass.GetDataTable(qry, ht);
        }

        // ─── Günlük Satış Listesi ───────────────────────────────────────────────
        private void LoadSalesGrid()
        {
            string qry = @"SELECT aDate AS 'Tarih',
                                  COUNT(MainID) AS 'Sipariş Sayısı',
                                  ROUND(SUM(total),2) AS 'Günlük Ciro (₺)'
                           FROM tblMain
                           WHERE status = 'Paid'
                             AND aDate BETWEEN @start AND @end
                           GROUP BY aDate
                           ORDER BY aDate DESC";

            Hashtable ht = new Hashtable();
            ht.Add("@start", dtStart.Value.ToString("yyyy-MM-dd"));
            ht.Add("@end",   dtEnd.Value.ToString("yyyy-MM-dd"));

            dgvDailySales.DataSource = MainClass.GetDataTable(qry, ht);
        }

        // ─── Raporu Yazdır ──────────────────────────────────────────────────────
        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            PrintReport();
        }

        private void PrintReport()
        {
            try
            {
                using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                {
                    pd.PrintPage += (s, ev) =>
                    {
                        System.Drawing.Graphics g = ev.Graphics;
                        var fntTitle  = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
                        var fntHeader = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
                        var fntNormal = new System.Drawing.Font("Arial", 9);
                        int y = 30;

                        g.DrawString("SATIŞ RAPORU", fntTitle, System.Drawing.Brushes.Black, 20, y);
                        y += 30;
                        g.DrawString($"Tarih Aralığı: {dtStart.Value:dd.MM.yyyy} - {dtEnd.Value:dd.MM.yyyy}", fntHeader, System.Drawing.Brushes.Black, 20, y);
                        y += 20;
                        g.DrawString($"Toplam Ciro: {lblTotalRevenue.Text}", fntNormal, System.Drawing.Brushes.Black, 20, y);
                        y += 15;
                        g.DrawString($"Toplam Sipariş: {lblTotalOrders.Text}", fntNormal, System.Drawing.Brushes.Black, 20, y);
                        y += 25;

                        g.DrawString("EN ÇOK SATILAN ÜRÜNLER", fntHeader, System.Drawing.Brushes.Black, 20, y);
                        y += 20;
                        foreach (DataGridViewRow row in dgvTopProducts.Rows)
                        {
                            string line = $"  {row.Cells[0].Value}  |  Adet: {row.Cells[1].Value}  |  Ciro: {row.Cells[2].Value} ₺";
                            g.DrawString(line, fntNormal, System.Drawing.Brushes.Black, 20, y);
                            y += 14;
                        }
                    };

                    PrintDialog dlg = new PrintDialog();
                    dlg.Document = pd;
                    if (dlg.ShowDialog() == DialogResult.OK)
                        pd.Print();
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("frmReports.PrintReport", ex);
                MessageBox.Show("Yazdırma hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
