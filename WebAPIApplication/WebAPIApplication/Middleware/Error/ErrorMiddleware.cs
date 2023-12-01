using System.Net;
using Newtonsoft.Json;

namespace WebAPIApplication.Middleware.Error;

public class ErrorMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var messageArr = new[] { "An error occurred while processing your request." };
        var response = new
        {
            error = new
            {
                message = messageArr
            }
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}