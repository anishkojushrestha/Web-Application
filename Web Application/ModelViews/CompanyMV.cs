namespace Web_Application.ModelViews
{
    public class CompanyMV
    {
        public int Id { get; set; }


        public string CompanyName { get; set; }
        public string Email { get; set; }

        public int PanNumber { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime AchiveFrom { get; set; }
        public DateTime AchiveTo { get; set; }

        public List<ContactPersonVM> contactPersonVM { get; set; }
        
    }
}
