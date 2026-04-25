using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
			LoadDashboardData();
		}

		private void LoadDashboardData()
		{
			try
			{
				// Total Revenue
				string qryRevenue = "Select Sum(total) from tblMain";
				lblRevenue.Text = GetScalarValue(qryRevenue).ToString("N2") + " TL";

				// Total Orders
				string qryOrders = "Select Count(*) from tblMain";
				lblOrders.Text = GetScalarValue(qryOrders).ToString();

				// Total Products
				string qryProducts = "Select Count(*) from products";
				lblProducts.Text = GetScalarValue(qryProducts).ToString();

				// Active Tables
				string qryActiveTables = "Select Count(*) from tblMain where status = 'Pending'";
				lblActiveTables.Text = GetScalarValue(qryActiveTables).ToString();
			}
			catch (Exception ex)
			{
				// Fallback if tables don't exist yet
				lblRevenue.Text = "0.00 TL";
				lblOrders.Text = "0";
				lblProducts.Text = "0";
				lblActiveTables.Text = "0";
			}
		}

		private double GetScalarValue(string qry)
		{
			double value = 0;
			try
			{
				using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
				{
					if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
					object result = cmd.ExecuteScalar();
					if (result != null && result != DBNull.Value)
					{
						value = Convert.ToDouble(result);
					}
				}
			}
			finally
			{
				MainClass.con.Close();
			}
			return value;
		}
	}
}
