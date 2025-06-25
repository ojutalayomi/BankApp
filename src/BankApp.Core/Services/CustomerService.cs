using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Services;

/// <summary>
/// Provides business logic services for managing customers and their accounts.
/// </summary>
/// <remarks>
/// This service class implements the ICustomerService interface and provides
/// high-level business operations for customer management including creation,
/// account linking, and customer data operations.
/// </remarks>
public class CustomerService : ICustomerService
{
    /// <summary>
    /// The repository for customer data access operations.
    /// </summary>
    private readonly ICustomerRepository _customerRepo;
    
    /// <summary>
    /// The repository for account data access operations.
    /// </summary>
    private readonly IAccountRepository _accountRepo;

    /// <summary>
    /// Initializes a new instance of the CustomerService class.
    /// </summary>
    /// <param name="customerRepo">The customer repository for data access.</param>
    /// <param name="accountRepo">The account repository for data access.</param>
    /// <remarks>
    /// This constructor injects both customer and account repository dependencies
    /// to enable comprehensive customer and account management operations.
    /// </remarks>
    public CustomerService(ICustomerRepository customerRepo, IAccountRepository accountRepo)
    {
        _customerRepo = customerRepo;
        _accountRepo = accountRepo;
    }

    /// <summary>
    /// Creates a new customer with a default current account.
    /// </summary>
    /// <param name="customer">The customer to create.</param>
    /// <remarks>
    /// This method creates a new customer and automatically creates a default
    /// current account for them. The account is linked to the customer and
    /// both are saved to their respective repositories.
    /// </remarks>
    public void CreateCustomer(Customer customer)
    {
        // Create a default current account for the customer
        var defaultAccount = new Account($"{customer.Name}'s Current Account", AccountType.Current);
        
        // Add the account to the accounts repository
        _accountRepo.Add(defaultAccount);
        
        // Add the account number to the customer's account list
        customer.AddAccount(defaultAccount.AccountNumber);
        
        // Save the customer
        _customerRepo.Add(customer);
    }

    /// <summary>
    /// Updates an existing customer's information.
    /// </summary>
    /// <param name="customer">The customer with updated information.</param>
    /// <remarks>
    /// This method updates the customer's information in the repository.
    /// No validation is performed as the customer should already exist.
    /// </remarks>
    public void UpdateCustomer(Customer customer)
    {
        _customerRepo.Update(customer);
    }

    /// <summary>
    /// Retrieves a customer by their name.
    /// </summary>
    /// <param name="customerName">The name of the customer to find.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for a customer using their full name.
    /// This is commonly used for customer lookup operations.
    /// </remarks>
    public Customer? GetCustomer(string customerName)
    {
        return _customerRepo.GetCustomer(customerName);
    }

    /// <summary>
    /// Retrieves all customers from the system.
    /// </summary>
    /// <returns>An enumerable collection of all customers.</returns>
    /// <remarks>
    /// This method returns all customers in the system regardless of their status.
    /// </remarks>
    public IEnumerable<Customer> GetAllCustomers()
    {
        return _customerRepo.GetAll();
    }

    /// <summary>
    /// Deletes a customer from the system.
    /// </summary>
    /// <param name="customerName">The name of the customer to delete.</param>
    /// <remarks>
    /// This method removes the customer from the repository if they exist.
    /// If the customer doesn't exist, no action is taken.
    /// Note: This does not delete the customer's accounts, only the customer record.
    /// </remarks>
    public void DeleteCustomer(string customerName)
    {
        var customer = _customerRepo.GetCustomer(customerName);
        if (customer != null)
        {
            _customerRepo.Delete(customerName);
        }
    }

    /// <summary>
    /// Retrieves all accounts associated with a specific customer.
    /// </summary>
    /// <param name="customerName">The name of the customer whose accounts to retrieve.</param>
    /// <returns>An enumerable collection of accounts associated with the customer.</returns>
    /// <remarks>
    /// This method finds the customer by name and then retrieves all accounts
    /// that are linked to that customer through their account numbers.
    /// Returns an empty collection if the customer is not found.
    /// </remarks>
    public IEnumerable<Account> GetCustomerAccounts(string customerName)
    {
        var customer = _customerRepo.GetCustomer(customerName);
        if (customer == null)
            return Enumerable.Empty<Account>();

        var accounts = new List<Account>();
        foreach (var accountNumber in customer.AccountNumbers)
        {
            var account = _accountRepo.GetByAccountNumber(accountNumber);
            if (account != null)
            {
                accounts.Add(account);
            }
        }
        return accounts;
    }
} 