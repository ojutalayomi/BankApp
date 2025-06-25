using BankApp.Abstractions;

namespace BankApp.Core.Services;

/// <summary>
/// Provides business logic services for managing bank accounts and transactions.
/// </summary>
/// <remarks>
/// This service class implements the IAccountService interface and provides
/// high-level business operations for account management including deposits,
/// withdrawals, transfers, and account validation.
/// </remarks>
public class AccountService : IAccountService
{
    /// <summary>
    /// The repository for account data access operations.
    /// </summary>
    private readonly IAccountRepository _accountRepo;

    /// <summary>
    /// Initializes a new instance of the AccountService class.
    /// </summary>
    /// <param name="accountRepo">The account repository for data access.</param>
    /// <remarks>
    /// This constructor injects the account repository dependency
    /// to enable data access operations.
    /// </remarks>
    public AccountService(IAccountRepository accountRepo)
    {
        _accountRepo = accountRepo;
    }

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
    public void Deposit(string accountNumber, decimal amount)
    {
        var account = _accountRepo.GetByAccountNumber(accountNumber);
        
        if (account == null)
            throw new ArgumentException($"Account with number {accountNumber} not found.");
        
        if (account.IsFrozen)
            throw new InvalidOperationException("Account is frozen.");
        
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be positive.");
        
        account.Deposit(amount);
        _accountRepo.Update(account);
    }

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
    public void Transfer(string sourceAccountNumber, string destinationAccountNumber, decimal amount)
    {
        var sourceAccount = _accountRepo.GetByAccountNumber(sourceAccountNumber);
        var destinationAccount = _accountRepo.GetByAccountNumber(destinationAccountNumber);
        
        if (sourceAccount == null)
            throw new ArgumentException($"Source account with number {sourceAccountNumber} not found.");
        
        if (destinationAccount == null)
            throw new ArgumentException($"Destination account with number {destinationAccountNumber} not found.");
        
        if (amount <= 0)
            throw new ArgumentException("Transfer amount must be positive.");
        
        if (sourceAccount.IsFrozen)
            throw new InvalidOperationException("Source account is frozen.");
        
        if (destinationAccount.IsFrozen)
            throw new InvalidOperationException("Destination account is frozen.");
        
        if (sourceAccount.AccountBalance < amount)
            throw new InvalidOperationException("Insufficient funds.");
        
        sourceAccount.Transfer(amount, destinationAccount);
        
        _accountRepo.Update(sourceAccount);
        _accountRepo.Update(destinationAccount);
    }

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
    public void Withdraw(string accountNumber, decimal amount)
    {
        var account = _accountRepo.GetByAccountNumber(accountNumber);
        
        if (account == null)
            throw new ArgumentException($"Account with number {accountNumber} not found.");
        
        if (account.IsFrozen)
            throw new InvalidOperationException("Account is frozen.");
        
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.");
        
        if (account.AccountBalance < amount)
            throw new InvalidOperationException("Insufficient funds.");

        account.Withdraw(amount);
        _accountRepo.Update(account);
    }
} 