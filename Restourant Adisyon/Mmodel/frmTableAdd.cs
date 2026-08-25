using System;
using System.Collections;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmTableAdd : SampleAdd
    {
        public frmTableAdd()
        {
            InitializeComponent();
        }

        public int id = 0;

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                guna2MessageDialog1.Show("Masa adı boş olamaz.");
                txtName.Focus();
                return;
            }

            string qry;
            if (id == 0)
                qry = "INSERT INTO tables (tName) VALUES (@Name)";
            else
                qry = "UPDATE tables SET tName=@Name WHERE tID=@id";

            Hashtable ht = new Hashtable();
            ht.Add("@id",   id);
            ht.Add("@Name", txtName.Text.Trim());

            if (MainClass.Sql(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Masa kaydedildi.");
                id = 0;
                txtName.Clear();
                txtName.Focus();
            }
        }
    }
}
