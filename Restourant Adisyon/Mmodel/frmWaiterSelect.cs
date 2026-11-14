using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.UI.Theme;

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
            this.BackColor = AppTheme.Background;
            this.StartPosition = FormStartPosition.CenterScreen;

            if (label1 != null)
            {
                label1.Text = "👤 Garson Seçimi";
                label1.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
                label1.ForeColor = AppTheme.TextPrimary;
            }

            string qry = "SELECT * FROM staff WHERE sRole = 'Garson' OR sRole = 'Waiter' ORDER BY sName";
            DataTable dt = MainClass.GetDataTable(qry);

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.BackColor = Color.Transparent;

            foreach (DataRow row in dt.Rows)
            {
                string name = row["sName"].ToString();
                Guna2Button b = new Guna2Button
                {
                    Text = $"👤 {name}\nGarson",
                    Width = 170,
                    Height = 65,
                    BorderRadius = 12,
                    FillColor = AppTheme.CardBackground,
                    ForeColor = AppTheme.TextPrimary,
                    BorderColor = AppTheme.BorderColor,
                    BorderThickness = 1,
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    Margin = new Padding(8),
                    Cursor = Cursors.Hand
                };

                b.HoverState.FillColor = AppTheme.BrandPrimary;
                b.HoverState.ForeColor = Color.White;
                b.HoverState.BorderColor = AppTheme.BrandPrimary;

                b.Click += (s, ev) =>
                {
                    waiterName = name;
                    this.Close();
                };

                flowLayoutPanel1.Controls.Add(b);
            }
        }
    }
}
