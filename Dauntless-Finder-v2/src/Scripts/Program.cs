using Dauntless_Finder_v2.Shared.src.Models;
using System.Text.Json;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        ArmourData? data = ReadData();
        if (data == null)
        {
            Console.WriteLine("Loading data failed.");
            return;
        }

        // Run test for now, eventually replace with proper testing / benchmarks
        Test(data);
    }

    private static void Test(ArmourData data)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        // Do Perk Checker

        watch.Stop();
        Console.WriteLine($"Time to Check: {watch.ElapsedMilliseconds}");
    }

    public static ArmourData? ReadData()
    {
        string filepath = "armour-data.json";
        #if DEBUG
        filepath = "../../../armour-data.json";
        #endif

        string txt = File.ReadAllText(filepath);
        ArmourData? result = JsonSerializer.Deserialize<ArmourData>(txt);
        return result;
    }
}
