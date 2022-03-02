using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TwilioPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<TiwlioSMSTestMessagesController> _logger;
        private readonly IConfiguration _configuration;

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

        public ReportController(ILogger<TiwlioSMSTestMessagesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getAll")]
        public List<MessageResource> Get()
        {
            TwilioClient.Init(AccountSid, AuthToken);
            var messages = MessageResource.Read(
                dateSentBefore: new DateTime(2022, 11, 22),
                dateSentAfter: new DateTime(2022, 3, 2),
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber("+573186496074"),
                limit: 100
            );

            return messages.ToList();
        }

        [HttpGet]
        [Route("getByMessageSid")]
        public MessageResource GetByMessageId(string messageSID)
        {
            //"SM26b1153c791146cb833b9c0f847f9989"
            TwilioClient.Init(AccountSid, AuthToken);
            var message = MessageResource.Fetch(pathSid: messageSID);
            return message;
        }

    }
}
