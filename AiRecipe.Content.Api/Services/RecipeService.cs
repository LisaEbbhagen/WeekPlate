using AiRecipe.Content.Api.Clients;
using AiRecipe.Content.Api.Data;
using AiRecipe.Content.Api.DTOs;
using AiRecipe.Content.Api.Exceptions;
using AiRecipe.Content.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using System.Linq;
using static AiRecipe.Content.Api.DTOs.PagingDto;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;

namespace AiRecipe.Content.Api.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDbContext _context;
        private readonly HybridCache _cache;
        private readonly ILlmClient _llmClient;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(RecipeDbContext context, HybridCache cache, ILlmClient llmClient, ILogger<RecipeService> logger)
        {
            _context = context;
            _cache = cache;
            _llmClient = llmClient;
            _logger = logger;
        }

        public async Task<MealPlanDto> ImportWeeklyMenuAsync(string prompt)
        {
            try
            {
                var mealPlan = await _llmClient.GetWeeklyMenuAsync(prompt);

                if (mealPlan?.Days == null || !mealPlan.Days.Any())
                {
                    // If the AI response is empty we can't proceed — surface as 404 so callers know nothing was created.
                    _logger.LogWarning("AI returned an empty meal plan for prompt: {Prompt}", prompt);
                    throw new NotFoundException($"AI returned an empty meal plan for prompt: `{prompt}`");
                }

                // Load current DB state once to avoid repeated queries inside the loop.
                // We keep local lists to detect duplicates and reuse entities when importing multiple recipes.
                var existingCategories = await _context.Categories.ToListAsync();
                var existingIngredients = await _context.Ingredients.ToListAsync();
                var existingTitles = await _context.Recipes.Select(r => r.Title.ToLower()).ToListAsync();

                foreach (var day in mealPlan.Days)
                {
                    var aiRecipe = day.Recipe;

                    // Skip recipes already present by title to avoid creating duplicates.
                    if (existingTitles.Contains(aiRecipe.Title.ToLower()))
                    {
                        continue;
                    }

                    var category = existingCategories
                        .FirstOrDefault(c => string.Equals(c.Name, aiRecipe.CategoryName, StringComparison.OrdinalIgnoreCase));

                    if (category == null)
                    {
                        // Create and track a new category so subsequent recipes in this import reuse it.
                        category = new Category { Name = aiRecipe.CategoryName };
                        _context.Categories.Add(category);
                        existingCategories.Add(category); // track locally to avoid duplicates during import
                    }

                    var newRecipe = new Recipe
                    {
                        Title = $"{day.DayName}: {aiRecipe.Title}",
                        Category = category,
                        TotalTimeMinutes = aiRecipe.TotalTimeMinutes,
                        Portions = aiRecipe.Portions,
                        Instructions = aiRecipe.Instructions,
                        RecipeIngredients = new List<RecipeIngredient>(),
                        CreatedAt = DateTime.UtcNow
                    };

                    // Remove duplicate ingredient names coming from the AI to prevent repeated RecipeIngredient entries.
                    var uniqueAiIngredients = aiRecipe.Ingredients
                        .DistinctBy(i => i.IngredientName.Trim().ToLower())
                        .ToList();

                    foreach (var ing in uniqueAiIngredients)
                    {
                        var ingredient = existingIngredients
                            .FirstOrDefault(i => string.Equals(i.Name, ing.IngredientName, StringComparison.OrdinalIgnoreCase));

                        if (ingredient == null)
                        {
                            // Create and track the ingredient so other recipes/ingredients reuse it within this import run.
                            ingredient = new Ingredient
                            {
                                Name = ing.IngredientName,
                                CreatedAt = DateTime.UtcNow,
                            };
                            _context.Ingredients.Add(ingredient);
                            existingIngredients.Add(ingredient); // track locally
                        }

                        newRecipe.RecipeIngredients.Add(new RecipeIngredient
                        {
                            Ingredient = ingredient,
                            Amount = ParseAmount(ing.Amount),
                            Unit = ing.Unit
                        });
                    }


                    _context.Recipes.Add(newRecipe);
                    // Track the title to avoid duplicate recipe creation for later days in the same import.
                    existingTitles.Add(aiRecipe.Title.ToLower());
                }

                await _context.SaveChangesAsync();
                // Persist all new entities in a single transaction and invalidate the recipe cache
                // so clients see the newly imported recipes.
                await _cache.RemoveByTagAsync("all-recipes", default);
                _logger.LogInformation("Successfully imported weekly menu with prompt: {Prompt}", prompt);
                return mealPlan;
            }
            catch (LlmClientBadGatewayException ex)
            {
                _logger.LogError(ex, "Communication error with Llm Service during import.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while importing weekly menu.");
                throw;
            }
        }

        public async Task<PagedResponse<RecipeResponse>> GetRecipesAsync(int page, int pageSize, string? category, int? totalTimeMinutes, string? titleContains)
        {
            // Build a cache key that includes filter and pagination parameters so each distinct query is cached separately.
            var cacheKey = $"recipes_page{page}_size{pageSize}_cat{category}_time{totalTimeMinutes}_title{titleContains}";
            var tags = new[] { "all-recipes" };

            // Use hybrid cache to avoid hitting the DB for repeated identical queries. Tagging allows invalidation on create/update/delete.
            return await _cache.GetOrCreateAsync(cacheKey, async cancel =>
            {
                var query = _context.Recipes
                    .Include(r => r.Category)
                    .Include(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(category))
                {
                    // Case-insensitive category filter. We compare lowercase strings here for simplicity.
                    query = query.Where(r => r.Category.Name.ToLower() == category.ToLower());
                }

                if (totalTimeMinutes.HasValue)
                {
                    // Filter recipes that take at most the given total time to satisfy client constraints.
                    query = query.Where(r => r.TotalTimeMinutes <= totalTimeMinutes.Value);
                }

                if (!string.IsNullOrWhiteSpace(titleContains))
                {
                    // Case-insensitive substring search on title for simple client-side searching.
                    query = query.Where(r => r.Title.ToLower().Contains(titleContains.ToLower()));
                }

                // Count total results before applying Skip/Take so we can compute pagination metadata.
                var totalCount = await query.CountAsync();

                // Pagination: Skip and Take - calculate the window of items to return for this page.
                // We count first so clients receive total pages and navigation flags.
                var recipes = await query
                    .OrderBy(r => r.Title)
                    .Skip((page - 1) * pageSize) // Skip items from previous pages
                    .Take(pageSize) // Take the number of items for the current page
                    .ToListAsync();

                var recipeResponses = recipes.Select(r => new RecipeResponse(
                        r.RecipeId,
                        r.Title,
                        r.Category.Name,
                        r.TotalTimeMinutes,
                        r.Portions,
                        r.RecipeIngredients.Select(ri => new RecipeIngredientDto
                        {
                            IngredientName = ri.Ingredient.Name,
                            Amount = FormatAmount(ri.Amount),
                            Unit = ri.Unit
                        }).ToList(),
                        r.Instructions,
                        r.CreatedAt
                    ))
                    .ToList();

                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Build pagination metadata so clients can render page controls.
                var meta = new PaginationMeta(
                    page,
                    pageSize,
                    totalPages,
                    totalCount,
                    page < totalPages,
                    page > 1
                );

                return new PagedResponse<RecipeResponse>(recipeResponses, meta);
            },
            tags: tags
            );
        }

        public async Task<RecipeResponse?> GetRecipeByIdAsync(int recipeId)
        {
            var query = _context.Recipes.AsQueryable();

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (recipe == null)
            {
                throw new NotFoundException($"Recipe with ID {recipeId} was not found in the database.");
            }

            return new RecipeResponse(
            recipe.RecipeId,
            recipe.Title,
            recipe.Category.Name,
            recipe.TotalTimeMinutes,
            recipe.Portions,
            recipe.RecipeIngredients.Select(ri => new RecipeIngredientDto
            {
                IngredientName = ri.Ingredient.Name,
                Amount = FormatAmount(ri.Amount),
                Unit = ri.Unit
            }).ToList(),
            recipe.Instructions,
            recipe.CreatedAt);
        }

        public async Task<RecipeResponse> CreateRecipeAsync(RecipeCreateDto request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == request.CategoryName.ToLower());

            if (category == null)
            {
                // Create a new category if none exists with the given name
                category = new Category { Name = request.CategoryName };
            }

            var ingredientNames = request.Ingredients.Select(i => i.IngredientName.ToLower()).ToList();

            var existingIngredients = await _context.Ingredients
                .Where(i => ingredientNames.Contains(i.Name.ToLower()))
                .ToListAsync();

            var newRecipe = new Recipe
            {
                Title = request.Title,
                Category = category,
                TotalTimeMinutes = request.TotalTimeMinutes,
                Portions = request.Portions,
                RecipeIngredients = request.Ingredients.Select(i =>
                {
                    var existing = existingIngredients
                        .FirstOrDefault(ei => ei.Name.ToLower() == i.IngredientName.ToLower());

                    return new RecipeIngredient
                    {
                        Ingredient = existing ?? new Ingredient { Name = i.IngredientName },
                        Amount = ParseAmount(i.Amount),
                        Unit = i.Unit
                    };
                }).ToList(),
                Instructions = request.Instructions,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Add(newRecipe);
            await _context.SaveChangesAsync();

            // Clear all cached entries tagged with "all-recipes" so clients see updated data
            await _cache.RemoveByTagAsync("all-recipes");

            return new RecipeResponse(
                newRecipe.RecipeId,
                newRecipe.Title,
                newRecipe.Category.Name,
                newRecipe.TotalTimeMinutes,
                newRecipe.Portions,
                newRecipe.RecipeIngredients.Select(ri => new RecipeIngredientDto
                {
                    IngredientName = ri.Ingredient.Name,
                    Amount = FormatAmount(ri.Amount),
                    Unit = ri.Unit
                }).ToList(),
                newRecipe.Instructions,
                newRecipe.CreatedAt
            );
        }

        public async Task<RecipeResponse?> UpdateRecipeAsync(int recipeId, RecipeUpdateDto request)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (recipe == null)
            {
                throw new NotFoundException($"Recipe with ID {recipeId} was not found in the database.");
            }

            // Update the entity with the provided values
            if (request.Title != null)
            {
                recipe.Title = request.Title;
            }

            if (request.Category != null)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Category.Name.ToLower());

                if (category == null)
                {
                    // If the requested category does not exist, create it
                    category = new Category { Name = request.Category.Name };
                }
                recipe.Category = category;
            }

            if (request.TotalTimeMinutes != null)
            {
                recipe.TotalTimeMinutes = request.TotalTimeMinutes.Value;
            }

            if (request.Portions != null)
            {
                recipe.Portions = request.Portions.Value;
            }

            if (request.Ingredients != null)
            {
                // Replace ingredients: clear current list and re-add from DTOs
                recipe.RecipeIngredients.Clear();
                var ingredientNames = request.Ingredients.Select(i => i.IngredientName.ToLower()).ToList();
                var existingIngredients = await _context.Ingredients
                   .Where(i => ingredientNames.Contains(i.Name.ToLower()))
                   .ToListAsync();

                foreach (var dto in request.Ingredients)
                {
                    var existing = existingIngredients.FirstOrDefault(ei => ei.Name.ToLower() == dto.IngredientName.ToLower());

                    recipe.RecipeIngredients.Add(new RecipeIngredient
                    {
                        Ingredient = existing ?? new Ingredient { Name = dto.IngredientName },
                        Amount = ParseAmount(dto.Amount),
                        Unit = dto.Unit
                    });
                }
            }

            if (request.Instructions != null)
            {
                recipe.Instructions = request.Instructions;
            }
            recipe.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Clear cache so clients receive the updated resource
            await _cache.RemoveByTagAsync("all-recipes");

            return new RecipeResponse(
                recipe.RecipeId,
                recipe.Title,
                recipe.Category.Name,
                recipe.TotalTimeMinutes,
                recipe.Portions,
                recipe.RecipeIngredients.Select(ri => new RecipeIngredientDto
                {
                    IngredientName = ri.Ingredient.Name,
                    Amount = FormatAmount(ri.Amount),
                    Unit = ri.Unit
                }).ToList(),
                recipe.Instructions,
                recipe.UpdatedAt
            );
        }

        public async Task DeleteRecipeAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            // If it doesn't exist, throw NotFound so callers know the resource is missing
            if (recipe == null)
            {
                throw new NotFoundException($"Recipe with ID {recipeId} was not found in the database.");
            }

            // Remove the recipe entity
            _context.Remove(recipe);
            await _context.SaveChangesAsync();

            // Clear cache entries tagged with "all-recipes" so other requests see the deletion
            await _cache.RemoveByTagAsync("all-recipes");
        }

        // Help-method: turns string into decimal (for database storage), if parsing fails return 0
        private decimal ParseAmount(string? amount) =>
            decimal.TryParse(amount, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0;

        // Help-method: turns decimal into string (for views only)
        private string FormatAmount(decimal? amount) =>
            (amount ?? 0).ToString(CultureInfo.InvariantCulture);
        
    }
}