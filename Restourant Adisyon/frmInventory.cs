using System;
using System.Collections;
using System.Data;
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
            LoadMaterials();
            LoadProductsForRecipe();
        }

        private void LoadMaterials()
        {
            string qry = "SELECT mID, mName AS 'Malzeme Adı', mQty AS 'Miktar', mUnit AS 'Birim' FROM tblMaterials ORDER BY mName";
            dgvMaterials.DataSource = MainClass.GetDataTable(qry);
        }

        private void btnAddMaterial_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMName.Text))
            {
                MessageBox.Show("Malzeme adı boş bırakılamaz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMName.Focus();
                return;
            }

            decimal qty = 0;
            if (!string.IsNullOrWhiteSpace(txtMQty.Text) && !decimal.TryParse(txtMQty.Text, out qty))
            {
                MessageBox.Show("Geçerli bir miktar girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string qry = "INSERT INTO tblMaterials (mName, mQty, mUnit) VALUES (@name, @qty, @unit)";
            Hashtable ht = new Hashtable();
            ht.Add("@name", txtMName.Text.Trim());
            ht.Add("@qty",  qty);
            ht.Add("@unit", txtMUnit.Text.Trim());

            if (MainClass.Sql(qry, ht) > 0)
            {
                MessageBox.Show("Malzeme eklendi.", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMName.Text = "";
                txtMQty.Text = "";
                txtMUnit.Text = "";
                LoadMaterials();
            }
        }

        private void LoadProductsForRecipe()
        {
            string qry = "SELECT pID 'id', pName 'name' FROM products ORDER BY pName";
            MainClass.CBFill(qry, cbProduct);
        }

        private void cbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecipe();
        }

        private void LoadRecipe()
        {
            if (cbProduct.SelectedValue == null || cbProduct.SelectedIndex < 0) return;
            int proID = Convert.ToInt32(cbProduct.SelectedValue);

            string qry = @"SELECT r.rID AS ID, m.mName AS 'Malzeme', r.qtyNeeded AS 'Gereken Miktar', m.mUnit AS 'Birim'
                           FROM tblRecipe r
                           INNER JOIN tblMaterials m ON m.mID = r.mID
                           WHERE r.proID = @proID";
            Hashtable ht = new Hashtable();
            ht.Add("@proID", proID);
            dgvRecipe.DataSource = MainClass.GetDataTable(qry, ht);
        }

        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            // Tarife ekleme mantığı - ileride genişletilebilir
        }
    }
}
