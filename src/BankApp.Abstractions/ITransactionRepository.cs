namespace BankApp.Abstractions;

/// <summary>
/// Defines the contract for transaction data access operations.
/// </summary>
/// <remarks>
/// This interface provides methods for managing transaction entities in the data store,
/// including CRUD operations and specialized queries for transaction history and reporting.
/// </remarks>
public interface ITransactionRepository
{
    /// <summary>
    /// Retrieves all transactions from the data store.
    /// </summary>
    /// <returns>An enumerable collection of all transactions.</returns>
    IEnumerable<Transaction> GetAll();
    
    /// <summary>
    /// Retrieves a transaction by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction.</param>
    /// <returns>The transaction if found; otherwise, null.</returns>
    Transaction? GetById(string id);
    
    /// <summary>
    /// Retrieves all transactions associated with a specific account number.
    /// </summary>
    /// <param name="accountNumber">The account number to search for transactions.</param>
    /// <returns>An enumerable collection of transactions for the specified account.</returns>
    IEnumerable<Transaction> GetByAccountNumber(string accountNumber);
    
    /// <summary>
    /// Adds a new transaction to the data store.
    /// </summary>
    /// <param name="transaction">The transaction to add.</param>
    void Add(Transaction transaction);
    
    /// <summary>
    /// Updates an existing transaction in the data store.
    /// </summary>
    /// <param name="transaction">The transaction with updated information.</param>
    void Update(Transaction transaction);
    
    /// <summary>
    /// Removes a transaction from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to delete.</param>
    void Delete(string id);
    
    /// <summary>
    /// Checks if a transaction exists in the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <returns>True if the transaction exists; otherwise, false.</returns>
    bool Exists(string id);
} 