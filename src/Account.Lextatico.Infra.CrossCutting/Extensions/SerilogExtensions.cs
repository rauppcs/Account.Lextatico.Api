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
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            builder.UseSerilog();
        }
    }
}
