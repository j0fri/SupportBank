using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SupportBank.ConsoleApp
{
  class Program
  {
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    static void Main()
    {
      ConfigureNLog();
      logger.Info("SupportBank starting up");

      new ConsoleRunner().Run(new Bank());
    }

    private static void ConfigureNLog()
    {
      var config = new LoggingConfiguration();
      var target = new FileTarget
      {
        FileName = @"SupportBank.log",
        Layout = @"${longdate} ${level} - ${logger}: ${message}"
      };
      config.AddTarget("File Logger", target);
      config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
      LogManager.Configuration = config;
    }
  }
}
