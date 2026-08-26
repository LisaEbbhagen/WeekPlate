using AiRecipe.LlmProxy.Api.DTOs;
using AiRecipe.LlmProxy.Api.Exceptions;
using OpenAI;
using OpenAI.Chat;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                string systemInstructions = """
                    You are a helpful senior chef assistant.
                    Generate a 5-day dinner meal plan in raw JSON format.

                    CRITICAL LANGUAGE RULES: 
                    1. All JSON keys (e.g., "theme", "days", "recipe", "ingredientName") MUST remain in English exactly as defined in the schema.
                    2. All text values MUST be written in Swedish (including recipe titles, instructions, ingredient names, category names, theme, and day names like 'Måndag', 'Tisdag').
                    3. Use standard Swedish cooking units (e.g., 'dl', 'msk', 'tsk', 'g', 'kg', 'st', 'klyftor').
                                        
                    The JSON must strictly follow this structure:

                    {
                      "theme": "Kort beskrivande tema på svenska (t.ex. Snabba vardagsrätter)",
                      "days": [
                        {
                          "dayName": "Måndag",
                          "recipe": {
                            "title": "Recepttitel på svenska",
                            "categoryName": "Pasta",
                            "totalTimeMinutes": 30,
                            "portions": 4,
                            "ingredients": [
                               { "ingredientName": "Kycklingfilé", "amount": "500", "unit": "g" }
                            ],
                            "instructions": "Steg för steg-instruktioner på svenska."
                          }
                        }
                      ]
                    }

                    ADDITIONAL RULES:
                    - The 'amount' field MUST be a string representation of a number (e.g., '1', '2.5') without text or fractions.
                    - Do NOT include markdown formatting (do NOT wrap in ```json ... ```) and do NOT include any conversational text. Output raw JSON only.
                    """; 
          
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
                        System.Net.HttpStatusCode.Forbidden => new LlmForbiddenException("Forbidden access to OpenAI API."),
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


