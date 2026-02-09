using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.BAL.Dto
{
    public class FinalizeAndPayResponseVM
    {
        public string SessionId { get; set; }
        public decimal FinalAmount { get; set; }
        //public string? PaymentOrderId { get; set; }
        //public string? PaymentGatewayUrl { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Message { get; set; }
    }
}
