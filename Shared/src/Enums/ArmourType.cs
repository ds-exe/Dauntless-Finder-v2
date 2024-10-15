using System.Text.Json.Serialization;

namespace Dauntless_Finder_v2.Shared.src.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ArmourType
{
    Head,
    Torso,
    Arms,
    Legs
}
