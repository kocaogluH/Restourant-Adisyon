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
            if (MainClass.IsValidUser(txtuser.Text, txtpass.Text) == true)
            {
                guna2MessageDialog1.Show("invalid username or password");
                return;
            }
            else
            {
                this.Hide();
                formMain form = new formMain();
                form.Show();
            }
         
            
		}
	}
}
