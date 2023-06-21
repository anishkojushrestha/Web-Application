namespace Web_Application.ModelViews
{
    public class AMCEntryVM
    {
        public int? Id { get; set; }
        public string? Client { get; set; }
        public DateTime OpenDate { get; set; }
        public int AMCAmount { get; set; }
        public DateTime FollowUpDate { get; set; }
        public DateTime CloseDate { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
    }
}
