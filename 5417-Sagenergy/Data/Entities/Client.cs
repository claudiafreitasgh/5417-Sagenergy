using System.ComponentModel.DataAnnotations;

namespace _5417_Sagenergy.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "NIF")]
        public string Nif { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}