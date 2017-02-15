using System;
using System.Linq;

namespace SupportBank.ConsoleApp
{
  class ConsoleRunner
  {
    private enum CommandType
    {
      ListAll,
      ListOne
    }

    private struct Command
    {
      public CommandType Type { get; set; }
      public string Target { get; set; }
    }

    public void Run(Bank accounts)
    {
      PrintWelcomeBanner();

      while (true)
      {
        var command = PromptForCommand();

        switch (command.Type)
        {
          case CommandType.ListAll:
            ListAllAccounts(accounts);
            break;

          case CommandType.ListOne:
            ListOneAccount(accounts[command.Target]);
            break;
        }
      }
    }

    private void PrintWelcomeBanner()
    {
      Console.WriteLine("Welcome to SupportBank!");
      Console.WriteLine("=======================");
      Console.WriteLine();
      Console.WriteLine("Available commands:");
      Console.WriteLine("  List All - list all account balances");
      Console.WriteLine("  List [Account] - list transactions for the specified account");
      Console.WriteLine();
    }

    private Command PromptForCommand()
    {
      while (true)
      {
        Console.Write("Your command> ");
        string commandText = Console.ReadLine();

        Command command;

        if (ParseCommand(commandText, out command))
        {
          return command;
        }

        Console.WriteLine("Sorry, I didn't understand that");
        Console.WriteLine();
      }
    }

    private bool ParseCommand(string commandText, out Command command)
    {
      command = new Command();

      if (!commandText.StartsWith("List "))
      {
        return false;
      }

      if (commandText.Substring(5) == "All")
      {
        command.Type = CommandType.ListAll;
      }
      else
      {
        command.Type = CommandType.ListOne;
        command.Target = commandText.Substring(5);
      }

      return true;
    }

    private void ListAllAccounts(Bank accounts)
    {
      Console.WriteLine("All accounts");

      foreach (var account in accounts)
      {
        Console.WriteLine($"  {account.Owner} {(account.Balance < 0 ? "owes" : "is owed")} {Math.Abs(account.Balance):C}");
      }

      Console.WriteLine();
    }

    private void ListOneAccount(Account account)
    {
      Console.WriteLine($"Account {account.Owner}");

      foreach (var transaction in account.AllTransactions)
      {
        Console.WriteLine(
          $"  {transaction.Date:d}: {transaction.FromAccount} paid {transaction.ToAccount} {transaction.Amount:C} for {transaction.Narrative}");
      }

      Console.WriteLine();
    }
  }
}