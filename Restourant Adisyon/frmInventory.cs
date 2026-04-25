using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Restourant_Adisyon
{
    public partial class frmInventory : Form
    {
        public frmInventory()
        {
            InitializeComponent();
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            CreateTablesIfNotExists();
            LoadMaterials();
            LoadProductsForRecipe();
        }

        private void CreateTablesIfNotExists()
        {
            string qry = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblMaterials' AND xtype='U')
            CREATE TABLE tblMaterials (
                mID INT PRIMARY KEY IDENTITY(1,1),
                mName NVARCHAR(100),
                mQty DECIMAL(18,2) DEFAULT 0,
                mUnit NVARCHAR(20)
            );

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblRecipe' AND xtype='U')
            CREATE TABLE tblRecipe (
                rID INT PRIMARY KEY IDENTITY(1,1),
                proID INT,
                mID INT,
                qtyNeeded DECIMAL(18,2)
            );";
            
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            if (MainClass.con.State == ConnectionState.Closed) MainClass.con.Open();
            cmd.ExecuteNonQuery();
            if (MainClass.con.State == ConnectionState.Open) MainClass.con.Close();
        }

        private void LoadMaterials()
        {
            string qry = "Select * from tblMaterials";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvMaterials.DataSource = dt;
        }

        private void LoadProductsForRecipe()
        {
            string qry = "Select pID, pName from products";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbProduct.DataSource = dt;
            cbProduct.DisplayMember = "pName";
            cbProduct.ValueMember = "pID";
        }

        private void btnAddMaterial_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMName.Text)) return;
            
            string qry = "Insert into tblMaterials (mName, mQty, mUnit) Values(@name, @qty, @unit)";
            Hashtable ht = new Hashtable();
            ht.Add("@name", txtMName.Text);
            ht.Add("@qty", Convert.ToDecimal(txtMQty.Text));
            ht.Add("@unit", txtMUnit.Text);

            if (MainClass.Sql(qry, ht) > 0)
            {
                MessageBox.Show("Material Added");
                LoadMaterials();
            }
        }

        private void cbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecipe();
        }

        private void LoadRecipe()
        {
            if (cbProduct.SelectedValue == null) return;
            int proID = Convert.ToInt32(cbProduct.SelectedValue);
            string qry = @"Select r.rID, m.mName, r.qtyNeeded, m.mUnit 
                           from tblRecipe r 
                           inner join tblMaterials m on m.mID = r.mID 
                           where r.proID = " + proID;
            
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(qry, MainClass.con);
            da.Fill(dt);
            dgvRecipe.DataSource = dt;
        }

        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            // Logic to add recipe item
            // For brevity, assuming mID is selected from another combo
        }
    }
}
