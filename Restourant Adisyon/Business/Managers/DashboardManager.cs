using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Restourant_Adisyon.Core.DTOs;

namespace Restourant_Adisyon.Business.Managers
{
    public class DashboardManager
    {
        private static DashboardManager _instance;
        public static DashboardManager Instance => _instance ?? (_instance = new DashboardManager());

        public DashboardSummaryDto GetDashboardSummary()
        {
            var summary = new DashboardSummaryDto();
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                // Toplam Ciro
                object resTotalRev = MainClass.SqlScalar("SELECT IFNULL(SUM(total), 0) FROM tblMain WHERE status='Paid'");
                summary.TotalRevenue = resTotalRev != null && resTotalRev != DBNull.Value ? Convert.ToDecimal(resTotalRev) : 0m;

                // Bugünkü Ciro
                Hashtable htToday = new Hashtable { { "@today", today } };
                object resTodayRev = MainClass.SqlScalar("SELECT IFNULL(SUM(total), 0) FROM tblMain WHERE status='Paid' AND (aDate=@today OR aDate LIKE @today || '%')", htToday);
                summary.TodayRevenue = resTodayRev != null && resTodayRev != DBNull.Value ? Convert.ToDecimal(resTodayRev) : 0m;

                // Toplam Sipariş
                object resTotalOrders = MainClass.SqlScalar("SELECT COUNT(*) FROM tblMain WHERE status='Paid'");
                summary.TotalOrders = resTotalOrders != null && resTotalOrders != DBNull.Value ? Convert.ToInt32(resTotalOrders) : 0;

                // Bugünkü Sipariş
                object resTodayOrders = MainClass.SqlScalar("SELECT COUNT(*) FROM tblMain WHERE status='Paid' AND (aDate=@today OR aDate LIKE @today || '%')", htToday);
                summary.TodayOrders = resTodayOrders != null && resTodayOrders != DBNull.Value ? Convert.ToInt32(resTodayOrders) : 0;

                // Toplam Ürün
                object resTotalProducts = MainClass.SqlScalar("SELECT COUNT(*) FROM products");
                summary.TotalProducts = resTotalProducts != null && resTotalProducts != DBNull.Value ? Convert.ToInt32(resTotalProducts) : 0;

                // Aktif Masa Sayısı
                object resActiveTables = MainClass.SqlScalar("SELECT COUNT(DISTINCT TableName) FROM tblMain WHERE status='Pending' AND TableName IS NOT NULL AND TableName != ''");
                summary.ActiveTablesCount = resActiveTables != null && resActiveTables != DBNull.Value ? Convert.ToInt32(resActiveTables) : 0;

                // Toplam Masa Sayısı
                object resTotalTables = MainClass.SqlScalar("SELECT COUNT(*) FROM tables");
                summary.TotalTablesCount = resTotalTables != null && resTotalTables != DBNull.Value ? Convert.ToInt32(resTotalTables) : 0;

                // Kritik Stok Sayısı
                object resLowStock = MainClass.SqlScalar("SELECT COUNT(*) FROM tblMaterials WHERE mQty < 5");
                summary.LowStockCount = resLowStock != null && resLowStock != DBNull.Value ? Convert.ToInt32(resLowStock) : 0;

                // En Çok Satan Ürünler (Top 5)
                string qryTop = @"SELECT IFNULL(p.pName, 'Ürün #' || d.proID) as proName,
                                         IFNULL(c.catName, 'Genel') as catName,
                                         SUM(d.qty) as SoldQty,
                                         SUM(d.amount) as TotalAmt
                                  FROM tblDetails d
                                  LEFT JOIN products p ON p.pID = d.proID
                                  LEFT JOIN category c ON c.catID = p.CategoryID
                                  GROUP BY d.proID
                                  ORDER BY SoldQty DESC
                                  LIMIT 5";
                DataTable dtTop = MainClass.GetDataTable(qryTop);
                if (dtTop != null)
                {
                    foreach (DataRow row in dtTop.Rows)
                    {
                        summary.TopProducts.Add(new TopProductDto
                        {
                            ProductName        = row["proName"].ToString(),
                            CategoryName       = row["catName"].ToString(),
                            TotalQuantitySold  = Convert.ToInt32(row["SoldQty"] ?? 0),
                            TotalAmount        = Convert.ToDecimal(row["TotalAmt"] ?? 0)
                        });
                    }
                }

                // Son Siparişler (Recent 7)
                string qryRecent = @"SELECT MainID, TableName, WaiterName, orderType, total, status, aTime
                                     FROM tblMain
                                     ORDER BY MainID DESC
                                     LIMIT 7";
                DataTable dtRecent = MainClass.GetDataTable(qryRecent);
                if (dtRecent != null)
                {
                    foreach (DataRow row in dtRecent.Rows)
                    {
                        summary.RecentOrders.Add(new RecentOrderDto
                        {
                            OrderId     = Convert.ToInt32(row["MainID"]),
                            TableName   = row["TableName"] != DBNull.Value ? row["TableName"].ToString() : "Masa -",
                            WaiterName  = row["WaiterName"] != DBNull.Value ? row["WaiterName"].ToString() : "-",
                            OrderType   = row["orderType"] != DBNull.Value ? row["orderType"].ToString() : "Masada",
                            TotalAmount = Convert.ToDecimal(row["total"] ?? 0),
                            Status      = row["status"] != DBNull.Value ? row["status"].ToString() : "Pending",
                            OrderTime   = row["aTime"] != DBNull.Value ? row["aTime"].ToString() : "-"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("DashboardManager.GetDashboardSummary", ex);
            }

            return summary;
        }
    }
}
