using AiRecipe.Content.Api.DTOs;
using static AiRecipe.Content.Api.DTOs.PagingDto;

namespace AiRecipe.Content.Api.Services
{
    public interface IRecipeService
    {
        //Task: "I promise to fix this later.."
        Task <MealPlanDto> ImportWeeklyMenuAsync(string prompt);
        Task<PagedResponse<RecipeResponse>> GetRecipesAsync(int page, int pageSize, string? category, int? totalTimeMinutes, string? titleContains);
        Task<RecipeResponse> CreateRecipeAsync(RecipeCreateDto request);
        Task<RecipeResponse?> UpdateRecipeAsync(int RecipeId, RecipeUpdateDto request);
        Task<RecipeResponse?> GetRecipeByIdAsync(int RecipeId);
        Task DeleteRecipeAsync(int RecipeId);
    }
}
