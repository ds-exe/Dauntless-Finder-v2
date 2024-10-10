using Dauntless_Finder_v2.Models;

namespace Dauntless_Finder_v2;

internal class Program
{
    static void Main(string[] args)
    {
        Run();
    }

    static async void Run()
    {
        Data? data = await GetData.FetchData();
        if (data == null)
        {
            return;
        }
    }


}
