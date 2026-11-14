using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.Core.DTOs;
using Restourant_Adisyon.UI.Controls;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon
{
    public partial class frmHome : Form
    {
        private Panel _pnlContainer;
        private FlowLayoutPanel _pnlCards;
        private Panel _pnlQuickActions;
        private TableLayoutPanel _pnlBottomSplit;
        private Guna2Panel _pnlRecentOrders;
        private Guna2Panel _pnlTopSelling;
        private DataGridView _dgvRecentOrders;
        private FlowLayoutPanel _flpTopProducts;

        public frmHome()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            // Orijinal kontrolleri modern dinamik container içine al
            SetupModernDashboardLayout();

            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyThemeStyles();

            ApplyThemeStyles();
            ApplyLocalization();
            LoadDashboardData();

            // Her 20 saniyede canlı veri güncellemesi
            Timer autoRefreshTimer = new Timer { Interval = 20000 };
            autoRefreshTimer.Tick += (s, ev) => LoadDashboardData();
            autoRefreshTimer.Start();
        }

        private void SetupModernDashboardLayout()
        {
            this.Controls.Clear();
            this.AutoScroll = true;
            this.BackColor = AppTheme.Background;

            _pnlContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(24, 20, 24, 20),
                BackColor = AppTheme.Background
            };
            this.Controls.Add(_pnlContainer);

            // ── 1. ÜST KPI KARTLARI PANELİ (FlowLayout) ──────────────────────────
            _pnlCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Margin = new Padding(0, 0, 0, 16),
                BackColor = Color.Transparent
            };
            _pnlContainer.Controls.Add(_pnlCards);

            // 4 Kartı Yeniden Boyutlandır ve Şekillendir
            ConfigureKpiCard(guna2Panel1, label2, lblRevenue, "💰", AppTheme.BrandPrimary);
            ConfigureKpiCard(guna2Panel2, label4, lblOrders, "🧾", AppTheme.Info);
            ConfigureKpiCard(guna2Panel3, label6, lblProducts, "🍽️", AppTheme.Success);
            ConfigureKpiCard(guna2Panel4, label8, lblActiveTables, "🪑", AppTheme.Warning);

            _pnlCards.Controls.Add(guna2Panel1);
            _pnlCards.Controls.Add(guna2Panel2);
            _pnlCards.Controls.Add(guna2Panel3);
            _pnlCards.Controls.Add(guna2Panel4);

            // ── 2. HIZLI EYLEMLER ÇUBUĞU (Quick Actions) ─────────────────────────
            _pnlQuickActions = new Panel
            {
                Dock = DockStyle.Top,
                Height = 62,
                Padding = new Padding(0, 10, 0, 10),
                BackColor = Color.Transparent
            };
            _pnlContainer.Controls.Add(_pnlQuickActions);
            _pnlContainer.Controls.SetChildIndex(_pnlQuickActions, 0); // Üst kartların altına

            CreateQuickActionButtons();

            // ── 3. ALT BÖLÜM: 2 SÜTUNLU CANLI PANEL (Recent Orders & Top Products) ──
            _pnlBottomSplit = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 380,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 10, 0, 20)
            };
            _pnlBottomSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            _pnlBottomSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            _pnlContainer.Controls.Add(_pnlBottomSplit);
            _pnlContainer.Controls.SetChildIndex(_pnlBottomSplit, 0);

            // Sol: Son Siparişler Kartı
            _pnlRecentOrders = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 14,
                BorderThickness = 1,
                Margin = new Padding(0, 0, 12, 0),
                Padding = new Padding(16)
            };
            BuildRecentOrdersSection();
            _pnlBottomSplit.Controls.Add(_pnlRecentOrders, 0, 0);

            // Sağ: En Çok Satanlar Kartı
            _pnlTopSelling = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 14,
                BorderThickness = 1,
                Margin = new Padding(12, 0, 0, 0),
                Padding = new Padding(16)
            };
            BuildTopSellingSection();
            _pnlBottomSplit.Controls.Add(_pnlTopSelling, 1, 0);
        }

        private void ConfigureKpiCard(Guna2Panel card, Label titleLbl, Label valueLbl, string icon, Color accentColor)
        {
            if (card == null) return;

            card.Size = new Size(240, 110);
            card.Margin = new Padding(0, 0, 16, 16);
            card.BorderRadius = 16;
            card.BorderThickness = 1;

            if (titleLbl != null)
            {
                titleLbl.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                titleLbl.Location = new Point(16, 14);
                titleLbl.AutoSize = true;
            }

            if (valueLbl != null)
            {
                valueLbl.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
                valueLbl.Location = new Point(14, 46);
                valueLbl.Size = new Size(210, 42);
                valueLbl.TextAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void CreateQuickActionButtons()
        {
            _pnlQuickActions.Controls.Clear();

            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent
            };
            _pnlQuickActions.Controls.Add(flp);

            var actions = new[]
            {
                new { Title = "⚡ Hızlı Satış (POS)", Color = AppTheme.BrandPrimary, Action = (Action)(() => formMain.Instance.AddControls(new Mmodel.frmPOS())) },
                new { Title = "🪑 Masalar & Kroki", Color = AppTheme.Info,         Action = (Action)(() => formMain.Instance.AddControls(new Vview.frmTableView())) },
                new { Title = "👨‍🍳 Mutfak (KDS)",    Color = AppTheme.Warning,      Action = (Action)(() => formMain.Instance.AddControls(new Vview.frmKitchenView())) },
                new { Title = "📦 Stok & Reçete",   Color = AppTheme.Success,      Action = (Action)(() => formMain.Instance.AddControls(new frmInventory())) }
            };

            foreach (var act in actions)
            {
                Guna2Button btn = new Guna2Button
                {
                    Text = act.Title,
                    Size = new Size(160, 38),
                    Margin = new Padding(0, 0, 12, 0),
                    AutoRoundedCorners = true,
                    FillColor = act.Color,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Animated = true,
                    Cursor = Cursors.Hand
                };
                btn.Click += (s, e) => act.Action();
                flp.Controls.Add(btn);
            }
        }

        private void BuildRecentOrdersSection()
        {
            _pnlRecentOrders.Controls.Clear();

            Label header = new Label
            {
                Text = "📋 " + LocalizationService.Instance.GetString("Recent_Orders"),
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _pnlRecentOrders.Controls.Add(header);

            _dgvRecentOrders = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = AppTheme.CardBackground,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            _pnlRecentOrders.Controls.Add(_dgvRecentOrders);
            _pnlRecentOrders.Controls.SetChildIndex(header, 0);

            GridStyler.Apply(_dgvRecentOrders, "Cart_Empty");
        }

        private void BuildTopSellingSection()
        {
            _pnlTopSelling.Controls.Clear();

            Label header = new Label
            {
                Text = "🏆 " + LocalizationService.Instance.GetString("Top_Selling_Leaderboard"),
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _pnlTopSelling.Controls.Add(header);

            _flpTopProducts = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 0)
            };
            _pnlTopSelling.Controls.Add(_flpTopProducts);
            _pnlTopSelling.Controls.SetChildIndex(header, 0);
        }

        private void ApplyThemeStyles()
        {
            this.BackColor = AppTheme.Background;
            if (_pnlContainer != null) _pnlContainer.BackColor = AppTheme.Background;

            // Kart Renkleri
            guna2Panel1.FillColor = AppTheme.CardBackground;
            guna2Panel1.BorderColor = AppTheme.BrandPrimary;
            label2.ForeColor = AppTheme.BrandPrimary;
            lblRevenue.ForeColor = AppTheme.TextPrimary;

            guna2Panel2.FillColor = AppTheme.CardBackground;
            guna2Panel2.BorderColor = AppTheme.Info;
            label4.ForeColor = AppTheme.Info;
            lblOrders.ForeColor = AppTheme.TextPrimary;

            guna2Panel3.FillColor = AppTheme.CardBackground;
            guna2Panel3.BorderColor = AppTheme.Success;
            label6.ForeColor = AppTheme.Success;
            lblProducts.ForeColor = AppTheme.TextPrimary;

            guna2Panel4.FillColor = AppTheme.CardBackground;
            guna2Panel4.BorderColor = AppTheme.Warning;
            label8.ForeColor = AppTheme.Warning;
            lblActiveTables.ForeColor = AppTheme.TextPrimary;

            // Alt Paneller
            if (_pnlRecentOrders != null)
            {
                _pnlRecentOrders.FillColor = AppTheme.CardBackground;
                _pnlRecentOrders.BorderColor = AppTheme.BorderColor;
            }
            if (_pnlTopSelling != null)
            {
                _pnlTopSelling.FillColor = AppTheme.CardBackground;
                _pnlTopSelling.BorderColor = AppTheme.BorderColor;
            }
            if (_dgvRecentOrders != null)
            {
                GridStyler.Apply(_dgvRecentOrders, "Cart_Empty");
            }
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label2 != null) label2.Text = loc.GetString("Total_Revenue");
            if (label4 != null) label4.Text = loc.GetString("Total_Orders");
            if (label6 != null) label6.Text = loc.GetString("Nav_Products");
            if (label8 != null) label8.Text = loc.GetString("Nav_Tables");
            CreateQuickActionButtons();
        }

        private void LoadDashboardData()
        {
            try
            {
                DashboardSummaryDto summary = DashboardManager.Instance.GetDashboardSummary();

                // KPI Verileri
                lblRevenue.Text      = summary.TotalRevenue.ToString("N2") + " ₺";
                lblOrders.Text       = summary.TotalOrders.ToString();
                lblProducts.Text     = summary.TotalProducts.ToString();
                lblActiveTables.Text = $"{summary.ActiveTablesCount} / {summary.TotalTablesCount} (%{summary.OccupancyRate:N0})";

                // Son Siparişler Tablosunu Doldur
                if (_dgvRecentOrders != null)
                {
                    var dt = new System.Data.DataTable();
                    dt.Columns.Add("MainID", typeof(int));
                    dt.Columns.Add("Masa", typeof(string));
                    dt.Columns.Add("Garson", typeof(string));
                    dt.Columns.Add("Saat", typeof(string));
                    dt.Columns.Add("Tutar", typeof(string));
                    dt.Columns.Add("Durum", typeof(string));

                    foreach (var o in summary.RecentOrders)
                    {
                        dt.Rows.Add(o.OrderId, o.TableName, o.WaiterName, o.OrderTime, o.TotalAmount.ToString("N2") + " ₺", o.Status);
                    }
                    _dgvRecentOrders.DataSource = dt;
                    if (_dgvRecentOrders.Columns.Contains("MainID"))
                        _dgvRecentOrders.Columns["MainID"].Visible = false;
                }

                // En Çok Satanlar Akışını Doldur
                if (_flpTopProducts != null)
                {
                    _flpTopProducts.Controls.Clear();
                    int rank = 1;
                    foreach (var p in summary.TopProducts)
                    {
                        Guna2Panel rowPnl = new Guna2Panel
                        {
                            Size = new Size(310, 52),
                            BorderRadius = 10,
                            BorderThickness = 1,
                            BorderColor = AppTheme.BorderColor,
                            FillColor = AppTheme.Secondary,
                            Margin = new Padding(0, 0, 0, 8),
                            Padding = new Padding(10, 8, 10, 8)
                        };

                        Label lblRank = new Label
                        {
                            Text = $"#{rank}",
                            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                            ForeColor = AppTheme.BrandPrimary,
                            Size = new Size(32, 34),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Location = new Point(8, 9)
                        };

                        Label lblName = new Label
                        {
                            Text = p.ProductName,
                            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                            ForeColor = AppTheme.TextPrimary,
                            Size = new Size(160, 20),
                            Location = new Point(42, 8),
                            AutoEllipsis = true
                        };

                        Label lblCat = new Label
                        {
                            Text = p.CategoryName,
                            Font = new Font("Segoe UI", 8F),
                            ForeColor = AppTheme.TextSecondary,
                            Size = new Size(160, 16),
                            Location = new Point(42, 28)
                        };

                        Label lblQty = new Label
                        {
                            Text = $"{p.TotalQuantitySold} Adet\n{p.TotalAmount:N0} ₺",
                            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                            ForeColor = AppTheme.Success,
                            Size = new Size(90, 36),
                            TextAlign = ContentAlignment.MiddleRight,
                            Location = new Point(210, 8)
                        };

                        rowPnl.Controls.Add(lblRank);
                        rowPnl.Controls.Add(lblName);
                        rowPnl.Controls.Add(lblCat);
                        rowPnl.Controls.Add(lblQty);

                        _flpTopProducts.Controls.Add(rowPnl);
                        rank++;
                    }

                    if (summary.TopProducts.Count == 0)
                    {
                        Label emptyLbl = new Label
                        {
                            Text = "Henüz satış kaydı bulunmuyor.",
                            ForeColor = AppTheme.TextSecondary,
                            Font = new Font("Segoe UI", 9.5F),
                            AutoSize = true,
                            Margin = new Padding(12)
                        };
                        _flpTopProducts.Controls.Add(emptyLbl);
                    }
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("frmHome.LoadDashboardData", ex);
            }
        }
    }
}
