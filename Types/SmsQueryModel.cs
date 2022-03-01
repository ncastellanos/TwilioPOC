using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace TwilioPOC.Types
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SMSType
    {
        WithoutMediaFiles = 1,
        WithMediaFiles = 2,
        WithNotificationCallBack = 3,
        WithOpenLinkconfirmation = 4,
        WithSendconfirmationToTwilio = 5
    }
}
