namespace api.middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
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
                // Handle exception
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An error occurred.");
            }
        }
    }
}
