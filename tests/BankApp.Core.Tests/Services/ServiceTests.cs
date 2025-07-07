using Xunit;
using BankApp.Core.Services;
using BankApp.Data.Repositories;
using BankApp.Abstractions.Enums;
using Moq;
using BankApp.Abstractions;

namespace BankApp.Core.Tests.Services;

public class ServiceTests
{
    [Fact]
    public void AccountService_Deposit_ShouldIncreaseBalance()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var account = new Account("Test Account", AccountType.Current, "1234");
        mockAccountRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(account);
        mockAccountRepo.Setup(r => r.Update(It.IsAny<Account>())).Callback<Account>(a => account = a);
        mockTransactionRepo.Setup(r => r.Add(It.IsAny<Transaction>()));
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act
        service.Deposit(account.AccountNumber, 100.00m);

        // Assert
        Assert.Equal(100.00m, account.AccountBalance);
        Assert.Single(account.TransactionIds);
        mockTransactionRepo.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public void AccountService_Deposit_ShouldThrowExceptionForFrozenAccount()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var account = new Account("Test Account", AccountType.Current, "1234");
        account.FreezeAccount();
        mockAccountRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(account);
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            service.Deposit(account.AccountNumber, 100.00m));
        Assert.Equal("Account is frozen.", exception.Message);
    }

    [Fact]
    public void AccountService_Deposit_ShouldThrowExceptionForNegativeAmount()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var account = new Account("Test Account", AccountType.Current, "1234");
        mockAccountRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(account);
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            service.Deposit(account.AccountNumber, -50.00m));
        Assert.Contains("Deposit amount must be positive.", exception.Message);
    }

    [Fact]
    public void AccountService_Withdraw_ShouldDecreaseBalance()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var account = new Account("Test Account", AccountType.Current, "1234");
        account.Deposit(200.00m); // Add initial balance
        mockAccountRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(account);
        mockAccountRepo.Setup(r => r.Update(It.IsAny<Account>())).Callback<Account>(a => account = a);
        mockTransactionRepo.Setup(r => r.Add(It.IsAny<Transaction>()));
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act
        service.Withdraw(account.AccountNumber, 50.00m);

        // Assert
        Assert.Equal(150.00m, account.AccountBalance);
        Assert.Equal(2, account.TransactionIds.Count);
        mockTransactionRepo.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public void AccountService_Withdraw_ShouldThrowExceptionForInsufficientFunds()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var account = new Account("Test Account", AccountType.Current, "1234");
        account.Deposit(100.00m);
        mockAccountRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(account);
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            service.Withdraw(account.AccountNumber, 150.00m));
        Assert.Equal("Insufficient funds.", exception.Message);
    }

    [Fact]
    public void AccountService_Withdraw_ShouldThrowExceptionForFrozenAccount()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var account = new Account("Test Account", AccountType.Current, "1234");
        account.Deposit(100.00m);
        account.FreezeAccount();
        mockAccountRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(account);
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            service.Withdraw(account.AccountNumber, 50.00m));
        Assert.Equal("Account is frozen.", exception.Message);
    }

    [Fact]
    public void AccountService_Transfer_ShouldTransferBetweenAccounts()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var sourceAccount = new Account("Source Account", AccountType.Current, "1234");
        var destinationAccount = new Account("Destination Account", AccountType.Saving, "12343");
        sourceAccount.Deposit(200.00m);
        
        mockAccountRepo.Setup(r => r.GetByAccountNumber(sourceAccount.AccountNumber)).Returns(sourceAccount);
        mockAccountRepo.Setup(r => r.GetByAccountNumber(destinationAccount.AccountNumber)).Returns(destinationAccount);
        mockAccountRepo.Setup(r => r.Update(It.IsAny<Account>())).Callback<Account>(a => 
        {
            if (a.AccountNumber == sourceAccount.AccountNumber) sourceAccount = a;
            if (a.AccountNumber == destinationAccount.AccountNumber) destinationAccount = a;
        });
        mockTransactionRepo.Setup(r => r.Add(It.IsAny<Transaction>()));
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act
        service.Transfer(sourceAccount.AccountNumber, destinationAccount.AccountNumber, 75.00m);

        // Assert
        Assert.Equal(125.00m, sourceAccount.AccountBalance);
        Assert.Equal(75.00m, destinationAccount.AccountBalance);
        Assert.Equal(2, sourceAccount.TransactionIds.Count);
        Assert.Single(destinationAccount.TransactionIds);
        mockTransactionRepo.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Exactly(2));
    }

    [Fact]
    public void AccountService_Transfer_ShouldThrowExceptionForInsufficientFunds()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var sourceAccount = new Account("Source Account", AccountType.Current, "1234");
        var destinationAccount = new Account("Destination Account", AccountType.Saving, "12343");
        sourceAccount.Deposit(50.00m);
        
        mockAccountRepo.Setup(r => r.GetByAccountNumber(sourceAccount.AccountNumber)).Returns(sourceAccount);
        mockAccountRepo.Setup(r => r.GetByAccountNumber(destinationAccount.AccountNumber)).Returns(destinationAccount);
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            service.Transfer(sourceAccount.AccountNumber, destinationAccount.AccountNumber, 100.00m));
        Assert.Equal("Insufficient funds.", exception.Message);
    }

    [Fact]
    public void AccountService_Transfer_ShouldThrowExceptionForFrozenSourceAccount()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var sourceAccount = new Account("Source Account", AccountType.Current, "1234");
        var destinationAccount = new Account("Destination Account", AccountType.Saving, "12343");
        sourceAccount.Deposit(200.00m);
        sourceAccount.FreezeAccount();
        
        mockAccountRepo.Setup(r => r.GetByAccountNumber(sourceAccount.AccountNumber)).Returns(sourceAccount);
        mockAccountRepo.Setup(r => r.GetByAccountNumber(destinationAccount.AccountNumber)).Returns(destinationAccount);
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            service.Transfer(sourceAccount.AccountNumber, destinationAccount.AccountNumber, 50.00m));
        Assert.Equal("Source account is frozen.", exception.Message);
    }

    [Fact]
    public void AccountService_Transfer_ShouldThrowExceptionForFrozenDestinationAccount()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var sourceAccount = new Account("Source Account", AccountType.Current, "1234");
        var destinationAccount = new Account("Destination Account", AccountType.Saving, "12343");
        sourceAccount.Deposit(200.00m);
        destinationAccount.FreezeAccount();
        
        mockAccountRepo.Setup(r => r.GetByAccountNumber(sourceAccount.AccountNumber)).Returns(sourceAccount);
        mockAccountRepo.Setup(r => r.GetByAccountNumber(destinationAccount.AccountNumber)).Returns(destinationAccount);
        
        var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => 
            service.Transfer(sourceAccount.AccountNumber, destinationAccount.AccountNumber, 50.00m));
        Assert.Equal("Destination account is frozen.", exception.Message);
    }

    [Fact]
    public void CustomerService_CreateCustomer_ShouldCreateCustomerWithAccount()
    {
        // Arrange
        var mockRepo = new Mock<ICustomerRepository>();
        var mockAccountRepo = new Mock<IAccountRepository>();
        var service = new CustomerService(mockRepo.Object, mockAccountRepo.Object);
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "john1234");

        // Act
        service.CreateCustomer(customer, AccountType.Current);

        // Assert
        mockRepo.Verify(r => r.Add(customer), Times.Once);
        mockAccountRepo.Verify(r => r.Add(It.IsAny<Account>()), Times.Once);
        Assert.Single(customer.AccountNumbers);
    }

    [Fact]
    public void CustomerService_GetCustomer_ShouldReturnCustomer()
    {
        // Arrange
        var mockRepo = new Mock<ICustomerRepository>();
        var mockAccountRepo = new Mock<IAccountRepository>();
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "john1234");
        mockRepo.Setup(r => r.GetCustomer("John Doe")).Returns(customer);
        
        var service = new CustomerService(mockRepo.Object, mockAccountRepo.Object);

        // Act
        var result = service.GetCustomer("John Doe");

        // Assert
        Assert.Equal(customer, result);
    }

    [Fact]
    public void CustomerService_GetAllCustomers_ShouldReturnAllCustomers()
    {
        // Arrange
        var mockRepo = new Mock<ICustomerRepository>();
        var mockAccountRepo = new Mock<IAccountRepository>();
        var customers = new List<Customer>
        {
            new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "john1234"),
            new Customer("Jane Smith", Gender.Female, 25, DateTime.Now.AddYears(-25), "US", MaritalStatus.Married, "555-5678", "456 Oak St", "Jane", "jane1234")
        };
        mockRepo.Setup(r => r.GetAll()).Returns(customers);
        
        var service = new CustomerService(mockRepo.Object, mockAccountRepo.Object);

        // Act
        var result = service.GetAllCustomers().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "John Doe");
        Assert.Contains(result, c => c.Name == "Jane Smith");
    }

    [Fact]
    public void CustomerService_UpdateCustomer_ShouldUpdateCustomer()
    {
        // Arrange
        var mockRepo = new Mock<ICustomerRepository>();
        var mockAccountRepo = new Mock<IAccountRepository>();
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "john1234");
        
        var service = new CustomerService(mockRepo.Object, mockAccountRepo.Object);

        // Act
        service.UpdateCustomer(customer);

        // Assert
        mockRepo.Verify(r => r.Update(customer), Times.Once);
    }

    [Fact]
    public void CustomerService_DeleteCustomer_ShouldDeleteCustomer()
    {
        // Arrange
        var mockRepo = new Mock<ICustomerRepository>();
        var mockAccountRepo = new Mock<IAccountRepository>();
        var customer = new Customer("John Doe", Gender.Male, 30, DateTime.Now.AddYears(-30), "US", MaritalStatus.Single, "555-1234", "123 Main St", "John", "john1234");
        mockRepo.Setup(r => r.GetCustomer("John Doe")).Returns(customer);
        
        var service = new CustomerService(mockRepo.Object, mockAccountRepo.Object);

        // Act
        service.DeleteCustomer("John Doe");

        // Assert
        mockRepo.Verify(r => r.Delete("John Doe"), Times.Once);
    }

    [Fact]
    public void TransactionService_CreateTransaction_ShouldCreateTransaction()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var account = new Account("Test Account", AccountType.Current, "1234");
        mockAccountRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(account);
        
        var service = new TransactionService(mockAccountRepo.Object, mockTransactionRepo.Object);
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Deposit,
            Amount = 100.00m,
            SourceAccount = account.AccountNumber,
            DestinationAccount = "",
            DateCreated = DateTime.Now,
            Status = TransactionStatus.Completed,
            Description = "Test transaction",
            BalanceAfterTransaction = 100.00m,
            Initiator = "Test",
            Channel = "ATM",
            ReferenceNumber = Guid.NewGuid().ToString()
        };

        // Act
        service.CreateTransaction(transaction);

        // Assert
        mockTransactionRepo.Verify(r => r.Add(transaction), Times.Once);
    }

    [Fact]
    public void TransactionService_GetTransactionHistory_ShouldReturnAccountTransactions()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var account = new Account("Test Account", AccountType.Current, "1234");
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                TransactionType = TransactionType.Deposit,
                Amount = 100.00m,
                SourceAccount = account.AccountNumber,
                DestinationAccount = "",
                DateCreated = DateTime.Now,
                Status = TransactionStatus.Completed,
                Description = "Test deposit",
                BalanceAfterTransaction = 100.00m,
                Initiator = "Test",
                Channel = "ATM",
                ReferenceNumber = Guid.NewGuid().ToString()
            }
        };
        
        mockAccountRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(account);
        mockTransactionRepo.Setup(r => r.GetByAccountNumber(account.AccountNumber)).Returns(transactions);
        
        var service = new TransactionService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act
        var result = service.GetTransactionHistory(account.AccountNumber).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(TransactionType.Deposit, result[0].TransactionType);
    }

    [Fact]
    public void TransactionService_GetTransaction_ShouldReturnTransaction()
    {
        // Arrange
        var mockAccountRepo = new Mock<IAccountRepository>();
        var mockTransactionRepo = new Mock<ITransactionRepository>();
        var transaction = new Transaction
        {
            Id = "test-id",
            TransactionType = TransactionType.Deposit,
            Amount = 100.00m,
            SourceAccount = "123456",
            DestinationAccount = "",
            DateCreated = DateTime.Now,
            Status = TransactionStatus.Completed,
            Description = "Test transaction",
            BalanceAfterTransaction = 100.00m,
            Initiator = "Test",
            Channel = "ATM",
            ReferenceNumber = Guid.NewGuid().ToString()
        };
        
        mockTransactionRepo.Setup(r => r.GetById("test-id")).Returns(transaction);
        
        var service = new TransactionService(mockAccountRepo.Object, mockTransactionRepo.Object);

        // Act
        var result = service.GetTransaction("test-id");

        // Assert
        Assert.Equal(transaction, result);
    }
} 