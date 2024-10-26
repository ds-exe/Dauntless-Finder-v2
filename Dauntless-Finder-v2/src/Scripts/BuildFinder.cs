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
        List<TempBuild> tempBuilds = [];
        FindArmourPiece(armourType, requiredPerks, maxBuilds, currentPerkValues, new TempBuild(), tempBuilds);

        List<Build> builds = [];
        foreach (var tempBuild in tempBuilds)
        {
            builds.Add(CreateBuild(tempBuild));
        }

        return builds;
    }

    protected Build CreateBuild(TempBuild tempBuild)
    {
        Build build = new();

        build.Head = new BuildItem()
        {
            Id = tempBuild.ArmourPieces[ArmourType.HEAD]
        };
        if (tempBuild.Perks.Count > 0)
        {
            build.Head.Cells.Add(tempBuild.Perks.FirstOrDefault());
            tempBuild.Perks.RemoveAt(0);
        }
        build.Torso = new BuildItem()
        {
            Id = tempBuild.ArmourPieces[ArmourType.TORSO]
        };
        if (tempBuild.Perks.Count > 0)
        {
            build.Torso.Cells.Add(tempBuild.Perks.FirstOrDefault());
            tempBuild.Perks.RemoveAt(0);
            if (tempBuild.Perks.Count > 0)
            {
                build.Torso.Cells.Add(tempBuild.Perks.FirstOrDefault());
                tempBuild.Perks.RemoveAt(0);
            }
        }
        build.Arms = new BuildItem()
        {
            Id = tempBuild.ArmourPieces[ArmourType.ARMS]
        };
        if (tempBuild.Perks.Count > 0)
        {
            build.Arms.Cells.Add(tempBuild.Perks.FirstOrDefault());
            tempBuild.Perks.RemoveAt(0);
        }
        build.Legs = new BuildItem()
        {
            Id = tempBuild.ArmourPieces[ArmourType.LEGS]
        };
        if (tempBuild.Perks.Count > 0)
        {
            build.Legs.Cells.Add(tempBuild.Perks.FirstOrDefault());
            tempBuild.Perks.RemoveAt(0);
            if (tempBuild.Perks.Count > 0)
            {
                build.Legs.Cells.Add(tempBuild.Perks.FirstOrDefault());
                tempBuild.Perks.RemoveAt(0);
            }
        }

        return build;
    }

    protected bool FindArmourPiece(ArmourType armourType, List<int> requiredPerks, int maxBuilds, Dictionary<int, int> currentPerkValues, TempBuild tempBuild, List<TempBuild> tempBuilds)
    {
        if (tempBuilds.Count >= maxBuilds)
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
            AddBuild(tempBuild, tempBuilds, currentPerkValues, maxBuilds);
            return true;
        }
        if (!isDefined)
        {
            var emptyCellSlots = maxEmptyCellSlots - totalPerkThreshold;
            var validBuild = emptyCellSlots >= 0;
            if (validBuild)
            {
                AddBuild(tempBuild, tempBuilds, currentPerkValues, maxBuilds);
            }
            return validBuild;
        }

        switch (armourType)
        {
            case ArmourType.HEAD:
            case ArmourType.ARMS:
                return FindArmourPiece3Perk(armourType, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds);
            case ArmourType.TORSO:
            case ArmourType.LEGS:
                return FindArmourPiece2Perk(armourType, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds);
        }

        return false;
    }

    protected bool FindArmourPiece2Perk(ArmourType armourType, List<int> requiredPerks, int maxBuilds, Dictionary<int, int> currentPerkValues, TempBuild tempBuild, List<TempBuild> tempBuilds)
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
                    var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds); // Recurse
                    //if (found)
                    //{
                    //    //Console.WriteLine(armour.Id);
                    //    return found;
                    //}
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
                var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds); // Recurse
                //if (found)
                //{
                //    //Console.WriteLine(armour.Id);
                //    return found;
                //}
                IncreaseCurrentPerkValues(perks, currentPerkValues);
            }
        }

        tempBuild.ArmourPieces[armourType] = 0; // A armour ID of 0 means any piece
        var foundGeneric = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds); // Recurse
        //if (foundGeneric)
        //{
        //    //Console.WriteLine($"Generic {armourType}");
        //    return foundGeneric;
        //}

        return false;
    }

    protected bool FindArmourPiece3Perk(ArmourType armourType, List<int> requiredPerks, int maxBuilds, Dictionary<int, int> currentPerkValues, TempBuild tempBuild, List<TempBuild> tempBuilds)
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
                        var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds); // Recurse
                        //if (found)
                        //{
                        //    //Console.WriteLine(armour.Id);
                        //    return found;
                        //}
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
                    var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds); // Recurse
                    //if (found)
                    //{
                    //    //Console.WriteLine(armour.Id);
                    //    return found;
                    //}
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
                var found = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds); // Recurse
                //if (found)
                //{
                //    //Console.WriteLine(armour.Id);
                //    return found;
                //}
                IncreaseCurrentPerkValues(perks, currentPerkValues);
            }
        }

        tempBuild.ArmourPieces[armourType] = 0; // A armour ID of 0 means any piece
        var foundGeneric = FindArmourPiece(armourType + 1, requiredPerks, maxBuilds, currentPerkValues, tempBuild, tempBuilds); // Recurse
        //if (foundGeneric)
        //{
        //    //Console.WriteLine($"Generic {armourType}");
        //    return foundGeneric;
        //}

        return false;
    }

    protected void AddBuild(TempBuild tempBuild, List<TempBuild> tempBuilds, Dictionary<int, int> currentPerkValues, int maxBuilds)
    {
        if (tempBuilds.Contains(tempBuild))
        {
            return;
        }

        List<int> heads = [];
        if (tempBuild.ArmourPieces[ArmourType.HEAD] == 0)
        {
            heads = armourData.Heads[0][0].Select(rec => rec.Key).ToList();
        }
        else
        {
            heads.Add(tempBuild.ArmourPieces[ArmourType.HEAD]);
        }
        List<int> torsos = [];
        if (tempBuild.ArmourPieces[ArmourType.TORSO] == 0)
        {
            torsos = armourData.Torsos[0].Select(rec => rec.Key).ToList();
        }
        else
        {
            torsos.Add(tempBuild.ArmourPieces[ArmourType.TORSO]);
        }
        List<int> arms = [];
        if (tempBuild.ArmourPieces[ArmourType.ARMS] == 0)
        {
            arms = armourData.Arms[0][0].Select(rec => rec.Key).ToList();
        }
        else
        {
            arms.Add(tempBuild.ArmourPieces[ArmourType.ARMS]);
        }
        List<int> legs = [];
        if (tempBuild.ArmourPieces[ArmourType.LEGS] == 0)
        {
            legs = armourData.Legs[0].Select(rec => rec.Key).ToList();
        }
        else
        {
            legs.Add(tempBuild.ArmourPieces[ArmourType.LEGS]);
        }

        List<int> perks = [];
        foreach (var item in currentPerkValues)
        {
            for (int i = 0; i < item.Value; i++)
            {
                perks.Add(item.Key);
            }
        }

        foreach (var head in heads)
        {
            if (tempBuilds.Count >= maxBuilds)
            {
                break;
            }
            foreach (var torso in torsos)
            {
                if (tempBuilds.Count >= maxBuilds)
                {
                    break;
                }
                foreach (var arm in arms)
                {
                    if (tempBuilds.Count >= maxBuilds)
                    {
                        break;
                    }
                    foreach (var leg in legs)
                    {
                        if (tempBuilds.Count >= maxBuilds)
                        {
                            break;
                        }
                        TempBuild build = new();
                        build.ArmourPieces[ArmourType.HEAD] = head;
                        build.ArmourPieces[ArmourType.TORSO] = torso;
                        build.ArmourPieces[ArmourType.ARMS] = arm;
                        build.ArmourPieces[ArmourType.LEGS] = leg;

                        build.Perks = perks;

                        tempBuilds.Add(build);
                    }
                }
            }
        }
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
