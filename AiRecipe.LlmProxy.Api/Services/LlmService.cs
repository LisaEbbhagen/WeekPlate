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

        public async Task<WeeklyMenuPlanDto> GenerateWeeklyMenuFromDbAsync(WeeklyMenuRequestDto requestDto)
        {
            if (requestDto.AvailableRecipes == null || !requestDto.AvailableRecipes.Any())
            {
                throw new LlmProxyException("AvailableRecipes cannot be null or empty.");
            }
            // Create System Message (instructions)
            try
            {
                string systemInstructions = """
                    You are a helpful senior chef and meal planning assistant.
                    Your task is to create a 5-day weekly menu plan by selecting recipes from a provided list of candidate recipes based on the user's preferences.

                    CRITICAL SELECTION RULES: 
                    1. The weekly menu must consist of exactly 5 days (Måndag to Fredag). If the user dont specifically request a number of days, default to 5 days.
                    2. NEVER invent new recipes, fake IDs, or modify existing recipe details.
                    3. Never repeat the same recipe in the weekly menu. Each day must have a unique recipe.
                    4. Match the user's preferences, dietary restrictions, and allergies against the titles, categories, and ingredient names in the candidate list.

                    CRITICAL LANGUAGE RULES: 
                    1. All JSON keys (e.g., "theme", "days", "dayName", "recipeId", "ingredientName") MUST remain in English exactly as defined in the schema.
                    2. All text values MUST be written in Swedish (e.g., 'theme', and day names like 'Måndag', 'Tisdag').                    
                                        
                    The JSON must strictly follow this structure:

                    {
                      "theme": "Kort beskrivande tema på svenska (t.ex. Snabba vardagsrätter)",
                      "days": [
                        {
                          "dayName": "Måndag",
                          "recipeId": 12 
                        }
                      ]
                    }

                    ADDITIONAL RULES:
                    - The 'recipeId' field MUST be an integer matching an 'id' from the provided candidate list.    
                    - The number of objects in 'days' MUST match the 'numberOfDays' requested by the user or 5 if not specified.                    
                    - Do NOT include markdown formatting (do NOT wrap in ```json ... ```) and do NOT include any conversational text. Output raw JSON only.
                    """; 
          
                string recipesJson = JsonSerializer.Serialize(requestDto.AvailableRecipes);
                string userPrompt = $"""
                    Create a weekly menu for { requestDto.NumberOfDays} days.

                    User Preferences:                    
                    { (string.IsNullOrWhiteSpace(requestDto.UserPreferences) ? "Variated and balanced weekly menu" : requestDto.UserPreferences)}

                    Available recipes in the database:
                    { recipesJson}
                """;

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = systemInstructions },
                        new { role = "user", content = userPrompt }
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

                _logger.LogInformation(aiTextAnswer);
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                var result = JsonSerializer.Deserialize<WeeklyMenuPlanDto>(aiTextAnswer, options);

                return result ?? throw new LlmProxyException("Failed to deserialize the meal plan.");
            }

            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout occurred while accessing OpenAI API.");
                throw new LlmTimeOutException("Timeout occurred while accessing OpenAI API.", ex);
            }
            catch (Exception ex) when (ex is not LlmUnauthorizedException && ex is not LlmTimeOutException && ex is not LlmRateLimitException)
            {
                _logger.LogError(ex, "Failed to generate mealplan.");
                throw new LlmProxyException("Failed to generate mealplan from prompt.", ex);
            }
        }
    }
}


