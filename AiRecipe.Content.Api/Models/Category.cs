using System.ComponentModel.DataAnnotations;

namespace AiRecipe.Content.Api.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconUrl { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
 