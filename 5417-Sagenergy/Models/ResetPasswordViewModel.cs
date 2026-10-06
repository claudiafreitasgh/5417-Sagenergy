using System.ComponentModel.DataAnnotations;

namespace _5417_Sagenergy.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string Confirm { get; set; }

        public string Token { get; set; }
    }
}