using MassTransit;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace Account.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions
{
    public static class AddPublisherExtensions
    {
        public static IRabbitMqBusFactoryConfigurator AddRabbitMqMassTransitDirectPublisher<T>(this IRabbitMqBusFactoryConfigurator cfg, string exchangeName)
            where T : class
        {
            cfg.AddRabbitMqMassTransitPublisher<T>(exchangeName, ExchangeType.Direct);

            return cfg;
        }

        public static IRabbitMqBusFactoryConfigurator AddRabbitMqMassTransitTopicPublisher<T>(this IRabbitMqBusFactoryConfigurator cfg, string exchangeName)
            where T : class
        {
            cfg.AddRabbitMqMassTransitPublisher<T>(exchangeName, ExchangeType.Topic);

            return cfg;
        }

        public static IRabbitMqBusFactoryConfigurator AddRabbitMqMassTransitPublisher<T>(this IRabbitMqBusFactoryConfigurator cfg,
            string exchangeName,
            string exchangeType = ExchangeType.Fanout)
            where T : class
        {
            cfg.Message<T>(x => x.SetEntityName(exchangeName));

            cfg.Publish<T>(x =>
            {
                x.AutoDelete = true;
                x.ExchangeType = exchangeType;
            });

            return cfg;
        }

        public static IServiceBusBusFactoryConfigurator AddServiceBusMassTransitDirectPublisher<T>(this IServiceBusBusFactoryConfigurator cfg, string exchangeName)
            where T : class
        {
            cfg.AddServiceBusMassTransitPublisher<T>(exchangeName, ExchangeType.Direct);

            return cfg;
        }

        public static IServiceBusBusFactoryConfigurator AddServiceBusMassTransitTopicPublisher<T>(this IServiceBusBusFactoryConfigurator cfg, string exchangeName)
            where T : class
        {
            cfg.AddServiceBusMassTransitPublisher<T>(exchangeName, ExchangeType.Topic);

            return cfg;
        }

        public static IServiceBusBusFactoryConfigurator AddServiceBusMassTransitPublisher<T>(this IServiceBusBusFactoryConfigurator cfg,
            string exchangeName,
            string exchangeType = ExchangeType.Fanout)
            where T : class
        {
            cfg.Message<T>(x => x.SetEntityName(exchangeName));

            cfg.Publish<T>(x =>
            {
            });

            return cfg;
        }
    }
}
