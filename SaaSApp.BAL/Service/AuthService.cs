using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SaaSApp.BAL.Dto;
using SaaSApp.BAL.Interface;
using SaaSApp.DAL.Auth;
using SaaSApp.DAL.Data;
using SaaSApp.DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SaaSApp.BAL.Service
{
    public class AuthService : IAuth
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        public AuthService(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<RegisterResponseVM> Registration(RegistrationRequestVM registration)
        {
            try
            {
                var existingEmail = await _userManager.FindByEmailAsync(registration.Email);
                var existingPhone = _context.Users.Where(u => u.PhoneNumber == registration.PhoneNumber).Select(u => u.PhoneNumber).FirstOrDefault();
                if (existingEmail != null || existingPhone != null)
                {
                    throw new Exception("Email or Phone number already exists.");
                }
                // Tenant entry
                var tenantData = new Tenant
                {
                    TenantId = Guid.NewGuid(),
                    Name = registration.BusinessName,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.tenants.AddAsync(tenantData);
                await _context.SaveChangesAsync();
                // Users entry
                string name = registration.FirstName.Trim().Split(' ')[0];
                string phone = registration.PhoneNumber.Trim();
                string firstThree = phone.Substring(0, 3);
                string userName = $"{name}{firstThree}";
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = registration.Email,
                    PhoneNumber = registration.PhoneNumber,
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = tenantData.TenantId,
                    UserRole = registration.Role
                };
                var result = await _userManager.CreateAsync(user, registration.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                // Check Roles
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await _roleManager.RoleExistsAsync(UserRoles.Vendor))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Vendor));
                if (!await _roleManager.RoleExistsAsync(UserRoles.Customer))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));
                // Assign Roles
                if (registration.Role == UserRoles.Admin)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                else if (registration.Role == UserRoles.Vendor)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Vendor);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Customer);
                }
                return new RegisterResponseVM
                {
                    UserId = user.Id,
                    TenantId = tenantData.TenantId,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = registration.Role,
                    BusinessName = tenantData.Name
                };
            }
            catch (Exception ex)
            {

                throw new Exception($"Registration failed: {ex.Message}");
            }
        }

        public async Task<VendorResponseVM> VendorRegistration(VendorRegistrationVM registration)
        {
            try
            {
                var existingEmail = await _userManager.FindByEmailAsync(registration.Email);
                var existingPhone = _context.Users.Where(u => u.PhoneNumber == registration.PhoneNumber).Select(u => u.PhoneNumber).FirstOrDefault();
                if (existingEmail != null || existingPhone != null)
                {
                    throw new Exception("Email or Phone number already exists.");
                }               
                // Users entry
                string name = registration.FirstName.Trim().Split(' ')[0];
                string phone = registration.PhoneNumber.Trim();
                string firstThree = phone.Substring(0, 3);
                string userName = $"{name}{firstThree}";
                string tenantId = await _context.tenants.Select(p=> p.TenantId.ToString()).FirstOrDefaultAsync(t => t == registration.TenantId);


                
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = registration.Email,
                    PhoneNumber = registration.PhoneNumber,
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = Convert.GetTypeCode(tenantId) == TypeCode.String ? Guid.Parse(tenantId) : Guid.Empty,
                    UserRole = registration.Role
                };
                var result = await _userManager.CreateAsync(user, registration.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                // Check Roles
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await _roleManager.RoleExistsAsync(UserRoles.Vendor))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Vendor));
                if (!await _roleManager.RoleExistsAsync(UserRoles.Customer))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));
                // Assign Roles
                if (registration.Role == UserRoles.Admin)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                else if (registration.Role == UserRoles.Vendor)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Vendor);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Customer);
                }
                return new VendorResponseVM
                {
                    UserId = user.Id,
                    TenantId = Convert.GetTypeCode(tenantId) == TypeCode.String ? Guid.Parse(tenantId) : Guid.Empty,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = registration.Role,
                };
            }
            catch (Exception ex)
            {

                throw new Exception($"Registration failed: {ex.Message}");
            }
        }

        public async Task<LoginResponseVM> Login(LoginRequestVM login)
        {
            try
            {
                var user = await _context.Users.Include(p => p.tenant)
                    .FirstOrDefaultAsync(u => u.PhoneNumber == login.PhoneNumber);
                if (user == null)
                {
                    throw new Exception("Invalid phone number");
                }
                var passwordCheck = await _userManager.CheckPasswordAsync(user, login.Password);
                if (!passwordCheck)
                {
                    throw new Exception("Invalid password.");
                }
                // Claims (JWT ke andar data)
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("tenantId", user.TenantId.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                );

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        Convert.ToInt32(_configuration["Jwt:ExpireMinutes"])
                    ),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                return new LoginResponseVM
                {
                    PhoneNumber = user.PhoneNumber,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserId = user.Id,
                    TenantId = user.TenantId.ToString(),
                    Role = user.UserRole
                };
            }
            catch (Exception ex)
            {

                throw new Exception($"Login failed: {ex.Message}");
            }
        }

    }
}
