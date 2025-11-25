namespace Application.Interfaces;

/// <summary>
/// Generic repository interface for basic CRUD operations.
/// This interface can be implemented for any entity type.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Gets an entity by its unique identifier
    /// </summary>
    /// <param name="id">The entity ID</param>
    /// <returns>The entity if found, null otherwise</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all entities of type T
    /// </summary>
    /// <returns>A collection of all entities</returns>
    Task<IEnumerable<T>> ListAsync();

    /// <summary>
    /// Adds a new entity to the repository
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <returns>The added entity with generated ID</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">The entity to update</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity by its ID
    /// </summary>
    /// <param name="id">The entity ID to delete</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Saves all pending changes to the database
    /// </summary>
    Task<int> SaveChangesAsync();
}