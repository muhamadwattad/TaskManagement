namespace TaskManagement.Gateway.Middlewares
{
    public class LoggingMidleware
    {
        private readonly ILogger<LoggingMidleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingMidleware(ILogger<LoggingMidleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //var iuser = context.RequestServices.GetRequiredService<ICurrentUser>();

            //var userId = iuser.GetId().ToString() ?? string.Empty;
            //var userName = iuser.Name;

            //if (userName != null)
            //    _logger.LogInformation("Request: {@UserId} {@UserName}", userId, userName);

            // Log request details


            _logger.LogInformation($"Request Method: {context.Request.Method}");
            _logger.LogInformation($"Request Path: {context.Request.Path}");

            // Log request headers
            _logger.LogInformation("Request Headers:");
            foreach (var header in context.Request.Headers)
            {
                _logger.LogInformation($"\t{header.Key}: {header.Value}");
            }

            // Log request body (if present)
            if (context.Request.ContentLength.HasValue && context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering(); // Enable rewinding the request body stream
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                _logger.LogInformation($"Request Body: {requestBody}");

                // Rewind the request body stream so it can be read again by subsequent middleware/components
                context.Request.Body.Position = 0;
            }


            await _next(context);
        }
    }
}
