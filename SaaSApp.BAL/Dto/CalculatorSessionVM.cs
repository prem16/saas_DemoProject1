using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.BAL.Dto
{
    public class CalculatorSessionVM
    {
        public Guid SessionId { get; set; }
        /*public Guid VendorId { get; set; }   */         // Vendor (User) who owns this session
        public string? Role { get; set; }
        public string TenantId { get; set; }            // Tenant for SaaS isolation                                                   // public decimal? FinalAmount { get; set; }     // Filled when vendor finalizes calculation
        public DateTime CreatedAt { get; set; }
    }
}
