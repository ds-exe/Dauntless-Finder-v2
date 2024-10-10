using Dauntless_Finder_v2.src.Models;
using System.Text.Json;

namespace Dauntless_Finder_v2_data;

public class Program
{
    private static void Main(string[] args)
    {
        _ = ReadData();
    }

    private static async Task<Data?> ReadData()
    {
        using (FileStream stream = File.OpenRead("data.json"))
        {
            return await JsonSerializer.DeserializeAsync<Data>(stream);
        }
    }
}
