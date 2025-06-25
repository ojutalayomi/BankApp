using BankApp.Abstractions.Enums;

namespace BankApp.Abstractions;

/// <summary>
/// Provides static methods for generating account numbers and creating account instances.
/// </summary>
/// <remarks>
/// This utility class handles the generation of unique account numbers and provides
/// convenience methods for creating different types of accounts with appropriate naming.
/// </remarks>
public static class AccountGenerator
{
    /// <summary>
    /// The starting account number for new accounts.
    /// </summary>
    private static int _accountNumberCounter = 1000000; // Starting account number
    
    /// <summary>
    /// Lock object for thread-safe account number generation.
    /// </summary>
    private static readonly object LockObject = new object();

    /// <summary>
    /// Generates a unique account number.
    /// </summary>
    /// <returns>A unique account number as string.</returns>
    /// <remarks>
    /// This method uses a thread-safe counter to ensure each generated account number
    /// is unique across the application. The account numbers start from 1000000 and
    /// increment sequentially.
    /// </remarks>
    public static string GenerateAccountNumber()
    {
        lock (LockObject)
        {
            return (++_accountNumberCounter).ToString();
        }
    }

    /// <summary>
    /// Creates a new account with generated account number.
    /// </summary>
    /// <param name="accountName">Name of the account.</param>
    /// <param name="accountType">Type of account (Current or Saving).</param>
    /// <returns>A new Account instance with a generated account number.</returns>
    /// <remarks>
    /// This method combines account number generation with account creation for convenience.
    /// The account number is automatically generated using the GenerateAccountNumber method.
    /// </remarks>
    public static Account CreateAccount(string accountName, AccountType accountType)
    {
        var accountNumber = GenerateAccountNumber();
        return new Account(accountName, accountNumber, accountType);
    }

    /// <summary>
    /// Creates a new account with specified account number.
    /// </summary>
    /// <param name="accountName">Name of the account.</param>
    /// <param name="accountNumber">Account number to use.</param>
    /// <param name="accountType">Type of account (Current or Saving).</param>
    /// <returns>A new Account instance with the specified account number.</returns>
    /// <remarks>
    /// This method allows manual specification of the account number, which is useful
    /// for testing or when importing accounts from external systems.
    /// </remarks>
    public static Account CreateAccount(string accountName, string accountNumber, AccountType accountType)
    {
        return new Account(accountName, accountNumber, accountType);
    }

    /// <summary>
    /// Creates a default current account for a customer.
    /// </summary>
    /// <param name="customerName">Customer name to use as account name.</param>
    /// <returns>A new Current Account instance with a generated account number.</returns>
    /// <remarks>
    /// This convenience method creates a current account with a standardized naming
    /// convention: "{customerName}'s Current Account".
    /// </remarks>
    public static Account CreateDefaultCurrentAccount(string customerName)
    {
        return CreateAccount($"{customerName}'s Current Account", AccountType.Current);
    }

    /// <summary>
    /// Creates a default saving account for a customer.
    /// </summary>
    /// <param name="customerName">Customer name to use as account name.</param>
    /// <returns>A new Saving Account instance with a generated account number.</returns>
    /// <remarks>
    /// This convenience method creates a saving account with a standardized naming
    /// convention: "{customerName}'s Saving Account".
    /// </remarks>
    public static Account CreateDefaultSavingAccount(string customerName)
    {
        return CreateAccount($"{customerName}'s Saving Account", AccountType.Saving);
    }
} 