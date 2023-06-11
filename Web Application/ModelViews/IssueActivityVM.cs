namespace Web_Application.ModelViews
{
    public class IssueActivityVM
    {
        public int? Id { get; set; }
        public String? IsseId { get; set; }
        public string? ActivityDescription { get; set; }
        public List<IFormFile>? Attachment { get; set; }
        public string? AttachmentName { get; set; }
    }
}
