using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System;
using System.CodeDom;
using System.Xml;
using System.Drawing;

namespace Restourant_Adisyon
{
	class MainClass
	{
		public static readonly string con_string = @"Data Source=DESKTOP-PNVQC5R\SQLEXPRESS01;Initial Catalog=RM;Integrated Security=True;Encrypt=False;";
		public static SqlConnection con = new SqlConnection(con_string);

		public static bool IsValidUser(string user, string pass)
		{
			bool isValid = false;
			string qry = "Select * from users where username = '" + user + "' and  upass = '" + pass + "' ";
			SqlCommand cmd = new SqlCommand(qry, con);
			DataTable dt = new DataTable();
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			if (dt.Rows.Count > 0)
			{
				isValid = true;
				USER = dt.Rows[0]["uNAME"].ToString();

			}
			return isValid;


		}

		public static string user;

		public static string USER
		{
			get { return user; }
			private set { user = value; }
		}

		

	    public static int Sql (string qry, Hashtable ht)
		{
			int res = 0;

			

			try
			{
				SqlCommand cmd = new SqlCommand(qry, con);
				cmd.CommandType = CommandType.Text;

				foreach ( DictionaryEntry item in ht)
				{
					cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
				}

				if (con.State == ConnectionState.Closed) { con.Open(); }
				res = cmd.ExecuteNonQuery();
				if (con.State == ConnectionState.Open) { con.Close(); }
			}
			catch (Exception ex) 
			{
				MessageBox.Show(ex.ToString());
				con.Close();			
			}
			return res;
		}


		public static void LoadData(string qry , DataGridView gv,ListBox lb)
		{

			gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormating);
            try
			{
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);

				for (int i = 0; i < lb.Items.Count; i++)
				{
					string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
					gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();
				}
				gv.DataSource = dt;
            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				con.Close();
			}
		}

		private static void gv_CellFormating(object sender, DataGridViewCellFormattingEventArgs e)
		{
			Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
			int count = 0; 

			foreach (DataGridViewRow row in gv.Rows)
			{
				count++;
				row.Cells[0].Value = count;
			}
		}

		public static void BlurBackground(Form Model)
		{
			Form Background = new Form();
			using (Model)
			{
				Background.StartPosition = FormStartPosition.Manual;
				Background.FormBorderStyle = FormBorderStyle.None;
				Background.Opacity = 0.5d;
				Background.BackColor = Color.Black;
				Background.Size = formMain.Instance.Size ;
				Background.Location = formMain.Instance.Location ;
				Background.ShowInTaskbar = false;
				Background.Show();
				Model.Owner = Background;
				Model.ShowDialog(Background);
				Background.Dispose();
			}
		}

		public static void CBFill(string qry, ComboBox cb)
		{
			SqlCommand cmd = new SqlCommand(qry, con);
			cmd.CommandType = CommandType.Text;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt); 

			cb.DisplayMember = "name";
			cb.ValueMember = "id";
			cb.DataSource = dt;
			cb.SelectedIndex = -1;
		}

	} 
}




