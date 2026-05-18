using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AiRecipe.Content.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAllergen = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientId);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FKCategoryId = table.Column<int>(type: "int", nullable: false),
                    TotalTimeMinutes = table.Column<int>(type: "int", nullable: false),
                    Portions = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_Categories_FKCategoryId",
                        column: x => x.FKCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    FKRecipeId = table.Column<int>(type: "int", nullable: false),
                    FKIngredientId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => new { x.FKRecipeId, x.FKIngredientId });
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_FKIngredientId",
                        column: x => x.FKIngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_FKRecipeId",
                        column: x => x.FKRecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CreatedAt", "Description", "IconUrl", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3854), null, null, "Pasta", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3857), null, null, "Soup", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3858), null, null, "Salad", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3858), null, null, "Stew", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3859), null, null, "Asian", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "IngredientId", "CreatedAt", "IsAllergen", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3956), true, "Spaghetti", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3957), false, "Tomato Sauce", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3958), false, "Chicken Breast", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3959), true, "Heavy Cream", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3960), true, "Salmon", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3961), false, "Rice", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3962), false, "Lentils", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3963), false, "Avocado", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "CreatedAt", "FKCategoryId", "Instructions", "Portions", "Title", "TotalTimeMinutes", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3998), 1, "Boil pasta, mix with warm sauce.", 4, "Quick Tomato Pasta", 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(4000), 2, "Sauté chicken, add cream and simmer.", 2, "Creamy Chicken Soup", 30, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(4002), 3, "Grill the salmon and mix with salad and avocado.", 2, "Salmon Salad with Avocado", 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(4003), 4, "Cook lentils until soft in a spicy broth.", 6, "Lentil Stew", 45, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(4004), 5, "Fry the chicken, serve with boiled rice.", 3, "Chicken with Rice", 25, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "RecipeIngredients",
                columns: new[] { "FKIngredientId", "FKRecipeId", "Amount", "Unit" },
                values: new object[,]
                {
                    { 1, 1, 400m, "g" },
                    { 2, 1, 500m, "g" },
                    { 3, 2, 300m, "g" },
                    { 4, 2, 2m, "dl" },
                    { 5, 3, 2m, "pcs" },
                    { 8, 3, 1m, "pcs" },
                    { 7, 4, 3m, "dl" },
                    { 3, 5, 400m, "g" },
                    { 6, 5, 4m, "servings" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_FKIngredientId",
                table: "RecipeIngredients",
                column: "FKIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_FKCategoryId",
                table: "Recipes",
                column: "FKCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
