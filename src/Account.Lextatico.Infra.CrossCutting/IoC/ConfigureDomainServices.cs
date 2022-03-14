using Account.Lextatico.Domain.Interfaces.Services;
using Account.Lextatico.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureDomainServices
    {
        public static IServiceCollection AddLextaticoDomainServices(this IServiceCollection services)
        {
            // DOMAIN SERVICES
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
