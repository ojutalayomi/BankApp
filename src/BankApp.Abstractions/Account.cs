using BankApp.Abstractions.Enums;

namespace BankApp.Abstractions;

/// <summary>
/// Represents a bank account with transaction capabilities and status management.
/// </summary>
/// <remarks>
/// This class provides core banking functionality including deposits, withdrawals, transfers,
/// and account status management. Each account maintains its own transaction history
/// and can be frozen/unfrozen by account managers.
/// </remarks>
public class Account
{
    /// <summary>
    /// Gets or sets the name of the account.
    /// </summary>
    /// <value>A string representing the account name.</value>
    public string AccountName { get; set; }
    
    /// <summary>
    /// Gets or sets the unique account number.
    /// </summary>
    /// <value>A string representing the account number.</value>
    public string AccountNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the type of account (Current or Saving).
    /// </summary>
    /// <value>An AccountType enum value.</value>
    public AccountType AccountType { get; set; }
    
    /// <summary>
    /// Gets or sets the current balance of the account.
    /// </summary>
    /// <value>A decimal representing the account balance in currency units.</value>
    public decimal AccountBalance { get; set; }
    
    /// <summary>
    /// Gets or sets the list of transactions associated with this account.
    /// </summary>
    /// <value>A list of Transaction objects representing the account's transaction history.</value>
    public List<Transaction> TransactionHistory { get; set; }
    
    /// <summary>
    /// Gets or sets the date when the account was created.
    /// </summary>
    /// <value>A DateTime representing the account creation date.</value>
    public DateTime DateCreated { get; set; }
    
