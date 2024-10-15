using CsvHelper;
using Dauntless_Finder_v2.Shared.src.Enums;
using Dauntless_Finder_v2.Shared.src.Models;
using System.Globalization;
using System.Text.Json;

namespace Dauntless_Finder_v2.DataHandler.src.Scripts;

public class GenerateData
{
    public static void GenerateJsonData()
    {
        Data data = ReadCSVData();
        WriteData(data);
    }

    private static Data ReadCSVData()
    {
        using (var perkReader = new StreamReader("Perk-Data.csv"))
        using (var perkCSV = new CsvReader(perkReader, CultureInfo.InvariantCulture))
        using (var armourReader = new StreamReader("Armour-Data.csv"))
        using (var armourCSV = new CsvReader(armourReader, CultureInfo.InvariantCulture))
        {
            var perkRecords = perkCSV.GetRecords<PerkCSVData>().ToList();
            var armourRecords = armourCSV.GetRecords<ArmourCSVData>().ToList();

            var data = new Data();
            AddPerks(data, perkRecords);
            AddArmour(data, armourRecords);
            return data;
        }
    }

    private static void AddPerks(Data data, IEnumerable<PerkCSVData> records)
    {
        data.Perks = new Dictionary<int, Perk>();

        int id = 1;
        foreach (var record in records)
        {
            var perk = new Perk()
            {
                Id = id,
                Name = record.Name,
                Threshold = record.Cost,
            };

            data.Perks.Add(id, perk);
            id++;
        }
    }

    private static void AddArmour(Data data, IEnumerable<ArmourCSVData> records)
    {
        data.Armours = new Dictionary<int, Armour>();
        Dictionary<string, int> perkValues = new Dictionary<string, int>();
        foreach (var perk in data.Perks)
        {
            perkValues.Add(perk.Value.Name, perk.Key);
        }

        int id = 1;
        foreach (var record in records)
        {
            data.Armours[id] = new Armour
            {
                Id = id++,
                Name = record.Name,
                Type = ArmourType.Head,
                Stats = new List<Stat>()
                {
                    new Stat()
                    {
                        Perks = new Dictionary<int, int>()
                        {
                            [perkValues[record.A]] = 2,
                            [perkValues[record.B]] = 3,
                            [perkValues[record.C]] = 2
                        }
                    }
                },
                Cell_Slots = 1
            };

            data.Armours[id] = new Armour
            {
                Id = id++,
                Name = record.Name,
                Type = ArmourType.Torso,
                Stats = new List<Stat>()
                {
                    new Stat()
                    {
                        Perks = new Dictionary<int, int>()
                        {
                            [perkValues[record.A]] = 3,
                            [perkValues[record.B]] = 2
                        }
                    }
                },
                Cell_Slots = 2
            };

            data.Armours[id] = new Armour
            {
                Id = id++,
                Name = record.Name,
                Type = ArmourType.Arms,
                Stats = new List<Stat>()
                {
                    new Stat()
                    {
                        Perks = new Dictionary<int, int>()
                        {
                            [perkValues[record.A]] = 2,
                            [perkValues[record.B]] = 2,
                            [perkValues[record.C]] = 2
                        }
                    }
                },
                Cell_Slots = 1
            };

            data.Armours[id] = new Armour
            {
                Id = id++,
                Name = record.Name,
                Type = ArmourType.Legs,
                Stats = new List<Stat>()
                {
                    new Stat()
                    {
                        Perks = new Dictionary<int, int>()
                        {
                            [perkValues[record.A]] = 3,
                            [perkValues[record.D]] = 2
                        }
                    }
                },
                Cell_Slots = 2
            };
        }
    }

    private static void WriteData(Data data)
    {
        string json = JsonSerializer.Serialize(data);
        string filepath = "data.json";

#if DEBUG
        filepath = "../../../../Dauntless-Finder-v2/data.json";
#endif

        File.WriteAllText(filepath, json);
    }
}
