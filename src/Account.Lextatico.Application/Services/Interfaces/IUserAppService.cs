using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Application.Dtos.User;

namespace Account.Lextatico.Application.Services.Interfaces
{
    public interface IUserAppService
    {
        Task<UserDetailDto> GetUserLoggedAsync();
        Task<bool> CreateAsync(UserSignInDto userSignIn);
        Task<bool> ForgotPasswordAsync(UserForgotPasswordDto userForgotPassword);
        Task<bool> ResetPasswordAsync(UserResetPasswordDto userResetPassword);
    }
}
