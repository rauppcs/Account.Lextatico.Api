using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Account.Lextatico.Application.EventHandlers.ApplicationUser
{
    public class UserUpdatedEventHandler : BaseEventHandler<UserUpdatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserUpdatedEventHandler(IPublishEndpoint publishEndpoint, IWebHostEnvironment hostEnvironment)
        {
            _publishEndpoint = publishEndpoint;
            _hostEnvironment = hostEnvironment;
        }

        public override async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(notification, x =>
            {
                if (!_hostEnvironment.IsProduction())
                    x.SetRoutingKey(notification.RountingKey);
            });
        }
    }
}
