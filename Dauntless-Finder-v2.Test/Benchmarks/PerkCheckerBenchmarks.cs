using BenchmarkDotNet.Attributes;
using Dauntless_Finder_v2.App.src.Scripts;
using NUnit.Framework;

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

    [Benchmark]
    public void PerkChecker2()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = new() { 1, 6, 3, 5 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks, requestedPerks);
    }

    [Benchmark]
    public void PerkChecker3()
    {
        List<int> requiredPerks = new() { 1, 6, 3, 5 };
        List<int> requestedPerks = new() { 4 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());
    }
}
