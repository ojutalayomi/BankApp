using BankApp.Abstractions.Enums;
using BankApp.Abstractions;

namespace BankApp.Core.Tests.Models;

public class AccountTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange & Act
        var account = new Account("Test Account", "123456", AccountType.Saving, "1234567");

        // Assert
        Assert.Equal("Test Account", account.AccountName);
        Assert.Equal("123456", account.AccountNumber);
        Assert.Equal(AccountType.Saving, account.AccountType);
        Assert.NotNull(account.TransactionIds);
        Assert.Empty(account.TransactionIds);
        Assert.False(account.IsFrozen);
        Assert.True(account.DateCreated <= DateTime.Now);
    }

    [Fact]
    public void Deposit_IncreasesBalance()
    {
        // Arrange
        var account = new Account("Test Account", AccountType.Current, "123456")
        {
            AccountBalance = 100m
        };
        var amount = 50m;

        // Act
        var transaction = account.Deposit(amount);

        // Assert
        Assert.Equal(150m, account.AccountBalance);
        Assert.Single(account.TransactionIds);
        Assert.Equal(TransactionType.Deposit, transaction.TransactionType);
        Assert.Equal(TransactionStatus.Completed, transaction.Status);
        Assert.Equal(account.TransactionIds[0], transaction.Id);
    }

    [Fact]
    public void Withdraw_DecreasesBalance()
    {
        // Arrange
        var account = new Account("Test Account", AccountType.Current, "123456")
        {
            AccountBalance = 100m
        };
        var amount = 50m;

        // Act
        var transaction = account.Withdraw(amount);

        // Assert
        Assert.Equal(50m, account.AccountBalance);
        Assert.Single(account.TransactionIds);
        Assert.Equal(TransactionType.Withdraw, transaction.TransactionType);
        Assert.Equal(TransactionStatus.Completed, transaction.Status);
        Assert.Equal(account.TransactionIds[0], transaction.Id);
    }

    [Fact]
    public void Withdraw_ThrowsException_WhenInsufficientFunds()
    {
        // Arrange
        var account = new Account("Test Account", AccountType.Current, "123456")
        {
            AccountBalance = 100m
        };
        var amount = 150m;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => account.Withdraw(amount));
    }

    [Fact]
    public void Transfer_MovesMoney()
    {
        // Arrange
        var sourceAccount = new Account("Source Account", AccountType.Current, "123")
        {
            AccountBalance = 200m
        };
        var destinationAccount = new Account("Destination Account", AccountType.Saving, "456")
        {
            AccountBalance = 100m
        };
        var amount = 75m;

        // Act
        var (sourceTransaction, destinationTransaction) = sourceAccount.Transfer(amount, destinationAccount);

        // Assert
        Assert.Equal(125m, sourceAccount.AccountBalance);
        Assert.Equal(175m, destinationAccount.AccountBalance);
        Assert.Single(sourceAccount.TransactionIds);
        Assert.Single(destinationAccount.TransactionIds);
        Assert.Equal(TransactionType.Transfer, sourceTransaction.TransactionType);
        Assert.Equal(TransactionType.Transfer, destinationTransaction.TransactionType);
        Assert.Equal(sourceAccount.TransactionIds[0], sourceTransaction.Id);
        Assert.Equal(destinationAccount.TransactionIds[0], destinationTransaction.Id);
    }

    [Fact]
    public void FreezeAccount_SetsIsFrozen()
    {
        // Arrange
        var account = new Account("Test Account", AccountType.Current, "123456");

        // Act
        account.FreezeAccount();

        // Assert
        Assert.True(account.IsFrozen);
    }
}