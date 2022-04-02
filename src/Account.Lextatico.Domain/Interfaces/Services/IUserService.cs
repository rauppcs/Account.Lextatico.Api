using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Models;

namespace Account.Lextatico.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserLoggedAsync();
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> CreateAsync(ApplicationUser applicationUser, string password);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string password, string resetToken);
    }
}
