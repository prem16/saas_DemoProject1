using Microsoft.AspNetCore.Identity;
using SaaSApp.DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.DAL.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserRole { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid TenantId { get; set; }
        public Tenant tenant { get; set; }
    }
}
