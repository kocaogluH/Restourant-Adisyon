using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.Core.DTOs;
using Restourant_Adisyon.Core.Enums;
using Restourant_Adisyon.Mmodel;
using Restourant_Adisyon.UI.Controls;
using Restourant_Adisyon.UI.Theme;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon.Vview
{
    public partial class frmTableView : SampleView
    {
        private FlowLayoutPanel _flpTableMap;
        private Panel _pnlMapHeader;
        private Guna2Button _btnToggleView;
        private Guna2Button _btnFilterAll;
        private Guna2Button _btnFilterEmpty;
        private Guna2Button _btnFilterOccupied;

        private bool _isMapMode = true;
        private TableStatus? _currentFilter = null;

        public frmTableView()
        {
            InitializeComponent();
        }

        private void frmTableView_Load(object sender, EventArgs e)
        {
            SetupMapUi();

            if (guna2DataGridView1 != null)
                GridStyler.Apply(guna2DataGridView1, "Henüz masa eklenmemiş. Eklemek için '+' butonuna tıklayın.");

            ApplyThemeAndLocalization();

            LocalizationService.Instance.OnLanguageChanged += (s, ev) => ApplyThemeAndLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyThemeAndLocalization();

            GetData();
            RenderTableMap();
        }

        private void SetupMapUi()
        {
            // Başlık Çubuğu & Filtreler
            _pnlMapHeader = new Panel
            {
                Location = new Point(21, 130),
                Size = new Size(this.Width - 42, 44),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.Transparent
            };
            this.Controls.Add(_pnlMapHeader);

            // Görünüm Değiştir Butonu (Harita / Liste)
            _btnToggleView = new Guna2Button
            {
                Text = "📋 Liste Görünümü",
                Size = new Size(140, 36),
                Location = new Point(0, 4),
                AutoRoundedCorners = true,
                FillColor = AppTheme.BrandSecondary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Animated = true,
                Cursor = Cursors.Hand
            };
            _btnToggleView.Click += (s, e) =>
            {
                _isMapMode = !_isMapMode;
                _btnToggleView.Text = _isMapMode ? "📋 Liste Görünümü" : "🪑 Kroki Haritası";
                guna2DataGridView1.Visible = !_isMapMode;
                _flpTableMap.Visible = _isMapMode;
                if (_isMapMode) RenderTableMap();
            };
            _pnlMapHeader.Controls.Add(_btnToggleView);

            // Filtre Butonları
            _btnFilterAll = CreateFilterButton("Tümü", 150, () => { _currentFilter = null; RenderTableMap(); });
            _btnFilterEmpty = CreateFilterButton("🟢 Boş Masalar", 260, () => { _currentFilter = TableStatus.Empty; RenderTableMap(); });
            _btnFilterOccupied = CreateFilterButton("🔴 Dolu Masalar", 380, () => { _currentFilter = TableStatus.Occupied; RenderTableMap(); });

            _pnlMapHeader.Controls.Add(_btnFilterAll);
            _pnlMapHeader.Controls.Add(_btnFilterEmpty);
            _pnlMapHeader.Controls.Add(_btnFilterOccupied);

            // Görsel Masa Haritası Paneli
            _flpTableMap = new FlowLayoutPanel
            {
                Location = new Point(21, 185),
                Size = new Size(this.Width - 42, this.Height - 200),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(8)
            };
            this.Controls.Add(_flpTableMap);

            guna2DataGridView1.Visible = false;
        }

        private Guna2Button CreateFilterButton(string text, int x, Action onClick)
        {
            Guna2Button btn = new Guna2Button
            {
                Text = text,
                Size = new Size(110, 36),
                Location = new Point(x, 4),
                AutoRoundedCorners = true,
                FillColor = AppTheme.Secondary,
                ForeColor = AppTheme.TextPrimary,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Animated = true,
                Cursor = Cursors.Hand
            };
            btn.Click += (s, e) => onClick();
            return btn;
        }

        private void ApplyThemeAndLocalization()
        {
            this.BackColor = AppTheme.Background;
            if (label2 != null)
            {
                label2.Text = LocalizationService.Instance.GetString("Table_Header");
                label2.ForeColor = AppTheme.BrandPrimary;
            }
            if (label1 != null)
            {
                label1.Text = LocalizationService.Instance.GetString("Search");
                label1.ForeColor = AppTheme.TextSecondary;
            }
            if (txtSearch != null)
            {
                txtSearch.FillColor = AppTheme.CardBackground;
                txtSearch.ForeColor = AppTheme.TextPrimary;
                txtSearch.BorderColor = AppTheme.BorderColor;
            }
            if (guna2DataGridView1 != null)
            {
                GridStyler.Apply(guna2DataGridView1, "Cart_Empty");
            }
        }

        private void RenderTableMap()
        {
            if (_flpTableMap == null) return;
            _flpTableMap.Controls.Clear();

            string search = txtSearch != null ? txtSearch.Text.Trim() : "";
            List<TableCardDto> tables = TableManager.Instance.GetAllTableCards(search, _currentFilter);

            foreach (var t in tables)
            {
                bool isBusy = t.Status == TableStatus.Occupied;
                Color statusColor = isBusy ? AppTheme.Danger : AppTheme.Success;
                string statusText = isBusy ? "DOLU" : "BOŞ";

                Guna2Panel card = new Guna2Panel
                {
                    Size = new Size(175, 155),
                    Margin = new Padding(10),
                    BorderRadius = 14,
                    BorderThickness = 2,
                    BorderColor = statusColor,
                    FillColor = AppTheme.CardBackground,
                    Cursor = Cursors.Hand
                };

                // Masa Adı
                Label lblName = new Label
                {
                    Text = t.TableName,
                    Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    ForeColor = AppTheme.TextPrimary,
                    Location = new Point(12, 12),
                    Size = new Size(150, 26),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                card.Controls.Add(lblName);

                // Kapasite
                Label lblCap = new Label
                {
                    Text = "👥 " + t.Capacity + " Kişilik",
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = AppTheme.TextSecondary,
                    Location = new Point(14, 40),
                    Size = new Size(140, 18)
                };
                card.Controls.Add(lblCap);

                // Durum Rozeti
                Guna2Button badge = new Guna2Button
                {
                    Text = statusText,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    FillColor = statusColor,
                    ForeColor = Color.White,
                    Size = new Size(65, 24),
                    Location = new Point(14, 62),
                    AutoRoundedCorners = true
                };
                card.Controls.Add(badge);

                if (isBusy)
                {
                    // Tutar
                    Label lblAmt = new Label
                    {
                        Text = t.ActiveBillAmount.ToString("N2") + " ₺",
                        Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                        ForeColor = AppTheme.BrandPrimary,
                        Location = new Point(12, 92),
                        Size = new Size(150, 24)
                    };
                    card.Controls.Add(lblAmt);

                    // Garson & Geçen Süre
                    Label lblDetail = new Label
                    {
                        Text = $"⏱️ {t.ElapsedMinutes} dk | {t.WaiterName}",
                        Font = new Font("Segoe UI", 8F),
                        ForeColor = AppTheme.TextSecondary,
                        Location = new Point(14, 120),
                        Size = new Size(150, 20)
                    };
                    card.Controls.Add(lblDetail);
                }
                else
                {
                    Label lblFree = new Label
                    {
                        Text = "Sipariş Açmak İçin Tıklayın ➜",
                        Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                        ForeColor = AppTheme.TextSecondary,
                        Location = new Point(14, 105),
                        Size = new Size(150, 36)
                    };
                    card.Controls.Add(lblFree);
                }

                // Tıklama Eylemi: Masayı doğrudan POS'a aktar
                EventHandler openPos = (s, e) =>
                {
                    frmPOS pos = new frmPOS();
                    pos.OrderType = "Masada";
                    formMain.Instance.AddControls(pos);
                };

                card.Click += openPos;
                lblName.Click += openPos;

                _flpTableMap.Controls.Add(card);
            }

            if (tables.Count == 0)
            {
                Label emptyLbl = new Label
                {
                    Text = "Kriterlere uygun masa bulunamadı. Yeni masa eklemek için '+' butonuna tıklayın.",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = AppTheme.TextSecondary,
                    AutoSize = true,
                    Margin = new Padding(20)
                };
                _flpTableMap.Controls.Add(emptyLbl);
            }
        }

        public void GetData()
        {
            string qry = "SELECT tID, tName FROM tables WHERE tName LIKE @search ORDER BY tName";
            Hashtable ht = new Hashtable();
            ht.Add("@search", "%" + txtSearch.Text.Trim() + "%");
            DataTable dt = MainClass.GetDataTable(qry, ht);

            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);

            guna2DataGridView1.CellFormatting -= gv_Cell;
            guna2DataGridView1.CellFormatting += gv_Cell;

            for (int i = 0; i < lb.Items.Count; i++)
                guna2DataGridView1.Columns[((DataGridViewColumn)lb.Items[i]).Name].DataPropertyName = dt.Columns[i].ColumnName;
            guna2DataGridView1.DataSource = dt;
        }

        private void gv_Cell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmTableAdd());
            GetData();
            RenderTableMap();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
            RenderTableMap();
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                frmTableAdd frm = new frmTableAdd();
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                frm.txtName.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                MainClass.BlurBackground(frm);
                GetData();
                RenderTableMap();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                if (guna2MessageDialog1.Show("Bu masayı silmek istediğinize emin misiniz?") == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                    string qry = "DELETE FROM tables WHERE tID=" + id;
                    Hashtable ht = new Hashtable();
                    MainClass.Sql(qry, ht);
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Masa başarıyla silindi.");
                    GetData();
                    RenderTableMap();
                }
            }
        }
    }
}
