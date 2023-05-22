using System.ComponentModel.DataAnnotations;

namespace Web_Application.ModelViews
{
    public class RegisterVM
    {

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

        [Required]
        [MaxLength(50)]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(50)]
        public string ConfirmPassword { get; set; }
    }
}
