namespace Web_Application.ModelViews
{
    public class IssueTransferVM
    {
        public int? Id { get; set; }
        public String IssueId { get; set; }
        public string TransferFrom { get; set; }
        public string TransferTo { get; set; }
        public DateTime TransferDate { get; set; }
        public string CurrentStage { get; set; }
    }
}
