using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.DAL.EntityModel
{
    public class CalculatorOperation
    {
        [Key]
        public Guid Id { get; set; }                       // Primary Key
        public Guid SessionId { get; set; }      // Foreign Key to CalculatorSession
        public decimal Operand1 { get; set; }
        public decimal Operand2 { get; set; }
        public string Operator { get; set; }              // "+", "-", "*", "/"
        public decimal Result { get; set; }
        public DateTime CreatedAt { get; set; }
        public CalculatorSession Session { get; set; }
    }
}
