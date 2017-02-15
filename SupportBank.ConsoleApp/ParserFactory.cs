using System.IO;

namespace SupportBank.ConsoleApp
{
  public class ParserFactory
  {
    public static IParser GetParser(string filename)
    {
      var extension = Path.GetExtension(filename);

      switch (extension)
      {
        case ".csv":
          return new CSVParser();

        case ".json":
          return new JsonParser();

        default:
          return null;
      }
    }
  }
}