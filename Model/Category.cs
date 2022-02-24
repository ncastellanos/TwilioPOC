using Newtonsoft.Json;

namespace TwilioPOC.Model
{
    public class Category
    {
        public string[] Value { get; }
        private JsonToken _jsonToken;

        public Category(string[] value, JsonToken jsonToken)
        {
            Value = value;
            _jsonToken = jsonToken;
        }

        [JsonIgnore]
        public bool IsArray => _jsonToken == JsonToken.StartArray;
    }
}
