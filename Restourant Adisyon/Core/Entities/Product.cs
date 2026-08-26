using System.Collections.Generic;

namespace Restourant_Adisyon.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Barcode { get; set; }
        public byte[] Image { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
