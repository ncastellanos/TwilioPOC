using System.Runtime.Serialization;

namespace TwilioPOC.Model
{
    public enum EventType
    {
        Processed,
        Deferred,
        Delivered,
        Open,
        Click,
        Bounce,
        Dropped,
        SpamReport,
        Unsubscribe,
        [EnumMember(Value = "group_unsubscribe")]
        GroupUnsubscribe,
        [EnumMember(Value = "group_resubscribe")]
        GroupResubscribe
    }
}
