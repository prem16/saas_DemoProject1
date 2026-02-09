using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.BAL.Dto
{
    public class UpdateCalculatorVM
    {
        public string Id { get; set; }
        public string SessionId { get; set; }
        public decimal Operand1 { get; set; }
        public decimal Operand2 { get; set; }
        public string Operator { get; set; }
        //public string? Result { get; set; }

    }
}
