using System.Net;

namespace CarRentalSystem.Api.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger <ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, error, message) = exception switch
            {
                ArgumentOutOfRangeException tle => (
                    HttpStatusCode.UnprocessableEntity,
                    "worng_meter_reading",
                    tle.InnerException?.Message),
                InvalidOperationException tae => (
                    HttpStatusCode.BadRequest,
                    "something_went_wrong",
                    tae.InnerException?.Message),
                _ => (
                    HttpStatusCode.InternalServerError,
                    "internal_error",
                    "An unexpected error occurred.")
            };

            _logger.LogError(exception, "Unhandled exception: {Error}", error);

            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var payload = new
            {
                error,
                message,
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsJsonAsync(payload);
        }
    }
}
