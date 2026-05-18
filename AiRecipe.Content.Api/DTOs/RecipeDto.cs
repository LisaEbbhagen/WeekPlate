using AiRecipe.Content.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AiRecipe.Content.Api.DTOs
{
   public record RecipeResponse(
        int RecipeId,
        string Title,
        string Category,
        int TotalTimeMinutes,
        int Portions,
        List<RecipeIngredientDto> Ingredients,
        string Instructions,
        DateTime CreatedAt
    );
}
