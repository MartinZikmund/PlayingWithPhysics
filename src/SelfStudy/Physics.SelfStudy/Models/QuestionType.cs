using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Physics.Shared.SelfStudy.Models.Questions
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QuestionType
    {
        Select,
        Input
    }
}
