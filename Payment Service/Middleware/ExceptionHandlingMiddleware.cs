using System.ComponentModel.DataAnnotations;
using MassTransit;
using PaymentService.Infrastructure.Persistence.Configurations;
using SHARE.Model;

namespace PaymentService.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate next, IPublishEndpoint publisher)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, publisher);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, IPublishEndpoint publisher)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            DatabaseException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        await publisher.Publish(new ExceptionMessage(exception.Message, exception.StackTrace));

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message = exception.Message
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}