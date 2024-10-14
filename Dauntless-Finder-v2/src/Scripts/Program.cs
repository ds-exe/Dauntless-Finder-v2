using Dauntless_Finder_v2.Shared.src.Models;
using System.Text.Json;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        ArmourData? data = ReadData();
        //var armour = data?.Armour;
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
