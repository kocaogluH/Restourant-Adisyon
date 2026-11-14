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
        private FlowLayoutPanel _flpFilterPills;
        private Guna2Button _btnToggleView;

        private bool _isMapMode = true;
        private TableStatus? _currentStatusFilter = null;
        private string _currentZoneFilter = "Tümü";

        public frmTableView()
        {
            InitializeComponent();
        }

        private void frmTableView_Load(object sender, EventArgs e)
        {
            SetupConcept2MapUi();

            if (guna2DataGridView1 != null)
                GridStyler.Apply(guna2DataGridView1, "Henüz masa eklenmemiş. Eklemek için '+' butonuna tıklayın.");

            ApplyThemeAndLocalization();

            LocalizationService.Instance.OnLanguageChanged += (s, ev) => ApplyThemeAndLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, theme) => ApplyThemeAndLocalization();

            GetData();
            RenderTableMap();
        }

        private void SetupConcept2MapUi()
        {
            // Filtre Hapları Çubuğu (Konsept 2 Tarzı Bölge & Durum Filtreleri)
            _flpFilterPills = new FlowLayoutPanel
            {
                Location = new Point(21, 128),
                Size = new Size(this.Width - 42, 46),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            this.Controls.Add(_flpFilterPills);

            // Görünüm Değiştirici Buton
            _btnToggleView = new Guna2Button
            {
                Text = "📋 Liste Görünümü",
                Size = new Size(140, 36),
                Margin = new Padding(0, 4, 12, 0),
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
            _flpFilterPills.Controls.Add(_btnToggleView);

            // Filtre Hapları
            _flpFilterPills.Controls.Add(CreatePill("🌟 Tümü", true, () => { _currentStatusFilter = null; RenderTableMap(); }));
            _flpFilterPills.Controls.Add(CreatePill("🟢 Boş Masalar", false, () => { _currentStatusFilter = TableStatus.Empty; RenderTableMap(); }));
            _flpFilterPills.Controls.Add(CreatePill("🔴 Dolu Masalar", false, () => { _currentStatusFilter = TableStatus.Occupied; RenderTableMap(); }));
            _flpFilterPills.Controls.Add(CreatePill("🏛️ Ana Salon", false, () => { _currentZoneFilter = "Ana Salon"; RenderTableMap(); }));
            _flpFilterPills.Controls.Add(CreatePill("🌿 Teras", false, () => { _currentZoneFilter = "Teras"; RenderTableMap(); }));

            // İnteraktif Masa Krokisi Gridi (Konsept 2 Tarzı)
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

        private Guna2Button CreatePill(string text, bool isActive, Action onClick)
        {
            Guna2Button btn = new Guna2Button
            {
                Text = text,
                Size = new Size(115, 36),
                Margin = new Padding(0, 4, 8, 0),
                AutoRoundedCorners = true,
                FillColor = isActive ? AppTheme.BrandPrimary : AppTheme.Secondary,
                ForeColor = isActive ? Color.White : AppTheme.TextPrimary,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Animated = true,
                Cursor = Cursors.Hand
            };
            btn.Click += (s, e) =>
            {
                foreach (Control c in _flpFilterPills.Controls)
                {
                    if (c is Guna2Button b && b != _btnToggleView)
                    {
                        b.FillColor = AppTheme.Secondary;
                        b.ForeColor = AppTheme.TextPrimary;
                    }
                }
                btn.FillColor = AppTheme.BrandPrimary;
                btn.ForeColor = Color.White;
                onClick();
            };
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
            List<TableCardDto> tables = TableManager.Instance.GetAllTableCards(search, _currentStatusFilter);

            foreach (var t in tables)
            {
                bool isBusy = t.Status == TableStatus.Occupied;
                Color statusColor = isBusy ? AppTheme.Danger : AppTheme.Success;
                string statusText = isBusy ? "🔴 DOLU" : "🟢 MÜSAİT";

                // Konsept 2 Işıltılı Masa Kartı
                Guna2Panel card = new Guna2Panel
                {
                    Size = new Size(185, 165),
                    Margin = new Padding(12),
                    BorderRadius = 16,
                    BorderThickness = 2,
                    BorderColor = statusColor,
                    FillColor = AppTheme.CardBackground,
                    Cursor = Cursors.Hand
                };

                // Masa Başlığı
                Label lblName = new Label
                {
                    Text = t.TableName,
                    Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                    ForeColor = AppTheme.TextPrimary,
                    Location = new Point(14, 12),
                    Size = new Size(155, 26),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                card.Controls.Add(lblName);

                // Kapasite Rozeti
                Label lblCap = new Label
                {
                    Text = "👥 " + t.Capacity + " Kişilik Masalar",
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = AppTheme.TextSecondary,
                    Location = new Point(16, 40),
                    Size = new Size(150, 18)
                };
                card.Controls.Add(lblCap);

                // Durum Rozeti (Konsept 2 stili)
                Guna2Button badge = new Guna2Button
                {
                    Text = statusText,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    FillColor = statusColor,
                    ForeColor = Color.White,
                    Size = new Size(76, 24),
                    Location = new Point(16, 64),
                    AutoRoundedCorners = true
                };
                card.Controls.Add(badge);

                if (isBusy)
                {
                    // Tutar
                    Label lblAmt = new Label
                    {
                        Text = t.ActiveBillAmount.ToString("N2") + " ₺",
                        Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                        ForeColor = AppTheme.BrandPrimary,
                        Location = new Point(14, 96),
                        Size = new Size(155, 26)
                    };
                    card.Controls.Add(lblAmt);

                    // Garson & Geçen Süre
                    Label lblDetail = new Label
                    {
                        Text = $"⏱️ {t.ElapsedMinutes} dk | 👤 {t.WaiterName}",
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        ForeColor = AppTheme.TextSecondary,
                        Location = new Point(16, 126),
                        Size = new Size(155, 20)
                    };
                    card.Controls.Add(lblDetail);
                }
                else
                {
                    Label lblFree = new Label
                    {
                        Text = "Sipariş Açmak İçin Dokunun ➜",
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                        ForeColor = AppTheme.TextSecondary,
                        Location = new Point(16, 110),
                        Size = new Size(155, 36)
                    };
                    card.Controls.Add(lblFree);
                }

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
