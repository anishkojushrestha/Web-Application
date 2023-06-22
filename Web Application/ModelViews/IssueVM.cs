
namespace Web_Application.ModelViews
{
    public class IssueVM
    {
        public int? Id { get; set; }
        public string? IssueNo { get; set; }
        public string? IssueDescription { get; set; }
        public string? IssueGeneratorSteps { get; set; }
        public List<IFormFile>? Attachments { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? Status { get; set; }
        public int? AssignTo { get; set; }
        public int? TrasferTo { get; set; }
        public string? TrasferName { get; set; }

        public string? CloseBy { get; set; }

        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? UserId { get; set; }
        public string? Support { get; set; }
        public int? ContactId { get; set; }
        public string? ContactName { get; set; }

        public int? PhoneNumber { get; set; }

        public string? AssignedDate { get; set; }

        public int? TransferTo { get; set; }
        public string? ContactEmail { get; set; }
        public string? AssignedEmail { get; set; }
    }
}
