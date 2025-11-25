using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}