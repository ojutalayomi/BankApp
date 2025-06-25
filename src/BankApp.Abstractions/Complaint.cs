using BankApp.Abstractions.Enums;

namespace BankApp.Abstractions;

/// <summary>
/// Represents a customer complaint with tracking and resolution capabilities.
/// </summary>
/// <remarks>
/// This class manages customer complaints throughout their lifecycle, from creation
/// to resolution. It tracks the complaint status, resolution details, and timing
/// information for both creation and resolution.
/// </remarks>
public class Complaint
{
    /// <summary>
    /// Gets or sets the unique identifier for the complaint.
    /// </summary>
    /// <value>A string representing the complaint's unique ID.</value>
    public string Id { get; set; }
    
    /// <summary>
    /// Gets or sets the description or details of the complaint.
    /// </summary>
    /// <value>A string containing the complaint description.</value>
    public string Narration { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the complaint was created.
    /// </summary>
    /// <value>A DateTime representing when the complaint was filed.</value>
    public DateTime TimeCreated { get; set; }
    
    /// <summary>
    /// Gets or sets the current status of the complaint.
    /// </summary>
    /// <value>A ComplaintStatus enum value indicating the complaint's current state.</value>
    public ComplaintStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the customer who filed the complaint.
    /// </summary>
    /// <value>A string representing the customer's ID.</value>
    public string CustomerId { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the account related to the complaint, if applicable.
    /// </summary>
    /// <value>A string representing the account ID, or null if not applicable.</value>
    public string? AccountId { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the account manager assigned to handle the complaint.
    /// </summary>
    /// <value>A string representing the account manager's ID.</value>
    public string AccountManagerId { get; set; }
    
    /// <summary>
    /// Gets or sets the details of how the complaint was resolved.
    /// </summary>
    /// <value>A string containing the resolution details.</value>
    public string ResolutionDetails { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the complaint was resolved.
    /// </summary>
    /// <value>A DateTime representing when the complaint was resolved, or null if not yet resolved.</value>
    public DateTime? TimeResolved { get; set; }

    /// <summary>
    /// Initializes a new instance of the Complaint class with default values.
    /// </summary>
    /// <remarks>
    /// This constructor is primarily used for JSON deserialization.
    /// The complaint is initialized with Pending status and empty values.
    /// </remarks>
    public Complaint()
    {
        Id = Guid.NewGuid().ToString();
        Narration = string.Empty;
        TimeCreated = DateTime.Now;
        Status = ComplaintStatus.Pending;
        CustomerId = string.Empty;
        AccountManagerId = string.Empty;
        ResolutionDetails = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the Complaint class with specified narration and customer ID.
    /// </summary>
    /// <param name="narration">The description or details of the complaint.</param>
    /// <param name="customerId">The identifier of the customer who filed the complaint.</param>
    /// <remarks>
    /// This constructor creates a new complaint with Pending status and automatically
    /// sets the creation time to the current date and time.
    /// </remarks>
    public Complaint(string narration, string customerId)
    {
        Id = Guid.NewGuid().ToString();
        Narration = narration;
        TimeCreated = DateTime.Now;
        Status = ComplaintStatus.Pending;
        CustomerId = customerId;
        AccountManagerId = string.Empty;
        ResolutionDetails = string.Empty;
    }
} 