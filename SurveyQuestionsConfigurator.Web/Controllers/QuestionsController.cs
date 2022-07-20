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

            return View();
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
        public ActionResult Create(FormCollection pCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int type = Convert.ToInt32(pCollection["Type"]);
                    ErrorCode tResult = ErrorCode.ERROR;
                    switch ((QuestionType)type)
                    {
                        case QuestionType.SMILEY:
                            {
                                tResult = AddSmileyQuestion(pCollection);
                            }
                            break;
                        case QuestionType.SLIDER:
                            {
                                tResult = AddSliderQuestion(pCollection);
                            }
                            break;
                        case QuestionType.STAR:
                            {
                                tResult = AddStarQuestion(pCollection);
                            }
                            break;
                    }

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            return RedirectToAction("Index");
                            break;
                        case ErrorCode.VALIDATION:
                            ModelState.AddModelError("Order", "Question order already in use, Try using another one.");
                            break;
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        private ErrorCode AddSmileyQuestion(FormCollection pCollection)
        {
            try
            {
                int tId = -1;
                int tOrder = Convert.ToInt32(pCollection["Order"]);
                string tText = pCollection["Text"];
                QuestionType tType = QuestionType.SMILEY;
                int tNumberOfSmileyFaces = Convert.ToInt32(pCollection["NumberOfSmileyFaces"]);

                SmileyQuestion tSmileyQuestion = new SmileyQuestion(tId, tOrder, tText, tType, tNumberOfSmileyFaces);
                ErrorCode tResult = mQuestionManager.InsertSmileyQuestion(tSmileyQuestion);
                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        private ErrorCode AddSliderQuestion(FormCollection pCollection)
        {
            try
            {
                int tId = -1;
                int tOrder = Convert.ToInt32(pCollection["Order"]);
                string tText = pCollection["Text"];
                QuestionType tType = QuestionType.SLIDER;
                int tStartValue = Convert.ToInt32(pCollection["StartValue"]);
                int tEndValue = Convert.ToInt32(pCollection["EndValue"]);
                string tStartValueCaption = pCollection["StartValueCaption"];
                string tEndValueCaption = pCollection["EndValueCaption"];


                SliderQuestion tSliderQuestion = new SliderQuestion(tId, tOrder, tText, tType, tStartValue, tEndValue, tStartValueCaption, tEndValueCaption);
                ErrorCode tResult = mQuestionManager.InsertSliderQuestion(tSliderQuestion);

                if (tEndValue < tStartValue)
                {
                    //ModelState.AddModelError("EndValue", "End value must be larger than start value");
                    TempData["Error"] = "End value must be larger than start value";
                }

                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        private ErrorCode AddStarQuestion(FormCollection pCollection)
        {
            try
            {
                int tId = -1;
                int tOrder = Convert.ToInt32(pCollection["Order"]);
                string tText = pCollection["Text"];
                QuestionType tType = QuestionType.STAR;
                int tNumberOfStars = Convert.ToInt32(pCollection["NumberOfStars"]);

                StarQuestion tStarQuestion = new StarQuestion(tId, tOrder, tText, tType, tNumberOfStars);
                ErrorCode tResult = mQuestionManager.InsertStarQuestion(tStarQuestion);
                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int id, QuestionType type)
        {
            switch (type)
            {
                case QuestionType.SMILEY:
                    {
                        SmileyQuestion tSmileyQuestion = new SmileyQuestion(id);
                        mQuestionManager.GetSmileyQuestionByID(ref tSmileyQuestion);
                        return View("~/Views/SmileyQuestion/Edit.cshtml", tSmileyQuestion);
                    }
                    break;
                case QuestionType.SLIDER:
                    {
                        SliderQuestion tSliderQuestion = new SliderQuestion(id);
                        mQuestionManager.GetSliderQuestionByID(ref tSliderQuestion);
                        return View("~/Views/SliderQuestion/Edit.cshtml", tSliderQuestion);
                    }
                    break;
                case QuestionType.STAR:
                    {
                        StarQuestion tStarQuestion = new StarQuestion(id);
                        mQuestionManager.GetStarQuestionByID(ref tStarQuestion);
                        return View("~/Views/StarQuestion/Edit.cshtml", tStarQuestion);
                    }
                    break;
            }
            return View();
        }

        // POST: Questions/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection pCollection)
        {
            try
            {
                string strtype = pCollection["Type"];
                int type = Convert.ToInt32(pCollection["Type"]);

                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    ErrorCode tResult = ErrorCode.ERROR;
                    switch ((QuestionType)type)
                    {
                        case QuestionType.SMILEY:
                            {
                                tResult = EditSmileyQuestion(id, pCollection);
                            }
                            break;
                        case QuestionType.SLIDER:
                            {
                                tResult = EditSliderQuestion(id, pCollection);
                            }
                            break;
                        case QuestionType.STAR:
                            {
                                tResult = EditStarQuestion(id, pCollection);
                            }
                            break;
                    }

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            return RedirectToAction("Index");
                            break;
                        case ErrorCode.VALIDATION:
                            TempData["Error"] = "Question order already in use, Try using another one.";
                            ModelState.AddModelError("Order", "Question order already in use, Try using another one."); // didn't work because it bounce 2 requests
                            break;
                    }
                }


                return RedirectToAction("Edit", new { id = id, type = (QuestionType)type });

                return Redirect(Request.UrlReferrer.PathAndQuery);
                return View();
            }
            catch
            {
                return View();
            }
        }

        private ErrorCode EditSmileyQuestion(int id, FormCollection pCollection)
        {
            try
            {
                int tId = id;
                int tOrder = Convert.ToInt32(pCollection["Order"]);
                string tText = pCollection["Text"];
                QuestionType tType = QuestionType.SMILEY;
                int tNumberOfSmileyFaces = Convert.ToInt32(pCollection["NumberOfSmileyFaces"]);

                SmileyQuestion tSmileyQuestion = new SmileyQuestion(tId, tOrder, tText, tType, tNumberOfSmileyFaces);
                ErrorCode tResult = mQuestionManager.UpdateSmileyQuestion(tSmileyQuestion);
                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }


        private ErrorCode EditSliderQuestion(int id, FormCollection pCollection)
        {
            try
            {
                int tId = id;
                int tOrder = Convert.ToInt32(pCollection["Order"]);
                string tText = pCollection["Text"];
                QuestionType tType = QuestionType.SLIDER;
                int tStartValue = Convert.ToInt32(pCollection["StartValue"]);
                int tEndValue = Convert.ToInt32(pCollection["EndValue"]);
                string tStartValueCaption = pCollection["StartValueCaption"];
                string tEndValueCaption = pCollection["EndValueCaption"];


                SliderQuestion tSliderQuestion = new SliderQuestion(tId, tOrder, tText, tType, tStartValue, tEndValue, tStartValueCaption, tEndValueCaption);
                ErrorCode tResult = mQuestionManager.UpdateSliderQuestion(tSliderQuestion);

                if (tEndValue < tStartValue)
                {
                    TempData["Error"] = "End value must be larger than start value";
                }

                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        private ErrorCode EditStarQuestion(int id, FormCollection pCollection)
        {
            try
            {
                int tId = id;
                int tOrder = Convert.ToInt32(pCollection["Order"]);
                string tText = pCollection["Text"];
                QuestionType tType = QuestionType.STAR;
                int tNumberOfStars = Convert.ToInt32(pCollection["NumberOfStars"]);

                StarQuestion tStarQuestion = new StarQuestion(tId, tOrder, tText, tType, tNumberOfStars);
                ErrorCode tResult = mQuestionManager.UpdateStarQuestion(tStarQuestion);
                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
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
