using System.Text.RegularExpressions;
using BankApp.Core.Services;
using BankApp.Abstractions;
using BankApp.Data.Repositories;
using BankApp.Data.JsonStorage;
using BankApp.Abstractions.Enums;

namespace BankApp.CLI;

internal class Program
{
    private static string _session_customer_id;
    private static string _session_account_manager_id;
    private static AccountService _accountService;
    private static CustomerService _customerService;
    private static TransactionService _transactionService;
    private static AccountManagerService _accountManagerService;
    private static IAccountRepository _accountRepo;
    private static ICustomerRepository _customerRepo;
    private static IAccountManagerRepository _accountManagerRepo;

    static void Main(string[] args)
    {
        // Initialize account number counter from existing accounts
        var accounts = JsonDatabase.LoadAccounts();
        int maxAccountNumber = accounts
            .Select(a => int.TryParse(a.AccountNumber, out int num) ? num : 1000000)
            .DefaultIfEmpty(1000000)
            .Max();
        AccountGenerator.InitializeCounter(maxAccountNumber);

        Console.WriteLine("🏦 Welcome to BankApp - JSON Database System");
        Console.WriteLine("=============================================");
        
        // Initialize JSON database
        DatabaseManager.InitializeDatabase();
        Console.WriteLine($"✅ Database initialized at: {JsonDatabase.GetDatabasePath()}");
        
        // Initialize services
        InitializeServices();
        
        // Show main menu
        // ShowMainMenu();
        FirstPage();
    }

    static void InitializeServices()
    {
        _accountRepo = new JsonAccountRepository();
        _customerRepo = new JsonCustomerRepository();
        var transactionRepo = new JsonTransactionRepository();
        
        _accountService = new AccountService(_accountRepo, transactionRepo);
        _customerService = new CustomerService(_customerRepo, _accountRepo);
        _transactionService = new TransactionService(_accountRepo, transactionRepo);
        _accountManagerRepo = new JsonAccountManagerRepository();
        _accountManagerService = new AccountManagerService(_accountManagerRepo);
    }

