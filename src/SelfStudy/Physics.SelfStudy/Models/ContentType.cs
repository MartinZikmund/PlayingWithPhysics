using System.Text.Json.Serialization;

namespace Physics.Shared.SelfStudy.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContentType
    {
        Chapter,
        AdditionalResources,
        KnowledgeCheck,
        Literature,
        RealWorld,
        Tasks,
        ToRemember
    }
}
