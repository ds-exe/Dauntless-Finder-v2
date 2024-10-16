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

        return armourData;
    }

    private static void AddArmour(ArmourData armourData, Armour armour)
    {
        var perks = armour.Stats[armour.Stats.Count-1].Perks.ToList();
        var basicArmour = new BasicArmour()
        {
            Id = armour.Id,
            Perks = armour.Stats[armour.Stats.Count - 1].Perks
        };

        switch (armour.Type)
        {
            case ArmourType.HEAD:
                InitialiseDictionary(armourData.Helms, perks[0].Key, perks[1].Key, perks[2].Key);
                armourData.Helms[perks[0].Key][perks[1].Key][perks[2].Key].Add(basicArmour);
                InitialiseDictionary(armourData.Helms, perks[1].Key, perks[2].Key, perks[0].Key);
                armourData.Helms[perks[1].Key][perks[2].Key][perks[0].Key].Add(basicArmour);
                InitialiseDictionary(armourData.Helms, perks[2].Key, perks[0].Key, perks[1].Key);
                armourData.Helms[perks[2].Key][perks[0].Key][perks[1].Key].Add(basicArmour);
                break;
            case ArmourType.TORSO:
                InitialiseDictionary(armourData.Torsos, perks[0].Key, perks[1].Key);
                armourData.Torsos[perks[0].Key][perks[1].Key].Add(basicArmour);
                InitialiseDictionary(armourData.Torsos, perks[1].Key, perks[0].Key);
                armourData.Torsos[perks[1].Key][perks[0].Key].Add(basicArmour);
                break;
            case ArmourType.ARMS:
                InitialiseDictionary(armourData.Arms, perks[0].Key, perks[1].Key, perks[2].Key);
                armourData.Arms[perks[0].Key][perks[1].Key][perks[2].Key].Add(basicArmour);
                InitialiseDictionary(armourData.Arms, perks[1].Key, perks[2].Key, perks[0].Key);
                armourData.Arms[perks[1].Key][perks[2].Key][perks[0].Key].Add(basicArmour);
                InitialiseDictionary(armourData.Arms, perks[2].Key, perks[0].Key, perks[1].Key);
                armourData.Arms[perks[2].Key][perks[0].Key][perks[1].Key].Add(basicArmour);
                break;
            case ArmourType.LEGS:
                InitialiseDictionary(armourData.Legs, perks[0].Key, perks[1].Key);
                armourData.Legs[perks[0].Key][perks[1].Key].Add(basicArmour);
                InitialiseDictionary(armourData.Legs, perks[1].Key, perks[0].Key);
                armourData.Legs[perks[1].Key][perks[0].Key].Add(basicArmour);
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

    private static void InitialiseDictionary(Dictionary<int, List<BasicArmour>> dict, int key)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new List<BasicArmour>());
        }
    }
}
