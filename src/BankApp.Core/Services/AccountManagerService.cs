using BankApp.Abstractions;

namespace BankApp.Core.Services;

/// <summary>
/// Provides business logic services for managing account managers.
/// </summary>
/// <remarks>
/// This service class implements the IAccountManagerService interface and provides
/// high-level business operations for account manager management including creation,
/// authentication, validation, and data access operations.
/// </remarks>
public class AccountManagerService : IAccountManagerService
{
    /// <summary>
    /// The repository for account manager data access operations.
    /// </summary>
    private readonly IAccountManagerRepository _accountManagerRepo;

    /// <summary>
    /// Initializes a new instance of the AccountManagerService class.
    /// </summary>
    /// <param name="accountManagerRepo">The account manager repository for data access.</param>
    /// <remarks>
    /// This constructor injects the account manager repository dependency
    /// to enable data access operations.
    /// </remarks>
    public AccountManagerService(IAccountManagerRepository accountManagerRepo)
    {
        _accountManagerRepo = accountManagerRepo;
    }

    /// <summary>
    /// Creates a new account manager in the system.
    /// </summary>
    /// <param name="accountManager">The account manager to create.</param>
    /// <exception cref="InvalidOperationException">Thrown when username or email already exists.</exception>
    /// <remarks>
    /// This method validates that the username and email are unique before creating
    /// the account manager. If either already exists, an exception is thrown.
    /// </remarks>
    public void CreateAccountManager(AccountManager accountManager)
    {
        if (_accountManagerRepo.UsernameExists(accountManager.Username))
        {
            throw new InvalidOperationException("Username already exists.");
        }

        if (_accountManagerRepo.EmailExists(accountManager.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        _accountManagerRepo.Add(accountManager);
    }

    /// <summary>
    /// Updates an existing account manager's information.
    /// </summary>
    /// <param name="accountManager">The account manager with updated information.</param>
    /// <remarks>
    /// This method updates the account manager's information in the repository.
    /// No validation is performed as the account manager should already exist.
    /// </remarks>
    public void UpdateAccountManager(AccountManager accountManager)
    {
        _accountManagerRepo.Update(accountManager);
    }

    /// <summary>
    /// Retrieves an account manager by their username.
    /// </summary>
    /// <param name="username">The username of the account manager to find.</param>
    /// <returns>The account manager if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for an account manager using their username,
    /// which is commonly used for authentication purposes.
    /// </remarks>
    public AccountManager? GetAccountManager(string username)
    {
        return _accountManagerRepo.GetByUsername(username);
    }

    /// <summary>
    /// Retrieves an account manager by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the account manager to find.</param>
    /// <returns>The account manager if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for an account manager using their unique ID.
    /// </remarks>
    public AccountManager? GetAccountManagerById(string id)
    {
        return _accountManagerRepo.GetById(id);
    }

    /// <summary>
    /// Retrieves all account managers from the system.
    /// </summary>
    /// <returns>An enumerable collection of all account managers.</returns>
    /// <remarks>
    /// This method returns all account managers regardless of their active status.
    /// </remarks>
    public IEnumerable<AccountManager> GetAllAccountManagers()
    {
        return _accountManagerRepo.GetAll();
    }

    /// <summary>
    /// Deletes an account manager from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the account manager to delete.</param>
    /// <remarks>
    /// This method removes the account manager from the repository if they exist.
    /// If the account manager doesn't exist, no action is taken.
    /// </remarks>
    public void DeleteAccountManager(string id)
    {
        var accountManager = _accountManagerRepo.GetById(id);
        if (accountManager != null)
        {
            _accountManagerRepo.Delete(id);
        }
    }

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
    public bool AuthenticateAccountManager(string username, string password)
    {
        var accountManager = _accountManagerRepo.GetByUsername(username);
        if (accountManager == null || !accountManager.IsActive)
        {
            return false;
        }

        return accountManager.VerifyPassword(password);
    }

    /// <summary>
    /// Checks if a username is already in use by another account manager.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns>True if the username exists; otherwise, false.</returns>
    /// <remarks>
    /// This method is used for validation during account manager creation
    /// to ensure usernames are unique.
    /// </remarks>
    public bool UsernameExists(string username)
    {
        return _accountManagerRepo.UsernameExists(username);
    }

    /// <summary>
    /// Checks if an email address is already in use by another account manager.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>True if the email exists; otherwise, false.</returns>
    /// <remarks>
    /// This method is used for validation during account manager creation
    /// to ensure email addresses are unique.
    /// </remarks>
    public bool EmailExists(string email)
    {
        return _accountManagerRepo.EmailExists(email);
    }
} 