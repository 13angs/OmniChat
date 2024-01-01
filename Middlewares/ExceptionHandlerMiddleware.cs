using Newtonsoft.Json;
using OmniChat.Exceptions;
using OmniChat.Models;
using Serilog;

namespace OmniChat.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ErrorResponse response = new()
            {
                Message = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            };

            if (ex is BadHttpRequestException)
            {
                response.Status = StatusCodes.Status400BadRequest;
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                Log.Warning(ex.Message);
            }
            else if (ex is UnauthorizedAccessException)
            {
                response.Status = StatusCodes.Status401Unauthorized;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                Log.Warning(ex.Message);
            }
            else if (ex is NotFoundException)
            {
                response.Status = StatusCodes.Status404NotFound;
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                Log.Warning(ex.Message);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                Log.Error(ex, ex.Message);
            }

            string strRes = JsonConvert.SerializeObject(response);
            context.Response.Headers.Add("content-type", "application/json");
            await context.Response.WriteAsync(strRes);
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware
        (this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}