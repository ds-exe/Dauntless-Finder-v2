using CsvHelper;
using Dauntless_Finder_v2.src.Enums;
using Dauntless_Finder_v2.src.Models;
using System.Globalization;
using System.Text.Json;

namespace Dauntless_Finder_v2.src.Scripts;

public class GetData
{
    public static Data FetchData()
    {
        Data data = ReadCSVData();
        WriteData(data);
        return data;
    }

    private static Data ReadCSVData()
    {
        using (var reader = new StreamReader("Armor-Data.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<CSVData>();

            var data = new Data();
            data.Armour = new List<Armour>();

            int id = 0;
            foreach (var record in records)
            {
                data.Armour.Add(new Armour
                {
                    Id = id++,
                    Name = record.Name,
                    Type = ArmourType.Helm,
                    Element = record.Element,
                    Perks = new Dictionary<string, int>()
                    {
                        [record.A] = 2,
                        [record.B] = 3,
                        [record.C] = 2
                    },
                    CellSlots = 1
                });

                data.Armour.Add(new Armour
                {
                    Id = id++,
                    Name = record.Name,
                    Type = ArmourType.Torso,
                    Element = record.Element,
                    Perks = new Dictionary<string, int>()
                    {
                        [record.A] = 3,
                        [record.B] = 2
                    },
                    CellSlots = 2
                });

                data.Armour.Add(new Armour
                {
                    Id = id++,
                    Name = record.Name,
                    Type= ArmourType.Arms,
                    Element = record.Element,
                    Perks = new Dictionary<string, int>()
                    {
                        [record.A] = 2,
                        [record.B] = 2,
                        [record.C] = 2
                    },
                    CellSlots = 1
                });

                data.Armour.Add(new Armour
                {
                    Id = id++,
                    Name = record.Name,
                    Type = ArmourType.Legs,
                    Element = record.Element,
                    Perks = new Dictionary<string, int>()
                    {
                        [record.A] = 3,
                        [record.D] = 2
                    },
                    CellSlots = 2
                });
            }

            return data;
        }
    }

    private static void WriteData(Data data)
    {
        string json = JsonSerializer.Serialize(data);
        File.WriteAllText("data.json", json);
    }

    public static async Task<Data?> ReadData()
    {
        using (FileStream stream = File.OpenRead("data.json"))
        {
            return await JsonSerializer.DeserializeAsync<Data>(stream);
        }
    }
}
