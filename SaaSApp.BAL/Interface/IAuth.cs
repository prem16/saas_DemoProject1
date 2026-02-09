using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaaSApp.BAL.Dto;

namespace SaaSApp.BAL.Interface
{
    public interface IAuth
    {
        public Task<RegisterResponseVM> Registration(RegistrationRequestVM registration);
        public Task<VendorResponseVM> VendorRegistration(VendorRegistrationVM registration);
        public Task<LoginResponseVM> Login(LoginRequestVM login);
    }
}
