using BenchmarkDotNet.Attributes;
using Dauntless_Finder_v2.App.src.Scripts;

namespace Dauntless_Finder_v2.Test.Benchmarks;

public class BuildFinderBenchmarks
{
    protected BuildFinder buildFinder { get; set; }

    public BuildFinderBenchmarks()
    {
        buildFinder = new BuildFinder();
    }

    [Benchmark]
    public void Benchmark1()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = new() { 1 };
        var sut = buildFinder.GetBuilds([5, 8, 17, 24, 30, 36], 50);
    }
}
