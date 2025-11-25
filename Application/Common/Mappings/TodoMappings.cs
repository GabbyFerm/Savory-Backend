using Application.DTOs;
using Domain.Entities;

namespace Application.Common.Mappings;

/// <summary>
/// Simple mapping methods for TodoItem entity to DTOs
/// (Alternative to AutoMapper for lightweight projects)
/// </summary>
public static class TodoMappings
{
    public static TodoItemDto ToDto(this TodoItem entity)
    {
        return new TodoItemDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            IsCompleted = entity.IsCompleted,
            DueDate = entity.DueDate,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}