using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InventoryHubAPI.Service.Interfaces;

namespace InventoryHubAPI.Middleware
{
    public class LoggingMiddleware 
    {
        private readonly RequestDelegate _next;
        //private readonly ILoggerService _logger;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Resolve the scoped ILoggerService here
            var logger = context.RequestServices.GetRequiredService<ILoggerService>();

            // Log Request
            context.Request.EnableBuffering();

            string requestBody = string.Empty;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            logger.LogRequest(context.Request.Method, context.Request.Path, requestBody);

            // Capture and Log Response
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context); // Proceed to the next middleware

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                logger.LogResponse(context.Request.Method, context.Request.Path, context.Response.StatusCode, responseText);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}