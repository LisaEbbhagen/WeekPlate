using System.ComponentModel.DataAnnotations;

namespace AiRecipe.Content.Api.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAllergen { get; set; }
        public string? Type { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
    }
}
