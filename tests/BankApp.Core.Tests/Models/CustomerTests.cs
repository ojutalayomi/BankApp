using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Tests.Models;

public class CustomerTests
{
    [Fact]
    public void CanCreateCustomer()
    {
        // Arrange & Act
        var customer = new Customer(
            "John Doe", 
            Gender.Male, 
            30, 
            new DateTime(1993, 5, 15), 
            "American", 
            MaritalStatus.Single, 
            "123-456-7890", 
            "123 Main St, City, State 12345",
            "John", 
            "john1234"
        );

        // Assert
        Assert.Equal("John Doe", customer.Name);
        Assert.Equal(30, customer.Age);
        Assert.Equal("123 Main St, City, State 12345", customer.Address);
        Assert.NotNull(customer.AccountNumbers);
        Assert.Empty(customer.AccountNumbers);
        Assert.NotNull(customer.Complaints);
        Assert.NotNull(customer.Transactions);
    }

    [Fact]
    public void CanFileComplaint()
    {
        // Arrange
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "American", MaritalStatus.Single, "123-456-7890", "123 Main St", "john", "password123");

        // Act
        customer.FileComplaint("I lost my card");

        // Assert
        Assert.Single(customer.Complaints);
        Assert.Equal("I lost my card", customer.Complaints[0].Narration);
    }

    [Fact]
    public void VerifyPassword_ReturnsTrueForCorrectPassword()
    {
        // Arrange
        const string password = "securePassword123";
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "American", MaritalStatus.Single, "123-456-7890", "123 Main St", "john", password);

        // Act
        var result = customer.VerifyPassword(password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ReturnsFalseForIncorrectPassword()
    {
        // Arrange
        const string correctPassword = "securePassword123";
        const string wrongPassword = "wrongPassword456";
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "American", MaritalStatus.Single, "123-456-7890", "123 Main St", "john", correctPassword);

        // Act
        var result = customer.VerifyPassword(wrongPassword);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Password_IsHashedAndNotStoredAsPlainText()
    {
        // Arrange
        const string password = "securePassword123";
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "American", MaritalStatus.Single, "123-456-7890", "123 Main St", "john", password);

        // Assert
        Assert.NotEqual(password, customer.Password);
        Assert.True(customer.Password.Length > password.Length); // Hashed password should be longer
        Assert.True(customer.VerifyPassword(password)); // Should still verify correctly
    }
}