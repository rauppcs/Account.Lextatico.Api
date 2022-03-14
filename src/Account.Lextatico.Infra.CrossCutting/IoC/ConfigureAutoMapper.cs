using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureAutoMapper
    {
        public static IServiceCollection AddLextaticoAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.Load("Account.Lextatico.Application"));

            return services;
        }
    }
}
