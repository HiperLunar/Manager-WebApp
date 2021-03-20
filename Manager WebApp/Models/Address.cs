using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_WebApp.Models
{
    public class Address
    {
        public Guid ID { get; set; }
        [Required]
        public string Information { get; set; }
        [Display(Name = "Commercial?")]
        public bool IsCommercial { get; set; }
        public ICollection<UserAddress> Users { get; set; }
    }
}
