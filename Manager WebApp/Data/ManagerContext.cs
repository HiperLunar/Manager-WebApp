using Manager_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Manager_WebApp.Data
{
    public class ManagerContext : DbContext
    {
        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<Department> Department { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => new { e.Name })
                .IsUnique(true);

            modelBuilder.Entity<Address>()
                .HasIndex(e => new { e.Information })
                .IsUnique(true);

            modelBuilder.Entity<UserAddress>()
                .HasKey(e => new { e.UserID, e.AddressID });

            modelBuilder.Entity<Department>()
                .HasIndex(e => new { e.Name })
                .IsUnique(true);
        }
    }
}
