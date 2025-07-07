# 🏗️ BankApp Architecture & Design Decisions

## 🎯 Design Philosophy

BankApp follows a **Clean Architecture** approach with emphasis on:
- **Separation of Concerns**: Clear boundaries between layers
- **Dependency Inversion**: High-level modules don't depend on low-level modules
- **Testability**: Easy to unit test all components
- **Maintainability**: Code that's easy to understand and modify
- **Scalability**: Architecture that can grow with requirements

## 🏛️ Architecture Overview

### **Layered Architecture**

```
┌─────────────────────────────────────┐
│           Presentation Layer        │
│         (BankApp.CLI)               │
├─────────────────────────────────────┤
│            Core Layer               │
│         (BankApp.Core)              │
├─────────────────────────────────────┤
│           Data Layer                │
│         (BankApp.Data)              │
├─────────────────────────────────────┤
│        Abstractions Layer           │
│     (BankApp.Abstractions)          │
└─────────────────────────────────────┘
```

### **Layer Responsibilities**

#### **1. Abstractions Layer** (`BankApp.Abstractions`)
- **Purpose**: Define contracts and domain models
- **Components**:
  - Domain entities (Account, Customer, Transaction, etc.)
  - Repository interfaces
  - Enums and constants
  - Utility classes

#### **2. Core Layer** (`BankApp.Core`)
- **Purpose**: Implement business logic and orchestration
- **Components**:
  - Business services (AccountService, CustomerService, etc.)
  - Validation utilities
  - Business rules and constraints
  - Service interfaces

#### **3. Data Layer** (`BankApp.Data`)
- **Purpose**: Handle data persistence and access
- **Components**:
  - Repository implementations
  - JSON storage management
  - Database operations
  - Data mapping and serialization

#### **4. Presentation Layer** (`BankApp.CLI`)
- **Purpose**: User interface and interaction
- **Components**:
  - Console user interface
  - Menu system
  - Input validation
  - User interaction handling

## 🔧 Design Patterns

### **1. Repository Pattern**

**Purpose**: Abstract data access layer from business logic

**Implementation**:
```csharp
public interface IAccountRepository
{
    IEnumerable<Account> GetAll();
    Account GetByAccountNumber(string accountNumber);
    void Add(Account account);
    void Update(Account account);
    void Delete(string accountNumber);
}
```

**Benefits**:
- Decouples business logic from data access
- Enables easy testing with mocks
- Allows switching data sources without code changes
- Provides consistent data access interface

### **2. Service Layer Pattern**

**Purpose**: Encapsulate business logic and orchestrate operations

**Implementation**:
```csharp
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepo;
    private readonly ITransactionRepository _transactionRepo;
    
    public void Deposit(string accountNumber, decimal amount)
    {
        // Business logic implementation
    }
}
```

**Benefits**:
- Centralizes business logic
- Provides transaction management
- Enables cross-cutting concerns
- Improves testability

### **3. Dependency Injection Pattern**

**Purpose**: Manage dependencies and enable loose coupling

**Implementation**:
```csharp
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepo;
    private readonly ITransactionRepository _transactionRepo;
    
    public AccountService(IAccountRepository accountRepo, ITransactionRepository transactionRepo)
    {
        _accountRepo = accountRepo;
        _transactionRepo = transactionRepo;
    }
}
```

**Benefits**:
- Reduces coupling between components
- Enables easy testing with mocks
- Improves maintainability
- Supports configuration changes

### **4. Factory Pattern**

**Purpose**: Create objects without specifying exact classes

**Implementation**:
```csharp
public static class AccountGenerator
{
    public static Account CreateAccount(string accountName, AccountType accountType, string customerId)
    {
        return new Account(accountName, accountType, customerId);
    }
}
```

**Benefits**:
- Encapsulates object creation logic
- Provides consistent object creation
- Enables easy testing
- Supports different creation strategies

## 📊 Data Design Decisions

### **1. JSON Storage Strategy**

**Decision**: Use JSON files for data persistence

