using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class PerkChecker
{
    protected ArmourData armourData { get; set; }

    protected Data data { get; set; }

    public PerkChecker()
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
    }

    public List<int> GetAvailablePerks(List<int> requiredPerks, List<int> requestedPerks)
    {
        requiredPerks.Sort();
        requestedPerks.Sort();
        List<int> availablePerks = [];

        int? requestedPerk = requestedPerks.First();
        while (requestedPerk != null)
        {
            requiredPerks.Add((int)requestedPerk); // Add requirement for build
            requiredPerks.Sort();

            var (foundBuild, emptyCellSlots) = GetAvailablePerksInternal(requiredPerks);
            if (foundBuild)
            {
                BuildFound((int)requestedPerk, availablePerks, emptyCellSlots, requestedPerks);
            }

            requiredPerks.Remove((int)requestedPerk); // Remove requirement for build
            requestedPerks.Remove((int)requestedPerk); // Remove as checked
            requestedPerk = requestedPerks.FirstOrDefault();
        }

        return availablePerks;
    }

    protected (bool, int) GetAvailablePerksInternal(List<int> requiredPerks)
    {
        Dictionary<int, int> currentPerkValues = GetCurrentPerkValues(requiredPerks);

        return FindArmourPiece(0, 0, requiredPerks, currentPerkValues, 0);
    }

    protected (bool, int) FindArmourPiece(int perkIndex, int armourIndex, List<int> requiredPerks, Dictionary<int, int> currentPerkValues, int emptyCellSlots)
    {
        var buildComplete = true;
        foreach (var requiredPerk in requiredPerks)
        {
            if (currentPerkValues[requiredPerk] >= 0)
            {
                buildComplete = false;
                break;
            }
        }
        if (buildComplete)
        {
            return (true, 0); // TODO: Replace 0 with number of empty cell slots
        }
        if (armourIndex >= 4)
        {
            return (buildComplete, buildComplete ? 0 : 0); // TODO: Replace 1st 0 with number of empty cell slots
        }

        // Recursive method for each ArmourType index 0, 1, 2, 3

        return (false, 0);
    }

    protected Dictionary<int, int> GetCurrentPerkValues(List<int> requiredPerks)
    {
        Dictionary<int, int> currentPerkValues = [];

        foreach (var perk in requiredPerks)
        {
            currentPerkValues[perk] = data.Perks[perk].Threshold;
        }

        return currentPerkValues;
    }

    protected void BuildFound(int requestedPerk, List<int> availablePerks, int emptyCellSlots, List<int> requestedPerks)
    {
        availablePerks.Add(requestedPerk);
        if (emptyCellSlots > 0)
        {
            // TODO: Logic to check all requested perks for if any have threshold less than empty slots

            //availablePerks.Add(requestedPerk);
            //requestedPerks.Remove(requestedPerk); // Remove as found
        }
    }
}
