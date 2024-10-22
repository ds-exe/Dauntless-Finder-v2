namespace Dauntless_Finder_v2.App.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        BruteForceChecker bruteForceChecker = new BruteForceChecker();

        //List<int> requiredPerks = new() { 1, 6, 3, 5 };
        //List<int> requestedPerks = new() { 4 };
        //PerkChecker perkChecker = new PerkChecker();
        //List<int> output = perkChecker.GetAvailablePerks(requiredPerks, requestedPerks);

        //var isViable = bruteForceChecker.IsBuildViable([5, 8, 17, 24, 30, 44, 48, 71, 77]);
        //var isViable2 = bruteForceChecker.IsBuildViable([7, 10, 13, 22, 25, 42, 72, 76]);
        //var isViable3 = bruteForceChecker.IsBuildViable([16, 32]);
        //var isViable4 = bruteForceChecker.IsBuildViable([5, 8, 17, 24, 30, 77]);
        //List<int> foundPerks = [];
        //for (int i = 1; i <= 80; i++)
        //{
        //    var isViable5 = bruteForceChecker.IsBuildViable([57, 32, i]);
        //    if (isViable5)
        //    {
        //        foundPerks.Add(i);
        //    }
        //}

        List<int> requiredPerks = [5, 8, 17, 24, 30];
        List<int> requestedPerks = [];
        //for (int i = 1; i <= 80; i++)
        //{
        //    requestedPerks.Add(i);
        //}
        //requestedPerks = requestedPerks.Except(requiredPerks).ToList();
        //List<int> foundPerks = [];
        //foreach (int i in requestedPerks)
        //{
        //    requiredPerks.Add(i);
        //    var sut = bruteForceChecker.IsBuildViable(requiredPerks);
        //    if (sut)
        //    {
        //        foundPerks.Add(i);
        //    }
        //    requiredPerks.Remove(i);
        //}
        requiredPerks.Add(36);
        var sut = bruteForceChecker.IsBuildViable(requiredPerks);
    }
}
