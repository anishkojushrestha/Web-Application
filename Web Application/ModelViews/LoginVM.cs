using System.ComponentModel.DataAnnotations;

namespace Web_Application.ModelViews
{
    public class LoginVM
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
