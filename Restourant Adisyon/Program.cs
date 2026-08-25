using System;
using System.Windows.Forms;

namespace Restourant_Adisyon
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Global hata yakalama - uygulamanın çökmesini önler
            Application.ThreadException += (s, e) =>
            {
                MainClass.LogError("Application.ThreadException", e.Exception);
                MessageBox.Show(
                    "Beklenmeyen bir hata oluştu:\n" + e.Exception.Message +
                    "\n\nHata detayları error.log dosyasına kaydedildi.",
                    "Uygulama Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                    MainClass.LogError("UnhandledException", ex);
            };

            // SQLite veritabanını başlat (dosya yoksa oluşturur)
            MainClass.InitializeDatabase();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}
