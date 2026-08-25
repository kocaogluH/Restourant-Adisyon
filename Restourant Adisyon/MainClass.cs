using System;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Restourant_Adisyon
{
    /// <summary>
    /// Uygulamanın merkez sınıfı. SQLite veritabanı bağlantısını, sorgu yardımcılarını
    /// ve ortak UI yardımcılarını barındırır.
    /// </summary>
    class MainClass
    {
        // Veritabanı dosyası uygulamanın yanında RM.db olarak saklanır
        public static readonly string DbPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RM.db");

        public static readonly string con_string =
            $"Data Source={DbPath};Version=3;Journal Mode=WAL;";

        public static SQLiteConnection con = new SQLiteConnection(con_string);

        // ── Oturum Bilgileri ─────────────────────────────────────────────────────
        public static string user;
        public static string role;
        public static string USER  { get { return user; }  set { user  = value; } }
        public static string ROLE  { get { return role; }  set { role  = value; } }

        // ── Global Hata Günlüğü ──────────────────────────────────────────────────
        private static readonly string LogFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");

        public static void LogError(string context, Exception ex)
        {
            try
            {
                File.AppendAllText(LogFile,
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context}] {ex}\r\n\r\n");
            }
            catch { /* Günlük yazılamazsa yut */ }
        }

        // ── Veritabanı Başlatma ──────────────────────────────────────────────────
        /// <summary>
        /// Uygulama ilk açıldığında çağrılır. Yoksa veritabanı dosyasını ve tüm
        /// tabloları oluşturur; varsayılan admin kullanıcısını ekler.
        /// </summary>
        public static void InitializeDatabase()
        {
            try
            {
                string ddl = @"
PRAGMA journal_mode=WAL;

CREATE TABLE IF NOT EXISTS category (
    catID   INTEGER PRIMARY KEY AUTOINCREMENT,
    catName TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS products (
    pID        INTEGER PRIMARY KEY AUTOINCREMENT,
    pName      TEXT    NOT NULL,
    pPrice     REAL    NOT NULL DEFAULT 0,
    CategoryID INTEGER,
    pBarcode   TEXT,
    pImage     BLOB
);

CREATE TABLE IF NOT EXISTS tables (
    tID   INTEGER PRIMARY KEY AUTOINCREMENT,
    tName TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS users (
    userID   INTEGER PRIMARY KEY AUTOINCREMENT,
    username TEXT NOT NULL,
    upass    TEXT NOT NULL,
    uNAME    TEXT,
    uphone   TEXT,
    uRole    TEXT DEFAULT 'Staff'
);

CREATE TABLE IF NOT EXISTS staff (
    staffID INTEGER PRIMARY KEY AUTOINCREMENT,
    sName   TEXT,
    sPhone  TEXT,
    sRole   TEXT
);

CREATE TABLE IF NOT EXISTS tblMain (
    MainID    INTEGER PRIMARY KEY AUTOINCREMENT,
    aDate     TEXT,
    aTime     TEXT,
    TableName TEXT,
    WaiterName TEXT,
    status    TEXT,
    orderType TEXT,
    total     REAL DEFAULT 0,
    received  REAL DEFAULT 0,
    change    REAL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS tblDetails (
    DetailID INTEGER PRIMARY KEY AUTOINCREMENT,
    MainID   INTEGER,
    proID    INTEGER,
    qty      INTEGER,
    price    REAL,
    amount   REAL
);

CREATE TABLE IF NOT EXISTS tblMaterials (
    mID   INTEGER PRIMARY KEY AUTOINCREMENT,
    mName TEXT,
    mQty  REAL DEFAULT 0,
    mUnit TEXT
);

CREATE TABLE IF NOT EXISTS tblRecipe (
    rID       INTEGER PRIMARY KEY AUTOINCREMENT,
    proID     INTEGER,
    mID       INTEGER,
    qtyNeeded REAL DEFAULT 0
);

INSERT OR IGNORE INTO users (rowid, username, upass, uNAME, uRole)
    VALUES (1, 'admin', '123', 'Yönetici', 'Admin');
";
                using (var tmpCon = new SQLiteConnection(con_string))
                {
                    tmpCon.Open();
                    using (var cmd = new SQLiteCommand(ddl, tmpCon))
                        cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError("InitializeDatabase", ex);
                MessageBox.Show("Veritabanı başlatılamadı: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Giriş Doğrulama (SQL Injection kapalı) ──────────────────────────────
        public static bool IsValidUser(string user, string pass)
        {
            bool isValid = false;
            try
            {
                string qry = "SELECT * FROM users WHERE username=@u AND upass=@p";
                using (var cmd = new SQLiteCommand(qry, GetOpenConnection()))
                {
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@p", pass);
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            isValid = true;
                            USER = dt.Rows[0]["uNAME"].ToString();
                            ROLE = dt.Rows[0]["uRole"] != DBNull.Value
                                        ? dt.Rows[0]["uRole"].ToString()
                                        : "Staff";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("IsValidUser", ex);
                MessageBox.Show("Giriş hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { CloseConnection(); }
            return isValid;
        }

        // ── Yardımcı Bağlantı Metotları ─────────────────────────────────────────
        public static SQLiteConnection GetOpenConnection()
        {
            if (con.State == ConnectionState.Closed) con.Open();
            return con;
        }

        public static void CloseConnection()
        {
            if (con.State == ConnectionState.Open) con.Close();
        }

        // ── Genel DML (Insert/Update/Delete) ────────────────────────────────────
        public static int Sql(string qry, Hashtable ht)
        {
            int res = 0;
            try
            {
                using (var cmd = new SQLiteCommand(qry, GetOpenConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    foreach (DictionaryEntry item in ht)
                        cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value ?? DBNull.Value);
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError("Sql(DML)", ex);
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { CloseConnection(); }
            return res;
        }

        // ── Scalar Sorgu (tek değer döner) ──────────────────────────────────────
        public static object SqlScalar(string qry, Hashtable ht = null)
        {
            object res = null;
            try
            {
                using (var cmd = new SQLiteCommand(qry, GetOpenConnection()))
                {
                    if (ht != null)
                        foreach (DictionaryEntry item in ht)
                            cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value ?? DBNull.Value);
                    res = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                LogError("SqlScalar", ex);
            }
            finally { CloseConnection(); }
            return res;
        }

        // ── DataTable Döndüren Sorgu ─────────────────────────────────────────────
        public static DataTable GetDataTable(string qry, Hashtable ht = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = new SQLiteCommand(qry, GetOpenConnection()))
                {
                    if (ht != null)
                        foreach (DictionaryEntry item in ht)
                            cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value ?? DBNull.Value);
                    using (var da = new SQLiteDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                LogError("GetDataTable", ex);
                MessageBox.Show("Veri yükleme hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { CloseConnection(); }
            return dt;
        }

        // ── GridView Yükleme (sütun eşlemeli) ───────────────────────────────────
        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {
            gv.CellFormatting += gv_CellFormating;
            try
            {
                DataTable dt = GetDataTable(qry);
                for (int i = 0; i < lb.Items.Count; i++)
                {
                    string colName = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colName].DataPropertyName = dt.Columns[i].ColumnName;
                }
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                LogError("LoadData", ex);
                MessageBox.Show("Liste yükleme hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void gv_CellFormating(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            int count = 0;
            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        // ── ComboBox Doldurma ────────────────────────────────────────────────────
        public static void CBFill(string qry, ComboBox cb)
        {
            try
            {
                DataTable dt = GetDataTable(qry);
                cb.DisplayMember = "name";
                cb.ValueMember   = "id";
                cb.DataSource    = dt;
                cb.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                LogError("CBFill", ex);
            }
        }

        // ── Arka Plan Bulanıklaştır (Modal Dialog) ───────────────────────────────
        public static void BlurBackground(Form model)
        {
            Form background = new Form();
            using (model)
            {
                background.StartPosition    = FormStartPosition.Manual;
                background.FormBorderStyle  = FormBorderStyle.None;
                background.Opacity          = 0.5d;
                background.BackColor        = Color.Black;
                background.Size             = formMain.Instance.Size;
                background.Location         = formMain.Instance.Location;
                background.ShowInTaskbar    = false;
                background.Show();
                model.Owner = background;
                model.ShowDialog(background);
                background.Dispose();
            }
        }

        // ── Barkod Okuyucu Yardımcısı ────────────────────────────────────────────
        /// <summary>
        /// Barkod ile ürün arar. Ürün bulunamazsa null döner.
        /// Barkod okuyucu klavye emülasyonu yapar; bu metot ilgili TextBox'ın
        /// KeyDown (Enter) veya TextChanged eventından çağrılabilir.
        /// </summary>
        public static DataRow FindProductByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return null;
            string qry = @"SELECT p.*, c.catName FROM products p
                           LEFT JOIN category c ON c.catID = p.CategoryID
                           WHERE p.pBarcode = @barcode LIMIT 1";
            Hashtable ht = new Hashtable();
            ht.Add("@barcode", barcode);
            DataTable dt = GetDataTable(qry, ht);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}