    /// <summary>
    /// Gets a value indicating whether the account is frozen.
    /// </summary>
    /// <value>True if the account is frozen; otherwise, false.</value>
    public bool IsFrozen { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Account class with default values.
    /// </summary>
    /// <remarks>
    /// This constructor is primarily used for JSON deserialization.
    /// </remarks>
    public Account()
    {
        AccountName = string.Empty;
        AccountNumber = string.Empty;
        AccountType = AccountType.Current;
        AccountBalance = 0m;
        TransactionHistory = new List<Transaction>();
        DateCreated = DateTime.Now;
        IsFrozen = false;
    }

    /// <summary>
    /// Initializes a new instance of the Account class with specified name and type.
    /// </summary>
    /// <param name="accountName">The name of the account.</param>
    /// <param name="accountType">The type of account (Current or Saving).</param>
    /// <remarks>
    /// This constructor automatically generates a unique account number using AccountGenerator.
    /// </remarks>
    public Account(string accountName, AccountType accountType)
    {
        AccountName = accountName;
        AccountNumber = AccountGenerator.GenerateAccountNumber();
        AccountType = accountType;
        TransactionHistory = [];
        DateCreated = DateTime.Now;
        IsFrozen = false;
    }

    /// <summary>
    /// Initializes a new instance of the Account class with specified name, account number, and type.
    /// </summary>
    /// <param name="accountName">The name of the account.</param>
    /// <param name="accountNumber">The account number to use.</param>
    /// <param name="accountType">The type of account (Current or Saving).</param>
    /// <remarks>
    /// This constructor allows manual specification of the account number.
    /// </remarks>
    public Account(string accountName, string accountNumber, AccountType accountType)
    {
        AccountName = accountName;
        AccountNumber = accountNumber;
        AccountType = accountType;
        TransactionHistory = [];
        DateCreated = DateTime.Now;
        IsFrozen = false;
    }

    /// <summary>
    /// Deposits the specified amount into the account.
    /// </summary>
    /// <param name="amount">The amount to deposit. Must be positive.</param>
    /// <exception cref="InvalidOperationException">Thrown when the account is frozen.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the amount is not positive.</exception>
    /// <remarks>
    /// This method adds the specified amount to the account balance and creates
    /// a transaction record in the account's transaction history.
    /// </remarks>
    public void Deposit(decimal amount)
    {
        if (IsFrozen)
        {
            throw new InvalidOperationException("Account is frozen.");
        }
        
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be positive.");
        }

        AccountBalance += amount;
        TransactionHistory.Add(new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Deposit,
            Amount = amount,
            SourceAccount = AccountNumber,
            DestinationAccount = "",
            DateCreated = DateTime.Now,
            Status = TransactionStatus.Completed,
            Description = $"Deposit of {amount:C}",
            BalanceAfterTransaction = AccountBalance,
            Initiator = "System",
            Channel = "ATM",
            ReferenceNumber = Guid.NewGuid().ToString()
        });
    }

    /// <summary>
    /// Withdraws the specified amount from the account.
    /// </summary>
    /// <param name="amount">The amount to withdraw. Must be positive.</param>
    /// <exception cref="InvalidOperationException">Thrown when the account is frozen or has insufficient funds.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the amount is not positive.</exception>
    /// <remarks>
    /// This method subtracts the specified amount from the account balance and creates
    /// a transaction record in the account's transaction history.
    /// </remarks>
    public void Withdraw(decimal amount)
    {
        if (IsFrozen)
        {
            throw new InvalidOperationException("Account is frozen.");
        }
        
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Withdrawal amount must be positive.");
        }

        if (AccountBalance < amount)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }

        AccountBalance -= amount;
        TransactionHistory.Add(new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Withdraw,
            Amount = amount,
            SourceAccount = AccountNumber,
            DestinationAccount = "",
            DateCreated = DateTime.Now,
            Status = TransactionStatus.Completed,
            Description = $"Withdrawal of {amount:C}",
            BalanceAfterTransaction = AccountBalance,
            Initiator = "System",
            Channel = "ATM",
            ReferenceNumber = Guid.NewGuid().ToString()
        });
    }

    /// <summary>
    /// Transfers the specified amount from this account to the destination account.
    /// </summary>
    /// <param name="amount">The amount to transfer. Must be positive.</param>
    /// <param name="destinationAccount">The destination account to transfer funds to.</param>
    /// <exception cref="InvalidOperationException">Thrown when either account is frozen or source account has insufficient funds.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the amount is not positive.</exception>
    /// <remarks>
    /// This method transfers funds between accounts and creates transaction records
    /// in both accounts' transaction histories with the same reference number.
    /// </remarks>
    public void Transfer(decimal amount, Account destinationAccount)
    {
        if (IsFrozen)
        {
            throw new InvalidOperationException("Source account is frozen.");
        }
        
        if (destinationAccount.IsFrozen)
        {
            throw new InvalidOperationException("Destination account is frozen.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Transfer amount must be positive.");
        }
        
        if (AccountBalance < amount)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }

        AccountBalance -= amount;
        destinationAccount.AccountBalance += amount;
        
        var transactionDate = DateTime.Now;
        var referenceNumber = Guid.NewGuid().ToString();

        var transferOut = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Transfer,
            Amount = amount,
            SourceAccount = AccountNumber,
            DestinationAccount = destinationAccount.AccountNumber,
            DateCreated = transactionDate,
            Status = TransactionStatus.Completed,
            Description = $"Transfer to {destinationAccount.AccountNumber}",
            BalanceAfterTransaction = AccountBalance,
            Initiator = "System",
            Channel = "Online",
            ReferenceNumber = referenceNumber
        };
        TransactionHistory.Add(transferOut);

        var transferIn = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Transfer,
            Amount = amount,
            SourceAccount = AccountNumber,
            DestinationAccount = destinationAccount.AccountNumber,
            DateCreated = transactionDate,
            Status = TransactionStatus.Completed,
            Description = $"Transfer from {AccountNumber}",
            BalanceAfterTransaction = destinationAccount.AccountBalance,
            Initiator = "System",
            Channel = "Online",
            ReferenceNumber = referenceNumber
        };
        destinationAccount.TransactionHistory.Add(transferIn);
    }

    /// <summary>
    /// Freezes the account, preventing all transactions.
    /// </summary>
    /// <remarks>
    /// Once frozen, the account cannot perform deposits, withdrawals, or transfers
    /// until it is unfrozen using the UnfreezeAccount method.
    /// </remarks>
    public void FreezeAccount()
    {
        IsFrozen = true;
    }

    /// <summary>
    /// Unfreezes the account, allowing transactions to resume.
    /// </summary>
    /// <remarks>
    /// This method restores the account to an active state where transactions
    /// can be performed normally.
    /// </remarks>
    public void UnfreezeAccount()
    {
        IsFrozen = false;
    }
} 