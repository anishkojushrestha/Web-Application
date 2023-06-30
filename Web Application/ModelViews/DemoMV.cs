namespace Web_Application.ModelViews
{
    public class DemoMV
    {
        public int? Id { get; set; }
        public int? QuatationPrice { get; set; }
        public string? SoftwareType { get; set; }
        public int? TotalUser { get; set; }
        public int? NoOfBranch { get; set; }
        public int? NoOfCompany { get; set; }
        public string? SaleStage { get; set; }
        public string? FeedBack { get; set; }
        public List<IFormFile>? Attechment { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set;}
        public string? MarketedBy { get; set; }
        public string? ImplementedBy { get; set; }
        public string? FollowUpDate { get; set; }
    }
}
