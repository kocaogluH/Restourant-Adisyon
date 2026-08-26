using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;

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

            // Header Style
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 55, 89);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 38;

            // Zebra Striping Rows
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 85, 126);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);

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
                using (Brush brush = new SolidBrush(Color.FromArgb(140, 150, 175)))
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
