using BankApp.Abstractions;
using BankApp.Data.JsonStorage;

namespace BankApp.Data.Repositories;

public class JsonAccountManagerRepository : IAccountManagerRepository
{
    public AccountManager? GetById(string id)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        return accountManagers.FirstOrDefault(am => am.Id == id);
    }

    public AccountManager? GetByUsername(string username)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        return accountManagers.FirstOrDefault(am => am.Username == username);
    }

    public AccountManager? GetByEmail(string email)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        return accountManagers.FirstOrDefault(am => am.Email == email);
    }

    public void Update(AccountManager accountManager)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        var existingIndex = accountManagers.FindIndex(am => am.Id == accountManager.Id);
        
        if (existingIndex != -1)
        {
            accountManagers[existingIndex] = accountManager;
            JsonDatabase.SaveAccountManagers(accountManagers);
        }
    }

    public void Add(AccountManager accountManager)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        accountManagers.Add(accountManager);
        JsonDatabase.SaveAccountManagers(accountManagers);
    }

    public void Delete(string id)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        var accountManagerToRemove = accountManagers.FirstOrDefault(am => am.Id == id);
        
        if (accountManagerToRemove != null)
        {
            accountManagers.Remove(accountManagerToRemove);
            JsonDatabase.SaveAccountManagers(accountManagers);
        }
    }

    public IEnumerable<AccountManager> GetAll()
    {
        return JsonDatabase.LoadAccountManagers();
    }

    public bool Exists(string id)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        return accountManagers.Any(am => am.Id == id);
    }

    public bool UsernameExists(string username)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        return accountManagers.Any(am => am.Username == username);
    }

    public bool EmailExists(string email)
    {
        var accountManagers = JsonDatabase.LoadAccountManagers();
        return accountManagers.Any(am => am.Email == email);
    }
} 