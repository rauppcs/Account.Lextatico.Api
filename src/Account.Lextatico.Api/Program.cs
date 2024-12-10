using System.Net.Mime;
using Account.Lextatico.Api.Configurations;
using Account.Lextatico.Infra.CrossCutting.Extensions;
using Account.Lextatico.Infra.CrossCutting.IoC;
using Account.Lextatico.Infra.Identity.User;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HostEnvironmentEnvExtensions = Account.Lextatico.Infra.CrossCutting.Extensions.HostEnvironmentEnvExtensions;
using Account.Lextatico.Domain.Events;
using Account.Lextatico.Application.EventHandlers;
using Account.Lextatico.Infra.CrossCutting.Extensions.MassTransitExtensions;
using Account.Lextatico.Infra.CrossCutting.Middlewares;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsLocalDevelopment())
    builder.Configuration.AddUserSecrets<Program>();

builder.Host.UseLextaticoSerilog(builder.Environment, builder.Configuration);

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
    .AddLextaticoHealthChecks(builder.Configuration, builder.Environment)
    .AddLextaticoContext(builder.Configuration, builder.Environment)
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

app.UseRequestSerilog();

app.UseLogging();

app.UseErrorHandling();

app.UseTransaction();

app.MapHealthChecks("/status",
              new HealthCheckOptions()
              {
                  ResponseWriter = async (context, report) =>
                  {
                      var result = JsonSerializer.Serialize(
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
