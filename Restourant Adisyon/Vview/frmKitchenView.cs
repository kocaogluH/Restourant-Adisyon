using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;

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
            LocalizationService.Instance.OnLanguageChanged += (s, ev) => GetOrders();
            GetOrders();

            // Her 30 saniyede otomatik yenile
            Timer t = new Timer { Interval = 30000 };
            t.Tick += (s, ev) => GetOrders();
            t.Start();
        }

        private void GetOrders()
        {
            flowLayoutPanel1.Controls.Clear();

            string qry1 = "SELECT * FROM tblMain WHERE status IN ('Pending','Cooking') ORDER BY MainID ASC";
            DataTable dt1 = MainClass.GetDataTable(qry1);

            var loc = LocalizationService.Instance;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string currentStatus = dt1.Rows[i]["status"].ToString();

                FlowLayoutPanel p1 = new FlowLayoutPanel
                {
                    AutoSize = true, Width = 230, Height = 350,
                    FlowDirection = FlowDirection.TopDown,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    BackColor = currentStatus == "Cooking"
                        ? Color.FromArgb(255, 253, 210)
                        : Color.White
                };

                FlowLayoutPanel p2 = new FlowLayoutPanel
                {
                    BackColor = Color.FromArgb(50, 55, 89),
                    AutoSize  = true, Width = 230, Height = 125,
                    FlowDirection = FlowDirection.TopDown,
                    Margin = Padding.Empty
                };

                p2.Controls.Add(new Label { ForeColor = Color.White, Margin = new Padding(10, 10, 3, 0), AutoSize = true, Text = loc.GetString("Table") + " : " + dt1.Rows[i]["TableName"], Font = new Font("Arial", 10, FontStyle.Bold) });
                p2.Controls.Add(new Label { ForeColor = Color.White, Margin = new Padding(10,  5, 3, 0), AutoSize = true, Text = loc.GetString("Waiter") + " : " + dt1.Rows[i]["WaiterName"] });
                p2.Controls.Add(new Label { ForeColor = Color.White, Margin = new Padding(10,  5, 3, 0), AutoSize = true, Text = loc.GetString("Time") + " : " + dt1.Rows[i]["aTime"] });
                p2.Controls.Add(new Label { ForeColor = Color.FromArgb(241, 85, 126), Margin = new Padding(10, 5, 3, 0), AutoSize = true, Text = "Tip : " + dt1.Rows[i]["orderType"] });
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
                b.Margin    = new Padding(35, 10, 3, 10);
                b.Tag       = dt1.Rows[i]["MainID"].ToString();

                if (currentStatus == "Pending")
                {
                    b.Text      = loc.GetString("Start_Cooking");
                    b.FillColor = Color.FromArgb(255, 153, 51);
                }
                else
                {
                    b.Text      = loc.GetString("Mark_Ready");
                    b.FillColor = Color.FromArgb(241, 85, 126);
                }

                b.Click += (ss, ee) =>
                {
                    int id = Convert.ToInt32((ss as Guna.UI2.WinForms.Guna2Button).Tag);
                    string nextStatus = currentStatus == "Pending" ? "Cooking" : "Ready";
                    string qry = "UPDATE tblMain SET status=@st WHERE MainID=@ID";
                    Hashtable ht = new Hashtable();
                    ht.Add("@st", nextStatus);
                    ht.Add("@ID", id);
                    if (MainClass.Sql(qry, ht) > 0)
                        GetOrders();
                };
                p1.Controls.Add(b);
                flowLayoutPanel1.Controls.Add(p1);
            }
        }
    }
}
