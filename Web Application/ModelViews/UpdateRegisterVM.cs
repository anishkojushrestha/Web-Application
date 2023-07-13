using System.ComponentModel.DataAnnotations;

namespace Web_Application.ModelViews
{
    public class UpdateRegisterVM
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
    }
}
