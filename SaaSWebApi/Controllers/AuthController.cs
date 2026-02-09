using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaaSApp.BAL.Dto;
using SaaSApp.BAL.Interface;

namespace SaaSWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        public AuthController(IAuth authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegistrationRequestVM registration)
        {
            try
            {
                var result = await _authService.Registration(registration);
                if (result == null)
                {
                    return BadRequest("Registration failed.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        [HttpPost("vendorregister")]
        public async Task<IActionResult> VendorRegistration(VendorRegistrationVM registration)
        {
            try
            {
                var result = await _authService.VendorRegistration(registration);
                if (result == null)
                {
                    return BadRequest("Registration failed.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestVM login)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(login.PhoneNumber) || string.IsNullOrWhiteSpace(login.Password))
                {
                    return BadRequest("Phone number and password are required.");
                }
                var result = await _authService.Login(login);
                if (result == null)
                {
                    return BadRequest("Login failed");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
