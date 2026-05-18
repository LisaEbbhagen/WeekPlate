using System.ComponentModel.DataAnnotations;

namespace AiRecipe.Content.Api.DTOs
{
    public record RecipeIngredientDto()
    {
        [Required(ErrorMessage = "Ingredient name is required.")]
        [MaxLength(100, ErrorMessage = "Ingredient name must be between 1 and 100 characters.")]
        public string IngredientName { get; set; } = string.Empty;

        public string? Amount { get; set; }
        public string? Unit { get; set; }
    }
}
