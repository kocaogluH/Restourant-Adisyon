using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Services;

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
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ApplyLocalization();
            LoadMaterials();
            LoadProductsForRecipe();
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label1 != null) label1.Text = loc.GetString("Material_Name");
            if (label2 != null) label2.Text = loc.GetString("Quantity");
            if (label3 != null) label3.Text = loc.GetString("Unit");
            if (label4 != null) label4.Text = loc.GetString("Recipe_Header");
            if (btnAddMaterial != null) btnAddMaterial.Text = loc.GetString("Add_Material");
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
    }
}
