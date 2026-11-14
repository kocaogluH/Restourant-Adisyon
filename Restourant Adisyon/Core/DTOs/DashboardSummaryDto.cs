using System;
using System.Collections.Generic;

namespace Restourant_Adisyon.Core.DTOs
{
    public class DashboardSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TodayOrders { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveTablesCount { get; set; }
        public int TotalTablesCount { get; set; }
        public int LowStockCount { get; set; }

        public double OccupancyRate => TotalTablesCount > 0 
            ? Math.Round((double)ActiveTablesCount / TotalTablesCount * 100.0, 1) 
            : 0.0;

        public List<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();
        public List<RecentOrderDto> RecentOrders { get; set; } = new List<RecentOrderDto>();
    }

    public class TopProductDto
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class RecentOrderDto
    {
        public int OrderId { get; set; }
        public string TableName { get; set; }
        public string WaiterName { get; set; }
        public string OrderType { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string OrderTime { get; set; }
    }
}
