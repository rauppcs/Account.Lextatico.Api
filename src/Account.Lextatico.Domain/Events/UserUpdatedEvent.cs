using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Models;

namespace Account.Lextatico.Domain.Events
{
    public class UserUpdatedEvent : DomainEvent
    {
        public ApplicationUser ApplicationUser { get; }
        public UserUpdatedEvent(ApplicationUser applicationUser) 
            : base("lextatico.UserUpdated")
        {
            ApplicationUser = applicationUser;
        }
    }
}
