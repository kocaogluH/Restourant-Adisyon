using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Restourant_Adisyon.UI.Controls;
using Restourant_Adisyon.UI.Theme;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmBillList : SampleAdd
    {
        public frmBillList()
        {
            InitializeComponent();
        }

        public int MainID = 0;

        private void frmBillList_Load(object sender, EventArgs e)
        {
            if (label1 != null)
            {
                label1.Text = "📋 Açık Adisyonlar Listesi";
            }

            if (guna2DataGridView1 != null)
            {
                GridStyler.Apply(guna2DataGridView1, "Şu anda açık adisyon bulunmuyor.");
            }

            // Save butonunu bu ekranda gizleyebiliriz veya yenileme butonu yapabiliriz
            if (btnSave != null)
            {
                btnSave.Text = "🔄 Yenile";
                AppTheme.StyleButton(btnSave, ButtonRole.Info);
            }

            LoadData();
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected override void OnEnterConfirm()
        {
            if (guna2DataGridView1 != null && guna2DataGridView1.CurrentRow != null)
            {
                MainID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                this.Close();
            }
            else
            {
                btnSave_Click(btnSave, EventArgs.Empty);
            }
        }

        private void LoadData()
        {
            string qry = @"SELECT MainID, TableName, WaiterName, orderType, status, total
                           FROM tblMain
                           WHERE status <> 'Paid'
                           ORDER BY MainID DESC";

            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvtable);
            lb.Items.Add(dgvWaiter);
            lb.Items.Add(dgvType);
            lb.Items.Add(dgvStatus);
            lb.Items.Add(dgvTotal);

            MainClass.LoadData(qry, guna2DataGridView1, lb);
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

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell?.OwningColumn.Name == "dgvedit")
            {
                MainID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                this.Close();
            }
        }
    }
}
