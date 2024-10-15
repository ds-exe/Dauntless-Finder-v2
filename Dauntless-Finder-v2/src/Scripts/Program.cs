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

        // Run test for now, eventually replace with proper testing / benchmarks
        Test(armourData);
    }

    private static void Test(ArmourData data)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        // Do Perk Checker

        watch.Stop();
        Console.WriteLine($"Time to Check: {watch.ElapsedMilliseconds}");
    }
}
