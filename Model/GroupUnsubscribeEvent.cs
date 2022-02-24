using Newtonsoft.Json;

namespace TwilioPOC.Model
{
    public class GroupUnsubscribeEvent : ClickEvent
    {
        [JsonProperty("asm_group_id")]
        public int AsmGroupId { get; set; }
    }
}
