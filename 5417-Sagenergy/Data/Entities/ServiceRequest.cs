using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace _5417_Sagenergy.Data.Entities
{
    public class ServiceRequest : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Request date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime RequestDate { get; set; }

        [Required]
        public Client Client { get; set; }

        public string Description { get; set; }

        public IEnumerable<ServiceRequestDetail> Items { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Lines => Items == null ? 0 : Items.Count();

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Items == null ? 0 : Items.Sum(i => i.Value);
    }
}
