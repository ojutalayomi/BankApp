using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Tests.Models;

public class TransactionTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Act
        var transaction = new Transaction();

        // Assert
        Assert.NotNull(transaction.Id);
        Assert.Equal(TransactionType.Deposit, transaction.TransactionType);
        Assert.Equal(0, transaction.Amount);
        Assert.Equal(string.Empty, transaction.SourceAccount);
        Assert.Equal(string.Empty, transaction.DestinationAccount);
        Assert.Equal(TransactionStatus.Pending, transaction.Status);
        Assert.True(transaction.DateCreated <= DateTime.Now);
        Assert.Equal(string.Empty, transaction.Description);
        Assert.Equal(0, transaction.BalanceAfterTransaction);
        Assert.Equal(string.Empty, transaction.Initiator);
        Assert.Equal(string.Empty, transaction.Channel);
        Assert.NotNull(transaction.ReferenceNumber);
    }

    [Fact]
    public void CanSetAndGetProperties()
    {
        // Arrange
        var transaction = new Transaction();
        var testId = Guid.NewGuid().ToString();
        var testAmount = 100.50m;
        var testSourceAccount = "123456789";
        var testDestinationAccount = "987654321";
        var testDescription = "Test transaction";
        var testInitiator = "John Doe";
        var testChannel = "ATM";
        var testReferenceNumber = "REF123456";

        // Act
        transaction.Id = testId;
        transaction.TransactionType = TransactionType.Transfer;
        transaction.Amount = testAmount;
        transaction.SourceAccount = testSourceAccount;
        transaction.DestinationAccount = testDestinationAccount;
        transaction.Status = TransactionStatus.Completed;
        transaction.Description = testDescription;
        transaction.BalanceAfterTransaction = 500.00m;
        transaction.Initiator = testInitiator;
        transaction.Channel = testChannel;
        transaction.ReferenceNumber = testReferenceNumber;

        // Assert
        Assert.Equal(testId, transaction.Id);
        Assert.Equal(TransactionType.Transfer, transaction.TransactionType);
        Assert.Equal(testAmount, transaction.Amount);
        Assert.Equal(testSourceAccount, transaction.SourceAccount);
        Assert.Equal(testDestinationAccount, transaction.DestinationAccount);
        Assert.Equal(TransactionStatus.Completed, transaction.Status);
        Assert.Equal(testDescription, transaction.Description);
        Assert.Equal(500.00m, transaction.BalanceAfterTransaction);
        Assert.Equal(testInitiator, transaction.Initiator);
        Assert.Equal(testChannel, transaction.Channel);
        Assert.Equal(testReferenceNumber, transaction.ReferenceNumber);
    }

    [Theory]
    [InlineData(TransactionType.Deposit)]
    [InlineData(TransactionType.Withdraw)]
    [InlineData(TransactionType.Transfer)]
    public void CanSetAllTransactionTypes(TransactionType transactionType)
    {
        // Arrange
        var transaction = new Transaction();

        // Act
        transaction.TransactionType = transactionType;

        // Assert
        Assert.Equal(transactionType, transaction.TransactionType);
    }

    [Theory]
    [InlineData(TransactionStatus.Pending)]
    [InlineData(TransactionStatus.Completed)]
    [InlineData(TransactionStatus.Failed)]
    [InlineData(TransactionStatus.Cancelled)]
    public void CanSetAllTransactionStatuses(TransactionStatus status)
    {
        // Arrange
        var transaction = new Transaction();

        // Act
        transaction.Status = status;

        // Assert
        Assert.Equal(status, transaction.Status);
    }

    [Fact]
    public void DateCreated_IsSetToCurrentTime()
    {
        // Arrange
        var beforeCreation = DateTime.Now;

        // Act
        var transaction = new Transaction();
        var afterCreation = DateTime.Now;

        // Assert
        Assert.True(transaction.DateCreated >= beforeCreation);
        Assert.True(transaction.DateCreated <= afterCreation);
    }

    [Fact]
    public void Id_IsUniqueForEachTransaction()
    {
        // Act
        var transaction1 = new Transaction();
        var transaction2 = new Transaction();
        var transaction3 = new Transaction();

        // Assert
        Assert.NotEqual(transaction1.Id, transaction2.Id);
        Assert.NotEqual(transaction2.Id, transaction3.Id);
        Assert.NotEqual(transaction1.Id, transaction3.Id);
    }

    [Fact]
    public void ReferenceNumber_IsUniqueForEachTransaction()
    {
        // Act
        var transaction1 = new Transaction();
        var transaction2 = new Transaction();
        var transaction3 = new Transaction();

        // Assert
        Assert.NotEqual(transaction1.ReferenceNumber, transaction2.ReferenceNumber);
        Assert.NotEqual(transaction2.ReferenceNumber, transaction3.ReferenceNumber);
        Assert.NotEqual(transaction1.ReferenceNumber, transaction3.ReferenceNumber);
    }
} 