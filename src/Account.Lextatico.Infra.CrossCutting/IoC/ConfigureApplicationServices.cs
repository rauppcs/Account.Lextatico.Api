using Microsoft.Extensions.DependencyInjection;

namespace Account.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureApplicationServices
    {
        public static IServiceCollection AddLextaticoApplicationServices(this IServiceCollection services)
        {
            // services.AddScoped<IAuthAppService, AuthAppService>();

            return services;
        }
    }
}
