using System;
using System.Collections.Generic;
using System.Linq;

namespace SupportBank.ConsoleApp
{
  class ConsoleRunner
  {
    private enum CommandType
    {
      ListAll,
      ListOne,
      ImportFile
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

          case CommandType.ImportFile:
            ImportFile(accounts, command.Target);
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
      Console.WriteLine("  Import File [Filename] - import transactions from the specified file");
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
      if (commandText.StartsWith("List "))
      {
        return ParseListCommand(commandText.Substring(5), out command);
      }

      command = new Command();

      if (commandText.StartsWith("Import File "))
      {
        command.Target = commandText.Substring("Import File ".Length);
        command.Type = CommandType.ImportFile;
        return true;
      }

      return false;
    }

    private bool ParseListCommand(string target, out Command command)
    {
      command = new Command();

      if (target == "All")
      {
        command.Type = CommandType.ListAll;
      }
      else
      {
        command.Type = CommandType.ListOne;
        command.Target = target;
      }

      return true;
    }

    private void ListAllAccounts(Bank accounts)
    {
      Console.WriteLine("All accounts");

      foreach (var account in accounts)
      {
        Console.WriteLine(
          $"  {account.Owner} {(account.Balance < 0 ? "owes" : "is owed")} {Math.Abs(account.Balance):C}");
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

    private void ImportFile(Bank accounts, string filename)
    {
      var parser = ParserFactory.GetParser(filename);

      if (parser == null)
      {
        Console.WriteLine("Sorry, I'm not sure how to import that type of file.");
        Console.WriteLine();
        return;
      }

      Console.WriteLine($"Importing {filename}...");

      try
      {
        var transactions = parser.ReadFile(filename);
        accounts.ImportTransactions(transactions);
      }
      catch (Exception e)
      {
        Console.WriteLine("Unable to import transactions due to error - one or more transactions have not been loaded");
        Console.WriteLine(e.Message);
        Console.WriteLine();
        return;
      }

      Console.WriteLine("Done");
      Console.WriteLine();
    }

  }
}