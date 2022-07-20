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
        public ActionResult Details(int id, QuestionType type)
        {
            //Console.WriteLine(type);

            switch (type)
            {
                case QuestionType.SMILEY:
                    {
                        SmileyQuestion tSmileyQuestion = new SmileyQuestion(id);
                        mQuestionManager.GetSmileyQuestionByID(ref tSmileyQuestion);
                        return View("~/Views/SmileyQuestion/Details.cshtml", tSmileyQuestion);
                    }
                case QuestionType.SLIDER:
                    {
                        SliderQuestion tSliderQuestion = new SliderQuestion(id);
                        mQuestionManager.GetSliderQuestionByID(ref tSliderQuestion);
                        return View("~/Views/SliderQuestion/Details.cshtml", tSliderQuestion);
                    }
                case QuestionType.STAR:
                    {
                        StarQuestion tStarQuestion = new StarQuestion(id);
                        mQuestionManager.GetStarQuestionByID(ref tStarQuestion);
                        return View("~/Views/StarQuestion/Details.cshtml", tStarQuestion);
                    }
                default:
                    break;
            }

            if (type == QuestionType.SMILEY)
            {

            }
            return View("~/Views/Home/About.cshtml");
            //return new EmptyResult();
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                int type = Convert.ToInt32(collection["Type"]);
                if ((QuestionType)type == QuestionType.SMILEY)
                {
                    return View("~/Views/Home/About.cshtml");
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(SliderQuestion collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(StarQuestion collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


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
        public ActionResult Delete(int id, QuestionType type)
        {
            switch (type)
            {
                case QuestionType.SMILEY:
                    {
                        SmileyQuestion tSmileyQuestion = new SmileyQuestion(id);
                        mQuestionManager.GetSmileyQuestionByID(ref tSmileyQuestion);
                        return View(tSmileyQuestion);

                    }
                case QuestionType.SLIDER:
                    {
                        SliderQuestion tSliderQuestion = new SliderQuestion(id);
                        mQuestionManager.GetSliderQuestionByID(ref tSliderQuestion);
                        return View(tSliderQuestion);
                    }
                case QuestionType.STAR:
                    {
                        StarQuestion tStarQuestion = new StarQuestion(id);
                        mQuestionManager.GetStarQuestionByID(ref tStarQuestion);
                        return View(tStarQuestion);
                    }
                default:
                    break;
            }
            return View();
        }

        // POST: Questions/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                mQuestionManager.DeleteQuestionByID(id);
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
