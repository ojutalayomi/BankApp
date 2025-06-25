namespace BankApp.Abstractions.Enums;

/// <summary>
/// Represents the gender identity options for customers.
/// </summary>
/// <remarks>
/// This enum provides inclusive gender options for customer demographic information.
/// It supports standard gender categories as well as non-binary options.
/// </remarks>
public enum Gender
{
    /// <summary>
    /// Male gender identity.
    /// </summary>
    Male,
    
    /// <summary>
    /// Female gender identity.
    /// </summary>
    Female,
    
    /// <summary>
    /// Other gender identity or non-binary options.
    /// </summary>
    /// <remarks>
    /// This option includes non-binary, gender-fluid, and other gender identities
    /// that don't fit into the traditional male/female binary.
    /// </remarks>
    Other
} 