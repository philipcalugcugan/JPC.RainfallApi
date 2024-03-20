using JPC.Application.Shared.Rainfall.Dto;
using System.Net;
using System.Text.Json;

namespace JPC.RainfallApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponse(exception);

            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case KeyNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = ex.Message;
                    break;
                case BadHttpRequestException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                case RainfallApiException ex:
                    response.StatusCode = (int)ex.StatusCode;
                    errorResponse.Message = ex.Message;
                    errorResponse.Type = ex.StatusCode.ToString();
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal Server errors: " + exception.Message;
                    break;
            }

            _logger.LogError(exception, exception.Source);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }

    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string ExceptionType { get; set; }

        public ErrorResponse(Exception exception)
        {
            Message = exception.Message;
            ExceptionType = exception.GetType().FullName;
        }
    }
}
