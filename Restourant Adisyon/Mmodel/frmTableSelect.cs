using System;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmTableSelect : Form
    {
        public frmTableSelect()
        {
            InitializeComponent();
        }

        public string TableName = "";

        private void frmTableSelect_Load(object sender, EventArgs e)
        {
            string qry = "SELECT * FROM tables ORDER BY tName";
            DataTable dt = MainClass.GetDataTable(qry);

            flowLayoutPanel1.Controls.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string tname = row["tName"].ToString();
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.Text        = tname;
                b.Width       = 150;
                b.Height      = 150;
                b.BorderRadius = 10;
                b.FillColor   = IsTableBusy(tname)
                    ? Color.FromArgb(231, 76, 60)    // Kırmızı – dolu
                    : Color.FromArgb(46, 204, 113);   // Yeşil – boş
                b.HoverState.FillColor = Color.FromArgb(50, 55, 89);
                b.Click += new EventHandler(b_click);
                flowLayoutPanel1.Controls.Add(b);
            }
        }

        private bool IsTableBusy(string tableName)
        {
            string qry = "SELECT COUNT(*) FROM tblMain WHERE TableName=@t AND status='Pending'";
            Hashtable ht = new Hashtable();
            ht.Add("@t", tableName);
            object res = MainClass.SqlScalar(qry, ht);
            return res != null && Convert.ToInt64(res) > 0;
        }

        private void b_click(object sender, EventArgs e)
        {
            TableName = ((Guna.UI2.WinForms.Guna2Button)sender).Text;
            this.Close();
        }
    }
}
