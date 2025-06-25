using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Tests.Models;

public class AccountManagerTests
{
    [Fact]
    public void DefaultConstructor_InitializesPropertiesCorrectly()
    {
        // Act
        var accountManager = new AccountManager();

        // Assert
        Assert.NotNull(accountManager.AccountManagerId);
        Assert.Equal(string.Empty, accountManager.AccountManagerName);
        Assert.Equal(string.Empty, accountManager.ContactInformation);
        Assert.NotNull(accountManager.ManagedCustomers);
        Assert.Empty(accountManager.ManagedCustomers);
        Assert.NotNull(accountManager.ManagedAccounts);
        Assert.Empty(accountManager.ManagedAccounts);
    }

    [Fact]
    public void ParameterizedConstructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        const string managerName = "John Smith";
        const string username = "johnsmith";
        const string password = "password123";
        const string email = "john.smith@bank.com";
        const string phoneNumber = "555-123-4567";
        const string contactInfo = "Main Branch";

        // Act
        var accountManager = new AccountManager(managerName, username, password, email, phoneNumber, contactInfo);

        // Assert
        Assert.NotNull(accountManager.AccountManagerId);
        Assert.Equal(managerName, accountManager.AccountManagerName);
        Assert.Equal(username, accountManager.Username);
        Assert.Equal(email, accountManager.Email);
        Assert.Equal(phoneNumber, accountManager.PhoneNumber);
        Assert.Equal(contactInfo, accountManager.ContactInformation);
        Assert.NotNull(accountManager.ManagedCustomers);
        Assert.Empty(accountManager.ManagedCustomers);
        Assert.NotNull(accountManager.ManagedAccounts);
        Assert.Empty(accountManager.ManagedAccounts);
        Assert.True(accountManager.IsActive);
        Assert.True(accountManager.VerifyPassword(password));
    }

    [Fact]
    public void CanSetAndGetProperties()
    {
        // Arrange
        var accountManager = new AccountManager();
        var testId = Guid.NewGuid().ToString();
        const string testName = "Jane Doe";
        const string testContactInfo = "jane.doe@bank.com";
        var testCustomers = new List<Customer>();
        var testAccounts = new List<Account>();

        // Act
        accountManager.AccountManagerId = testId;
        accountManager.AccountManagerName = testName;
        accountManager.ContactInformation = testContactInfo;
        accountManager.ManagedCustomers = testCustomers;
        accountManager.ManagedAccounts = testAccounts;

        // Assert
        Assert.Equal(testId, accountManager.AccountManagerId);
        Assert.Equal(testName, accountManager.AccountManagerName);
        Assert.Equal(testContactInfo, accountManager.ContactInformation);
        Assert.Equal(testCustomers, accountManager.ManagedCustomers);
        Assert.Equal(testAccounts, accountManager.ManagedAccounts);
    }

    [Fact]
    public void Id_IsUniqueForEachAccountManager()
    {
        // Act
        var manager1 = new AccountManager();
        var manager2 = new AccountManager();
        var manager3 = new AccountManager();

        // Assert
        Assert.NotEqual(manager1.AccountManagerId, manager2.AccountManagerId);
        Assert.NotEqual(manager2.AccountManagerId, manager3.AccountManagerId);
        Assert.NotEqual(manager1.AccountManagerId, manager3.AccountManagerId);
    }

    [Fact]
    public void DeactivateAccount_FreezesAccount()
    {
        // Arrange
        var accountManager = new AccountManager();
        var account = new Account("Test Account", AccountType.Current);

        // Act
        accountManager.DeactivateAccount(account);

        // Assert
        Assert.True(account.IsFrozen);
    }

    [Fact]
    public void RespondToComplaint_UpdatesComplaintCorrectly()
    {
        // Arrange
        var accountManager = new AccountManager();
        var complaint = new Complaint("Test complaint", "CUST123");
        const string resolutionDetails = "Issue has been resolved by replacing the card";

        // Act
        accountManager.RespondToComplaint(complaint, resolutionDetails);

        // Assert
        Assert.Equal(resolutionDetails, complaint.ResolutionDetails);
        Assert.Equal(ComplaintStatus.Resolved, complaint.Status);
        Assert.NotNull(complaint.TimeResolved);
        Assert.True(complaint.TimeResolved <= DateTime.Now);
    }

    [Fact]
    public void RespondToComplaint_SetsTimeResolvedToCurrentTime()
    {
        // Arrange
        var accountManager = new AccountManager();
        var complaint = new Complaint("Test complaint", "CUST123");
        var beforeResponse = DateTime.Now;

        // Act
        accountManager.RespondToComplaint(complaint, "Resolved");
        var afterResponse = DateTime.Now;

        // Assert
        Assert.True(complaint.TimeResolved >= beforeResponse);
        Assert.True(complaint.TimeResolved <= afterResponse);
    }

    [Fact]
    public void CanAddCustomersToManagedCustomers()
    {
        // Arrange
        var accountManager = new AccountManager();
        var customer1 = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "American", MaritalStatus.Single, "123-456-7890", "123 Main St", "John", "john1234");
        var customer2 = new Customer("Jane Doe", Gender.Female, 25, DateTime.Now.AddYears(-25), "Canadian", MaritalStatus.Married, "987-654-3210", "456 Oak Ave", "Jane", "jane1234");

        // Act
        accountManager.ManagedCustomers.Add(customer1);
        accountManager.ManagedCustomers.Add(customer2);

        // Assert
        Assert.Equal(2, accountManager.ManagedCustomers.Count);
        Assert.Contains(customer1, accountManager.ManagedCustomers);
        Assert.Contains(customer2, accountManager.ManagedCustomers);
    }

    [Fact]
    public void CanAddAccountsToManagedAccounts()
    {
        // Arrange
        var accountManager = new AccountManager();
        var account1 = new Account("Current Account", AccountType.Current);
        var account2 = new Account("Saving Account", AccountType.Saving);

        // Act
        accountManager.ManagedAccounts.Add(account1);
        accountManager.ManagedAccounts.Add(account2);

        // Assert
        Assert.Equal(2, accountManager.ManagedAccounts.Count);
        Assert.Contains(account1, accountManager.ManagedAccounts);
        Assert.Contains(account2, accountManager.ManagedAccounts);
    }

    [Fact]
    public void DeactivateAccount_DoesNotAffectOtherAccounts()
    {
        // Arrange
        var accountManager = new AccountManager();
        var account1 = new Account("Account 1", AccountType.Current);
        var account2 = new Account("Account 2", AccountType.Saving);

        // Act
        accountManager.DeactivateAccount(account1);

        // Assert
        Assert.True(account1.IsFrozen);
        Assert.False(account2.IsFrozen);
    }

    [Fact]
    public void RespondToComplaint_DoesNotAffectOtherComplaints()
    {
        // Arrange
        var accountManager = new AccountManager();
        var complaint1 = new Complaint("Complaint 1", "CUST1");
        var complaint2 = new Complaint("Complaint 2", "CUST2");

        // Act
        accountManager.RespondToComplaint(complaint1, "Resolved complaint 1");

        // Assert
        Assert.Equal(ComplaintStatus.Resolved, complaint1.Status);
        Assert.Equal(ComplaintStatus.Pending, complaint2.Status);
        Assert.NotNull(complaint1.TimeResolved);
        Assert.Null(complaint2.TimeResolved);
    }

    [Fact]
    public void VerifyPassword_ReturnsTrueForCorrectPassword()
    {
        // Arrange
        const string password = "securePassword123";
        var accountManager = new AccountManager("Test Manager", "testuser", password, "test@email.com", "555-123-4567", "Test Branch");

        // Act
        var result = accountManager.VerifyPassword(password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ReturnsFalseForIncorrectPassword()
    {
        // Arrange
        const string correctPassword = "securePassword123";
        const string wrongPassword = "wrongPassword456";
        var accountManager = new AccountManager("Test Manager", "testuser", correctPassword, "test@email.com", "555-123-4567", "Test Branch");

        // Act
        var result = accountManager.VerifyPassword(wrongPassword);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Password_IsHashedAndNotStoredAsPlainText()
    {
        // Arrange
        const string password = "securePassword123";
        var accountManager = new AccountManager("Test Manager", "testuser", password, "test@email.com", "555-123-4567", "Test Branch");

        // Assert
        Assert.NotEqual(password, accountManager.Password);
        Assert.True(accountManager.Password.Length > password.Length); // Hashed password should be longer
        Assert.True(accountManager.VerifyPassword(password)); // Should still verify correctly
    }
} 