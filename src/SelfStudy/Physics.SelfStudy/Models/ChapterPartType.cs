using System.Text.Json.Serialization;

namespace Physics.Shared.SelfStudy.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChapterPartType
    {
        Text,
        Html,
        Rtf,
        Question
    }
}
