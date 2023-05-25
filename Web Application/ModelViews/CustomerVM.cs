using System.ComponentModel.DataAnnotations;

namespace Web_Application.ModelViews
{
    public class CustomerVM
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string ContactPerson { get; set; }

        [Required]
        public int phoneNumber { get; set; }

        [Required]
        public int MobileNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int PanNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
