namespace BankApp.Abstractions;

/// <summary>
/// Defines the contract for account data access operations.
/// </summary>
/// <remarks>
/// This interface provides methods for managing account entities in the data store,
/// including CRUD operations and specialized queries for account lookup and validation.
/// </remarks>
public interface IAccountRepository
{
    /// <summary>
    /// Retrieves all accounts from the data store.
    /// </summary>
    /// <returns>An enumerable collection of all accounts.</returns>
    IEnumerable<Account> GetAll();
    
    /// <summary>
    /// Retrieves an account by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the account.</param>
    /// <returns>The account if found; otherwise, null.</returns>
    Account? GetById(string id);
    
    /// <summary>
    /// Retrieves an account by its account number.
    /// </summary>
    /// <param name="accountNumber">The account number to search for.</param>
    /// <returns>The account if found; otherwise, null.</returns>
    Account? GetByAccountNumber(string accountNumber);
    
    /// <summary>
    /// Adds a new account to the data store.
    /// </summary>
    /// <param name="account">The account to add.</param>
    void Add(Account account);
    
    /// <summary>
    /// Updates an existing account in the data store.
    /// </summary>
    /// <param name="account">The account with updated information.</param>
    void Update(Account account);
    
    /// <summary>
    /// Removes an account from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the account to delete.</param>
    void Delete(string id);
    
    /// <summary>
    /// Checks if an account exists in the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <returns>True if the account exists; otherwise, false.</returns>
    bool Exists(string id);
} 