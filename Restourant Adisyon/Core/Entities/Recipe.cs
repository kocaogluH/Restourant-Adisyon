namespace Restourant_Adisyon.Core.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        public decimal QuantityUsed { get; set; }

        public virtual Product Product { get; set; }
        public virtual Material Material { get; set; }
    }
}