**Rationale**:
- **Simplicity**: Easy to understand and debug
- **Portability**: No database server required
- **Human Readable**: Easy to inspect and modify
- **Cross-Platform**: Works on all operating systems

**Implementation**:
```csharp
public class JsonDatabase
{
    private static readonly string DatabasePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "Database");
    
    public static string GetDatabasePath() => DatabasePath;
}
```

### **2. Normalized Data Structure**

**Decision**: Separate transactions from accounts

**Before**:
```json
{
  "accountName": "Account",
  "transactionHistory": [
    { "id": "txn-1", "amount": 100 }
  ]
}
```

**After**:
```json
{
  "accountName": "Account",
  "transactionIds": ["txn-1"]
}
```

**Benefits**:
- **Reduced Duplication**: Transactions stored once
- **Better Performance**: Smaller account objects
- **Easier Queries**: Independent transaction access
- **Scalability**: Better handling of large transaction histories

### **3. Reference-Based Relationships**

**Decision**: Use IDs for entity relationships

**Implementation**:
```csharp
public class Account
{
    public List<string> TransactionIds { get; set; }
    public string CustomerId { get; set; }
}

public class Customer
{
    public List<string> AccountNumbers { get; set; }
}
```

**Benefits**:
- **Loose Coupling**: Entities don't directly reference each other
- **Flexibility**: Easy to modify relationships
- **Performance**: Smaller object sizes
- **Serialization**: Easier JSON serialization

## 🔐 Security Design Decisions

### **1. Password Hashing**

**Decision**: Use salted password hashing

**Implementation**:
```csharp
public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
        {
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            
            return Convert.ToBase64String(hashBytes);
        }
    }
}
```

**Benefits**:
- **Security**: Protects against rainbow table attacks
- **Salt**: Unique salt per password
- **Iterations**: Computational cost prevents brute force
- **Standards**: Follows industry best practices

### **2. Session Management**

**Decision**: Simple session tracking with user IDs

**Implementation**:
```csharp
private static string _session_customer_id;
private static string _session_account_manager_id;
```

**Benefits**:
- **Simplicity**: Easy to implement and understand
- **Security**: Prevents unauthorized access
- **User Experience**: Maintains user context
- **Validation**: Enables access control

## 🧪 Testing Design Decisions

### **1. Mock-Based Testing**

**Decision**: Use Moq for dependency mocking

**Implementation**:
```csharp
[Fact]
public void AccountService_Deposit_ShouldIncreaseBalance()
{
    var mockAccountRepo = new Mock<IAccountRepository>();
    var mockTransactionRepo = new Mock<ITransactionRepository>();
    
    var service = new AccountService(mockAccountRepo.Object, mockTransactionRepo.Object);
    
    // Test implementation
}
```

**Benefits**:
- **Isolation**: Tests focus on specific components
- **Speed**: Fast test execution
- **Reliability**: No external dependencies
- **Control**: Full control over test scenarios

### **2. Test Organization**

**Decision**: Mirror production structure in test projects

**Structure**:
```
tests/
├── BankApp.Core.Tests/
│   ├── Models/
│   ├── Services/
│   └── Utilities/
├── BankApp.Data.Tests/
└── BankApp.CLI.Tests/
```

**Benefits**:
- **Organization**: Easy to find relevant tests
- **Maintenance**: Clear test structure
- **Coverage**: Comprehensive test coverage
- **Scalability**: Easy to add new tests

## 🔄 Error Handling Design

### **1. Exception-Based Error Handling**

**Decision**: Use exceptions for error conditions

**Implementation**:
```csharp
public void Withdraw(decimal amount)
{
    if (IsFrozen)
        throw new InvalidOperationException("Account is frozen.");
    
    if (amount <= 0)
        throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
    
    if (AccountBalance < amount)
        throw new InvalidOperationException("Insufficient funds.");
}
```

**Benefits**:
- **Clarity**: Clear error conditions
- **Propagation**: Errors bubble up appropriately
- **Handling**: Easy to catch and handle specific errors
- **Documentation**: Self-documenting error conditions

