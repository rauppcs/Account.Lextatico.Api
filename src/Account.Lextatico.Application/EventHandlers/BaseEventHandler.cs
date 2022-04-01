using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Events;
using MediatR;

namespace Account.Lextatico.Application.EventHandlers
{
    public abstract class BaseEventHandler<T> : INotificationHandler<T>
        where T : DomainEvent
    {
        public abstract Task Handle(T notification, CancellationToken cancellationToken);
    }
}
