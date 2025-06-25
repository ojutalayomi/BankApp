using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Services;

/// <summary>
/// Defines the contract for transaction business operations.
/// </summary>
/// <remarks>
/// This interface provides methods for managing transactions including creation,
/// retrieval, and status updates. It serves as the contract for the TransactionService
/// implementation.
/// </remarks>
public interface ITransactionService
{
    /// <summary>
    /// Creates a new transaction in the system.
    /// </summary>
    /// <param name="transaction">The transaction to create.</param>
    /// <returns>The created transaction with any generated fields populated.</returns>
    /// <remarks>
    /// This method adds a new transaction to the repository. The transaction
    /// should have all required fields populated before calling this method.
    /// </remarks>
    Transaction CreateTransaction(Transaction transaction);
    
    /// <summary>
    /// Retrieves a transaction by its unique identifier.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction to find.</param>
    /// <returns>The transaction if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for a transaction using its unique ID.
    /// </remarks>
    Transaction? GetTransaction(string transactionId);
    
    /// <summary>
    /// Retrieves all transactions associated with a specific account.
    /// </summary>
    /// <param name="accountNumber">The account number to search for transactions.</param>
    /// <returns>An enumerable collection of transactions for the specified account.</returns>
    /// <remarks>
    /// This method returns all transactions where the account number appears
    /// as either the source or destination account.
    /// </remarks>
    IEnumerable<Transaction> GetTransactionsByAccount(string accountNumber);
    
    /// <summary>
    /// Retrieves the transaction history for a specific account.
    /// </summary>
    /// <param name="accountNumber">The account number to get transaction history for.</param>
    /// <returns>An enumerable collection of transactions for the specified account.</returns>
    /// <remarks>
    /// This method is an alias for GetTransactionsByAccount and returns all
    /// transactions associated with the specified account number.
    /// </remarks>
    IEnumerable<Transaction> GetTransactionHistory(string accountNumber);
    
    /// <summary>
    /// Retrieves all transactions from the system.
    /// </summary>
    /// <returns>An enumerable collection of all transactions.</returns>
    /// <remarks>
    /// This method returns all transactions in the system regardless of their status
    /// or associated accounts.
    /// </remarks>
    IEnumerable<Transaction> GetAllTransactions();
    
    /// <summary>
    /// Updates the status of a specific transaction.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction to update.</param>
    /// <param name="status">The new status to set for the transaction.</param>
    /// <remarks>
    /// This method finds the transaction by ID and updates its status.
    /// If the transaction is not found, no action is taken.
    /// </remarks>
    void UpdateTransactionStatus(string transactionId, TransactionStatus status);
} 