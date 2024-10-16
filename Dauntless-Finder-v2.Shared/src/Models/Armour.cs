using Dauntless_Finder_v2.Shared.src.Enums;

namespace Dauntless_Finder_v2.Shared.src.Models;

public class Armour
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ArmourType Type { get; set; }

    public List<Stat> Stats { get; set; }

    public int CellSlots { get; set; }
}
