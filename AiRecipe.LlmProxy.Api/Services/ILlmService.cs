using AiRecipe.LlmProxy.Api.DTOs;

namespace AiRecipe.LlmProxy.Api.Services
{
    public interface ILlmService
    {
        Task<WeeklyMenuPlanDto> GenerateWeeklyMenuFromDbAsync(WeeklyMenuRequestDto requestDto);
    }
}
