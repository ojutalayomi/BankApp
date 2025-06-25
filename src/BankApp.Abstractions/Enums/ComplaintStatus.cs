namespace BankApp.Abstractions.Enums;

/// <summary>
/// Represents the different states a complaint can be in during its lifecycle.
/// </summary>
/// <remarks>
/// This enum tracks the progress of customer complaints from initial filing
/// through to final resolution or closure.
/// </remarks>
public enum ComplaintStatus
{
    /// <summary>
    /// The complaint has been filed but not yet reviewed or assigned.
    /// </summary>
    /// <remarks>
    /// This is the initial state when a customer first files a complaint.
    /// The complaint is waiting for an account manager to review and take action.
    /// </remarks>
    Pending,
    
    /// <summary>
    /// The complaint is being actively worked on by an account manager.
    /// </summary>
    /// <remarks>
    /// An account manager has been assigned and is investigating or working
    /// towards resolving the complaint.
    /// </remarks>
    InProgress,
    
    /// <summary>
    /// The complaint has been successfully resolved.
    /// </summary>
    /// <remarks>
    /// The issue has been addressed and the customer has been notified
    /// of the resolution. The complaint is considered closed.
    /// </remarks>
    Resolved,
    
    /// <summary>
    /// The complaint has been closed without resolution or is no longer active.
    /// </summary>
    /// <remarks>
    /// This status is used when a complaint cannot be resolved, is withdrawn,
    /// or is closed for other reasons.
    /// </remarks>
    Closed
} 