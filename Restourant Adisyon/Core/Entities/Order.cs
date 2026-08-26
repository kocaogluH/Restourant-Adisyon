using System;
using System.Collections.Generic;
using System.Linq;
using Restourant_Adisyon.Core.Enums;

namespace Restourant_Adisyon.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string OrderTime { get; set; } = DateTime.Now.ToString("HH:mm");
        public string TableName { get; set; }
        public string WaiterName { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string OrderType { get; set; } = "Masada";

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // Parçalı ödeme / Split bill hesaplamaları
        public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
        public decimal PaidAmount  => Payments.Sum(p => p.Amount);
        public decimal RemainingAmount => Math.Max(0m, TotalAmount - PaidAmount);
        public bool IsFullyPaid => RemainingAmount <= 0m && TotalAmount > 0m;
    }
}
