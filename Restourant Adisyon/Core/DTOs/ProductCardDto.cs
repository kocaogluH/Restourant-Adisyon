using System.Drawing;

namespace Restourant_Adisyon.Core.DTOs
{
    public class ProductCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Barcode { get; set; }
        public Image ProductImage { get; set; }
        public decimal StockQty { get; set; }
    }
}
