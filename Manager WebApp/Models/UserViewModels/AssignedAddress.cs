using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_WebApp.Models.UserViewModels
{
    public class AssignedAddress
    {
        public Guid AddressID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}
