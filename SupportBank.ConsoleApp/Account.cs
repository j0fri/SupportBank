using System.Collections.Generic;

namespace SupportBank.ConsoleApp
{
  internal class Account
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
  }
}