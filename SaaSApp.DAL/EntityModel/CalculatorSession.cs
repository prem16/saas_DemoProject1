using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.DAL.EntityModel
{
    public class CalculatorSession
    {
        [Key]   
        public Guid SessionId { get; set; }           // Primary Key
        public Guid VendorId { get; set; }            // Vendor (User) who owns this session
        public Guid TenantId { get; set; }            // Tenant for SaaS isolation
       // public decimal? FinalAmount { get; set; }     // Filled when vendor finalizes calculation
        public DateTime CreatedAt { get; set; }

        // Navigation property - One-to-Many: Session has multiple operations
        public ICollection<CalculatorOperation> Operations { get; set; }
    }
}
