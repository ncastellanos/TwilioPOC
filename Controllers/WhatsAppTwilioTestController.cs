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
        public IActionResult SendTest()
        {
            TwilioClient.Init(AccountSid, AuthToken);
            var messageOptions = new CreateMessageOptions(new PhoneNumber("whatsapp:+573186496074"))
            {
                From = new PhoneNumber("whatsapp:+14155238886"),
                Body = "Tes de mensaje para campañas http://www.yummycupcakes.com/"
            };

            var message = MessageResource.Create(messageOptions);
            return Ok(message);
        }
    }
}
