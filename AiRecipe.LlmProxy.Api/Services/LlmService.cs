using AiRecipe.LlmProxy.Api.DTOs;
using AiRecipe.LlmProxy.Api.Exceptions;
using OpenAI;
using OpenAI.Chat;
using System.Net.Http;
using System.Text.Json;

namespace AiRecipe.LlmProxy.Api.Services
{
    public class LlmService : ILlmService
    {
        private readonly ILogger<LlmService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;


        public LlmService(ILogger<LlmService> logger, HttpClient httpClient, IConfiguration config)
        {
            _logger = logger;
            _httpClient = httpClient;
            _apiKey = config["OpenAI:ApiKey"] ?? throw new ArgumentNullException("OpenAI API Key is missing.");
        }

        public async Task<MealPlanDto> GenerateWeeklyMenuAsync(string prompt)
        {
            // Create System Message (instructions)
            try
            {
                string systemInstructions = "You are a helpful senior chef assistant." +
                    "Generate a 5-day dinner meal plan in raw JSON format. " +
                    "The JSON must strictly follow this structure: " +
                    "{ " +
                    "  \"theme\": \"Optional short theme description\", " +
                    "  \"days\": [ " +
                    "    { " +
                    "      \"dayName\": \"Monday\", " +
                    "      \"recipe\": { " +
                    "        \"title\": \"Recipe Title\", " +
                    "        \"categoryName\": \"Pasta\", " +
                    "        \"totalTimeMinutes\": 30, " +
                    "        \"portions\": 4, " +
                    "        \"ingredients\": [ " +
                    "           { \"ingredientName\": \"Ingredient name\", \"amount\": \"1.0\", \"unit\": \"dl\" } " +
                    "        ], " +
                    "        \"instructions\": \"Step by step guide.\" " +
                    "      } " +
                    "    } " +
                    "  ] " +
                    "}. " +
                    "Important: The 'amount' field MUST be a number string (e.g., '1.0', '5') without any text or fractions." +
                    "Do not include markdown formatting or conversational text.";

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = systemInstructions },
                        new { role = "user", content = prompt }
                    },
                    response_format = new { type = "json_object" }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
                request.Headers.Add("Authorization", $"Bearer {_apiKey}");
                request.Content = JsonContent.Create(requestBody);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("OpenAI API returned: {StatusCode}", response.StatusCode);
                    throw response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.Unauthorized => new LlmUnauthorizedException("Unauthorized access to OpenAI API."),
                        System.Net.HttpStatusCode.TooManyRequests => new LlmRateLimitException("Rate limit exceeded for OpenAI API."),
                        _ => new LlmProxyException($"An unexpected error occurred while accessing OpenAI API: {response.StatusCode}")
                    };
                }

                // 3. Deserialize JSON-answer to MealPlanDto
                var responseJson = await response.Content.ReadFromJsonAsync<JsonElement>();

                var aiTextAnswer = responseJson.GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "";

                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                _logger.LogInformation(aiTextAnswer);
                var result = JsonSerializer.Deserialize<MealPlanDto>(aiTextAnswer, options);

                return result ?? throw new LlmProxyException("Failed to deserialize the meal plan.");
            }

            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout occurred while accessing OpenAI API for prompt: {Prompt}", prompt);
                throw new LlmTimeOutException("Timeout occurred while accessing OpenAI API.", ex);
            }
            catch (Exception ex) when (ex is not LlmUnauthorizedException && ex is not LlmTimeOutException && ex is not LlmRateLimitException)
            {
                _logger.LogError(ex, "Failed to generate mealplan from: {Prompt}", prompt);
                throw new LlmProxyException("Failed to generate mealplan from prompt.", ex);
            }
        }
    }
}


