using Restourant_Adisyon.Core.Enums;

namespace Restourant_Adisyon.Core.Entities
{
    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public StaffRole Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PinCode { get; set; }
    }
}
