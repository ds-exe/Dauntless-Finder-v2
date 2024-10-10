using Dauntless_Finder_v2.src.Models;

namespace Dauntless_Finder_v2.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        Data? data = DataProcessor.GenerateJsonData();
        if (data == null)
        {
            return;
        }
    }
}
