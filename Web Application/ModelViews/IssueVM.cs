
namespace Web_Application.ModelViews
{
    public class IssueVM
    {
        public int Id { get; set; }
        public string IssueNo { get; set; }
        public string IssueCreaterName { get; set; }
        public string IssueDescription { get; set; }
        public string IssueGenerationSteps { get; set; }
        public IFormFile Attachments { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
