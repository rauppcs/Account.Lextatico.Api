using System.Reflection;
using Account.Lextatico.Api.Filters;
using Account.Lextatico.Domain.Configurations;
using Account.Lextatico.Domain.Security;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;

namespace Account.Lextatico.Api.Configurations
{
    public static class ApiConfigurations
    {
        public static IServiceCollection AddLextaticoControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    // FILTERS
                    options.Filters.Add<ValidationModelAttribute>();

                    // CONVENCTIONS
                    options.Conventions.Add(new RouteTokenTransformerConvention(new UrlPatterner()));
                })
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddFluentValidationAutoValidation(options => { options.DisableDataAnnotationsValidation = true; });
            
            services.AddValidatorsFromAssembly(Assembly.Load("Account.Lextatico.Application"));

            return services;
        }

        public static IServiceCollection AddLexitaticoCors(this IServiceCollection services)
        {
            services.AddCors(optionsCors =>
            {
                optionsCors.AddDefaultPolicy(optionsPolicy =>
                {
                    optionsPolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            return services;
        }

        public static IServiceCollection AddLextaticoSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("doc",
                    new OpenApiInfo
                    {
                        Title = "Account Lextatico Api",
                        Version = "v1",
                        Contact = new OpenApiContact
                        {
                            Name = "Cassiano dos Santos Raupp",
                            Email = "cassiano.raupp@outlook.com",
                            Url = new Uri("https://cassiano3795.github.io/cassianoraupp/")
                        }
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o Token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddLextaticoJwtConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var signingConfigurations = new SigningConfiguration(configuration["SecretKeyJwt"]);
            services.AddSingleton(signingConfigurations);

            var tokenConfiguration = new TokenConfiguration();
            configuration.Bind("TokenConfiguration", tokenConfiguration);

            services.AddSingleton(tokenConfiguration);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfiguration.Audience;
                paramsValidation.ValidIssuer = tokenConfiguration.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.RequireExpirationTime = true;
                paramsValidation.ClockSkew = TimeSpan.FromSeconds(30);
            });

            services.AddAuthorizationCore();

            return services;
        }

        public static IServiceCollection AddLextaticoUrlsConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<Urls>(configuration.GetSection("Urls"));

            return services;
        }
    }
}