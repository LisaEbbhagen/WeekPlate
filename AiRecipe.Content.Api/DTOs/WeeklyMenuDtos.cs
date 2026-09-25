namespace AiRecipe.Content.Api.DTOs
{
    //DTOs for the request to the LlmProxy.Api
    public record RecipeCandidateDto(
        int Id,
        string Title,
        string CategoryName,
        int TotalTimeMinutes,
        List<string> IngredientNames
        );
    
    public class WeeklyMenuRequestDto
    {
        public string? UserPreferences { get; set; }
        public int NumberOfDays { get; set; } = 5;
        public List<RecipeCandidateDto> AvailableRecipes { get; set; } = new();
    }

    public record WeeklyMenuDayPlanDto(
        string DayName,
        int RecipeId
    );

    //DTOs for the response from the API to React frontend
    public record WeeklyMenuDayResponseDto(
        string DayName,
        RecipeResponse? Recipe
    );

    public record WeeklyMenuResponseDto(
        string? Theme,
        List<WeeklyMenuDayResponseDto> Days
    );
}
