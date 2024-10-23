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
        requiredPerks = requiredPerks.ToList();
        requestedPerks = requestedPerks.ToList();
        requiredPerks.Sort();
        requestedPerks.Sort();
        List<int> availablePerks = [];

        int requestedPerk = requestedPerks.FirstOrDefault();
        while (requestedPerk != 0)
        {
            requiredPerks.Add(requestedPerk); // Add requirement for build
            requiredPerks.Sort();

            var (foundBuild, emptyCellSlots) = GetAvailablePerksInternal(requiredPerks);
            if (foundBuild)
            {
                BuildFound(requestedPerk, availablePerks, emptyCellSlots, requestedPerks);
            }

            requiredPerks.Remove(requestedPerk); // Remove requirement for build
            requestedPerks.Remove(requestedPerk); // Remove as checked
            requestedPerk = requestedPerks.FirstOrDefault();
        }

        return availablePerks;
    }

    protected (bool, int) GetAvailablePerksInternal(List<int> requiredPerks)
    {
        List<int> requiredPerksCopy = requiredPerks.ToList();
        Dictionary<int, int> currentPerkValues = GetCurrentPerkValues(requiredPerksCopy);

        var armourType = Enum.GetValues(typeof(ArmourType)).Cast<ArmourType>().Min();
        return FindArmourPiece(armourType, requiredPerksCopy, currentPerkValues);
    }

    protected (bool, int) FindArmourPiece(ArmourType armourType, List<int> requiredPerks, Dictionary<int, int> currentPerkValues)
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
                }
                totalPerkThreshold += currentPerkValues[requiredPerk];
            }
        }
        if (buildComplete && isDefined)
        {
            return (true, maxEmptyCellSlots);
        }
        if (!isDefined)
        {
            var emptyCellSlots = maxEmptyCellSlots - totalPerkThreshold;
            return (emptyCellSlots >= 0, emptyCellSlots);
        }

        switch (armourType)
        {
            case ArmourType.HEAD:
            case ArmourType.ARMS:
                return FindArmourPiece3Perk(armourType, requiredPerks, currentPerkValues);
            case ArmourType.TORSO:
            case ArmourType.LEGS:
                return FindArmourPiece2Perk(armourType, requiredPerks, currentPerkValues);
        }

        return (false, 0);
    }

    protected (bool, int) FindArmourPiece2Perk(ArmourType armourType, List<int> requiredPerks, Dictionary<int, int> currentPerkValues)
    {
        Dictionary<int, Dictionary<int, List<BasicArmour>>> currentData = armourType == ArmourType.TORSO ? armourData.Torsos : armourData.Legs;
        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] > 0 && currentData.ContainsKey(requiredPerks[i]))
            {
                for (int j = i + 1; j < requiredPerks.Count; j++)
                {
                    if (currentPerkValues[requiredPerks[j]] > 0 && currentData[requiredPerks[i]].ContainsKey(requiredPerks[j]))
                    {
                        foreach (var armour in currentData[requiredPerks[i]][requiredPerks[j]])
                        {
                            Dictionary<int, int>? perks = armour.Perks;
                            if (perks != null)
                            {
                                ReduceCurrentPerkValues(perks, currentPerkValues);
                                var (found, cellSlots) = FindArmourPiece(armourType + 1, requiredPerks, currentPerkValues); // Recurse
                                if (found)
                                {
                                    //Console.WriteLine(armour.Id);
                                    return (found, cellSlots);
                                }
                                IncreaseCurrentPerkValues(perks, currentPerkValues);
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] > 0 && currentData.ContainsKey(requiredPerks[i]))
            {
                Dictionary<int, int>? perks = currentData[requiredPerks[i]][0].FirstOrDefault()?.Perks; // Data is sorted so no need to iterate
                if (perks != null)
                {
                    ReduceCurrentPerkValues(perks, currentPerkValues);
                    var (found, cellSlots) = FindArmourPiece(armourType + 1, requiredPerks, currentPerkValues); // Recurse
                    if (found)
                    {
                        //Console.WriteLine(currentData[requiredPerksRepeatCheck[i]][0].FirstOrDefault().Id);
                        return (found, cellSlots);
                    }
                    IncreaseCurrentPerkValues(perks, currentPerkValues);
                }
            }
        }

        var (foundGeneric, cellSlotsGeneric) = FindArmourPiece(armourType + 1, requiredPerks, currentPerkValues); // Recurse
        if (foundGeneric)
        {
            //Console.WriteLine($"Generic {armourType}");
            return (foundGeneric, cellSlotsGeneric);
        }

        return (false, 0);
    }

    protected (bool, int) FindArmourPiece3Perk(ArmourType armourType, List<int> requiredPerks, Dictionary<int, int> currentPerkValues)
    {
        Dictionary<int, Dictionary<int, Dictionary<int, List<BasicArmour>>>> currentData = armourType == ArmourType.HEAD ? armourData.Heads : armourData.Arms;
        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] > 0 && currentData.ContainsKey(requiredPerks[i]))
            {
                for (int j = i + 1; j < requiredPerks.Count; j++)
                {
                    if (currentPerkValues[requiredPerks[j]] > 0 && currentData[requiredPerks[i]].ContainsKey(requiredPerks[j]))
                    {
                        for (int k = j + 1; k < requiredPerks.Count; k++)
                        {
                            if (currentPerkValues[requiredPerks[k]] > 0 && currentData[requiredPerks[i]][requiredPerks[j]].ContainsKey(requiredPerks[k]))
                            {
                                foreach (var armour in currentData[requiredPerks[i]][requiredPerks[j]][requiredPerks[k]])
                                {
                                    Dictionary<int, int>? perks = armour.Perks;
                                    if (perks != null)
                                    {
                                        ReduceCurrentPerkValues(perks, currentPerkValues);
                                        var (found, cellSlots) = FindArmourPiece(armourType + 1, requiredPerks, currentPerkValues); // Recurse
                                        if (found)
                                        {
                                            //Console.WriteLine(armour.Id);
                                            return (found, cellSlots);
                                        }
                                        IncreaseCurrentPerkValues(perks, currentPerkValues);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] > 0 && currentData.ContainsKey(requiredPerks[i]))
            {
                for (int j = i + 1; j < requiredPerks.Count; j++)
                {
                    if (currentPerkValues[requiredPerks[j]] > 0 && currentData[requiredPerks[i]].ContainsKey(requiredPerks[j]))
                    {
                        foreach (var armour in currentData[requiredPerks[i]][requiredPerks[j]][0])
                        {
                            Dictionary<int, int>? perks = armour.Perks;
                            if (perks != null)
                            {
                                ReduceCurrentPerkValues(perks, currentPerkValues);
                                var (found, cellSlots) = FindArmourPiece(armourType + 1, requiredPerks, currentPerkValues); // Recurse
                                if (found)
                                {
                                    //Console.WriteLine(armour.Id);
                                    return (found, cellSlots);
                                }
                                IncreaseCurrentPerkValues(perks, currentPerkValues);
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] > 0 && currentData.ContainsKey(requiredPerks[i]))
            {
                Dictionary<int, int>? perks = currentData[requiredPerks[i]][0][0].FirstOrDefault()?.Perks; // Data is sorted so no need to iterate
                if (perks != null)
                {
                    ReduceCurrentPerkValues(perks, currentPerkValues);
                    var (found, cellSlots) = FindArmourPiece(armourType + 1, requiredPerks, currentPerkValues); // Recurse
                    if (found)
                    {
                        //Console.WriteLine(currentData[requiredPerksRepeatCheck2[i]][0][0].FirstOrDefault().Id);
                        return (found, cellSlots);
                    }
                    IncreaseCurrentPerkValues(perks, currentPerkValues);
                }
            }
        }

        var (foundGeneric, cellSlotsGeneric) = FindArmourPiece(armourType + 1, requiredPerks, currentPerkValues); // Recurse
        if (foundGeneric)
        {
            //Console.WriteLine($"Generic {armourType}");
            return (foundGeneric, cellSlotsGeneric);
        }

        return (false, 0);
    }

    protected void ReduceCurrentPerkValues(Dictionary<int, int> perks, Dictionary<int, int> currentPerkValues)
    {
        foreach (var perk in perks)
        {
            if (currentPerkValues.ContainsKey(perk.Key))
            {
                currentPerkValues[perk.Key] -= perk.Value;
            }
        }
    }

    protected void IncreaseCurrentPerkValues(Dictionary<int, int> perks, Dictionary<int, int> currentPerkValues)
    {
        foreach (var perk in perks)
        {
            if (currentPerkValues.ContainsKey(perk.Key))
            {
                currentPerkValues[perk.Key] += perk.Value;
            }
        }
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
        requestedPerks.Remove(requestedPerk);
        if (emptyCellSlots > 0)
        {
            var perkThresholds = GetCurrentPerkValues(requestedPerks);
            foreach (var perk in perkThresholds)
            {
                if (perk.Value <= emptyCellSlots)
                {
                    availablePerks.Add(perk.Key);
                    requestedPerks.Remove(perk.Key);
                }
            }
        }
    }
}
