using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Message;

namespace TwilioPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TiwlioMessagesController : ControllerBase
    {
        private readonly string publicHostUrl = "https://b817-190-27-34-113.ngrok.io";

        private readonly ISendGridClient _sendGridClient;

        private readonly ILogger<TiwlioMessagesController> _logger;

        private readonly string accountSid = "ACfd1997723266ad7462b086211656bc75";
        private readonly string authToken = "7161b0dba4ad58cf52ebec5b208ca41b";


        public TiwlioMessagesController(ILogger<TiwlioMessagesController> logger, ISendGridClient sendGridClient)
        {
            _logger = logger;
            _sendGridClient = sendGridClient;
        }

        [HttpGet("{messageType}")]
        public async Task<string> GetAsync(int messageType)
        {
            if (messageType <= -1)
                throw new ArgumentException(" cannot be null or empty.");

            if (messageType == 1)
            {
                SendSMS();
            }
            else if (messageType == 2)
            {
                SendWhatsApp();
            }
            else if (messageType == 3)
            {
                await SendEmailWithTemplateAndCustomFieldsAsync();

                await SendEmailWithTemplateAndCustomFieldsAndTrackingAsync();

                await SendEmailWithTemplateAsync();
                await SendEmailWithTemplateAndTrackingAsync();

                await SendEmailWithOutTemplateAsync();
                await SendEmailWithOutTemplateAndTrackingAsync();
            }
            return "mensaje enviado";
        }

        private void SendWhatsApp()
        {
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                body: "Hello there!",
                from: new Twilio.Types.PhoneNumber("whatsapp:+19034033069"),
                to: new Twilio.Types.PhoneNumber("whatsapp:+573186496074")
            );

            Console.WriteLine(message.Sid);
        }

        private void SendSMS()
        {
            // Example get status from message
            const string feedback = "SMce7f4a23286f43958264be396ea7dbba";
            const string messagewithLink = "SMfb32b0e5fe4c4f418fbefc69a9a48642";

            var mediaUrl = new[]
            {
                new Uri("https://demo.twilio.com/owl.png")
            }.ToList();

            string accountSid = "ACfd1997723266ad7462b086211656bc75";
            string authToken = "7161b0dba4ad58cf52ebec5b208ca41b";
            TwilioClient.Init(accountSid, authToken);


            //1.  Example with get status on callback. OK
            var messageCallBack = MessageResource.Create(
                body: "messageCallBack",
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber("+573186496074"),
                statusCallback: new Uri(publicHostUrl + "/N5NotificationSms/status")
            );

            //2.  Confirm with link
            var messageWithLink = MessageResource.Create(
                body: "Open to confirm: " + publicHostUrl + "/N5NotificationSms/getOpenLink/" + Guid.NewGuid().ToString(),
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber("+573186496074"),
                provideFeedback: true,
                statusCallback: new Uri(publicHostUrl + "/N5NotificationSms/status")
            );

            //media message
            var mediaMessage = MessageResource.Create(
                body: "hola normal message",
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber("+573186496074"),
                mediaUrl: mediaUrl
                );


            //// Send Feedback to Twilio
            const string normalsid = "MM14b598f894604b1ca2ff98f360b7dcdd";
            FeedbackResource.Create(normalsid, outcome: FeedbackResource.OutcomeEnum.Confirmed);

        }

        private async Task SendEmailWithOutTemplateAndTrackingAsync()
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
                    UtmSource = publicHostUrl + "/N5NotificationSms/getUTMLink"
                }
            };
            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + publicHostUrl + "/N5Notification/getUTMLink'>subscribir</a>",
                TrackingSettings = trackingSettings,
            };
            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            Console.WriteLine(response.Headers);
        }

        private async Task SendEmailWithOutTemplateAsync()
        {
            var from = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var to = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Javier");

            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + publicHostUrl + "/N5Notification/getUTMLink'>subscribir</a>",
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            Console.WriteLine(response.Headers);
        }

        private async Task SendEmailWithTemplateAsync()
        {
            var from = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var to = new EmailAddress("norberto.castellanos@n5now.com", "Norberto Castellanos");
            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + publicHostUrl + "/N5Notification/getUTMLink'>subscribir</a>",
                TemplateId = "d-20f7065ae52a4412b9fed5f7796d030e"
            };

            msg.Contents = new List<Content>(){
                new Content(){
                    Type = "text/html",
                }
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var client = new SendGridClient("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");
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            Console.WriteLine(response.Headers);
        }

        private async Task SendEmailWithTemplateAndCustomFieldsAsync()
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
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + publicHostUrl + "/N5Notification/getUTMLink'>subscribir</a>",
                TemplateId = "d-20f7065ae52a4412b9fed5f7796d030e",
                Personalizations = personalizations
            };

            msg.Contents = new List<Content>(){
                new Content(){
                    Type = "text/html",
                }
            };

            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));
            var client = new SendGridClient("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");
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            Console.WriteLine(response.Headers);
        }

        private async Task SendEmailWithTemplateAndCustomFieldsAndTrackingAsync()
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
                    UtmSource = publicHostUrl + "/N5NotificationSms/getUTMLink"
                }
            };

            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + publicHostUrl + "/N5Notification/getUTMLink'>subscribir</a>",
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
            var client = new SendGridClient("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");
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            Console.WriteLine(response.Headers);
        }

        private async Task SendEmailWithTemplateAndTrackingAsync()
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
                    UtmSource = publicHostUrl + "/N5NotificationSms/getUTMLink"
                }
            };

            //FIELD_CUSTOM_01  FIELD_CUSTOM_02 FIELD_CUSTOM_03
            Dictionary<string, string> substitutionsValues = new Dictionary<string, string>
            {
                { "FIELD_CUSTOM_01", "valor para el custom field 0001" },
                { "FIELD_CUSTOM_02", "valor para el custom field 0002" },
                { "FIELD_CUSTOM_03", "1977" }
            };

            var msg = new SendGridMessage
            {
                From = from,
                ReplyTo = to,
                Subject = "Sending with Twilio SendGrid is Fun hiring for funny",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong><a href='" + publicHostUrl + "/N5Notification/getUTMLink'>subscribir</a>",
                TemplateId = "d-83a06542e45749cdbb88e75eaa78be5a",
                TrackingSettings = trackingSettings,
            };
            // msg.AddSubstitutions(substitutionsValues, 0);
            msg.AddTo(new EmailAddress("norberto.castellanos@gmail.com", "Norberto javier"));

            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(true);

            Console.WriteLine(response.Headers);
        }
    }
}
