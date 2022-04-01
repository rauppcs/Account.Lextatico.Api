using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Events;
using MassTransit;

namespace Account.Lextatico.Application.EventHandlers.ApplicationUser
{
    public class UserUpdatedEventHandler : BaseEventHandler<UserUpdatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public UserUpdatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public override async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(notification, x =>
            {
                x.SetRoutingKey(notification.RountingKey);
            });
        }
    }
}
