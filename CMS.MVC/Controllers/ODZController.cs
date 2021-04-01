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

namespace CMS.MVC.Controllers
{
    public class ODZController : Controller
    {
        // GET: ODZ
        public ActionResult Index()
        {
            try
            {
            ODZUser oDZUser = new ODZUser();
            return View(oDZUser);
            }
            catch (CmsExceptions ex)
            {

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
                    client.BaseAddress = new Uri(apiLink);
                    var postTask = client.PostAsJsonAsync("Login", oDZUser);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(ODZHome));
                    }
                }
                return View(oDZUser);
            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }

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
                    context.Cases.AddOrUpdate(issue);
                    context.SaveChanges();
                }
                
            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }
            return RedirectToAction(nameof(ODZHome));

        }
        public ActionResult ODZHome()
        {
            using (CMSDBContext context = new CMSDBContext())
            {
               var user= context.Cases.ToList();
                return View(user);
            }
            
        }
        [HttpPost]
        public ActionResult ODZHome(string search)
        {
            try
            {
                using (CMSDBContext context = new CMSDBContext())
                {
                    var users = context.Cases.ToList();
                    if (search != null)
                    {
                        users = (List<Case>)context.Cases.Where(u => u.EmailId.Contains(search.ToLower()) || u.Location.Contains(search.ToLower())).ToList();
                        //Where(u => u.EmailId.Contains(search.ToLower()) || u.FirstName.Contains(search.ToLower()) || u.LastName.Contains(search.ToLower())).ToList();
                    }
                    return View(users);
                }

            }
            catch (CmsExceptions ex)
            {

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
                    context.Cases.AddOrUpdate(issue);
                    context.SaveChanges();
                }
                return View(issue);
            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }
        }
        [HttpGet]

        public ActionResult SendEmail(int caseId)
        {
            using (CMSDBContext context=new CMSDBContext())
            {
                ViewBag.ServiceId = new SelectList(CmsDAL.GetService(), "ServiceId", "ServiceId");
                var issue = context.Cases.Where(a => a.CaseId == caseId).FirstOrDefault();
                return View(issue);
            }
        }
        [HttpPost]
        public ActionResult SendEmail( Case issue)
        {
            //MailMessage mail = new MailMessage(ConfigurationManager.AppSettings["SMTPuser"]);
            if (ModelState.IsValid)
            {

                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress("sushma.sanda99@gmail.com"));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPuser"]);
                mail.Subject = "Case details";
                StringBuilder Body = new StringBuilder();
                Body.Append("EmailId : " + issue.EmailId);
                Body.Append("IncidentType : " + issue.IncidentType);
                Body.Append("IncidentDescription : " + issue.IncidentDescription);
                Body.Append("Location : " + issue.Location);
                Body.Append("CustomerPhone : " + issue.CustomerPhone);
                mail.Body = Body.ToString();
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                (ConfigurationManager.AppSettings["SMTPuser"], ConfigurationManager.AppSettings["SMTPpassword"]);// Enter seders User name and password
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("SendEmail",issue);

            }
            else
            {
                return View();
            }
            
        }

        public ActionResult SendToLDZCase(int CaseId, LDZCas cas,Case user)
        {
            using (CMSDBContext context = new CMSDBContext())
            {
                user = context.Cases.Where(a => a.CaseId == CaseId).FirstOrDefault();
                cas.CaseId = Convert.ToInt32(user.CaseId);
                cas.EmailId = user.EmailId;
                cas.IncidentType = user.IncidentType;
                cas.IncidentDescription = user.IncidentDescription;
                cas.ServiceId = user.ServiceId;
                cas.Location = user.Location;
                cas.CustomerPhone = user.CustomerPhone;
               // cas.ProviderName = "";
               // cas.CaseStatus = "Pending";
                context.LDZCases.Add(cas);
                context.SaveChanges();
                //return View(context.LDZCases.ToList());
                return View();
            }
        }

    }
}
