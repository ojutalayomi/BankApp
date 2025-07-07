using BankApp.Abstractions.Enums;

namespace BankApp.Abstractions;

/// <summary>
/// Represents a bank customer with personal information, accounts, and transaction history.
/// </summary>
/// <remarks>
/// This class manages customer information including personal details, authentication
/// credentials, associated accounts, complaints, and transaction history. Customers
/// can have multiple accounts and can file complaints when needed.
/// </remarks>
public class Customer
{
    /// <summary>
    /// Gets or sets the unique identifier for the customer.
    /// </summary>
    /// <value>A string representing the customer's unique ID.</value>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// Gets or sets the full name of the customer.
    /// </summary>
    /// <value>A string representing the customer's full name.</value>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the gender of the customer.
    /// </summary>
    /// <value>A Gender enum value representing the customer's gender.</value>
    public Gender Gender { get; set; }
    
    /// <summary>
    /// Gets or sets the age of the customer.
    /// </summary>
    /// <value>An integer representing the customer's age.</value>
    public int Age { get; set; }
    
    /// <summary>
    /// Gets or sets the date of birth of the customer.
    /// </summary>
    /// <value>A DateTime representing the customer's date of birth.</value>
    public DateTime DateOfBirth { get; set; }
    
    /// <summary>
    /// Gets or sets the nationality of the customer.
    /// </summary>
    /// <value>A string representing the customer's nationality.</value>
    public string Nationality { get; set; }
    
    /// <summary>
    /// Gets or sets the marital status of the customer.
    /// </summary>
    /// <value>A MaritalStatus enum value representing the customer's marital status.</value>
    public MaritalStatus MaritalStatus { get; set; }
    
    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    /// <value>A string representing the customer's phone number.</value>
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the address of the customer.
    /// </summary>
    /// <value>A string representing the customer's address.</value>
    public string Address { get; set; }
    
    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    /// <value>A string representing the customer's login username.</value>
    public string Username { get; set; }
    
    /// <summary>
    /// Gets or sets the hashed password for authentication.
    /// </summary>
    /// <value>A string containing the hashed password.</value>
    public string Password { get; set; }
    
    /// <summary>
    /// Gets or sets the list of account numbers associated with this customer.
    /// </summary>
    /// <value>A list of strings representing the customer's account numbers.</value>
    public List<string> AccountNumbers { get; set; }
    
    /// <summary>
    /// Gets or sets the list of complaints filed by this customer.
    /// </summary>
    /// <value>A list of Complaint objects representing the customer's complaints.</value>
    public List<Complaint> Complaints { get; set; }
    
    /// <summary>
    /// Gets or sets the list of transactions performed by this customer.
    /// </summary>
    /// <value>A list of Transaction objects representing the customer's transaction history.</value>
    public List<Transaction> Transactions { get; set; }

    /// <summary>
    /// Initializes a new instance of the Customer class with default values.
    /// </summary>
    /// <remarks>
    /// This constructor is primarily used for JSON deserialization.
    /// All properties are initialized with default values.
    /// </remarks>
    public Customer()
    {
        Name = string.Empty;
        Gender = Gender.Male;
        Age = 0;
        DateOfBirth = DateTime.Now;
        Nationality = string.Empty;
        MaritalStatus = MaritalStatus.Single;
        PhoneNumber = string.Empty;
        Address = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
        AccountNumbers = new List<string>();
        Complaints = new List<Complaint>();
        Transactions = new List<Transaction>();
    }

    /// <summary>
    /// Initializes a new instance of the Customer class with specified values.
    /// </summary>
    /// <param name="name">The full name of the customer.</param>
    /// <param name="gender">The gender of the customer.</param>
    /// <param name="age">The age of the customer.</param>
    /// <param name="dateOfBirth">The date of birth of the customer.</param>
    /// <param name="nationality">The nationality of the customer.</param>
    /// <param name="maritalStatus">The marital status of the customer.</param>
    /// <param name="phoneNumber">The phone number of the customer.</param>
    /// <param name="address">The address of the customer.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The plain text password to be hashed.</param>
    /// <remarks>
    /// This constructor automatically hashes the provided password using PasswordHasher
    /// and initializes empty collections for accounts, complaints, and transactions.
    /// </remarks>
    public Customer(string name, Gender gender, int age, DateTime dateOfBirth, string nationality, MaritalStatus maritalStatus, string phoneNumber, string address, string username, string password)
    {
        Name = name;
        Gender = gender;
        Age = age;
        DateOfBirth = dateOfBirth;
        Nationality = nationality;
        MaritalStatus = maritalStatus;
        PhoneNumber = phoneNumber;
        Address = address;
        Username = username;
        Password = PasswordHasher.HashPassword(password);
        AccountNumbers = new List<string>();
        Complaints = [];
        Transactions = [];
    }

    /// <summary>
    /// Adds an account number to the customer's list of accounts.
    /// </summary>
    /// <param name="accountNumber">The account number to add.</param>
    /// <remarks>
    /// This method checks if the account number is already in the list before adding it
    /// to prevent duplicates.
    /// </remarks>
    public void AddAccount(string accountNumber)
    {
        if (!AccountNumbers.Contains(accountNumber))
    {
            AccountNumbers.Add(accountNumber);
        }
    }

    /// <summary>
    /// Removes an account number from the customer's list of accounts.
    /// </summary>
    /// <param name="accountNumber">The account number to remove.</param>
    /// <remarks>
    /// This method removes the specified account number from the customer's account list.
    /// If the account number is not found, no action is taken.
    /// </remarks>
    public void RemoveAccount(string accountNumber)
    {
        AccountNumbers.Remove(accountNumber);
    }

    /// <summary>
    /// Files a new complaint for the customer.
    /// </summary>
    /// <param name="narration">The description or details of the complaint.</param>
    /// <remarks>
    /// This method creates a new Complaint object with the customer's name as the customer ID
    /// and adds it to the customer's complaints list.
    /// </remarks>
    public void FileComplaint(string narration)
    {
        var complaint = new Complaint(narration, Name);
        Complaints.Add(complaint);
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