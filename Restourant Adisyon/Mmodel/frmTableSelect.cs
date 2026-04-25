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

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmTableSelect : Form
    {
        public frmTableSelect()
        {
            InitializeComponent();
        }

        public string TableName;
       

        private void frmTableSelect_Load(object sender, EventArgs e)
        {
            string qry = "Select * from tables";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                string tname = row["tname"].ToString();
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.Text = tname;
                b.Width = 150; 
                b.Height = 150;
                b.BorderRadius = 10;
                
                // Check if table is busy
                if (IsTableBusy(tname))
                {
                    b.FillColor = Color.FromArgb(231, 76, 60); // Red for Busy
                }
                else
                {
                    b.FillColor = Color.FromArgb(46, 204, 113); // Green for Free
                }

                b.HoverState.FillColor = Color.FromArgb(50, 55, 89);
                b.Click += new EventHandler(b_click);
                flowLayoutPanel1.Controls.Add(b);
            }
        }

        private bool IsTableBusy(string tableName)
        {
            bool isBusy = false;
            string qry = "Select Count(*) from tblMain where TableName = '" + tableName + "' and status = 'Pending'";
            try
            {
                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    isBusy = count > 0;
                }
            }
            finally
            {
                MainClass.con.Close();
            }
            return isBusy;
        }
        private void b_click(object sender, EventArgs e)
        {
            TableName = (sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            this.Close();

        }

    }
}
