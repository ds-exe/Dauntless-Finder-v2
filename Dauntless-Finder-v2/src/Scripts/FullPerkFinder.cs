using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class FullPerkFinder
{
    private ArmourData armourData { get; set; }
    private Data data { get; set; }
    BuildFinder buildFinder { get; set; }

    public void CalculateMaxPerkBuilds()
    {
        armourData = FileHandler.ReadData<ArmourData>("armour-data.json");
        if (armourData == null)
        {
            throw new Exception("Loading armour data failed.");
        }

        data = FileHandler.ReadData<Data>("data.json");
        if (data == null)
        {
            throw new Exception("Loading data failed.");
        }

        buildFinder = new BuildFinder();

        var perks = data.Perks.OrderBy(rec => rec.Value.Threshold).Select(rec => rec.Value).ToList();
        Dictionary<int, List<Perk>> perkDict = [];
        foreach (var perk in perks)
        {
            if (!perkDict.ContainsKey(perk.Threshold))
            {
                perkDict[perk.Threshold] = [];
            }
            perkDict[perk.Threshold].Add(perk);
        }

        CalculateSumT2(8, 0, [], perkDict);
    }

    private void CalculateSumT2(int index, int currentPerkValue, List<int> perkValues, Dictionary<int, List<Perk>> perkDict)
    {
        if (currentPerkValue > 29)
        {
            return;
        }
        if (currentPerkValue == 29)
        {
            List<int> perkIds = [];

            var distinctPerkValues = perkValues.Distinct().ToList();
            //Console.WriteLine(string.Join(", ", perkValues));
            RecurseSelectedPerks(0, selectedPerkList: [], perkValues, distinctPerkValues, perkDict);

            return;
        }
        for (int i = index; i > 1; i--)
        {
            perkValues.Add(i);
            CalculateSumT2(i, currentPerkValue + i, perkValues, perkDict);
            perkValues.Remove(i);
        }
    }

    private bool RecurseSelectedPerks(int index, List<int> selectedPerkList, List<int> perkValues, List<int> distinctPerkValues, Dictionary<int, List<Perk>> perkDict)
    {
        var builds = buildFinder.GetBuilds(selectedPerkList.Select(rec => rec).ToList(), 1);
        if (builds.Count == 0)
        {
            return false;
        }
        if (index == distinctPerkValues.Count())
        {
            if (builds.Count > 0)
            {
                Console.WriteLine(string.Join(", ", selectedPerkList));
                Console.WriteLine("Head: " + data.Armours[builds.FirstOrDefault().Head.Id].Name["en"]);
                Console.WriteLine("Torso: " + data.Armours[builds.FirstOrDefault().Torso.Id].Name["en"]);
                Console.WriteLine("Arms: " + data.Armours[builds.FirstOrDefault().Arms.Id].Name["en"]);
                Console.WriteLine("Legs: " + data.Armours[builds.FirstOrDefault().Legs.Id].Name["en"]);
                return true;
            }
            return false;
        }

        var perkValue = distinctPerkValues[index];
        var count = perkValues.FindAll(rec => rec == perkValue).Count();
        var filteredPerks = perkDict[perkValue].Select(rec => rec.Id).ToList();
        return RecurseSpecificPerk(0, count, filteredPerks, index + 1, selectedPerkList, perkValues, distinctPerkValues, perkDict);
    }

    private bool RecurseSpecificPerk(int index, int count, List<int> filteredPerks, int mainIndex, List<int> selectedPerkList, List<int> perkValues, List<int> distinctPerkValues, Dictionary<int, List<Perk>> perkDict)
    {
        if (filteredPerks.Count() < count)
        {
            return false;
        }
        if (count == 0)
        {
            return RecurseSelectedPerks(mainIndex, selectedPerkList, perkValues, distinctPerkValues, perkDict);
        }

        for (int i = index; i < filteredPerks.Count(); i++)
        {
            selectedPerkList.Add(filteredPerks[i]);
            if (RecurseSpecificPerk(i + 1, count - 1, filteredPerks, mainIndex, selectedPerkList, perkValues, distinctPerkValues, perkDict))
            {
                return true;
            }
            selectedPerkList.Remove(filteredPerks[i]);
        }
        return false;
    }
}