    static void FirstPage()
    {
        while (true)
        {
            Console.WriteLine("1. Continue as a Customer");
            Console.WriteLine("2. Continue as an Account Manager");
            Console.WriteLine("0. Exit");
            Console.Write("\nSelect an option: ");
            
            var choice = Console.ReadLine() ?? string.Empty;
            
            switch (choice)
            {
                case "1":
                    Login("customer");
                    break;
                case "2":
                    Login("accountManager");
                    break;
                case "0":
                    Console.WriteLine("👋 Thank you for using BankApp!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void Login(string type)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Do you have an account? ");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            var input = Console.ReadLine() ?? string.Empty;

            if (input == "1")
            {
                Console.Clear();
                Console.WriteLine("\nSign In | BankApp");
                Console.WriteLine("Please enter your login details.");
                Console.WriteLine("Enter 0 to go back.");
                Console.WriteLine("======================");
                Console.Write("Username: ");
                var username = Console.ReadLine() ?? string.Empty;
                
                if(username == "0") return;

                Console.Write("\nPassword: ");
                var password = Console.ReadLine() ?? string.Empty;
                
                bool authenticated = false;
                
                if (type == "accountManager")
                {
                    // Try to authenticate as account manager first
                    authenticated = _accountManagerService.AuthenticateAccountManager(username, password);

                    switch (authenticated)
                    {
                        // Fallback to hardcoded admin for backward compatibility
                        case false when username == "admin" && password == "<PASSWORD>":
                            authenticated = true;
                            Console.WriteLine("Welcome back, Admin!");
                            break;
                        case true:
                        {
                            var accountManager = _accountManagerService.GetAccountManager(username);
                            if (accountManager != null) {
                                authenticated = true;
                                _session_account_manager_id = accountManager.Id;
                            }
                            Console.WriteLine($"Welcome back, {accountManager?.AccountManagerName}!");
                            break;
                        }
                    }
                }
                else if (type == "customer")
                {
                    // For customers, check if it's the hardcoded admin
                    if (username == "admin" && password == "<PASSWORD>")
                    {
                        authenticated = true;
                        Console.WriteLine("Welcome back, Admin!");
                    }
                    // Check customer credentials in a database
                    else
                    {
                        var customer = _customerRepo.GetCustomer(username);
                        if (customer != null && customer.VerifyPassword(password))
                        {
                            authenticated = true;
                            Console.WriteLine($"Welcome back, {customer.Name}!");
                            _session_customer_id = customer.Id;
                        }
                    }
                }
                
                if (authenticated)
                {
                    switch (type)
                    {
                        case "accountManager":
                            ShowMainMenu();
                            break;
                        case "customer":
                            ShowTransactionMenu();
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid username or password. Please try again.");
                    continue;
                }
            }
            else
            {
                if (type == "accountManager")
                {
                    CreateAccountManager();
                }
                else
                {
                    CreateCustomer();
                }
                return;
            }

            break;
        }
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n📋 MAIN MENU");
            Console.WriteLine("============");
            Console.WriteLine("1. Customer Management");
            Console.WriteLine("2. Account Management");
            Console.WriteLine("3. Transaction Operations");
            Console.WriteLine("4. Account Manager Management");
            Console.WriteLine("5. Database Management");
            Console.WriteLine("6. View Statistics");
            Console.WriteLine("0. Exit");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    ShowCustomerMenu();
                    break;
                case "2":
                    ShowAccountMenu();
                    break;
                case "3":
                    ShowTransactionMenu();
                    break;
                case "4":
                    ShowAccountManagerMenu();
                    break;
                case "5":
                    ShowDatabaseMenu();
                    break;
                case "6":
                    ShowStatistics();
                    break;
                case "0":
                    Console.WriteLine("👋 Thank you for using BankApp!");
                    return;
                default:
                    Console.WriteLine("❌ Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void ShowCustomerMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n👥 CUSTOMER MANAGEMENT");
            Console.WriteLine("=====================");
            Console.WriteLine("1. Create New Customer");
            Console.WriteLine("2. View All Customers");
            Console.WriteLine("3. Find Customer");
            Console.WriteLine("4. Update Customer");
            Console.WriteLine("5. Delete Customer");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    CreateCustomer();
                    break;
                case "2":
                    ViewAllCustomers();
                    break;
                case "3":
                    FindCustomer();
                    break;
                case "4":
                    UpdateCustomer();
                    break;
                case "5":
                    DeleteCustomer();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void ShowAccountMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n💳 ACCOUNT MANAGEMENT");
            Console.WriteLine("====================");
            Console.WriteLine("1. Create New Account");
            Console.WriteLine("2. Create Account for Customer");
            Console.WriteLine("3. View All Accounts");
            Console.WriteLine("4. View Accounts by Customer");
            Console.WriteLine("5. Find Account");
            Console.WriteLine("6. Freeze/Unfreeze Account");
            Console.WriteLine("7. Delete Account");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    CreateAccount();
                    break;
                case "2":
                    CreateAccountForCustomer();
                    break;
                case "3":
                    ViewAllAccounts();
                    break;
                case "4":
                    ViewAccountsByCustomer();
                    break;
                case "5":
                    FindAccount();
                    break;
                case "6":
                    ToggleAccountFreeze();
                    break;
                case "7":
                    DeleteAccount();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void ShowTransactionMenu()
    {
        while (true)
        {
            Console.WriteLine("\n💰 TRANSACTION OPERATIONS");
            Console.WriteLine("=========================");
            Console.WriteLine("1. Deposit Money");
            Console.WriteLine("2. Withdraw Money");
            Console.WriteLine("3. Transfer Money");
            Console.WriteLine("4. View Transaction History");
            Console.WriteLine("5. View Account Balance");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    DepositMoney();
                    break;
                case "2":
                    WithdrawMoney();
                    break;
                case "3":
                    TransferMoney();
                    break;
                case "4":
                    ViewTransactionHistory();
                    break;
                case "5":
                    ViewAccountBalance();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void ShowDatabaseMenu()
    {
        while (true)
        {
            Console.WriteLine("\n🗄️ DATABASE MANAGEMENT");
            Console.WriteLine("=====================");
            Console.WriteLine("1. Create Backup");
            Console.WriteLine("2. Restore from Backup");
            Console.WriteLine("3. Clear All Data");
            Console.WriteLine("4. View Database Info");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    CreateBackup();
                    break;
                case "2":
                    RestoreBackup();
                    break;
                case "3":
                    ClearAllData();
                    break;
                case "4":
                    ViewDatabaseInfo();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Invalid option. Please try again.");
                    break;
            }
        }
    }

    // Customer Operations
    static void CreateCustomer()
    {
        Console.WriteLine("\n📝 CREATE NEW CUSTOMER");
        Console.WriteLine("=====================");
        
        try
        {
            string name;
            while (true)
            {
                Console.Write("Name: ");
                name = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("❌ Name cannot be empty.");
                }
                else if (Regex.IsMatch(name, @"\d"))
                {
                    Console.WriteLine("❌ Name cannot contain number.");
                } 
                else if (name.Length < 3)
                {
                    Console.WriteLine("❌ Name must have minimum of 3 letters");
                } 
                else
                {
                    break;
                }
            }

            int age;
            while (true)
            {
                Console.Write("Age: ");
                if (int.TryParse(Console.ReadLine(), out age) && age >= 18 && age <= 120)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Invalid age. Must be between 18 and 120.");
                }
            }

            Gender gender;
            while (true)
            {
                Console.Write("Gender (0=Male, 1=Female, 2=Other): ");
                string input = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(input, out int value) && value >= 0 && value <= 2)
                {
                    gender = (Gender)value;
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Invalid gender selection.");
                }
            }
            
            AccountType accountType;
            while (true)
            {
                Console.Write("AccountType (0=Savings, 1=Current, 2=Fixed Deposit): ");
                string input = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(input, out int value) && value >= 0 && value <= 2)
                {
                    accountType = (AccountType)value;
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Invalid account type selection.");
                }
            }

            DateTime dateOfBirth;
            while (true)
            {
                Console.Write("Date of Birth (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out var dob))
                {
                    dateOfBirth = dob;
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Invalid date format.");
                }
            }

            string nationality;
            while (true)
            {
                Console.Write("Nationality: ");
                nationality = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(nationality))
                {
                    Console.WriteLine("❌ Nationality cannot be empty.");
                } 
                else if (nationality?.Length < 3)
                {
                    Console.WriteLine("❌ Nationality cannot be less than 3 letters");
                } 
                else
                {
                    break;
                }
            }

            MaritalStatus maritalStatus;
            while (true)
            {
                Console.Write("Marital Status (0=Single, 1=Married, 2=Divorced, 3=Widowed): ");
                string input = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(input, out int value) && value >= 0 && value <= 3)
                {
                    maritalStatus = (MaritalStatus)value;
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Invalid marital status selection.");
                }
            }

            string phoneNumber;
            while (true)
            {
                Console.Write("Phone Number: ");
                phoneNumber = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    Console.WriteLine("❌ Phone number cannot be empty.");
                } else if (phoneNumber.Length < 10)
                {
                    Console.WriteLine("❌ Phone number cannot be less than 10 digits.");
                }
                else
                {
                    break;
                }
            }

