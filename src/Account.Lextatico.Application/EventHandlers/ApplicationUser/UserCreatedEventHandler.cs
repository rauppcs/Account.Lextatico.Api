using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Events;
using MassTransit;
using MediatR;

namespace Account.Lextatico.Application.EventHandlers.ApplicationUser
{
    public class UserCreatedEventHandler : BaseEventHandler<UserCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public UserCreatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public override async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(notification, x =>
            {
                x.SetRoutingKey(notification.RountingKey);
            });
        }
    }
}
