using System;
using System.Collections;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmCategoryAdd : SampleAdd
    {
        public frmCategoryAdd()
        {
            InitializeComponent();
        }

        public int id = 0;

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                guna2MessageDialog1.Show("Kategori adı boş olamaz.");
                txtName.Focus();
                return;
            }

            string qry;
            if (id == 0)
                qry = "INSERT INTO category (catName) VALUES (@Name)";
            else
                qry = "UPDATE category SET catName=@Name WHERE catID=@id";

            Hashtable ht = new Hashtable();
            ht.Add("@id",   id);
            ht.Add("@Name", txtName.Text.Trim());

            if (MainClass.Sql(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Başarıyla kaydedildi.");
                id = 0;
                txtName.Clear();
                txtName.Focus();
            }
        }
    }
}
