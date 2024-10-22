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
    public void PerkChecker1()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = new() { 1 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks, requestedPerks);
    }
}
