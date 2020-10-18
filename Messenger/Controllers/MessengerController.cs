using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Collections.Specialized;


using JusiBase;

namespace Messenger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessengerController : ControllerBase
    {
       private readonly ILogger<MessengerController> _logger;

        public MessengerController(ILogger<MessengerController> logger)
        {
            _logger = logger;
        }

        //http://localhost:32773/api/messenger/push?subject=hallo&text=testmessage
        //http://messenger.prod.j1/api/messenger/push?subject=hallo&text=testmessage

        //encoding via Aufruf JusiBase System.Web.HttpUtility.UrlEncode(message);
        [HttpGet("{messageType}", Name = "Get")]
        public ResponseTrigger Get(string messageType, string subject, string text)
        {

            try
            {
                if (messageType == "push")
                { 
                Console.WriteLine("Push mit Betreff '{0}' soll gesendet werden.", subject);

                var parameters = new NameValueCollection {
    { "token", "aop9xnr3udsnk9j1ah797wzrictmcc" },
    { "user", "uc2bjy2kt32htszmo6ess5emdxrk82" },
    { "message", text },
    { "title", subject }
};

                using (var client = new WebClient())
                {
                    client.UploadValues("https://api.pushover.net/1/messages.json", parameters);
                }

                Console.WriteLine("Push mit Betreff '{0}' gesendet", subject);
                return new ResponseTrigger
                {
                    ReturnCode = 1,
                    ReturnState = "gesendet"
                };
                }
                else
                { 
                return new ResponseTrigger
                {
                    ReturnCode = 0,
                    ReturnState = "unbekannt"
                };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler bei pushover Versand: " + ex);

                return new ResponseTrigger
                {
                    ReturnCode = 0,
                    ReturnState = "Fehler"
                };

            }

            
        }
    }
}
