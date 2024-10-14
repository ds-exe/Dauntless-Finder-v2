namespace Dauntless_Finder_v2.Shared.src.Models;

public class ArmourData
{
    public Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> Helms { get; set; } = [];

    public Dictionary<int, Dictionary<int, List<int>>> Torsos { get; set; } = [];

    public Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> Arms { get; set; } = [];

    public Dictionary<int, Dictionary<int, List<int>>> Legs { get; set; } = [];
}
