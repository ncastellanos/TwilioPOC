using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TwilioPOC.Model
{
    public class BounceEvent : DroppedEvent
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public BounceEventType BounceType { get; set; }
    }
}
