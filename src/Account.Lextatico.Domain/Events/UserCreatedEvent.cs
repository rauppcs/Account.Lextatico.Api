using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Models;

namespace Account.Lextatico.Domain.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        public ApplicationUser ApplicationUser { get; }

        public UserCreatedEvent(ApplicationUser applicationUser)
            : base("lextatico.UserCreated")
        {
            ApplicationUser = applicationUser;
        }
    }
}
