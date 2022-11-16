using Microsoft.Extensions.Logging;
using NLog;

namespace PublishSubscribe.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occured on path: {context.Request.Path.Value}  QueryString on path: {context.Request.QueryString} ex{ex.ToString()}");
                context.Response.StatusCode = 500;
            }
        }
    }
}
