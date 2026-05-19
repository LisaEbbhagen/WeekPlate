using AiRecipe.Content.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AiRecipe.Content.Api.Filters
{
    public class ApiKeyFilter : IAsyncActionFilter  
    {
        private readonly ILogger<ApiKeyFilter> _logger;
        private readonly string _apiKey;
    

        public ApiKeyFilter(ILogger<ApiKeyFilter> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiKey = configuration["ServiceSettings:InternalApiKey"] 
                ?? throw new InvalidOperationException("Internal API Key is not configured in User Secrets");
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var expected = _apiKey;
            context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var extracted);

            // Detta kommer synas i din "Output"-ruta i Visual Studio när du kör anropet
            System.Diagnostics.Debug.WriteLine($"DEBUG: Expected: '{expected}', Received: '{extracted}'");

            if (string.IsNullOrEmpty(expected))
            {
                _logger.LogError("API key could not be found in the configuration!");
            }

            if (!context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var extractedApiKey))
            {
                throw new LlmClientUnauthorizedException("API key is missing.");
            }

            if (!_apiKey.Equals(extractedApiKey))
            {
                throw new LlmClientForbiddenException("Invalid API key.");
            }
            await next();
        }
    }
}