using AiRecipe.Content.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AiRecipe.Content.Api.Data
{
    public class RecipeDbContext : DbContext
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for the join table
            // This ensures an ingredient can appear only once per recipe.
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.FKRecipeId, ri.FKIngredientId });

            // Relation: Recipe -> RecipeIngredients
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.FKRecipeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete: when a recipe is removed, its RecipeIngredients are removed too(but the ingridients stays in the database).

            // Relation: Ingredient -> RecipeIngredients
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.FKIngredientId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict prevent accidentally deleting an ingredient that is used by a recipe.


            // Relation: Recipe -> Category
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Category)
                .WithMany() 
                .HasForeignKey(r => r.FKCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // We don't want to delete a category just because a recipe is removed

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Pasta", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 2, Name = "Soppa", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 3, Name = "Sallad", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 4, Name = "Gryta", CreatedAt = DateTime.UtcNow },
                new Category { CategoryId = 5, Name = "Asiatiskt", CreatedAt = DateTime.UtcNow }
            );

            // Seed Ingredients
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { IngredientId = 1, Name = "Spaghetti", IsAllergen = true, CreatedAt = DateTime.UtcNow }, // Gluten
                new Ingredient { IngredientId = 2, Name = "Tomatsås", IsAllergen = false, CreatedAt = DateTime.UtcNow },
                new Ingredient { IngredientId = 3, Name = "Kycklingfilé", IsAllergen = false, CreatedAt = DateTime.UtcNow },
                new Ingredient { IngredientId = 4, Name = "Vispgrädde", IsAllergen = true, CreatedAt = DateTime.UtcNow }, // Dairy
                new Ingredient { IngredientId = 5, Name = "Lax", IsAllergen = true, CreatedAt = DateTime.UtcNow }, // Fish
                new Ingredient { IngredientId = 6, Name = "Ris", IsAllergen = false, CreatedAt = DateTime.UtcNow },
                new Ingredient { IngredientId = 7, Name = "Linser", IsAllergen = false, CreatedAt = DateTime.UtcNow },
                new Ingredient { IngredientId = 8, Name = "Avokado", IsAllergen = false, CreatedAt = DateTime.UtcNow }
            );

            // Seed Recipes
            modelBuilder.Entity<Recipe>().HasData(
                new Recipe { RecipeId = 1, Title = "Snabb tomatpasta", FKCategoryId = 1, TotalTimeMinutes = 15, Portions = 4, Instructions = "Koka pastan, blanda med varm sås.", CreatedAt = DateTime.UtcNow },
                new Recipe { RecipeId = 2, Title = "Krämig kycklingsoppa", FKCategoryId = 2, TotalTimeMinutes = 30, Portions = 2, Instructions = "Stek kycklingen, tillsätt grädde och sjud.", CreatedAt = DateTime.UtcNow },
                new Recipe { RecipeId = 3, Title = "Laxsallad med avokado", FKCategoryId = 3, TotalTimeMinutes = 20, Portions = 2, Instructions = "Grilla laxen och blanda med sallad och avokado.", CreatedAt = DateTime.UtcNow },
                new Recipe { RecipeId = 4, Title = "Linsgryta", FKCategoryId = 4, TotalTimeMinutes = 45, Portions = 6, Instructions = "Koka linserna tills de är mjuka i en kryddig buljong.", CreatedAt = DateTime.UtcNow },
                new Recipe { RecipeId = 5, Title = "Kyckling med ris", FKCategoryId = 5, TotalTimeMinutes = 25, Portions = 3, Instructions = "Stek kycklingen, servera med kokt ris.", CreatedAt = DateTime.UtcNow }
            );

            // Seed Recipe-Ingredient Relationships
            modelBuilder.Entity<RecipeIngredient>().HasData(
                // Recipe 1: Pasta
                new RecipeIngredient { FKRecipeId = 1, FKIngredientId = 1, Amount = 400, Unit = "g" },
                new RecipeIngredient { FKRecipeId = 1, FKIngredientId = 2, Amount = 500, Unit = "g" },

                // Recipe 2: Soup
                new RecipeIngredient { FKRecipeId = 2, FKIngredientId = 3, Amount = 300, Unit = "g" },
                new RecipeIngredient { FKRecipeId = 2, FKIngredientId = 4, Amount = 2, Unit = "dl" },

                // Recipe 3: Salad
                new RecipeIngredient { FKRecipeId = 3, FKIngredientId = 5, Amount = 2, Unit = "st" },
                new RecipeIngredient { FKRecipeId = 3, FKIngredientId = 8, Amount = 1, Unit = "st" },

                // Recipe 4: Stew
                new RecipeIngredient { FKRecipeId = 4, FKIngredientId = 7, Amount = 3, Unit = "dl" },

                // Recipe 5: Asian
                new RecipeIngredient { FKRecipeId = 5, FKIngredientId = 3, Amount = 400, Unit = "g" },
                new RecipeIngredient { FKRecipeId = 5, FKIngredientId = 6, Amount = 4, Unit = "portioner" }
            );
        }
    }
}
