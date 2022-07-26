using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult Arabic(string language)
        {
            CultureInfo mDefaultCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
            Configuration tConfigFile = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationCollection tSettings = tConfigFile.AppSettings.Settings;


            tSettings = tConfigFile.AppSettings.Settings;
            string mDefaultCultureString = "DefaultCulture";

            tSettings[mDefaultCultureString].Value = language;
            tConfigFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(tConfigFile.AppSettings.SectionInformation.Name);

            return Redirect(HttpContext.Request.Headers["Referer"]);
        }
        public ActionResult English(string language)
        {
            CultureInfo mDefaultCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
            Configuration tConfigFile = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationCollection tSettings = tConfigFile.AppSettings.Settings;


            tSettings = tConfigFile.AppSettings.Settings;
            string mDefaultCultureString = "DefaultCulture";

            tSettings[mDefaultCultureString].Value = language;
            tConfigFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(tConfigFile.AppSettings.SectionInformation.Name);

            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }

        // GET: Language/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Language/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Language/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Language/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Language/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Language/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Language/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
