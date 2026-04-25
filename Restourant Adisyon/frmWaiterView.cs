using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon
{
    public partial class frmWaiterView : Form
    {
        public frmWaiterView()
        {
            InitializeComponent();
        }

        private void frmWaiterView_Load(object sender, EventArgs e)
        {
            GetReadyOrders();
        }

        private void GetReadyOrders()
        {
            flowLayoutPanel1.Controls.Clear();
            string qry1 = @"Select * from tblMain where status = 'Ready' ";
            SqlCommand cmd1 = new SqlCommand(qry1, MainClass.con);
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(dt1);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                FlowLayoutPanel p1 = new FlowLayoutPanel();
                p1.AutoSize = true;
                p1.Width = 230;
                p1.Height = 250;
                p1.FlowDirection = FlowDirection.TopDown;
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Margin = new Padding(10);
                p1.BackColor = Color.FromArgb(204, 255, 204); // Light Green for Ready

                FlowLayoutPanel p2 = new FlowLayoutPanel();
                p2.BackColor = Color.FromArgb(50, 55, 89);
                p2.AutoSize = true;
                p2.Width = 230;
                p2.Height = 80;
                p2.FlowDirection = FlowDirection.TopDown;

                p2.Controls.Add(new Label { ForeColor = Color.White, AutoSize = true, Text = "Table :" + dt1.Rows[i]["TableName"], Margin = new Padding(10, 10, 3, 0) });
                p2.Controls.Add(new Label { ForeColor = Color.White, AutoSize = true, Text = "Waiter :" + dt1.Rows[i]["waiterName"], Margin = new Padding(10, 5, 3, 5) });
                p1.Controls.Add(p2);

                int mid = Convert.ToInt32(dt1.Rows[i]["MainID"]);
                string qry2 = "Select pName, qty from tblDetails d inner join products p on p.pID = d.proID where d.MainID = " + mid;
                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);

                foreach (DataRow row in dt2.Rows)
                {
                    p1.Controls.Add(new Label { ForeColor = Color.Black, AutoSize = true, Text = row["pName"] + " x" + row["qty"], Margin = new Padding(10, 5, 3, 0) });
                }

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.Size = new Size(150, 35);
                b.FillColor = Color.FromArgb(241, 85, 126);
                b.Margin = new Padding(35, 10, 3, 10);
                b.Text = "Mark Served";
                b.Tag = dt1.Rows[i]["MainID"].ToString();
                b.Click += (ss, ee) => 
                {
                    int id = Convert.ToInt32((ss as Guna.UI2.WinForms.Guna2Button).Tag);
                    string qry = "Update tblMain set status = 'Served' where MainID = @ID";
                    Hashtable ht = new Hashtable();
                    ht.Add("@ID", id);
                    if (MainClass.Sql(qry, ht) > 0)
                    {
                        GetReadyOrders();
                    }
                };
                p1.Controls.Add(b);

                flowLayoutPanel1.Controls.Add(p1);
            }
        }
    }
}
