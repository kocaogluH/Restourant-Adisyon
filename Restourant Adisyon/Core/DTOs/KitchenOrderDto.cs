using System.Collections.Generic;

namespace Restourant_Adisyon.Core.DTOs
{
    public class KitchenOrderDto
    {
        public int OrderId { get; set; }
        public string TableName { get; set; }
        public string WaiterName { get; set; }
        public string OrderType { get; set; }
        public string OrderTime { get; set; }
        public string Status { get; set; }
        public int ElapsedMinutes { get; set; }
        public List<KitchenItemDto> Items { get; set; } = new List<KitchenItemDto>();
    }

    public class KitchenItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public bool IsReady { get; set; }
    }
}
