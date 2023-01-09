using System.Text.Json.Serialization;

namespace RPG_game_dotnet.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3,
        Assassin = 4,
        Thief = 5,
        Ranger = 6,
        Paladin = 7,
        Warlock = 8
    }
}