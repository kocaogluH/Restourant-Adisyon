namespace Restourant_Adisyon.Core.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal CriticalLevel { get; set; } = 5m;
    }
}
