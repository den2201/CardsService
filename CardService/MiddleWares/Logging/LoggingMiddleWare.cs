using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CardService.Services.Logging
{
    public class RequestLogService
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLogService(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLogService>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation(
                    "Request {method} {url} {content} =>   Response {status}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Request?.ContentType,
                    context.Response?.StatusCode);
            }
        }
    }
}
