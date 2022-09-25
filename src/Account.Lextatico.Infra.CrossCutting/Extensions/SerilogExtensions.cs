using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Account.Lextatico.Infra.CrossCutting.Extensions
{
    public static class SerilogExtensions
    {
        public static void UseLextaticoSerilog(this IHostBuilder builder, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            builder.UseSerilog((context, cfg) =>
            {
                cfg.Enrich.FromLogContext();

                cfg.Enrich.WithProperty("Application", "Account.Lextatico.Api");

                cfg.Enrich.WithMachineName();

                cfg.Enrich.WithEnvironmentName();

                cfg.Enrich.WithCorrelationId();

                cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

                cfg.MinimumLevel.Override("System", LogEventLevel.Warning);

                cfg.WriteTo.MongoDB(context.Configuration.GetConnectionString("LextaticoMongoDbLogs"), "logs");

                cfg.WriteTo.Console();

                if (hostEnvironment.IsProduction())
                    cfg.MinimumLevel.Information();
                else
                    cfg.MinimumLevel.Debug();
            });
        }
    }
}
