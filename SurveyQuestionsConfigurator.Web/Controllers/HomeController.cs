using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuestionManager mQuestionManager;

        public HomeController()
        {
            try
            {
                mQuestionManager = new QuestionManager();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                mQuestionManager.DeleteQuestionByID(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return new EmptyResult();
            }
        }

        public ActionResult Index()
        {
            try
            {
                List<Question> testList = new List<Question>();
                mQuestionManager.GetAllQuestions(ref testList);
                return View(testList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return new EmptyResult();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}