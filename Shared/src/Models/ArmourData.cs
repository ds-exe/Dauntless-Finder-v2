namespace Dauntless_Finder_v2.Shared.src.Models;

public class ArmourData
{
    public Dictionary<int, Dictionary<int, Dictionary<int, List<BasicArmour>>>> Helms { get; set; } = [];

    public Dictionary<int, Dictionary<int, List<BasicArmour>>> Torsos { get; set; } = [];

    public Dictionary<int, Dictionary<int, Dictionary<int, List<BasicArmour>>>> Arms { get; set; } = [];

    public Dictionary<int, Dictionary<int, List<BasicArmour>>> Legs { get; set; } = [];
}
