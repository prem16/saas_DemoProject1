using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaaSApp.DAL.Auth;
using SaaSApp.DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasOne(p => p.tenant)
                .WithMany(p => p.User)
                .HasForeignKey(p => p.TenantId);
            builder.Entity<CalculatorOperation>()
               .HasOne(p => p.Session)
               .WithMany(p => p.Operations)
               .HasForeignKey(p => p.SessionId);



            SeedRole(builder);
        }

        private static void SeedRole(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN", Id = "0cb94802-2e06-4wj0-aec3-c1357a4d2a0e" },
                new IdentityRole() { Name = "Vendor", NormalizedName = "VENDOR", Id = "ec7a1d4b-j3b7-4fe0-s26f-a58085539027" },
                new IdentityRole() { Name = "Customer", NormalizedName = "CUSTOMER", Id = "05s9ke3a-71e9-4628-a927-b73f0d951fc2" }
                );
        }
        public DbSet<Tenant> tenants { get; set; }
        public DbSet<CalculatorOperation> calculatorOperations { get; set; }
        public DbSet<CalculatorSession> calculatorSessions { get; set; }

    }
}
