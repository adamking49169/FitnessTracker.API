using System.Net;
using System.Text.Json;

namespace FitnessTracker.API.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
    {
        try
        {
            await next(ctx);
        }
        catch (Exception ex)
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ctx.Response.ContentType = "application/problem+json";

            var problem = new
            {
                title = "An unexpected error occurred.",
                detail = ex.Message
            };
            var json = JsonSerializer.Serialize(problem);
            await ctx.Response.WriteAsync(json);
        }
    }
}
