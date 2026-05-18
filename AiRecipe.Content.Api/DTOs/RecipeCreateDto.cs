using System.ComponentModel.DataAnnotations;

namespace AiRecipe.Content.Api.DTOs
{
    public record RecipeCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "The title must be between 1 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        [MaxLength(100, ErrorMessage = "The category name must be between 1 and 100 characters.")]
        public string CategoryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Time is required (prep + cooking).")]
        [Range(1, 1000, ErrorMessage = "Total time must be between 1 and 1000 minutes.")]
        public int TotalTimeMinutes { get; set; }

        [Required(ErrorMessage = "Portions is required.")]
        [Range(1, 20, ErrorMessage = "The number of portions must be between 1 and 20")]
        public int Portions { get; set; } 

        [Required(ErrorMessage = "The recipe must contain at least one ingredient." )]
        public RecipeIngredientDto[] Ingredients { get; set; } = Array.Empty<RecipeIngredientDto>();

        [Required(ErrorMessage = "The recipe must have cooking instructions.")]
        public string Instructions { get; set; } = string.Empty;
    }
}
