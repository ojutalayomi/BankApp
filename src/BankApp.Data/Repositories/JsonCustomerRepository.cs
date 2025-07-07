using BankApp.Abstractions;
using BankApp.Data.JsonStorage;

namespace BankApp.Data.Repositories;

/// <summary>
/// Provides JSON-based persistence for Customer entities.
/// </summary>
/// <remarks>
/// This repository implements the ICustomerRepository interface using JSON files
/// for data storage. It provides CRUD operations for Customer entities with
/// automatic serialization and deserialization of data to/from JSON format.
/// </remarks>
public class JsonCustomerRepository : ICustomerRepository
{
    /// <summary>
    /// Retrieves a customer by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    /// <remarks>
    /// This method searches for a customer using their unique ID.
    /// It loads all customers from the JSON file and searches for a match.
    /// </remarks>
    public Customer? GetCustomerById(string id)
    {
        var customers = JsonDatabase.LoadCustomers();
        return customers.FirstOrDefault(c => c.Id == id);
    }

    /// <summary>
    /// Retrieves a customer by their username.
    /// </summary>
    /// <param name="username">The username of the customer.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    /// <remarks>
    /// This method loads all customers from the JSON file and searches for a customer
    /// with the specified username. This is commonly used for authentication purposes.
    /// </remarks>
    public Customer? GetCustomer(string username)
    {
        var customers = JsonDatabase.LoadCustomers();
        return customers.FirstOrDefault(c => c.Username == username);
    }

    /// <summary>
    /// Retrieves a customer by their full name.
    /// </summary>
    /// <param name="customerName">The full name of the customer.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    /// <remarks>
    /// This method loads all customers from the JSON file and searches for a customer
    /// with the specified full name. This is useful for customer lookup operations.
    /// </remarks>
    public Customer? GetByName(string customerName)
    {
        var customers = JsonDatabase.LoadCustomers();
        return customers.FirstOrDefault(c => c.Name == customerName);
    }

    /// <summary>
    /// Updates an existing customer in the JSON storage.
    /// </summary>
    /// <param name="customer">The customer with updated information.</param>
    /// <remarks>
    /// This method loads all customers, finds the customer to update by ID,
    /// replaces it with the updated version, and saves the changes back to the JSON file.
    /// </remarks>
    public void Update(Customer customer)
    {
        var customers = JsonDatabase.LoadCustomers();
        var existingIndex = customers.FindIndex(c => c.Id == customer.Id);
        
        if (existingIndex != -1)
        {
            customers[existingIndex] = customer;
            JsonDatabase.SaveCustomers(customers);
        }
    }

    /// <summary>
    /// Adds a new customer to the JSON storage.
    /// </summary>
    /// <param name="customer">The customer to add.</param>
    /// <remarks>
    /// This method loads all existing customers, adds the new customer to the list,
    /// and saves the updated list back to the JSON file.
    /// </remarks>
    public void Add(Customer customer)
    {
        var customers = JsonDatabase.LoadCustomers();
        customers.Add(customer);
        JsonDatabase.SaveCustomers(customers);
    }

    /// <summary>
    /// Removes a customer from the JSON storage by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete.</param>
    /// <remarks>
    /// This method loads all customers, finds the customer to delete by ID,
    /// removes it from the list, and saves the updated list back to the JSON file.
    /// </remarks>
    public void Delete(string id)
    {
        var customers = JsonDatabase.LoadCustomers();
        var customerToRemove = customers.FirstOrDefault(c => c.Id == id);
        
        if (customerToRemove != null)
        {
            customers.Remove(customerToRemove);
            JsonDatabase.SaveCustomers(customers);
        }
    }

    /// <summary>
    /// Retrieves all customers from the JSON storage.
    /// </summary>
    /// <returns>An enumerable collection of all customers.</returns>
    /// <remarks>
    /// This method loads and returns all customers stored in the JSON file.
    /// </remarks>
    public IEnumerable<Customer> GetAll()
    {
        return JsonDatabase.LoadCustomers();
    }

    /// <summary>
    /// Checks if a customer exists in the JSON storage by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <returns>True if the customer exists; otherwise, false.</returns>
    /// <remarks>
    /// This method loads all customers and checks if any customer has the specified ID.
    /// </remarks>
    public bool Exists(string id)
    {
        var customers = JsonDatabase.LoadCustomers();
        return customers.Any(c => c.Id == id);
    }
} 