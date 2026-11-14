using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon.UI.Controls
{
    public static class GridStyler
    {
        public static void Apply(DataGridView grid, string emptyMessageKey = null)
        {
            if (grid == null) return;

            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;
            grid.AllowUserToAddRows = false;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.BackgroundColor = AppTheme.CardBackground;
            grid.GridColor = AppTheme.BorderColor;

            // Header Style
            grid.ColumnHeadersDefaultCellStyle.BackColor = ThemeManager.Instance.SurfaceElevated;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 40;

            // Rows
            grid.DefaultCellStyle.BackColor = AppTheme.CardBackground;
            grid.DefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = AppTheme.BrandPrimary;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            grid.AlternatingRowsDefaultCellStyle.BackColor = ThemeManager.Instance.CurrentTheme == Core.Enums.ThemeMode.CleanWhiteLight 
                ? Color.FromArgb(248, 250, 252) 
                : Color.FromArgb(25, 33, 50);
            grid.AlternatingRowsDefaultCellStyle.ForeColor = AppTheme.TextPrimary;

            // Empty-State Handling
            string msg = !string.IsNullOrEmpty(emptyMessageKey)
                ? LocalizationService.Instance.GetString(emptyMessageKey)
                : "Henüz kayıt bulunmuyor. Eklemek için '+' butonuna tıklayın.";

            grid.Paint -= (s, e) => Grid_Paint(grid, e, msg);
            grid.Paint += (s, e) => Grid_Paint(grid, e, msg);
        }

        private static void Grid_Paint(DataGridView grid, PaintEventArgs e, string emptyMessage)
        {
            if (grid.Rows.Count == 0)
            {
                using (Font font = new Font("Segoe UI", 11F, FontStyle.Regular))
                using (Brush brush = new SolidBrush(AppTheme.TextSecondary))
                {
                    string text = string.IsNullOrEmpty(emptyMessage)
                        ? "Henüz kayıt bulunmuyor."
                        : emptyMessage;

                    SizeF textSize = e.Graphics.MeasureString(text, font);
                    float x = (grid.Width - textSize.Width) / 2;
                    float y = (grid.Height - textSize.Height) / 2;

                    if (x > 0 && y > 0)
                    {
                        e.Graphics.DrawString(text, font, brush, x, y);
                    }
                }
            }
        }
    }
}
