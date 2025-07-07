using BankApp.Data.Repositories;
using BankApp.Abstractions.Enums;
using BankApp.Abstractions;
using BankApp.Data.JsonStorage;

namespace BankApp.Data.Tests;

public class RepositoryTests
{
    public RepositoryTests()
    {
        // Clear JSON data before each test to ensure clean state
        JsonDatabase.ClearAllData();
    }

    [Fact]
    public void JsonAccountRepository_Add_ShouldAddAccount()
    {
        // Arrange
        var repository = new JsonAccountRepository();
        var account = new Account("Test Account", AccountType.Current, "1234");

        // Act
        repository.Add(account);

        // Assert
        var retrievedAccount = repository.GetByAccountNumber(account.AccountNumber);
        Assert.NotNull(retrievedAccount);
        Assert.Equal(account.AccountNumber, retrievedAccount.AccountNumber);
        Assert.Equal(account.AccountName, retrievedAccount.AccountName);
    }

    [Fact]
    public void JsonAccountRepository_GetAll_ShouldReturnAllAccounts()
    {
        // Arrange
        var repository = new JsonAccountRepository();
        var account1 = new Account("Test Account 1", AccountType.Current, "1234");
        var account2 = new Account("Test Account 2", AccountType.Saving, "12343");

        // Act
        repository.Add(account1);
        repository.Add(account2);
        var allAccounts = repository.GetAll().ToList();

        // Assert
        Assert.Equal(2, allAccounts.Count);
        Assert.Contains(allAccounts, a => a.AccountNumber == account1.AccountNumber);
        Assert.Contains(allAccounts, a => a.AccountNumber == account2.AccountNumber);
    }

    [Fact]
    public void JsonAccountRepository_Update_ShouldUpdateAccount()
    {
        // Arrange
        var repository = new JsonAccountRepository();
        var account = new Account("Original Name", AccountType.Current, "1234");
        repository.Add(account);

        // Act
        account.AccountName = "Updated Name";
        repository.Update(account);

        // Assert
        var updatedAccount = repository.GetByAccountNumber(account.AccountNumber);
        Assert.Equal("Updated Name", updatedAccount?.AccountName);
    }

    [Fact]
    public void JsonAccountRepository_Delete_ShouldRemoveAccount()
    {
        // Arrange
        var repository = new JsonAccountRepository();
        var account = new Account("Test Account", AccountType.Current, "12343");
        repository.Add(account);

        // Act
        repository.Delete(account.AccountNumber);

        // Assert
        var deletedAccount = repository.GetByAccountNumber(account.AccountNumber);
        Assert.Null(deletedAccount);
    }

    [Fact]
    public void JsonAccountRepository_Exists_ShouldReturnTrueForExistingAccount()
    {
        // Arrange
        var repository = new JsonAccountRepository();
        var account = new Account("Test Account", AccountType.Current, "12343");
        repository.Add(account);

        // Act
        var exists = repository.Exists(account.AccountNumber);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void JsonAccountRepository_Exists_ShouldReturnFalseForNonExistingAccount()
    {
        // Arrange
        var repository = new JsonAccountRepository();

        // Act
        var exists = repository.Exists("NONEXISTENT");

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public void JsonCustomerRepository_Add_ShouldAddCustomer()
    {
        // Arrange
        var repository = new JsonCustomerRepository();
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "123456");

        // Act
        repository.Add(customer);

        // Assert
        var retrievedCustomer = repository.GetByName(customer.Name);
        Assert.NotNull(retrievedCustomer);
        Assert.Equal(customer.Name, retrievedCustomer.Name);
        Assert.Equal(customer.Age, retrievedCustomer.Age);
    }

    [Fact]
    public void JsonCustomerRepository_GetAll_ShouldReturnAllCustomers()
    {
        // Arrange
        var repository = new JsonCustomerRepository();
        var customer1 = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "john1234");
        var customer2 = new Customer("Jane Smith", Gender.Female, 25, DateTime.Now.AddYears(-25), "US", MaritalStatus.Married, "555-5678", "456 Oak St", "Jane", "jane1234");

        // Act
        repository.Add(customer1);
        repository.Add(customer2);
        var allCustomers = repository.GetAll().ToList();

        // Assert
        Assert.Equal(2, allCustomers.Count);
        Assert.Contains(allCustomers, c => c.Name == customer1.Name);
        Assert.Contains(allCustomers, c => c.Name == customer2.Name);
    }

    [Fact]
    public void JsonCustomerRepository_Update_ShouldUpdateCustomer()
    {
        // Arrange
        var repository = new JsonCustomerRepository();
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "john1234");
        repository.Add(customer);

        // Act
        customer.PhoneNumber = "555-9999";
        repository.Update(customer);

        // Assert
        var updatedCustomer = repository.GetByName(customer.Name);
        Assert.Equal("555-9999", updatedCustomer?.PhoneNumber);
    }

    [Fact]
    public void JsonCustomerRepository_Delete_ShouldRemoveCustomer()
    {
        // Arrange
        var repository = new JsonCustomerRepository();
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "john1234");
        repository.Add(customer);

        // Act
        repository.Delete(customer.Id);

        // Assert
        var deletedCustomer = repository.GetByName(customer.Name);
        Assert.Null(deletedCustomer);
    }

    [Fact]
    public void JsonTransactionRepository_Add_ShouldAddTransaction()
    {
        // Arrange
        var repository = new JsonTransactionRepository();
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Deposit,
            Amount = 100.00m,
            SourceAccount = "123456",
            DestinationAccount = "",
            DateCreated = DateTime.Now,
            Status = TransactionStatus.Completed,
            Description = "Test deposit",
            BalanceAfterTransaction = 100.00m,
            Initiator = "Test",
            Channel = "ATM",
            ReferenceNumber = Guid.NewGuid().ToString()
        };

        // Act
        repository.Add(transaction);

        // Assert
        var retrievedTransaction = repository.GetById(transaction.Id);
        Assert.NotNull(retrievedTransaction);
        Assert.Equal(transaction.Id, retrievedTransaction.Id);
        Assert.Equal(transaction.Amount, retrievedTransaction.Amount);
    }

    [Fact]
    public void JsonTransactionRepository_GetByAccountNumber_ShouldReturnAccountTransactions()
    {
        // Arrange
        var repository = new JsonTransactionRepository();
        var transaction1 = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Deposit,
            Amount = 100.00m,
            SourceAccount = "123456",
            DestinationAccount = "",
            DateCreated = DateTime.Now,
            Status = TransactionStatus.Completed,
            Description = "Test deposit",
            BalanceAfterTransaction = 100.00m,
            Initiator = "Test",
            Channel = "ATM",
            ReferenceNumber = Guid.NewGuid().ToString()
        };
        var transaction2 = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Withdraw,
            Amount = 50.00m,
            SourceAccount = "123456",
            DestinationAccount = "",
            DateCreated = DateTime.Now,
            Status = TransactionStatus.Completed,
            Description = "Test withdrawal",
            BalanceAfterTransaction = 50.00m,
            Initiator = "Test",
            Channel = "ATM",
            ReferenceNumber = Guid.NewGuid().ToString()
        };

        // Act
        repository.Add(transaction1);
        repository.Add(transaction2);
        var accountTransactions = repository.GetByAccountNumber("123456").ToList();

        // Assert
        Assert.Equal(2, accountTransactions.Count);
        Assert.All(accountTransactions, t => Assert.Equal("123456", t.SourceAccount));
    }
} 