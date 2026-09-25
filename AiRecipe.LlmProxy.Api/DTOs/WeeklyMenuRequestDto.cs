
namespace AiRecipe.LlmProxy.Api.DTOs
{
    public record WeeklyMenuRequestDto
    (
        string? UserPreferences,
        int NumberOfDays,
        List<RecipeCandidateDto> AvailableRecipes
        );

    public record RecipeCandidateDto
    (
        int Id,
        string Title,
        string CategoryName,
        int TotalTimeMinutes,
        List<string> IngredientNames
    );
    
}
