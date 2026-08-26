using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Restourant_Adisyon.Core.Entities;

namespace Restourant_Adisyon.Business.Services
{
    public class InventoryService
    {
        /// <summary>
        /// Sipariş verildiğinde veya mutfağa yollandığında ürün reçetesine göre hammadde stoklarını düşer.
        /// </summary>
        public void DeductStockForOrder(Order order)
        {
            if (order == null || order.Items == null) return;

            foreach (var item in order.Items)
            {
                DeductStockForProduct(item.ProductId, item.Quantity);
            }
            CheckCriticalLevels();
        }

        public void DeductStockForProduct(int productId, int productQty)
        {
            string qryRecipe = "SELECT mID, qtyNeeded FROM tblRecipe WHERE proID = @proID";
            Hashtable ht = new Hashtable();
            ht.Add("@proID", productId);

            DataTable dtRecipe = MainClass.GetDataTable(qryRecipe, ht);
            foreach (DataRow row in dtRecipe.Rows)
            {
                int mID = Convert.ToInt32(row["mID"]);
                decimal qtyNeeded = Convert.ToDecimal(row["qtyNeeded"]);
                decimal totalDeduct = qtyNeeded * productQty;

                string qryDeduct = "UPDATE tblMaterials SET mQty = mQty - @deduct WHERE mID = @mID";
                Hashtable htDeduct = new Hashtable();
                htDeduct.Add("@deduct", totalDeduct);
                htDeduct.Add("@mID", mID);

                MainClass.Sql(qryDeduct, htDeduct);
            }
        }

        public List<string> CheckCriticalLevels()
        {
            List<string> warnings = new List<string>();
            string qry = "SELECT mName, mQty, mUnit FROM tblMaterials WHERE mQty <= 5";
            DataTable dt = MainClass.GetDataTable(qry);

            foreach (DataRow row in dt.Rows)
            {
                string warn = $"⚠ STOK UYARISI: {row["mName"]} kritik seviyenin altında! (Kalan: {row["mQty"]} {row["mUnit"]})";
                warnings.Add(warn);
            }

            return warnings;
        }
    }
}
