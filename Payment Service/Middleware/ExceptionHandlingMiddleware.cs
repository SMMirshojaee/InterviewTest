using System.ComponentModel.DataAnnotations;
using PaymentService.Infrastructure.Persistence.Configurations;

namespace PaymentService.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next )
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized, 
                DatabaseException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = exception.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }

}
