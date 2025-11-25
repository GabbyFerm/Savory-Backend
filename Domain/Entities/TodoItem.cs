using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a todo item in the system
/// </summary>
public class TodoItem : BaseEntity
{
    /// <summary>
    /// Title of the todo item
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional description with more details
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether the todo item has been completed
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Optional due date for the todo item
    /// </summary>
    public DateTime? DueDate { get; set; }
}