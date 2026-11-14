namespace Restourant_Adisyon.Core.Enums
{
    public enum TableStatus
    {
        Empty = 0,         // Boş / Müsait (Yeşil)
        Occupied = 1,      // Dolu / Masada Müşteri Var (Kırmızı/Pembe)
        BillRequested = 2, // Hesap İstendi (Turuncu)
        Reserved = 3       // Rezerve (Mavi)
    }
}
