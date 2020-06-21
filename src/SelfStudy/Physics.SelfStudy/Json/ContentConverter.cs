using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Physics.SelfStudy.Models;
using Physics.SelfStudy.Models.Contents;
using Physics.Shared.SelfStudy.Models;
using System;

namespace Physics.SelfStudy.Json
{
    public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(IContent).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    public class ContentConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IContent));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            var jsonString = jo.ToString();
            var typeString = jo["Type"].Value<string>();
            var contentType = Enum.Parse<ContentType>(typeString);
            switch (contentType)
            {
                case ContentType.Chapter:
                    return JsonConvert.DeserializeObject<Chapter>(jsonString);
                case ContentType.AdditionalResources:
                    return JsonConvert.DeserializeObject<AdditionalResourcesContent>(jsonString);
                case ContentType.KnowledgeCheck:
                    return JsonConvert.DeserializeObject<KnowledgeCheckContent>(jsonString);
                case ContentType.Image:
                    return JsonConvert.DeserializeObject<ImageContent>(jsonString);
                case ContentType.Literature:
                    return JsonConvert.DeserializeObject<LiteratureContent>(jsonString);
                case ContentType.RealWorld:
                    return JsonConvert.DeserializeObject<RealWorldContent>(jsonString);
                case ContentType.Tasks:
                    return JsonConvert.DeserializeObject<TasksContent>(jsonString);
                case ContentType.ToRemember:
                    return JsonConvert.DeserializeObject<ToRememberContent>(jsonString);
                case ContentType.Text:
                    return JsonConvert.DeserializeObject<ParagraphContent>(jsonString);
                default:
                    throw new InvalidOperationException("Unsupported content type.");
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
    //public class ContentConverter : JsonConverter<IContent>
    //{
    //    public override bool CanConvert(Type type)
    //    {
    //        return typeof(IContent) == type;
    //    }

    //    public override IContent Read(
    //        ref Utf8JsonReader reader,
    //        Type typeToConvert,
    //        JsonSerializerOptions options)
    //    {
    //        var document = JsonDocument.ParseValue(ref reader);
    //        ContentType contentType;
    //        if (!document.RootElement.TryGetProperty("Type", out var objectProperty) || !Enum.TryParse<ContentType>(objectProperty.ToString(), out contentType))
    //        {
    //            throw new JsonException("Type not formatted properly");

    //        }
    //        switch (contentType)
    //        {
    //            case ContentType.Chapter:
    //                return JsonSerializer.Deserialize<IContent>(document.RootElement.GetRawText());
    //            case ContentType.AdditionalResources:
    //                return JsonSerializer.Deserialize<IContent>(document.RootElement.GetRawText());
    //            case ContentType.KnowledgeCheck:
    //                return JsonSerializer.Deserialize<IContent>(document.RootElement.GetRawText());
    //            case ContentType.Literature:
    //                return JsonSerializer.Deserialize<IContent>(document.RootElement.GetRawText());
    //            case ContentType.RealWorld:
    //                return JsonSerializer.Deserialize<IContent>(document.RootElement.GetRawText());
    //            case ContentType.Tasks:
    //                return JsonSerializer.Deserialize<IContent>(document.RootElement.GetRawText());
    //            case ContentType.ToRemember:
    //                return JsonSerializer.Deserialize<IContent>(document.RootElement.GetRawText());
    //            default:
    //                throw new InvalidOperationException("Unsupported content type.");
    //        }
    //    }

    //    public override void Write(
    //            Utf8JsonWriter writer,
    //            IContent value,
    //            JsonSerializerOptions options)
    //    {
    //        JsonSerializer.Serialize(writer, value);
    //    }
    //}
}
