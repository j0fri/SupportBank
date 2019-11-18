using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json;

namespace SupportBank.ConsoleApp
{
  class JsonParser : IParser
  {
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public IEnumerable<Transaction> ReadFile(string filename)
    {
      logger.Info($"Loading transactions from file {filename}");

      var contents = File.ReadAllText(filename);
      return JsonConvert.DeserializeObject<List<Transaction>>(contents);
    }
  }
}