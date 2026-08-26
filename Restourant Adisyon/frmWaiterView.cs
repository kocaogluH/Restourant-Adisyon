using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;

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
            LocalizationService.Instance.OnLanguageChanged += (s, ev) => GetReadyOrders();
            GetReadyOrders();
        }

        private void GetReadyOrders()
        {
            flowLayoutPanel1.Controls.Clear();

            string qry1 = "SELECT * FROM tblMain WHERE status = 'Ready' ORDER BY MainID ASC";
            DataTable dt1 = MainClass.GetDataTable(qry1);
            var loc = LocalizationService.Instance;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                FlowLayoutPanel p1 = new FlowLayoutPanel
                {
                    AutoSize = true, Width = 230, Height = 250,
                    FlowDirection = FlowDirection.TopDown,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    BackColor = Color.FromArgb(204, 255, 204)
                };

                FlowLayoutPanel p2 = new FlowLayoutPanel
                {
                    BackColor = Color.FromArgb(50, 55, 89),
                    AutoSize  = true, Width = 230, Height = 80,
                    FlowDirection = FlowDirection.TopDown
                };

                p2.Controls.Add(new Label { ForeColor = Color.White, AutoSize = true, Text = loc.GetString("Table") + " : " + dt1.Rows[i]["TableName"], Margin = new Padding(10, 10, 3, 0) });
                p2.Controls.Add(new Label { ForeColor = Color.White, AutoSize = true, Text = loc.GetString("Waiter") + " : " + dt1.Rows[i]["WaiterName"], Margin = new Padding(10, 5, 3, 5) });
                p1.Controls.Add(p2);

                int mid = Convert.ToInt32(dt1.Rows[i]["MainID"]);
                string qry2 = "SELECT p.pName, d.qty FROM tblDetails d INNER JOIN products p ON p.pID=d.proID WHERE d.MainID=@ID";
                Hashtable ht2 = new Hashtable();
                ht2.Add("@ID", mid);
                DataTable dt2 = MainClass.GetDataTable(qry2, ht2);

                foreach (DataRow row in dt2.Rows)
                {
                    p1.Controls.Add(new Label { ForeColor = Color.Black, AutoSize = true, Text = row["pName"] + " x" + row["qty"], Margin = new Padding(10, 5, 3, 0) });
                }

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.Size      = new Size(150, 35);
                b.FillColor = Color.FromArgb(241, 85, 126);
                b.Margin    = new Padding(35, 10, 3, 10);
                b.Text      = loc.GetString("Mark_Served");
                b.Tag       = dt1.Rows[i]["MainID"].ToString();
                b.Click += (ss, ee) =>
                {
                    int id = Convert.ToInt32((ss as Guna.UI2.WinForms.Guna2Button).Tag);
                    string qry = "UPDATE tblMain SET status='Served' WHERE MainID=@ID";
                    Hashtable ht = new Hashtable();
                    ht.Add("@ID", id);
                    if (MainClass.Sql(qry, ht) > 0)
                        GetReadyOrders();
                };
                p1.Controls.Add(b);
                flowLayoutPanel1.Controls.Add(p1);
            }
        }
    }
}
