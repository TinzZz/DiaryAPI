using Diary.Domain.Enum;
using Diary.Domain.Result;
using ILogger = Serilog.ILogger;
namespace Diary.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
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

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            _logger.Error(exception, exception.Message);

            var errorMessage = exception.Message;
            var response = exception switch
            {
                UnauthorizedAccessException _ => new BaseResult() { ErrorMessage = errorMessage, ErrorCode = (int)ErrorCodes.InternalServerError },   // сюда прописываем свой код ошибки
                _ => new BaseResult() { ErrorMessage = "Internal server error", ErrorCode = (int)ErrorCodes.InternalServerError }
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)response.ErrorCode;
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
