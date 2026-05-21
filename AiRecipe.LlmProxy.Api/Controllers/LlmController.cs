using AiRecipe.LlmProxy.Api.DTOs;
using AiRecipe.LlmProxy.Api.Filters;
using AiRecipe.LlmProxy.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json;

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
        /// Generate a weekly meal plan from the upstream LLM service.
        /// </summary>
        /// <param name="prompt">Instructions for the meal plan returned as JSON.</param>
        /// <returns>MealPlanDto parsed from the LLM response.</returns>
        /// <response code="200">Meal plan generated successfully.</response>
        /// <response code="400">Invalid prompt provided.</response>
        /// <response code="401">Unauthorized - API key is missing or invalid.</response>
        /// <response code="403">Forbidden - API key does not have access to this resource.</response>
        /// <response code="429">Too many requests - rate limit exceeded.</response>
        /// <response code="500">Failed to call or parse response from the LLM service.</response>
        /// <response code="504">Gateway Timeout - LLM service did not respond in time.</response>
        [HttpGet("generate")]
        public async Task<ActionResult<MealPlanDto>> Generate([FromQuery] string prompt)
        {
            // Service wraps the OpenAI client and returns a typed DTO or throws on failure.
            var response = await _llmService.GenerateWeeklyMenuAsync(prompt);
            return Ok(response);
        }
    }
}
