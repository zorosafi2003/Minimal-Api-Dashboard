using FluentValidation;
using System.Text.Json;

namespace MiniApp.Api.Middlewares
{
    internal sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var response = new
            {
                title = "Server Error",
                status = (int) StatusCodes.Status400BadRequest,
                detail = exception.Message,
                errors = GetErrors(exception)
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest ;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
        {
            IReadOnlyDictionary<string, string[]> errors = null;
            if (exception is ValidationException validationException)
            {
                errors = validationException.Errors.GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);
            }
            return errors;
        }
    }
}
