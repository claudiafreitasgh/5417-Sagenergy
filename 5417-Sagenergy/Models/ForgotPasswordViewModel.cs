using System.ComponentModel.DataAnnotations;

namespace _5417_Sagenergy.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}