using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwilioPOC.Types;

namespace TwilioPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendGridMailTestMessagesController : ControllerBase
    {
        private string WebHookHostConfig
        {
            get
            {
                return _configuration.GetSection("WebHookNotification").GetSection("Host").Value;
            }
        }

        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger<TiwlioSMSTestMessagesController> _logger;
        private readonly IConfiguration _configuration;

        private string ApiKey
        {
            get
            {
                return _configuration.GetValue<string>("SendGrid:Apikey");
            }
        }

        public SendGridMailTestMessagesController(ILogger<TiwlioSMSTestMessagesController> logger,
                                        ISendGridClient sendGridClient,
                                        IConfiguration configuration)
        {
            _logger = logger;
            _sendGridClient = sendGridClient;
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] EmailQueryModel emailType)
        {
            Response result = null;
            switch (emailType.EmailType)
            {
                case EmailType.WithOutTemplate:
                    result = await SendEmailWithOutTemplateAsync();
                    break;

                case EmailType.WithOutTemplateAndTracking:
                    result = await SendEmailWithOutTemplateAndTrackingAsync();
                    break;

                case EmailType.WithTemplate:
                    result = await SendEmailWithTemplateAsync();
                    break;

                case EmailType.WithTemplateAndTracking:
                    result = await SendEmailWithTemplateAndTrackingAsync();
                    break;

                case EmailType.WithTemplateAndCustomerFields:
                    result = await SendEmailWithTemplateAndCustomFieldsAsync();
                    break;

                case EmailType.WithTemplateAndCustomerAndTrackingFields:
                    result = await SendEmailWithTemplateAndCustomFieldsAndTrackingAsync();
                    break;
            }
            return Ok(result);
        }

        private async Task<Response> SendEmailWithOutTemplateAndTrackingAsync()
        {
            var from = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var to = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Javier");

            TrackingSettings trackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking()
                {
                    Enable = true,
                    EnableText = false
                },

                OpenTracking = new OpenTracking()
                {
                    Enable = true,
                    SubstitutionTag = "<n5origin>"
                },

                SubscriptionTracking = new SubscriptionTracking
                {
                    Enable = true,
                    Text = "text to insert into the text/plain portion of the message",
                    Html = "<bold>HTML to insert into the text/html portion of the message</bold>",
                    SubstitutionTag = "text to insert into the text/plain portion of the message"
                },

                Ganalytics = new Ganalytics()
                {
                    Enable = true,
                    UtmCampaign = "some campaign",
                    UtmContent = "some content",
                    UtmMedium = "some medium",
                    UtmTerm = "n5campaign",
                    UtmSource = WebHookHostConfig + "/N5NotificationSms/getUTMLink"
                }
            };
            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + WebHookHostConfig + "/N5Notification/getUTMLink'>subscribir</a>",
                TrackingSettings = trackingSettings,
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            return response;
        }

        private async Task<Response> SendEmailWithOutTemplateAsync()
        {
            var from = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var to = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Javier");
            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + WebHookHostConfig + "/N5Notification/getUTMLink'>subscribir</a>",
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            return response;
        }

        private async Task<Response> SendEmailWithTemplateAsync()
        {
            var from = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var to = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + WebHookHostConfig + "/N5Notification/getUTMLink'>subscribir</a>",
                TemplateId = "d-20f7065ae52a4412b9fed5f7796d030e"
            };

            msg.Contents = new List<Content>(){
                new Content(){
                    Type = "text/html",
                }
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var client = new SendGridClient(ApiKey);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            return response;
        }

        private async Task<Response> SendEmailWithTemplateAndCustomFieldsAsync()
        {
            var from = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var to = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            Dictionary<string, string> substitutionsValues = new Dictionary<string, string>
            {
                { "FIELD_CUSTOM_01", "valor para el custom field 0001" },
                { "FIELD_CUSTOM_02", "valor para el custom field 0002" },
                { "FIELD_CUSTOM_03", "1977" }
            };

            var personalizations = new List<Personalization>
            {
                new Personalization
                {
                    TemplateData = substitutionsValues
                }
            };

            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + WebHookHostConfig + "/N5Notification/getUTMLink'>subscribir</a>",
                TemplateId = "d-20f7065ae52a4412b9fed5f7796d030e",
                Personalizations = personalizations
            };

            msg.Contents = new List<Content>(){
                new Content(){
                    Type = "text/html",
                }
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var client = new SendGridClient(ApiKey);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            return response;
        }

        private async Task<Response> SendEmailWithTemplateAndCustomFieldsAndTrackingAsync()
        {
            var from = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var to = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            Dictionary<string, string> substitutionsValues = new Dictionary<string, string>
            {
                { "FIELD_CUSTOM_01", "valor para el custom field 0001" },
                { "FIELD_CUSTOM_02", "valor para el custom field 0002" },
                { "FIELD_CUSTOM_03", "1977" }
            };

            var personalizations = new List<Personalization>
            {
                new Personalization
                {
                    TemplateData = substitutionsValues
                }
            };

            TrackingSettings trackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking()
                {
                    Enable = true,
                    EnableText = false
                },
                OpenTracking = new OpenTracking()
                {
                    Enable = true,
                    SubstitutionTag = "<n5origin>"
                },
                SubscriptionTracking = new SubscriptionTracking
                {
                    Enable = true,
                    Text = "text to insert into the text/plain portion of the message",
                    Html = "<bold>HTML to insert into the text/html portion of the message</bold>",
                    SubstitutionTag = "text to insert into the text/plain portion of the message"
                },
                Ganalytics = new Ganalytics()
                {
                    Enable = true,
                    UtmCampaign = "some campaign",
                    UtmContent = "some content",
                    UtmMedium = "some medium",
                    UtmTerm = "n5campaign",
                    UtmSource = WebHookHostConfig + "/N5NotificationSms/getUTMLink"
                }
            };

            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + WebHookHostConfig + "/N5Notification/getUTMLink'>subscribir</a>",
                TemplateId = "d-20f7065ae52a4412b9fed5f7796d030e",
                Personalizations = personalizations,
                TrackingSettings = trackingSettings
            };

            msg.Contents = new List<Content>(){
                new Content(){
                    Type = "text/html",
                }
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var client = new SendGridClient(ApiKey);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            return response;
        }

        private async Task<Response> SendEmailWithTemplateAndTrackingAsync()
        {
            var from = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var to = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Javier");

            TrackingSettings trackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking()
                {
                    Enable = true,
                    EnableText = false
                },

                OpenTracking = new OpenTracking()
                {
                    Enable = true,
                    SubstitutionTag = "<n5origin>"
                },

                SubscriptionTracking = new SubscriptionTracking
                {
                    Enable = true,
                    Text = "text to insert into the text/plain portion of the message",
                    Html = "<bold>HTML to insert into the text/html portion of the message</bold>",
                    SubstitutionTag = "text to insert into the text/plain portion of the message"
                },

                Ganalytics = new Ganalytics()
                {
                    Enable = true,
                    UtmCampaign = "some campaign",
                    UtmContent = "some content",
                    UtmMedium = "some medium",
                    UtmTerm = "n5campaign",
                    UtmSource = WebHookHostConfig + "/N5NotificationSms/getUTMLink"
                }
            };

            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + WebHookHostConfig + "/N5Notification/getUTMLink'>subscribir</a>",
                TemplateId = "d-83a06542e45749cdbb88e75eaa78be5a",
                TrackingSettings = trackingSettings,
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(true);
            return response;
        }
    }
}
