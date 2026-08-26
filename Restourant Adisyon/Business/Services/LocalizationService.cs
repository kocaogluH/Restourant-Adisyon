using System;
using System.Collections.Generic;
using Restourant_Adisyon.Core.Enums;

namespace Restourant_Adisyon.Business.Services
{
    public class LocalizationService
    {
        private static LocalizationService _instance;
        public static LocalizationService Instance => _instance ?? (_instance = new LocalizationService());

        public Language CurrentLanguage { get; private set; } = Language.TR;

        public event EventHandler OnLanguageChanged;

        private readonly Dictionary<string, string> _tr = new Dictionary<string, string>
        {
            // Menü Butonları
            { "Nav_Home", "Ana Sayfa" },
            { "Nav_Categories", "Kategoriler" },
            { "Nav_Products", "Ürünler" },
            { "Nav_Tables", "Masalar" },
            { "Nav_Staff", "Personel" },
            { "Nav_POS", "POS Satış" },
            { "Nav_Kitchen", "Mutfak Ekranı" },
            { "Nav_Waiter", "Garson Servis" },
            { "Nav_Inventory", "Stok Takibi" },
            { "Nav_Reports", "Raporlar" },
            { "Nav_Settings", "Ayarlar" },

            // Genel Başlıklar & Butonlar
            { "App_Title", "RESTORAN ADİSYON\nYönetim Sistemi" },
            { "Active_User", "👤 Aktif Kullanıcı" },
            { "Login_Title", "Giriş Yap" },
            { "Login_Subtitle", "Lütfen bilgilerinizi giriniz" },
            { "Username", "Kullanıcı Adı veya PIN" },
            { "Password", "Şifre" },
            { "Login_Btn", "Giriş Yap" },
            { "Exit_Btn", "Çıkış" },
            { "Search", "Ara..." },
            { "Add_New", "Yeni Ekle" },
            { "Save", "Kaydet" },
            { "Close", "Kapat" },
            { "Cancel", "İptal" },
            { "Delete", "Sil" },
            { "Edit", "Düzenle" },
            { "Total", "Toplam" },
            { "Success", "Başarılı" },
            { "Error", "Hata" },
            { "Warning", "Uyarı" },
            { "Confirm_Delete", "Bu kaydı silmek istediğinize emin misiniz?" },
            { "Deleted_Successfully", "Başarıyla silindi." },
            { "Saved_Successfully", "Başarıyla kaydedildi." },

            // Checkout / Ödeme
            { "Checkout_Title", "Ödeme Al / Fiş Kes" },
            { "Bill_Amount", "Fatura Tutarı" },
            { "Received_Amount", "Alınan Tutar" },
            { "Change_Amount", "Para Üstü" },
            { "Pay_Bill", "Ödemeyi Tamamla" },
            { "Split_Payment_Success", "Parçalı ödeme alındı! Kalan Tutar: " },
            { "Payment_Completed", "Hesabın tamamı kapatıldı! Fiş yazdırılıyor..." },

            // POS Ekranı
            { "All_Categories", "Tümü" },
            { "POS_NewOrder", "Yeni Sipariş" },
            { "POS_DineIn", "Masada" },
            { "POS_TakeAway", "Gel-Al" },
            { "POS_Delivery", "Paket" },
            { "POS_KOT", "Mutfağa Gönder (KOT)" },
            { "POS_BillList", "Adisyonlar" },
            { "POS_Checkout", "Hesap Kapat" },
            { "Order_Saved", "Sipariş mutfağa gönderildi!" },
            { "Select_Order_Type", "Lütfen sipariş tipini seçin (Masada/Paket/Gel-Al)." },
            { "Cart_Empty", "Lütfen önce sepetinize ürün ekleyin." },
            { "Barcode_NotFound", "Ürün bulunamadı: " },

            // Mutfak & Garson
            { "Start_Cooking", "Pişirmeye Başla" },
            { "Mark_Ready", "Hazır!" },
            { "Mark_Served", "Servis Edildi" },
            { "Status_Pending", "Bekliyor" },
            { "Status_Cooking", "Pişiriliyor" },
            { "Status_Ready", "Hazır" },
            { "Status_Served", "Servis Edildi" },
            { "Table", "Masa" },
            { "Waiter", "Garson" },
            { "Time", "Saat" },

            // Kategori, Masa, Ürün, Personel Formları
            { "Cat_Name", "Kategori Adı" },
            { "Cat_Header", "Kategori Yönetimi" },
            { "Table_Name", "Masa Adı" },
            { "Table_Header", "Masa Yönetimi" },
            { "Table_Select", "Masa Seçimi" },
            { "Product_Name", "Ürün Adı" },
            { "Price", "Fiyat" },
            { "Category", "Kategori" },
            { "Product_Header", "Ürün Yönetimi" },
            { "Browse_Image", "Resim Seç" },
            { "Staff_Name", "Personel Adı" },
            { "Phone", "Telefon" },
            { "Role", "Rol" },
            { "Staff_Header", "Personel Yönetimi" },

            // Raporlar & Stok
            { "Total_Revenue", "Toplam Ciro" },
            { "Total_Orders", "Toplam Sipariş" },
            { "Top_Products", "En Çok Satılan Ürünler" },
            { "Daily_Sales", "Günlük Satış Listesi" },
            { "Print_Report", "Raporu Yazdır" },
            { "Add_Material", "Malzeme Ekle" },
            { "Material_Name", "Malzeme Adı" },
            { "Quantity", "Miktar" },
            { "Unit", "Birim" },
            { "Recipe_Header", "Ürün Reçetesi" },
            { "Stock_Warning", "⚠ STOK UYARISI" }
        };

