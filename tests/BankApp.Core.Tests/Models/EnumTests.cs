using BankApp.Abstractions.Enums;

namespace BankApp.Core.Tests.Models;

public class EnumTests
{
    [Fact]
    public void AccountType_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)AccountType.Current);
        Assert.Equal(1, (int)AccountType.Saving);
        Assert.Equal(2, (int)AccountType.FixedDeposit);
        Assert.Equal("Saving", nameof(AccountType.Saving));
        Assert.Equal("Current", nameof(AccountType.Current));
        Assert.Equal("FixedDeposit", nameof(AccountType.FixedDeposit));
    }

    [Fact]
    public void TransactionType_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)TransactionType.Withdraw);
        Assert.Equal(1, (int)TransactionType.Deposit);
        Assert.Equal(2, (int)TransactionType.Transfer);
        Assert.Equal("Withdraw", nameof(TransactionType.Withdraw));
        Assert.Equal("Deposit", nameof(TransactionType.Deposit));
        Assert.Equal("Transfer", nameof(TransactionType.Transfer));
    }

    [Fact]
    public void TransactionStatus_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)TransactionStatus.Pending);
        Assert.Equal(1, (int)TransactionStatus.Completed);
        Assert.Equal(2, (int)TransactionStatus.Failed);
        Assert.Equal(3, (int)TransactionStatus.Cancelled);
    }

    [Fact]
    public void ComplaintStatus_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)ComplaintStatus.Pending);
        Assert.Equal(1, (int)ComplaintStatus.InProgress);
        Assert.Equal(2, (int)ComplaintStatus.Resolved);
        Assert.Equal(3, (int)ComplaintStatus.Closed);
    }

    [Fact]
    public void Gender_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)Gender.Male);
        Assert.Equal(1, (int)Gender.Female);
        Assert.Equal(2, (int)Gender.Other);
    }

    [Fact]
    public void MaritalStatus_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)MaritalStatus.Single);
        Assert.Equal(1, (int)MaritalStatus.Married);
        Assert.Equal(2, (int)MaritalStatus.Divorced);
        Assert.Equal(3, (int)MaritalStatus.Widowed);
    }
} 