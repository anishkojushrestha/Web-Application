
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
        public string? ResolveBy { get; set; } 

        public string? DeletedBy { get; set; }
        public string? CloseBy { get; set; }

        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? ContactId { get; set; }
        public string? ContactName { get; set; }

        public int? PhoneNumber { get; set; }

    }
}
