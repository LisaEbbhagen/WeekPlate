namespace AiRecipe.Content.Api.DTOs
{
    public class ShoppingListItemDto
    {
        public string IngredientName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Unit { get; set; } = string.Empty;

        public string? UnparsedAmount { get; set; }
    }

    public class ShoppingListCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public List<ShoppingListItemDto> Items { get; set; } = new();
    }

    public class ShoppingListDto
    {
        public List<ShoppingListCategoryDto> Categories { get; set; } = new();
    }
}
