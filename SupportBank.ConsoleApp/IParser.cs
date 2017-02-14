using System.Collections.Generic;

namespace SupportBank.ConsoleApp
{
  internal interface IParser
  {
    IEnumerable<Transaction> ReadFile(string filename);
  }
}