using Newtonsoft.Json;

namespace TwilioPOC.Model
{
    public class EventClass
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("sg_event_id")]
        public string SgEventId { get; set; }

        [JsonProperty("sg_message_id")]
        public string SgMessageId { get; set; }

        [JsonProperty("smtp-id")]
        public string SmtpId { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
