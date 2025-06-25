namespace BankApp.Data.JsonStorage;

/// <summary>
/// Provides high-level database management operations for the JSON-based database system.
/// </summary>
/// <remarks>
/// This class provides utility methods for database initialization, backup and restore
/// operations, statistics gathering, and data management. It acts as a facade for
/// the underlying JsonDatabase operations.
/// </remarks>
public static class DatabaseManager
{
    /// <summary>
    /// Initializes the database system.
    /// </summary>
    /// <remarks>
    /// This method ensures the database is properly initialized. It will be called
    /// automatically by the JsonDatabase static constructor, but can also be called
    /// explicitly if needed.
    /// </remarks>
    public static void InitializeDatabase()
    {
        // This will be called automatically by JsonDatabase static constructor,
        // but we can also call it explicitly if needed
        var _ = JsonDatabase.GetDatabasePath();
    }

    /// <summary>
    /// Creates a backup of the database to the specified directory.
    /// </summary>
    /// <param name="backupPath">The directory path where the backup should be created.</param>
    /// <remarks>
    /// This method creates a timestamped backup directory and copies all JSON files
    /// from the database directory to the backup location. The backup directory name
    /// includes the current date and time for easy identification.
    /// </remarks>
    public static void BackupDatabase(string backupPath)
    {
        var dataPath = JsonDatabase.GetDatabasePath();
        var backupDir = Path.Combine(backupPath, "BankApp_Backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        
        if (!Directory.Exists(backupDir))
        {
            Directory.CreateDirectory(backupDir);
        }

        // Copy all JSON files to backup directory
        var jsonFiles = Directory.GetFiles(dataPath, "*.json");
        foreach (var file in jsonFiles)
        {
            var fileName = Path.GetFileName(file);
            var backupFile = Path.Combine(backupDir, fileName);
            File.Copy(file, backupFile, true);
        }
    }

    /// <summary>
    /// Restores the database from a backup directory.
    /// </summary>
    /// <param name="backupPath">The directory path containing the backup files.</param>
    /// <exception cref="DirectoryNotFoundException">Thrown when the backup directory does not exist.</exception>
    /// <remarks>
    /// This method copies all JSON files from the backup directory to the database
    /// directory, effectively restoring the database to the state it was in when
    /// the backup was created. This operation will overwrite existing database files.
    /// </remarks>
    public static void RestoreDatabase(string backupPath)
    {
        var dataPath = JsonDatabase.GetDatabasePath();
        
        if (!Directory.Exists(backupPath))
        {
            throw new DirectoryNotFoundException($"Backup directory not found: {backupPath}");
        }

        // Copy all JSON files from backup to data directory
        var jsonFiles = Directory.GetFiles(backupPath, "*.json");
        foreach (var file in jsonFiles)
        {
            var fileName = Path.GetFileName(file);
            var dataFile = Path.Combine(dataPath, fileName);
            File.Copy(file, dataFile, true);
        }
    }

    /// <summary>
    /// Gets comprehensive statistics about the database.
    /// </summary>
    /// <returns>A DatabaseStats object containing database statistics.</returns>
    /// <remarks>
    /// This method loads all data from the database and compiles statistics including
    /// record counts, database size, and last modification time. This is useful for
    /// monitoring and reporting purposes.
    /// </remarks>
    public static DatabaseStats GetDatabaseStats()
    {
        var accounts = JsonDatabase.LoadAccounts();
        var customers = JsonDatabase.LoadCustomers();
        var transactions = JsonDatabase.LoadTransactions();
        var accountManagers = JsonDatabase.LoadAccountManagers();

        return new DatabaseStats
        {
            TotalAccounts = accounts.Count,
            TotalCustomers = customers.Count,
            TotalTransactions = transactions.Count,
            TotalAccountManagers = accountManagers.Count,
            DatabaseSize = GetDatabaseSize(),
            LastModified = GetLastModifiedDate()
        };
    }

    /// <summary>
    /// Clears all data from the database.
    /// </summary>
    /// <remarks>
    /// This method resets all JSON files to contain empty collections, effectively
    /// clearing all data from the database. This operation cannot be undone.
    /// </remarks>
    public static void ClearDatabase()
    {
        JsonDatabase.ClearAllData();
    }

    /// <summary>
    /// Calculates the total size of the database files in bytes.
    /// </summary>
    /// <returns>The total size of all JSON database files in bytes.</returns>
    /// <remarks>
    /// This method calculates the combined size of all JSON files in the database
    /// directory by summing the file sizes.
    /// </remarks>
    private static long GetDatabaseSize()
    {
        var dataPath = JsonDatabase.GetDatabasePath();
        var jsonFiles = Directory.GetFiles(dataPath, "*.json");
        return jsonFiles.Sum(file => new FileInfo(file).Length);
    }

    /// <summary>
    /// Gets the most recent modification time of any database file.
    /// </summary>
    /// <returns>The most recent modification time, or DateTime.MinValue if no files exist.</returns>
    /// <remarks>
    /// This method finds the most recent modification time among all JSON files
    /// in the database directory. This is useful for tracking when the database
    /// was last updated.
    /// </remarks>
    private static DateTime GetLastModifiedDate()
    {
        var dataPath = JsonDatabase.GetDatabasePath();
        var jsonFiles = Directory.GetFiles(dataPath, "*.json");
        
        if (!jsonFiles.Any())
            return DateTime.MinValue;

        return jsonFiles.Max(file => File.GetLastWriteTime(file));
    }
}

/// <summary>
/// Contains statistics about the database.
/// </summary>
/// <remarks>
/// This class holds various metrics about the database including record counts,
/// database size, and modification timestamps. It is used for monitoring and
/// reporting purposes.
/// </remarks>
public class DatabaseStats
{
    /// <summary>
    /// Gets or sets the total number of accounts in the database.
    /// </summary>
    public int TotalAccounts { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of customers in the database.
    /// </summary>
    public int TotalCustomers { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of transactions in the database.
    /// </summary>
    public int TotalTransactions { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of account managers in the database.
    /// </summary>
    public int TotalAccountManagers { get; set; }
    
    /// <summary>
    /// Gets or sets the total size of the database files in bytes.
    /// </summary>
    public long DatabaseSize { get; set; }
    
    /// <summary>
    /// Gets or sets the most recent modification time of any database file.
    /// </summary>
    public DateTime LastModified { get; set; }
} 