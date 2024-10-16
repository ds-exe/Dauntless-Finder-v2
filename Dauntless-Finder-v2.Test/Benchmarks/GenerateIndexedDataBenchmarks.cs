using BenchmarkDotNet.Attributes;
using Dauntless_Finder_v2.DataHandler.src.Scripts;

namespace Dauntless_Finder_v2.Test.Benchmarks;

public class GenerateIndexedDataBenchmarks
{
    [Benchmark]
    public void GenerateArmourData()
    {
        GenerateIndexedData.GenerateArmourData();
    }
}
