using System.Collections;
using System.Collections.Generic;
using log4net;
using log4net.Core;

namespace SupportBank.ConsoleApp
{
  class Bank : IEnumerable<Account>
  {
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly Dictionary<string, Account> accounts = new Dictionary<string, Account>();

    public Account this[string owner] => GetOrCreateAccount(owner);

    public IEnumerator<Account> GetEnumerator()
    {
      return accounts.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void ImportTransactions(IEnumerable<Transaction> transactions)
    {
      foreach (var transaction in transactions)
      {
        GetOrCreateAccount(transaction.FromAccount).OutgoingTransactions.Add(transaction);
        GetOrCreateAccount(transaction.ToAccount).IncomingTransactions.Add(transaction);
      }
    }

    private Account GetOrCreateAccount(string owner)
    {
      if (accounts.ContainsKey(owner))
      {
        return accounts[owner];
      }

      logger.Debug($"Adding account for {owner}");
      var newAccount = new Account(owner);
      accounts[owner] = newAccount;
      return newAccount;
    }

  }
}