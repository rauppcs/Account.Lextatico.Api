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
            services.AddRabbitMqAccountHost(configuration);

            return services;
        }

        private static IServiceCollection AddRabbitMqAccountHost(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit<IAccountBus>(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Publish<DomainEvent>(x => x.Exclude = true);

                    cfg.Publish<INotification>(x => x.Exclude = true);

                    cfg.ConfigurationRabbitMqAccountHost(configuration);

                    cfg.AddRabbitMqMassTransitDirectPublisher<UserCreatedEvent>("lextatico.exchange.UserCreatedEvent");

                    cfg.AddRabbitMqMassTransitDirectPublisher<UserUpdatedEvent>("lextatico.exchange.UserUpdatedEvent");

                    cfg.UseRawJsonSerializer(isDefault: true);

                    cfg.UseRawJsonDeserializer(isDefault: true);

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        public static IServiceCollection AddLextaticoMassTransitWithServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServiceBusAccountHost(configuration);

            return services;
        }

        private static IServiceCollection AddServiceBusAccountHost(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit<IBus>(x =>
            {
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Publish<DomainEvent>(x => x.Exclude = true);

                    cfg.Publish<INotification>(x => x.Exclude = true);

                    cfg.ConfigurationServiceBusAccountHost(configuration);

                    cfg.AddServiceBusMassTransitDirectPublisher<UserCreatedEvent>("lextatico.exchange.UserCreatedEvent");

                    cfg.AddServiceBusMassTransitDirectPublisher<UserUpdatedEvent>("lextatico.exchange.UserUpdatedEvent");

                    cfg.UseRawJsonSerializer(isDefault: true);

                    cfg.UseRawJsonDeserializer(isDefault: true);

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
