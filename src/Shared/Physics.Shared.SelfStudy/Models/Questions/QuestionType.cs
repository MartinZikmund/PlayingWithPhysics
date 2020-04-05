using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Physics.Shared.SelfStudy.Models.Questions
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuestionType
    {
        Select,
        Input
    }
}
