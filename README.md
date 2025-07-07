# 🏦 BankApp - C# Banking Management System

A comprehensive banking management system built with C# .NET 8.0, featuring a clean architecture with JSON-based data storage, user authentication, and complete banking operations.

## 📋 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Technology Stack](#-technology-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Usage Guide](#-usage-guide)
- [Database Structure](#-database-structure)
- [API Reference](#-api-reference)
- [Testing](#-testing)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

### 🏛️ **Account Management**
- Create and manage bank accounts (Current & Savings)
- Account balance tracking and updates
- Account freeze/unfreeze functionality
- Account number generation with sequential numbering
- Account ownership linking to customers

### 👥 **Customer Management**
- Customer registration and profile management
- Customer authentication system
- Multiple account ownership per customer
- Customer complaint tracking
- Personal information management (name, age, nationality, etc.)

### 👨‍💼 **Account Manager System**
- Account manager registration and authentication
- Administrative account creation capabilities
- System-wide account management
- Customer account oversight

### 💰 **Transaction Management**
- Deposit, withdrawal, and transfer operations
- Comprehensive transaction history tracking
- Transaction status management (Pending, Completed, Failed)
- Transaction metadata (initiator, channel, reference numbers)
- Real-time balance updates

### 🗄️ **Data Management**
- JSON-based persistent storage
- Database backup and restore functionality
- Data clearing and reset capabilities
- Database statistics and reporting

### 🔐 **Security Features**
- Password hashing with salt
- Session management for authenticated users
- Input validation and sanitization
- Account access control

## 🏗️ Architecture

The application follows a **Clean Architecture** pattern with clear separation of concerns:

```
BankApp/
├── src/
│   ├── BankApp.Abstractions/     # Domain models and interfaces
│   ├── BankApp.Core/             # Business logic and services
│   ├── BankApp.Data/             # Data access and repositories
│   └── BankApp.CLI/              # User interface and presentation
└── tests/                        # Unit and integration tests
```

### **Architecture Layers**

1. **Abstractions Layer** (`BankApp.Abstractions`)
   - Domain entities (Account, Customer, Transaction, etc.)
   - Repository interfaces
   - Enums and constants

2. **Core Layer** (`BankApp.Core`)
   - Business logic services
   - Validation utilities
   - Service interfaces

3. **Data Layer** (`BankApp.Data`)
   - Repository implementations
   - JSON storage management
   - Database operations

4. **Presentation Layer** (`BankApp.CLI`)
   - Console user interface
   - User interaction handling
   - Menu system

## 🛠️ Technology Stack

- **Framework**: .NET 8.0
- **Language**: C# 12.0
- **Storage**: JSON-based file system
- **Testing**: xUnit with Moq
- **Architecture**: Clean Architecture with Repository Pattern
- **Platform**: Cross-platform (Windows, macOS, Linux)

## 📁 Project Structure

```
BankApp/
├── src/
│   ├── BankApp.Abstractions/
│   │   ├── Account.cs
│   │   ├── Customer.cs
│   │   ├── Transaction.cs
│   │   ├── AccountManager.cs
│   │   ├── Complaint.cs
│   │   ├── Enums/
│   │   │   ├── AccountType.cs
│   │   │   ├── TransactionType.cs
│   │   │   ├── TransactionStatus.cs
│   │   │   └── ...
│   │   └── Interfaces/
│   │       ├── IAccountRepository.cs
│   │       ├── ICustomerRepository.cs
│   │       └── ...
│   ├── BankApp.Core/
│   │   ├── Services/
│   │   │   ├── AccountService.cs
│   │   │   ├── CustomerService.cs
│   │   │   └── TransactionService.cs
│   │   └── Utilities/
│   │       └── Validators.cs
│   ├── BankApp.Data/
│   │   ├── Repositories/
│   │   │   ├── JsonAccountRepository.cs
│   │   │   ├── JsonCustomerRepository.cs
│   │   │   └── ...
│   │   └── JsonStorage/
│   │       ├── JsonDatabase.cs
│   │       ├── DatabaseManager.cs
│   │       └── SampleData/
│   └── BankApp.CLI/
│       └── Program.cs
└── tests/
    ├── BankApp.Core.Tests/
    ├── BankApp.Data.Tests/
    └── BankApp.CLI.Tests/
```

## 🚀 Getting Started

### **Prerequisites**

- .NET 8.0 SDK or later
- Any code editor (Visual Studio, VS Code, Rider, etc.)

### **Installation**

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd BankApp
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project src/BankApp.CLI/BankApp.CLI.csproj
   ```

### **Running Tests**

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/BankApp.Core.Tests/

# Run with coverage (if available)
dotnet test --collect:"XPlat Code Coverage"
```

## 📖 Usage Guide

### **First Time Setup**

1. **Launch the application**
   ```bash
   dotnet run --project src/BankApp.CLI/BankApp.CLI.csproj
   ```

2. **Create an Account Manager**
   - Select "Account Manager" from the main menu
   - Choose "Create New Account Manager"
   - Fill in the required information

3. **Create Customers and Accounts**
   - Use the Account Manager to create customers
   - Create accounts for customers or standalone accounts

### **Customer Operations**

- **Login**: Use customer credentials to access account features
- **View Balance**: Check account balances
- **Deposit Money**: Add funds to accounts
- **Withdraw Money**: Remove funds from accounts
- **Transfer Money**: Transfer between accounts
- **View Transaction History**: See all account transactions

### **Account Manager Operations**

- **Customer Management**: Create, view, update, and delete customers
- **Account Management**: Create, view, and manage all accounts
- **System Administration**: Backup, restore, and clear database
- **Statistics**: View system-wide statistics and reports

## 🗄️ Database Structure

The application uses three main JSON files for data storage:

### **accounts.json**
```json
{
  "accountName": "John Doe's Current Account",
  "accountNumber": "1000001",
  "accountType": 0,
  "accountBalance": 1500.00,
  "transactionIds": ["txn-001", "txn-002"],
  "dateCreated": "2024-01-15T10:30:00.000Z",
  "isFrozen": false
}
```

### **customers.json**
```json
{
  "name": "John Doe",
  "gender": 0,
  "age": 30,
  "dateOfBirth": "1994-05-15T00:00:00.000Z",
  "nationality": "American",
  "maritalStatus": 1,
  "phoneNumber": "555-123-4567",
  "address": "123 Main St, New York, NY 10001",
  "accountNumbers": ["1000001"],
  "complaints": [],
  "transactions": []
}
```

### **transactions.json**
```json
{
  "id": "txn-001",
  "transactionType": 1,
  "amount": 500.00,
  "sourceAccount": "",
  "destinationAccount": "1000001",
  "status": 1,
  "dateCreated": "2024-01-15T10:30:00.000Z",
  "description": "Initial deposit",
  "balanceAfterTransaction": 500.00,
  "initiator": "System",
  "channel": "ATM",
  "referenceNumber": "ref-001"
}
```

## 🔧 API Reference

### **Core Services**

#### **AccountService**
- `Deposit(string accountNumber, decimal amount)`
- `Withdraw(string accountNumber, decimal amount)`
- `Transfer(string sourceAccount, string destinationAccount, decimal amount)`

#### **CustomerService**
- `CreateCustomer(Customer customer)`
- `GetCustomer(string name)`
- `GetAllCustomers()`
- `UpdateCustomer(Customer customer)`
- `DeleteCustomer(string id)`

#### **TransactionService**
- `CreateTransaction(Transaction transaction)`
- `GetTransactionsByAccount(string accountNumber)`
- `GetTransaction(string id)`

### **Repository Interfaces**

#### **IAccountRepository**
- `GetAll()`
- `GetByAccountNumber(string accountNumber)`
- `Add(Account account)`
- `Update(Account account)`
- `Delete(string accountNumber)`

#### **ICustomerRepository**
- `GetAll()`
- `GetById(string id)`
- `GetByName(string name)`
- `Add(Customer customer)`
- `Update(Customer customer)`
- `Delete(string id)`

## 🧪 Testing

The project includes comprehensive test coverage:

### **Test Projects**
- **BankApp.Core.Tests**: Unit tests for business logic
- **BankApp.Data.Tests**: Integration tests for data access
- **BankApp.CLI.Tests**: UI and user interaction tests

### **Test Categories**
- **Model Tests**: Entity validation and behavior
- **Service Tests**: Business logic and service operations
- **Repository Tests**: Data access and persistence
- **Utility Tests**: Helper functions and validators

### **Running Tests**
```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "FullyQualifiedName~AccountTests"
```

## 🤝 Contributing

### **Development Setup**

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes**
4. **Add tests for new functionality**
5. **Run tests to ensure everything works**
   ```bash
   dotnet test
   ```
6. **Commit your changes**
   ```bash
   git commit -m "Add your feature description"
   ```
7. **Push to your branch**
   ```bash
   git push origin feature/your-feature-name
   ```
8. **Create a Pull Request**

### **Coding Standards**

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Write unit tests for new functionality
- Ensure all tests pass before submitting

### **Architecture Guidelines**

- Maintain separation of concerns
- Use dependency injection
- Follow the repository pattern
- Keep business logic in the Core layer
- Use interfaces for testability

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

If you encounter any issues or have questions:

1. **Check the documentation** in this README
2. **Review existing issues** in the repository
3. **Create a new issue** with detailed information
4. **Contact the development team**

## 🔄 Version History

### **v1.0.0** (Current)
- Initial release with core banking functionality
- Customer and account management
- Transaction processing
- JSON-based data storage
- Console-based user interface
- Comprehensive test coverage

---

**BankApp** - Empowering banking operations with modern C# development practices. 