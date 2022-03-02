using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Message;
using TwilioPOC.Types;

namespace TwilioPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TiwlioSMSTestMessagesController : ControllerBase
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger<TiwlioSMSTestMessagesController> _logger;
        private readonly IConfiguration _configuration;

        private string WebHookHostConfig
        {
            get
            {
                return _configuration.GetSection("WebHookNotification").GetSection("Host").Value;
            }
        }

        private string AccountSid
        {
            get
            {
                return _configuration.GetSection("Twilio").GetSection("accountSid").Value;
            }
        }

        private string AuthToken
        {
            get
            {
                return _configuration.GetSection("Twilio").GetSection("authToken").Value;
            }
        }

        public TiwlioSMSTestMessagesController(ILogger<TiwlioSMSTestMessagesController> logger,
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
        /// <param name="fullNumber"></param>
        /// <param name="smsType"></param>
        /// <returns></returns>
        [HttpGet("fullNumber")]
        public async Task<IActionResult> GetAsync(string fullNumber, [FromQuery] SMSType SmsType)
        {
            MessageResource result = null;
            switch (SmsType)
            {
                case SMSType.WithoutMediaFiles:
                    result = await SendWithoutMediaFiles(fullNumber);
                    break;

                case SMSType.WithMediaFiles:
                    result = await SendWithMediaFiles(fullNumber);
                    break;

                case SMSType.WithNotificationCallBack:
                    result = await SendWithNotificationCallBack(fullNumber);
                    break;

                case SMSType.WithOpenLinkconfirmation:
                    result = await SendWithOpenLinkconfirmation(fullNumber);
                    break;

                case SMSType.WithSendconfirmationToTwilio:
                    var resultFeedback = SendWithConfirmationToTwilio(fullNumber);
                    _logger.LogInformation(resultFeedback.MessageSid);
                    break;

                case SMSType.WithScheduled:
                    result = await SendWithScheduled(fullNumber);
                    break;
            }

            var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Log.Information("\n\t\t\t\t SMS status: Status");
            Log.Information(jsonResult);
            Log.Information("\n");

            return Ok(result);
        }

        private async Task<MessageResource> SendWithScheduled(string fullNumber)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            MessageResource messageCallBack = await MessageResource.CreateAsync(
                body: "message simple sms",
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber(fullNumber),
                scheduleType: MessageResource.ScheduleTypeEnum.Fixed,
                sendAt: new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, System.Globalization.Calendar.CurrentEra, 1, 1)
            );
            return messageCallBack;
        }

        private async Task<MessageResource> SendWithoutMediaFiles(string fullNumber)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            MessageResource messageCallBack = await MessageResource.CreateAsync(
                body: "message simple sms",
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber(fullNumber)
            );
            return messageCallBack;
        }

        private FeedbackResource SendWithConfirmationToTwilio(string messageId)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            var response = FeedbackResource.Create(messageId, outcome: FeedbackResource.OutcomeEnum.Confirmed);
            return response;
        }

        private async Task<MessageResource> SendWithNotificationCallBack(string fullNumber)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            MessageResource messageCallBack = await MessageResource.CreateAsync(
               body: "messageCallBack",
               from: new Twilio.Types.PhoneNumber("+19034033069"),
               to: new Twilio.Types.PhoneNumber(fullNumber),
               statusCallback: new Uri(WebHookHostConfig + "/N5NotificationSms/status")
           );
            return messageCallBack;
        }

        private async Task<MessageResource> SendWithOpenLinkconfirmation(string fullNumber)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            var messageWithLink = await MessageResource.CreateAsync(
                body: "Open to confirm: " + WebHookHostConfig + "/N5NotificationSms/getOpenLink/" + Guid.NewGuid().ToString(),
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber(fullNumber),
                provideFeedback: true
            );
            return messageWithLink;
        }

        private async Task<MessageResource> SendWithMediaFiles(string fullNumber)
        {
            const string resourceDemo = "https://demo.twilio.com/owl.png";
            var mediaUrl = new[] { new Uri(resourceDemo) }.ToList();

            TwilioClient.Init(AccountSid, AuthToken);
            var mediaMessage = await MessageResource.CreateAsync(
                body: "hola normal message",
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber(fullNumber),
                mediaUrl: mediaUrl);
            return mediaMessage;
        }
    }
}
