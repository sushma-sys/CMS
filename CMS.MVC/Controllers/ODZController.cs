using CMS.DAL;
using CMS.Entities;
using CMS.Exceptions;
using CMS.MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mail;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using MailMessage = System.Net.Mail.MailMessage;
using System.Text;
using System.Web.Security;

namespace CMS.MVC.Controllers
{
    public class ODZController : Controller
    {
        // GET: ODZ
       // private static log4net.ILog Log { get; set; }
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ODZController));
        CMSDBContext context = new CMSDBContext();
        public ActionResult Index()
        {
            try
            {
                logger.Info("User Logged");
                ODZUser oDZUser = new ODZUser();
                return View(oDZUser);
            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Index(ODZUser oDZUser)
        {
            try
            {
                string apiLink = ConfigurationManager.AppSettings["ApiUrl"];
                using (var client = new HttpClient())
                {
                    FormsAuthentication.SetAuthCookie(oDZUser.EmailId, false);
                    client.BaseAddress = new Uri(apiLink);
                    var postTask = client.PostAsJsonAsync("Login", oDZUser);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        DateTime updatedDate = (DateTime)context.ODZUsers.Where(n=>n.EmailId==oDZUser.EmailId).Select(u => u.PasswordUpdatedOn).FirstOrDefault();
                        TimeSpan cheakDate = DateTime.Now - updatedDate;
                        if (cheakDate.Days >= Convert.ToInt32(ConfigurationManager.AppSettings["Expiry"]))
                        {
                            return RedirectToAction(nameof(ResetPassword));
                        }
                        else
                        {
                            return RedirectToAction(nameof(ODZHome));
                        }
                    }
                    else
                    {
                        ViewBag.Error = "UserName/Password incorrect";
                    }
                }
                return View(oDZUser);

            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ODZCaseCreat()
        {
            try
            {
                ViewBag.ServiceId = new SelectList(CmsDAL.GetService(), "ServiceId", "ServiceId");
                Case issue = new Case();
                return View(issue);
            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ODZCaseCreat(Case issue)
        {
            try
            {
                ViewBag.ServiceId = new SelectList(CmsDAL.GetService(), "ServiceId", "ServiceId");
                using (CMSDBContext context = new CMSDBContext())
                {
                    SendEmail(issue);
                    InsertToLDZCase(issue);
                    context.Cases.AddOrUpdate(issue);
                    context.SaveChanges();
                    
                }

            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
            return RedirectToAction(nameof(ODZHome));

        }
        public ActionResult ODZHome()
        {
            try
            {
                using (CMSDBContext context = new CMSDBContext())
                {
                    ViewData["EmailId"] = User.Identity.Name;
                    //  ViewBag.Message = "Sent Successed!!";
                    var user = context.Cases.ToList();
                    return View(user);
                }
            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }

        }
        //[HttpPost]
        //public ActionResult ODZHome(string search)
        //{
        //    try
        //    {
        //        using (CMSDBContext context = new CMSDBContext())
        //        {
        //            var users = context.Cases.ToList();
        //            if (search != null)
        //            {
        //                users = (List<Case>)context.Cases.Where(u => u.EmailId.Contains(search.ToLower()) || u.Location.Contains(search.ToLower())).ToList();
        //                //Where(u => u.EmailId.Contains(search.ToLower()) || u.FirstName.Contains(search.ToLower()) || u.LastName.Contains(search.ToLower())).ToList();
        //            }
        //            return View(users);
        //        }

        //    }
        //    catch (CmsExceptions ex)
        //    {
        //        logger.Error(ex.ToString());
        //        throw ex;
        //    }
        //}
        public ActionResult ResetPassword()
        {
            try
            {
                ResetPasswordModel reset = new ResetPasswordModel();
                ViewData["EmailId"] = User.Identity.Name;
                return View(reset);
            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }


        }
        [HttpPost]
        public ActionResult ResetPassword(string EmailId, ResetPasswordModel reset)
        {
            try
            {
                using (CMSDBContext context = new CMSDBContext())
                {
                    ODZUser user = new ODZUser();
                    user.EmailId = EmailId;
                    var exists = context.ODZUsers.Where(a => a.EmailId == user.EmailId).FirstOrDefault();
                    if (exists != null)
                    {
                        //user.EmailId = exists.EmailId;
                        user.FirstName = exists.FirstName;
                        user.LastName = exists.LastName;
                        user.PasswordUpdatedOn = DateTime.Now;
                        user.Password = reset.NewPassword;
                        context.ODZUsers.AddOrUpdate(user);
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }


        }
        public PartialViewResult Edit(int caseId)
        {
            try
            {
                using (CMSDBContext context = new CMSDBContext())
                {
                    ViewBag.ServiceId = new SelectList(CmsDAL.GetService(), "ServiceId", "ServiceId");
                    var user = context.Cases.Where(a => a.CaseId == caseId).FirstOrDefault();
                    return PartialView(user);
                }
            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
        }
        //[HttpPost]
        //public JsonResult Edit(Case issue)
        //{
        //    ViewBag.ServiceId = new SelectList(CmsDAL.GetService(), "ServiceId", "ServiceId");
        //    using (CMSDBContext context = new CMSDBContext())
        //    {
        //        context.Cases.AddOrUpdate(issue);
        //        context.SaveChanges();
        //    }
        //    return Json(issue,JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult Edit(Case issue)
        {
            try
            {
                ViewBag.ServiceId = new SelectList(CmsDAL.GetService(), "ServiceId", "ServiceId");
                using (CMSDBContext context = new CMSDBContext())
                {
                    SendEmail(issue);
                    context.Cases.AddOrUpdate(issue);
                    context.SaveChanges();
                }
                return View(issue);
            }
            catch (CmsExceptions ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
        }
        //[HttpGet]

        //public ActionResult SendEmail(int caseId)
        //{
        //    using (CMSDBContext context=new CMSDBContext())
        //    {
        //        ViewBag.ServiceId = new SelectList(CmsDAL.GetService(), "ServiceId", "ServiceId");
        //        var issue = context.Cases.Where(a => a.CaseId == caseId).FirstOrDefault();
        //        return View(issue);
        //    }
        //}
        //[HttpPost]
        //public ActionResult SendEmail( Case issue)
        //{
        //    //MailMessage mail = new MailMessage(ConfigurationManager.AppSettings["SMTPuser"]);
        //    if (ModelState.IsValid)
        //    {

        //        MailMessage mail = new MailMessage();
        //        mail.To.Add(new MailAddress("sushma.sanda99@gmail.com"));
        //        mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPuser"]);
        //        mail.Subject = "Case details";
        //        StringBuilder Body = new StringBuilder();
        //        Body.Append("EmailId : " + issue.EmailId);
        //        Body.Append("IncidentType : " + issue.IncidentType);
        //        Body.Append("IncidentDescription : " + issue.IncidentDescription);
        //        Body.Append("Location : " + issue.Location);
        //        Body.Append("CustomerPhone : " + issue.CustomerPhone);
        //        mail.Body = Body.ToString();
        //       // mail.Body = Model.Case;
        //        mail.IsBodyHtml = true;
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.Port = 587;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new System.Net.NetworkCredential
        //        (ConfigurationManager.AppSettings["SMTPuser"], ConfigurationManager.AppSettings["SMTPpassword"]);// Enter seders User name and password
        //        smtp.EnableSsl = true;
        //        smtp.Send(mail);
        //        return View("SendEmail",issue);

        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}
        private static void SendEmail(Case issue)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress("sushma.sanda99@gmail.com"));
            mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPuser"]);
            mail.Subject = "Case details";
            // message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            // mail.Body = string.Format(body, issue.EmailId, issue.IncidentType, issue.IncidentDescription, issue.ServiceId, issue.Location, issue.CustomerPhone);
            StringBuilder Body = new StringBuilder();
            Body.Append("EmailId : " + issue.EmailId);
            Body.Append("IncidentType : " + issue.IncidentType);
            Body.Append("IncidentDescription : " + issue.IncidentDescription);
            Body.Append("Location : " + issue.Location);
            Body.Append("CustomerPhone : " + issue.CustomerPhone);
            mail.Body = Body.ToString();
            // mail.Body = Model.Case;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (ConfigurationManager.AppSettings["SMTPuser"], ConfigurationManager.AppSettings["SMTPpassword"]);// Enter seders User name and password
            smtp.EnableSsl = true;
            smtp.Send(mail);
            // return true;
        }
        private static void InsertToLDZCase(Case cas)
        {
            using (CMSDBContext context = new CMSDBContext())
            {
                if (cas != null)
                {
                    LDZCas cc = new LDZCas();
                    cc.EmailId = cas.EmailId;
                    cc.IncidentType = cas.IncidentType;
                    cc.IncidentDescription = cas.IncidentDescription;
                    cc.ServiceId = cas.ServiceId;
                    cc.Location = cas.Location;
                    cc.CustomerPhone = cas.CustomerPhone;
                    cc.ProviderName = "";
                    cc.CaseStatus = "Pending";
                    context.LDZCases.Add(cc);
                    context.SaveChanges();
                }
            }

        }
    }
}
