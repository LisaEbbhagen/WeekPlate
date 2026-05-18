using AiRecipe.Content.Api.DTOs;
using System.ComponentModel.DataAnnotations;

namespace AiRecipe.Content.Api.DTOs
{
    public record RecipeUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Title must be between 1 and 100 characters.")]
        public string? Title { get; set; }
        
        [MaxLength(100, ErrorMessage = "Category name must be between 1 och 100 characters.")]
        public CategoryCreateDto? Category { get; set; }
        
        [Range(1, 1000, ErrorMessage = "Total time must be between 1 and 1000 minutes.")]
        public int? TotalTimeMinutes { get; set; }

        [Range(1, 20, ErrorMessage = "Portions must be between 1 and 20.")]
        public int? Portions { get; set; }
        public RecipeIngredientDto[]? Ingredients { get; set; } 
        public string? Instructions { get; set; } 
    }
}