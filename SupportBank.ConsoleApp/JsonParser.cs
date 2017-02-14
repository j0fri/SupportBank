using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace SupportBank.ConsoleApp
{
  class JsonParser : IParser
  {
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    public IEnumerable<Transaction> ReadFile(string filename)
    {
      logger.Info($"Loading transactions from file {filename}");

      var contents = File.ReadAllText(filename);
      return JsonConvert.DeserializeObject<List<Transaction>>(contents);
    }
  }
}