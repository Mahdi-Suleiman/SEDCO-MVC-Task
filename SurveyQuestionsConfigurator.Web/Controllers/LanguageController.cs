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
        #region Properties & Attributes
        Configuration mConfigFile;
        KeyValueConfigurationCollection mSettings;
        string mDefaultCultureSectionName;

        #endregion

        #region Constructor
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
            }
        }

        #endregion

        #region Actions
        /// <summary>
        /// GET: Language
        /// Change the langauge based on the passed "language" parameter
        /// </summary>
        /// <returns>
        /// ActionResult
        /// </returns>
        public ActionResult Index(string language)
        {
            try
            {
                mSettings[mDefaultCultureSectionName].Value = language;
                mConfigFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(mConfigFile.AppSettings.SectionInformation.Name);

                //Redirect back to where the request came from
                //better UX
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }
        }

        #endregion
    }
}
