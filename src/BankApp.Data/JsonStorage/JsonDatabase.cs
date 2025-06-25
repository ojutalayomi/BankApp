using System.Text.Json;
using BankApp.Abstractions;

namespace BankApp.Data.JsonStorage;

/// <summary>
/// Provides static methods for JSON-based database operations.
/// </summary>
/// <remarks>
/// This class manages the JSON file storage system for the BankApp. It handles
/// serialization and deserialization of all entity types (accounts, customers,
/// transactions, account managers) to and from JSON files. The class automatically
/// initializes the database structure on first access.
/// </remarks>
public static class JsonDatabase
{
    /// <summary>
    /// The directory where all JSON database files are stored.
    /// </summary>
    /// <remarks>
    /// This path is relative to the application's base directory and contains
    /// all the JSON files for the database.
    /// </remarks>
    private static readonly string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
    
    /// <summary>
    /// The file path for storing account data.
    /// </summary>
    private static readonly string AccountsFile = Path.Combine(DataDirectory, "accounts.json");
    
    /// <summary>
    /// The file path for storing customer data.
    /// </summary>
    private static readonly string CustomersFile = Path.Combine(DataDirectory, "customers.json");
    
    /// <summary>
    /// The file path for storing transaction data.
    /// </summary>
    private static readonly string TransactionsFile = Path.Combine(DataDirectory, "transactions.json");
    
    /// <summary>
    /// The file path for storing account manager data.
    /// </summary>
    private static readonly string AccountManagersFile = Path.Combine(DataDirectory, "accountManagers.json");
    
    /// <summary>
    /// JSON serialization options for consistent formatting and naming.
    /// </summary>
    /// <remarks>
    /// These options ensure that JSON is written with proper indentation and
    /// uses camelCase naming convention for properties.
    /// </remarks>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Static constructor that initializes the database structure.
    /// </summary>
    /// <remarks>
    /// This constructor ensures the data directory exists and initializes
    /// all JSON files with empty collections if they don't exist.
    /// </remarks>
    static JsonDatabase()
    {
        // Ensure data directory exists
        if (!Directory.Exists(DataDirectory))
        {
            Directory.CreateDirectory(DataDirectory);
        }
        
        // Initialize database files if they don't exist
        InitializeDatabase();
    }

    /// <summary>
    /// Initializes the database by creating empty JSON files if they don't exist.
    /// </summary>
    /// <remarks>
    /// This method creates the initial JSON files with empty collections for
    /// accounts, customers, transactions, and account managers.
    /// </remarks>
    private static void InitializeDatabase()
    {
        // Initialize accounts.json
        if (!File.Exists(AccountsFile))
        {
            File.WriteAllText(AccountsFile, JsonSerializer.Serialize(new List<Account>(), JsonOptions));
        }

        // Initialize customers.json
        if (!File.Exists(CustomersFile))
        {
            File.WriteAllText(CustomersFile, JsonSerializer.Serialize(new List<Customer>(), JsonOptions));
        }

        // Initialize transactions.json
        if (!File.Exists(TransactionsFile))
        {
            File.WriteAllText(TransactionsFile, JsonSerializer.Serialize(new List<Transaction>(), JsonOptions));
        }

        // Initialize accountManagers.json
        if (!File.Exists(AccountManagersFile))
        {
            File.WriteAllText(AccountManagersFile, JsonSerializer.Serialize(new List<AccountManager>(), JsonOptions));
        }
    }

    /// <summary>
    /// Loads all accounts from the JSON file.
    /// </summary>
    /// <returns>A list of all accounts stored in the database.</returns>
    /// <remarks>
    /// This method deserializes the accounts.json file and returns the list of accounts.
    /// If the file is corrupted or doesn't exist, it returns an empty list.
    /// </remarks>
    public static List<Account> LoadAccounts()
    {
        try
        {
            var json = File.ReadAllText(AccountsFile);
            return JsonSerializer.Deserialize<List<Account>>(json, JsonOptions) ?? new List<Account>();
        }
        catch (Exception)
        {
            return new List<Account>();
        }
    }

    /// <summary>
    /// Saves all accounts to the JSON file.
    /// </summary>
    /// <param name="accounts">The list of accounts to save.</param>
    /// <remarks>
    /// This method serializes the list of accounts to JSON format and writes
    /// it to the accounts.json file.
    /// </remarks>
    public static void SaveAccounts(List<Account> accounts)
    {
        var json = JsonSerializer.Serialize(accounts, JsonOptions);
        File.WriteAllText(AccountsFile, json);
    }

