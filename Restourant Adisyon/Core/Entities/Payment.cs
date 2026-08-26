using System;
using Restourant_Adisyon.Core.Enums;

namespace Restourant_Adisyon.Core.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime PaidAt { get; set; } = DateTime.Now;

        public virtual Order Order { get; set; }
    }
}
