using BenchmarkDotNet.Attributes;
using Dauntless_Finder_v2.App.src.Scripts;

namespace Dauntless_Finder_v2.Test.Benchmarks;

public class PerkCheckerBenchmarks
{
    protected PerkChecker perkChecker { get; set; }

    public PerkCheckerBenchmarks()
    {
        perkChecker = new PerkChecker();
    }

    [Benchmark]
    public void Benchmark1()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = new() { 1 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks, requestedPerks);
    }

    [Benchmark]
    public void Benchmark2()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = new() { 1, 6, 3, 5 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks, requestedPerks);
    }

    [Benchmark]
    public void Benchmark3()
    {
        List<int> requiredPerks = new() { 1, 6, 3, 5 };
        List<int> requestedPerks = new() { 4 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());
    }

    [Benchmark]
    public void AllPerksBenchmark()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = [];
        for (int i = 1; i <= 80; i++)
        {
            requestedPerks.Add(i);
        }
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());
    }

    [Benchmark]
    public void RequireAllThreshold8Benchmark()
    {
        List<int> requiredPerks = [16, 32];
        List<int> requestedPerks = [57];
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());
    }

    [Benchmark]
    public void HighPerkBenchmark()
    {
        var sut = perkChecker.GetAvailablePerks([16], [32]);

        var sut2 = perkChecker.GetAvailablePerks([57], [16]);

        var sut3 = perkChecker.GetAvailablePerks([32], [57]);
    }

    [Benchmark]
    public void LimitedBuildBenchmark()
    {
        List<int> requestedPerks = [8, 20, 26, 32, 38, 41, 42, 46, 48, 50, 51, 55, 67];
        var sut = perkChecker.GetAvailablePerks([32, 57], requestedPerks.ToList());
    }

    [Benchmark]
    public void LimitedBuildBenchmarkReverse()
    {
        List<int> requestedPerks = [];
        for (int i = 1; i <= 80; i++)
        {
            requestedPerks.Add(i);
        }
        List<int> removedPerks = [8, 20, 26, 32, 38, 41, 42, 46, 48, 50, 51, 55, 67, 32, 57];
        requestedPerks = requestedPerks.Except(removedPerks).ToList();

        var sut = perkChecker.GetAvailablePerks([32, 57], requestedPerks.ToList());
    }

    [Benchmark]
    public void RequireLargeQuantityValid()
    {
        List<int> requiredPerks = [5, 8, 17, 24, 30];
        List<int> requestedPerks = [];
        for (int i = 1; i <= 80; i++)
        {
            requestedPerks.Add(i);
        }
        requestedPerks = requestedPerks.Except(requiredPerks).ToList();
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());
    }
}
