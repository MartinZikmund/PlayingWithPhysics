using System.Text.Json.Serialization;

namespace Physics.Shared.SelfStudy.Models.Questions
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuestionType
    {
        Select,
        Input
    }
}
