using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Account.Lextatico.Infra.Data.Context;
using Account.Lextatico.Domain.Models;
using Account.Lextatico.Infra.Identity.Extensions;

namespace Account.Lextatico.Infra.CrossCutting.IoC
{
    public static class ConfigureIdentity
    {
        public static IServiceCollection AddLextaticoIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                .AddEntityFrameworkStores<LextaticoContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
