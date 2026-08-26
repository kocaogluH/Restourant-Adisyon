using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Restourant_Adisyon.Core.Entities;

namespace Restourant_Adisyon.Business.Services
{
    public class PrintService
    {
        public void PrintKOT(Order order)
        {
            if (order == null) return;

            try
            {
                using (PrintDocument pd = new PrintDocument())
                {
                    pd.PrintPage += (s, ev) =>
                    {
                        Graphics g = ev.Graphics;
                        var fntTitle  = new Font("Arial", 14, FontStyle.Bold);
                        var fntHeader = new Font("Arial", 10, FontStyle.Bold);
                        var fntNormal = new Font("Arial", 9);
                        int y = 10, x = 10;

                        g.DrawString("MUTFAK FİŞİ (KOT)", fntTitle, Brushes.Black, x + 30, y); y += 25;
                        g.DrawString($"Masa: {order.TableName}", fntHeader, Brushes.Black, x, y); y += 18;
                        g.DrawString($"Garson: {order.WaiterName}", fntNormal, Brushes.Black, x, y); y += 15;
                        g.DrawString($"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm}", fntNormal, Brushes.Black, x, y); y += 15;
                        g.DrawString(new string('-', 35), fntNormal, Brushes.Black, x, y); y += 15;

                        foreach (var item in order.Items)
                        {
                            string itemLine = $"{item.Quantity}x  {item.Product?.Name ?? "Ürün #" + item.ProductId}";
                            g.DrawString(itemLine, fntHeader, Brushes.Black, x, y); y += 18;
                        }

                        g.DrawString(new string('-', 35), fntNormal, Brushes.Black, x, y); y += 15;
                        g.DrawString($"Sipariş Tipi: {order.OrderType}", fntNormal, Brushes.Black, x, y);
                    };

                    pd.Print();
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("PrintKOT", ex);
            }
        }
    }
}
