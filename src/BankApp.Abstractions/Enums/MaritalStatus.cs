namespace BankApp.Abstractions.Enums;

/// <summary>
/// Represents the marital status options for customers.
/// </summary>
/// <remarks>
/// This enum provides standard marital status categories for customer demographic
/// information and regulatory compliance requirements.
/// </remarks>
public enum MaritalStatus
{
    /// <summary>
    /// Single or never married.
    /// </summary>
    Single,
    
    /// <summary>
    /// Currently married.
    /// </summary>
    Married,
    
    /// <summary>
    /// Divorced from previous marriage.
    /// </summary>
    Divorced,
    
    /// <summary>
    /// Widowed from previous marriage.
    /// </summary>
    Widowed
} 