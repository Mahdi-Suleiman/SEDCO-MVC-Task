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

        /// <summary>
        /// GET: /Questions
        /// Fill the list to be rendered in the view
        /// </summary>
        /// <returns>
        /// ActionResult
        /// </returns>
        [HttpGet]
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
                return View();
            }
        }

        /// <summary>
        /// GET: /Questions/Details/5?tType=SMILEY
        /// Create a new question object based on the tType passed in the query string
        /// and return it with the view to render its details
        /// </summary>
        /// <returns>
        /// ActionResult
        /// </returns>
        [HttpGet]
        public ActionResult Details(int id, QuestionType type)
        {
            try
            {
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
                }
                return View();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View();
            }
        }

        /// <summary>
        /// GET: /Questions/Create
        /// Returns the create view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: Questions/Create
        /// 1) Validate the form the comes with the POST request
        /// 2) Get the corresponding question tType from the form
        /// 3) Add the corresponding question tType
        /// 4) Based on the result, redirect to the index action or return the same view with a validation error on the order field
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int tType = Convert.ToInt32(collection["Type"]);
                    ErrorCode tResult = ErrorCode.ERROR;
                    switch ((QuestionType)tType)
                    {
                        case QuestionType.SMILEY:
                            {
                                tResult = AddSmileyQuestion(collection);
                            }
                            break;
                        case QuestionType.SLIDER:
                            {
                                tResult = AddSliderQuestion(collection);
                            }
                            break;
                        case QuestionType.STAR:
                            {
                                tResult = AddStarQuestion(collection);
                            }
                            break;
                    }

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            return RedirectToAction("Index");
                        case ErrorCode.VALIDATION:
                            ModelState.AddModelError("Order", "Question order already in use, Try using another one.");
                            break;
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View();
            }
        }

        private ErrorCode AddSmileyQuestion(FormCollection collection)
        {
            try
            {
                int tId = -1;
                int tOrder = Convert.ToInt32(collection["Order"]);
                string tText = collection["Text"];
                QuestionType tType = QuestionType.SMILEY;
                int tNumberOfSmileyFaces = Convert.ToInt32(collection["NumberOfSmileyFaces"]);

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

        private ErrorCode AddSliderQuestion(FormCollection collection)
        {
            try
            {
                int tId = -1;
                int tOrder = Convert.ToInt32(collection["Order"]);
                string tText = collection["Text"];
                QuestionType tType = QuestionType.SLIDER;
                int tStartValue = Convert.ToInt32(collection["StartValue"]);
                int tEndValue = Convert.ToInt32(collection["EndValue"]);
                string tStartValueCaption = collection["StartValueCaption"];
                string tEndValueCaption = collection["EndValueCaption"];


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

        private ErrorCode AddStarQuestion(FormCollection collection)
        {
            try
            {
                int tId = -1;
                int tOrder = Convert.ToInt32(collection["Order"]);
                string tText = collection["Text"];
                QuestionType tType = QuestionType.STAR;
                int tNumberOfStars = Convert.ToInt32(collection["NumberOfStars"]);

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

        /// <summary>
        /// GET: Questions/Edit/5?type=SMILEY
        /// GET: Questions/Edit/5?type=SMILEY?ModelErrorName=loremIpsum?ModelErrorMessage=loremIpsum
        /// </summary>
        /// <param name="ModelErrorName"></param>
        /// <param name="ModelErrorMessage"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id, QuestionType type, string ModelErrorName = null, string ModelErrorMessage = null, int order = -1)
        {
            if (ModelErrorName != null && ModelErrorMessage != null)
            {
                ModelState.AddModelError(ModelErrorName, ModelErrorMessage); // didn't work because it bounce 2 requests
            }

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
        public ActionResult Edit(int id, QuestionType type, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ErrorCode tResult = ErrorCode.ERROR;
                    switch ((QuestionType)type)
                    {
                        case QuestionType.SMILEY:
                            {
                                tResult = EditSmileyQuestion(id, collection);
                            }
                            break;
                        case QuestionType.SLIDER:
                            {
                                tResult = EditSliderQuestion(id, collection);
                            }
                            break;
                        case QuestionType.STAR:
                            {
                                tResult = EditStarQuestion(id, collection);
                            }
                            break;
                    }

                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            return RedirectToAction("Index");
                            break;
                        case ErrorCode.VALIDATION:
                            //TempData["Error"] = "Question order already in use, Try using another one.";
                            return RedirectToAction("Edit", new { id = id, type = (QuestionType)type, ModelErrorName = "Order", ModelErrorMessage = "Question order already in use, Try using another one." });
                            //ModelState.AddModelError("Order", "Question order already in use, Try using another one."); // didn't work because it bounce 2 requests
                            break;
                    }
                }


                return RedirectToAction("Edit", new { id = id, type = (QuestionType)type });
                return Redirect(Request.UrlReferrer.PathAndQuery);
                return View();
            }
            catch
            {
                return Redirect(Request.UrlReferrer.PathAndQuery);
                return View();
            }
        }

        private ErrorCode EditSmileyQuestion(int id, FormCollection collection)
        {
            try
            {
                int tId = id;
                int tOrder = Convert.ToInt32(collection["Order"]);
                string tText = collection["Text"];
                QuestionType tType = QuestionType.SMILEY;
                int tNumberOfSmileyFaces = Convert.ToInt32(collection["NumberOfSmileyFaces"]);

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


        private ErrorCode EditSliderQuestion(int id, FormCollection Collection)
        {
            try
            {
                int tId = id;
                int tOrder = Convert.ToInt32(Collection["Order"]);
                string tText = Collection["Text"];
                QuestionType tType = QuestionType.SLIDER;
                int tStartValue = Convert.ToInt32(Collection["StartValue"]);
                int tEndValue = Convert.ToInt32(Collection["EndValue"]);
                string tStartValueCaption = Collection["StartValueCaption"];
                string tEndValueCaption = Collection["EndValueCaption"];


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

        private ErrorCode EditStarQuestion(int id, FormCollection collection)
        {
            try
            {
                int tId = id;
                int tOrder = Convert.ToInt32(collection["Order"]);
                string tText = collection["Text"];
                QuestionType tType = QuestionType.STAR;
                int tNumberOfStars = Convert.ToInt32(collection["NumberOfStars"]);

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
