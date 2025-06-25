using BankApp.Abstractions;

namespace BankApp.Core.Services;

/// <summary>
/// Defines the contract for account manager business operations.
/// </summary>
/// <remarks>
/// This interface provides methods for managing account managers including creation,
/// authentication, validation, and data access operations. It serves as the contract
/// for the AccountManagerService implementation.
/// </remarks>
public interface IAccountManagerService
{
    /// <summary>
    /// Creates a new account manager in the system.
    /// </summary>
    /// <param name="accountManager">The account manager to create.</param>
    /// <exception cref="InvalidOperationException">Thrown when username or email already exists.</exception>
    /// <remarks>
    /// This method validates that the username and email are unique before creating
    /// the account manager. If either already exists, an exception is thrown.
    /// </remarks>
    void CreateAccountManager(AccountManager accountManager);
    
    /// <summary>
    /// Updates an existing account manager's information.
    /// </summary>
    /// <param name="accountManager">The account manager with updated information.</param>
    /// <remarks>
    /// This method updates the account manager's information in the repository.
    /// No validation is performed as the account manager should already exist.
    /// </remarks>
    void UpdateAccountManager(AccountManager accountManager);
    
    /// <summary>
    /// Retrieves an account manager by their username.
    /// </summary>
    /// <param name="username">The username of the account manager to find.</param>
    /// <returns>The account manager if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for an account manager using their username,
    /// which is commonly used for authentication purposes.
    /// </remarks>
    AccountManager? GetAccountManager(string username);
    
    /// <summary>
    /// Retrieves an account manager by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the account manager to find.</param>
    /// <returns>The account manager if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for an account manager using their unique ID.
    /// </remarks>
    AccountManager? GetAccountManagerById(string id);
    
    /// <summary>
    /// Retrieves all account managers from the system.
    /// </summary>
    /// <returns>An enumerable collection of all account managers.</returns>
    /// <remarks>
    /// This method returns all account managers regardless of their active status.
    /// </remarks>
    IEnumerable<AccountManager> GetAllAccountManagers();
    
    /// <summary>
    /// Deletes an account manager from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the account manager to delete.</param>
    /// <remarks>
    /// This method removes the account manager from the repository if they exist.
    /// If the account manager doesn't exist, no action is taken.
    /// </remarks>
    void DeleteAccountManager(string id);
    
    /// <summary>
    /// Authenticates an account manager using their username and password.
    /// </summary>
    /// <param name="username">The username of the account manager.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>True if authentication is successful; otherwise, false.</returns>
    /// <remarks>
    /// This method verifies the account manager's credentials by checking if the
    /// account manager exists, is active, and the password matches the stored hash.
    /// </remarks>
    bool AuthenticateAccountManager(string username, string password);
    
    /// <summary>
    /// Checks if a username is already in use by another account manager.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns>True if the username exists; otherwise, false.</returns>
    /// <remarks>
    /// This method is used for validation during account manager creation
    /// to ensure usernames are unique.
    /// </remarks>
    bool UsernameExists(string username);
    
    /// <summary>
    /// Checks if an email address is already in use by another account manager.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>True if the email exists; otherwise, false.</returns>
    /// <remarks>
    /// This method is used for validation during account manager creation
    /// to ensure email addresses are unique.
    /// </remarks>
    bool EmailExists(string email);
} 