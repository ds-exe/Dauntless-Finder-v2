using Dauntless_Finder_v2.Shared.src.Models;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        //FullPerkFinder fullPerkFinder = new FullPerkFinder();
        //fullPerkFinder.CalculateMaxPerkBuilds();
        //return;
        GenerateTests generateTests = new GenerateTests();
        generateTests.GenerateLimitedTests();
    }
}
