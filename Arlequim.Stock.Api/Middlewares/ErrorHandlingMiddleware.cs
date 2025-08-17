using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Arlequim.Stock.Api.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
        {
            try
            {
                await next(ctx);
            }
            catch (Exception ex)
            {
                await WriteProblemAsync(ctx, ex);
            }
        }

        private static async Task WriteProblemAsync(HttpContext ctx, Exception ex)
        {
            var (status, title) = MapException(ex);

            var problem = new
            {
                type = "about:blank",
                title,
                status,
                traceId = ctx.TraceIdentifier,
                // detalhe opcional (não exponha stack em prod)
                detail = ex is ArgumentException aex ? aex.Message : ex.Message
            };

            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = status;
            await ctx.Response.WriteAsJsonAsync(problem);
        }

        private static (int status, string title) MapException(Exception ex) =>
            ex switch
            {
                ArgumentException => ((int)HttpStatusCode.BadRequest, "Invalid request"),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Resource not found"),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized"),
                InvalidOperationException => ((int)HttpStatusCode.Conflict, "Operation not allowed"),
                DbUpdateConcurrencyException => ((int)HttpStatusCode.Conflict, "Concurrency conflict"),
                _ => ((int)HttpStatusCode.InternalServerError, "Unexpected error")
            };
    }
}
