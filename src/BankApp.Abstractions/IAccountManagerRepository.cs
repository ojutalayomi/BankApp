namespace BankApp.Abstractions;

/// <summary>
/// Defines the contract for account manager data access operations.
/// </summary>
/// <remarks>
/// This interface provides methods for managing account manager entities in the data store,
/// including CRUD operations and specialized queries for authentication and validation.
/// </remarks>
public interface IAccountManagerRepository
{
    /// <summary>
    /// Retrieves all account managers from the data store.
    /// </summary>
    /// <returns>An enumerable collection of all account managers.</returns>
    IEnumerable<AccountManager> GetAll();
    
    /// <summary>
    /// Retrieves an account manager by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the account manager.</param>
    /// <returns>The account manager if found; otherwise, null.</returns>
    AccountManager? GetById(string id);
    
    /// <summary>
    /// Retrieves an account manager by their username.
    /// </summary>
    /// <param name="username">The username of the account manager.</param>
    /// <returns>The account manager if found; otherwise, null.</returns>
    AccountManager? GetByUsername(string username);
    
    /// <summary>
    /// Retrieves an account manager by their email address.
    /// </summary>
    /// <param name="email">The email address of the account manager.</param>
    /// <returns>The account manager if found; otherwise, null.</returns>
    AccountManager? GetByEmail(string email);
    
    /// <summary>
    /// Adds a new account manager to the data store.
    /// </summary>
    /// <param name="accountManager">The account manager to add.</param>
    void Add(AccountManager accountManager);
    
    /// <summary>
    /// Updates an existing account manager in the data store.
    /// </summary>
    /// <param name="accountManager">The account manager with updated information.</param>
    void Update(AccountManager accountManager);
    
    /// <summary>
    /// Removes an account manager from the data store by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the account manager to delete.</param>
    void Delete(string id);
    
    /// <summary>
    /// Checks if an account manager exists in the data store by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <returns>True if the account manager exists; otherwise, false.</returns>
    bool Exists(string id);
    
    /// <summary>
    /// Checks if a username is already in use by another account manager.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns>True if the username exists; otherwise, false.</returns>
    bool UsernameExists(string username);
    
    /// <summary>
    /// Checks if an email address is already in use by another account manager.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>True if the email exists; otherwise, false.</returns>
    bool EmailExists(string email);
} 