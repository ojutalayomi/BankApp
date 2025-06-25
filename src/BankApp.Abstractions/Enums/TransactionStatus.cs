namespace BankApp.Abstractions.Enums;

/// <summary>
/// Represents the different states a transaction can be in during its lifecycle.
/// </summary>
/// <remarks>
/// This enum tracks the progress of banking transactions from initiation
/// through to completion, failure, or cancellation.
/// </remarks>
public enum TransactionStatus
{
    /// <summary>
    /// The transaction has been initiated but not yet processed.
    /// </summary>
    /// <remarks>
    /// This is the initial state when a transaction is first created.
    /// The transaction is waiting to be processed by the banking system.
    /// </remarks>
    Pending,
    
    /// <summary>
    /// The transaction has been successfully processed and completed.
    /// </summary>
    /// <remarks>
    /// The transaction has been executed successfully and the account
    /// balances have been updated accordingly.
    /// </remarks>
    Completed,
    
    /// <summary>
    /// The transaction failed to process due to an error or insufficient funds.
    /// </summary>
    /// <remarks>
    /// The transaction could not be completed due to various reasons such as
    /// insufficient funds, account frozen, or system errors.
    /// </remarks>
    Failed,
    
    /// <summary>
    /// The transaction was cancelled before processing.
    /// </summary>
    /// <remarks>
    /// The transaction was cancelled either by the user or by the system
    /// before it could be processed.
    /// </remarks>
    Cancelled
} 