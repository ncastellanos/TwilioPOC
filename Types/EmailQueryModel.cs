using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TwilioPOC.Types
{
    public enum EmailType
    {
        WithOutTemplate,

        WithOutTemplateAndTracking,

        WithTemplate,

        WithTemplateAndTracking,

        WithTemplateAndCustomerFields,
        
        WithTemplateAndCustomerAndTrackingFields
    }

    public class EmailQueryModel
    {
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmailType EmailType { get; set; }
    }
}
