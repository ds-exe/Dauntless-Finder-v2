using System.Text.Json;

namespace Dauntless_Finder_v2.Shared.src.Scripts;

public class FileHandler
{
    public static T? ReadData<T>(string path)
    {
        #if DEBUG
        path = $"../../../../Dauntless-Finder-v2/{path}";
        #endif

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };
        string txt = File.ReadAllText(path);
        T? result = JsonSerializer.Deserialize<T>(txt, serializeOptions);
        return result;
    }

    public static void WriteData<T>(T data, string path)
    {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };
        string json = JsonSerializer.Serialize(data, serializeOptions);

        #if DEBUG
        path = $"../../../../Dauntless-Finder-v2/{path}";
        #endif

        File.WriteAllText(path, json);
    }
}
