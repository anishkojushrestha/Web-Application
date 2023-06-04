using System.ComponentModel.DataAnnotations;

namespace Web_Application.ModelViews
{
    public class RegisterVM
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

        [Required]
        [MaxLength(50)]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(50)]
        public string ConfirmPassword { get; set; }

        public int CompanyId { get; set; }
    }
}
