using System.Net;
using Account.Lextatico.Infra.CrossCutting.Extensions;
using Account.Lextatico.Infra.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Account.Lextatico.Infra.Data.Extensions;
using Microsoft.Extensions.Logging;

namespace Account.Lextatico.Infra.CrossCutting.Middlewares
{
    public static class TransactionUnitExtensions
    {
        public static IApplicationBuilder UseTransaction(this IApplicationBuilder app)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<TransactionUnitMiddleware>();

            return app;
        }
    }

    public class TransactionUnitMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionUnitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, LextaticoContext lextaticoContext, IMediator mediator, ILogger<TransactionUnitMiddleware> logger)
        {
            try
            {
                string httpVerb = httpContext.Request.Method.ToUpper();

                if (httpVerb == "POST" || httpVerb == "PUT" || httpVerb == "DELETE")
                {
                    var strategy = lextaticoContext.CreateExecutionStrategy();

                    await strategy.ExecuteAsync(async () =>
                    {
                        await using var transaction = await lextaticoContext.StartTransactionAsync();

                        await _next(httpContext);

                        var httpStatusCode = Enum.Parse<HttpStatusCode>(httpContext.Response.StatusCode.ToString());

                        var pathSplit = httpContext.Request.Path.Value.Split("/");

                        if (httpStatusCode.IsSuccess() || pathSplit.Contains("login"))
                        {
                            await lextaticoContext.SubmitTransactionAsync(transaction);
                            await mediator.DispatchDomainEventsAsync(lextaticoContext);
                        }
                            
                        else
                        {
                            await lextaticoContext.UndoTransaction(transaction);
                            await lextaticoContext.DiscardCurrentTransactionAsync();
                        }
                    });
                }
                else
                {
                    await _next(httpContext);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro durante a execução");
                
                await lextaticoContext.UndoTransaction();
                await lextaticoContext.DiscardCurrentTransactionAsync();

                throw ex;
            }
        }
    }
}
