namespace BankApp.Core.Services;

/// <summary>
/// Defines the contract for account business operations.
/// </summary>
/// <remarks>
/// This interface provides methods for managing bank accounts including deposits,
/// withdrawals, transfers, and account validation. It serves as the contract
/// for the AccountService implementation.
/// </remarks>
public interface IAccountService
{
    /// <summary>
    /// Deposits money into a specified account.
    /// </summary>
    /// <param name="accountNumber">The account number to deposit money into.</param>
    /// <param name="amount">The amount to deposit.</param>
    /// <exception cref="ArgumentException">Thrown when the account is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the account is frozen.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the amount is not positive.</exception>
    /// <remarks>
    /// This method validates the account exists, is not frozen, and the amount is positive
    /// before performing the deposit operation. The account balance is updated and
    /// a transaction record is created.
    /// </remarks>
    void Deposit(string accountNumber, decimal amount);
    
    /// <summary>
    /// Transfers money from one account to another.
    /// </summary>
    /// <param name="sourceAccountNumber">The account number to transfer money from.</param>
    /// <param name="destinationAccountNumber">The account number to transfer money to.</param>
    /// <param name="amount">The amount to transfer.</param>
    /// <exception cref="ArgumentException">Thrown when either account is not found or amount is not positive.</exception>
    /// <exception cref="InvalidOperationException">Thrown when either account is frozen or source account has insufficient funds.</exception>
    /// <remarks>
    /// This method validates both accounts exist, are not frozen, the amount is positive,
    /// and the source account has sufficient funds before performing the transfer.
    /// Both accounts are updated and transaction records are created for both accounts.
    /// </remarks>
    void Transfer(string sourceAccountNumber, string destinationAccountNumber, decimal amount);
    
    /// <summary>
    /// Withdraws money from a specified account.
    /// </summary>
    /// <param name="accountNumber">The account number to withdraw money from.</param>
    /// <param name="amount">The amount to withdraw.</param>
    /// <exception cref="ArgumentException">Thrown when the account is not found or amount is not positive.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the account is frozen or has insufficient funds.</exception>
    /// <remarks>
    /// This method validates the account exists, is not frozen, the amount is positive,
    /// and the account has sufficient funds before performing the withdrawal.
    /// The account balance is updated and a transaction record is created.
    /// </remarks>
    void Withdraw(string accountNumber, decimal amount);
} 