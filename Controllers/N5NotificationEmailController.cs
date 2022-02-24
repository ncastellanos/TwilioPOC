using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TwilioPOC.Model;
using TwilioPOC.Parser;

namespace TwilioPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class N5NotificationEmailController : ControllerBase
    {
        [HttpPost]
        [Route("status")]
        public async Task<IActionResult> Events()
        {
            IEnumerable<Event> events = await EventParser.ParseAsync(Request.Body);
            return Ok();
        }

        [HttpGet]
        [Route("getOpenLink/{id}")]
        public IActionResult GetOpenLink(string id)
        {
            Trace.WriteLine(id);
            return Ok();
        }

        [HttpGet]
        [Route("getUTMLink")]
        public IActionResult GetUtmLink()
        {
            string messageStatus = Guid.NewGuid().ToString();
            return Ok(messageStatus);
        }

    }
}
