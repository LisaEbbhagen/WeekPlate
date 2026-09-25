using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiRecipe.Content.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDataToSwedish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7697));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7703), "Soppa" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7704), "Sallad" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7705), "Gryta" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7706), "Asiatiskt" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7815));

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7816), "Tomatsås" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7818), "Kycklingfilé" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7819), "Vispgrädde" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7820), "Lax" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7821), "Ris" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7822), "Linser" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7823), "Avokado" });

            migrationBuilder.UpdateData(
                table: "RecipeIngredients",
                keyColumns: new[] { "FKIngredientId", "FKRecipeId" },
                keyValues: new object[] { 5, 3 },
                column: "Unit",
                value: "st");

            migrationBuilder.UpdateData(
                table: "RecipeIngredients",
                keyColumns: new[] { "FKIngredientId", "FKRecipeId" },
                keyValues: new object[] { 8, 3 },
                column: "Unit",
                value: "st");

            migrationBuilder.UpdateData(
                table: "RecipeIngredients",
                keyColumns: new[] { "FKIngredientId", "FKRecipeId" },
                keyValues: new object[] { 6, 5 },
                column: "Unit",
                value: "portioner");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7857), "Koka pastan, blanda med varm sås.", "Snabb tomatpasta" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7860), "Stek kycklingen, tillsätt grädde och sjud.", "Krämig kycklingsoppa" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7862), "Grilla laxen och blanda med sallad och avokado.", "Laxsallad med avokado" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7863), "Koka linserna tills de är mjuka i en kryddig buljong.", "Linsgryta" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7865), "Stek kycklingen, servera med kokt ris.", "Kyckling med ris" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3854));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3857), "Soup" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3858), "Salad" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3858), "Stew" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3859), "Asian" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3956));

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3957), "Tomato Sauce" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3958), "Chicken Breast" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3959), "Heavy Cream" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3960), "Salmon" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3961), "Rice" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3962), "Lentils" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3963), "Avocado" });

            migrationBuilder.UpdateData(
                table: "RecipeIngredients",
                keyColumns: new[] { "FKIngredientId", "FKRecipeId" },
                keyValues: new object[] { 5, 3 },
                column: "Unit",
                value: "pcs");

            migrationBuilder.UpdateData(
                table: "RecipeIngredients",
                keyColumns: new[] { "FKIngredientId", "FKRecipeId" },
                keyValues: new object[] { 8, 3 },
                column: "Unit",
                value: "pcs");

            migrationBuilder.UpdateData(
                table: "RecipeIngredients",
                keyColumns: new[] { "FKIngredientId", "FKRecipeId" },
                keyValues: new object[] { 6, 5 },
                column: "Unit",
                value: "servings");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(3998), "Boil pasta, mix with warm sauce.", "Quick Tomato Pasta" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(4000), "Sauté chicken, add cream and simmer.", "Creamy Chicken Soup" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(4002), "Grill the salmon and mix with salad and avocado.", "Salmon Salad with Avocado" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(4003), "Cook lentils until soft in a spicy broth.", "Lentil Stew" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Instructions", "Title" },
                values: new object[] { new DateTime(2026, 3, 26, 13, 37, 18, 75, DateTimeKind.Utc).AddTicks(4004), "Fry the chicken, serve with boiled rice.", "Chicken with Rice" });
        }
    }
}
