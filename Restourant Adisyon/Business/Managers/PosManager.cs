using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using Restourant_Adisyon.Core.DTOs;
using Restourant_Adisyon.Core.Entities;

namespace Restourant_Adisyon.Business.Managers
{
    public class PosManager
    {
        private static PosManager _instance;
        public static PosManager Instance => _instance ?? (_instance = new PosManager());

        public List<Category> GetCategories()
        {
            var list = new List<Category>();
            try
            {
                string qry = "SELECT catID, catName FROM category ORDER BY catName";
                DataTable dt = MainClass.GetDataTable(qry);
                if (dt != null)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        list.Add(new Category
                        {
                            Id   = Convert.ToInt32(r["catID"]),
                            Name = r["catName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("PosManager.GetCategories", ex);
            }
            return list;
        }

        public List<ProductCardDto> GetProducts(string categoryName = "", string search = "")
        {
            var list = new List<ProductCardDto>();
            try
            {
                string qry = @"SELECT p.pID, p.pName, p.pPrice, p.CategoryID, IFNULL(c.catName, 'Genel') as catName, 
                                      p.pBarcode, p.pImage 
                               FROM products p 
                               LEFT JOIN category c ON c.catID = p.CategoryID 
                               WHERE 1=1";

                Hashtable ht = new Hashtable();

                if (!string.IsNullOrWhiteSpace(categoryName) && categoryName != "Tümü" && categoryName != "All")
                {
                    qry += " AND c.catName = @cat";
                    ht.Add("@cat", categoryName.Trim());
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    qry += " AND (p.pName LIKE @s OR p.pBarcode LIKE @s)";
                    ht.Add("@s", "%" + search.Trim() + "%");
                }

                qry += " ORDER BY p.pName ASC";

                DataTable dt = MainClass.GetDataTable(qry, ht);
                if (dt != null)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        Image img = null;
                        if (r["pImage"] != DBNull.Value)
                        {
                            try
                            {
                                byte[] bytes = (byte[])r["pImage"];
                                if (bytes.Length > 0)
                                {
                                    using (var ms = new MemoryStream(bytes))
                                        img = new Bitmap(ms);
                                }
                            }
                            catch { }
                        }

                        list.Add(new ProductCardDto
                        {
                            Id           = Convert.ToInt32(r["pID"]),
                            Name         = r["pName"].ToString(),
                            Price        = Convert.ToDecimal(r["pPrice"] ?? 0),
                            CategoryId   = r["CategoryID"] != DBNull.Value ? Convert.ToInt32(r["CategoryID"]) : 0,
                            CategoryName = r["catName"].ToString(),
                            Barcode      = r["pBarcode"] != DBNull.Value ? r["pBarcode"].ToString() : "",
                            ProductImage = img
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("PosManager.GetProducts", ex);
            }

            return list;
        }

        public ProductCardDto FindProductByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return null;

            try
            {
                string qry = @"SELECT p.pID, p.pName, p.pPrice, p.CategoryID, IFNULL(c.catName, 'Genel') as catName, p.pBarcode 
                               FROM products p 
                               LEFT JOIN category c ON c.catID = p.CategoryID 
                               WHERE p.pBarcode = @b LIMIT 1";
                Hashtable ht = new Hashtable { { "@b", barcode.Trim() } };
                DataTable dt = MainClass.GetDataTable(qry, ht);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    return new ProductCardDto
                    {
                        Id           = Convert.ToInt32(r["pID"]),
                        Name         = r["pName"].ToString(),
                        Price        = Convert.ToDecimal(r["pPrice"] ?? 0),
                        CategoryId   = r["CategoryID"] != DBNull.Value ? Convert.ToInt32(r["CategoryID"]) : 0,
                        CategoryName = r["catName"].ToString(),
                        Barcode      = r["pBarcode"]?.ToString() ?? ""
                    };
                }
            }
            catch (Exception ex)
            {
                MainClass.LogError("PosManager.FindProductByBarcode", ex);
            }

            return null;
        }
    }
}
