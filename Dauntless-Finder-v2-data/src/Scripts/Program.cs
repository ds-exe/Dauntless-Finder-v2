using Shared.src.Models;

namespace Dauntless_Finder_v2_data.src.Scripts;

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
