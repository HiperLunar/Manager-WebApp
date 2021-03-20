using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager_WebApp.Models
{
    public class User
    {
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        public string Description { get; set; }
        public ICollection<UserAddress> Addresses { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}
