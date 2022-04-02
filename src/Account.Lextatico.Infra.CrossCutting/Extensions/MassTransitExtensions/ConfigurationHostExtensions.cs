using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Account.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions
{
    public static class ConfigurationHostExtensions
    {
        public static void ConfigurationRabbitMqAccountHost(this IRabbitMqBusFactoryConfigurator cfg, IConfiguration configuration)
        {
            cfg.Host(configuration.GetConnectionString("RabbitMqAccount"), config =>
            {
                config.PublisherConfirmation = true;
            });
        }

        public static void ConfigurationServiceBusAccountHost(this IServiceBusBusFactoryConfigurator cfg, IConfiguration configuration)
        {
            cfg.Host(configuration.GetConnectionString("ServiceBusAccount"), config =>
            {
            });
        }
    }
}
