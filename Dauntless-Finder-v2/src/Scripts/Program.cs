namespace Dauntless_Finder_v2.App.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        

        List<int> requiredPerks = new() { 1, 6, 3, 5 };
        List<int> requestedPerks = new() { 4 };
        PerkChecker perkChecker = new PerkChecker();
        List<int> output = perkChecker.GetAvailablePerks(requiredPerks, requestedPerks);

        List<int> requiredPerks2 = new() { 1, 3, 4, 5, 6 };
        BruteForceChecker bruteForceChecker = new BruteForceChecker();
        bruteForceChecker.IsBuildViable(requiredPerks2);
        bruteForceChecker.IsBuildViable([1]);

    }
}
