using Microsoft.Extensions.DependencyInjection;

namespace Account.Lextatico.Infra.Identity.User
{
    public static class Abstractions
    {
        public static IServiceCollection AddLextaticoAspNetUser(this IServiceCollection services)
        {
            services.AddScoped<IAspNetUser, AspNetUser>();

            return services;
        }
    }
}
