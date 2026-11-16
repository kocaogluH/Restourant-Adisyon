using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.Core.DTOs;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon.Vview
{
    public partial class frmKitchenView : Form
    {
        private Panel _pnlTopBar;
        private Label _lblStats;
        private Timer _refreshTimer;

        public frmKitchenView()
        {
            InitializeComponent();
        }

        private void frmKitchenView_Load(object sender, EventArgs e)
        {
            SetupKdsHeader();
            ApplyTheme();

            LocalizationService.Instance.OnLanguageChanged += (s, ev) => GetOrders();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => { ApplyTheme(); GetOrders(); };

            GetOrders();

            // Her 15 saniyede otomatik yenile
            _refreshTimer = new Timer { Interval = 15000 };
            _refreshTimer.Tick += (s, ev) => GetOrders();
            _refreshTimer.Start();

            MainClass.EnableEnterKeyConfirmation(this, () => GetOrders());
        }

        private void SetupKdsHeader()
        {
            this.BackColor = AppTheme.Background;

            _pnlTopBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = AppTheme.CardBackground,
                Padding = new Padding(16, 8, 16, 8)
            };
            this.Controls.Add(_pnlTopBar);
            this.Controls.SetChildIndex(_pnlTopBar, 0);

            _lblStats = new Label
            {
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "👨‍🍳 Mutfak Ekranı (KDS) | Aktif Siparişler Yükleniyor...",
                AutoSize = true
            };
            _pnlTopBar.Controls.Add(_lblStats);

            Guna2Button btnRefresh = new Guna2Button
            {
                Text = "🔄 Yenile",
                Dock = DockStyle.Right,
                Width = 90,
                AutoRoundedCorners = true,
                FillColor = AppTheme.Secondary,
                ForeColor = AppTheme.TextPrimary,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.Click += (s, e) => GetOrders();
            _pnlTopBar.Controls.Add(btnRefresh);

            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.BackColor = AppTheme.Background;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Padding = new Padding(12);
        }

        private void ApplyTheme()
        {
            this.BackColor = AppTheme.Background;
            if (_pnlTopBar != null)
            {
                _pnlTopBar.BackColor = AppTheme.CardBackground;
                _lblStats.ForeColor = AppTheme.TextPrimary;
            }
            if (flowLayoutPanel1 != null)
            {
                flowLayoutPanel1.BackColor = AppTheme.Background;
            }
        }

        private void GetOrders()
        {
            if (flowLayoutPanel1 == null) return;
            flowLayoutPanel1.Controls.Clear();

            List<KitchenOrderDto> orders = KitchenManager.Instance.GetActiveKitchenOrders();
            var loc = LocalizationService.Instance;

            int pendingCount = 0;
            int cookingCount = 0;

            foreach (var order in orders)
            {
                if (order.Status == "Pending") pendingCount++;
                else cookingCount++;

                bool isCooking = order.Status == "Cooking";
                bool isUrgent = order.ElapsedMinutes > 20;

                Color headerColor = isUrgent
                    ? AppTheme.Danger
                    : (isCooking ? AppTheme.Warning : AppTheme.Info);

                // Ana Fiş Kartı
                Guna2Panel card = new Guna2Panel
                {
                    Width = 250,
                    Height = 360,
                    BorderRadius = 14,
                    BorderThickness = 2,
                    BorderColor = headerColor,
                    FillColor = AppTheme.CardBackground,
                    Margin = new Padding(10)
                };

                // Fiş Başlığı
                Guna2Panel headerPanel = new Guna2Panel
                {
                    Dock = DockStyle.Top,
                    Height = 90,
                    FillColor = headerColor,
                    BorderRadius = 14,
                    Padding = new Padding(12, 10, 12, 6)
                };

                Label lblTable = new Label
                {
                    Text = order.TableName,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(10, 8),
                    AutoSize = true
                };
                headerPanel.Controls.Add(lblTable);

                Label lblType = new Label
                {
                    Text = $"[{order.OrderType}]",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(240, 240, 240),
                    Location = new Point(170, 10),
                    AutoSize = true
                };
                headerPanel.Controls.Add(lblType);

                Label lblDetails = new Label
                {
                    Text = $"Garson: {order.WaiterName}\nSaat: {order.OrderTime}",
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.White,
                    Location = new Point(10, 36),
                    AutoSize = true
                };
                headerPanel.Controls.Add(lblDetails);

                Label lblElapsed = new Label
                {
                    Text = isUrgent ? $"⏱️ {order.ElapsedMinutes} dk (ACİL!)" : $"⏱️ {order.ElapsedMinutes} dk",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = isUrgent ? Color.Yellow : Color.White,
                    Location = new Point(140, 48),
                    AutoSize = true
                };
                headerPanel.Controls.Add(lblElapsed);

                card.Controls.Add(headerPanel);

                // Kalemler Listesi Paneli
                FlowLayoutPanel itemsPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Padding = new Padding(8, 8, 8, 8),
                    BackColor = Color.Transparent
                };
                card.Controls.Add(itemsPanel);
                card.Controls.SetChildIndex(itemsPanel, 0);

                foreach (var item in order.Items)
                {
                    CheckBox chk = new CheckBox
                    {
                        Text = $"{item.Quantity}x {item.ProductName}",
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        ForeColor = AppTheme.TextPrimary,
                        AutoSize = true,
                        Margin = new Padding(4, 4, 4, 4),
                        Cursor = Cursors.Hand
                    };
                    chk.CheckedChanged += (s, e) =>
                    {
                        chk.Font = new Font("Segoe UI", 10F, chk.Checked ? FontStyle.Strikeout : FontStyle.Bold);
                        chk.ForeColor = chk.Checked ? AppTheme.TextSecondary : AppTheme.TextPrimary;
                    };
                    itemsPanel.Controls.Add(chk);
                }

                // Alt Eylem Butonu
                Guna2Button btnAction = new Guna2Button
                {
                    Dock = DockStyle.Bottom,
                    Height = 42,
                    BorderRadius = 10,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand,
                    Animated = true
                };

                if (order.Status == "Pending")
                {
                    btnAction.Text = "👨‍🍳 Hazırlamaya Başla";
                    btnAction.FillColor = AppTheme.Warning;
                }
                else
                {
                    btnAction.Text = "🔔 Hazır! (Garsona Bildir)";
                    btnAction.FillColor = AppTheme.Success;
                }

                btnAction.Click += (s, e) =>
                {
                    KitchenManager.Instance.AdvanceOrderStatus(order.OrderId, order.Status);
                    GetOrders();
                };

                card.Controls.Add(btnAction);

                flowLayoutPanel1.Controls.Add(card);
            }

            if (_lblStats != null)
            {
                _lblStats.Text = $"👨‍🍳 Mutfak Ekranı (KDS) | Toplam: {orders.Count} Sipariş (🟡 {pendingCount} Bekliyor, 🟠 {cookingCount} Hazırlanıyor)";
            }

            if (orders.Count == 0)
            {
                Label emptyLbl = new Label
                {
                    Text = "Şu anda bekleyen mutfak siparişi bulunmuyor. Tüm siparişler hazır! 🎉",
                    Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    ForeColor = AppTheme.Success,
                    AutoSize = true,
                    Margin = new Padding(30)
                };
                flowLayoutPanel1.Controls.Add(emptyLbl);
            }
        }
    }
}
