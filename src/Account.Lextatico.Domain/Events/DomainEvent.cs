using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Account.Lextatico.Domain.Events
{
    public abstract class DomainEvent : INotification
    {
        public DomainEvent(string routingKey)
        {
            RountingKey = routingKey;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();

        public string RountingKey { get; private set; }
    }
}
