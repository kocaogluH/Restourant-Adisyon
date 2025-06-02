using Restourant_Adisyon.Vview;
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
	public partial class formMain : Form
	{
		public formMain()
		{
			InitializeComponent();
		}


		public void AddControls(Form f)
		{
			ControlsPanel.Controls.Clear();
			f.Dock= DockStyle.Fill;
			f.TopLevel = false;
			ControlsPanel.Controls.Add(f);
			f.Show();

		}




		private void btnexit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void formMain_Load(object sender, EventArgs e)
		{
			lblUser.Text = MainClass.USER;
		}

		private void btnHome_Click(object sender, EventArgs e)
		{
			AddControls(new frmHome());
		}

        private void btnCatagories_Click(object sender, EventArgs e)
        {
            AddControls(new frmCategoryview());
        }
    }
}
