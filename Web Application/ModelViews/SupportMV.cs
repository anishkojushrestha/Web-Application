using System.ComponentModel.DataAnnotations;

namespace Web_Application.ModelViews
{
    public class SupportMV
    {
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string ClientName { get; set; }
        [Required]
        public string CallBy { get; set; }
        [Required]
        public string SupportStaff { get; set; }
        [Required]

        public DateOnly Date { get; set; }
        [Required]
        public string Issue { get; set; }
        [Required]
        public string FeedBack { get; set; }
        [Required]
        public string Remote { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
