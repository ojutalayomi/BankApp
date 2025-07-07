using BankApp.Abstractions;
using BankApp.Abstractions.Enums;

namespace BankApp.Core.Services;

/// <summary>
/// Defines the contract for customer business operations.
/// </summary>
/// <remarks>
/// This interface provides methods for managing customers including creation,
/// account linking, and customer data operations. It serves as the contract
/// for the CustomerService implementation.
/// </remarks>
public interface ICustomerService
{
    /// <summary>
    /// Creates a new customer with a default current account.
    /// </summary>
    /// <param name="customer">The customer to create.</param>
    /// <param name="accountType">The accountType to create.</param>
    /// <remarks>
    /// This method creates a new customer and automatically creates a default
    /// current account for them. The account is linked to the customer and
    /// both are saved to their respective repositories.
    /// </remarks>
    void CreateCustomer(Customer customer, AccountType? accountType);
    
    /// <summary>
    /// Updates an existing customer's information.
    /// </summary>
    /// <param name="customer">The customer with updated information.</param>
    /// <remarks>
    /// This method updates the customer's information in the repository.
    /// No validation is performed as the customer should already exist.
    /// </remarks>
    void UpdateCustomer(Customer customer);
    
    /// <summary>
    /// Retrieves a customer by their name.
    /// </summary>
    /// <param name="customerName">The name of the customer to find.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for a customer using their full name.
    /// This is commonly used for customer lookup operations.
    /// </remarks>
    Customer? GetCustomer(string customerName);
    
    /// <summary>
    /// Retrieves all customers from the system.
    /// </summary>
    /// <returns>An enumerable collection of all customers.</returns>
    /// <remarks>
    /// This method returns all customers in the system regardless of their status.
    /// </remarks>
    IEnumerable<Customer> GetAllCustomers();
    
    /// <summary>
    /// Deletes a customer from the system.
    /// </summary>
    /// <param name="customerName">The name of the customer to delete.</param>
    /// <remarks>
    /// This method removes the customer from the repository if they exist.
    /// If the customer doesn't exist, no action is taken.
    /// Note: This does not delete the customer's accounts, only the customer record.
    /// </remarks>
    void DeleteCustomer(string customerName);
    
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
    IEnumerable<Account> GetCustomerAccounts(string customerName);
} 