using Dauntless_Finder_v2.Shared.src.Enums;
using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class PerkChecker
{
    protected ArmourData armourData { get; set; }

    protected Data data { get; set; }

    protected static readonly int maxEmptyCellSlots = 6;

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

        var armourType = Enum.GetValues(typeof(ArmourType)).Cast<ArmourType>().Min();
        return FindArmourPiece(0, armourType, requiredPerks, currentPerkValues);
    }

    protected (bool, int) FindArmourPiece(int perkIndex, ArmourType armourType, List<int> requiredPerks, Dictionary<int, int> currentPerkValues)
    {
        var isDefined = Enum.IsDefined(typeof(ArmourType), armourType);
        int totalPerkThreshold = 0;
        var buildComplete = true;
        foreach (var requiredPerk in requiredPerks)
        {
            if (currentPerkValues[requiredPerk] > 0)
            {
                if (isDefined)
                {
                    buildComplete = false;
                    break;
                }
                totalPerkThreshold += currentPerkValues[requiredPerk];
            }
        }
        if (buildComplete)
        {
            return (true, maxEmptyCellSlots);
        }
        if (!isDefined)
        {
            var emptyCellSlots = maxEmptyCellSlots - totalPerkThreshold;
            return (emptyCellSlots < 0, emptyCellSlots);
        }

        // Recursive method for each ArmourType index 0, 1, 2, 3

        return (false, 0);
    }

    protected Dictionary<int, int> GetCurrentPerkValues(List<int> perkList)
    {
        Dictionary<int, int> currentPerkValues = [];

        foreach (var perk in perkList)
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
            var perkThresholds = GetCurrentPerkValues(requestedPerks);
            foreach (var perk in perkThresholds)
            {
                if (perk.Value < emptyCellSlots)
                {
                    availablePerks.Add(perk.Key);
                    requestedPerks.Remove(perk.Key);
                }
            }
        }
    }
}
