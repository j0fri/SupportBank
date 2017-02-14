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
      var parsedData = ReadCSV(@"Transactions2014.csv");

      foreach (var transaction in parsedData)
      {
        Console.WriteLine($"{transaction.Date:d}: {transaction.From} paid {transaction.To} {transaction.Amount:C} for {transaction.Narrative}");
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
  }
}
