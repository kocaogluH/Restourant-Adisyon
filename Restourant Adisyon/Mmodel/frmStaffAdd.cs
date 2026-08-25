using System;
using System.Collections;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmStaffAdd : SampleAdd
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        public int id = 0;

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                guna2MessageDialog1.Show("Ad Soyad boş olamaz.");
                txtName.Focus();
                return;
            }

            string qry;
            if (id == 0)
                qry = "INSERT INTO staff (sName, sPhone, sRole) VALUES (@Name, @Phone, @Role)";
            else
                qry = "UPDATE staff SET sName=@Name, sPhone=@Phone, sRole=@Role WHERE staffID=@id";

            Hashtable ht = new Hashtable();
            ht.Add("@id",    id);
            ht.Add("@Name",  txtName.Text.Trim());
            ht.Add("@Phone", txtPhone.Text.Trim());
            ht.Add("@Role",  cbRole.Text);

            if (MainClass.Sql(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Başarıyla kaydedildi.");
                id = 0;
                txtName.Clear();
                txtPhone.Clear();
                cbRole.SelectedIndex = -1;
                txtName.Focus();
            }
        }
    }
}
