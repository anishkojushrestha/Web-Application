using System.ComponentModel.DataAnnotations;

namespace Web_Application.ModelViews
{
    public class UserVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        public string Profile { get; set; }

        public bool IsActive { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? NewPassword { get; set; }
        public string? ValidFrom { get; set; }
        public string? ValidTo { get; set; }
        public string? Address { get; set; }
        public string? CompanyEmail { get; set; }
        public string? RegistrationDate { get; set; }
    }
}
