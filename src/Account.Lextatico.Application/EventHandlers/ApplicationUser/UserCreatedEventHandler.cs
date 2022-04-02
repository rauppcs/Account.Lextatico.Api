using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Account.Lextatico.Application.EventHandlers.ApplicationUser
{
    public class UserCreatedEventHandler : BaseEventHandler<UserCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserCreatedEventHandler(IPublishEndpoint publishEndpoint, IWebHostEnvironment hostEnvironment)
        {
            _publishEndpoint = publishEndpoint;
            _hostEnvironment = hostEnvironment;
        }

        public override async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(notification, x =>
            {
                if (!_hostEnvironment.IsProduction())
                    x.SetRoutingKey(notification.RountingKey);
            });
        }
    }
}
