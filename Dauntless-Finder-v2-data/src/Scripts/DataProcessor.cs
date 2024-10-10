using CsvHelper;
using Dauntless_Finder_v2.src.Enums;
using Dauntless_Finder_v2.src.Models;
using System.Globalization;
using System.Text.Json;

namespace Dauntless_Finder_v2.src.Scripts;

public class DataProcessor
{
    public static Data GenerateJsonData()
    {
        Data data = ReadCSVData();
        WriteData(data);
        return data;
    }

    private static Data ReadCSVData()
    {
        using (var reader = new StreamReader("Armour-Data.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<CSVData>().ToList();

            var data = new Data();
            AddPerks(data, records);
            AddArmour(data, records);
            return data;
        }
    }

    private static void AddPerks(Data data, IEnumerable<CSVData> records)
    {
        data.Perks = new Dictionary<int, string>();

        int id = 0;
        foreach (var record in records)
        {
            if (!data.Perks.ContainsValue(record.A))
            {
                data.Perks.Add(id++, record.A);
            }
            if (!data.Perks.ContainsValue(record.B))
            {
                data.Perks.Add(id++, record.B);
            }
            if (!data.Perks.ContainsValue(record.C))
            {
                data.Perks.Add(id++, record.C);
            }
            if (!data.Perks.ContainsValue(record.D))
            {
                data.Perks.Add(id++, record.D);
            }
        }
    }

    private static void AddArmour(Data data, IEnumerable<CSVData> records)
    {
        data.Armour = new Dictionary<int, Armour>();
        Dictionary<string, int> perkValues = new Dictionary<string, int>();
        foreach (var perk in data.Perks)
        {
            perkValues.Add(perk.Value, perk.Key);
        }

        int id = 0;
        foreach (var record in records)
        {
            data.Armour[id] = new Armour
            {
                Id = id++,
                Name = record.Name,
                Type = ArmourType.Helm,
                Element = record.Element,
                Perks = new Dictionary<int, int>()
                {
                    [perkValues[record.A]] = 2,
                    [perkValues[record.B]] = 3,
                    [perkValues[record.C]] = 2
                },
                CellSlots = 1
            };

            data.Armour[id] = new Armour
            {
                Id = id++,
                Name = record.Name,
                Type = ArmourType.Torso,
                Element = record.Element,
                Perks = new Dictionary<int, int>()
                {
                    [perkValues[record.A]] = 3,
                    [perkValues[record.B]] = 2
                },
                CellSlots = 2
            };

            data.Armour[id] = new Armour
            {
                Id = id++,
                Name = record.Name,
                Type = ArmourType.Arms,
                Element = record.Element,
                Perks = new Dictionary<int, int>()
                {
                    [perkValues[record.A]] = 2,
                    [perkValues[record.B]] = 2,
                    [perkValues[record.C]] = 2
                },
                CellSlots = 1
            };

            data.Armour[id] = new Armour
            {
                Id = id++,
                Name = record.Name,
                Type = ArmourType.Legs,
                Element = record.Element,
                Perks = new Dictionary<int, int>()
                {
                    [perkValues[record.A]] = 3,
                    [perkValues[record.D]] = 2
                },
                CellSlots = 2
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
