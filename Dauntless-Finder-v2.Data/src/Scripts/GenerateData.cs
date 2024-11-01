using CsvHelper;
using Dauntless_Finder_v2.Shared.src.Enums;
using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;
using System.Globalization;

namespace Dauntless_Finder_v2.DataHandler.src.Scripts;

[Obsolete("Data now fetched from API by FetchData", true)]
public class GenerateData
{
    public static void GenerateJsonData()
    {
        Data data = ReadCSVData();
        FileHandler.WriteData(data, "data.json");
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
                Name = new Dictionary<string, string>() { { "en", record.Name } },
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
            perkValues.Add(perk.Value.Name.FirstOrDefault().Value, perk.Key);
        }

        int id = 1;
        foreach (var record in records)
        {
            data.Armours[id] = new Armour
            {
                Id = id++,
                Name = new Dictionary<string, string>() { { "en", record.Name } },
                Type = ArmourType.HEAD,
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
                CellSlots = 1
            };

            data.Armours[id] = new Armour
            {
                Id = id++,
                Name = new Dictionary<string, string>() { { "en", record.Name } },
                Type = ArmourType.TORSO,
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
                CellSlots = 2
            };

            data.Armours[id] = new Armour
            {
                Id = id++,
                Name = new Dictionary<string, string>() { { "en", record.Name } },
                Type = ArmourType.ARMS,
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
                CellSlots = 1
            };

            data.Armours[id] = new Armour
            {
                Id = id++,
                Name = new Dictionary<string, string>() { { "en", record.Name } },
                Type = ArmourType.LEGS,
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
                CellSlots = 2
            };
        }
    }
}
