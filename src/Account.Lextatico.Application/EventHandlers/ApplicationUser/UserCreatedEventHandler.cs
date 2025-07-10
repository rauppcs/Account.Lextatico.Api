using Account.Lextatico.Domain.Events;
using Account.Lextatico.Infra.Services.MessageBroker.Bus;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Account.Lextatico.Application.EventHandlers.ApplicationUser
{
    public class UserCreatedEventHandler : BaseEventHandler<UserCreatedEvent>
    {
        private readonly IAccountBus _publishEndpoint;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserCreatedEventHandler(IAccountBus publishEndpoint, IWebHostEnvironment hostEnvironment)
        {
            _publishEndpoint = publishEndpoint;
            _hostEnvironment = hostEnvironment;
        }

        public override async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(notification, x =>
            {
                if (!_hostEnvironment.IsProduction())
                    x.SetRoutingKey(notification.RoutingKey);
            });
        }
    }
}
