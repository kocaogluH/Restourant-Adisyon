using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Restourant_Adisyon.Properties;

namespace Restourant_Adisyon
{
	public static class MainClass
	{
		private static readonly string con_string = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
		private static string user;
		
		public static string USER
		{
			get { return user; }
			private set { user = value; }
		}

		public static bool IsValidUser(string username, string password)
		{
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentException("Kullanıcı adı ve şifre boş olamaz.");
			}

			try
			{
				using (var con = new SqlConnection(con_string))
				{
					const string query = "SELECT uNAME FROM userss WHERE username = @user AND upass = @pass";
					using (var cmd = new SqlCommand(query, con))
					{
						cmd.Parameters.AddWithValue("@user", username);
						cmd.Parameters.AddWithValue("@pass", password);
						
						con.Open();
						var result = cmd.ExecuteScalar();
						
						if (result != null)
						{
							USER = result.ToString();
							SaveUserSettings(username, Settings.Default.RememberMe);
							return true;
						}
						return false;
					}
				}
			}
			catch (SqlException ex)
			{
				throw new Exception($"Veritabanı hatası: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Beklenmeyen hata: {ex.Message}", ex);
			}
		}

		public static void SaveUserSettings(string username, bool rememberMe)
		{
			try
			{
				Settings.Default.LastUser = rememberMe ? username : string.Empty;
				Settings.Default.RememberMe = rememberMe;
				Settings.Default.Save();
			}
			catch (Exception ex)
			{
				throw new Exception($"Ayarlar kaydedilirken hata oluştu: {ex.Message}", ex);
			}
		}

		public static string GetLastUser()
		{
			try
			{
				return Settings.Default.LastUser;
			}
			catch (Exception ex)
			{
				throw new Exception($"Son kullanıcı bilgisi alınamadı: {ex.Message}", ex);
			}
		}

		public static bool GetRememberMe()
		{
			try
			{
				return Settings.Default.RememberMe;
			}
			catch (Exception ex)
			{
				throw new Exception($"Beni hatırla ayarı alınamadı: {ex.Message}", ex);
			}
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