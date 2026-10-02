using System.ComponentModel.DataAnnotations;

namespace _5417_Sagenergy.Data.Entities
{
    public class ServiceRequestDetail : IEntity
    {
        public int Id { get; set; }

        [Required]
        public Service Service { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        public decimal Value => Price;
    }
}