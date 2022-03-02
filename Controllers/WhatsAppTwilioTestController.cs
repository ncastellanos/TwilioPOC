using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TwilioPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WhatsAppTwilioTestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private string AccountSid
        {
            get
            {
                return _configuration.GetValue<string>("Twilio:accountSid");
            }
        }

        private string AuthToken
        {
            get
            {
                return _configuration.GetSection("Twilio").GetSection("authToken").Value;
            }
        }

        public WhatsAppTwilioTestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("simpleWhatsAppMessage")]
        public IActionResult SendTest(string fullNumber)
        {
            string numberTo = string.Concat("whatsapp:", fullNumber);
            TwilioClient.Init(AccountSid, AuthToken);
            var message = MessageResource.Create(
                from: new PhoneNumber("whatsapp:+14155238886"),
                body: "Hello, there!",
                to: new PhoneNumber(numberTo)
                );

            return Ok(message);
        }

        [HttpGet]
        [Route("scheduled")]
        public IActionResult SendTestScheduled(string fullNumber)
        {
            string numberTo = string.Concat("whatsapp:", fullNumber);
            TwilioClient.Init(AccountSid, AuthToken);
            var message = MessageResource.Create(
                from: new PhoneNumber("whatsapp:+14155238886"),
                body: "Hello, there!",
                to: new PhoneNumber(numberTo),
                scheduleType: MessageResource.ScheduleTypeEnum.Fixed,
                sendAt: new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 2, System.Globalization.Calendar.CurrentEra, 1, 1)
                );
            return Ok(message);
        }
    }
}
