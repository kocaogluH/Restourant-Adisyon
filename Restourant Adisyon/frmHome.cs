using System;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;

namespace Restourant_Adisyon
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ApplyLocalization();
            LoadDashboardData();
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label2 != null) label2.Text = loc.GetString("Total_Revenue");
            if (label4 != null) label4.Text = loc.GetString("Total_Orders");
            if (label6 != null) label6.Text = loc.GetString("Nav_Products");
            if (label8 != null) label8.Text = loc.GetString("Nav_Tables");
        }

        private void LoadDashboardData()
        {
            try
            {
                // Toplam Ciro
                string qryRevenue = "SELECT IFNULL(SUM(total),0) FROM tblMain WHERE status='Paid'";
                lblRevenue.Text = GetScalarValue(qryRevenue).ToString("N2") + " ₺";

                // Toplam Sipariş
                string qryOrders = "SELECT COUNT(*) FROM tblMain WHERE status='Paid'";
                lblOrders.Text = ((long)GetScalarValue(qryOrders, true)).ToString();

                // Toplam Ürün
                string qryProducts = "SELECT COUNT(*) FROM products";
                lblProducts.Text = ((long)GetScalarValue(qryProducts, true)).ToString();

                // Aktif Masa
                string qryActiveTables = "SELECT COUNT(*) FROM tblMain WHERE status='Pending'";
                lblActiveTables.Text = ((long)GetScalarValue(qryActiveTables, true)).ToString();
            }
            catch (Exception ex)
            {
                MainClass.LogError("frmHome.LoadDashboardData", ex);
                lblRevenue.Text      = "0.00 ₺";
                lblOrders.Text       = "0";
                lblProducts.Text     = "0";
                lblActiveTables.Text = "0";
            }
        }

        private double GetScalarValue(string qry, bool returnLong = false)
        {
            try
            {
                object result = MainClass.SqlScalar(qry);
                if (result != null && result != DBNull.Value)
                    return returnLong ? Convert.ToInt64(result) : Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                MainClass.LogError("frmHome.GetScalarValue", ex);
            }
            return 0;
        }
    }
}
