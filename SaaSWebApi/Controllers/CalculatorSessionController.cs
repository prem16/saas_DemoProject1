using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaaSApp.BAL.Dto;
using SaaSApp.BAL.Interface;

namespace SaaSWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorSessionController : ControllerBase
    {
        private readonly IVendor _vendorService;
        public CalculatorSessionController(IVendor vendorService)
        {
            _vendorService = vendorService;
        }
        [HttpPost("CreateSession")]
        public async Task<IActionResult> CreateSession([FromBody] SaaSApp.BAL.Dto.CalculatorSessionVM session)
        {
            try
            {
                var result = await _vendorService.CalculatorSession(session);
                if (result == null)
                {
                    return BadRequest("Failed to create session.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while creating the session.");
            }
        }
        [HttpPost("CreateCalculation")]
        public async Task<IActionResult> CreateCalculation(CalculatorRequestVM session)
        {
            try
            {
                var calculation = await _vendorService.CalculatorCalculation(session);
                if (calculation == null)
                {
                    return BadRequest("Failed to create calculation");
                }
                return Ok(calculation);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while creating the calculation.");
            }
        }
        [HttpGet("AllCalculations")]
        public async Task<IActionResult> AllCalculations()
        {
            try
            {
                var calculations = await _vendorService.AllCalculatorCalculation();
                if (calculations == null || !calculations.Any())
                {
                    return NotFound("No calculations found.");
                }
                return Ok(calculations);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving calculations.");
            }
        }
        [HttpPut("UpdateCalculation")]
        public async Task<IActionResult> UpdateCalculation(UpdateCalculatorVM update)
        {
            try
            {
                var result = await _vendorService.UpdateCalculatorCalculation(update);
                if (result == null)
                {
                    return BadRequest("Failed to update calculation.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while updating the calculation.");
            }
        }
        [HttpPost("FinalizeCalculation")]
        public async Task<IActionResult> FinalizeAndPay(string sessionId)
        {
            try
            {
                var finalCalculation = await _vendorService.FinalCalculation(sessionId);
                if (finalCalculation == null)
                {
                    return BadRequest("Failed to finalize calculation and process payment.");
                }
                return Ok(finalCalculation);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to calculate Final Calculation");
            }
        }
    }
}
