using System;
using System.Collections;
using System.Data;
using System.Drawing;
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

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string currentStatus = dt1.Rows[i]["status"].ToString();

                FlowLayoutPanel p1 = new FlowLayoutPanel
                {
                    AutoSize = true, Width = 230, Height = 350,
                    FlowDirection = FlowDirection.TopDown,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new System.Windows.Forms.Padding(10),
                    BackColor = currentStatus == "Cooking"
                        ? Color.FromArgb(255, 253, 210)
                        : Color.White
                };

                FlowLayoutPanel p2 = new FlowLayoutPanel
                {
                    BackColor = Color.FromArgb(50, 55, 89),
                    AutoSize  = true, Width = 230, Height = 125,
                    FlowDirection = FlowDirection.TopDown,
                    Margin = System.Windows.Forms.Padding.Empty
                };

                p2.Controls.Add(new Label { ForeColor = Color.White, Margin = new System.Windows.Forms.Padding(10, 10, 3, 0), AutoSize = true, Text = "Masa : " + dt1.Rows[i]["TableName"], Font = new Font("Arial", 10, FontStyle.Bold) });
                p2.Controls.Add(new Label { ForeColor = Color.White, Margin = new System.Windows.Forms.Padding(10,  5, 3, 0), AutoSize = true, Text = "Garson : " + dt1.Rows[i]["WaiterName"] });
                p2.Controls.Add(new Label { ForeColor = Color.White, Margin = new System.Windows.Forms.Padding(10,  5, 3, 0), AutoSize = true, Text = "Saat : " + dt1.Rows[i]["aTime"] });
                p2.Controls.Add(new Label { ForeColor = Color.FromArgb(241, 85, 126), Margin = new System.Windows.Forms.Padding(10, 5, 3, 0), AutoSize = true, Text = "Tip : " + dt1.Rows[i]["orderType"] });
                p1.Controls.Add(p2);

                int mid = Convert.ToInt32(dt1.Rows[i]["MainID"]);
                string qry2 = "SELECT p.pName, d.qty FROM tblDetails d INNER JOIN products p ON p.pID=d.proID WHERE d.MainID=@ID";
                Hashtable ht = new Hashtable();
                ht.Add("@ID", mid);
                DataTable dt2 = MainClass.GetDataTable(qry2, ht);

                foreach (DataRow row in dt2.Rows)
                {
                    p1.Controls.Add(new Label
                    {
                        ForeColor = Color.Black,
                        Margin    = new System.Windows.Forms.Padding(10, 5, 3, 0),
                        AutoSize  = true,
                        Text      = row["pName"] + "  ×" + row["qty"]
                    });
                }

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button
                {
                    AutoRoundedCorners = true,
                    Size   = new Size(150, 35),
                    Margin = new System.Windows.Forms.Padding(35, 10, 3, 10),
                    Tag    = dt1.Rows[i]["MainID"].ToString()
                };

                if (currentStatus == "Pending")
                { b.Text = "Pişirmeye Başla"; b.FillColor = Color.FromArgb(52, 152, 219); }
                else
                { b.Text = "Hazır!"; b.FillColor = Color.FromArgb(46, 204, 113); }

                b.Click += new EventHandler(b_click);
                p1.Controls.Add(b);
                flowLayoutPanel1.Controls.Add(p1);
            }
        }

        private void b_click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            int id = Convert.ToInt32(btn.Tag);
            string nextStatus = btn.Text == "Pişirmeye Başla" ? "Cooking" : "Ready";

            string qry = "UPDATE tblMain SET status=@status WHERE MainID=@ID";
            Hashtable ht = new Hashtable();
            ht.Add("@ID",     id);
            ht.Add("@status", nextStatus);

            if (MainClass.Sql(qry, ht) > 0)
                GetOrders();
        }
    }
}
