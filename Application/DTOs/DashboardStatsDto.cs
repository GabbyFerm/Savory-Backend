namespace Application.DTOs;

/// <summary>
/// Dashboard statistics for current user
/// </summary>
public class DashboardStatsDto
{
    /// <summary>
    /// Total number of recipes
    /// </summary>
    public int TotalRecipes { get; set; }

    /// <summary>
    /// Recipes grouped by category
    /// </summary>
    public Dictionary<string, int> RecipesByCategory { get; set; } = new();

    /// <summary>
    /// Average cooking time in minutes
    /// </summary>
    public double AverageCookTime { get; set; }

    /// <summary>
    /// Average total time (prep + cook) in minutes
    /// </summary>
    public double AverageTotalTime { get; set; }

    /// <summary>
    /// Most recent recipes
    /// </summary>
    public List<RecipeDto> RecentRecipes { get; set; } = new();
}