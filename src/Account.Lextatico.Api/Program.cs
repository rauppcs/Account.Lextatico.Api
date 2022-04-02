using System.Net.Mime;
using Account.Lextatico.Api.Configurations;
using Account.Lextatico.Api.Extensions;
using Account.Lextatico.Infra.CrossCutting.Extensions;
using Account.Lextatico.Infra.CrossCutting.IoC;
using Account.Lextatico.Infra.Identity.User;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using HostEnvironmentEnvExtensions = Account.Lextatico.Api.Extensions.HostEnvironmentEnvExtensions;
using Account.Lextatico.Domain.Events;
using Account.Lextatico.Application.EventHandlers;
using Account.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions;

if (HostEnvironmentEnvExtensions.IsDocker())
    Thread.Sleep(30000);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((hostContext, builder) =>
{
    if (hostContext.HostingEnvironment.IsLocalDevelopment())
        builder.AddUserSecrets<Program>();
});

builder.Services
    .AddHttpContextAccessor()
    .AddLextaticoMessage()
    .AddLextaticoAspNetUser()
    .AddMediatR(typeof(BaseEventHandler<>).Assembly,
                typeof(DomainEvent).Assembly)
    .AddLextaticoEmailSettings(builder.Configuration)
    .AddLextaticoUrlsConfiguration(builder.Configuration)
    .AddLextaticoInfraServices()
    .AddLextaticoDomainServices()
    .AddLextaticoAutoMapper()
    .AddLextaticoApplicationServices()
    .AddLextaticoHealthChecks(builder.Configuration)
    .AddLextaticoContext(builder.Configuration)
    .AddLextaticoIdentity()
    .AddLextaticoJwtConfiguration(builder.Configuration)
    .AddLexitaticoCors()
    .AddLextaticoControllers()
    .AddLextaticoSwagger()
    .AddEndpointsApiExplorer();

if (!builder.Environment.IsProduction())
    builder.Services.AddLextaticoMassTransitWithRabbitMq(builder.Configuration);
else
    builder.Services.AddLextaticoMassTransitWithServiceBus(builder.Configuration);

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("doc/swagger.json", "Account Lextatico Api v1"));

if (!app.Environment.IsProduction())
{
    await app.Services.MigrateContextDbAsync();

    app.UseDeveloperExceptionPage();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();

    app.UseHsts();
}

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseTransaction();

app.MapHealthChecks("/status",
              new HealthCheckOptions()
              {
                  ResponseWriter = async (context, report) =>
                  {
                      var result = JsonConvert.SerializeObject(
                          new
                          {
                              statusApplication = report.Status.ToString(),
                              healthChecks = report.Entries.Select(e => new
                              {
                                  check = e.Key,
                                  ErrorMessage = e.Value.Exception?.Message,
                                  status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                              })
                          });
                      context.Response.ContentType = MediaTypeNames.Application.Json;
                      await context.Response.WriteAsync(result);
                  }
              });

app.MapHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

await app.RunAsync();
