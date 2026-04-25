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
    public partial class frmLogin: Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

		private void btnexit_Click(object sender, EventArgs e)
		{
            Application.Exit();
		}

		private void btnlogin_Click(object sender, EventArgs e)
		{
            // Temporary Bypass for development
            MainClass.USER = "Admin";
            MainClass.ROLE = "Admin";

            this.Hide();
            formMain form = new formMain();
            form.Show();
		}
	}
}
