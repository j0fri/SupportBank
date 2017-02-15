using System.Collections.Generic;
using System.Linq;

namespace SupportBank.ConsoleApp
{
  public class Account
  {
    public Account(string owner)
    {
      Owner = owner;
      IncomingTransactions = new List<Transaction>();
      OutgoingTransactions = new List<Transaction>();
    }

    public string Owner { get; set; }
    public List<Transaction> IncomingTransactions { get; private set; }
    public List<Transaction> OutgoingTransactions { get; private set; }

    public IEnumerable<Transaction> AllTransactions
    {
      get { return IncomingTransactions.Union(OutgoingTransactions).OrderBy(tx => tx.Date); }
    }

    public decimal Balance
    {
      get { return IncomingTransactions.Sum(tx => tx.Amount) - OutgoingTransactions.Sum(tx => tx.Amount); }
    }
  }
}