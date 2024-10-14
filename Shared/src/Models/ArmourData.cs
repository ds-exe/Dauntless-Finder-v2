namespace Dauntless_Finder_v2.Shared.src.Models;

public class ArmourData
{
    public Dictionary<int, Dictionary<int, Dictionary<int, List<Armour>>>> Helms { get; set; } = [];

    public Dictionary<int, Dictionary<int, List<Armour>>> Torsos { get; set; } = [];

    public Dictionary<int, Dictionary<int, Dictionary<int, List<Armour>>>> Arms { get; set; } = [];

    public Dictionary<int, Dictionary<int, List<Armour>>> Legs { get; set; } = [];
}
