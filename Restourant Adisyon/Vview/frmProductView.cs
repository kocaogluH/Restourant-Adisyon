using Restourant_Adisyon.Mmodel;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace Restourant_Adisyon.Vview
{
    public partial class frmProductView : SampleView
    {
        public frmProductView()
        {
            InitializeComponent();
        }

        private void frmProductView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        public void GetData()
        {
            string qry = @"SELECT p.pID, p.pName, p.pPrice, p.CategoryID, c.catName, p.pBarcode
                           FROM products p
                           INNER JOIN category c ON c.catID = p.CategoryID
                           WHERE p.pName LIKE @search
                           ORDER BY c.catName, p.pName";
            Hashtable ht = new Hashtable();
            ht.Add("@search", "%" + txtSearch.Text.Trim() + "%");

            DataTable dt = MainClass.GetDataTable(qry, ht);

            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvPrice);
            lb.Items.Add(dgvcatID);
            lb.Items.Add(dgvCat);

            guna2DataGridView1.CellFormatting -= gv_Cell;
            guna2DataGridView1.CellFormatting += gv_Cell;

            for (int i = 0; i < lb.Items.Count; i++)
                guna2DataGridView1.Columns[((DataGridViewColumn)lb.Items[i]).Name].DataPropertyName = dt.Columns[i].ColumnName;
            guna2DataGridView1.DataSource = dt;
        }

        private void gv_Cell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            { count++; row.Cells[0].Value = count; }
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmProductAdd());
            GetData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell == null) return;
            string colName = guna2DataGridView1.CurrentCell.OwningColumn.Name;

            if (colName == "dgvedit")
            {
                frmProductAdd frm = new frmProductAdd();
                frm.id  = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                frm.cID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvcatID"].Value);
                MainClass.BlurBackground(frm);
                GetData();
            }

            if (colName == "dgvdel")
            {
                guna2MessageDialog1.Icon    = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                if (guna2MessageDialog1.Show("Bu ürünü silmek istiyor musunuz?") == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                    string qry = "DELETE FROM products WHERE pID=@id";
                    Hashtable ht = new Hashtable();
                    ht.Add("@id", id);
                    MainClass.Sql(qry, ht);

                    guna2MessageDialog1.Icon    = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Ürün silindi.");
                    GetData();
                }
            }
        }
    }
}
