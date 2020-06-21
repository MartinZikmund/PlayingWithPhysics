using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Physics.Shared.SelfStudy.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContentType
    {
        Chapter,
        Text,
        Image,
        AdditionalResources,
        KnowledgeCheck,
        Literature,
        RealWorld,
        Tasks,
        ToRemember
    }
}
