using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Models;
using Account.Lextatico.Infra.Data.Context;
using MediatR;

namespace Account.Lextatico.Infra.Data.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task<int> DispatchDomainEventsAsync(this IMediator mediador, LextaticoContext lextaticoContext)
        {
            var entitiesBase = lextaticoContext.ChangeTracker.Entries<Base>()
                .Where(entityEntry =>
                    entityEntry.Entity.DomainEvents != null && entityEntry.Entity.DomainEvents.Any())
                .ToList();

            var entitiesBaseIdentity = lextaticoContext.ChangeTracker.Entries<BaseIdentityUser>()
                .Where(entityEntry => entityEntry.Entity.DomainEvents != null && entityEntry.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = entitiesBase.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

            domainEvents.AddRange(entitiesBaseIdentity.SelectMany(entityEntry => entityEntry.Entity.DomainEvents));

            entitiesBase.ForEach(entidade => entidade.Entity.ClearDomainEvents());

            entitiesBaseIdentity.ForEach(entidade => entidade.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async eventoDominio => await mediador.Publish(eventoDominio));

            await Task.WhenAll(tasks);

            return domainEvents.Count();
        }
    }
}
