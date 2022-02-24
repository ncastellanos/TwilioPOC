using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TwilioPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class N5NotificationSmsController : ControllerBase
    {
        [HttpPost]
        [Route("status")]
        public IActionResult ActionPost()
        {
            // Log the message id and status
            var smsSid = Request.Form["MessageSid"];
            var messageStatus = Request.Form["MessageStatus"];
            var logMessage = $"\"{smsSid}\", \"{messageStatus}\"";

            Trace.WriteLine(logMessage);
            return Ok();
        }

        [HttpGet]
        [Route("getOpenLink/{id}")]
        public IActionResult Status(string id)
        {
            Trace.WriteLine(id);
            return Ok();
        }
    }
}