### **2. Graceful Error Recovery**

**Decision**: Provide user-friendly error messages

**Implementation**:
```csharp
try
{
    _accountService.Transfer(sourceAccount, destinationAccount, amount);
    Console.WriteLine("✅ Transfer successful!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Transfer failed: {ex.Message}");
}
```

**Benefits**:
- **User Experience**: Clear error feedback
- **Debugging**: Helpful error messages
- **Recovery**: Users can correct errors
- **Logging**: Easy to log errors for debugging

## 📱 User Interface Design

### **1. Menu-Driven Interface**

**Decision**: Hierarchical menu system

**Implementation**:
```csharp
static void ShowMainMenu()
{
    Console.WriteLine("\n🏦 BANKAPP MAIN MENU");
    Console.WriteLine("===================");
    Console.WriteLine("1. Customer Login");
    Console.WriteLine("2. Account Manager Login");
    Console.WriteLine("3. Exit");
}
```

**Benefits**:
- **Usability**: Easy to navigate
- **Consistency**: Predictable interface
- **Accessibility**: Clear options
- **Maintainability**: Easy to modify

### **2. Input Validation**

**Decision**: Real-time input validation

**Implementation**:
```csharp
while (true)
{
    Console.Write("Amount: ₦");
    string input = Console.ReadLine() ?? string.Empty;
    
    if (!decimal.TryParse(input, out amount))
    {
        Console.WriteLine("❌ Invalid amount. Please enter a valid number.");
        continue;
    }
    
    if (amount <= 0)
    {
        Console.WriteLine("❌ Invalid amount. Must be positive.");
        continue;
    }
    
    break;
}
```

**Benefits**:
- **User Experience**: Immediate feedback
- **Data Quality**: Valid data entry
- **Error Prevention**: Prevents invalid operations
- **Guidance**: Helps users enter correct data

## 🔧 Configuration Design

### **1. Static Configuration**

**Decision**: Use static configuration for simplicity

**Implementation**:
```csharp
public static class JsonDatabase
{
    private static readonly string DatabasePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "Database");
}
```

**Benefits**:
- **Simplicity**: Easy to understand and modify
- **Performance**: No configuration loading overhead
- **Reliability**: No configuration file dependencies
- **Portability**: Works across environments

### **2. Environment-Based Paths**

**Decision**: Use application base directory for data storage

**Benefits**:
- **Portability**: Works on any operating system
- **Security**: Data stored with application
- **Simplicity**: No complex path configuration
- **Deployment**: Easy to deploy and run

## 📈 Scalability Considerations

### **1. Modular Architecture**

**Benefits**:
- **Extensibility**: Easy to add new features
- **Maintainability**: Clear component boundaries
- **Testing**: Independent component testing
- **Deployment**: Can deploy components separately

### **2. Interface-Based Design**

**Benefits**:
- **Flexibility**: Easy to swap implementations
- **Testing**: Easy to mock dependencies
- **Evolution**: Can evolve implementations independently
- **Integration**: Easy to integrate with external systems

### **3. JSON Storage Limitations**

**Current Limitations**:
- **Concurrency**: No built-in concurrency control
- **Performance**: File-based storage limitations
- **Scalability**: Limited to single-file storage
- **Transactions**: No ACID guarantees

**Future Considerations**:
- **Database Migration**: Plan for SQL database migration
- **Caching**: Implement caching for performance
- **Sharding**: Consider data sharding strategies
- **Replication**: Plan for data replication

## 🎯 Design Principles Summary

1. **SOLID Principles**: All design decisions follow SOLID principles
2. **Clean Architecture**: Clear separation of concerns
3. **Testability**: All components are easily testable
4. **Maintainability**: Code is easy to understand and modify
5. **Scalability**: Architecture supports future growth
6. **Security**: Security considerations in all design decisions
7. **User Experience**: Focus on usability and accessibility

---

*This design document serves as a reference for architectural decisions and design patterns used in the BankApp system.* 