using Dauntless_Finder_v2.Shared.src.Enums;
using Dauntless_Finder_v2.Shared.src.Models;
using System.Text.Json;

namespace Dauntless_Finder_v2.DataHandler.src.Scripts;

public class GenerateIndexedData
{
    public static void GenerateArmourData()
    {
        Data? data = ReadData();
        if (data == null)
        {
            return;
        }

        ArmourData armourData = AddArmour(data);
        WriteArmourData(armourData);
    }

    private static ArmourData AddArmour(Data data)
    {
        ArmourData armourData = new ArmourData();
        foreach (var armourKeyValue in data.Armour)
        {
            var armour = armourKeyValue.Value;
            AddArmour(armourData, armour);
        }

        return armourData;
    }

    private static void AddArmour(ArmourData armourData, Armour armour)
    {
        var perks = armour.Perks.ToList();
        switch (armour.ArmourType)
        {
            case ArmourType.Helm:
                InitialiseDictionary(armourData.Helms, perks[0].Key, perks[1].Key, perks[2].Key);
                armourData.Helms[perks[0].Key][perks[1].Key][perks[2].Key].Add(armour.Id);
                InitialiseDictionary(armourData.Helms, perks[1].Key, perks[2].Key, perks[0].Key);
                armourData.Helms[perks[1].Key][perks[2].Key][perks[0].Key].Add(armour.Id);
                InitialiseDictionary(armourData.Helms, perks[2].Key, perks[0].Key, perks[1].Key);
                armourData.Helms[perks[2].Key][perks[0].Key][perks[1].Key].Add(armour.Id);
                break;
            case ArmourType.Torso:
                InitialiseDictionary(armourData.Torsos, perks[0].Key, perks[1].Key);
                armourData.Torsos[perks[0].Key][perks[1].Key].Add(armour.Id);
                InitialiseDictionary(armourData.Torsos, perks[1].Key, perks[0].Key);
                armourData.Torsos[perks[1].Key][perks[0].Key].Add(armour.Id);
                break;
            case ArmourType.Arms:
                InitialiseDictionary(armourData.Arms, perks[0].Key, perks[1].Key, perks[2].Key);
                armourData.Arms[perks[0].Key][perks[1].Key][perks[2].Key].Add(armour.Id);
                InitialiseDictionary(armourData.Arms, perks[1].Key, perks[2].Key, perks[0].Key);
                armourData.Arms[perks[1].Key][perks[2].Key][perks[0].Key].Add(armour.Id);
                InitialiseDictionary(armourData.Arms, perks[2].Key, perks[0].Key, perks[1].Key);
                armourData.Arms[perks[2].Key][perks[0].Key][perks[1].Key].Add(armour.Id);
                break;
            case ArmourType.Legs:
                InitialiseDictionary(armourData.Legs, perks[0].Key, perks[1].Key);
                armourData.Legs[perks[0].Key][perks[1].Key].Add(armour.Id);
                InitialiseDictionary(armourData.Legs, perks[1].Key, perks[0].Key);
                armourData.Legs[perks[1].Key][perks[0].Key].Add(armour.Id);
                break;
        }
    }

    private static void InitialiseDictionary(Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> dict, int key, int key2, int key3)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new Dictionary<int, Dictionary<int, List<int>>>());
        }

        InitialiseDictionary(dict[key], key2, key3);
    }

    private static void InitialiseDictionary(Dictionary<int, Dictionary<int, List<int>>> dict, int key, int key2)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new Dictionary<int, List<int>>());
        }

        InitialiseDictionary(dict[key], key2);
    }

    private static void InitialiseDictionary(Dictionary<int, List<int>> dict, int key)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new List<int>());
        }
    }

    public static Data? ReadData()
    {
        string filepath = "data.json";
        #if DEBUG
        filepath = "../../../../Dauntless-Finder-v2/data.json";
        #endif

        string txt = File.ReadAllText(filepath);
        Data? result = JsonSerializer.Deserialize<Data>(txt);
        return result;
    }

    private static void WriteArmourData(ArmourData armourData)
    {
        string json = JsonSerializer.Serialize(armourData);
        string filepath = "armour-data.json";

        #if DEBUG
        filepath = "../../../../Dauntless-Finder-v2/armour-data.json";
        #endif

        File.WriteAllText(filepath, json);
    }
}
