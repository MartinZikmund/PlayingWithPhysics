using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Physics.Shared.SelfStudy.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContentType
    {
        Chapter,
        Text,
        AdditionalResources,
        KnowledgeCheck,
        Literature,
        RealWorld,
        Tasks,
        ToRemember
    }
}
