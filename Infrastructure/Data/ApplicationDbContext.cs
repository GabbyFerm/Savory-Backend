using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure RecipeIngredient (many-to-many with quantity)
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeIngredients)
            .HasForeignKey(ri => ri.RecipeId);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId);

        // Configure Recipe -> User relationship
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.User)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);  // Delete recipes when user is deleted

        // Configure Recipe -> Category relationship
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Category)
            .WithMany(c => c.Recipes)
            .HasForeignKey(r => r.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);  // Don't delete category if recipes exist

        // Seed categories
        SeedCategories(modelBuilder);
    }

    private void SeedCategories(ModelBuilder modelBuilder)
    {
        var categories = new[]
        {
            new Category { Id = Guid.NewGuid(), Name = "Breakfast", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.NewGuid(), Name = "Lunch", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.NewGuid(), Name = "Dinner", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.NewGuid(), Name = "Dessert", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.NewGuid(), Name = "Snack", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.NewGuid(), Name = "Beverage", CreatedAt = DateTime.UtcNow }
        };

        modelBuilder.Entity<Category>().HasData(categories);
    }
}