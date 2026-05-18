using System.ComponentModel.DataAnnotations;

namespace AiRecipe.LlmProxy.Api.DTOs
{
    public record MealPlanDto
    ( 
      string? Theme,
      List<MealPlanDayDto> Days
    );

    public record MealPlanDayDto
    (
        [Required(ErrorMessage = "The recipe must belong to a day.")]
        string DayName,
        [Required(ErrorMessage = "The recipe is required.")]
        RecipeCreateDto Recipe
    );

}