    /// <summary>
    /// Loads all customers from the JSON file.
    /// </summary>
    /// <returns>A list of all customers stored in the database.</returns>
    /// <remarks>
    /// This method deserializes the customers.json file and returns the list of customers.
    /// If the file is corrupted or doesn't exist, it returns an empty list.
    /// </remarks>
    public static List<Customer> LoadCustomers()
    {
        try
        {
            var json = File.ReadAllText(CustomersFile);
            return JsonSerializer.Deserialize<List<Customer>>(json, JsonOptions) ?? new List<Customer>();
        }
        catch (Exception)
        {
            return new List<Customer>();
        }
    }

    /// <summary>
    /// Saves all customers to the JSON file.
    /// </summary>
    /// <param name="customers">The list of customers to save.</param>
    /// <remarks>
    /// This method serializes the list of customers to JSON format and writes
    /// it to the customers.json file.
    /// </remarks>
    public static void SaveCustomers(List<Customer> customers)
    {
        var json = JsonSerializer.Serialize(customers, JsonOptions);
        File.WriteAllText(CustomersFile, json);
    }

    /// <summary>
    /// Loads all transactions from the JSON file.
    /// </summary>
    /// <returns>A list of all transactions stored in the database.</returns>
    /// <remarks>
    /// This method deserializes the transactions.json file and returns the list of transactions.
    /// If the file is corrupted or doesn't exist, it returns an empty list.
    /// </remarks>
    public static List<Transaction> LoadTransactions()
    {
        try
        {
            var json = File.ReadAllText(TransactionsFile);
            return JsonSerializer.Deserialize<List<Transaction>>(json, JsonOptions) ?? new List<Transaction>();
        }
        catch (Exception)
        {
            return new List<Transaction>();
        }
    }

    /// <summary>
    /// Saves all transactions to the JSON file.
    /// </summary>
    /// <param name="transactions">The list of transactions to save.</param>
    /// <remarks>
    /// This method serializes the list of transactions to JSON format and writes
    /// it to the transactions.json file.
    /// </remarks>
    public static void SaveTransactions(List<Transaction> transactions)
    {
        var json = JsonSerializer.Serialize(transactions, JsonOptions);
        File.WriteAllText(TransactionsFile, json);
    }

    /// <summary>
    /// Loads all account managers from the JSON file.
    /// </summary>
    /// <returns>A list of all account managers stored in the database.</returns>
    /// <remarks>
    /// This method deserializes the accountManagers.json file and returns the list of account managers.
    /// If the file is corrupted or doesn't exist, it returns an empty list.
    /// </remarks>
    public static List<AccountManager> LoadAccountManagers()
    {
        try
        {
            var json = File.ReadAllText(AccountManagersFile);
            return JsonSerializer.Deserialize<List<AccountManager>>(json, JsonOptions) ?? new List<AccountManager>();
        }
        catch (Exception)
        {
            return new List<AccountManager>();
        }
    }

    /// <summary>
    /// Saves all account managers to the JSON file.
    /// </summary>
    /// <param name="accountManagers">The list of account managers to save.</param>
    /// <remarks>
    /// This method serializes the list of account managers to JSON format and writes
    /// it to the accountManagers.json file.
    /// </remarks>
    public static void SaveAccountManagers(List<AccountManager> accountManagers)
    {
        var json = JsonSerializer.Serialize(accountManagers, JsonOptions);
        File.WriteAllText(AccountManagersFile, json);
    }

    /// <summary>
    /// Clears all data from the database by saving empty collections to all JSON files.
    /// </summary>
    /// <remarks>
    /// This method resets all JSON files to contain empty collections, effectively
    /// clearing all data from the database.
    /// </remarks>
    public static void ClearAllData()
    {
        SaveAccounts(new List<Account>());
        SaveCustomers(new List<Customer>());
        SaveTransactions(new List<Transaction>());
        SaveAccountManagers(new List<AccountManager>());
    }

    /// <summary>
    /// Gets the path to the database directory.
    /// </summary>
    /// <returns>The full path to the database directory.</returns>
    /// <remarks>
    /// This method returns the path where all JSON database files are stored.
    /// </remarks>
    public static string GetDatabasePath()
    {
        return DataDirectory;
    }
} 