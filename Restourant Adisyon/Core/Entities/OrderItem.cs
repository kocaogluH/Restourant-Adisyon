namespace Restourant_Adisyon.Core.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount => Quantity * UnitPrice;

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
