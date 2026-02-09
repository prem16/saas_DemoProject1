using SaaSApp.BAL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.BAL.Interface
{
    public interface IVendor
    {
        public Task<CalculatorSessionVM> CalculatorSession(CalculatorSessionVM session);
        public Task<CalculatorCalculationResponseVM> CalculatorCalculation(CalculatorRequestVM calculation);
        public Task<List<CalculatorCalculationResponseVM>> AllCalculatorCalculation();
        public Task<UpdateCalculatorVM> UpdateCalculatorCalculation(UpdateCalculatorVM update);
        public Task<FinalizeAndPayResponseVM> FinalCalculation(string sessionId);
    }
}
