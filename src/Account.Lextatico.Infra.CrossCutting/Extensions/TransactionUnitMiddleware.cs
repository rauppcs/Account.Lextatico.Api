using System.Net;
using Account.Lextatico.Infra.Data.Context;
using Account.Lextatico.Infra.Data.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Account.Lextatico.Infra.CrossCutting.Extensions
{
    public static class TransactionUnitExtension
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

        public async Task Invoke(HttpContext httpContext, LextaticoContext lextaticoContext, IMediator mediator)
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
            catch (Exception)
            {
                await lextaticoContext.UndoTransaction();
                await lextaticoContext.DiscardCurrentTransactionAsync();
            }
        }
    }
}
