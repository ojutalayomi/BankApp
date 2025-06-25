namespace BankApp.Abstractions;

/// <summary>
/// Defines a generic contract for data access operations.
/// </summary>
/// <typeparam name="T">The type of entity to manage.</typeparam>
/// <remarks>
/// This interface provides a generic set of CRUD operations that can be implemented
/// by any repository class. It serves as a base contract for all repository interfaces.
/// </remarks>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of type T from the data store.
    /// </summary>
    /// <returns>An enumerable collection of all entities.</returns>
    IEnumerable<T> GetAll();
    
    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    T? GetById(string id);
    
    /// <summary>
    /// Adds a new entity to the data store.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    void Add(T entity);
    
    /// <summary>
    /// Updates an existing entity in the data store.
    /// </summary>
    /// <param name="entity">The entity with updated information.</param>
    void Update(T entity);
    
    /// <summary>
    /// Removes an entity from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    void Delete(string id);
    
    /// <summary>
    /// Checks if an entity exists in the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <returns>True if the entity exists; otherwise, false.</returns>
    bool Exists(string id);
} 