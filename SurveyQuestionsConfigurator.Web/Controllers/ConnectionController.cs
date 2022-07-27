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
        private SqlConnectionStringBuilder mBuilder; /// passed to ConnectionSettingsManager (Busniess Logic Layer)
        private readonly ConnectionSettingsManager mConnectionSettingsManager;
        private readonly ResourceManager mLocalResourceManager;

        public ConnectionController()
        {
            mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.Entities.Resources.LanguageStrings", typeof(LanguageStrings).Assembly);
            //mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.Entities.Resources.LanguageStrings", typeof(Controller).Assembly); //intintially cause exception
            mBuilder = new SqlConnectionStringBuilder();
            mConnectionSettingsManager = new ConnectionSettingsManager();
        }

        // GET: Connection
        public ActionResult Index(string message = null, string error = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                TempData[nameof(message)] = message;
            }

            if (!string.IsNullOrEmpty(error))
            {
                TempData[nameof(error)] = error;
            }

            mBuilder = mConnectionSettingsManager.GetConnectionString();
            ConnectionSetting tConnectionSettings = new ConnectionSetting(mBuilder);
            return View(tConnectionSettings);
        }

        // POST: Connection/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                SqlConnectionStringBuilder tBuilder = new SqlConnectionStringBuilder();
                tBuilder.DataSource = collection["ServerName"];
                tBuilder.InitialCatalog = collection["DatabaseName"];
                tBuilder.UserID = collection["UserId"];
                tBuilder.Password = collection["Password"];

                /// If Check Connectivity button is pressed
                if (collection["Checkconnectivity"] != null)
                {
                    return CheckConnectivity(tBuilder);
                    //return HttpNotFound();
                }

                /// If Save button is pressed
                ErrorCode tResult = mConnectionSettingsManager.SaveConnectionString(tBuilder);

                if (tResult == ErrorCode.SUCCESS)
                {
                    //TempData["Message"] = "Connection Settings Saved Successfully";
                    return RedirectToAction("Index", new { message = $"{ResourceStrings.SettingsSavedMessage}" });
                    //return RedirectToAction("Index", new { message = "Connection Settings Saved Successfully." });
                }
                else
                {
                    //TempData["Error"] = "Saving failed, please try again";
                    return RedirectToAction("Index", new { error = $"{ResourceStrings.ConnectionFailedError}" });
                    //return RedirectToAction("Index", new { error = "Saving failed, please try again" });
                }

                //return RedirectToAction("Index", new { message = "Connection Settings Saved Successfully" });
            }
            catch
            {
                return RedirectToAction("Index", new { error = $"{ResourceStrings.ConnectionFailedError}" });
                //return RedirectToAction("Index", new { error = "Saving failed, please try again" });
            }
        }

        private ActionResult CheckConnectivity(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                ErrorCode tResult = mConnectionSettingsManager.CheckConnectivity(pBuilder);
                if (tResult == ErrorCode.SUCCESS)
                {
                    //TempData["Message"] = "Connection was successful!";
                    TempData[$"{ResourceStrings.Message}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.SuccessfulConnectionMessage}")}";

                }
                else
                {
                    //TempData["Error"] = "Connection Failed!";
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
    }
}
