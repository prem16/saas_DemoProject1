using SaaSApp.DAL.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.DAL.EntityModel
{
    public class Tenant
    {
        [Key]
        public Guid TenantId { get; set; } = Guid.NewGuid(); // PK  
        public string? Name { get; set; } // Business name  
        public DateTime CreatedAt { get; set; }

        public ICollection<ApplicationUser>? User { get; set; } // Navigation property to ApplicationUser  
    }
}
