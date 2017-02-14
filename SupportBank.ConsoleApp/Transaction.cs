using System;

namespace SupportBank.ConsoleApp
{
  internal class Transaction
  {
    public DateTime Date { get; set; }
    public string FromAccount { get; set; }
    public string ToAccount { get; set; }
    public string Narrative { get; set; }
    public decimal Amount { get; set; }
  }
}