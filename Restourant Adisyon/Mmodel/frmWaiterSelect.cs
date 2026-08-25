using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmWaiterSelect : Form
    {
        public frmWaiterSelect()
        {
            InitializeComponent();
        }

        public string waiterName = "";

        private void frmWaiterSelect_Load(object sender, EventArgs e)
        {
            string qry = "SELECT * FROM staff WHERE sRole = 'Garson' OR sRole = 'Waiter' ORDER BY sName";
            DataTable dt = MainClass.GetDataTable(qry);

            flowLayoutPanel1.Controls.Clear();

            foreach (DataRow row in dt.Rows)
            {
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.Text       = row["sName"].ToString();
                b.Width      = 150;
                b.Height     = 50;
                b.FillColor  = Color.FromArgb(241, 85, 126);
                b.HoverState.FillColor = Color.FromArgb(50, 55, 89);
                b.Click += new EventHandler(b_click);
                flowLayoutPanel1.Controls.Add(b);
            }
        }

        private void b_click(object sender, EventArgs e)
        {
            waiterName = ((Guna.UI2.WinForms.Guna2Button)sender).Text;
            this.Close();
        }
    }
}
