using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Restourant_Adisyon.Core.Entities;
using Restourant_Adisyon.Core.Enums;

namespace Restourant_Adisyon.Business.Services
{
    public class AuthService
    {
        public static Staff CurrentUser { get; private set; }

        public bool LoginWithPassword(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            string qry = "SELECT * FROM users WHERE username=@u AND upass=@p LIMIT 1";
            Hashtable ht = new Hashtable();
            ht.Add("@u", username.Trim());
            ht.Add("@p", password.Trim());

            DataTable dt = MainClass.GetDataTable(qry, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                string roleStr = r["uRole"] != DBNull.Value ? r["uRole"].ToString() : "Staff";
                StaffRole role = ParseRole(roleStr);

                CurrentUser = new Staff
                {
                    Id       = Convert.ToInt32(r["userID"]),
                    Username = r["username"].ToString(),
                    Name     = r["uNAME"] != DBNull.Value ? r["uNAME"].ToString() : r["username"].ToString(),
                    Role     = role
                };

                MainClass.USER = CurrentUser.Name;
                MainClass.ROLE = role.ToString();
                return true;
            }
            return false;
        }

        public bool QuickLoginWithPin(string pinCode)
        {
            if (string.IsNullOrWhiteSpace(pinCode)) return false;

            string qry = "SELECT * FROM staff WHERE sPhone=@pin OR staffID=@pin LIMIT 1";
            Hashtable ht = new Hashtable();
            ht.Add("@pin", pinCode.Trim());

            DataTable dt = MainClass.GetDataTable(qry, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                StaffRole role = ParseRole(r["sRole"]?.ToString());

                CurrentUser = new Staff
                {
                    Id       = Convert.ToInt32(r["staffID"]),
                    Name     = r["sName"]?.ToString(),
                    Phone    = r["sPhone"]?.ToString(),
                    PinCode  = pinCode.Trim(),
                    Role     = role
                };

                MainClass.USER = CurrentUser.Name;
                MainClass.ROLE = role.ToString();
                return true;
            }
            return false;
        }

        public void ApplyRolePermissions(Form mainForm, StaffRole role)
        {
            // WinForms basit yetkilendirme
            bool isAdmin   = (role == StaffRole.Admin);
            bool isCashier = (role == StaffRole.Admin || role == StaffRole.Kasiyer);

            // Buton yetkileri Ana Form'da otomatik uygulanır
        }

        private StaffRole ParseRole(string roleStr)
        {
            if (string.IsNullOrEmpty(roleStr)) return StaffRole.Garson;
            if (roleStr.Equals("Admin", StringComparison.OrdinalIgnoreCase))   return StaffRole.Admin;
            if (roleStr.Equals("Kasiyer", StringComparison.OrdinalIgnoreCase)) return StaffRole.Kasiyer;
            if (roleStr.Equals("Mutfak", StringComparison.OrdinalIgnoreCase))  return StaffRole.Mutfak;
            return StaffRole.Garson;
        }
    }
}
