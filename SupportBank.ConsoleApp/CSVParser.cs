using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;

namespace SupportBank.ConsoleApp
{
  class CSVParser : IParser
  {
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    public IEnumerable<Transaction> ReadFile(string filename)
    {
      logger.Info($"Loading transactions from file {filename}");

      var lines = File.ReadAllLines(filename).Skip(1);

      foreach (var line in lines)
      {
        logger.Debug($"Parsing transaction: {line}");

        var fields = line.Split(',');

        if (fields.Length != 5)
        {
          ReportSkippedTransaction(line, "Wrong number of fields");
          continue;
        }

        DateTime date;
        if (!DateTime.TryParse(fields[0], out date))
        {
          ReportSkippedTransaction(line, "Invalid date");
          continue;
        }

        decimal amount;
        if (!decimal.TryParse(fields[4], out amount))
        {
          ReportSkippedTransaction(line, "Invalid transaction amount");
          continue;
        }

        yield return new Transaction
        {
          Date = date,
          FromAccount = fields[1],
          ToAccount = fields[2],
          Narrative = fields[3],
          Amount = amount
        };
      }
    }

    private void ReportSkippedTransaction(string transaction, string reason)
    {
      logger.Error($"Unable to process transaction because {reason}: {transaction}");
      Console.Error.WriteLine($"Skipping invalid transaction: {transaction}");
    }
  }
}