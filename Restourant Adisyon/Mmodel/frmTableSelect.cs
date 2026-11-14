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

        private void frmTableSelect_Load(object sender, EventArgs e)
        {
            this.BackColor = AppTheme.Background;
            if (label1 != null) label1.ForeColor = AppTheme.BrandPrimary;

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.BackColor = Color.Transparent;

            List<TableCardDto> tables = TableManager.Instance.GetAllTableCards();

            foreach (var t in tables)
            {
                bool isBusy = t.Status == TableStatus.Occupied;
                Color cardColor = isBusy ? AppTheme.Danger : AppTheme.Success;
                string statusText = isBusy ? $"🔴 DOLU ({t.ActiveBillAmount:N0} ₺)" : "🟢 BOŞ";

                Guna2Button b = new Guna2Button
                {
                    Text = $"{t.TableName}\n\n👥 {t.Capacity} Kişi\n{statusText}",
                    Width = 150,
                    Height = 130,
                    BorderRadius = 14,
                    FillColor = AppTheme.CardBackground,
                    ForeColor = AppTheme.TextPrimary,
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
