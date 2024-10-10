using CsvHelper;
using Dauntless_Finder_v2.Models;
using System.Globalization;
using System.Text.Json;

namespace Dauntless_Finder_v2;

public class GetData
{
    public static async Task<Data?> FetchData()
    {
        if (File.Exists("data.json"))
        {
            return await ReadData();
        }

        Data data = ReadCSVData();
        WriteData(data);
        return data;
    }

    static Data ReadCSVData()
    {
        using (var reader = new StreamReader("Armor-Data.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<CSVData>();

            var data = new Data();
            data.Helms = new List<Armour>();
            data.Torsos = new List<Armour>();
            data.Arms = new List<Armour>();
            data.Legs = new List<Armour>();
            foreach (var record in records)
            {
                data.Helms.Add(new Armour
                {
                    Name = record.Name,
                    Element = record.Element,
                    Perks = new Dictionary<string, int>()
                    {
                        [record.A] = 2,
                        [record.B] = 3,
                        [record.C] = 2
                    },
                    CellSlots = 1
                });

                data.Torsos.Add(new Armour
                {
                    Name = record.Name,
                    Element = record.Element,
                    Perks = new Dictionary<string, int>()
                    {
                        [record.A] = 3,
                        [record.B] = 2
                    },
                    CellSlots = 2
                });

                data.Arms.Add(new Armour
                {
                    Name = record.Name,
                    Element = record.Element,
                    Perks = new Dictionary<string, int>()
                    {
                        [record.A] = 2,
                        [record.B] = 2,
                        [record.C] = 2
                    },
                    CellSlots = 1
                });

                data.Legs.Add(new Armour
                {
                    Name = record.Name,
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

    static void WriteData(Data data)
    {
        string json = JsonSerializer.Serialize(data);
        File.WriteAllText("data.json", json);
    }

    static async Task<Data?> ReadData()
    {
        using (FileStream stream = File.OpenRead("data.json"))
        {
            return await JsonSerializer.DeserializeAsync<Data>(stream);
        }
    }
}
