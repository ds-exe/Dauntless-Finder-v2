using Dauntless_Finder_v2.Shared.src.Enums;
using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class GenerateTests
{
    BruteForceChecker bruteForceChecker = new BruteForceChecker();
    protected Data data { get; set; }

    public GenerateTests()
    {
        var dataTmp = FileHandler.ReadData<Data>("data.json");
        if (dataTmp == null)
        {
            throw new Exception("Loading data failed.");
        }
        data = dataTmp;
    }

    public void GenerateLimitedTests()
    {
        List<int> requiredPerks = [32, 57];
        List<int> requestedPerks = data.Perks.Select(rec => rec.Key).Except(requiredPerks).ToList();

        List<int> validPerks = [];
        List<int> invalidPerks = [];
        foreach (var perk in requestedPerks)
        {
            requiredPerks.Add(perk);
            var valid = bruteForceChecker.IsBuildViable(requiredPerks.ToList());
            requiredPerks.Remove(perk);
            if (valid)
            {
                validPerks.Add(perk);
            }
            else
            {
                invalidPerks.Add(perk);
            }
        }

        Console.WriteLine($"\nValid Perks:\n{string.Join(", ", validPerks)}");
        Console.WriteLine($"\nInvalid Perks:\n{string.Join(", ", invalidPerks)}");
    }
}
