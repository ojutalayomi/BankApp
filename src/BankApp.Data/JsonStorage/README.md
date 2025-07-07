# JSON Database System

This directory contains the JSON-based database implementation for BankApp.

## 📁 **File Structure**

```
JsonStorage/
├── JsonDatabase.cs          # Core JSON database operations
├── DatabaseManager.cs       # High-level database management
├── SampleData/              # Example JSON files
│   ├── accounts.json
│   ├── customers.json
│   └── transactions.json
└── README.md               # This file
```

## 🗄️ **Database Files**

When you run the application, the following JSON files will be created in the `Data/` directory:

- **`accounts.json`** - Stores all account information
- **`customers.json`** - Stores all customer information  
- **`transactions.json`** - Stores all transaction records

## 🔧 **How to Use**

### **1. Using JSON Repositories**

Replace the in-memory repositories with JSON repositories:

```csharp
// Instead of:
services.AddScoped<IAccountRepository, AccountRepository>();

// Use:
services.AddScoped<IAccountRepository, JsonAccountRepository>();
```

### **2. Database Operations**

```csharp
// Load all accounts
var accounts = JsonDatabase.LoadAccounts();

// Save accounts
JsonDatabase.SaveAccounts(accounts);

// Get database statistics
var stats = DatabaseManager.GetDatabaseStats();
Console.WriteLine($"Total Accounts: {stats.TotalAccounts}");
```

### **3. Backup and Restore**

```csharp
// Create backup
DatabaseManager.BackupDatabase("/Users/username/Backups/");

// Restore from backup
DatabaseManager.RestoreDatabase("/Users/username/Backups/BankApp_Backup_20240115_143022/");
```

## 📊 **JSON Structure Examples**

### **Account Record**
```json
{
  "accountName": "John Doe's Current Account",
  "accountNumber": "1000001",
  "accountType": 0,
  "accountBalance": 1500.00,
  "transactionIds": [],
  "dateCreated": "2024-01-15T10:30:00.000Z",
  "isFrozen": false
}
```

### **Customer Record**
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
  "accountNumbers": [
    "10000123"
  ],
  "complaints": [],
  "transactions": []
}
```

### **Transaction Record**
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

## 🔄 **Enum Values**

### **AccountType**
- `0` = Current
- `1` = Saving
- `2` = Fixed Deposit

### **Gender**
- `0` = Male
- `1` = Female
- `2` = Other

### **MaritalStatus**
- `0` = Single
- `1` = Married
- `2` = Divorced
- `3` = Widowed

### **TransactionType**
- `0` = Withdraw
- `1` = Deposit
- `2` = Transfer

### **TransactionStatus**
- `0` = Pending
- `1` = Completed
- `2` = Failed
- `3` = Cancelled

## ⚡ **Performance Considerations**

- **Read Operations**: Fast for small datasets
- **Write Operations**: Slower due to file I/O
- **Large Datasets**: Consider indexing or pagination
- **Concurrent Access**: Not thread-safe by default

## 🛡️ **Data Safety**

- **Automatic Backup**: Use `DatabaseManager.BackupDatabase()`
- **Error Handling**: Graceful fallback to empty collections
- **File Validation**: JSON format validation on load
- **Data Integrity**: Transaction-based updates

## 🚀 **Next Steps**

1. **Replace In-Memory Repositories**: Use JSON repositories in your DI container
2. **Add Data Validation**: Implement JSON schema validation
3. **Optimize Performance**: Add caching for frequently accessed data
4. **Add Indexing**: Implement search indexes for better performance
5. **Concurrency Control**: Add file locking for multi-user scenarios 