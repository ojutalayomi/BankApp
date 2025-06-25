namespace BankApp.Abstractions.Enums;

/// <summary>
/// Represents the different types of banking transactions.
/// </summary>
/// <remarks>
/// This enum defines the standard transaction types that can be performed
/// in the banking system, each with different business logic and validation rules.
/// </remarks>
public enum TransactionType
{
    /// <summary>
    /// A transaction that removes money from an account.
    /// </summary>
    /// <remarks>
    /// Withdrawal transactions decrease the account balance and typically
    /// require sufficient funds to be available in the account.
    /// </remarks>
    Withdraw,
    
    /// <summary>
    /// A transaction that adds money to an account.
    /// </summary>
    /// <remarks>
    /// Deposit transactions increase the account balance and are typically
    /// the most straightforward type of transaction to process.
    /// </remarks>
    Deposit,
    
    /// <summary>
    /// A transaction that moves money from one account to another.
    /// </summary>
    /// <remarks>
    /// Transfer transactions involve two accounts - a source account that
    /// loses money and a destination account that gains money. Both accounts
    /// must be valid and the source account must have sufficient funds.
    /// </remarks>
    Transfer
} 