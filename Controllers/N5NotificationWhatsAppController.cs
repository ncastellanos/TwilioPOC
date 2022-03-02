using Microsoft.AspNetCore.Mvc;
using Serilog;
using TwilioPOC.Model;

namespace TwilioPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class N5NotificationWhatsAppController : ControllerBase
    {
        [HttpPost]
        [Route("status")]
        public IActionResult ActionPost()
        {
            NotificationWhatsAppRequest result = new NotificationWhatsAppRequest
            {
                SmsSid = Request.Form["SmsSid"][0].ToString(),
                SmsStatus = Request.Form["SmsStatus"][0].ToString(),
                MessageStatus = Request.Form["MessageStatus"][0].ToString(),
                ChannelToAddress = Request.Form["ChannelToAddress"][0].ToString(),
                To = Request.Form["To"][0].ToString(),
                ChannelPrefix = Request.Form["ChannelPrefix"][0].ToString(),
                MessageSid = Request.Form["MessageSid"][0].ToString(),
                AccountSid = Request.Form["AccountSid"][0].ToString(),
                From = Request.Form["From"][0].ToString(),
                ApiVersion = Request.Form["ApiVersion"][0].ToString(),
                ChannelInstallSid = Request.Form["ChannelInstallSid"][0].ToString()
            };
            var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Log.Information("\n\t\t\t\t NOTIFICATION WhatsApp Status Message:  ");
            Log.Information(jsonResult);
            Log.Information("\n");

            return Ok(result);
        }

        [HttpPost]
        [Route("getResponseMessage")]
        public IActionResult GetResponseMessages()
        {
            WhatsAppUserMessage result = new WhatsAppUserMessage
            {
                SmsMessageSid = Request.Form["SmsMessageSid"][0].ToString(),
                NumMedia = Request.Form["NumMedia"][0].ToString(),
                ProfileName = Request.Form["ProfileName"][0].ToString(),
                SmsSid = Request.Form["SmsSid"][0].ToString(),
                WaId = Request.Form["WaId"][0].ToString(),
                SmsStatus = Request.Form["SmsStatus"][0].ToString(),
                Body = Request.Form["Body"][0].ToString(),
                To = Request.Form["To"][0].ToString(),
                NumSegments = Request.Form["NumSegments"][0].ToString(),
                MessageSid = Request.Form["MessageSid"][0].ToString(),
                AccountSid = Request.Form["AccountSid"][0].ToString(),
                From = Request.Form["From"][0].ToString(),
                ApiVersion = Request.Form["ApiVersion"][0].ToString()
            };

            var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Log.Information("\n\t\t\t\t MESSAGE SENDED BY USER ON WHATSAPP");
            Log.Information(jsonResult);

            return Ok(result);
        }
    }
}