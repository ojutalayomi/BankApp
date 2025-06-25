using BankApp.Abstractions.Enums;
using BankApp.Core.Utilities;

namespace BankApp.Core.Tests.Utilities;

public class ValidatorsTests
{
    [Theory]
    [InlineData("John Doe", true)]
    [InlineData("J", false)] // Too short
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace only
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false)] // Too long (101 chars)
    [InlineData("Jane Smith", true)]
    public void IsValidCustomerName_ValidatesCorrectly(string name, bool expected)
    {
        // Act
        var result = Validators.IsValidCustomerName(name);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(18, true)]
    [InlineData(25, true)]
    [InlineData(120, true)]
    [InlineData(17, false)] // Too young
    [InlineData(121, false)] // Too old
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValidAge_ValidatesCorrectly(int age, bool expected)
    {
        // Act
        var result = Validators.IsValidAge(age);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1234567890", true)]
    [InlineData("123-456-7890", true)]
    [InlineData("(123) 456-7890", true)]
    [InlineData("123 456 7890", true)]
    [InlineData("123456789", false)] // Too short
    [InlineData("1234567890123456", false)] // Too long
    [InlineData("", false)]
    [InlineData("abc-def-ghij", false)] // Contains letters
    public void IsValidPhoneNumber_ValidatesCorrectly(string phoneNumber, bool expected)
    {
        // Act
        var result = Validators.IsValidPhoneNumber(phoneNumber);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123456", true)]
    [InlineData("123456789", true)]
    [InlineData("12345678901234567890", true)]
    [InlineData("12345", false)] // Too short
    [InlineData("123456789012345678901", false)] // Too long
    [InlineData("", false)]
    [InlineData("abc123", false)] // Contains letters
    public void IsValidAccountNumber_ValidatesCorrectly(string accountNumber, bool expected)
    {
        // Act
        var result = Validators.IsValidAccountNumber(accountNumber);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1.0, true)]
    [InlineData(100.50, true)]
    [InlineData(1000000, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(1000001, false)] // Exceeds maximum
    public void IsValidTransactionAmount_ValidatesCorrectly(decimal amount, bool expected)
    {
        // Act
        var result = Validators.IsValidTransactionAmount(amount);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user.name@domain.co.uk", true)]
    [InlineData("invalid-email", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("@domain.com", false)]
    [InlineData("user@", false)]
    public void IsValidEmail_ValidatesCorrectly(string email, bool expected)
    {
        // Act
        var result = Validators.IsValidEmail(email);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123 Main Street, City, State 12345", true)]
    [InlineData("Short St", false)] // Too short
    [InlineData("", false)]
    [InlineData(
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false)] // Too long
    public void IsValidAddress_ValidatesCorrectly(string address, bool expected)
    {
        // Act
        var result = Validators.IsValidAddress(address);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("American", true)]
    [InlineData("A", false)] // Too short
    [InlineData("", false)]
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false)] // Too long
    public void IsValidNationality_ValidatesCorrectly(string nationality, bool expected)
    {
        // Act
        var result = Validators.IsValidNationality(nationality);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Gender.Male, true)]
    [InlineData(Gender.Female, true)]
    [InlineData(Gender.Other, true)]
    public void IsValidGender_ValidatesCorrectly(Gender gender, bool expected)
    {
        // Act
        var result = Validators.IsValidGender(gender);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(MaritalStatus.Single, true)]
    [InlineData(MaritalStatus.Married, true)]
    [InlineData(MaritalStatus.Divorced, true)]
    [InlineData(MaritalStatus.Widowed, true)]
    public void IsValidMaritalStatus_ValidatesCorrectly(MaritalStatus maritalStatus, bool expected)
    {
        // Act
        var result = Validators.IsValidMaritalStatus(maritalStatus);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(AccountType.Current, true)]
    [InlineData(AccountType.Saving, true)]
    public void IsValidAccountType_ValidatesCorrectly(AccountType accountType, bool expected)
    {
        // Act
        var result = Validators.IsValidAccountType(accountType);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(TransactionType.Deposit, true)]
    [InlineData(TransactionType.Withdraw, true)]
    [InlineData(TransactionType.Transfer, true)]
    public void IsValidTransactionType_ValidatesCorrectly(TransactionType transactionType, bool expected)
    {
        // Act
        var result = Validators.IsValidTransactionType(transactionType);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("This is a valid complaint with sufficient detail", true)]
    [InlineData("Short", false)] // Too short
    [InlineData("", false)]
    [InlineData(
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false)] // Too long
    public void IsValidComplaintNarration_ValidatesCorrectly(string narration, bool expected)
    {
        // Act
        var result = Validators.IsValidComplaintNarration(narration);

        // Assert
        Assert.Equal(expected, result);
    }
} 