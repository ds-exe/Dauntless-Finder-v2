using Dauntless_Finder_v2.src.Enums;

namespace Dauntless_Finder_v2.src.Models;

public class Armour
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ArmourType Type { get; set; }

    public string Element { get; set; }

    public Dictionary<int, int> Perks { get; set; }

    public int CellSlots { get; set; }
}
