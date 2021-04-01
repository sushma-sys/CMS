using CMS.Entities;
using CMS.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CMS.MVC.Controllers
{
    public class LDZController : Controller
    {
        // GET: LDZ
        public ActionResult LDZLogin()
        {
            try
            {
                LDZUser lDZUser = new LDZUser();
                return View(lDZUser);
            }
            catch (CmsExceptions ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public ActionResult LDZLogin(LDZUser lDZUser)
        {
            string apiLink = ConfigurationManager.AppSettings["ApiUrl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiLink);
                var postTask = client.PostAsJsonAsync("LDZLogin", lDZUser);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(LDZHome));
                }
            }
            return View(lDZUser);

        }
        public ActionResult LDZHome()
        {
            return View();
        }
        public ActionResult LDZCases()
        {
            using (CMSDBContext context=new CMSDBContext())
            {
                LDZCas lDZCas = new LDZCas();
                return View(context.LDZCases.ToList());
            }
            
        }
        public ActionResult Edit(int lDZCaseId)
        {
            using (CMSDBContext context=new CMSDBContext())
            {
                ViewBag.ProviderName = new SelectList(context.Providers.ToList(), "ProviderName", "ProviderName");
                //ViewBag.CaseStatus = new SelectList(context.Providers.ToList(), "CaseStatus", "CaseStatus");
                var user = context.LDZCases.Where(a => a.LDZCaseId == lDZCaseId).FirstOrDefault();
                return View(user);
            }
           // return View(user);
        }
        [HttpPost]
        public ActionResult Edit(LDZCas lDZCas)
        {
            using (CMSDBContext context = new CMSDBContext())
            {
                ViewBag.ProviderName = new SelectList(context.Providers.ToList(), "ProviderName", "ProviderName");
                //ViewBag.CaseStatus = new SelectList(context.Providers.ToList(), "CaseStatus", "CaseStatus");
                context.LDZCases.AddOrUpdate(lDZCas);
                context.SaveChanges();
            }
            return View(lDZCas);
        }

    }
}