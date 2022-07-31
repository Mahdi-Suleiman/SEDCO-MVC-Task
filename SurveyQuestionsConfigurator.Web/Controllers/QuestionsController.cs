using Microsoft.AspNet.SignalR;
using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Entities.Resources;
using SurveyQuestionsConfigurator.QuestionLogic;
using SurveyQuestionsConfigurator.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using static SurveyQuestionsConfigurator.Entities.Generic;
using static SurveyQuestionsConfigurator.Entities.Resources.EnumResourceStrings;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class QuestionsController : Controller
    {

        #region Properties & Attributes
        private readonly QuestionManager mQuestionManager;
        private readonly ResourceManager mLocalResourceManager;
        private readonly string mOfflineViewName;

        enum KeyConstants
        {
            ServerName,
            DatabaseName,
            UserId,
            Password,
            Checkconnectivity
        }

        enum ActionNameConstants
        {
            Index
        }

        #endregion

        #region Constructor
        public QuestionsController()
        {
            try
            {
                mQuestionManager = new QuestionManager();
                QuestionManager.refreshDataEvent += Refresh;
                mQuestionManager.WatchForChanges(); /// Subscribe to data changes event
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.Entities.Resources.LanguageStrings", typeof(LanguageStrings).Assembly);
                mOfflineViewName = "~/Views/Shared/Offline.cshtml";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
        #endregion

        #region Actions

        /// <summary>
        /// GET: /Questions
        /// Fill the list to be rendered in the view
        /// </summary>
        /// <returns>
        /// View
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                List<Question> testList = new List<Question>();
                ErrorCode tResult = mQuestionManager.GetAllQuestions(ref testList);
                switch (tResult)
                {
                    case ErrorCode.SUCCESS:
                    case ErrorCode.EMPTY:
                        return View(testList);
                    default:
                        return View("~/Views/Shared/Offline.cshtml");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View();
            }
        }
        public void Refresh(ErrorCode pErrorCode, List<Question> pQuestionList)
        {
            try
            {
                //tHubContext.Clients.All.addNewMessageToPage(null, null);
                switch (pErrorCode)
                {
                    case ErrorCode.SUCCESS:
                    case ErrorCode.EMPTY:
                        {
                            var tHubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionsHub>();
                            tHubContext.Clients.All.RefreshQuestionsList(pQuestionList);
                        }
                        break;
                    default:
                        break;
                }
                //RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //RedirectToAction("Index");
            }
        }

        /// <summary>
        /// GET: /Questions/Details/5?tType=SMILEY
        /// Create a new question object based on the tType passed in the query string
        /// and return it with the view to render its details
        /// </summary>
        /// <returns>
        /// View
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
                            ErrorCode tResult = mQuestionManager.GetSmileyQuestionByID(ref tSmileyQuestion);
                            switch (tResult)
                            {
                                case ErrorCode.SUCCESS:
                                case ErrorCode.EMPTY:
                                    return View("~/Views/SmileyQuestion/Details.cshtml", tSmileyQuestion);
                                default:
                                    return View("~/Views/Shared/Offline.cshtml");
                            }
                        }
                    case QuestionType.SLIDER:
                        {
                            SliderQuestion tSliderQuestion = new SliderQuestion(id);
                            ErrorCode tResult = mQuestionManager.GetSliderQuestionByID(ref tSliderQuestion);
                            switch (tResult)
                            {
                                case ErrorCode.SUCCESS:
                                case ErrorCode.EMPTY:
                                    return View("~/Views/SliderQuestion/Details.cshtml", tSliderQuestion);
                                default:
                                    return View("~/Views/Shared/Offline.cshtml");
                            }
                        }
                    case QuestionType.STAR:
                        {
                            StarQuestion tStarQuestion = new StarQuestion(id);
                            ErrorCode tResult = mQuestionManager.GetStarQuestionByID(ref tStarQuestion);
                            switch (tResult)
                            {
                                case ErrorCode.SUCCESS:
                                case ErrorCode.EMPTY:
                                    return View("~/Views/StarQuestion/Details.cshtml", tStarQuestion);
                                default:
                                    return View("~/Views/Shared/Offline.cshtml");
                            }
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
        /// <returns>
        /// View
        /// </returns>
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View();
            }
        }

        /// <summary>
        /// POST: Questions/Create
        /// 1) Validate the form the comes with the POST request
        /// 2) Get the corresponding question tType from the form
        /// 3) Add the corresponding question tType
        /// 4) Based on the result, redirect to the index action or return the same view with a validation error on the order field
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>
        /// View
        /// </returns>
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
                            ModelState.AddModelError($"{ResourceStrings.Order}", mLocalResourceManager.GetString($"{ResourceStrings.OrderAlreadyInUse}"));
                            break;
                        default:
                            return View("~/Views/Shared/Offline.cshtml");
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
        /// GET: Questions/Edit/5?type=SMILEY
        /// GET: Questions/Edit/5?type=SMILEY?ModelErrorName=loremIpsum?ModelErrorMessage=loremIpsum
        /// GET: Questions/Edit/5?type=SMILEY?ModelErrorName=loremIpsum?ModelErrorMessage=loremIpsum?order=1
        /// 1) Check if any errors are redirected from a failed POST request, if exist add to ModelState
        /// 2) Pass id, type and order to a function that rturns an appropriate view based on the existance of the question
        /// </summary>
        /// <returns>
        /// View
        /// </returns>
        [HttpGet]
        public ActionResult Edit(int id, QuestionType type, int order = -1, string ModelErrorName = null, string ModelErrorMessage = null)
        {
            try
            {
                if (ModelErrorName != null && ModelErrorMessage != null)
                {
                    ModelState.AddModelError(ModelErrorName, ModelErrorMessage);
                }

                switch (type)
                {
                    case QuestionType.SMILEY:
                    default:
                        {
                            return GetEditSmileyQuestion(id, type, order);
                        }
                    case QuestionType.SLIDER:
                        {
                            return GetEditSliderQuestion(id, type, order);
                        }
                    case QuestionType.STAR:
                        {
                            return GetEditStarQuestion(id, type, order);
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.SomethingWentWrongError}")}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// POST: Questions/Edit/5?type=SMILEY
        /// 1) Validated the posted form
        /// 2) Get question order from posted form
        /// 3) Return view based on the corresponding returned result
        /// </summary>
        /// <returns>
        /// View
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, QuestionType type, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int tOrder = Convert.ToInt32(collection["Order"]);
                    ErrorCode tResult = ErrorCode.ERROR;
                    switch (type)
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
                        case ErrorCode.VALIDATION:
                            return RedirectToAction("Edit", new { id = id, type = type, order = tOrder, ModelErrorName = $"{ResourceStrings.Order}", ModelErrorMessage = $"{mLocalResourceManager.GetString($"{ResourceStrings.OrderAlreadyInUse}")}" });
                        default:
                            return RedirectToAction("Edit", new { id = id, type = type, order = tOrder, ModelErrorName = $"{ResourceStrings.Order}", ModelErrorMessage = $"{mLocalResourceManager.GetString($"{ResourceStrings.SomethingWentWrongError}")}" });
                    }
                }
                return RedirectToAction("Edit", new { id = id, type = type });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return RedirectToAction("Edit", new { id = id, type = type });
                //return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }

        /// <summary>
        /// GET: Questions/Delete/5?type=SMILEY
        /// 1) Get a question form DB
        /// 2) Return its corresponding view
        /// </summary>
        /// <returns>
        /// View
        /// </returns>
        [HttpGet]
        public ActionResult Delete(int id, QuestionType type)
        {
            try
            {
                switch (type)
                {
                    case QuestionType.SMILEY:
                        {
                            SmileyQuestion tSmileyQuestion = new SmileyQuestion(id);
                            ErrorCode tResult = mQuestionManager.GetSmileyQuestionByID(ref tSmileyQuestion);
                            return View(tSmileyQuestion);
                        }
                    case QuestionType.SLIDER:
                        {
                            SliderQuestion tSliderQuestion = new SliderQuestion(id);
                            ErrorCode tResult = mQuestionManager.GetSliderQuestionByID(ref tSliderQuestion);
                            return View(tSliderQuestion);
                        }
                    case QuestionType.STAR:
                        {
                            StarQuestion tStarQuestion = new StarQuestion(id);
                            ErrorCode tResult = mQuestionManager.GetStarQuestionByID(ref tStarQuestion);
                            return View(tStarQuestion);
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
        /// POST: Questions/Delete/5type=SMILEY
        /// 1) Try to delete a question and get the operation
        /// 2) Return corresponding view with appropriate message to view
        /// </summary>
        /// <returns>
        /// View
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                ErrorCode tResult = mQuestionManager.DeleteQuestionByID(id);
                switch (tResult)
                {
                    case ErrorCode.SUCCESS:
                        return RedirectToAction("Index");
                    default:
                        TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.SomethingWentWrongError}")}";
                        return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View();
            }
        }

        #endregion

        #region actions that returns their relative partial views

        /// <summary>
        /// used by jquery ajax requests
        /// </summary>
        /// <returns>
        /// PartialView("_CreateSmileyQuestion")
        /// </returns>
        public ActionResult PartialSmiley()
        {
            try
            {
                return PartialView("_CreateSmileyQuestion");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return PartialView("_CreateSmileyQuestion");
            }
        }

        /// <summary>
        /// used by jquery ajax requests
        /// </summary>
        /// <returns>
        /// PartialView("_CreateSliderQuestion")
        /// </returns>
        public ActionResult PartialSlider()
        {
            try
            {
                return PartialView("_CreateSliderQuestion");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return PartialView("_CreateSmileyQuestion");
            }
        }

        /// <summary>
        /// used by jquery ajax requests
        /// </summary>
        /// <returns>
        /// PartialView("_CreateStarQuestion")
        /// </returns>
        public ActionResult PartialStar()
        {
            try
            {
                return PartialView("_CreateStarQuestion");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return PartialView("_CreateSmileyQuestion");
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// 1) Create a question object and attempt to add it to DB
        /// 2) Return operation's result
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
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

        /// <summary>
        /// 1) Create a question object and attempt to add it to DB
        /// 2) Return operation's result
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
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
                    TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.EndValueError}")}";
                }

                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// 1) Create a question object and attempt to add it to DB
        /// 2) Return operation's result
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
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
        /// Get question by id
        /// Check of there is a passed order from post edit request that is caused by a validation error, if any set it as the order of the question (This improves the UX by not reseting the order field to 1 on every request)
        /// </summary>
        /// <returns>
        /// View("DisabledEdit")
        /// View("~/Views/SmileyQuestion/Edit.cshtml")
        /// </returns>
        private ActionResult GetEditSmileyQuestion(int id, QuestionType type, int order)
        {
            try
            {
                SmileyQuestion tSmileyQuestion = new SmileyQuestion(id);
                ErrorCode tReslut = mQuestionManager.GetSmileyQuestionByID(ref tSmileyQuestion);
                if (order != -1)
                {
                    tSmileyQuestion.Order = order;
                }
                if (tReslut != ErrorCode.SUCCESS)
                {
                    TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.QuestionDoesNotExistError}")}";
                    return View("DisabledEdit");
                }
                return View("~/Views/SmileyQuestion/Edit.cshtml", tSmileyQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View("~/Views/SmileyQuestion/Edit.cshtml");
            }
        }

        /// <summary>
        /// Get question by id
        /// Check of there is a passed order from post edit request that is caused by a validation error, if any set it as the order of the question (This improves the UX by not reseting the order field to 1 on every request)
        /// </summary>
        /// <returns>
        /// View("DisabledEdit")
        /// View("~/Views/SmileyQuestion/Edit.cshtml")
        /// </returns>
        private ActionResult GetEditSliderQuestion(int id, QuestionType type, int order)
        {
            try
            {
                SliderQuestion tSliderQuestion = new SliderQuestion(id);
                ErrorCode tReslut = mQuestionManager.GetSliderQuestionByID(ref tSliderQuestion);
                if (order != -1)
                {
                    tSliderQuestion.Order = order;
                }
                if (tReslut != ErrorCode.SUCCESS)
                {
                    TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.QuestionDoesNotExistError}")}";
                    return View("DisabledEdit");
                }
                return View("~/Views/SliderQuestion/Edit.cshtml", tSliderQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View("~/Views/SliderQuestion/Edit.cshtml");
            }
        }

        /// <summary>
        /// Get question by id
        /// Check of there is a passed order from post edit request that is caused by a validation error, if any set it as the order of the question (This improves the UX by not reseting the order field to 1 on every request)
        /// </summary>
        /// <returns>
        /// View("DisabledEdit")
        /// View("~/Views/SmileyQuestion/Edit.cshtml")
        /// </returns>
        private ActionResult GetEditStarQuestion(int id, QuestionType type, int order)
        {
            try
            {
                StarQuestion tStarQuestion = new StarQuestion(id);
                ErrorCode tReslut = mQuestionManager.GetStarQuestionByID(ref tStarQuestion);
                if (order != -1)
                {
                    tStarQuestion.Order = order;
                }
                if (tReslut != ErrorCode.SUCCESS)
                {
                    TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.QuestionDoesNotExistError}")}";
                    return View("DisabledEdit");
                }
                return View("~/Views/StarQuestion/Edit.cshtml", tStarQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View("~/Views/StarQuestion/Edit.cshtml");
            }
        }

        /// <summary>
        /// 1) Create a question object and attempt to add it to DB
        /// 2) Return operation's result
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
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

        /// <summary>
        /// 1) Create a question object and attempt to add it to DB
        /// 2) Return operation's result
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
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
                    TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.EndValueError}")}";
                }

                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

        /// <summary>
        /// 1) Create a question object and attempt to add it to DB
        /// 2) Return operation's result
        /// </summary>
        /// <returns>
        /// ErrorCode.SUCCESS
        /// ErrorCode.ERROR
        /// ErrorCode.VALIDATION
        /// </returns>
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

        #endregion

    }
}
