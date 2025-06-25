using BankApp.Abstractions.Enums;
using System;
using System.Collections.Generic;
using BankApp.Abstractions;
using Xunit;

namespace BankApp.Core.Tests.Models;

public class AccountTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange & Act
        var account = new Account("Test Account", "123456", AccountType.Saving);

        // Assert
        Assert.Equal("Test Account", account.AccountName);
        Assert.Equal("123456", account.AccountNumber);
        Assert.Equal(AccountType.Saving, account.AccountType);
        Assert.NotNull(account.TransactionHistory);
        Assert.Empty(account.TransactionHistory);
        Assert.False(account.IsFrozen);
        Assert.True(account.DateCreated <= DateTime.Now);
    }

    [Fact]
    public void Deposit_IncreasesBalance()
    {
        // Arrange
        var account = new Account("Test Account", "123456", AccountType.Current);
        account.AccountBalance = 100m;
        var amount = 50m;

        // Act
        account.Deposit(amount);

        // Assert
        Assert.Equal(150m, account.AccountBalance);
        Assert.Single(account.TransactionHistory);
        Assert.Equal(TransactionType.Deposit, account.TransactionHistory[0].TransactionType);
        Assert.Equal(TransactionStatus.Completed, account.TransactionHistory[0].Status);
    }

    [Fact]
    public void Withdraw_DecreasesBalance()
    {
        // Arrange
        var account = new Account("Test Account", "123456", AccountType.Current);
        account.AccountBalance = 100m;
        var amount = 50m;

        // Act
        account.Withdraw(amount);

        // Assert
        Assert.Equal(50m, account.AccountBalance);
        Assert.Single(account.TransactionHistory);
        Assert.Equal(TransactionType.Withdraw, account.TransactionHistory[0].TransactionType);
        Assert.Equal(TransactionStatus.Completed, account.TransactionHistory[0].Status);
    }

    [Fact]
    public void Withdraw_ThrowsException_WhenInsufficientFunds()
    {
        // Arrange
        var account = new Account("Test Account", "123456", AccountType.Current);
        account.AccountBalance = 100m;
        var amount = 150m;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => account.Withdraw(amount));
    }

    [Fact]
    public void Transfer_MovesMoney()
    {
        // Arrange
        var sourceAccount = new Account("Source Account", "123", AccountType.Current);
        sourceAccount.AccountBalance = 200m;
        var destinationAccount = new Account("Destination Account", "456", AccountType.Saving);
        destinationAccount.AccountBalance = 100m;
        var amount = 75m;

        // Act
        sourceAccount.Transfer(amount, destinationAccount);

        // Assert
        Assert.Equal(125m, sourceAccount.AccountBalance);
        Assert.Equal(175m, destinationAccount.AccountBalance);
        Assert.Single(sourceAccount.TransactionHistory);
        Assert.Single(destinationAccount.TransactionHistory);
        Assert.Equal(TransactionType.Transfer, sourceAccount.TransactionHistory[0].TransactionType);
        Assert.Equal(TransactionType.Transfer, destinationAccount.TransactionHistory[0].TransactionType);
    }

    [Fact]
    public void FreezeAccount_SetsIsFrozen()
    {
        // Arrange
        var account = new Account("Test Account", "123456", AccountType.Current);

        // Act
        account.FreezeAccount();

        // Assert
        Assert.True(account.IsFrozen);
    }
}