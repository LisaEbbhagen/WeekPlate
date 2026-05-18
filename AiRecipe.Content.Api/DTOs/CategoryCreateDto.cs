using System.ComponentModel.DataAnnotations;

namespace AiRecipe.Content.Api.DTOs
{
    public record CategoryCreateDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50, ErrorMessage = "The name must be between 1 and 50 characters.")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } 
        public string? IconUrl { get; set; } 
    }
}
