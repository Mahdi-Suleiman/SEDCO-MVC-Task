using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class ConnectionController : Controller
    {
        private SqlConnectionStringBuilder mBuilder; /// passed to ConnectionSettingsManager (Busniess Logic Layer)
        private readonly ConnectionSettingsManager mConnectionSettingsManager;

        public ConnectionController()
        {
            mBuilder = new SqlConnectionStringBuilder();
            mConnectionSettingsManager = new ConnectionSettingsManager();
        }

        // GET: Connection
        public ActionResult Index(string pMessage = null)
        {
            mBuilder = mConnectionSettingsManager.GetConnectionString();
            ConnectionSetting tConnectionSettings = new ConnectionSetting(mBuilder);
            return View(tConnectionSettings);
        }

        // GET: Connection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Connection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Connection/Create
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

        // GET: Connection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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

                if (collection["Checkconnectivity"] != null)
                {
                    return CheckConnectivity(tBuilder);
                    //return HttpNotFound();
                }


                ErrorCode tResult = mConnectionSettingsManager.SaveConnectionString(tBuilder);

                if (tResult == ErrorCode.SUCCESS)
                {
                    TempData["Message"] = "Connection Settings Saved Successfully";
                }
                else
                {
                    TempData["Error"] = "Saving failed, please try again";
                }

                return RedirectToAction("Index");

            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        private ActionResult CheckConnectivity(SqlConnectionStringBuilder pBuilder)
        {
            try
            {
                ErrorCode tResult = mConnectionSettingsManager.CheckConnectivity(pBuilder);
                if (tResult == ErrorCode.SUCCESS)
                {
                    TempData["Message"] = "Connection was successful!";
                }
                else
                {
                    TempData["Error"] = "Connection Failed!";

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return RedirectToAction("Index");
            }
        }

        // GET: Connection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Connection/Delete/5
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
