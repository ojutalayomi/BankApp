using BankApp.Abstractions.Enums;

namespace BankApp.Abstractions;

/// <summary>
/// Represents a banking transaction with comprehensive tracking and metadata.
/// </summary>
/// <remarks>
/// This class captures all details of a banking transaction including type, amount,
/// source and destination accounts, status, timing, and additional metadata for
/// audit and reporting purposes.
/// </remarks>
public class Transaction
{
    /// <summary>
    /// Gets or sets the unique identifier for the transaction.
    /// </summary>
    /// <value>A string representing the transaction's unique ID.</value>
    public string Id { get; set; }
    
    /// <summary>
    /// Gets or sets the type of transaction (Withdraw, Deposit, or Transfer).
    /// </summary>
    /// <value>A TransactionType enum value indicating the transaction type.</value>
    public TransactionType TransactionType { get; set; }
    
    /// <summary>
    /// Gets or sets the monetary amount of the transaction.
    /// </summary>
    /// <value>A decimal representing the transaction amount in currency units.</value>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the account number of the source account.
    /// </summary>
    /// <value>A string representing the source account number, or empty for deposits.</value>
    public string SourceAccount { get; set; }
    
    /// <summary>
    /// Gets or sets the account number of the destination account.
    /// </summary>
    /// <value>A string representing the destination account number, or empty for withdrawals.</value>
    public string DestinationAccount { get; set; }
    
    /// <summary>
    /// Gets or sets the current status of the transaction.
    /// </summary>
    /// <value>A TransactionStatus enum value indicating the transaction's current state.</value>
    public TransactionStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the transaction was created.
    /// </summary>
    /// <value>A DateTime representing when the transaction was initiated.</value>
    public DateTime DateCreated { get; set; }
    
    /// <summary>
    /// Gets or sets the description or notes for the transaction.
    /// </summary>
    /// <value>A string containing the transaction description.</value>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets the account balance after the transaction was processed.
    /// </summary>
    /// <value>A decimal representing the balance after the transaction.</value>
    public decimal BalanceAfterTransaction { get; set; }
    
    /// <summary>
    /// Gets or sets the name or identifier of who initiated the transaction.
    /// </summary>
    /// <value>A string representing the transaction initiator.</value>
    public string Initiator { get; set; }
    
    /// <summary>
    /// Gets or sets the channel through which the transaction was performed.
    /// </summary>
    /// <value>A string representing the transaction channel (e.g., ATM, Online, Branch).</value>
    public string Channel { get; set; }
    
    /// <summary>
    /// Gets or sets the unique reference number for the transaction.
    /// </summary>
    /// <value>A string representing the transaction reference number.</value>
    public string ReferenceNumber { get; set; }

    /// <summary>
    /// Initializes a new instance of the Transaction class with default values.
    /// </summary>
    /// <remarks>
    /// This constructor is primarily used for JSON deserialization.
    /// All properties are initialized with sensible default values.
    /// </remarks>
    public Transaction()
    {
        Id = Guid.NewGuid().ToString();
        TransactionType = TransactionType.Deposit;
        Amount = 0;
        SourceAccount = string.Empty;
        DestinationAccount = string.Empty;
        Status = TransactionStatus.Pending;
        DateCreated = DateTime.Now;
        Description = string.Empty;
        BalanceAfterTransaction = 0;
        Initiator = string.Empty;
        Channel = string.Empty;
        ReferenceNumber = Guid.NewGuid().ToString();
    }
} 