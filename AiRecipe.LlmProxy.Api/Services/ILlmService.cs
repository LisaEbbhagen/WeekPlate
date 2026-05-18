using AiRecipe.LlmProxy.Api.DTOs;

namespace AiRecipe.LlmProxy.Api.Services
{
    public interface ILlmService
    {
        Task<MealPlanDto> GenerateWeeklyMenuAsync(string prompt);
    }
}
