using Dauntless_Finder_v2.Shared.src.Enums;

namespace Dauntless_Finder_v2.Shared.src.Models
{
    public class TempBuild : IEquatable<TempBuild>
    {
        public Dictionary<ArmourType, int> ArmourPieces { get; set; } = [];

        public List<int> Perks { get; set; } = [];

        public bool Equals(TempBuild? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return ArmourPieces[ArmourType.HEAD] == other.ArmourPieces[ArmourType.HEAD] &&
                ArmourPieces[ArmourType.TORSO] == other.ArmourPieces[ArmourType.TORSO] &&
                ArmourPieces[ArmourType.ARMS] == other.ArmourPieces[ArmourType.ARMS] &&
                ArmourPieces[ArmourType.LEGS] == other.ArmourPieces[ArmourType.LEGS];
        }

        public override bool Equals(object? obj)
        {
            return obj is Build build && Equals(build);
        }

        public override int GetHashCode()
        {
            return $"{ArmourPieces[ArmourType.HEAD]}{ArmourPieces[ArmourType.TORSO]}{ArmourPieces[ArmourType.ARMS]}{ArmourPieces[ArmourType.LEGS]}".GetHashCode();
        }
    }
}
