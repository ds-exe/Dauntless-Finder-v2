using Dauntless_Finder_v2.Shared.src.Enums;
using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;

namespace Dauntless_Finder_v2.DataHandler.src.Scripts;

public class GenerateIndexedData
{
    public static void GenerateJsonData()
    {
        ArmourData? armourData = GenerateArmourData();
        FileHandler.WriteData(armourData, "armour-data.json");
    }

    public static ArmourData? GenerateArmourData()
    {
        Data? data = FileHandler.ReadData<Data>("data.json");
        if (data == null)
        {
            return null;
        }

        ArmourData armourData = AddArmour(data);
        return armourData;
    }

    private static ArmourData AddArmour(Data data)
    {
        ArmourData armourData = new ArmourData();
        foreach (var armourKeyValue in data.Armours)
        {
            var armour = armourKeyValue.Value;
            AddArmour(armourData, armour);
        }

        SortArmourData(armourData);

        return armourData;
    }

    private static void AddArmour(ArmourData armourData, Armour armour)
    {
        var basicArmour = new BasicArmour()
        {
            Id = armour.Id,
            Perks = armour.Stats[armour.Stats.Count - 1].Perks
        };

        var perks = basicArmour.Perks.Select(rec => rec.Key).ToList();
        perks.Sort();

        switch (armour.Type)
        {
            case ArmourType.HEAD:
                InitialiseDictionary(armourData.Heads, perks[0], perks[1], perks[2]);
                armourData.Heads[perks[0]][perks[1]][perks[2]].Add(basicArmour);
                InitialiseDictionary(armourData.Heads, perks[0], perks[1], 0);
                armourData.Heads[perks[0]][perks[1]][0].Add(basicArmour);
                InitialiseDictionary(armourData.Heads, perks[0], perks[2], 0);
                armourData.Heads[perks[0]][perks[2]][0].Add(basicArmour);
                InitialiseDictionary(armourData.Heads, perks[0], 0, 0);
                armourData.Heads[perks[0]][0][0].Add(basicArmour);

                InitialiseDictionary(armourData.Heads, perks[1], perks[2], 0);
                armourData.Heads[perks[1]][perks[2]][0].Add(basicArmour);
                InitialiseDictionary(armourData.Heads, perks[1], 0, 0);
                armourData.Heads[perks[1]][0][0].Add(basicArmour);

                InitialiseDictionary(armourData.Heads, perks[2], 0, 0);
                armourData.Heads[perks[2]][0][0].Add(basicArmour);
                break;
            case ArmourType.TORSO:
                InitialiseDictionary(armourData.Torsos, perks[0], perks[1]);
                armourData.Torsos[perks[0]][perks[1]].Add(basicArmour);
                InitialiseDictionary(armourData.Torsos, perks[0], 0);
                armourData.Torsos[perks[0]][0].Add(basicArmour);

                InitialiseDictionary(armourData.Torsos, perks[1], 0);
                armourData.Torsos[perks[1]][0].Add(basicArmour);
                break;
            case ArmourType.ARMS:
                InitialiseDictionary(armourData.Arms, perks[0], perks[1], perks[2]);
                armourData.Arms[perks[0]][perks[1]][perks[2]].Add(basicArmour);
                InitialiseDictionary(armourData.Arms, perks[0], perks[1], 0);
                armourData.Arms[perks[0]][perks[1]][0].Add(basicArmour);
                InitialiseDictionary(armourData.Arms, perks[0], perks[2], 0);
                armourData.Arms[perks[0]][perks[2]][0].Add(basicArmour);
                InitialiseDictionary(armourData.Arms, perks[0], 0, 0);
                armourData.Arms[perks[0]][0][0].Add(basicArmour);

                InitialiseDictionary(armourData.Arms, perks[1], perks[2], 0);
                armourData.Arms[perks[1]][perks[2]][0].Add(basicArmour);
                InitialiseDictionary(armourData.Arms, perks[1], 0, 0);
                armourData.Arms[perks[1]][0][0].Add(basicArmour);

                InitialiseDictionary(armourData.Arms, perks[2], 0, 0);
                armourData.Arms[perks[2]][0][0].Add(basicArmour);
                break;
            case ArmourType.LEGS:
                InitialiseDictionary(armourData.Legs, perks[0], perks[1]);
                armourData.Legs[perks[0]][perks[1]].Add(basicArmour);
                InitialiseDictionary(armourData.Legs, perks[0], 0);
                armourData.Legs[perks[0]][0].Add(basicArmour);

                InitialiseDictionary(armourData.Legs, perks[1], 0);
                armourData.Legs[perks[1]][0].Add(basicArmour);
                break;
        }
    }

    private static void InitialiseDictionary(Dictionary<int, Dictionary<int, Dictionary<int, List<BasicArmour>>>> dict, int key, int key2, int key3)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new Dictionary<int, Dictionary<int, List<BasicArmour>>>());
        }

        InitialiseDictionary(dict[key], key2, key3);
    }

    private static void InitialiseDictionary(Dictionary<int, Dictionary<int, List<BasicArmour>>> dict, int key, int key2)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new Dictionary<int, List<BasicArmour>>());
        }

        InitialiseDictionary(dict[key], key2);
    }

    protected static void SortArmourData(ArmourData armourData)
    {
        foreach (var dict in armourData.Heads)
        {
            if (dict.Value.ContainsKey(0))
            {
                if (dict.Value[0].ContainsKey(0))
                {
                    dict.Value[0][0] = dict.Value[0][0].OrderByDescending(rec => rec.Perks[dict.Key]).ToList();
                }
            }
        }

        foreach (var dict in armourData.Torsos)
        {
            if (dict.Value.ContainsKey(0))
            {
                dict.Value[0] = dict.Value[0].OrderByDescending(rec => rec.Perks[dict.Key]).ToList();
            }
        }

        foreach (var dict in armourData.Arms)
        {
            if (dict.Value.ContainsKey(0))
            {
                if (dict.Value[0].ContainsKey(0))
                {
                    dict.Value[0][0] = dict.Value[0][0].OrderByDescending(rec => rec.Perks[dict.Key]).ToList();
                }
            }
        }

        foreach (var dict in armourData.Legs)
        {
            if (dict.Value.ContainsKey(0))
            {
                dict.Value[0] = dict.Value[0].OrderByDescending(rec => rec.Perks[dict.Key]).ToList();
            }
        }
    }

    private static void InitialiseDictionary(Dictionary<int, List<BasicArmour>> dict, int key)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new List<BasicArmour>());
        }
    }
}
