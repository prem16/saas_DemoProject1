using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.BAL.Dto
{
    public class LoginResponseVM
    {
        public string? PhoneNumber { get; set; }
        public string? UserId { get; set; }
        public string? TenantId { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}
