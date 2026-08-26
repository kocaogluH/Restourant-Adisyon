using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Restourant_Adisyon.Core.Entities;
using Restourant_Adisyon.Core.Enums;

namespace Restourant_Adisyon.Business.Services
{
    public class OrderService
    {
        private readonly InventoryService _inventoryService = new InventoryService();

        public Order GetOrderById(int orderId)
        {
            string qryMain = "SELECT * FROM tblMain WHERE MainID = @id";
            Hashtable ht = new Hashtable();
            ht.Add("@id", orderId);
            DataTable dtMain = MainClass.GetDataTable(qryMain, ht);

            if (dtMain.Rows.Count == 0) return null;

            DataRow r = dtMain.Rows[0];
            Order order = new Order
            {
                Id         = Convert.ToInt32(r["MainID"]),
                TableName  = r["TableName"]?.ToString(),
                WaiterName = r["WaiterName"]?.ToString(),
                OrderType  = r["OrderType"]?.ToString() ?? "Masada"
            };

            // Status Parse
            string statusStr = r["status"]?.ToString();
            if (Enum.TryParse(statusStr, out OrderStatus status))
                order.Status = status;

            // Load Items
            string qryItems = @"SELECT d.*, p.pName FROM tblDetails d
                                INNER JOIN products p ON p.pID = d.proID
                                WHERE d.MainID = @id";
            DataTable dtItems = MainClass.GetDataTable(qryItems, ht);
            foreach (DataRow row in dtItems.Rows)
            {
                order.Items.Add(new OrderItem
                {
                    Id        = Convert.ToInt32(row["DetailID"]),
                    OrderId   = order.Id,
                    ProductId = Convert.ToInt32(row["proID"]),
                    Quantity  = Convert.ToInt32(row["qty"]),
                    UnitPrice = Convert.ToDecimal(row["price"])
                });
            }

            // Load Payments
            string qryPayments = "SELECT * FROM tblPayments WHERE OrderID = @id";
            DataTable dtPay = MainClass.GetDataTable(qryPayments, ht);
            if (dtPay != null && dtPay.Rows.Count > 0)
            {
                foreach (DataRow payRow in dtPay.Rows)
                {
                    order.Payments.Add(new Payment
                    {
                        Id      = Convert.ToInt32(payRow["PaymentID"]),
                        OrderId = order.Id,
                        Amount  = Convert.ToDecimal(payRow["Amount"]),
                        PaidAt  = Convert.ToDateTime(payRow["PaidAt"])
                    });
                }
            }
            else
            {
                // Fallback for existing received values
                decimal received = Convert.ToDecimal(r["received"] ?? 0);
                if (received > 0)
                {
                    order.Payments.Add(new Payment
                    {
                        OrderId = order.Id,
                        Amount  = Math.Min(received, order.TotalAmount),
                        Method  = PaymentMethod.Nakit
                    });
                }
            }

            return order;
        }

        public bool AddPayment(int orderId, decimal amount, PaymentMethod method, out decimal remainingAmount)
        {
            remainingAmount = 0m;
            Order order = GetOrderById(orderId);
            if (order == null) return false;

            decimal payAmount = Math.Min(amount, order.RemainingAmount);
            if (payAmount <= 0)
            {
                remainingAmount = order.RemainingAmount;
                return false;
            }

            // Tabloyu güvenceye al
            string ensureTable = @"CREATE TABLE IF NOT EXISTS tblPayments (
                                    PaymentID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    OrderID INTEGER,
                                    Amount REAL,
                                    Method TEXT,
                                    PaidAt TEXT
                                );";
            MainClass.Sql(ensureTable, new Hashtable());

            string qryAddPay = @"INSERT INTO tblPayments (OrderID, Amount, Method, PaidAt)
                                VALUES (@orderId, @amount, @method, @paidAt)";
            Hashtable ht = new Hashtable();
            ht.Add("@orderId", orderId);
            ht.Add("@amount",  payAmount);
            ht.Add("@method",  method.ToString());
            ht.Add("@paidAt",  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            MainClass.Sql(qryAddPay, ht);

            // Re-fetch order to check remaining amount
            Order updatedOrder = GetOrderById(orderId);
            remainingAmount = updatedOrder.RemainingAmount;

            if (updatedOrder.IsFullyPaid || remainingAmount <= 0)
            {
                // Tamamı ödendi - Masa kapat
                string qryUpdateMain = "UPDATE tblMain SET status='Paid', received=@rec WHERE MainID=@id";
                Hashtable htUp = new Hashtable();
                htUp.Add("@rec", updatedOrder.PaidAmount);
                htUp.Add("@id",  orderId);
                MainClass.Sql(qryUpdateMain, htUp);

                // Stok Düşümü Tetikle
                _inventoryService.DeductStockForOrder(updatedOrder);
            }

            return true;
        }
    }
}
