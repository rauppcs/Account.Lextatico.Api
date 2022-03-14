using Account.Lextatico.Infra.CrossCutting.CustomChecks;
using Account.Lextatico.Infra.Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureHealthChecks
    {
        public static IServiceCollection AddLextaticoHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString(nameof(LextaticoContext)));

            sqlStringBuilder.Password = configuration["DbPassword"];

            var connectionString = sqlStringBuilder.ToString();

            services.AddHealthChecks()
                .AddCheck<SelfCheck>("API")
                .AddSqlServer(connectionString, name: "SqlServer");

            return services;
        }
    }
}
