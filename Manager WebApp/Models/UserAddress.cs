using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_WebApp.Models
{
    public class UserAddress
    {
        public Guid UserID { get; set; }
        public Guid AddressID { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
    }
}
