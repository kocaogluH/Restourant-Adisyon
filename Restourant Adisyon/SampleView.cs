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
    public partial class SampleView : Form
    {
        public SampleView()
        {
            InitializeComponent();
        }

        public virtual void btnAdd_Click(object sender, EventArgs e)
        {
            // Base implementation
        }

        public virtual void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Base implementation
        }

        // Protected property to access txtSearch
        protected Guna.UI2.WinForms.Guna2TextBox SearchTextBox
        {
            get { return txtSearch; }
        }
    }
}
