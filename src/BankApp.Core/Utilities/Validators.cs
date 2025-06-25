using BankApp.Abstractions.Enums;

namespace BankApp.Core.Utilities;

/// <summary>
/// Provides validation utilities for the BankApp system.
/// </summary>
/// <remarks>
/// This static class contains various validation methods used throughout the application
/// to ensure data integrity and business rule compliance. All methods are static and
/// can be called without instantiating the class.
/// </remarks>
public static class Validators
{
    /// <summary>
    /// Validates customer name format and length.
    /// </summary>
    /// <param name="name">The customer name to validate.</param>
    /// <returns>True if the name is valid; otherwise, false.</returns>
    /// <remarks>
    /// A valid customer name must not be null or whitespace, and must be between
    /// 2 and 100 characters in length.
    /// </remarks>
    public static bool IsValidCustomerName(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.Length >= 2 && name.Length <= 100;
    }

    /// <summary>
    /// Validates customer age range.
    /// </summary>
    /// <param name="age">The age to validate.</param>
    /// <returns>True if the age is valid; otherwise, false.</returns>
    /// <remarks>
    /// A valid age must be between 18 and 120 years old, inclusive.
    /// This ensures customers are of legal age and the age is reasonable.
    /// </remarks>
    public static bool IsValidAge(int age)
    {
        return age >= 18 && age <= 120;
    }

    /// <summary>
    /// Validates phone number format.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <returns>True if the phone number is valid; otherwise, false.</returns>
    /// <remarks>
    /// A valid phone number must not be null or whitespace, and when cleaned of
    /// spaces, dashes, and parentheses, must contain only digits and be between
    /// 10 and 15 characters in length.
    /// </remarks>
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Remove spaces, dashes, and parentheses
        var cleaned = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        
        // Check if it's all digits and has reasonable length
        return cleaned.All(char.IsDigit) && cleaned.Length >= 10 && cleaned.Length <= 15;
    }

    /// <summary>
    /// Validates account number format.
    /// </summary>
    /// <param name="accountNumber">The account number to validate.</param>
    /// <returns>True if the account number is valid; otherwise, false.</returns>
    /// <remarks>
    /// A valid account number must not be null or whitespace, contain only digits,
    /// and be between 6 and 20 characters in length.
    /// </remarks>
    public static bool IsValidAccountNumber(string accountNumber)
    {
        return !string.IsNullOrWhiteSpace(accountNumber) && 
               accountNumber.All(char.IsDigit) && 
               accountNumber.Length >= 6 && 
               accountNumber.Length <= 20;
    }

    /// <summary>
    /// Validates transaction amount range.
    /// </summary>
    /// <param name="amount">The amount to validate.</param>
    /// <returns>True if the amount is valid; otherwise, false.</returns>
    /// <remarks>
    /// A valid transaction amount must be greater than zero and not exceed
    /// 1,000,000 (1 million) to prevent excessive transactions.
    /// </remarks>
    public static bool IsValidTransactionAmount(decimal amount)
    {
        return amount > 0 && amount <= 1000000; // Max 1 million per transaction
    }

    /// <summary>
    /// Validates email address format.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True if the email is valid; otherwise, false.</returns>
    /// <remarks>
    /// Uses the .NET MailAddress class to validate email format. A valid email
    /// must not be null or whitespace and must conform to standard email format.
    /// </remarks>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates address format and length.
    /// </summary>
    /// <param name="address">The address to validate.</param>
    /// <returns>True if the address is valid; otherwise, false.</returns>
    /// <remarks>
    /// A valid address must not be null or whitespace and must be between
    /// 10 and 200 characters in length to ensure it's detailed enough but not excessive.
    /// </remarks>
    public static bool IsValidAddress(string address)
    {
        return !string.IsNullOrWhiteSpace(address) && address.Length >= 10 && address.Length <= 200;
    }

    /// <summary>
    /// Validates nationality format and length.
    /// </summary>
    /// <param name="nationality">The nationality to validate.</param>
    /// <returns>True if the nationality is valid; otherwise, false.</returns>
    /// <remarks>
    /// A valid nationality must not be null or whitespace and must be between
    /// 2 and 50 characters in length.
    /// </remarks>
    public static bool IsValidNationality(string nationality)
    {
        return !string.IsNullOrWhiteSpace(nationality) && nationality.Length >= 2 && nationality.Length <= 50;
    }

    /// <summary>
    /// Validates gender enum value.
    /// </summary>
    /// <param name="gender">The gender to validate.</param>
    /// <returns>True if the gender is valid; otherwise, false.</returns>
    /// <remarks>
    /// Checks if the provided gender value is a defined value in the Gender enum.
    /// </remarks>
    public static bool IsValidGender(Gender gender)
    {
        return Enum.IsDefined(typeof(Gender), gender);
    }

    /// <summary>
    /// Validates marital status enum value.
    /// </summary>
    /// <param name="maritalStatus">The marital status to validate.</param>
    /// <returns>True if the marital status is valid; otherwise, false.</returns>
    /// <remarks>
    /// Checks if the provided marital status value is a defined value in the MaritalStatus enum.
    /// </remarks>
    public static bool IsValidMaritalStatus(MaritalStatus maritalStatus)
    {
        return Enum.IsDefined(typeof(MaritalStatus), maritalStatus);
    }

    /// <summary>
    /// Validates account type enum value.
    /// </summary>
    /// <param name="accountType">The account type to validate.</param>
    /// <returns>True if the account type is valid; otherwise, false.</returns>
    /// <remarks>
    /// Checks if the provided account type value is a defined value in the AccountType enum.
    /// </remarks>
    public static bool IsValidAccountType(AccountType accountType)
    {
        return Enum.IsDefined(typeof(AccountType), accountType);
    }

    /// <summary>
    /// Validates transaction type enum value.
    /// </summary>
    /// <param name="transactionType">The transaction type to validate.</param>
    /// <returns>True if the transaction type is valid; otherwise, false.</returns>
    /// <remarks>
    /// Checks if the provided transaction type value is a defined value in the TransactionType enum.
    /// </remarks>
    public static bool IsValidTransactionType(TransactionType transactionType)
    {
        return Enum.IsDefined(typeof(TransactionType), transactionType);
    }

    /// <summary>
    /// Validates complaint narration format and length.
    /// </summary>
    /// <param name="narration">The complaint narration to validate.</param>
    /// <returns>True if the narration is valid; otherwise, false.</returns>
    /// <remarks>
    /// A valid complaint narration must not be null or whitespace and must be between
    /// 10 and 1000 characters in length to ensure it's detailed enough but not excessive.
    /// </remarks>
    public static bool IsValidComplaintNarration(string narration)
    {
        return !string.IsNullOrWhiteSpace(narration) && narration.Length >= 10 && narration.Length <= 1000;
    }
} 