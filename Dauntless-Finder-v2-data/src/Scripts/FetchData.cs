using Dauntless_Finder_v2.Shared.src.Models;
using Dauntless_Finder_v2.Shared.src.Scripts;
using System.Text.Json;

namespace Dauntless_Finder_v2.data.src.Scripts;

public class FetchData
{
    public static async Task GetData()
    {
        try
        {
            Config? config = FileHandler.ReadData<Config>("config.json");
            if (config == null)
            {
                return;
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Phalanx-Api-Key", config.ApiKey);
            HttpResponseMessage response = await client.GetAsync(config.Url);
            if (response.IsSuccessStatusCode)
            {
                var dataString = await response.Content.ReadAsStringAsync();

                var serializeOptions = FileHandler.GetJsonSerializerOptions();
                var data = JsonSerializer.Deserialize<Data>(dataString, serializeOptions);

                FileHandler.WriteData(data, "data.json");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
