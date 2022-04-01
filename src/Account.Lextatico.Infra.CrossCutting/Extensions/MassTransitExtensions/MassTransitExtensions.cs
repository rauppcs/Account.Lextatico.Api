using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Lextatico.Domain.Events;
using Account.Lextatico.Domain.Models;
using Account.Lextatico.Infra.Services.MessageBroker.Bus;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddLextaticoMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAccountHost(configuration);

            return services;
        }

        private static IServiceCollection AddAccountHost(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Publish<DomainEvent>(x => x.Exclude = true);

                    cfg.Publish<INotification>(x => x.Exclude = true);

                    cfg.ConfigurationAccountHost(configuration);

                    cfg.AddMassTransitDirectPublisher<UserCreatedEvent>("lextatico.exchange:UserCreatedEvent");

                    cfg.AddMassTransitDirectPublisher<UserUpdatedEvent>("lextatico.exchange:UserUpdatedEvent");

                    cfg.UseRawJsonSerializer(isDefault: true);

                    cfg.UseRawJsonDeserializer(isDefault: true);

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
