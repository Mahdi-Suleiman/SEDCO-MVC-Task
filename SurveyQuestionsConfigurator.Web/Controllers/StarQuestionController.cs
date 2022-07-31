using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using SurveyQuestionsConfigurator.Entities.Resources;
using SurveyQuestionsConfigurator.QuestionLogic;
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
    public class StarQuestionController : Controller
    {
        private readonly QuestionManager mQuestionManager;
        private readonly string mOfflineViewName;
        private readonly ResourceManager mLocalResourceManager;


        enum ActionNameConstants
        {
            Index,
        }
        enum ControllerConstants
        {
            Questions,
        }
        public StarQuestionController()
        {
            mQuestionManager = new QuestionManager();
            mOfflineViewName = "~/Views/Shared/Offline.cshtml";
            mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.Entities.Resources.LanguageStrings", typeof(LanguageStrings).Assembly);

        }

        // GET: StarQuestion/Create
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

        // POST: StarQuestion/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                StarQuestion tStarQuestion = null;
                if (ModelState.IsValid)
                {
                    int tType = Convert.ToInt32(collection[nameof(ResourceStrings.Type)]);
                    string tSliderModelError = null, tSliderModelErrorMessaage = null;
                    ErrorCode tResult = AddStarQuestion(collection, ref tStarQuestion, ref tSliderModelError, ref tSliderModelErrorMessaage);


                    switch (tResult)
                    {
                        case ErrorCode.SUCCESS:
                            return RedirectToAction(nameof(ActionNameConstants.Index), nameof(ControllerConstants.Questions));
                        case ErrorCode.VALIDATION:
                            if (tSliderModelError != null && tSliderModelErrorMessaage != null)
                            {
                                ModelState.AddModelError(tSliderModelError, tSliderModelErrorMessaage);
                            }
                            break;
                        default:
                            return View(mOfflineViewName);
                    }
                }
                return View(tStarQuestion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return View();
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
        private ErrorCode AddStarQuestion(FormCollection collection, ref StarQuestion pStarQuestion, ref string pSliderModelError, ref string pSliderModelErrorMessaage)
        {
            try
            {
                int tId = -1;
                int tOrder = Convert.ToInt32(collection[nameof(ResourceStrings.Order)]);
                string tText = collection[nameof(ResourceStrings.Text)];
                QuestionType tType = QuestionType.STAR;
                int tNumberOfStars = Convert.ToInt32(collection[nameof(ResourceStrings.NumberOfStars)]);

                StarQuestion tStarQuestion = new StarQuestion(tId, tOrder, tText, tType, tNumberOfStars);
                pStarQuestion = tStarQuestion;
                ErrorCode tResult = mQuestionManager.InsertStarQuestion(tStarQuestion);

                if (tResult == ErrorCode.VALIDATION)
                {
                    pSliderModelError = $"{ResourceStrings.Order}";
                    pSliderModelErrorMessaage = $"{mLocalResourceManager.GetString($"{ResourceStrings.OrderAlreadyInUse}")}";
                }

                return tResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return ErrorCode.ERROR;
            }
        }

    }
}
