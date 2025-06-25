using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Tests.Models;

public class ComplaintTests
{
    [Fact]
    public void DefaultConstructor_InitializesPropertiesCorrectly()
    {
        // Act
        var complaint = new Complaint();

        // Assert
        Assert.NotNull(complaint.Id);
        Assert.Equal(string.Empty, complaint.Narration);
        Assert.True(complaint.TimeCreated <= DateTime.Now);
        Assert.Equal(ComplaintStatus.Pending, complaint.Status);
        Assert.Equal(string.Empty, complaint.CustomerId);
        Assert.Null(complaint.AccountId);
        Assert.Equal(string.Empty, complaint.AccountManagerId);
        Assert.Equal(string.Empty, complaint.ResolutionDetails);
        Assert.Null(complaint.TimeResolved);
    }

    [Fact]
    public void ParameterizedConstructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var narration = "My card is not working properly";
        var customerId = "CUST123";

        // Act
        var complaint = new Complaint(narration, customerId);

        // Assert
        Assert.NotNull(complaint.Id);
        Assert.Equal(narration, complaint.Narration);
        Assert.True(complaint.TimeCreated <= DateTime.Now);
        Assert.Equal(ComplaintStatus.Pending, complaint.Status);
        Assert.Equal(customerId, complaint.CustomerId);
        Assert.Null(complaint.AccountId);
        Assert.Equal(string.Empty, complaint.AccountManagerId);
        Assert.Equal(string.Empty, complaint.ResolutionDetails);
        Assert.Null(complaint.TimeResolved);
    }

    [Fact]
    public void CanSetAndGetProperties()
    {
        // Arrange
        var complaint = new Complaint();
        var testId = Guid.NewGuid().ToString();
        var testNarration = "Test complaint description";
        var testCustomerId = "CUST456";
        var testAccountId = "ACC789";
        var testAccountManagerId = "AM001";
        var testResolutionDetails = "Issue resolved by replacing card";

        // Act
        complaint.Id = testId;
        complaint.Narration = testNarration;
        complaint.Status = ComplaintStatus.InProgress;
        complaint.CustomerId = testCustomerId;
        complaint.AccountId = testAccountId;
        complaint.AccountManagerId = testAccountManagerId;
        complaint.ResolutionDetails = testResolutionDetails;
        complaint.TimeResolved = DateTime.Now;

        // Assert
        Assert.Equal(testId, complaint.Id);
        Assert.Equal(testNarration, complaint.Narration);
        Assert.Equal(ComplaintStatus.InProgress, complaint.Status);
        Assert.Equal(testCustomerId, complaint.CustomerId);
        Assert.Equal(testAccountId, complaint.AccountId);
        Assert.Equal(testAccountManagerId, complaint.AccountManagerId);
        Assert.Equal(testResolutionDetails, complaint.ResolutionDetails);
        Assert.NotNull(complaint.TimeResolved);
    }

    [Theory]
    [InlineData(ComplaintStatus.Pending)]
    [InlineData(ComplaintStatus.InProgress)]
    [InlineData(ComplaintStatus.Resolved)]
    [InlineData(ComplaintStatus.Closed)]
    public void CanSetAllComplaintStatuses(ComplaintStatus status)
    {
        // Arrange
        var complaint = new Complaint();

        // Act
        complaint.Status = status;

        // Assert
        Assert.Equal(status, complaint.Status);
    }

    [Fact]
    public void TimeCreated_IsSetToCurrentTime()
    {
        // Arrange
        var beforeCreation = DateTime.Now;

        // Act
        var complaint = new Complaint();
        var afterCreation = DateTime.Now;

        // Assert
        Assert.True(complaint.TimeCreated >= beforeCreation);
        Assert.True(complaint.TimeCreated <= afterCreation);
    }

    [Fact]
    public void Id_IsUniqueForEachComplaint()
    {
        // Act
        var complaint1 = new Complaint();
        var complaint2 = new Complaint();
        var complaint3 = new Complaint();

        // Assert
        Assert.NotEqual(complaint1.Id, complaint2.Id);
        Assert.NotEqual(complaint2.Id, complaint3.Id);
        Assert.NotEqual(complaint1.Id, complaint3.Id);
    }

    [Fact]
    public void CanSetTimeResolved()
    {
        // Arrange
        var complaint = new Complaint();
        var resolvedTime = DateTime.Now.AddHours(2);

        // Act
        complaint.TimeResolved = resolvedTime;

        // Assert
        Assert.Equal(resolvedTime, complaint.TimeResolved);
    }

    [Fact]
    public void CanClearTimeResolved()
    {
        // Arrange
        var complaint = new Complaint();
        complaint.TimeResolved = DateTime.Now;

        // Act
        complaint.TimeResolved = null;

        // Assert
        Assert.Null(complaint.TimeResolved);
    }

    [Fact]
    public void CanSetAccountIdToNull()
    {
        // Arrange
        var complaint = new Complaint();
        complaint.AccountId = "ACC123";

        // Act
        complaint.AccountId = null;

        // Assert
        Assert.Null(complaint.AccountId);
    }

    [Fact]
    public void ParameterizedConstructor_SetsTimeCreatedCorrectly()
    {
        // Arrange
        var beforeCreation = DateTime.Now;
        var narration = "Test complaint";
        var customerId = "CUST123";

        // Act
        var complaint = new Complaint(narration, customerId);
        var afterCreation = DateTime.Now;

        // Assert
        Assert.True(complaint.TimeCreated >= beforeCreation);
        Assert.True(complaint.TimeCreated <= afterCreation);
    }
} 