namespace Web_Application.ModelViews
{
    public class AMCEntryVM
    {
        public int? Id { get; set; }
        public string? Client { get; set; }
        public string OpenDate { get; set; }
        public long AMCAmount { get; set; }
        public string? FollowUpDate { get; set; }
        public string? CloseDate { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
    }
}
