using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Account.Lextatico.Domain.Events;
using Microsoft.AspNetCore.Identity;

namespace Account.Lextatico.Domain.Models
{
    public abstract class Base
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();

        public IReadOnlyList<DomainEvent> DomainEvents
            => _domainEvents?.AsReadOnly() ?? new List<DomainEvent>().AsReadOnly();

        public void AddDomainEvent(DomainEvent evento) => _domainEvents.Add(evento);

        public void RemoveDomainEvent(DomainEvent evento) => _domainEvents.Remove(evento);

        public void ClearDomainEvents() => _domainEvents.Clear();

        public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;

        public void SetUpdateddAt(DateTime updatedAt) => UpdatedAt = updatedAt;
    }

    public abstract class BaseIdentityUser : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();

        public IReadOnlyList<DomainEvent> DomainEvents
            => _domainEvents?.AsReadOnly() ?? new List<DomainEvent>().AsReadOnly();

        public void AddDomainEvent(DomainEvent evento) => _domainEvents.Add(evento);

        public void RemoveDomainEvent(DomainEvent evento) => _domainEvents.Remove(evento);

        public void ClearDomainEvents() => _domainEvents.Clear();

        public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;

        public void SetUpdateddAt(DateTime updatedAt) => UpdatedAt = updatedAt;
    }
}
