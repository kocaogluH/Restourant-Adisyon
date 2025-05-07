using System.Data;
using System.Data.SqlClient;

namespace Restourant_Adisyon
{
	class MainClass
	{
		public static readonly string con_string = @"Data Source=DESKTOP-PNVQC5R\SQLEXPRESS;Initial Catalog=RM;Integrated Security=True;Encrypt=False;";
		public static SqlConnection con = new SqlConnection(con_string);

		public static bool IsValidUser(string user, string pass)
		{
			bool isValid = false;
				string qry = "Select * from userss where username = '"+user+"' and  upass = '"+ pass +"' ";
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

		}
	}



//SqlCommand cmd = new SqlCommand(qry, con);
//cmd.Parameters.AddWithValue("@user", user);
//cmd.Parameters.AddWithValue("@pass", pass);

//SqlDataAdapter da = new SqlDataAdapter(cmd);
//DataTable dt = new DataTable();
//da.Fill(dt);
//No8 User ID = Halil ; Password = 123;