        private readonly Dictionary<string, string> _en = new Dictionary<string, string>
        {
            // Menu Buttons
            { "Nav_Home", "Home" },
            { "Nav_Categories", "Categories" },
            { "Nav_Products", "Products" },
            { "Nav_Tables", "Tables" },
            { "Nav_Staff", "Staff" },
            { "Nav_POS", "POS Sales" },
            { "Nav_Kitchen", "Kitchen View" },
            { "Nav_Waiter", "Waiter Service" },
            { "Nav_Inventory", "Inventory" },
            { "Nav_Reports", "Reports" },
            { "Nav_Settings", "Settings" },

            // General Titles & Buttons
            { "App_Title", "RESTAURANT POS\nManagement System" },
            { "Active_User", "👤 Active User" },
            { "Login_Title", "Sign In" },
            { "Login_Subtitle", "Please enter your credentials" },
            { "Username", "Username or PIN" },
            { "Password", "Password" },
            { "Login_Btn", "Login" },
            { "Exit_Btn", "Exit" },
            { "Search", "Search..." },
            { "Add_New", "Add New" },
            { "Save", "Save" },
            { "Close", "Close" },
            { "Cancel", "Cancel" },
            { "Delete", "Delete" },
            { "Edit", "Edit" },
            { "Total", "Total" },
            { "Success", "Success" },
            { "Error", "Error" },
            { "Warning", "Warning" },
            { "Confirm_Delete", "Are you sure you want to delete this record?" },
            { "Deleted_Successfully", "Deleted successfully." },
            { "Saved_Successfully", "Saved successfully." },

            // Checkout / Payment
            { "Checkout_Title", "Checkout / Receipt" },
            { "Bill_Amount", "Bill Amount" },
            { "Received_Amount", "Received Amount" },
            { "Change_Amount", "Change" },
            { "Pay_Bill", "Complete Payment" },
            { "Split_Payment_Success", "Partial payment received! Remaining: " },
            { "Payment_Completed", "Bill fully paid! Printing receipt..." },

            // POS Screen
            { "All_Categories", "All" },
            { "POS_NewOrder", "New Order" },
            { "POS_DineIn", "Dine In" },
            { "POS_TakeAway", "Takeaway" },
            { "POS_Delivery", "Delivery" },
            { "POS_KOT", "Send to Kitchen (KOT)" },
            { "POS_BillList", "Bills" },
            { "POS_Checkout", "Checkout" },
            { "Order_Saved", "Order sent to kitchen!" },
            { "Select_Order_Type", "Please select order type (Dine-in/Delivery/Takeaway)." },
            { "Cart_Empty", "Please add items to cart first." },
            { "Barcode_NotFound", "Product not found: " },

            // Kitchen & Waiter
            { "Start_Cooking", "Start Cooking" },
            { "Mark_Ready", "Ready!" },
            { "Mark_Served", "Mark Served" },
            { "Status_Pending", "Pending" },
            { "Status_Cooking", "Cooking" },
            { "Status_Ready", "Ready" },
            { "Status_Served", "Served" },
            { "Table", "Table" },
            { "Waiter", "Waiter" },
            { "Time", "Time" },

            // Category, Table, Product, Staff Forms
            { "Cat_Name", "Category Name" },
            { "Cat_Header", "Category Management" },
            { "Table_Name", "Table Name" },
            { "Table_Header", "Table Management" },
            { "Table_Select", "Select Table" },
            { "Product_Name", "Product Name" },
            { "Price", "Price" },
            { "Category", "Category" },
            { "Product_Header", "Product Management" },
            { "Browse_Image", "Select Image" },
            { "Staff_Name", "Staff Name" },
            { "Phone", "Phone" },
            { "Role", "Role" },
            { "Staff_Header", "Staff Management" },

            // Reports & Inventory
            { "Total_Revenue", "Total Revenue" },
            { "Total_Orders", "Total Orders" },
            { "Top_Products", "Top Selling Products" },
            { "Daily_Sales", "Daily Sales List" },
            { "Print_Report", "Print Report" },
            { "Add_Material", "Add Material" },
            { "Material_Name", "Material Name" },
            { "Quantity", "Quantity" },
            { "Unit", "Unit" },
            { "Recipe_Header", "Product Recipe" },
            { "Stock_Warning", "⚠ STOCK WARNING" }
        };

        public void ChangeLanguage(Language language)
        {
            CurrentLanguage = language;
            OnLanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetString(string key)
        {
            if (string.IsNullOrEmpty(key)) return "";

            var dict = CurrentLanguage == Language.TR ? _tr : _en;
            if (dict.TryGetValue(key, out string val))
                return val;

            return key;
        }
    }
}
