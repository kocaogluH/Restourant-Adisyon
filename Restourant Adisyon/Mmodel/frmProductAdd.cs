using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmProductAdd : SampleAdd
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }

        public int id  = 0;
        public int cID = 0;

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            string qry = "SELECT catID 'id', catName 'name' FROM category ORDER BY catName";
            MainClass.CBFill(qry, cbCat);

            if (cID > 0) cbCat.SelectedValue = cID;
            if (id  > 0) ForUpdateLoadData();
        }

        string filePath;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resimler|*.png;*.jpg;*.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                txtImage.Image = new Bitmap(filePath);
            }
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                guna2MessageDialog1.Show("Ürün adı boş olamaz.");
                txtName.Focus();
                return;
            }
            double price = 0;
            if (!double.TryParse(txtPrice.Text, out price) || price < 0)
            {
                guna2MessageDialog1.Show("Geçerli bir fiyat girin.");
                txtPrice.Focus();
                return;
            }
            if (cbCat.SelectedValue == null)
            {
                guna2MessageDialog1.Show("Lütfen kategori seçin.");
                return;
            }

            byte[] imageByteArray = null;
            if (txtImage.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    new Bitmap(txtImage.Image).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    imageByteArray = ms.ToArray();
                }
            }

            string qry;
            if (id == 0)
                qry = "INSERT INTO products (pName, pPrice, CategoryID, pImage) VALUES (@Name, @price, @cat, @img)";
            else
                qry = "UPDATE products SET pName=@Name, pPrice=@price, CategoryID=@cat, pImage=@img WHERE pID=@id";

            Hashtable ht = new Hashtable();
            ht.Add("@id",    id);
            ht.Add("@Name",  txtName.Text.Trim());
            ht.Add("@price", price);
            ht.Add("@cat",   Convert.ToInt32(cbCat.SelectedValue));
            ht.Add("@img",   imageByteArray ?? (object)System.DBNull.Value);

            if (MainClass.Sql(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Başarıyla kaydedildi.");
                id = 0; cID = 0;
                txtName.Text = "";
                txtPrice.Text = "";
                cbCat.SelectedIndex = -1;
                txtImage.Image = Properties.Resources.productPic;
                txtName.Focus();
            }
        }

        private void ForUpdateLoadData()
        {
            string qry = "SELECT * FROM products WHERE pID = @id";
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            DataTable dt = MainClass.GetDataTable(qry, ht);

            if (dt.Rows.Count > 0)
            {
                txtName.Text  = dt.Rows[0]["pName"].ToString();
                txtPrice.Text = dt.Rows[0]["pPrice"].ToString();

                if (dt.Rows[0]["pImage"] != System.DBNull.Value)
                {
                    byte[] imageArray = (byte[])dt.Rows[0]["pImage"];
                    if (imageArray.Length > 0)
                        txtImage.Image = Image.FromStream(new MemoryStream(imageArray));
                }
            }
        }
    }
}
