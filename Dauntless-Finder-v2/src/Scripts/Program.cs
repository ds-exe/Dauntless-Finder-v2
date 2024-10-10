using Shared.src.Models;
using System.Text.Json;

namespace Dauntless_Finder_v2.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        Data? data = ReadData();
        var armour = data?.Armour;
    }

    private static Data? ReadData()
    {
        string filepath = "data.json";
        #if DEBUG
        filepath = "../../../data.json";
        #endif

        string txt = File.ReadAllText(filepath);
        Data? result = JsonSerializer.Deserialize<Data>(txt);
        return result;
    }
}
