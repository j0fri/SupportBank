using System;
using System.Collections.Generic;
using System.Xml;
using log4net;

namespace SupportBank.ConsoleApp
{
  public class XmlParser : IParser
  {
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public IEnumerable<Transaction> ReadFile(string filename)
    {
      logger.Info($"Loading transactions from file {filename}");

      XmlDocument xml = new XmlDocument();
      xml.Load(filename);

      foreach (XmlElement transaction in xml.SelectNodes("TransactionList/SupportTransaction"))
      {
        logger.Debug($"Parsing transaction: {transaction}");

        DateTime date;
        try
        {
          date = DateTime.FromOADate(double.Parse(transaction.GetAttribute("Date")));
        }
        catch (Exception)
        {
          ReportSkippedTransaction(transaction, "Invalid date");
          continue;
        }

        decimal amount;
        if (!decimal.TryParse(transaction.SelectSingleNode("Value")?.InnerText, out amount))
        {
          ReportSkippedTransaction(transaction, "Invalid transaction amount");
          continue;
        }

        string description = transaction.SelectSingleNode("Description")?.InnerText;
        if (description == null)
        {
          ReportSkippedTransaction(transaction, "No description found");
          continue;
        }

        string from = transaction.SelectSingleNode("Parties/From")?.InnerText;
        if (from == null)
        {
          ReportSkippedTransaction(transaction, "No from party found");
          continue;
        }

        string to = transaction.SelectSingleNode("Parties/To")?.InnerText;
        if (to == null)
        {
          ReportSkippedTransaction(transaction, "No to party found");
          continue;
        }

        yield return new Transaction
        {
          Date = date,
          FromAccount = from,
          ToAccount = to,
          Narrative = description,
          Amount = amount
        };
      }
    }

    private void ReportSkippedTransaction(XmlNode transaction, string reason)
    {
      logger.Error($"Unable to process transaction because {reason}: {transaction.InnerXml}");
      Console.Error.WriteLine($"Skipping invalid transaction: {transaction.InnerXml}");
    }

  }
}