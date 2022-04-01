using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using User = Account.Lextatico.Domain.Models.ApplicationUser;

namespace Account.Lextatico.Application.Consumers.ApplicationUser
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly UserManager<User> _userManager;

        public UserCreatedEventConsumer(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            await _userManager.CreateAsync(context.Message.ApplicationUser);
        }
    }
}
