using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        ArmourData? armourData = FileHandler.ReadData<ArmourData>("armour-data.json");
        if (armourData == null)
        {
            Console.WriteLine("Loading data failed.");
            return;
        }
        Data? data = FileHandler.ReadData<Data>("data.json");
    }
}
