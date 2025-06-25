using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Tests.Utilities;

public class AccountGeneratorTests
{
    [Fact]
    public void GenerateAccountNumber_ReturnsUniqueNumbers()
    {
        // Act
        var accountNumber1 = AccountGenerator.GenerateAccountNumber();
        var accountNumber2 = AccountGenerator.GenerateAccountNumber();
        var accountNumber3 = AccountGenerator.GenerateAccountNumber();

        // Assert
        Assert.NotEqual(accountNumber1, accountNumber2);
        Assert.NotEqual(accountNumber2, accountNumber3);
        Assert.NotEqual(accountNumber1, accountNumber3);
    }

    [Fact]
    public void GenerateAccountNumber_ReturnsSequentialNumbers()
    {
        // Act
        var accountNumber1 = AccountGenerator.GenerateAccountNumber();
        var accountNumber2 = AccountGenerator.GenerateAccountNumber();

        // Assert
        var number1 = int.Parse(accountNumber1);
        var number2 = int.Parse(accountNumber2);
        Assert.Equal(number1 + 1, number2);
    }

    [Fact]
    public void CreateAccount_WithGeneratedNumber_CreatesValidAccount()
    {
        // Arrange
        var accountName = "Test Account";
        var accountType = AccountType.Current;

        // Act
        var account = AccountGenerator.CreateAccount(accountName, accountType);

        // Assert
        Assert.Equal(accountName, account.AccountName);
        Assert.Equal(accountType, account.AccountType);
        Assert.NotNull(account.AccountNumber);
        Assert.False(account.IsFrozen);
        Assert.NotNull(account.TransactionHistory);
        Assert.Empty(account.TransactionHistory);
    }

    [Fact]
    public void CreateAccount_WithSpecifiedNumber_CreatesValidAccount()
    {
        // Arrange
        var accountName = "Test Account";
        var accountNumber = "123456789";
        var accountType = AccountType.Saving;

        // Act
        var account = AccountGenerator.CreateAccount(accountName, accountNumber, accountType);

        // Assert
        Assert.Equal(accountName, account.AccountName);
        Assert.Equal(accountNumber, account.AccountNumber);
        Assert.Equal(accountType, account.AccountType);
        Assert.False(account.IsFrozen);
    }

    [Fact]
    public void CreateDefaultCurrentAccount_CreatesCurrentAccount()
    {
        // Arrange
        var customerName = "John Doe";

        // Act
        var account = AccountGenerator.CreateDefaultCurrentAccount(customerName);

        // Assert
        Assert.Equal($"{customerName}'s Current Account", account.AccountName);
        Assert.Equal(AccountType.Current, account.AccountType);
        Assert.NotNull(account.AccountNumber);
    }

    [Fact]
    public void CreateDefaultSavingAccount_CreatesSavingAccount()
    {
        // Arrange
        var customerName = "Jane Doe";

        // Act
        var account = AccountGenerator.CreateDefaultSavingAccount(customerName);

        // Assert
        Assert.Equal($"{customerName}'s Saving Account", account.AccountName);
        Assert.Equal(AccountType.Saving, account.AccountType);
        Assert.NotNull(account.AccountNumber);
    }

    [Fact]
    public void CreateAccount_GeneratesDifferentAccountNumbers()
    {
        // Act
        var account1 = AccountGenerator.CreateAccount("Account 1", AccountType.Current);
        var account2 = AccountGenerator.CreateAccount("Account 2", AccountType.Saving);

        // Assert
        Assert.NotEqual(account1.AccountNumber, account2.AccountNumber);
    }
} 