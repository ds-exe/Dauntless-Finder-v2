using Dauntless_Finder_v2.data.src.Scripts;

namespace Dauntless_Finder_v2.DataHandler.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        MainAsync().Wait();
    }

    private static async Task MainAsync()
    {
        await FetchData.GetData();

        GenerateIndexedData.GenerateJsonData();
    }

}
