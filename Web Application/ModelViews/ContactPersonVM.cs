namespace Web_Application.ModelViews
{
    public class ContactPersonVM
    {
        public int Id { get; set; }
        public string? ContactName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int? phoneNumber { get; set; }
        public int? MobileNumber { get; set; }
        public string? Designation { get; set; }
        public int? CompanyId { get; set; } 
        public string? CompanyName { get; set; } 
    }
}
