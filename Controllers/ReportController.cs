using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<TiwlioMessagesController> _logger;

        public ReportController(ILogger<TiwlioMessagesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("getAll")]
        public List<MessageResource> Get()
        {
            string accountSid = "ACfd1997723266ad7462b086211656bc75";
            string authToken = "7161b0dba4ad58cf52ebec5b208ca41b";

            TwilioClient.Init(accountSid, authToken);
            var messages = MessageResource.Read(
                dateSent: new DateTime(2022, 2, 22),
                from: new Twilio.Types.PhoneNumber("+19034033069"),
                to: new Twilio.Types.PhoneNumber("+573186496074"),
                limit: 20
            );

            return messages.ToList();
        }

    }
}
