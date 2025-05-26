using System;
using System.Windows.Forms;

namespace Restourant_Adisyon.Mmodel
{
    public partial class frmCategoryAdd : SampleAdd
    {
        public frmCategoryAdd()
        {
            InitializeComponent();
            base.InitializeComponent();
            label1.Text = "Kategori Ekle";
            btnSave.Text = "KAYDET";
            btnClose.Text = "KAPAT";
        }

        protected override void btnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kaydet butonuna tıklandı!");
        }

        protected override void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;
                Message msg = Message.Create(this.Handle, 0XA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            }
        }
    }
}
