using Dauntless_Finder_v2.Shared.src.Enums;
using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;

namespace Dauntless_Finder_v2.App.src.Scripts;

public class BruteForceFinder
{
    protected Data data { get; set; }

    protected IEnumerable<Armour> headArmour;
    protected IEnumerable<Armour> torsoArmour;
    protected IEnumerable<Armour> armsArmour;
    protected IEnumerable<Armour> legsArmour;

    protected static readonly int maxEmptyCellSlots = 6;

    public BruteForceFinder()
    {
        data = FileHandler.ReadData<Data>("data.json");
        if (data == null)
        {
            throw new Exception("Loading data failed.");
        }

        headArmour = data.Armours.Select(rec => rec.Value).Where(rec => rec.Type == ArmourType.HEAD);
        torsoArmour = data.Armours.Select(rec => rec.Value).Where(rec => rec.Type == ArmourType.TORSO);
        armsArmour = data.Armours.Select(rec => rec.Value).Where(rec => rec.Type == ArmourType.ARMS);
        legsArmour = data.Armours.Select(rec => rec.Value).Where(rec => rec.Type == ArmourType.LEGS);
    }

    public List<Build> GetAllBuilds(List<int> requiredPerks, int maxBuilds)
    {
        requiredPerks = requiredPerks.ToList();
        requiredPerks.Sort();
        List<Build> builds = new List<Build>();
        Dictionary<int, int> currentPerkValues = GetCurrentPerkValues(requiredPerks);
        foreach (var head in headArmour)
        {
            if (builds.Count >= maxBuilds)
            {
                break;
            }
            ReduceCurrentPerkValues(head.Stats[head.Stats.Count - 1].Perks, currentPerkValues);
            foreach (var torso in torsoArmour)
            {
                if (builds.Count >= maxBuilds)
                {
                    break;
                }
                ReduceCurrentPerkValues(torso.Stats[torso.Stats.Count - 1].Perks, currentPerkValues);
                foreach (var arms in armsArmour)
                {
                    if (builds.Count >= maxBuilds)
                    {
                        break;
                    }
                    ReduceCurrentPerkValues(arms.Stats[arms.Stats.Count - 1].Perks, currentPerkValues);
                    foreach (var legs in legsArmour)
                    {
                        if (builds.Count >= maxBuilds)
                        {
                            break;
                        }
                        ReduceCurrentPerkValues(legs.Stats[legs.Stats.Count - 1].Perks, currentPerkValues);

                        var isComplete = IsBuildComplete(requiredPerks, currentPerkValues);
                        if (isComplete)
                        {
                            Console.WriteLine($"{head.Name["en"]} {torso.Name["en"]} {arms.Name["en"]} {legs.Name["en"]}");
                            // TODO: Also add cells for better testing
                            builds.Add(new Build()
                            {
                                Head = new BuildItem()
                                {
                                    Id = head.Id,
                                },
                                Torso = new BuildItem()
                                {
                                    Id= torso.Id,
                                },
                                Arms = new BuildItem()
                                {
                                    Id = arms.Id,
                                },
                                Legs = new BuildItem()
                                {
                                    Id = legs.Id,
                                }
                            });
                            //return true;
                        }
                        IncreaseCurrentPerkValues(legs.Stats[legs.Stats.Count - 1].Perks, currentPerkValues);
                    }
                    IncreaseCurrentPerkValues(arms.Stats[arms.Stats.Count - 1].Perks, currentPerkValues);
                }
                IncreaseCurrentPerkValues(torso.Stats[torso.Stats.Count - 1].Perks, currentPerkValues);
            }
            IncreaseCurrentPerkValues(head.Stats[head.Stats.Count - 1].Perks, currentPerkValues);
        }

        return builds;
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

    protected bool IsBuildComplete(List<int> requiredPerks, Dictionary<int, int> currentPerkValues)
    {
        int totalPerkThreshold = 0;
        foreach (var requiredPerk in requiredPerks)
        {
            if (currentPerkValues[requiredPerk] > 0)
            {
                totalPerkThreshold += currentPerkValues[requiredPerk];
            }
        }

        var emptyCellSlots = maxEmptyCellSlots - totalPerkThreshold;
        return emptyCellSlots >= 0;
    }
}

