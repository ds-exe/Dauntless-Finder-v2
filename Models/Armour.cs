namespace Dauntless_Finder_v2.Models;

public class Armour
{
    public string Name { get; set; }

    public string Element { get; set; }

    public Dictionary<string, int> Perks { get; set; }

    public int CellSlots { get; set; }
}
