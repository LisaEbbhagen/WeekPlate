using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiRecipe.Content.Api.Models
{
    public class RecipeIngredient
    {        
        [ForeignKey("Recipe")]
        public int FKRecipeId { get; set; }

        [ForeignKey("Ingredient")]
        public int FKIngredientId { get; set; }

        public Ingredient Ingredient { get; set; } = null!;

        [Column(TypeName = "decimal(6,2)")]
        public decimal? Amount { get; set; }
        public string? Unit { get; set; } 
        public Recipe Recipe { get; set; } = null!;

    }
}
