using System;
using System.ComponentModel.DataAnnotations;

namespace Manager_WebApp.Models
{
    public class Department
    {
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? UserID { get; set; }
        public User User { get; set; }
    }
}
