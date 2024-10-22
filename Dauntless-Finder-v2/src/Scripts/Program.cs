namespace Dauntless_Finder_v2.App.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        List<int> requiredPerks = new() { 1, 6, 3, 5 };
        List<int> requestedPerks = new() { 4 };
        PerkChecker perkChecker = new PerkChecker();
        List<int> output = perkChecker.GetAvailablePerks(requiredPerks, requestedPerks);

        BruteForceChecker bruteForceChecker = new BruteForceChecker();
        var isViable = bruteForceChecker.IsBuildViable([5, 8, 17, 24, 30, 44, 48, 71, 77]);
        var isViable2 = bruteForceChecker.IsBuildViable([7, 10, 13, 22, 25, 42, 72, 76]);
        var isViable3 = bruteForceChecker.IsBuildViable([16, 32]);
        var isViable4 = bruteForceChecker.IsBuildViable([16, 57]);
        List<int> foundPerks = [];
        for (int i = 1; i <= 80; i++)
        {
            var isViable5 = bruteForceChecker.IsBuildViable([57, 32, i]);
            if (isViable5)
            {
                foundPerks.Add(i);
            }
        }
    }
}
