namespace Web_Application.ModelViews
{
    public class InqueryMV
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        public DateTime Miti { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string CompanyEmail { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ReferenceBY { get; set; }
        public string SoftwareType { get; set; }
        public string OrganaizationType { get; set; }
        public int Phone { get; set; }
        public int PanNo { get; set; }
        public string FollowUpBy { get; set; }
        public DateTime NextFollowUpBy { get; set; }
        public string FeedBack { get; set; }

        public IFormFile Attechment { get; set; }
    }
}
