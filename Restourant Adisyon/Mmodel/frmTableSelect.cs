using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Core.DTOs;
using Restourant_Adisyon.Core.Enums;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmTableSelect : Form
    {
        public frmTableSelect()
        {
            InitializeComponent();
        }

        public string TableName = "";

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTableSelect_Load(object sender, EventArgs e)
        {
            this.BackColor = AppTheme.Background;
            this.StartPosition = FormStartPosition.CenterScreen;

            MainClass.EnableEnterKeyConfirmation(
                this,
                () =>
                {
                    foreach (Control c in flowLayoutPanel1.Controls)
                    {
                        if (c is Guna2Button b && b.Visible && b.Enabled)
                        {
                            b.PerformClick();
                            break;
                        }
                    }
                },
                () => this.Close()
            );

            if (pictureBox1 != null)
                pictureBox1.Visible = false;

            if (guna2Panel1 != null)
            {
                guna2Panel1.FillColor = Color.FromArgb(15, 23, 42); // Obsidian Slate
                guna2Panel1.BackColor = Color.FromArgb(15, 23, 42);
                guna2Panel1.Height = 70;
            }

            if (label1 != null)
            {
                label1.Text = "🪑 Masa Seçimi";
                label1.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
                label1.ForeColor = Color.White;
                label1.Location = new Point(22, 20);
            }

            if (btnExit != null && guna2Panel1 != null)
            {
                btnExit.Location = new Point(guna2Panel1.Width - 46, (guna2Panel1.Height - 32) / 2);
                btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnExit.BackColor = Color.Transparent;
                btnExit.UseTransparentBackground = true;
            }

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.BackColor = Color.Transparent;

            List<TableCardDto> tables = TableManager.Instance.GetAllTableCards();

            if (tables == null || tables.Count == 0)
            {
                Panel pnlEmpty = new Panel
                {
                    Size = new Size(flowLayoutPanel1.Width - 40, 240),
                    BackColor = Color.Transparent
                };

                Label lblEmptyIcon = new Label
                {
                    Text = "🪑",
                    Font = new Font("Segoe UI", 38F),
                    ForeColor = Color.FromArgb(148, 163, 184),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 70
                };

                Label lblEmptyMsg = new Label
                {
                    Text = "Henüz kayıtlı masa bulunamadı.\nLütfen 'Masalar & Harita' menüsünden masa tanımlayınız.",
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

            foreach (var t in tables)
            {
                bool isBusy = t.Status == TableStatus.Occupied;
                Color cardColor = isBusy ? AppTheme.Danger : AppTheme.Success;
                string statusText = isBusy ? $"🔴 DOLU ({t.ActiveBillAmount:N0} ₺)" : "🟢 BOŞ";

                Guna2Button b = new Guna2Button
                {
                    Text = $"{t.TableName}\n\n👥 {t.Capacity} Kişi\n{statusText}",
                    Width = 155,
                    Height = 130,
                    BorderRadius = 14,
                    FillColor = AppTheme.CardBackground,
                    ForeColor = AppTheme.TextPrimary,
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    BorderColor = cardColor,
                    BorderThickness = 2,
                    Animated = true,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(10)
                };

                b.HoverState.FillColor = cardColor;
                b.HoverState.ForeColor = Color.White;

                b.Click += (s, ev) =>
                {
                    TableName = t.TableName;
                    this.Close();
                };

                flowLayoutPanel1.Controls.Add(b);
            }
        }
    }
}
