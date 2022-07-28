using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Entities.Resources;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using static SurveyQuestionsConfigurator.Entities.Generic;
using static SurveyQuestionsConfigurator.Entities.Resources.EnumResourceStrings;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class ConnectionController : Controller
    {
        #region Properties & Attributes

        private SqlConnectionStringBuilder mBuilder; /// passed to ConnectionSettingsManager (Busniess Logic Layer)
        private readonly ConnectionSettingsManager mConnectionSettingsManager;
        private readonly ResourceManager mLocalResourceManager;

        #endregion

        #region Constructor
        public ConnectionController()
        {
            try
            {
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.Entities.Resources.LanguageStrings", typeof(LanguageStrings).Assembly);
                //mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.Entities.Resources.LanguageStrings", typeof(Controller).Assembly); //intintially cause exception
                mBuilder = new SqlConnectionStringBuilder();
                mConnectionSettingsManager = new ConnectionSettingsManager();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #endregion

        #region Actions
        /// <summary>
        /// GET: Connection
        /// Get the connectino settings from .config file, fill the ConnectionSetting model and pass it to the view
        /// </summary>
        /// <returns>
        /// ActionResult
        /// </returns>
        public ActionResult Index(string message = null, string error = null)
        {
            try
            {
                ///show any message or error passed to the action
                ///TempData us used in "_Layout" view above @RenderBody() 
                if (!string.IsNullOrEmpty(message))
                {
                    TempData[$"{ResourceStrings.Message}"] = message;
                }

                if (!string.IsNullOrEmpty(error))
                {
                    TempData[$"{ResourceStrings.Error}"] = error;
                }

                mBuilder = mConnectionSettingsManager.GetConnectionString();
                ConnectionSetting tConnectionSetting = new ConnectionSetting(mBuilder);
                return View(tConnectionSetting);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View();
            }
        }

        /// <summary>
        /// POST: Connection/Edit/5
        /// This action either check for connectivity and call the corresponding methods for it
        /// or save the new connection settings in the config file
        /// </summary>
        /// <returns>
        /// ActionResult
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                SqlConnectionStringBuilder tBuilder = new SqlConnectionStringBuilder();
                tBuilder.DataSource = collection["ServerName"];
                tBuilder.InitialCatalog = collection["DatabaseName"];
                tBuilder.UserID = collection["UserId"];
                tBuilder.Password = collection["Password"];

                /// If Check Connectivity button is pressed
                if (collection["Checkconnectivity"] != null)
                {
                    return CheckConnectivity(tBuilder);
                }

                /// If Save button is pressed
                ErrorCode tResult = mConnectionSettingsManager.SaveConnectionString(tBuilder);

                if (tResult == ErrorCode.SUCCESS)
                {
                    return RedirectToAction("Index", new { message = $"{ResourceStrings.SettingsSavedMessage}" });
                }
                else
                {
                    return RedirectToAction("Index", new { error = $"{ResourceStrings.ConnectionFailedError}" });
                }
            }
            catch
            {
                return RedirectToAction("Index", new { error = $"{ResourceStrings.ConnectionFailedError}" });
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Checks if the entered connection settings are valid
        /// </summary>
        /// <returns>
        /// ActionResult
        /// </returns>
        private ActionResult CheckConnectivity(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                ErrorCode tResult = mConnectionSettingsManager.CheckConnectivity(pBuilder);
                if (tResult == ErrorCode.SUCCESS)
                {
                    TempData[$"{ResourceStrings.Message}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.SuccessfulConnectionMessage}")}";

                }
                else
                {
                    TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.ConnectionFailedError}")}";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return RedirectToAction("Index");
            }
        }

        #endregion

    }
}
