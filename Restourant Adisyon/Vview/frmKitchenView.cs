using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restourant_Adisyon.Vview
{
    public partial class frmKitchenView : Form
    {
        public frmKitchenView()
        {
            InitializeComponent();
        }

        private void frmKitchenView_Load(object sender, EventArgs e)
        {
            GetOrders();
        }

        private void GetOrders()
        {
            flowLayoutPanel1.Controls.Clear();
            string qry1 = @"Select * from tblMain where status in ('Pending', 'Cooking') ";
            SqlCommand cmd1 = new SqlCommand(qry1, MainClass.con);
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(dt1);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string currentStatus = dt1.Rows[i]["status"].ToString();

                FlowLayoutPanel p1 = new FlowLayoutPanel();
                p1.AutoSize = true;
                p1.Width = 230;
                p1.Height = 350;
                p1.FlowDirection = FlowDirection.TopDown;
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Margin = new Padding(10, 10, 10, 10);
                
                // Color based on status
                p1.BackColor = currentStatus == "Cooking" ? Color.FromArgb(255, 255, 204) : Color.White;

                FlowLayoutPanel p2 = new FlowLayoutPanel();
                p2.BackColor = Color.FromArgb(50, 55, 89);
                p2.AutoSize = true;
                p2.Width = 230;
                p2.Height = 125;
                p2.FlowDirection = FlowDirection.TopDown;
                p2.Margin = new Padding(0, 0, 0, 0);

                Label lb1 = new Label { ForeColor = Color.White, Margin = new Padding(10, 10, 3, 0), AutoSize = true, Text = "Table :" + dt1.Rows[i]["TableName"] };
                Label lb2 = new Label { ForeColor = Color.White, Margin = new Padding(10, 5, 3, 0), AutoSize = true, Text = "Waiter :" + dt1.Rows[i]["waiterName"] };
                Label lb3 = new Label { ForeColor = Color.White, Margin = new Padding(10, 5, 3, 0), AutoSize = true, Text = "Time :" + dt1.Rows[i]["aTime"] };

                p2.Controls.Add(lb1);
                p2.Controls.Add(lb2);
                p2.Controls.Add(lb3);
                p1.Controls.Add(p2);

                int mid = Convert.ToInt32(dt1.Rows[i]["MainID"]);
                string qry2 = "Select pName, qty from tblDetails d inner join products p on p.pID = d.proID where d.MainID = " + mid;
                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);

                foreach (DataRow row in dt2.Rows)
                {
                    Label lbItem = new Label { ForeColor = Color.Black, Margin = new Padding(10, 5, 3, 0), AutoSize = true, Text = row["pName"] + " x" + row["qty"] };
                    p1.Controls.Add(lbItem);
                }

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.Size = new Size(150, 35);
                b.Margin = new Padding(35, 10, 3, 10);
                b.Tag = dt1.Rows[i]["MainID"].ToString();

                if (currentStatus == "Pending")
                {
                    b.Text = "Start Cooking";
                    b.FillColor = Color.FromArgb(52, 152, 219); // Blue
                }
                else
                {
                    b.Text = "Ready";
                    b.FillColor = Color.FromArgb(46, 204, 113); // Green
                }

                b.Click += new EventHandler(b_click);
                p1.Controls.Add(b);

                flowLayoutPanel1.Controls.Add(p1);
            }
        }

        private void b_click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            int id = Convert.ToInt32(btn.Tag);
            string nextStatus = btn.Text == "Start Cooking" ? "Cooking" : "Ready";

            string qry = "Update tblMain set status = @status where MainID = @ID";
            Hashtable ht = new Hashtable();
            ht.Add("@ID", id);
            ht.Add("@status", nextStatus);

            if (MainClass.Sql(qry, ht) > 0)
            {
                GetOrders();
            }
        }


        //YouTubede anlatanan kişinin hata veren kodu 

        //private void b_click(object sender, EventArgs e)
        //{
        //  int ıd = Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());


        //   guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
        //   guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;


        //   if (guna2MessageDialog1.Show("Are you want to delete?") == DialogResult.Yes)
        //   {
        //       string qry = @"Update tblMain set status = 'Complete' where MainID = @ID";
        //       Hashtable ht = new Hashtable();
        //       ht.Add("@ID", id);


        //   }
        //   if (MainClass.Sql(qry,ht)>0)
        //   {
        //       guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
        //       guna2MessageDialog1.Show("Saved Successfully");
        //   }
        //   GetOrders();

        //}
    }
}
