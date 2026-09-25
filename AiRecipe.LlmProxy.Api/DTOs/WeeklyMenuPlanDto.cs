using System.Collections.Specialized;

namespace AiRecipe.LlmProxy.Api.DTOs
{
    public record WeeklyMenuPlanDto
    (
        string? Theme,
        List<WeeklyMenyPlanDayDto> Days
    );

    public record WeeklyMenyPlanDayDto
    (
        string DayName,
        int RecipeId
    );

}