            string address;
            while (true)
            {
                Console.Write("Address: ");
                address = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(address))
                {
                    Console.WriteLine("❌ Address cannot be empty.");
                } else if (address.Length < 10)
                {
                    Console.WriteLine("❌ Address is too short.");
                }
                else
                {
                    break;
                }
            }

            string username;
            while (true)
            {
                Console.Write("Username: ");
                username = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("❌ Username cannot be empty.");
                } 
                else if (username?.Length < 3)
                {
                    Console.WriteLine("❌ Username cannot be less than 3 letters");
                } 
                else
                {
                    break;
                }
            }

            string password;
            while (true)
            {
                Console.Write("Password: ");
                password = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("❌ Password cannot be empty.");
                }
                else if (password?.Length < 8)
                {
                    Console.WriteLine("❌ Password cannot be less than 8 characters");
                }
                else
                {
                    break;
                }
            }

            var customer = new Customer(name, gender, age, dateOfBirth, nationality, maritalStatus, phoneNumber, address, username, password);
            _customerService.CreateCustomer(customer, accountType);
            
            Console.Clear();
            Console.WriteLine($"✅ Customer '{name}' created successfully!");
            
            // Get the customer's accounts to display the account number
            var customerAccounts = _customerService.GetCustomerAccounts(name);
            var defaultAccount = customerAccounts.FirstOrDefault();
            if (defaultAccount != null)
            {
                Console.WriteLine($"Account Number: {defaultAccount.AccountNumber}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating customer: {ex.Message}");
        }
    }

    static void CreateAccountManager()
    {
        Console.WriteLine("\n👨‍💼 CREATE NEW ACCOUNT MANAGER");
        Console.WriteLine("=============================");
        
        try
        {
            string name;
            while (true)
            {
                Console.Write("Full Name: ");
                name = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("❌ Name cannot be empty.");
                }
                else if (Regex.IsMatch(name, @"\d"))
                {
                    Console.WriteLine("❌ Name cannot contain numbers.");
                } 
                else if (name.Length < 3)
                {
                    Console.WriteLine("❌ Name must have minimum of 3 letters");
                } 
                else
                {
                    break;
                }
            }

            string username;
            while (true)
            {
                Console.Write("Username: ");
                username = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("❌ Username cannot be empty.");
                } 
                else if (username.Length < 3)
                {
                    Console.WriteLine("❌ Username cannot be less than 3 characters");
                }
                else if (_accountManagerService.UsernameExists(username))
                {
                    Console.WriteLine("❌ Username already exists. Please choose another.");
                }
                else
                {
                    break;
                }
            }

            string email;
            while (true)
            {
                Console.Write("Email: ");
                email = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("❌ Email cannot be empty.");
                }
                else if (!email.Contains("@") || !email.Contains("."))
                {
                    Console.WriteLine("❌ Please enter a valid email address.");
                }
                else if (_accountManagerService.EmailExists(email))
                {
                    Console.WriteLine("❌ Email already exists. Please use another email.");
                }
                else
                {
                    break;
                }
            }

            string phoneNumber;
            while (true)
            {
                Console.Write("Phone Number: ");
                phoneNumber = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    Console.WriteLine("❌ Phone number cannot be empty.");
                } 
                else if (phoneNumber.Length < 10)
                {
                    Console.WriteLine("❌ Phone number cannot be less than 10 digits.");
                }
                else
                {
                    break;
                }
            }

            string contactInformation;
            while (true)
            {
                Console.Write("Contact Information (e.g., Branch, Department): ");
                contactInformation = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(contactInformation))
                {
                    Console.WriteLine("❌ Contact information cannot be empty.");
                }
                else if (contactInformation.Length < 5)
                {
                    Console.WriteLine("❌ Contact information is too short.");
                }
                else
                {
                    break;
                }
            }

            string password;
            while (true)
            {
                Console.Write("Password: ");
                password = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("❌ Password cannot be empty.");
                }
                else if (password.Length < 8)
                {
                    Console.WriteLine("❌ Password must be at least 8 characters long");
                }
                else
                {
                    break;
                }
            }

            string confirmPassword;
            while (true)
            {
                Console.Write("Confirm Password: ");
                confirmPassword = Console.ReadLine() ?? string.Empty;
                if (password != confirmPassword)
                {
                    Console.WriteLine("❌ Passwords do not match. Please try again.");
                }
                else
                {
                    break;
                }
            }

            var accountManager = new AccountManager(name, username, password, email, phoneNumber, contactInformation);
            _accountManagerService.CreateAccountManager(accountManager);
            
            Console.Clear();
            Console.WriteLine($"✅ Account Manager '{name}' created successfully!");
            Console.WriteLine($"Username: {username}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Manager ID: {accountManager.AccountManagerId}");
            Console.WriteLine("\nYou can now log in with your username and password.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating account manager: {ex.Message}");
        }
    }

    static void ViewAllAccountManagers()
    {
        Console.WriteLine("\n👨‍💼 ALL ACCOUNT MANAGERS");
        Console.WriteLine("=======================");
        
        var accountManagers = _accountManagerService.GetAllAccountManagers();
        if (!accountManagers.Any())
        {
            Console.WriteLine("No account managers found.");
            return;
        }

        foreach (var manager in accountManagers)
        {
            Console.WriteLine($"Name: {manager.AccountManagerName}");
            Console.WriteLine($"Username: {manager.Username}");
            Console.WriteLine($"Email: {manager.Email}");
            Console.WriteLine($"Phone: {manager.PhoneNumber}");
            Console.WriteLine($"Contact Info: {manager.ContactInformation}");
            Console.WriteLine($"Status: {(manager.IsActive ? "✅ Active" : "❌ Inactive")}");
            Console.WriteLine($"Created: {manager.DateCreated:yyyy-MM-dd}");
            Console.WriteLine($"Manager ID: {manager.AccountManagerId}");
            Console.WriteLine("---");
        }
    }

    static void FindAccountManager()
    {
        Console.Write("\nEnter account manager username to find: ");
        var username = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("❌ Username cannot be empty.");
            return;
        }

        var accountManager = _accountManagerService.GetAccountManager(username);
        if (accountManager == null)
        {
            Console.WriteLine("❌ Account manager not found.");
            return;
        }

        Console.WriteLine($"\nFound Account Manager:");
        Console.WriteLine($"Name: {accountManager.AccountManagerName}");
        Console.WriteLine($"Username: {accountManager.Username}");
        Console.WriteLine($"Email: {accountManager.Email}");
        Console.WriteLine($"Phone: {accountManager.PhoneNumber}");
        Console.WriteLine($"Contact Info: {accountManager.ContactInformation}");
        Console.WriteLine($"Status: {(accountManager.IsActive ? "Active" : "Inactive")}");
        Console.WriteLine($"Created: {accountManager.DateCreated:yyyy-MM-dd}");
        Console.WriteLine($"Manager ID: {accountManager.AccountManagerId}");
    }

    static void UpdateAccountManager()
    {
        Console.Write("\nEnter account manager username to update: ");
        var username = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("❌ Username cannot be empty.");
            return;
        }

        var accountManager = _accountManagerService.GetAccountManager(username);
        if (accountManager == null)
        {
            Console.WriteLine("❌ Account manager not found.");
            return;
        }

        Console.WriteLine($"\nUpdating account manager: {accountManager.AccountManagerName}");
        
        Console.Write("New phone number (or press Enter to keep current): ");
        var newPhone = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(newPhone))
        {
            accountManager.PhoneNumber = newPhone;
        }

        Console.Write("New contact information (or press Enter to keep current): ");
        var newContactInfo = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(newContactInfo))
        {
            accountManager.ContactInformation = newContactInfo;
        }

        Console.Write("New email (or press Enter to keep current): ");
        var newEmail = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(newEmail))
        {
            if (!newEmail.Contains("@") || !newEmail.Contains("."))
            {
                Console.WriteLine("❌ Please enter a valid email address.");
                return;
            }
            if (_accountManagerService.EmailExists(newEmail) && newEmail != accountManager.Email)
            {
                Console.WriteLine("❌ Email already exists. Please use another email.");
                return;
            }
            accountManager.Email = newEmail;
        }

        _accountManagerService.UpdateAccountManager(accountManager);
        Console.WriteLine("✅ Account manager updated successfully!");
    }

    static void DeleteAccountManager()
    {
        Console.Write("\nEnter account manager username to delete: ");
        var username = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("❌ Username cannot be empty.");
            return;
        }

        var accountManager = _accountManagerService.GetAccountManager(username);
        if (accountManager == null)
        {
            Console.WriteLine("❌ Account manager not found.");
            return;
        }

        Console.Write($"Are you sure you want to delete account manager '{accountManager.AccountManagerName}'? (y/N): ");
        var confirm = Console.ReadLine()?.ToLower();
        
        if (confirm == "y" || confirm == "yes")
        {
            _accountManagerService.DeleteAccountManager(accountManager.Id);
            Console.WriteLine("✅ Account manager deleted successfully!");
        }
        else
        {
            Console.WriteLine("❌ Deletion cancelled.");
        }
    }

    static void ViewAllCustomers()
    {
        Console.WriteLine("\n👥 ALL CUSTOMERS");
        Console.WriteLine("===============");
        
        var customers = _customerService.GetAllCustomers();
        if (!customers.Any())
        {
            Console.WriteLine("No customers found.");
            return;
        }

        foreach (var customer in customers)
        {
            Console.WriteLine($"Name: {customer.Name}");
            Console.WriteLine($"Age: {customer.Age}, Gender: {customer.Gender}");
            Console.WriteLine($"Phone: {customer.PhoneNumber}");
            
            // Get customer's accounts
            var customerAccounts = _customerService.GetCustomerAccounts(customer.Name);
            if (customerAccounts.Any())
            {
                var totalBalance = customerAccounts.Sum(a => a.AccountBalance);
                var accountNumbers = string.Join(", ", customerAccounts.Select(a => a.AccountNumber));
                Console.WriteLine($"Accounts: {accountNumbers} (Total Balance: ₦{totalBalance})");
            }
            else
            {
                Console.WriteLine("Accounts: None");
            }
            Console.WriteLine("---");
        }
    }

    static void FindCustomer()
    {
        Console.Write("\nEnter customer name to find: ");
        var name = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Name cannot be empty.");
            return;
        }

        var customer = _customerService.GetCustomer(name);
        if (customer == null)
        {
            Console.WriteLine("❌ Customer not found.");
            return;
        }

        Console.WriteLine($"\nFound Customer:");
        Console.WriteLine($"Name: {customer.Name}");
        Console.WriteLine($"Age: {customer.Age}, Gender: {customer.Gender}");
        Console.WriteLine($"Nationality: {customer.Nationality}");
        Console.WriteLine($"Phone: {customer.PhoneNumber}");
        Console.WriteLine($"Address: {customer.Address}");
        
        // Get customer's accounts
        var customerAccounts = _customerService.GetCustomerAccounts(customer.Name);
        if (customerAccounts.Any())
        {
            Console.WriteLine("Accounts:");
            foreach (var account in customerAccounts)
            {
                Console.WriteLine($"  - {account.AccountNumber}: ₦{account.AccountBalance} ({account.AccountType})");
            }
        }
        else
        {
            Console.WriteLine("Accounts: None");
        }
    }

    static void UpdateCustomer()
    {
        Console.Write("\nEnter customer name to update: ");
        var name = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Name cannot be empty.");
            return;
        }

        var customer = _customerService.GetCustomer(name);
        if (customer == null)
        {
            Console.WriteLine("❌ Customer not found.");
            return;
        }

        Console.WriteLine($"\nUpdating customer: {customer.Name}");
        Console.Write("New phone number (or press Enter to keep current): ");
        var newPhone = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(newPhone))
        {
            customer.PhoneNumber = newPhone;
        }

        Console.Write("New address (or press Enter to keep current): ");
        var newAddress = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(newAddress))
        {
            customer.Address = newAddress;
        }

        _customerService.UpdateCustomer(customer);
        Console.WriteLine("✅ Customer updated successfully!");
    }

    static void DeleteCustomer()
    {
        Console.Write("\nEnter customer name to delete: ");
        var name = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Name cannot be empty.");
            return;
        }

        Console.Write($"Are you sure you want to delete customer '{name}'? (y/N): ");
        var confirm = Console.ReadLine()?.ToLower();
        
        if (confirm == "y" || confirm == "yes")
        {
            _customerService.DeleteCustomer(name);
            Console.WriteLine("✅ Customer deleted successfully!");
        }
        else
        {
            Console.WriteLine("❌ Deletion cancelled.");
        }
    }

    // Account Operations
    static void CreateAccount()
    {
        Console.WriteLine("\n💳 CREATE NEW ACCOUNT");
        Console.WriteLine("====================");
        
        try
        {
            // Ask if this is for an existing customer or standalone account
            Console.WriteLine("Account Creation Type:");
            Console.WriteLine("1. Create account for existing customer");
            Console.WriteLine("2. Create standalone account");
            Console.Write("Select option: ");
            
            var accountType = Console.ReadLine() ?? string.Empty;
            
            string customerName = null;
            Customer customer = null;
            
            if (accountType == "1")
            {
                // Create account for existing customer
                Console.Write("Enter customer name: ");
                customerName = Console.ReadLine() ?? string.Empty;
                
                if (string.IsNullOrWhiteSpace(customerName))
                {
                    Console.WriteLine("❌ Customer name cannot be empty.");
                    return;
                }
                
                customer = _customerService.GetCustomer(customerName);
                if (customer == null)
                {
                    Console.WriteLine("❌ Customer not found.");
                    return;
                }
                
                Console.WriteLine($"✅ Found customer: {customer.Name}");
            }
            
            // Get account details
            Console.Write("Account Name: ");
            var accountName = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(accountName))
            {
                Console.WriteLine("❌ Account name cannot be empty.");
                return;
            }

            // Get account type with better validation
            AccountType type;
            while (true)
            {
                Console.Write("Account Type (0=Current, 1=Saving): ");
                var typeInput = Console.ReadLine() ?? string.Empty;
                
                if (int.TryParse(typeInput, out int typeValue) && typeValue >= 0 && typeValue <= 1)
                {
                    type = (AccountType)typeValue;
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Invalid account type. Please enter 0 for Current or 1 for Saving.");
                }
            }

            // Ask for initial deposit
            decimal initialDeposit = 0;
            Console.Write("Initial Deposit Amount (or press Enter for ₦0): ₦");
            var depositInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(depositInput))
            {
                if (!decimal.TryParse(depositInput, out initialDeposit) || initialDeposit < 0)
                {
                    Console.WriteLine("❌ Invalid deposit amount. Must be positive.");
                    return;
                }
            }

            // Create the account
            var account = new Account(accountName, type, customer.Id);
            
            // Add initial deposit if specified
            if (initialDeposit > 0)
            {
                account.Deposit(initialDeposit);
            }
            
            // Save the account
            _accountRepo.Add(account);
            
            // Link to customer if specified
            if (customer != null)
            {
                customer.AddAccount(account.AccountNumber);
                _customerService.UpdateCustomer(customer);
            }
            
            // Display success message
            Console.WriteLine("\n✅ Account created successfully!");
            Console.WriteLine($"Account Number: {account.AccountNumber}");
            Console.WriteLine($"Account Name: {account.AccountName}");
            Console.WriteLine($"Account Type: {account.AccountType}");
            Console.WriteLine($"Initial Balance: ₦{account.AccountBalance}");
            
            if (customer != null)
            {
                Console.WriteLine($"Linked to Customer: {customer.Name}");
            }
            
            // Show account details
            Console.WriteLine("\n📋 Account Details:");
            Console.WriteLine($"Created: {account.DateCreated}");
            Console.WriteLine($"Status: {(account.IsFrozen ? "Frozen" : "Active")}");
            Console.WriteLine($"Transaction History: {account.TransactionIds.Count} transactions");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating account: {ex.Message}");
        }
    }

    static void CreateAccountForCustomer()
    {
        Console.WriteLine("\n💳 CREATE ACCOUNT FOR CUSTOMER");
        Console.WriteLine("=============================");
        
        try
        {
            // Get customer name
            Console.Write("Enter customer name: ");
            var customerName = Console.ReadLine() ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(customerName))
            {
                Console.WriteLine("❌ Customer name cannot be empty.");
                return;
            }
            
            var customer = _customerService.GetCustomer(customerName);
            if (customer == null)
            {
                Console.WriteLine("❌ Customer not found.");
                return;
            }
            
            Console.WriteLine($"✅ Found customer: {customer.Name}");
            
            // Show existing accounts
            var existingAccounts = _customerService.GetCustomerAccounts(customer.Name);
            if (existingAccounts.Any())
            {
                Console.WriteLine("\n📋 Existing Accounts:");
                foreach (var existingAccount in existingAccounts)
                {
                    Console.WriteLine($"  - {existingAccount.AccountNumber}: {existingAccount.AccountName} ({existingAccount.AccountType}) - ₦{existingAccount.AccountBalance}");
                }
            }
            else
            {
                Console.WriteLine("\n📋 No existing accounts found.");
            }
            
            // Get account details
            Console.Write("\nAccount Name: ");
            var accountName = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(accountName))
            {
                Console.WriteLine("❌ Account name cannot be empty.");
                return;
            }

            // Get account type
            AccountType type;
            while (true)
            {
                Console.Write("Account Type (0=Current, 1=Saving): ");
                var typeInput = Console.ReadLine() ?? string.Empty;
                
                if (int.TryParse(typeInput, out int typeValue) && typeValue >= 0 && typeValue <= 1)
                {
                    type = (AccountType)typeValue;
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Invalid account type. Please enter 0 for Current or 1 for Saving.");
                }
            }

            // Ask for initial deposit
            decimal initialDeposit = 0;
            Console.Write("Initial Deposit Amount (or press Enter for ₦0): ₦");
            var depositInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(depositInput))
            {
                if (!decimal.TryParse(depositInput, out initialDeposit) || initialDeposit < 0)
                {
                    Console.WriteLine("❌ Invalid deposit amount. Must be positive.");
                    return;
                }
            }

            // Create the account
            var account = new Account(accountName, type, customer.Id);
            
            // Add initial deposit if specified
            if (initialDeposit > 0)
            {
                account.Deposit(initialDeposit);
            }
            
            // Save the account
            _accountRepo.Add(account);
            
            // Link to customer
            customer.AddAccount(account.AccountNumber);
            _customerService.UpdateCustomer(customer);
            
            // Display success message
            Console.WriteLine("\n✅ Account created and linked successfully!");
            Console.WriteLine($"Account Number: {account.AccountNumber}");
            Console.WriteLine($"Account Name: {account.AccountName}");
            Console.WriteLine($"Account Type: {account.AccountType}");
            Console.WriteLine($"Initial Balance: ₦{account.AccountBalance}");
            Console.WriteLine($"Linked to Customer: {customer.Name}");
            
            // Show updated account count
            var updatedAccounts = _customerService.GetCustomerAccounts(customer.Id);
            Console.WriteLine($"\n📊 Customer now has {updatedAccounts.Count()} account(s)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating account: {ex.Message}");
        }
    }

    static void ViewAllAccounts()
    {
        Console.WriteLine("\n💳 ALL ACCOUNTS");
        Console.WriteLine("==============");
        
        var accounts = _accountRepo.GetAll();
        if (!accounts.Any())
        {
            Console.WriteLine("No accounts found.");
            return;
        }

        Console.WriteLine($"Total Accounts: {accounts.Count()}");
        Console.WriteLine("=".PadRight(80, '='));
        
        foreach (var account in accounts)
        {
            Console.WriteLine($"Account Number: {account.AccountNumber}");
            Console.WriteLine($"Name: {account.AccountName}");
            Console.WriteLine($"Type: {account.AccountType}");
            Console.WriteLine($"Balance: ₦{account.AccountBalance:N2}");
            Console.WriteLine($"Status: {(account.IsFrozen ? "❄️ Frozen" : "✅ Active")}");
            Console.WriteLine($"Created: {account.DateCreated:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Transactions: {account.TransactionIds.Count} records");
            
            // Find which customer owns this account
            var customers = _customerService.GetAllCustomers();
            var accountOwner = customers.FirstOrDefault(c => c.AccountNumbers.Contains(account.AccountNumber));
            if (accountOwner != null)
            {
                Console.WriteLine($"Owner: {accountOwner.Name} (ID: {accountOwner.Id})");
            }
            else
            {
                Console.WriteLine("Owner: Standalone Account");
            }
            
            Console.WriteLine("-".PadRight(80, '-'));
        }
        
        // Show summary statistics
        var totalBalance = accounts.Sum(a => a.AccountBalance);
        var frozenAccounts = accounts.Count(a => a.IsFrozen);
        var activeAccounts = accounts.Count(a => !a.IsFrozen);
        var currentAccounts = accounts.Count(a => a.AccountType == AccountType.Current);
        var savingAccounts = accounts.Count(a => a.AccountType == AccountType.Saving);
        
        Console.WriteLine("\n📊 ACCOUNT SUMMARY");
        Console.WriteLine("==================");
        Console.WriteLine($"Total Balance: ₦{totalBalance:N2}");
        Console.WriteLine($"Active Accounts: {activeAccounts}");
        Console.WriteLine($"Frozen Accounts: {frozenAccounts}");
        Console.WriteLine($"Current Accounts: {currentAccounts}");
        Console.WriteLine($"Saving Accounts: {savingAccounts}");
    }

    static void ViewAccountsByCustomer()
    {
        Console.WriteLine("\n💳 VIEW ACCOUNTS BY CUSTOMER");
        Console.WriteLine("============================");
        
        Console.Write("Enter customer name: ");
        var customerName = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(customerName))
        {
            Console.WriteLine("❌ Customer name cannot be empty.");
            return;
        }

        var customer = _customerService.GetCustomer(customerName);
        if (customer == null)
        {
            Console.WriteLine("❌ Customer not found.");
            return;
        }

        var customerAccounts = _customerService.GetCustomerAccounts(customer.Id);
        
        Console.WriteLine($"\n👤 Customer: {customer.Name}");
        Console.WriteLine($"📧 Username: {customer.Username}");
        Console.WriteLine($"📞 Phone: {customer.PhoneNumber}");
        Console.WriteLine($"🏠 Address: {customer.Address}");
        Console.WriteLine("=".PadRight(80, '='));
        
        if (!customerAccounts.Any())
        {
            Console.WriteLine("No accounts found for this customer.");
            return;
        }

        Console.WriteLine($"Total Accounts: {customerAccounts.Count()}");
        Console.WriteLine("-".PadRight(80, '-'));
        
        var totalBalance = 0m;
        foreach (var account in customerAccounts)
        {
            Console.WriteLine($"Account Number: {account.AccountNumber}");
            Console.WriteLine($"Account Name: {account.AccountName}");
            Console.WriteLine($"Type: {account.AccountType}");
            Console.WriteLine($"Balance: ₦{account.AccountBalance:N2}");
            Console.WriteLine($"Status: {(account.IsFrozen ? "❄️ Frozen" : "✅ Active")}");
            Console.WriteLine($"Created: {account.DateCreated:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Transactions: {account.TransactionIds.Count} records");
            
            totalBalance += account.AccountBalance;
            Console.WriteLine("-".PadRight(80, '-'));
        }
        
        // Show customer summary
        Console.WriteLine("\n📊 CUSTOMER ACCOUNT SUMMARY");
        Console.WriteLine("===========================");
        Console.WriteLine($"Total Balance: ₦{totalBalance:N2}");
        Console.WriteLine($"Average Balance: ₦{(totalBalance / customerAccounts.Count()):N2}");
        Console.WriteLine($"Current Accounts: {customerAccounts.Count(a => a.AccountType == AccountType.Current)}");
        Console.WriteLine($"Saving Accounts: {customerAccounts.Count(a => a.AccountType == AccountType.Saving)}");
        Console.WriteLine($"Active Accounts: {customerAccounts.Count(a => !a.IsFrozen)}");
        Console.WriteLine($"Frozen Accounts: {customerAccounts.Count(a => a.IsFrozen)}");
    }

    static void FindAccount()
    {
        Console.Write("\nEnter account number to find: ");
        var accountNumber = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Console.WriteLine("❌ Account number cannot be empty.");
            return;
        }

        var account = _accountRepo.GetByAccountNumber(accountNumber);
        if (account == null)
        {
            Console.WriteLine("❌ Account not found.");
            return;
        }

        Console.WriteLine($"\nFound Account:");
        Console.WriteLine($"Number: {account.AccountNumber}");
        Console.WriteLine($"Name: {account.AccountName}");
        Console.WriteLine($"Type: {account.AccountType}");
        Console.WriteLine($"Balance: ₦{account.AccountBalance}");
        Console.WriteLine($"Status: {(account.IsFrozen ? "Frozen" : "Active")}");
        Console.WriteLine($"Created: {account.DateCreated}");
    }

    static void ToggleAccountFreeze()
    {
        Console.Write("\nEnter account number: ");
        var accountNumber = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Console.WriteLine("❌ Account number cannot be empty.");
            return;
        }

        var account = _accountRepo.GetByAccountNumber(accountNumber);
        if (account == null)
        {
            Console.WriteLine("❌ Account not found.");
            return;
        }

        if (account.IsFrozen)
        {
            account.UnfreezeAccount();
            _accountRepo.Update(account);
            Console.WriteLine("✅ Account unfrozen successfully!");
        }
        else
        {
            account.FreezeAccount();
            _accountRepo.Update(account);
            Console.WriteLine("✅ Account frozen successfully!");
        }
    }

    static void DeleteAccount()
    {
        Console.Write("\nEnter account number to delete: ");
        var accountNumber = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Console.WriteLine("❌ Account number cannot be empty.");
            return;
        }

        Console.Write($"Are you sure you want to delete account '{accountNumber}'? (y/N): ");
        var confirm = Console.ReadLine()?.ToLower();
        
        if (confirm == "y" || confirm == "yes")
        {
            _accountRepo.Delete(accountNumber);
            Console.WriteLine("✅ Account deleted successfully!");
        }
        else
        {
            Console.WriteLine("❌ Deletion cancelled.");
        }
    }

    // Transaction Operations
    static void DepositMoney()
    {
        Console.WriteLine("\n💰 DEPOSIT MONEY");
        Console.WriteLine("===============");
        
        Console.Write("Account Number: ");
        var accountNumber = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Console.WriteLine("❌ Account number cannot be empty.");
            return;
        }

        Console.Write("Amount: ₦");
        if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
        {
            Console.WriteLine("❌ Invalid amount. Must be positive.");
            return;
        }

        try
        {
            _accountService.Deposit(accountNumber, amount);
            var account = _accountRepo.GetByAccountNumber(accountNumber);
            if (account != null) Console.WriteLine($"✅ Deposit successful! New balance: ₦{account.AccountBalance}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error during deposit: {ex.Message}");
        }
    }

    static void WithdrawMoney()
    {
        Console.WriteLine("\n💰 WITHDRAW MONEY");
        Console.WriteLine("================");
        
        Console.Write("Account Number: ");
        var accountNumber = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Console.WriteLine("❌ Account number cannot be empty.");
            return;
        }

        Console.Write("Amount: ₦");
        if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
        {
            Console.WriteLine("❌ Invalid amount. Must be positive.");
            return;
        }

        try
        {
            _accountService.Withdraw(accountNumber, amount);
            var account = _accountRepo.GetByAccountNumber(accountNumber);
            Console.WriteLine($"✅ Withdrawal successful! New balance: ₦{account.AccountBalance}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error during withdrawal: {ex.Message}");
        }
    }

    static void TransferMoney()
    {
        Console.WriteLine("\n💰 TRANSFER MONEY");
        Console.WriteLine("================");

        string sourceAccount;
        while (true)
        {

            Console.Write("Source Account Number: ");
            string input = Console.ReadLine() ?? string.Empty;
        
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("❌ Source account number cannot be empty.");
            } 
            else if (!_customerRepo.GetCustomerById(_session_customer_id)!.AccountNumbers.Contains(input))
            {
                Console.WriteLine("❌ Source account number not found.");
            }
            else
            {
                sourceAccount = input;
                break;
            }
        }
        
        string destinationAccountNo;
        while (true)
        {

            Console.Write("Destination Account Number: ");
            string input = Console.ReadLine() ?? string.Empty;
            var destinationAccount = _accountRepo.GetByAccountNumber(input);
            destinationAccountNo = input;
        
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("❌ Destination account number cannot be empty.");
            }
            else if (string.IsNullOrWhiteSpace(destinationAccount?.AccountNumber))
            {
                 Console.WriteLine("❌ Destination account number not found. Try again.");
            }
            else
            {
                Console.WriteLine($"Destination account name is {destinationAccount.AccountName}. Please make sure name corresponds with the account number.");
                break;
            }
        }

        decimal amount;
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
            if (amount < 100)
            {
                Console.WriteLine("❌ Minimum transfer amount is ₦100.");
                continue;
            }
            break;
        }

        try
        {
            _accountService.Transfer(sourceAccount, destinationAccountNo, amount);
            var source = _accountRepo.GetByAccountNumber(sourceAccount);
            var destination = _accountRepo.GetByAccountNumber(destinationAccountNo);
            Console.WriteLine($"✅ Transfer successful!");
            Console.WriteLine($"Source balance: ₦{source?.AccountBalance}");
            Console.WriteLine($"Destination balance: ₦{destination?.AccountBalance}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error during transfer: {ex.Message}");
        }
    }

    static void ViewTransactionHistory()
    {
        Console.Write("\nEnter account number to view transactions: ");
        var accountNumber = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Console.WriteLine("❌ Account number cannot be empty.");
            return;
        }

        var account = _accountRepo.GetByAccountNumber(accountNumber);
        if (account == null)
        {
            Console.WriteLine("❌ Account not found.");
            return;
        }

        Console.WriteLine($"\n📋 TRANSACTION HISTORY - {account.AccountNumber}");
        Console.WriteLine("==========================================");
        
        var transactions = _transactionService.GetTransactionsByAccount(accountNumber);
        if (!transactions.Any())
        {
            Console.WriteLine("No transactions found.");
            return;
        }

        foreach (var transaction in transactions.OrderByDescending(t => t.DateCreated))
        {
            Console.WriteLine($"Date: {transaction.DateCreated}");
            Console.WriteLine($"Type: {transaction.TransactionType}");
            Console.WriteLine($"Amount: ₦{transaction.Amount}");
            Console.WriteLine($"Description: {transaction.Description}");
            Console.WriteLine($"Balance After: ₦{transaction.BalanceAfterTransaction}");
            Console.WriteLine("---");
        }
    }

    static void ViewAccountBalance()
    {
        Console.Write("\nEnter account number: ");
        var accountNumber = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Console.WriteLine("❌ Account number cannot be empty.");
            return;
        }

        var account = _accountRepo.GetByAccountNumber(accountNumber);
        if (account == null)
        {
            Console.WriteLine("❌ Account not found.");
            return;
        }

        Console.WriteLine($"\n💰 ACCOUNT BALANCE");
        Console.WriteLine("==================");
        Console.WriteLine($"Account: {account.AccountNumber}");
        Console.WriteLine($"Name: {account.AccountName}");
        Console.WriteLine($"Type: {Enum.GetName(typeof(AccountType), account.AccountType)}");
        Console.WriteLine($"Balance: ₦{account.AccountBalance}");
        Console.WriteLine($"Status: {(account.IsFrozen ? "Frozen" : "Active")}");
    }

    // Database Operations
    static void CreateBackup()
    {
        Console.Write("\nEnter backup directory path: ");
        var backupPath = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(backupPath))
        {
            Console.WriteLine("❌ Backup path cannot be empty.");
            return;
        }

        try
        {
            DatabaseManager.BackupDatabase(backupPath);
            Console.WriteLine("✅ Backup created successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating backup: {ex.Message}");
        }
    }

    static void RestoreBackup()
    {
        Console.Write("\nEnter backup directory path: ");
        var backupPath = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(backupPath))
        {
            Console.WriteLine("❌ Backup path cannot be empty.");
            return;
        }

        Console.Write("Are you sure you want to restore from backup? This will overwrite current data. (y/N): ");
        var confirm = Console.ReadLine()?.ToLower();
        
        if (confirm == "y" || confirm == "yes")
        {
            try
            {
                DatabaseManager.RestoreDatabase(backupPath);
                Console.WriteLine("✅ Database restored successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error restoring backup: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("❌ Restore cancelled.");
        }
    }

    static void ClearAllData()
    {
        Console.Write("Are you sure you want to clear all data? This action cannot be undone. (y/N): ");
        var confirm = Console.ReadLine()?.ToLower();
        
        if (confirm == "y" || confirm == "yes")
        {
            DatabaseManager.ClearDatabase();
            Console.WriteLine("✅ All data cleared successfully!");
        }
        else
        {
            Console.WriteLine("❌ Clear operation cancelled.");
        }
    }

    static void ViewDatabaseInfo()
    {
        var stats = DatabaseManager.GetDatabaseStats();
        
        Console.WriteLine("\n🗄️ DATABASE INFORMATION");
        Console.WriteLine("=======================");
        Console.WriteLine($"Total Accounts: {stats.TotalAccounts}");
        Console.WriteLine($"Total Customers: {stats.TotalCustomers}");
        Console.WriteLine($"Total Account Managers: {stats.TotalAccountManagers}");
        Console.WriteLine($"Total Transactions: {stats.TotalTransactions}");
        Console.WriteLine($"Database Size: {stats.DatabaseSize} bytes");
        Console.WriteLine($"Last Modified: {stats.LastModified}");
        Console.WriteLine($"Database Path: {JsonDatabase.GetDatabasePath()}");
    }

    static void ShowStatistics()
    {
        var stats = DatabaseManager.GetDatabaseStats();
        
        Console.WriteLine("\n📊 SYSTEM STATISTICS");
        Console.WriteLine("===================");
        Console.WriteLine($"Total Accounts: {stats.TotalAccounts}");
        Console.WriteLine($"Total Customers: {stats.TotalCustomers}");
        Console.WriteLine($"Total Account Managers: {stats.TotalAccountManagers}");
        Console.WriteLine($"Total Transactions: {stats.TotalTransactions}");
        Console.WriteLine($"Database Size: {stats.DatabaseSize} bytes");
        Console.WriteLine($"Last Modified: {stats.LastModified}");
        
        if (stats.TotalAccounts > 0)
        {
            var accounts = _accountRepo.GetAll();
            var totalBalance = accounts.Sum(a => a.AccountBalance);
            var avgBalance = totalBalance / stats.TotalAccounts;
            
            Console.WriteLine($"\n💰 FINANCIAL SUMMARY");
            Console.WriteLine("===================");
            Console.WriteLine($"Total Balance: ₦{totalBalance:N2}");
            Console.WriteLine($"Average Balance: ₦{avgBalance:N2}");
            Console.WriteLine($"Frozen Accounts: {accounts.Count(a => a.IsFrozen)}");
            Console.WriteLine($"Active Accounts: {accounts.Count(a => !a.IsFrozen)}");
        }
    }

    static void ShowAccountManagerMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n👨‍💼 ACCOUNT MANAGER MANAGEMENT");
            Console.WriteLine("=============================");
            Console.WriteLine("1. Create New Account Manager");
            Console.WriteLine("2. View All Account Managers");
            Console.WriteLine("3. Find Account Manager");
            Console.WriteLine("4. Update Account Manager");
            Console.WriteLine("5. Delete Account Manager");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    CreateAccountManager();
                    break;
                case "2":
                    ViewAllAccountManagers();
                    break;
                case "3":
                    FindAccountManager();
                    break;
                case "4":
                    UpdateAccountManager();
                    break;
                case "5":
                    DeleteAccountManager();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Invalid option. Please try again.");
                    break;
            }
        }
    }
}
