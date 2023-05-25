namespace Web_Application.ModelViews
{
    public class InstallationMV
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonEmail { get; set; }
        public int PanNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int PhoneNo { get; set; }
        public int MobileNo { get; set; }
        public int NoOfUser { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SupportStaff { get; set; }
        public string ReferenceBy { get; set; }
        public string FeedBack { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
