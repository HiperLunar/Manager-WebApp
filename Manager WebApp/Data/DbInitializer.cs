using Manager_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_WebApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(ManagerContext context)
        {
            if (context.User.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User { Name = "Angela Martin", Age = 28,     Email = "angela.martin@email.com",   Description="Accountant" },
                new User { Name = "Creed Bratton", Age = 57,     Email = "creed.bratton@email.com",   Description="Quality control" },
                new User { Name = "Michael Scott", Age = 45,     Email = "michael.scott@email.com",   Description="Regional manager" },
                new User { Name = "Darryl Philbin", Age = 35,    Email = "darryl.philbin@email.com",  Description="Deliveryman" },
                new User { Name = "Dwight K. Schrute", Age = 35, Email = "dwight.schrute@email.com",  Description="Sales and assistent regional manager" },
                new User { Name = "Andy Bernard", Age = 32,      Email = "andy.bernardog@email.com",  Description="Sales" },
                new User { Name = "Stanley Hudson", Age = 46,    Email = "stanley.hudson@email.com",  Description="Sales" },
                new User { Name = "Jim Halpert", Age = 32,       Email = "jim.halpert@email.com",     Description="Salesco regional manager" },
                new User { Name = "Ryan Howard", Age = 27,       Email = "ryan.howard@email.com",     Description="Temp" },
                new User { Name = "Phyllis Lapin", Age = 45,     Email = "phyllis.lapin@email.com",   Description="Sales" },
                new User { Name = "Toby Flenderson", Age = 44,   Email = "toby.flanderson@email.com", Description="Human resources" },
                new User { Name = "Roy Anderson", Age = 34,      Email = "roy.anderson@email.com",    Description="Deliveryman" },
                new User { Name = "Kelly Kapoor", Age = 26,      Email = "kelly.kapoor@email.com",    Description="Customer service" },
                new User { Name = "Oscar Martinez", Age = 35,    Email = "oscar.martinez@email.com",  Description="Accountant" },
                new User { Name = "Pam Beesly", Age = 30,        Email = "pam.beasly@email.com",      Description="Sales" },
                new User { Name = "Erin Hannon", Age = 26,       Email = "erin.hannon@email.com",     Description="Receptionist" },
                new User { Name = "Meredith Palmer", Age = 56,   Email = "meredith.palmer@email.com", Description="Supplies" },
                new User { Name = "Kevin Malone", Age = 36,      Email = "kevin.malone@email.com",    Description="Accountant" }
            };

            foreach (User user in users)
            {
                context.User.Add(user);
            }

            var addresses = new Address[]
            {
                new Address { Information = "Av. Brg. Faria Lima, 3477", IsCommercial = true,  },
                new Address { Information = "Av. Paulista, 1578",        IsCommercial = false, },
                new Address { Information = "R. Bobos, 0",               IsCommercial = false, },
                new Address { Information = "Av. Washington Luís",       IsCommercial = true,  },
                new Address { Information = "R. João Brícola, 24",       IsCommercial = true,  }
            };

            foreach (Address address in addresses)
            {
                context.Address.Add(address);
            }

            context.SaveChanges();
        }
    }
}
