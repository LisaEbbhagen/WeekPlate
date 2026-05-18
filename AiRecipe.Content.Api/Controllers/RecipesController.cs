using AiRecipe.Content.Api.DTOs;
using AiRecipe.Content.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using static AiRecipe.Content.Api.DTOs.PagingDto;

namespace AiRecipe.Content.Api.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    [EnableRateLimiting("sliding")] //activates rate limiting

    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        // Constructor injection of the recipe service
        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }
  
        /// <summary>
        /// Returns a paged list of recipes with optional filters.
        /// </summary>
        /// <param name="page">Page number (1-based).</param>
        /// <param name="pageSize">Number of items per page (clamped to a reasonable range).</param>
        /// <param name="category">Optional category name to filter by (case-insensitive).</param>
        /// <param name="totalTimeMinutes">Optional maximum total time (in minutes) to filter by.</param>
        /// <param name="titleContains">Optional substring to filter recipe titles (case-insensitive).</param>
        /// <returns>PagedResponse of recipes matching the filters.</returns>
        /// <response code="200">Paged list of recipes returned successfully.</response>
        /// <response code="400">Invalid query parameters (e.g. page &lt; 1) provided by the client.</response>
        /// <response code="401">Missing or invalid API key.</response>
        /// <response code="500">Server error while retrieving recipes.</response>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<RecipeResponse>>> GetRecipes(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? category = null,
            [FromQuery] int? totalTimeMinutes = null,
            [FromQuery] string? titleContains = null
            )
        {
            pageSize = Math.Clamp(pageSize, 1, 100);
            // Prevent very large page sizes from overwhelming the server or client.
            // The service will handle filtering, pagination and caching.
            var response = await _recipeService.GetRecipesAsync(page, pageSize, category, totalTimeMinutes, titleContains);
            return Ok(response);
        }

        /// <summary>
        /// Gets a single recipe by its id.
        /// </summary>
        /// <param name="id">Recipe identifier.</param>
        /// <returns>The recipe matching the id.</returns>
        /// <response code="200">Recipe found and returned.</response>
        /// <response code="404">Recipe with the given id was not found.</response>
        /// <response code="401">Missing or invalid API key.</response>
        /// <response code="500">Server error while retrieving the recipe.</response>
        [HttpGet("{id}", Name = "GetRecipeById")]
        public async Task<ActionResult<RecipeResponse>> GetRecipeById(int id)
        {
            // The service will throw NotFoundException if the recipe is missing; middleware translates it to 404.
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            return Ok(recipe);
        }

        /// <summary>
        /// Creates a new recipe.
        /// </summary>
        /// <param name="request">Recipe payload to create.</param>
        /// <returns>The created recipe with its id.</returns>
        /// <response code="201">Recipe created. Location header set to the new resource.</response>
        /// <response code="400">Invalid request payload.</response>
        /// <response code="401">Missing or invalid API key.</response>
        /// <response code="500">Server error while creating the recipe.</response>
        [HttpPost]
        public async Task<ActionResult<PagedResponse<RecipeResponse>>> CreateRecipe(RecipeCreateDto request)
        {
            // CreatedAtAction builds a Location header pointing to the newly created resource.
            var response = await _recipeService.CreateRecipeAsync(request);
            return CreatedAtAction(nameof(GetRecipeById), new { id = response.RecipeId }, response);
        }

        /// <summary>
        /// Partially update a recipe. Only fields present in the request are changed.
        /// </summary>
        /// <param name="id">Identifier of the recipe to update.</param>
        /// <param name="request">DTO containing fields to update (null means leave unchanged).</param>
        /// <returns>The updated recipe.</returns>
        /// <response code="200">Recipe updated successfully.</response>
        /// <response code="400">Invalid update payload.</response>
        /// <response code="404">Recipe with the given id was not found.</response>
        /// <response code="401">Missing or invalid API key.</response>
        /// <response code="500">Server error while updating the recipe.</response>
        [HttpPatch("{id}", Name = "UpdateRecipe")]
        // PATCH is used for partial updates only.
        public async Task<ActionResult<RecipeResponse>> UpdateRecipe(int id, RecipeUpdateDto request)
        {
            // Service applies only non-null fields from the DTO; it will throw NotFoundException if the recipe is missing.
            var recipe = await _recipeService.UpdateRecipeAsync(id, request);
            return Ok(recipe);
        }

        /// <summary>
        /// Delete a recipe by id.
        /// </summary>
        /// <param name="id">Identifier of the recipe to delete.</param>
        /// <response code="204">Recipe deleted successfully (no content).</response>
        /// <response code="404">Recipe with the given id was not found.</response>
        /// <response code="401">Missing or invalid API key.</response>
        /// <response code="500">Server error while deleting the recipe.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            // Service throws NotFoundException if the recipe doesn't exist; middleware translates that to 404.
            await _recipeService.DeleteRecipeAsync(id);
            // Return 204 No Content on successful deletion.
            return NoContent();
        }
    }
}
