using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiRecipe.Content.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreCategoryToIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Ingredients");

            migrationBuilder.AddColumn<string>(
                name: "StoreCategory",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1743));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1745));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1746));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1747));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1747));

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "StoreCategory" },
                values: new object[] { new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1859), "Övrigt" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "StoreCategory" },
                values: new object[] { new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1860), "Övrigt" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "StoreCategory" },
                values: new object[] { new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1862), "Övrigt" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "StoreCategory" },
                values: new object[] { new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1863), "Övrigt" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "StoreCategory" },
                values: new object[] { new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1864), "Övrigt" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "StoreCategory" },
                values: new object[] { new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1864), "Övrigt" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "StoreCategory" },
                values: new object[] { new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1865), "Övrigt" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "StoreCategory" },
                values: new object[] { new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1866), "Övrigt" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1897));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1898));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1900));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1901));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 25, 8, 55, 24, 242, DateTimeKind.Utc).AddTicks(1903));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreCategory",
                table: "Ingredients");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: true);

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
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7703));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7704));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7705));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7706));

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7815), "" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7816), "" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7818), "" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7819), "" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7820), "" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7821), "" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7822), "" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7823), "" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7857));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7860));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7862));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7863));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 11, 46, 56, 631, DateTimeKind.Utc).AddTicks(7865));
        }
    }
}
