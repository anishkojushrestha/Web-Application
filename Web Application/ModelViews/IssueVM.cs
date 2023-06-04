
namespace Web_Application.ModelViews
{
    public class IssueVM
    {
        public int Id { get; set; }
        public string IssueNo { get; set; }
        public string IssueCreaterName { get; set; }
        public string IssueDescription { get; set; }
        public string IssueGenerationSteps { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Status { get; set; }
        public int AssignTo { get; set; }
        public DateTime ResolveBy { get; set; } 

        public DateTime DeletedBy { get; set; }
        public DateTime CloseBy { get; set; }

        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int ContactId { get; set; }

    }
}
