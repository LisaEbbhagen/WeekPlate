using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiRecipe.Content.Api.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }
        public string Title { get; set; } = string.Empty;
        [ForeignKey ("CategoryId")]
        public int FKCategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int TotalTimeMinutes { get; set; }
        public int Portions { get; set; }     
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
        public string Instructions { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
