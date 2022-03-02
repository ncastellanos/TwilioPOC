using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using TwilioPOC.Model;

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
            NotificationSmsRequest result = new NotificationSmsRequest
            {
                SmsSid = Request.Form["SmsSid"][0].ToString(),
                SmsStatus = Request.Form["SmsStatus"][0].ToString(),
                MessageStatus = Request.Form["MessageStatus"][0].ToString(),
                To = Request.Form["To"][0].ToString(),
                MessageSid = Request.Form["MessageSid"][0].ToString(),
                AccountSid = Request.Form["AccountSid"][0].ToString(),
                From = Request.Form["From"][0].ToString(),
                ApiVersion = Request.Form["ApiVersion"][0].ToString()
            };

            var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Log.Information("\n\t\t\t\t NOTIFICATION SMS Status Message:  ");
            Log.Information(jsonResult);
            Log.Information("\n");

            return Ok(result);
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
