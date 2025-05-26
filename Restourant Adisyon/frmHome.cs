using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Restourant_Adisyon
{
	public partial class frmHome : Form
	{
		public frmHome()
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Dock = DockStyle.Fill;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			// Form yüklendiğinde yapılacak işlemler
		}
	}
}
