using AiRecipe.Content.Api.Clients;
using AiRecipe.Content.Api.DTOs;
using AiRecipe.Content.Api.Exceptions;
using AiRecipe.Content.Api.Filters;
using AiRecipe.Content.Api.Services;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.RateLimiting;
using System.ComponentModel.DataAnnotations;
using static AiRecipe.Content.Api.DTOs.PagingDto;

namespace AiRecipe.Content.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyFilter))]
    [EnableRateLimiting("sliding")] //activates rate limiting
    public class LlmClientController : ControllerBase
    {
        private readonly ILlmClient _llmClient;
        private readonly IRecipeService _recipeService;

        public LlmClientController(ILlmClient llmClient, IRecipeService recipeService)
        {
            _llmClient = llmClient;
            _recipeService = recipeService;
        }

        /// <summary>
        /// Generate and store a weekly meal plan using the internal LLM client.
        /// </summary>
        /// <param name="prompt">User instructions for the meal plan (e.g. "5 days, vegetarian, max 30 min").</param>
        /// <example> "Budget-friendly family meals for 5 days, max 30 min prep".</example>
        /// <returns>Confirmation that the weekly menu was generated and saved.</returns>
        /// <response code="200">Weekly menu generated and saved.</response>
        /// <response code="400">Prompt is empty or invalid.</response>
        /// <response code="401">Missing or invalid API key for this endpoint.</response>
        /// <response code="429">Too many requests - rate limit exceeded.</response>
        /// <response code ="500">Unexpected error while processing the request.</response>
        /// <response code="504">Timeout while waiting for the LLM response.</response>
        [HttpPost("generate-weeklyMealPlan")]
        [ProducesResponseType(typeof(MealPlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> GenerateWeeklyMenu([FromQuery, Required] string prompt)
        {
            // Ask the service to import the menu returned by the LLM and persist it.
            // The service handles deduplication and saving related entities.
            var response = await _recipeService.ImportWeeklyMenuAsync(prompt);

            // Return a small confirmation payload and the generated recipes. 
            return Ok(new
            {
                Message = $"Weekly menu generated and saved successfully from the prompt '{prompt}'.",
                Data = response
            });
        }
    }
}
