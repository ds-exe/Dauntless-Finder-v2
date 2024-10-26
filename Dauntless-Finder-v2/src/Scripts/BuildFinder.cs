using Dauntless_Finder_v2.Shared.src.Enums;
using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class BuildFinder
{
    protected ArmourData armourData { get; set; }

    protected Data data { get; set; }

    protected static readonly int maxEmptyCellSlots = 6;

    public BuildFinder()
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

    public List<Build> GetBuilds(List<int> requiredPerks, int maxBuilds)
    {
        requiredPerks = requiredPerks.ToList();
        requiredPerks.Sort();

        List<Build> builds = GetBuildsInternal(requiredPerks, maxBuilds);

        return builds;
    }

    protected List<Build> GetBuildsInternal(List<int> requiredPerks, int maxBuilds)
    {
        var armourType = Enum.GetValues(typeof(ArmourType)).Cast<ArmourType>().Min();
        Dictionary<int, int> currentPerkValues = GetCurrentPerkValues(requiredPerks);
        List<Build> builds = [];
        FindArmourPiece(armourType, requiredPerks, maxBuilds, currentPerkValues, new TempBuild(), builds);

        return builds;
    }

    protected bool FindArmourPiece(ArmourType armourType, List<int> requiredPerks, int maxBuilds, Dictionary<int, int> currentPerkValues, TempBuild tempBuild, List<Build> builds)
    {
        if (builds.Count >= maxBuilds)
        {
            return false;
        }

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
            // TODO: ADD BUILD

            return true;
        }
        if (!isDefined)
        {
            var emptyCellSlots = maxEmptyCellSlots - totalPerkThreshold;
            var validBuild = emptyCellSlots >= 0;

            // TODO: ADD BUILD
            return validBuild;
        }

        switch (armourType)
        {
            case ArmourType.HEAD:
            case ArmourType.ARMS:
                return FindArmourPiece3Perk(armourType, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds);
            case ArmourType.TORSO:
            case ArmourType.LEGS:
                return FindArmourPiece2Perk(armourType, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds);
        }

        return false;
    }

    protected bool FindArmourPiece2Perk(ArmourType armourType, List<int> requiredPerks, int maxBuilds, Dictionary<int, int> currentPerkValues, TempBuild tempBuild, List<Build> builds)
    {
        Dictionary<int, Dictionary<int, List<BasicArmour>>> currentData = armourType == ArmourType.TORSO ? armourData.Torsos : armourData.Legs;
        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] <= 0 || !currentData.ContainsKey(requiredPerks[i]))
            {
                continue;
            }
            for (int j = i + 1; j < requiredPerks.Count; j++)
            {
                if (currentPerkValues[requiredPerks[j]] <= 0 || !currentData[requiredPerks[i]].ContainsKey(requiredPerks[j]))
                {
                    continue;
                }
                foreach (var armour in currentData[requiredPerks[i]][requiredPerks[j]])
                {
                    Dictionary<int, int> perks = armour.Perks;
                    ReduceCurrentPerkValues(perks, currentPerkValues);
                    tempBuild.ArmourPieces[armourType] = armour.Id;
                    var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds); // Recurse
                    if (found)
                    {
                        //Console.WriteLine(armour.Id);
                        return found;
                    }
                    IncreaseCurrentPerkValues(perks, currentPerkValues);
                }
            }
        }

        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] <= 0 || !currentData.ContainsKey(requiredPerks[i]))
            {
                continue;
            }
            foreach (var armour in currentData[requiredPerks[i]][0])
            {
                Dictionary<int, int> perks = armour.Perks;
                ReduceCurrentPerkValues(perks, currentPerkValues);
                tempBuild.ArmourPieces[armourType] = armour.Id;
                var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds); // Recurse
                if (found)
                {
                    //Console.WriteLine(armour.Id);
                    return found;
                }
                IncreaseCurrentPerkValues(perks, currentPerkValues);
            }
        }

        tempBuild.ArmourPieces[armourType] = 0; // A armour ID of 0 means any piece
        var foundGeneric = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds); // Recurse
        if (foundGeneric)
        {
            //Console.WriteLine($"Generic {armourType}");
            return foundGeneric;
        }

        return false;
    }

    protected bool FindArmourPiece3Perk(ArmourType armourType, List<int> requiredPerks, int maxBuilds, Dictionary<int, int> currentPerkValues, TempBuild tempBuild, List<Build> builds)
    {
        Dictionary<int, Dictionary<int, Dictionary<int, List<BasicArmour>>>> currentData = armourType == ArmourType.HEAD ? armourData.Heads : armourData.Arms;
        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] <= 0 || !currentData.ContainsKey(requiredPerks[i]))
            {
                continue;
            }
            for (int j = i + 1; j < requiredPerks.Count; j++)
            {
                if (currentPerkValues[requiredPerks[j]] <= 0 || !currentData[requiredPerks[i]].ContainsKey(requiredPerks[j]))
                {
                    continue;
                }
                for (int k = j + 1; k < requiredPerks.Count; k++)
                {
                    if (currentPerkValues[requiredPerks[k]] <= 0 || !currentData[requiredPerks[i]][requiredPerks[j]].ContainsKey(requiredPerks[k]))
                    {
                        continue;
                    }
                    foreach (var armour in currentData[requiredPerks[i]][requiredPerks[j]][requiredPerks[k]])
                    {
                        Dictionary<int, int> perks = armour.Perks;
                        ReduceCurrentPerkValues(perks, currentPerkValues);
                        tempBuild.ArmourPieces[armourType] = armour.Id;
                        var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds); // Recurse
                        if (found)
                        {
                            //Console.WriteLine(armour.Id);
                            return found;
                        }
                        IncreaseCurrentPerkValues(perks, currentPerkValues);
                    }
                }
            }
        }

        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] <= 0 || !currentData.ContainsKey(requiredPerks[i]))
            {
                continue;
            }
            for (int j = i + 1; j < requiredPerks.Count; j++)
            {
                if (currentPerkValues[requiredPerks[j]] <= 0 || !currentData[requiredPerks[i]].ContainsKey(requiredPerks[j]))
                {
                    continue;
                }
                foreach (var armour in currentData[requiredPerks[i]][requiredPerks[j]][0])
                {
                    Dictionary<int, int> perks = armour.Perks;
                    ReduceCurrentPerkValues(perks, currentPerkValues);
                    tempBuild.ArmourPieces[armourType] = armour.Id;
                    var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds); // Recurse
                    if (found)
                    {
                        //Console.WriteLine(armour.Id);
                        return found;
                    }
                    IncreaseCurrentPerkValues(perks, currentPerkValues);
                }
            }
        }

        for (int i = 0; i < requiredPerks.Count; i++)
        {
            if (currentPerkValues[requiredPerks[i]] <= 0 || !currentData.ContainsKey(requiredPerks[i]))
            {
                continue;
            }
            foreach (var armour in currentData[requiredPerks[i]][0][0])
            {
                Dictionary<int, int> perks = armour.Perks;
                ReduceCurrentPerkValues(perks, currentPerkValues);
                tempBuild.ArmourPieces[armourType] = armour.Id;
                var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds); // Recurse
                if (found)
                {
                    //Console.WriteLine(armour.Id);
                    return found;
                }
                IncreaseCurrentPerkValues(perks, currentPerkValues);
            }
        }

        tempBuild.ArmourPieces[armourType] = 0; // A armour ID of 0 means any piece
        var foundGeneric = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, builds); // Recurse
        if (foundGeneric)
        {
            //Console.WriteLine($"Generic {armourType}");
            return foundGeneric;
        }

        return false;
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
}
