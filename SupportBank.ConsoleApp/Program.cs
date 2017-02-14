using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SupportBank.ConsoleApp
{
  class Program
  {
    static void Main()
    {
      var transactions = ReadCSV(@"Transactions2014.csv");
      var accounts = CreateAccountsFromTransactions(transactions);

      foreach (var account in accounts)
      {
        Console.WriteLine($"Account {account.Owner}");
        Console.WriteLine("  Incoming transactions:");

        foreach (var transaction in account.IncomingTransactions)
        {
          Console.WriteLine(
            $"    {transaction.Date:d}: {transaction.From} paid {transaction.To} {transaction.Amount:C} for {transaction.Narrative}");
        }

        Console.WriteLine("  Outgoing transactions:");

        foreach (var transaction in account.OutgoingTransactions)
        {
          Console.WriteLine(
            $"    {transaction.Date:d}: {transaction.From} paid {transaction.To} {transaction.Amount:C} for {transaction.Narrative}");
        }
      }

      Console.ReadLine();
    }
    private static IEnumerable<Transaction> ReadCSV(string filename)
    {
      var lines = File.ReadAllLines(filename).Skip(1);

      foreach (var line in lines)
      {
        var fields = line.Split(',');

        yield return new Transaction
        {
          Date = DateTime.Parse(fields[0]),
          From = fields[1],
          To = fields[2],
          Narrative = fields[3],
          Amount = decimal.Parse(fields[4])
        };
      }
    }

    private static IEnumerable<Account> CreateAccountsFromTransactions(IEnumerable<Transaction> transactions)
    {
      var accounts = new Dictionary<string, Account>();

      foreach (var transaction in transactions)
      {
        GetOrCreateAccount(accounts, transaction.From).OutgoingTransactions.Add(transaction);
        GetOrCreateAccount(accounts, transaction.To).IncomingTransactions.Add(transaction);
      }

      return accounts.Values;
    }

    private static Account GetOrCreateAccount(Dictionary<string, Account> accounts, string owner)
    {
      if (accounts.ContainsKey(owner))
      {
        return accounts[owner];
      }

      var newAccount = new Account(owner);
      accounts[owner] = newAccount;
      return newAccount;
    }
  }
}
