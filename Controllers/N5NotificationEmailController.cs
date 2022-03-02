using Microsoft.AspNetCore.Mvc;
using Serilog;
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
            var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(events);
            Log.Information("\n\t\t\t\t NOTIFICATION EMAIL Status");
            Log.Information(jsonResult);
            Log.Information("\n");

            return Ok(jsonResult);
        }

        [HttpGet]
        [Route("getOpenLink/{id}")]
        public IActionResult GetOpenLink(string id)
        {
            var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(id);
            Log.Information(jsonResult);
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
