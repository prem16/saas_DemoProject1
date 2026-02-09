using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SaaSApp.BAL.Dto;
using SaaSApp.BAL.Interface;
using SaaSApp.DAL.Auth;
using SaaSApp.DAL.Data;
using SaaSApp.DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SaaSApp.BAL.Service
{
    public class VendorService : IVendor
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        public VendorService(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<CalculatorSessionVM> CalculatorSession(CalculatorSessionVM session)
        {
            try
            {
                string userId =  _context.Users.Where(u => u.UserRole == session.Role.ToString()).Select(u => u.Id).FirstOrDefault();
                var tenantId =  _context.Users.Where(p=> p.UserRole == session.Role.ToString()).Select(p => p.TenantId).FirstOrDefault();
                var sessionData = new CalculatorSession
                {
                    SessionId = Guid.NewGuid(),
                    VendorId = Convert.GetTypeCode(userId) == TypeCode.String ? Guid.Parse(userId) : Guid.Empty,
                    //TenantId = Convert.GetTypeCode(tenantId) == TypeCode.String ? Guid.Parse(tenantId.ToString()) : Guid.Empty,
                    TenantId = tenantId != null ? Guid.Parse(tenantId.ToString()) : Guid.Empty,
                    CreatedAt = DateTime.UtcNow
                };
                _context.calculatorSessions.Add(sessionData);
                await _context.SaveChangesAsync();
                session.SessionId = sessionData.SessionId;
                return session;
            }
            catch (Exception ex)
            {

                throw new Exception("Error creating calculator session: " + ex.Message);
            }
        }
        public async Task<CalculatorCalculationResponseVM> CalculatorCalculation(CalculatorRequestVM calculation)
        {
			try
			{
                //string userId = _context.Users.Where(u => u.UserRole == calculation.Role.ToString()).Select(u => u.Id).FirstOrDefault();
                decimal result = calculation.Operator switch
                {
                    _ when calculation.Operator == "+" => calculation.Operand1 + calculation.Operand2,
                    _ when calculation.Operator == "-" => calculation.Operand1 - calculation.Operand2,
                    _ when calculation.Operator == "*" => calculation.Operand1 * calculation.Operand2,
                    _ when calculation.Operator == "/" => calculation.Operand2 == 0
                    ? throw new DivideByZeroException("Cannot divide by zero")
                                : calculation.Operand1 / calculation.Operand2,
                    _ => throw new InvalidOperationException("Invalid operator")
                };
                var operationData = new CalculatorOperation
                {
                    SessionId = Convert.GetTypeCode(calculation.SessionId) == TypeCode.String ? Guid.Parse(calculation.SessionId) : Guid.Empty,
                    Operand1 = calculation.Operand1,
                    Operand2 = calculation.Operand2,
                    Operator = calculation.Operator,
                    Result = result,
                    CreatedAt = DateTime.UtcNow
                };
                _context.calculatorOperations.Add(operationData);
                await _context.SaveChangesAsync();
               
                return new CalculatorCalculationResponseVM
                {
                    SessionId = calculation.SessionId.ToString(),
                    Operand1 = calculation.Operand1,
                    Operand2 = calculation.Operand2,
                    Operator = calculation.Operator,
                    Result = result
                };
            }
			catch (Exception ex)
			{

				throw new Exception("Error performing calculation: " + ex.Message);
            }
        }
        public async Task<List<CalculatorCalculationResponseVM>> AllCalculatorCalculation()
        {
            try
            {
                List<CalculatorCalculationResponseVM> allCalculation = new List<CalculatorCalculationResponseVM>();
                var calculationData = await _context.calculatorOperations.ToListAsync();
                return calculationData.Select(p => new CalculatorCalculationResponseVM
                {
                    SessionId = p.SessionId.ToString(),
                    Operand1 = p.Operand1,
                    Operand2 = p.Operand2,
                    Operator = p.Operator,
                    Result = p.Result
                }).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception("Data not found");
            }
        }
        public async Task<UpdateCalculatorVM> UpdateCalculatorCalculation(UpdateCalculatorVM update)
        {
            try
            {
                string getId = _context.calculatorOperations.Where(p => p.Id.ToString() == update.Id).Select(p => p.Id.ToString()).FirstOrDefault();
                if (getId == null )
                {
                    throw new Exception("Data not found");
                }
                // UpdateCalculatorVM updateCalculatorVM = new UpdateCalculatorVM();
                decimal result = update.Operator switch
                {
                    _ when update.Operator == "+" => update.Operand1 + update.Operand2,
                    _ when update.Operator == "-" => update.Operand1 - update.Operand2,
                    _ when update.Operator == "*" => update.Operand1 * update.Operand2,
                    _ when update.Operator == "/" => update.Operand2 == 0
                                ? throw new DivideByZeroException("Cannot divide by zero")
                                : update.Operand1 / update.Operand2,
                    _ => throw new InvalidOperationException("Invalid operator")
                };
                var updateCalculatorVM = new CalculatorOperation
                {
                    Id = Guid.Parse(getId),
                    SessionId = Convert.GetTypeCode(update.SessionId) == TypeCode.String ? Guid.Parse(update.SessionId) : Guid.Empty,
                    Operand1 = update.Operand1,
                    Operand2 = update.Operand2,
                    Operator = update.Operator,
                    Result = result,
                };
                 _context.calculatorOperations.Update(updateCalculatorVM);
                await _context.SaveChangesAsync();
                //return new UpdateCalculatorVM
                //{
                //    Id = update.Id,
                //    SessionId = update.SessionId,
                //    Operand1 = update.Operand1,
                //    Operand2 = update.Operand2,
                //    Operator = update.Operator,
                //    //Result = result.ToString()
                //};

                return updateCalculatorVM != null ? new UpdateCalculatorVM
                {
                    Id = update.Id,
                    SessionId = update.SessionId,
                    Operand1 = update.Operand1,
                    Operand2 = update.Operand2,
                    Operator = update.Operator,
                    //Result = result.ToString()
                } : throw new Exception("Update failed");
            }
            catch (Exception ex)
            {

                throw new Exception("Update failed" + ex.Message);
            }
        }

        public async Task<FinalizeAndPayResponseVM> FinalCalculation(string sessionId)
        {
            try
            {
                var session = await _context.calculatorSessions.Include(s=> s.Operations).FirstOrDefaultAsync(s => s.SessionId.ToString() == sessionId);
                if (session == null)
                {
                    throw new Exception("Session not found");
                }
                //if(session.Operations != null && session.Operations.Any())
                //{
                //    _context.calculatorOperations.RemoveRange(session.Operations);
                //}
                decimal finalAmount = session.Operations != null ? session.Operations.Sum(o => o.Result) : 0;
                if(finalAmount<= 0)
                {
                    //session.Operations = finalAmount;
                    //session.Status = "FinalizedNoPayment";
                    //await _context.SaveChangesAsync();
                    throw new Exception("No payment needed as the final amount is zero or negative");                 
                }
                return new FinalizeAndPayResponseVM
                {
                    SessionId = session.SessionId.ToString(),
                    FinalAmount = finalAmount,
                    Status = "NoPaymentNeeded",
                    Message = "Calculation complete - no payment required"
                };
            }
            catch (Exception ex)
            {

                throw new Exception("Delete failed" + ex.Message);
            }
        }
    }
}
