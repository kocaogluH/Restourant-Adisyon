using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Restourant_Adisyon.Core.DTOs;
using Restourant_Adisyon.Core.Enums;

namespace Restourant_Adisyon.Business.Managers
{
    public class TableManager
    {
        private static TableManager _instance;
        public static TableManager Instance => _instance ?? (_instance = new TableManager());

        public List<TableCardDto> GetAllTableCards(string search = "", TableStatus? filterStatus = null)
        {
            var list = new List<TableCardDto>();

            try
            {
                string qryTables = "SELECT tID, tName FROM tables ORDER BY tName";
                DataTable dtTables = MainClass.GetDataTable(qryTables);

                // Aktif siparişleri tek sorguda çek
                string qryActiveOrders = @"SELECT MainID, TableName, WaiterName, total, aTime, aDate, status 
                                           FROM tblMain 
                                           WHERE status='Pending' AND TableName IS NOT NULL AND TableName != ''";
                DataTable dtOrders = MainClass.GetDataTable(qryActiveOrders);
                var activeOrdersDict = new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);

                if (dtOrders != null)
                {
                    foreach (DataRow r in dtOrders.Rows)
                    {
                        string tName = r["TableName"].ToString().Trim();
                        if (!activeOrdersDict.ContainsKey(tName))
                            activeOrdersDict[tName] = r;
                    }
                }

                if (dtTables != null)
                {
                    foreach (DataRow row in dtTables.Rows)
                    {
                        string tableName = row["tName"]?.ToString() ?? "";
                        if (!string.IsNullOrEmpty(search) && !tableName.ToLower().Contains(search.ToLower()))
                            continue;

                        int tableId = Convert.ToInt32(row["tID"]);
                        var card = new TableCardDto
                        {
                            TableId   = tableId,
                            TableName = tableName,
                            Capacity  = 4
                        };

                        if (activeOrdersDict.TryGetValue(tableName.Trim(), out DataRow orderRow))
                        {
                            card.Status           = TableStatus.Occupied;
                            card.ActiveOrderId    = Convert.ToInt32(orderRow["MainID"]);
                            card.WaiterName       = orderRow["WaiterName"] != DBNull.Value ? orderRow["WaiterName"].ToString() : "-";
                            card.ActiveBillAmount = Convert.ToDecimal(orderRow["total"] ?? 0);
                            card.OrderTime        = orderRow["aTime"] != DBNull.Value ? orderRow["aTime"].ToString() : "-";

                            // Süre hesabı
                            try
                            {
                                if (DateTime.TryParse(card.OrderTime, out DateTime dtTime))
                                {
                                    card.ElapsedMinutes = Math.Max(0, (int)(DateTime.Now - dtTime).TotalMinutes);
                                }
                            }
                            catch { card.ElapsedMinutes = 0; }
                        }
                        else
                        {
                            card.Status = TableStatus.Empty;
                        }

                        if (filterStatus.HasValue && card.Status != filterStatus.Value)
                            continue;

                        list.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("TableManager.GetAllTableCards", ex);
            }

            return list;
        }

        public bool TransferTable(string fromTable, string toTable)
        {
            if (string.IsNullOrWhiteSpace(fromTable) || string.IsNullOrWhiteSpace(toTable))
                return false;

            try
            {
                string qry = "UPDATE tblMain SET TableName=@toTable WHERE TableName=@fromTable AND status='Pending'";
                Hashtable ht = new Hashtable
                {
                    { "@toTable",   toTable.Trim() },
                    { "@fromTable", fromTable.Trim() }
                };
                return MainClass.Sql(qry, ht) > 0;
            }
            catch (Exception ex)
            {
                MainClass.LogError("TableManager.TransferTable", ex);
                return false;
            }
        }
    }
}
