using AiRecipe.Content.Api.Clients;
using AiRecipe.Content.Api.DTOs;
using AiRecipe.Content.Api.Exceptions;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AiRecipe.Content.Api.Clients
{
    public class LlmClient : ILlmClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LlmClient> _logger;

        public LlmClient(HttpClient httpClient, IConfiguration config, ILogger<LlmClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            var apiKey = config["ServiceSettings:InternalApiKey"] ?? throw new ArgumentNullException("Internal Api Key is missing.");
            if (!_httpClient.DefaultRequestHeaders.Contains("x-api-key"))
            {
                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            }
        }

        public async Task<MealPlanDto> GetWeeklyMenuAsync(string prompt)
        {
            try
            {
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                var response = await _httpClient.GetAsync(
                    $"api/ai/generate?prompt={Uri.EscapeDataString(prompt)}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new LlmClientBadGatewayException($"LlmProxy returned an error: {response.StatusCode}");
                }

                var result = await response.Content.ReadFromJsonAsync<MealPlanDto>(options);
                return result ?? throw new LlmClientBadGatewayException("No answer from AI.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while calling LlmProxy.");
                throw new LlmClientBadGatewayException("An error occurred while calling LlmProxy.", ex);
            }
        }
    }
}
