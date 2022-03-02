using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace TwilioPOC.Types
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmailType
    {
        WithOutTemplate = 1,

        WithOutTemplateAndTracking = 2,

        WithTemplate = 3,

        WithTemplateAndTracking = 4,

        WithTemplateAndCustomerFields = 5,

        WithTemplateAndCustomerAndTrackingFields = 6
    }
}
