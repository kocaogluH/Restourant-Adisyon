using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restourant_Adisyon
{
    public partial class SampleAdd : Form
    {
        public SampleAdd()
        {
            InitializeComponent();
            EnableControls();
            label1.BringToFront();
            guna2Panel1.BringToFront();
        }

        protected virtual void EnableControls()
        {
            // Tüm kontrolleri aktif hale getir
            if (guna2Panel1 != null) guna2Panel1.Enabled = true;
            if (guna2Panel2 != null) guna2Panel2.Enabled = true;
            if (label1 != null) label1.Enabled = true;
            if (btnSave != null) btnSave.Enabled = true;
            if (btnClose != null) btnClose.Enabled = true;
            if (guna2PictureBox1 != null) guna2PictureBox1.Enabled = true;
        }

        protected virtual void btnSave_Click(object sender, EventArgs e)
        {
        }

        protected virtual void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
