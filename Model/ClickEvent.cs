using Newtonsoft.Json;
using System;
using TwilioPOC.Converters;

namespace TwilioPOC.Model
{
    public class ClickEvent : OpenEvent
    {
        [JsonConverter(typeof(UriConverter))]
        public Uri Url { get; set; }
    }
}
