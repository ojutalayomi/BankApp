using BankApp.Abstractions.Enums;

namespace BankApp.Abstractions;

/// <summary>
/// Represents an account manager who can manage customers, accounts, and handle complaints.
/// </summary>
/// <remarks>
/// Account managers are bank employees who have administrative privileges to manage
/// customer accounts, respond to complaints, and perform various banking operations.
/// They have authentication credentials and can be active or inactive.
/// </remarks>
public class AccountManager
{
    /// <summary>
    /// Gets or sets the unique identifier for the account manager.
    /// </summary>
    /// <value>A string representing the account manager's unique ID.</value>
    public string Id { get; set; }
    
    /// <summary>
    /// Gets or sets the account manager's employee ID.
    /// </summary>
    /// <value>A string representing the account manager's employee ID.</value>
    public string AccountManagerId { get; set; }
    
    /// <summary>
    /// Gets or sets the full name of the account manager.
    /// </summary>
    /// <value>A string representing the account manager's full name.</value>
    public string AccountManagerName { get; set; }
    
    /// <summary>
    /// Gets or sets the contact information for the account manager.
    /// </summary>
    /// <value>A string containing contact details like branch or department.</value>
    public string ContactInformation { get; set; }
    
    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    /// <value>A string representing the account manager's login username.</value>
    public string Username { get; set; }
    
    /// <summary>
    /// Gets or sets the hashed password for authentication.
    /// </summary>
    /// <value>A string containing the hashed password.</value>
    public string Password { get; set; }
    
    /// <summary>
    /// Gets or sets the email address of the account manager.
    /// </summary>
    /// <value>A string representing the account manager's email address.</value>
    public string Email { get; set; }
    
    /// <summary>
    /// Gets or sets the phone number of the account manager.
    /// </summary>
    /// <value>A string representing the account manager's phone number.</value>
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the account manager was created.
    /// </summary>
    /// <value>A DateTime representing the account manager's creation date.</value>
    public DateTime DateCreated { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the account manager is active.
    /// </summary>
    /// <value>True if the account manager is active; otherwise, false.</value>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the list of customers managed by this account manager.
    /// </summary>
    /// <value>A list of Customer objects managed by this account manager.</value>
    public List<Customer> ManagedCustomers { get; set; }
    
    /// <summary>
    /// Gets or sets the list of accounts managed by this account manager.
    /// </summary>
    /// <value>A list of Account objects managed by this account manager.</value>
    public List<Account> ManagedAccounts { get; set; }

    /// <summary>
    /// Initializes a new instance of the AccountManager class with default values.
    /// </summary>
    /// <remarks>
    /// This constructor is primarily used for JSON deserialization.
    /// All properties are initialized with default values.
    /// </remarks>
    public AccountManager()
    {
        Id = Guid.NewGuid().ToString();
        AccountManagerId = Guid.NewGuid().ToString();
        AccountManagerName = string.Empty;
        ContactInformation = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
        Email = string.Empty;
        PhoneNumber = string.Empty;
        DateCreated = DateTime.Now;
        IsActive = true;
        ManagedCustomers = [];
        ManagedAccounts = [];
    }

    /// <summary>
    /// Initializes a new instance of the AccountManager class with specified values.
    /// </summary>
    /// <param name="accountManagerName">The full name of the account manager.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The plain text password to be hashed.</param>
    /// <param name="email">The email address of the account manager.</param>
    /// <param name="phoneNumber">The phone number of the account manager.</param>
    /// <param name="contactInformation">The contact information (branch, department, etc.).</param>
    /// <remarks>
    /// This constructor automatically hashes the provided password using PasswordHasher
    /// and sets the account manager as active by default.
    /// </remarks>
    public AccountManager(string accountManagerName, string username, string password, string email, string phoneNumber, string contactInformation)
    {
        Id = Guid.NewGuid().ToString();
        AccountManagerId = Guid.NewGuid().ToString();
        AccountManagerName = accountManagerName;
        Username = username;
        Password = PasswordHasher.HashPassword(password);
        Email = email;
        PhoneNumber = phoneNumber;
        ContactInformation = contactInformation;
        DateCreated = DateTime.Now;
        IsActive = true;
        ManagedCustomers = [];
        ManagedAccounts = [];
    }

    /// <summary>
    /// Deactivates an account by freezing it.
    /// </summary>
    /// <param name="account">The account to deactivate.</param>
    /// <remarks>
    /// This method freezes the specified account, preventing any transactions
    /// until the account is unfrozen.
    /// </remarks>
    public void DeactivateAccount(Account account)
    {
        account.FreezeAccount();
    }

    /// <summary>
    /// Responds to a customer complaint with resolution details.
    /// </summary>
    /// <param name="complaint">The complaint to respond to.</param>
    /// <param name="resolutionDetails">The details of how the complaint was resolved.</param>
    /// <remarks>
    /// This method updates the complaint status to Resolved, sets the resolution details,
    /// and records the time when the complaint was resolved.
    /// </remarks>
    public void RespondToComplaint(Complaint complaint, string resolutionDetails)
    {
        complaint.ResolutionDetails = resolutionDetails;
        complaint.Status = ComplaintStatus.Resolved;
        complaint.TimeResolved = DateTime.Now;
    }

    /// <summary>
    /// Verifies if the provided password matches the stored hashed password.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <returns>True if the password matches; otherwise, false.</returns>
    /// <remarks>
    /// This method uses the PasswordHasher to compare the provided plain text password
    /// with the stored hashed password.
    /// </remarks>
    public bool VerifyPassword(string password)
    {
        return PasswordHasher.VerifyPassword(password, Password);
    }
} 