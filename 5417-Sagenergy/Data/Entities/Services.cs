using System.ComponentModel.DataAnnotations;

namespace _5417_Sagenergy.Data.Entities
{
    public class Service : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}

