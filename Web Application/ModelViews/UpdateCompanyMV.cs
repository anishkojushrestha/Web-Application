namespace Web_Application.ModelViews
{

    public class ContactInfo
    {
        public string? ContactName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public int? phoneNumber { get; set; }
        public int? MobileNumber { get; set; }
        public string? Designation { get; set; }
        public int? CompanyId { get; set; }
    }
    public class UpdateCompanyMV
    {
        public int Id { get; set; }


        public string CompanyName { get; set; }
        public string Email { get; set; }

        public int PanNumber { get; set; }

        public string CAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
      
        public List<ContactInfo> ContactInfos { get; set; }
    }
}
