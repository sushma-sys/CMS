using CMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CMS.MVC.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("sushma.sanda99@gmail.com", "sanda sushma");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "tejajet2020";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                ///throw ex;
                ViewBag.Error = "Some Error";
            }
            return View();
        }

        public ActionResult SendEmailwithCase()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendEmailwithCase(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("sushma.sanda99@gmail.com", "sanda sushma");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "tejajet2020";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }

            }
            catch (Exception ex)
            {
                ///throw ex;
                ViewBag.Error = "Some Error";
            }
            return View();
        }
    }
}
