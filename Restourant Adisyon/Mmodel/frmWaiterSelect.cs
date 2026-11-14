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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmWaiterSelect_Load(object sender, EventArgs e)
        {
            this.BackColor = AppTheme.Background;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Escape) this.Close(); };

            if (pictureBox1 != null)
                pictureBox1.Visible = false;

            if (guna2Panel1 != null)
            {
                guna2Panel1.FillColor = Color.FromArgb(15, 23, 42); // Obsidian Slate
                guna2Panel1.Height = 76;
            }

            if (label1 != null)
            {
                label1.Text = "👤 Garson Seçimi";
                label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                label1.ForeColor = Color.White;
                label1.Location = new Point(24, 22);
            }

            if (btnExit != null)
            {
                btnExit.Location = new Point(guna2Panel1.Width - 52, 20);
                btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            }

            string qry = "SELECT * FROM staff WHERE sRole = 'Garson' OR sRole = 'Waiter' ORDER BY sName";
            DataTable dt = MainClass.GetDataTable(qry);

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.BackColor = Color.Transparent;

            if (dt == null || dt.Rows.Count == 0)
            {
                Panel pnlEmpty = new Panel
                {
                    Size = new Size(flowLayoutPanel1.Width - 40, 240),
                    BackColor = Color.Transparent
                };

                Label lblEmptyIcon = new Label
                {
                    Text = "👤",
                    Font = new Font("Segoe UI", 38F),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 70
                };

                Label lblEmptyMsg = new Label
                {
                    Text = "Henüz 'Garson' rolünde personel bulunamadı.\nLütfen 'Personel' menüsünden garson tanımlayınız.",
                    Font = new Font("Segoe UI", 11.5F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(100, 116, 139),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };

                pnlEmpty.Controls.Add(lblEmptyMsg);
                pnlEmpty.Controls.Add(lblEmptyIcon);
                flowLayoutPanel1.Controls.Add(pnlEmpty);
                return;
            }

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
