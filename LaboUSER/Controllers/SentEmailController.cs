using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace LaboUSER.Controllers
{
    public class SentEmailController : Controller
    {
        //
        // GET: /SentEmail/

        public ActionResult Index()
        {
            return View();
        }
        public void SendEmail(Object mailMsg)
        {
            MailMessage mailMessage = (MailMessage)mailMsg;
            try
            {
                /* Setting should be kept somewhere so no need to 
                   pass as a parameter (might be in web.config)       */
                using (SmtpClient client = new SmtpClient("smtpout.secureserver.net", 80))
                {
                    // Configure the client
                  
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("info@hardyhat.com", "kunal@123");

                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailMessage.IsBodyHtml = true;
                    client.Send(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                // Code to Log error
            }
        }
       
    }
}
