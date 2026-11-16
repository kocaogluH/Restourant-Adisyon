using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.UI.Theme;

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
            ApplyTheme();
            LocalizationService.Instance.OnLanguageChanged += (s, ev) => GetReadyOrders();
            ThemeManager.Instance.OnThemeChanged += (s, ev) => ApplyTheme();
            GetReadyOrders();

            MainClass.EnableEnterKeyConfirmation(this, () => GetReadyOrders());
        }

        private void ApplyTheme()
        {
            this.BackColor = AppTheme.Background;
            if (label1 != null)
            {
                label1.Text = "🔔 Garson Servis Terminali - Hazır Siparişler";
                label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                label1.ForeColor = AppTheme.TextPrimary;
            }
        }

        private void GetReadyOrders()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.BackColor = Color.Transparent;

            string qry1 = "SELECT * FROM tblMain WHERE status = 'Ready' ORDER BY MainID ASC";
            DataTable dt1 = MainClass.GetDataTable(qry1);
            var loc = LocalizationService.Instance;

            if (dt1.Rows.Count == 0)
            {
                // Empty state kartı
                Guna2Panel emptyPanel = new Guna2Panel
                {
                    Size = new Size(500, 160),
                    FillColor = AppTheme.CardBackground,
                    BorderRadius = 14,
                    BorderColor = AppTheme.BorderColor,
                    BorderThickness = 1,
                    Margin = new Padding(20)
                };

                Label lblEmpty = new Label
                {
                    Text = "🎉 Tebrikler! Servise hazır bekleyen sipariş bulunmuyor.\nMutfak siparişleri tamamladığında bu ekranda otomatik listelenecektir.",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = AppTheme.TextSecondary,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                emptyPanel.Controls.Add(lblEmpty);
                flowLayoutPanel1.Controls.Add(emptyPanel);
                return;
            }

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                int mid = Convert.ToInt32(dt1.Rows[i]["MainID"]);
                string tableName = dt1.Rows[i]["TableName"].ToString();
                string waiterName = dt1.Rows[i]["WaiterName"].ToString();

                Guna2Panel card = new Guna2Panel
                {
                    Width = 270,
                    AutoSize = true,
                    FillColor = AppTheme.CardBackground,
                    BorderRadius = 14,
                    BorderColor = Color.FromArgb(16, 185, 129), // Emerald
                    BorderThickness = 2,
                    Margin = new Padding(12),
                    Padding = new Padding(12)
                };

                FlowLayoutPanel inner = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    BackColor = Color.Transparent
                };
                card.Controls.Add(inner);

                // Kart Üst Başlık
                Label lblTable = new Label
                {
                    Text = $"🪑 {tableName}",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(16, 185, 129),
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 2)
                };
                inner.Controls.Add(lblTable);

                Label lblWaiter = new Label
                {
                    Text = $"👤 Garson: {waiterName}  |  Adisyon #{mid}",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = AppTheme.TextSecondary,
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 10)
                };
                inner.Controls.Add(lblWaiter);

                // Çizgi
                Guna2Separator sep = new Guna2Separator
                {
                    Size = new Size(240, 10),
                    FillColor = AppTheme.BorderColor
                };
                inner.Controls.Add(sep);

                // Ürünler
                string qry2 = "SELECT p.pName, d.qty FROM tblDetails d INNER JOIN products p ON p.pID=d.proID WHERE d.MainID=@ID";
                Hashtable ht2 = new Hashtable();
                ht2.Add("@ID", mid);
                DataTable dt2 = MainClass.GetDataTable(qry2, ht2);

                foreach (DataRow row in dt2.Rows)
                {
                    Label lblItem = new Label
                    {
                        Text = $"🍽️  {row["pName"]}  (x{row["qty"]})",
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        ForeColor = AppTheme.TextPrimary,
                        AutoSize = true,
                        Margin = new Padding(0, 4, 0, 4)
                    };
                    inner.Controls.Add(lblItem);
                }

                // Servis Edildi Butonu
                Guna2Button btnServed = new Guna2Button
                {
                    Text = "✅ Masaya Servis Edildi",
                    Size = new Size(240, 38),
                    BorderRadius = 8,
                    FillColor = Color.FromArgb(16, 185, 129), // Emerald
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Margin = new Padding(0, 14, 0, 6),
                    Cursor = Cursors.Hand,
                    Tag = mid
                };

                btnServed.Click += (ss, ee) =>
                {
                    int id = Convert.ToInt32((ss as Guna2Button).Tag);
                    string qry = "UPDATE tblMain SET status='Served' WHERE MainID=@ID";
                    Hashtable ht = new Hashtable();
                    ht.Add("@ID", id);
                    if (MainClass.Sql(qry, ht) > 0)
                        GetReadyOrders();
                };

                inner.Controls.Add(btnServed);
                flowLayoutPanel1.Controls.Add(card);
            }
        }
    }
}
