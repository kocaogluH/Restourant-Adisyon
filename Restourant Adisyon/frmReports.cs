using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.UI.Controls;
using Restourant_Adisyon.UI.Theme;

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

            ApplyTheme();
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, args) => ApplyTheme();
            ApplyLocalization();

            if (dgvTopProducts != null)
                GridStyler.Apply(dgvTopProducts, "Bu tarih aralığında satılan ürün bulunmuyor.");
            if (dgvDailySales != null)
                GridStyler.Apply(dgvDailySales, "Bu tarih aralığında tamamlanan sipariş bulunmuyor.");

            LoadSummaryCards();
            LoadTopProducts();
            LoadDailySales();

            MainClass.EnableEnterKeyConfirmation(
                this,
                () => btnFilter_Click(btnFilter, EventArgs.Empty)
            );
        }

        private void ApplyTheme()
        {
            this.BackColor = AppTheme.Background;

            if (btnFilter != null)
            {
                btnFilter.BorderRadius = 8;
                btnFilter.FillColor = AppTheme.BrandPrimary;
                btnFilter.ForeColor = Color.White;
                btnFilter.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btnFilter.Cursor = Cursors.Hand;
            }

            if (lblTotalRevenue != null)
            {
                lblTotalRevenue.ForeColor = AppTheme.BrandPrimary;
                lblTotalRevenue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            }

            if (lblTotalOrders != null)
            {
                lblTotalOrders.ForeColor = AppTheme.Success;
                lblTotalOrders.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            }

            if (label1 != null) { label1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); label1.ForeColor = AppTheme.TextSecondary; }
            if (label3 != null) { label3.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); label3.ForeColor = AppTheme.TextSecondary; }
            if (label5 != null) { label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold); label5.ForeColor = AppTheme.TextPrimary; }
            if (label6 != null) { label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold); label6.ForeColor = AppTheme.TextPrimary; }
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label1 != null) label1.Text = loc.GetString("Total_Revenue") ?? "💰 Toplam Ciro";
            if (label3 != null) label3.Text = loc.GetString("Total_Orders") ?? "🧾 Toplam Sipariş";
            if (label5 != null) label5.Text = loc.GetString("Top_Products") ?? "🏆 En Çok Satan Ürünler";
            if (label6 != null) label6.Text = loc.GetString("Daily_Sales") ?? "📊 Günlük Satış Hareketleri";
            if (btnFilter != null) btnFilter.Text = loc.GetString("Search") ?? "🔍 Raporu Getir";
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
