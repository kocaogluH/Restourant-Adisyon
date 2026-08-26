using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;

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
            if (dtStart != null) dtStart.Value = DateTime.Today;
            if (dtEnd != null)   dtEnd.Value   = DateTime.Today.AddDays(1).AddSeconds(-1);

            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ApplyLocalization();

            LoadSummaryCards();
            LoadTopProducts();
            LoadDailySales();
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label1 != null) label1.Text = loc.GetString("Total_Revenue");
            if (label3 != null) label3.Text = loc.GetString("Total_Orders");
            if (label5 != null) label5.Text = loc.GetString("Top_Products");
            if (label6 != null) label6.Text = loc.GetString("Daily_Sales");
            if (btnFilter != null) btnFilter.Text = loc.GetString("Search");
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadSummaryCards();
            LoadTopProducts();
            LoadDailySales();
        }

        private void LoadSummaryCards()
        {
            Hashtable ht = new Hashtable();
            string startStr = dtStart != null ? dtStart.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
            string endStr   = dtEnd != null ? dtEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            ht.Add("@start", startStr);
            ht.Add("@end",   endStr);

            object rev = MainClass.SqlScalar(
                "SELECT IFNULL(SUM(total),0) FROM tblMain WHERE status='Paid' AND aDate BETWEEN @start AND @end", ht);
            lblTotalRevenue.Text = Convert.ToDouble(rev).ToString("N2") + " ₺";

            object cnt = MainClass.SqlScalar(
                "SELECT COUNT(MainID) FROM tblMain WHERE status='Paid' AND aDate BETWEEN @start AND @end", ht);
            lblTotalOrders.Text = Convert.ToInt64(cnt).ToString();
        }

        private void LoadTopProducts()
        {
            string qry = @"SELECT p.pName AS 'Ürün Adı', SUM(d.qty) AS 'Toplam Adet', SUM(d.amount) AS 'Toplam Ciro ₺'
                           FROM tblDetails d
                           INNER JOIN products p ON p.pID = d.proID
                           INNER JOIN tblMain m  ON m.MainID = d.MainID
                           WHERE m.status = 'Paid' AND m.aDate BETWEEN @start AND @end
                           GROUP BY p.pID, p.pName
                           ORDER BY SUM(d.qty) DESC
                           LIMIT 10";

            Hashtable ht = new Hashtable();
            string startStr = dtStart != null ? dtStart.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
            string endStr   = dtEnd != null ? dtEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            ht.Add("@start", startStr);
            ht.Add("@end",   endStr);

            dgvTopProducts.DataSource = MainClass.GetDataTable(qry, ht);
        }

        private void LoadDailySales()
        {
            string qry = @"SELECT MainID AS 'Adisyon No', aDate AS 'Tarih', TableName AS 'Masa',
                                  WaiterName AS 'Garson', total AS 'Tutar ₺', status AS 'Durum'
                           FROM tblMain
                           WHERE status='Paid' AND aDate BETWEEN @start AND @end
                           ORDER BY MainID DESC";

            Hashtable ht = new Hashtable();
            string startStr = dtStart != null ? dtStart.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
            string endStr   = dtEnd != null ? dtEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            ht.Add("@start", startStr);
            ht.Add("@end",   endStr);

            dgvDailySales.DataSource = MainClass.GetDataTable(qry, ht);
        }
    }
}
