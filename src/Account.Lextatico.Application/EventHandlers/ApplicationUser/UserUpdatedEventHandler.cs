using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Events;
using Account.Lextatico.Infra.Services.MessageBroker.Bus;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Account.Lextatico.Application.EventHandlers.ApplicationUser
{
    public class UserUpdatedEventHandler : BaseEventHandler<UserUpdatedEvent>
    {
        private readonly IAccountBus _publishEndpoint;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserUpdatedEventHandler(IAccountBus publishEndpoint, IWebHostEnvironment hostEnvironment)
        {
            _publishEndpoint = publishEndpoint;
            _hostEnvironment = hostEnvironment;
        }

        public override async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(notification, x =>
            {
                if (!_hostEnvironment.IsProduction())
                    x.SetRoutingKey(notification.RoutingKey);
            });
        }
    }
}
