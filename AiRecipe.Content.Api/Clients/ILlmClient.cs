using AiRecipe.Content.Api.DTOs;

namespace AiRecipe.Content.Api.Clients
{
    public interface ILlmClient
    {
        Task<MealPlanDto> GetWeeklyMenuAsync(string prompt);
    }
}
