using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TwilioPOC.Converters;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace TwilioPOC.Model
{
        public class Event
        {
            public string Email { get; set; }

            [JsonConverter(typeof(UnixDateTimeConverter))]
            public DateTime Timestamp { get; set; }

            [JsonProperty("smtp-id")]
            public string SmtpId { get; set; }

            [JsonProperty("event")]
            [JsonConverter(typeof(StringEnumConverter))]
            public EventType EventType { get; set; }

            [JsonConverter(typeof(CategoryConverter))]
            public Category Category { get; set; }

            [JsonProperty("sg_event_id")]
            public string SendGridEventId { get; set; }

            [JsonProperty("sg_message_id")]
            public string SendGridMessageId { get; set; }

            public string TLS { get; set; }

            [JsonExtensionData]
            public IDictionary<string, object> UniqueArgs { get; set; }

            [JsonProperty("marketing_campaign_id")]
            public string MarketingCampainId { get; set; }

            [JsonProperty("marketing_campaign_name")]
            public string MarketingCampainName { get; set; }

    }
}
