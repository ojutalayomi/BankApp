using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Services;

/// <summary>
/// Provides business logic services for managing transactions.
/// </summary>
/// <remarks>
/// This service class implements the ITransactionService interface and provides
/// high-level business operations for transaction management including creation,
/// retrieval, and status updates.
/// </remarks>
public class TransactionService : ITransactionService
{
    /// <summary>
    /// The repository for account data access operations.
    /// </summary>
    private readonly IAccountRepository _accountRepo;
    
    /// <summary>
    /// The repository for transaction data access operations.
    /// </summary>
    private readonly ITransactionRepository _transactionRepo;

    /// <summary>
    /// Initializes a new instance of the TransactionService class.
    /// </summary>
    /// <param name="accountRepo">The account repository for data access.</param>
    /// <param name="transactionRepo">The transaction repository for data access.</param>
    /// <remarks>
    /// This constructor injects both account and transaction repository dependencies
    /// to enable comprehensive transaction management operations.
    /// </remarks>
    public TransactionService(IAccountRepository accountRepo, ITransactionRepository transactionRepo)
    {
        _accountRepo = accountRepo;
        _transactionRepo = transactionRepo;
    }

    /// <summary>
    /// Creates a new transaction in the system.
    /// </summary>
    /// <param name="transaction">The transaction to create.</param>
    /// <returns>The created transaction with any generated fields populated.</returns>
    /// <remarks>
    /// This method adds a new transaction to the repository. The transaction
    /// should have all required fields populated before calling this method.
    /// </remarks>
    public Transaction CreateTransaction(Transaction transaction)
    {
        _transactionRepo.Add(transaction);
        return transaction;
    }

    /// <summary>
    /// Retrieves a transaction by its unique identifier.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction to find.</param>
    /// <returns>The transaction if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for a transaction using its unique ID.
    /// </remarks>
    public Transaction? GetTransaction(string transactionId)
    {
        return _transactionRepo.GetById(transactionId);
    }

    /// <summary>
    /// Retrieves all transactions associated with a specific account.
    /// </summary>
    /// <param name="accountNumber">The account number to search for transactions.</param>
    /// <returns>An enumerable collection of transactions for the specified account.</returns>
    /// <remarks>
    /// This method returns all transactions where the account number appears
    /// as either the source or destination account.
    /// </remarks>
    public IEnumerable<Transaction> GetTransactionsByAccount(string accountNumber)
    {
        return _transactionRepo.GetByAccountNumber(accountNumber);
    }

    /// <summary>
    /// Retrieves the transaction history for a specific account.
    /// </summary>
    /// <param name="accountNumber">The account number to get transaction history for.</param>
    /// <returns>An enumerable collection of transactions for the specified account.</returns>
    /// <remarks>
    /// This method is an alias for GetTransactionsByAccount and returns all
    /// transactions associated with the specified account number.
    /// </remarks>
    public IEnumerable<Transaction> GetTransactionHistory(string accountNumber)
    {
        return _transactionRepo.GetByAccountNumber(accountNumber);
    }

    /// <summary>
    /// Retrieves all transactions from the system.
    /// </summary>
    /// <returns>An enumerable collection of all transactions.</returns>
    /// <remarks>
    /// This method returns all transactions in the system regardless of their status
    /// or associated accounts.
    /// </remarks>
    public IEnumerable<Transaction> GetAllTransactions()
    {
        return _transactionRepo.GetAll();
    }

    /// <summary>
    /// Updates the status of a specific transaction.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction to update.</param>
    /// <param name="status">The new status to set for the transaction.</param>
    /// <remarks>
    /// This method finds the transaction by ID and updates its status.
    /// If the transaction is not found, no action is taken.
    /// </remarks>
    public void UpdateTransactionStatus(string transactionId, TransactionStatus status)
    {
        var transaction = _transactionRepo.GetById(transactionId);
        if (transaction != null)
        {
            transaction.Status = status;
            _transactionRepo.Update(transaction);
        }
    }
} 