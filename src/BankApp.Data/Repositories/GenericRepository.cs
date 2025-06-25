using BankApp.Abstractions;

namespace BankApp.Data.Repositories;

/// <summary>
/// Provides a generic implementation of the IRepository interface for in-memory storage.
/// </summary>
/// <typeparam name="T">The type of entity to manage.</typeparam>
/// <remarks>
/// This class provides a basic in-memory implementation of the repository pattern.
/// It uses a Dictionary for storage and is primarily intended for testing or
/// simple scenarios where persistence is not required.
/// </remarks>
public class GenericRepository<T> : IRepository<T> where T : class
{
    /// <summary>
    /// The in-memory storage dictionary for entities.
    /// </summary>
    /// <remarks>
    /// This dictionary stores entities using string keys. In a production environment,
    /// you would want to implement proper key generation based on entity properties.
    /// </remarks>
    protected readonly Dictionary<string, T> _storage = new();

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches the in-memory dictionary for an entity with the specified ID.
    /// </remarks>
    public virtual T? GetById(string id)
    {
        return _storage.TryGetValue(id, out var entity) ? entity : null;
    }

    /// <summary>
    /// Retrieves all entities from the repository.
    /// </summary>
    /// <returns>An enumerable collection of all entities.</returns>
    /// <remarks>
    /// This method returns all values stored in the in-memory dictionary.
    /// </remarks>
    public virtual IEnumerable<T> GetAll()
    {
        return _storage.Values;
    }

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <remarks>
    /// This method generates a new GUID as the key and stores the entity in the dictionary.
    /// In a production implementation, you would want to use a more appropriate key strategy.
    /// </remarks>
    public virtual void Add(T entity)
    {
        // This is a simplified implementation. In a real scenario,
        // you would need to determine the key based on the entity type
        var key = Guid.NewGuid().ToString();
        _storage[key] = entity;
    }

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity with updated information.</param>
    /// <remarks>
    /// This method generates a new GUID as the key and stores the updated entity.
    /// In a production implementation, you would want to update the existing entity
    /// using its actual identifier.
    /// </remarks>
    public virtual void Update(T entity)
    {
        // This is a simplified implementation. In a real scenario,
        // you would need to determine the key based on the entity type
        var key = Guid.NewGuid().ToString();
        _storage[key] = entity;
    }

    /// <summary>
    /// Removes an entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <remarks>
    /// This method removes the entity with the specified ID from the dictionary
    /// if it exists.
    /// </remarks>
    public virtual void Delete(string id)
    {
        if (_storage.ContainsKey(id))
        {
            _storage.Remove(id);
        }
    }

    /// <summary>
    /// Checks if an entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <returns>True if the entity exists; otherwise, false.</returns>
    /// <remarks>
    /// This method checks if the dictionary contains an entry with the specified ID.
    /// </remarks>
    public virtual bool Exists(string id)
    {
        return _storage.ContainsKey(id);
    }
} 