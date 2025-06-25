namespace BankApp.Abstractions;

/// <summary>
/// Defines the contract for customer data access operations.
/// </summary>
/// <remarks>
/// This interface provides methods for managing customer entities in the data store,
/// including CRUD operations and specialized queries for customer lookup and validation.
/// </remarks>
public interface ICustomerRepository
{
    /// <summary>
    /// Retrieves all customers from the data store.
    /// </summary>
    /// <returns>An enumerable collection of all customers.</returns>
    IEnumerable<Customer> GetAll();
    
    /// <summary>
    /// Retrieves a customer by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    Customer? GetById(string id);
    
    /// <summary>
    /// Retrieves a customer by their username.
    /// </summary>
    /// <param name="username">The username of the customer.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    Customer? GetCustomer(string username);
    
    /// <summary>
    /// Retrieves a customer by their full name.
    /// </summary>
    /// <param name="customerName">The full name of the customer.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    Customer? GetByName(string customerName);
    
    /// <summary>
    /// Adds a new customer to the data store.
    /// </summary>
    /// <param name="customer">The customer to add.</param>
    void Add(Customer customer);
    
    /// <summary>
    /// Updates an existing customer in the data store.
    /// </summary>
    /// <param name="customer">The customer with updated information.</param>
    void Update(Customer customer);
    
    /// <summary>
    /// Removes a customer from the data store by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete.</param>
    void Delete(string id);
    
    /// <summary>
    /// Checks if a customer exists in the data store by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <returns>True if the customer exists; otherwise, false.</returns>
    bool Exists(string id);
} 