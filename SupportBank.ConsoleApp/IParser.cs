using System.Collections.Generic;

namespace SupportBank.ConsoleApp
{
  public interface IParser
  {
    IEnumerable<Transaction> ReadFile(string filename);
  }
}