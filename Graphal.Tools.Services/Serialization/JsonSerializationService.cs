using Graphal.Tools.Abstractions.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Graphal.Tools.Services.Serialization
{
    public class JsonSerializationService : IJsonSerializationService
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public string Serialize<T>(T model)
        {
            return JsonConvert.SerializeObject(model, JsonSerializerSettings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, JsonSerializerSettings);
        }
    }
}