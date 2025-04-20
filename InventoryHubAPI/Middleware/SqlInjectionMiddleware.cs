using System.Text;

namespace InventoryHubAPI.Middleware
{
    internal class SqlInjectionMiddleware 
    {
        private readonly RequestDelegate _next;

        public SqlInjectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request method is POST or PUT
            if(context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                // Set SQL key words
                var sqlKeywords = new[] { "select", "insert", "update", "delete", "drop", "union", "or", "--", ";" };
                
                // Enable rewinding so we can read the body without consuming it permanently
                context.Request.EnableBuffering();

                // Read the body as text
                using var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);

                string bodyText = await reader.ReadToEndAsync();

                // Reset stream position for the next middleware to read again
                context.Request.Body.Position = 0;

                // Check for SQL keywords
                foreach (var keyword in sqlKeywords)
                {
                    if (bodyText.ToLower().Contains(keyword))
                    {
                        // Log suspicious request (optional)
                        Console.WriteLine($"SQL Injection Attempt Detected: {keyword}");

                        // Return 400 Bad Request
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Bad request. Suspicious input detected.");
                        return; // Stop pipeline here
                    }
                }
            }
            await _next(context);
        }
    }
}