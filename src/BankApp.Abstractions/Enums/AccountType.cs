namespace BankApp.Abstractions.Enums;

/// <summary>
/// Represents the different types of bank accounts available.
/// </summary>
/// <remarks>
/// This enum defines the standard account types that can be created in the banking system.
/// Each account type may have different features, interest rates, and transaction limits.
/// </remarks>
public enum AccountType
{
    /// <summary>
    /// A current account for everyday banking transactions.
    /// </summary>
    /// <remarks>
    /// Current accounts typically have no or low interest rates but offer
    /// unlimited transactions and check-writing capabilities.
    /// </remarks>
    Current,
    
    /// <summary>
    /// A savings account for accumulating funds with interest.
    /// </summary>
    /// <remarks>
    /// Savings accounts typically offer higher interest rates than current accounts
    /// but may have transaction limits and restrictions.
    /// </remarks>
    Saving,
    
    /// <summary>
    /// A fixed deposit account that locks funds for a set period at a fixed interest rate.
    /// </summary>
    /// <remarks>
    /// Fixed deposit accounts offer the highest interest rates but funds cannot be withdrawn
    /// before maturity without incurring penalties. No transactions are allowed during the term.
    /// </remarks>
    FixedDeposit
} 