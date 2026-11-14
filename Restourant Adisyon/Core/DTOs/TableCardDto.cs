using System;
using Restourant_Adisyon.Core.Enums;

namespace Restourant_Adisyon.Core.DTOs
{
    public class TableCardDto
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public int Capacity { get; set; } = 4;
        public string Zone { get; set; } = "Ana Salon";
        public TableStatus Status { get; set; } = TableStatus.Empty;
        
        // Dolu masaya ait bilgiler
        public int ActiveOrderId { get; set; }
        public string WaiterName { get; set; }
        public decimal ActiveBillAmount { get; set; }
        public string OrderTime { get; set; }
        public int ElapsedMinutes { get; set; }
    }
}
