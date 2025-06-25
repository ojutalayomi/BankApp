using BankApp.Abstractions;
using BankApp.Data.JsonStorage;

namespace BankApp.Data.Repositories;

/// <summary>
/// Provides JSON-based persistence for Transaction entities.
/// </summary>
/// <remarks>
/// This repository implements the ITransactionRepository interface using JSON files
/// for data storage. It provides CRUD operations for Transaction entities with
/// automatic serialization and deserialization of data to/from JSON format.
/// </remarks>
public class JsonTransactionRepository : ITransactionRepository
{
    /// <summary>
    /// Retrieves a transaction by its unique identifier.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction.</param>
    /// <returns>The transaction if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for a transaction using its unique ID.
    /// It loads all transactions from the JSON file and searches for a match.
    /// </remarks>
    public Transaction? GetById(string transactionId)
    {
        var transactions = JsonDatabase.LoadTransactions();
        return transactions.FirstOrDefault(t => t.Id == transactionId);
    }

    /// <summary>
    /// Updates an existing transaction in the JSON storage.
    /// </summary>
    /// <param name="transaction">The transaction with updated information.</param>
    /// <remarks>
    /// This method loads all transactions, finds the transaction to update by ID,
    /// replaces it with the updated version, and saves the changes back to the JSON file.
    /// </remarks>
    public void Update(Transaction transaction)
    {
        var transactions = JsonDatabase.LoadTransactions();
        var existingIndex = transactions.FindIndex(t => t.Id == transaction.Id);
        
        if (existingIndex != -1)
        {
            transactions[existingIndex] = transaction;
            JsonDatabase.SaveTransactions(transactions);
        }
    }

    /// <summary>
    /// Adds a new transaction to the JSON storage.
    /// </summary>
    /// <param name="transaction">The transaction to add.</param>
    /// <remarks>
    /// This method loads all existing transactions, adds the new transaction to the list,
    /// and saves the updated list back to the JSON file.
    /// </remarks>
    public void Add(Transaction transaction)
    {
        var transactions = JsonDatabase.LoadTransactions();
        transactions.Add(transaction);
        JsonDatabase.SaveTransactions(transactions);
    }

    /// <summary>
    /// Removes a transaction from the JSON storage by its unique identifier.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction to delete.</param>
    /// <remarks>
    /// This method loads all transactions, finds the transaction to delete by ID,
    /// removes it from the list, and saves the updated list back to the JSON file.
    /// </remarks>
    public void Delete(string transactionId)
    {
        var transactions = JsonDatabase.LoadTransactions();
        var transactionToRemove = transactions.FirstOrDefault(t => t.Id == transactionId);
        
        if (transactionToRemove != null)
        {
            transactions.Remove(transactionToRemove);
            JsonDatabase.SaveTransactions(transactions);
        }
    }

    /// <summary>
    /// Retrieves all transactions from the JSON storage.
    /// </summary>
    /// <returns>An enumerable collection of all transactions.</returns>
    /// <remarks>
    /// This method loads and returns all transactions stored in the JSON file.
    /// </remarks>
    public IEnumerable<Transaction> GetAll()
    {
        return JsonDatabase.LoadTransactions();
    }

    /// <summary>
    /// Retrieves all transactions associated with a specific account number.
    /// </summary>
    /// <param name="accountNumber">The account number to search for transactions.</param>
    /// <returns>An enumerable collection of transactions for the specified account.</returns>
    /// <remarks>
    /// This method loads all transactions and filters them to return only those
    /// where the account number appears as either the source or destination account.
    /// This is useful for generating account statements and transaction history.
    /// </remarks>
    public IEnumerable<Transaction> GetByAccountNumber(string accountNumber)
    {
        var transactions = JsonDatabase.LoadTransactions();
        return transactions.Where(t => 
            t.SourceAccount == accountNumber || t.DestinationAccount == accountNumber);
    }

    /// <summary>
    /// Checks if a transaction exists in the JSON storage by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <returns>True if the transaction exists; otherwise, false.</returns>
    /// <remarks>
    /// This method loads all transactions and checks if any transaction has the specified ID.
    /// </remarks>
    public bool Exists(string id)
    {
        var transactions = JsonDatabase.LoadTransactions();
        return transactions.Any(t => t.Id == id);
    }
} 