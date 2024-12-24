using System.Text.Json.Serialization;

namespace 宏碁班專案_社交媒體平台MVC.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PostReactionType
    {
        Like = 0,
        Love,
        Angry,
        Sad,
        Haha,
        Wow,
    }
}
