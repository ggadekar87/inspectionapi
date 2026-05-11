using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace api.filter
{
    public class CustomAuthenticationFilter : IAsyncActionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomAuthenticationFilter> _logger;

        public CustomAuthenticationFilter(IConfiguration configuration, ILogger<CustomAuthenticationFilter> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Respect [AllowAnonymous]
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await next();
                return;
            }

            // Retrieve expected key from configuration (e.g., appsettings or env var "ApiKey")
            var expectedKey = _configuration["ApiKey"] ?? string.Empty;

            // Try X-Api-Key header first, then Authorization: Bearer <key>
            var headers = context.HttpContext.Request.Headers;
            string? providedKey = null;

            if (headers.TryGetValue("X-Api-Key", out var apiKeyHeader) && !string.IsNullOrWhiteSpace(apiKeyHeader))
            {
                providedKey = apiKeyHeader.ToString();
            }
            else if (headers.TryGetValue("Authorization", out var authHeader) && authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                providedKey = authHeader.ToString().Substring("Bearer ".Length).Trim();
            }

            if (string.IsNullOrWhiteSpace(providedKey) || !string.Equals(providedKey, expectedKey, StringComparison.Ordinal))
            {
                _logger.LogWarning("Unauthorized request to {Path}", context.HttpContext.Request.Path);
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
