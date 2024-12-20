using System.Linq;
using FluentValidation;
using Account.Lextatico.Application.Dtos.User;
using Account.Lextatico.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Account.Lextatico.Application.Validators
{
    public abstract class UserDtoValidator<T> : AbstractValidator<T> where T : UserDto
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserDtoValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected void ValidateEmail()
        {
            RuleFor(userLoginDto => userLoginDto.Email)
                .EmailAddress()
                .WithMessage("Insira um endereço de email válido.");
        }

        protected void ValidatePassword()
        {
            RuleFor(userLoginDto => userLoginDto.Password)
                .Custom((password, context) =>
                {
                    var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();

                    var result = passwordValidator?.ValidateAsync(_userManager, null, password)
                        .GetAwaiter()
                        .GetResult();

                    if (result is not { Succeeded: true }) return;
                    
                    var messages = result.Errors.Select(error => error.Description);
                    foreach (var message in messages)
                    {
                        context.AddFailure("", message);
                    }
                });
        }
    }
}
