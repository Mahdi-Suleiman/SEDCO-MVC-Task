using SurveyQuestionsConfigurator.CommonHelpers;
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
        Configuration mConfigFile;
        KeyValueConfigurationCollection mSettings;
        string mDefaultCultureSectionName;

        public LanguageController()
        {
            try
            {
                mConfigFile = WebConfigurationManager.OpenWebConfiguration("~");
                mSettings = mConfigFile.AppSettings.Settings;
                mDefaultCultureSectionName = "DefaultCulture";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }
        /// <summary>
        /// GET: Language
        /// Change the langauge based on the passed "language" parameter
        /// </summary>
        public ActionResult Index(string language)
        {
            try
            {
                mSettings[mDefaultCultureSectionName].Value = language;
                mConfigFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(mConfigFile.AppSettings.SectionInformation.Name);

                //Redirect back to where the request came from
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }
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
