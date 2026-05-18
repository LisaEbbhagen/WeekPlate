using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AiRecipe.LlmProxy.Api.Filters
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
                _logger.LogError("API-nyckeln kunde inte hittas i konfigurationen!");
            }

            if (!context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "API Key is missing"
                };
                return;
            }
            if (!_apiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "Unauthorized: Wrong API Key"
                };
                return;
            }
            await next();
        }
    }
}