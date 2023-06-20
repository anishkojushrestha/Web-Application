namespace Web_Application.ModelViews
{
    public class CompanyMV
    {
        public int? Id { get; set; }


        public string? CompanyName { get; set; }
        public string? Category { get; set; }
        public string? Email { get; set; }

        public int? PanNumber { get; set; }

        public string? Address { get; set; }
        //public string? City { get; set; }
        public string? District { get; set; }
        
        public string? Country { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? RDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public string? VFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string? VTo { get; set; }

        public List<ContactPersonVM>? contactPersonVM { get; set; }
        
    }
}
