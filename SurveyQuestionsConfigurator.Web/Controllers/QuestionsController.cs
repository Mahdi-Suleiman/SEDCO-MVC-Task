using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.QuestionLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly QuestionManager mQuestionManager;
        public QuestionsController()
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

        // GET: Questions
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

        // GET: Questions/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
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

        // GET: Questions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Questions/Edit/5
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

        // GET: Questions/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Questions/Delete/5
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

        ///////// actions that returns their relative partial views
        public ActionResult PartialSmiley()
        {
            return PartialView("_CreateSmileyQuestion");
        }
        public ActionResult PartialSlider()
        {
            return PartialView("_CreateSliderQuestion");
        }
        public ActionResult PartialStar()
        {
            return PartialView("_CreateStarQuestion");
        }
    }
}
