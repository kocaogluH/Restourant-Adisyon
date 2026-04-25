using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        public int MainID = 0;
        public String OrderType;
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            LoadProducts();
        }

        private void AddCategory()
        {
            string qry = "Select * from Category";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryPanel.Controls.Clear();
            Guna.UI2.WinForms.Guna2Button b2 = new Guna.UI2.WinForms.Guna2Button();
            b2.FillColor = Color.FromArgb(50, 55, 89);
            b2.Size = new Size(134, 45);
            b2.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            b2.Text = "All Categories";
            b2.CheckedState.FillColor = Color.FromArgb(241, 85, 126);
            b2.Click += new EventHandler(b_click);
            CategoryPanel.Controls.Add(b2);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(94, 148, 255);
                    b.Size = new Size(180, 45);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["catName"].ToString();
                    b.CheckedState.FillColor = Color.FromArgb(241, 85, 126);

                    b.Click += new EventHandler(b_click);



                    CategoryPanel.Controls.Add(b);
                }
            }

        }
        private void b_click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            if (b.Text == "All Categories")
            {
                txtSearch.Text = "1";
                txtSearch.Text = "";
                return;
            }
            foreach (var tem in ProductPanel.Controls)
            {
                var pro = (ucProduct)tem;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }


        private void AddItems(string id, string proID, string name, string cat, string price, Image pimage)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(proID)
            };

            ProductPanel.Controls.Add(w);
            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    if (Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                         double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        return;
                    }
                }
                guna2DataGridView1.Rows.Add(new object[] { 0, 0, wdg.IsAccessible, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                GetTotal();
            };
        }

        private void LoadProducts()
        {
            string qry = "Select *from products inner join category  On catID = CategoryID  ";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["PImage"];
                byte[] imagebytearray = imagearray;

                AddItems("0", item["pID"].ToString(), item["pName"].ToString(), item["catName"].ToString(),
                    item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)));
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void GetTotal()
        {
            double tot = 0;
            lblTotal.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }

            lblTotal.Text = tot.ToString("N2");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "00";
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Delivery";
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Take Avay";
        }

        private void btnDin_Click(object sender, EventArgs e)
        {
            OrderType = "Din In";

            frmTableSelect frm = new frmTableSelect();
            MainClass.BlurBackground(frm);
            if (frm.TableName != "")
            {
                lblTable.Text = frm.TableName;
                lblTable.Visible = true;
            }
            else
            {
                lblTable.Text = "";
                lblTable.Visible = false;
            }

            frmWaiterSelect frm2 = new frmWaiterSelect();
            MainClass.BlurBackground(frm2);
            if (frm2.waiterName != "")
            {
                lblWaiter.Text = frm2.waiterName;
                lblWaiter.Visible = true;
            }
            else
            {
                lblWaiter.Text = "";
                lblWaiter.Visible = false;
            }
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(OrderType))
            {
                guna2MessageDialog1.Show("Please select order type");
                return;
            }

            if (guna2DataGridView1.Rows.Count == 0)
            {
                guna2MessageDialog1.Show("Please add products first");
                return;
            }

            string qry1 = "";
            string qry2 = "";
            int detailID = 0;

            if (MainID == 0)
            {
                qry1 = @"Insert into tblMain (aDate, aTime, TableName, WaiterName, status, orderType, total, received, change) 
                         Values(@aDate, @aTime, @TableName, @WaiterName, @status, @orderType, @total, @received, @change);
                         Select SCOPE_IDENTITY()";
            }
            else
            {
                qry1 = @"Update tblMain Set status = @status, total = @total, 
                         received = @received, change = @change where MainID = @ID";
            }

            SqlCommand cmd = new SqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", MainID);
            cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@TableName", lblTable.Text ?? "");
            cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text ?? "");
            cmd.Parameters.AddWithValue("@status", "Pending");
            cmd.Parameters.AddWithValue("@orderType", OrderType ?? "");
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text));
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));

            if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
            if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                if (detailID == 0)
                {
                    qry2 = @" Insert into tblDetails (MainID, proID, qty, price, amount) Values(@MainID, @proID, @qty, @price, @amount)";
                }
                else
                {
                    qry2 = @"Update tblDetails Set proID = @proID, qty = @qty, price = @price, amount = @amount where DetailID = @ID";
                }

                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                cmd2.ExecuteNonQuery();

                // Stock Deduction Logic
                string qryStock = @"Update tblMaterials 
                                    Set mQty = mQty - (r.qtyNeeded * @qty)
                                    from tblMaterials m
                                    inner join tblRecipe r on m.mID = r.mID
                                    where r.proID = @proID";
                
                SqlCommand cmdStock = new SqlCommand(qryStock, MainClass.con);
                cmdStock.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmdStock.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmdStock.ExecuteNonQuery();
            }
            
            if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }

            guna2MessageDialog1.Show("Saved Successfully");

            MainID = 0;
            detailID = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            lblTotal.Text = "0.00";
            OrderType = "";
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frm = new frmBillList();
            MainClass.BlurBackground(frm);

            if (frm.MainID > 0)
            {
                MainID = frm.MainID;
                LoadEntries();
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (MainID == 0)
            {
                guna2MessageDialog1.Show("Please select a bill first");
                return;
            }

            frmCheckout frm = new frmCheckout();
            frm.MainID = MainID;
            frm.amt = Convert.ToDouble(lblTotal.Text);
            MainClass.BlurBackground(frm);

            if (frm.isSuccess)
            {
                btnNew_Click(null, null); // Clear screen after successful checkout
            }
        }

        private void LoadEntries()
        {
            string qry = @"Select * from tblMain m
                        inner join tblDetails d on m.MainID = d.MainID
                        inner join products p on P.pID = d.proID
                         where m.MainID = " + MainID + "";

            SqlCommand cmd2 = new SqlCommand(qry, MainClass.con);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            guna2DataGridView1.Rows.Clear();

            foreach (DataRow item in dt2.Rows)
            {
                lblTable.Text = item["TableName"].ToString();
                lblWaiter.Text = item["WaiterName"].ToString();
                lblTable.Visible = true;
                lblWaiter.Visible = true;

                string detailid = item["DetailID"].ToString();
                string proid = item["proID"].ToString();
                string pname = item["pName"].ToString();
                string qty = item["qty"].ToString();
                string price = item["price"].ToString();
                string amount = item["amount"].ToString();

                object[] obj = { 0, detailid, proid, pname, qty, price, amount };
                guna2DataGridView1.Rows.Add(obj);
            }
            GetTotal();
        }
    }
}  

