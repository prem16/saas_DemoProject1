using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.BAL.Dto
{
    public class VendorResponseVM
    {
        public string? UserId { get; set; }
        public Guid TenantId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
