using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
            dtEnd.Value = DateTime.Now;
            LoadReports();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void LoadReports()
        {
            // 1. Load Statistics
            LoadStats();

            // 2. Load Top Selling Products
            LoadTopProducts();

            // 3. Load Daily Sales Chart Data (into grid for now)
            LoadSalesGrid();
        }

        private void LoadStats()
        {
            string start = dtStart.Value.ToString("yyyy-MM-dd");
            string end = dtEnd.Value.ToString("yyyy-MM-dd");

            // Total Revenue in range
            string qry = "Select sum(total) from tblMain where status = 'Paid' and aDate between @start and @end";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            
            if (MainClass.con.State == ConnectionState.Closed) MainClass.con.Open();
            object res = cmd.ExecuteScalar();
            lblTotalRevenue.Text = (res != DBNull.Value ? Convert.ToDouble(res).ToString("N2") : "0.00") + " ₺";

            // Total Orders
            cmd.CommandText = "Select count(MainID) from tblMain where status = 'Paid' and aDate between @start and @end";
            lblTotalOrders.Text = cmd.ExecuteScalar().ToString();

            if (MainClass.con.State == ConnectionState.Open) MainClass.con.Close();
        }

        private void LoadTopProducts()
        {
            string qry = @"Select TOP 5 p.pName as Product, sum(d.qty) as TotalQty, sum(d.amount) as Revenue
                           from tblDetails d
                           inner join products p on p.pID = d.proID
                           inner join tblMain m on m.MainID = d.MainID
                           where m.status = 'Paid' and m.aDate between @start and @end
                           group by p.pName
                           order by TotalQty DESC";

            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            cmd.Parameters.AddWithValue("@start", dtStart.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@end", dtEnd.Value.ToString("yyyy-MM-dd"));
            
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgvTopProducts.DataSource = dt;
        }

        private void LoadSalesGrid()
        {
            string qry = @"Select aDate as Date, sum(total) as DailyTotal, count(MainID) as OrderCount
                           from tblMain 
                           where status = 'Paid' and aDate between @start and @end
                           group by aDate
                           order by aDate DESC";

            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            cmd.Parameters.AddWithValue("@start", dtStart.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@end", dtEnd.Value.ToString("yyyy-MM-dd"));
            
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgvDailySales.DataSource = dt;
        }
    }
}
