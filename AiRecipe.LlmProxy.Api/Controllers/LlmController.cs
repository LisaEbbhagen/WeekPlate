using AiRecipe.LlmProxy.Api.DTOs;
using AiRecipe.LlmProxy.Api.Filters;
using AiRecipe.LlmProxy.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AiRecipe.LlmProxy.Api.Controllers
{
   
    [Route("api/ai")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyFilter))]
    [EnableRateLimiting("sliding")] //activates rate limiting
    public class LlmController : ControllerBase
    {
        private readonly ILlmService _llmService;
        public LlmController(ILlmService llmService)
        {
            _llmService = llmService;
        }

        /// <summary>
        /// Generate a weekly meal plan from recipies in database and user preferences.
        /// </summary>
        /// <param name="request">Instructions for the meal plan returned as JSON.</param>
        /// <returns>WeeklyMenyPlanDto parsed from the LLM response.</returns>
        /// <response code="200">Meal plan generated successfully.</response>
        /// <response code="400">Invalid user preferences provided.</response>
        /// <response code="401">Unauthorized - API key is missing or invalid.</response>
        /// <response code="403">Forbidden - API key does not have access to this resource.</response>
        /// <response code="429">Too many requests - rate limit exceeded.</response>
        /// <response code="500">Failed to call or parse response from the LLM service.</response>
        /// <response code="504">Gateway Timeout - LLM service did not respond in time.</response>
        [HttpPost("generate")]
        public async Task<ActionResult<WeeklyMenuPlanDto>> GenerateFromDb([FromBody] WeeklyMenuRequestDto request)
        {
            // Service wraps the OpenAI client and returns a typed DTO or throws on failure.
            var response = await _llmService.GenerateWeeklyMenuFromDbAsync(request);
            return Ok(response);
        }
    }
}
