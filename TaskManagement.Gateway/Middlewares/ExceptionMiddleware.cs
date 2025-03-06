using TaskManagement.Framework.Exceptions;

namespace TaskManagement.Gateway.Middlewares
{
    public class ExceptionMiddleware
    {
        private const string JsonContentType = "application/json";
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }



        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context) => InvokeAsync(context);

        async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e.Message);
                await Response(context, StatusCodes.Status404NotFound, e.Message);
            }
            catch (ValidationException e)
            {
                _logger.LogError(e.Message);
                await Response(context, StatusCodes.Status400BadRequest, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await Response(context, StatusCodes.Status400BadRequest, "Request Failed");
            }
        }
        async Task Response(HttpContext context, int code, string message)
        {
            // set http status code and content type
            context.Response.StatusCode = code;
            context.Response.ContentType = JsonContentType;

            // writes / returns error model to the response
            await context.Response.WriteAsJsonAsync(message);
        }

    }
}
