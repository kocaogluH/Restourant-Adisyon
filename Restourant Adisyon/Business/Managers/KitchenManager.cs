using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Restourant_Adisyon.Core.DTOs;

namespace Restourant_Adisyon.Business.Managers
{
    public class KitchenManager
    {
        private static KitchenManager _instance;
        public static KitchenManager Instance => _instance ?? (_instance = new KitchenManager());

        public List<KitchenOrderDto> GetActiveKitchenOrders()
        {
            var list = new List<KitchenOrderDto>();

            try
            {
                string qryOrders = "SELECT MainID, TableName, WaiterName, orderType, status, aTime, aDate FROM tblMain WHERE status IN ('Pending','Cooking') ORDER BY MainID ASC";
                DataTable dtOrders = MainClass.GetDataTable(qryOrders);

                if (dtOrders != null)
                {
                    foreach (DataRow row in dtOrders.Rows)
                    {
                        int mainId = Convert.ToInt32(row["MainID"]);
                        var order = new KitchenOrderDto
                        {
                            OrderId     = mainId,
                            TableName   = row["TableName"] != DBNull.Value ? row["TableName"].ToString() : "Masa -",
                            WaiterName  = row["WaiterName"] != DBNull.Value ? row["WaiterName"].ToString() : "-",
                            OrderType   = row["orderType"] != DBNull.Value ? row["orderType"].ToString() : "Masada",
                            Status      = row["status"]?.ToString() ?? "Pending",
                            OrderTime   = row["aTime"] != DBNull.Value ? row["aTime"].ToString() : "-"
                        };

                        // Geçen süre hesabı
                        try
                        {
                            if (DateTime.TryParse(order.OrderTime, out DateTime dtTime))
                                order.ElapsedMinutes = Math.Max(0, (int)(DateTime.Now - dtTime).TotalMinutes);
                        }
                        catch { order.ElapsedMinutes = 0; }

                        // Kalemler
                        string qryItems = @"SELECT IFNULL(p.pName, 'Ürün #' || d.proID) as pName, d.qty 
                                            FROM tblDetails d 
                                            LEFT JOIN products p ON p.pID = d.proID 
                                            WHERE d.MainID = @ID";
                        Hashtable ht = new Hashtable { { "@ID", mainId } };
                        DataTable dtItems = MainClass.GetDataTable(qryItems, ht);

                        if (dtItems != null)
                        {
                            foreach (DataRow itemRow in dtItems.Rows)
                            {
                                order.Items.Add(new KitchenItemDto
                                {
                                    ProductName = itemRow["pName"].ToString(),
                                    Quantity    = Convert.ToInt32(itemRow["qty"] ?? 1)
                                });
                            }
                        }

                        list.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("KitchenManager.GetActiveKitchenOrders", ex);
            }

            return list;
        }

        public bool AdvanceOrderStatus(int orderId, string currentStatus)
        {
            string nextStatus = currentStatus == "Pending" ? "Cooking" : "Ready";
            return SetOrderStatus(orderId, nextStatus);
        }

        public bool SetOrderStatus(int orderId, string status)
        {
            try
            {
                string qry = "UPDATE tblMain SET status=@st WHERE MainID=@ID";
                Hashtable ht = new Hashtable
                {
                    { "@st", status },
                    { "@ID", orderId }
                };
                return MainClass.Sql(qry, ht) > 0;
            }
            catch (Exception ex)
            {
                MainClass.LogError("KitchenManager.SetOrderStatus", ex);
                return false;
            }
        }
    }
}
