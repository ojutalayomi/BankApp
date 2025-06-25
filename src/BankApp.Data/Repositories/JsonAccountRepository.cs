using BankApp.Abstractions;
using BankApp.Data.JsonStorage;

namespace BankApp.Data.Repositories;

/// <summary>
/// Provides JSON-based persistence for Account entities.
/// </summary>
/// <remarks>
/// This repository implements the IAccountRepository interface using JSON files
/// for data storage. It provides CRUD operations for Account entities with
/// automatic serialization and deserialization of data to/from JSON format.
/// </remarks>
public class JsonAccountRepository : IAccountRepository
{
    /// <summary>
    /// Retrieves an account by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the account.</param>
    /// <returns>The account if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for an account using the account number as the identifier.
    /// It loads all accounts from the JSON file and searches for a match.
    /// </remarks>
    public Account? GetById(string id)
    {
        var accounts = JsonDatabase.LoadAccounts();
        return accounts.FirstOrDefault(a => a.AccountNumber == id);
    }

    /// <summary>
    /// Retrieves an account by its account number.
    /// </summary>
    /// <param name="accountNumber">The account number to search for.</param>
    /// <returns>The account if found; otherwise, null.</returns>
    /// <remarks>
    /// This method loads all accounts from the JSON file and searches for an account
    /// with the specified account number.
    /// </remarks>
    public Account? GetByAccountNumber(string accountNumber)
    {
        var accounts = JsonDatabase.LoadAccounts();
        return accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
    }

    /// <summary>
    /// Updates an existing account in the JSON storage.
    /// </summary>
    /// <param name="account">The account with updated information.</param>
    /// <remarks>
    /// This method loads all accounts, finds the account to update by account number,
    /// replaces it with the updated version, and saves the changes back to the JSON file.
    /// </remarks>
    public void Update(Account account)
    {
        var accounts = JsonDatabase.LoadAccounts();
        var existingIndex = accounts.FindIndex(a => a.AccountNumber == account.AccountNumber);
        
        if (existingIndex != -1)
        {
            accounts[existingIndex] = account;
            JsonDatabase.SaveAccounts(accounts);
        }
    }

    /// <summary>
    /// Adds a new account to the JSON storage.
    /// </summary>
    /// <param name="account">The account to add.</param>
    /// <remarks>
    /// This method loads all existing accounts, adds the new account to the list,
    /// and saves the updated list back to the JSON file.
    /// </remarks>
    public void Add(Account account)
    {
        var accounts = JsonDatabase.LoadAccounts();
        accounts.Add(account);
        JsonDatabase.SaveAccounts(accounts);
    }

    /// <summary>
    /// Removes an account from the JSON storage by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (account number) of the account to delete.</param>
    /// <remarks>
    /// This method loads all accounts, finds the account to delete by account number,
    /// removes it from the list, and saves the updated list back to the JSON file.
    /// </remarks>
    public void Delete(string id)
    {
        var accounts = JsonDatabase.LoadAccounts();
        var accountToRemove = accounts.FirstOrDefault(a => a.AccountNumber == id);
        
        if (accountToRemove != null)
        {
            accounts.Remove(accountToRemove);
            JsonDatabase.SaveAccounts(accounts);
        }
    }

    /// <summary>
    /// Retrieves all accounts from the JSON storage.
    /// </summary>
    /// <returns>An enumerable collection of all accounts.</returns>
    /// <remarks>
    /// This method loads and returns all accounts stored in the JSON file.
    /// </remarks>
    public IEnumerable<Account> GetAll()
    {
        return JsonDatabase.LoadAccounts();
    }

    /// <summary>
    /// Checks if an account exists in the JSON storage by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (account number) to check.</param>
    /// <returns>True if the account exists; otherwise, false.</returns>
    /// <remarks>
    /// This method loads all accounts and checks if any account has the specified account number.
    /// </remarks>
    public bool Exists(string id)
    {
        var accounts = JsonDatabase.LoadAccounts();
        return accounts.Any(a => a.AccountNumber == id);
    }
} 