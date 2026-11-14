using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Restourant_Adisyon.Business.Managers;
using Restourant_Adisyon.Business.Services;
using Restourant_Adisyon.UI.Controls;
using Restourant_Adisyon.UI.Theme;

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
            ApplyTheme();
            LocalizationService.Instance.OnLanguageChanged += (s, args) => ApplyLocalization();
            ThemeManager.Instance.OnThemeChanged += (s, args) => ApplyTheme();
            ApplyLocalization();

            if (dgvMaterials != null)
                GridStyler.Apply(dgvMaterials, "Henüz malzeme eklenmemiş. Eklemek için formu kullanın.");
            if (dgvRecipe != null)
                GridStyler.Apply(dgvRecipe, "Bu ürün için reçete eklenmemiş.");

            LoadMaterials();
            LoadProductsForRecipe();
        }

        private void ApplyTheme()
        {
            this.BackColor = AppTheme.Background;

            if (txtMName != null)
            {
                txtMName.BorderRadius = 8;
                txtMName.BorderColor = AppTheme.BorderColor;
                txtMName.FillColor = AppTheme.CardBackground;
                txtMName.ForeColor = AppTheme.TextPrimary;
                txtMName.PlaceholderText = "Örn: Un, Domates, Kıyma...";
            }

            if (txtMQty != null)
            {
                txtMQty.BorderRadius = 8;
                txtMQty.BorderColor = AppTheme.BorderColor;
                txtMQty.FillColor = AppTheme.CardBackground;
                txtMQty.ForeColor = AppTheme.TextPrimary;
            }

            if (txtMUnit != null)
            {
                txtMUnit.BorderRadius = 8;
                txtMUnit.BorderColor = AppTheme.BorderColor;
                txtMUnit.FillColor = AppTheme.CardBackground;
                txtMUnit.ForeColor = AppTheme.TextPrimary;
                txtMUnit.PlaceholderText = "kg, lt, adet...";
            }

            if (cbProduct != null)
            {
                cbProduct.BorderRadius = 8;
                cbProduct.BorderColor = AppTheme.BorderColor;
                cbProduct.FillColor = AppTheme.CardBackground;
                cbProduct.ForeColor = AppTheme.TextPrimary;
            }

            if (btnAddMaterial != null)
            {
                btnAddMaterial.BorderRadius = 8;
                btnAddMaterial.FillColor = AppTheme.BrandPrimary;
                btnAddMaterial.ForeColor = Color.White;
                btnAddMaterial.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btnAddMaterial.Cursor = Cursors.Hand;
            }

            if (label1 != null) { label1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); label1.ForeColor = AppTheme.TextSecondary; }
            if (label2 != null) { label2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); label2.ForeColor = AppTheme.TextSecondary; }
            if (label3 != null) { label3.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); label3.ForeColor = AppTheme.TextSecondary; }
            if (label4 != null) { label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold); label4.ForeColor = AppTheme.TextPrimary; }
        }

        private void ApplyLocalization()
        {
            var loc = LocalizationService.Instance;
            if (label1 != null) label1.Text = loc.GetString("Material_Name") ?? "Malzeme Adı";
            if (label2 != null) label2.Text = loc.GetString("Quantity") ?? "Miktar";
            if (label3 != null) label3.Text = loc.GetString("Unit") ?? "Birim";
            if (label4 != null) label4.Text = loc.GetString("Recipe_Header") ?? "📦 Ürün Reçete Eşleştirme";
            if (btnAddMaterial != null) btnAddMaterial.Text = loc.GetString("Add_Material") ?? "➕ Malzeme Ekle";
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
