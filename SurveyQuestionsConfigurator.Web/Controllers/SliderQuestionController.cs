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
    public class SliderQuestionController : Controller
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
        public SliderQuestionController()
        {
            mQuestionManager = new QuestionManager();
            mOfflineViewName = "~/Views/Shared/Offline.cshtml";
            mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.Entities.Resources.LanguageStrings", typeof(LanguageStrings).Assembly);

        }

        // GET: SliderQuestion/Create
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

        // POST: SliderQuestion/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                SliderQuestion tSliderQuestion = null;
                if (ModelState.IsValid)
                {
                    int tType = Convert.ToInt32(collection[nameof(ResourceStrings.Type)]);
                    string tSliderModelError = null, tSliderModelErrorMessaage = null;
                    ErrorCode tResult = AddSliderQuestion(collection, ref tSliderQuestion, ref tSliderModelError, ref tSliderModelErrorMessaage);


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
                return View(tSliderQuestion);
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
        private ErrorCode AddSliderQuestion(FormCollection collection, ref SliderQuestion pSliderQuestion, ref string pSliderModelError, ref string pSliderModelErrorMessaage)
        {
            try
            {
                int tId = -1;
                int tOrder = Convert.ToInt32(collection[nameof(ResourceStrings.Order)]);
                string tText = collection[nameof(ResourceStrings.Text)];
                QuestionType tType = QuestionType.SLIDER;
                int tStartValue = Convert.ToInt32(collection[nameof(ResourceStrings.StartValue)]);
                int tEndValue = Convert.ToInt32(collection[nameof(ResourceStrings.EndValue)]);
                string tStartValueCaption = collection[nameof(ResourceStrings.StartValueCaption)];
                string tEndValueCaption = collection[nameof(ResourceStrings.EndValueCaption)];


                SliderQuestion tSliderQuestion = new SliderQuestion(tId, tOrder, tText, tType, tStartValue, tEndValue, tStartValueCaption, tEndValueCaption);
                pSliderQuestion = tSliderQuestion;
                ErrorCode tResult = mQuestionManager.InsertSliderQuestion(tSliderQuestion);

                if (tResult == ErrorCode.VALIDATION)
                {
                    //TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.OrderAlreadyInUse}")}";
                    //TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.OrderAlreadyInUse}")}";
                    //pSliderModelError;
                    pSliderModelError = $"{ResourceStrings.Order}";
                    pSliderModelErrorMessaage = $"{mLocalResourceManager.GetString($"{ResourceStrings.OrderAlreadyInUse}")}";
                }

                if (tEndValue < tStartValue)
                {
                    //ModelState.AddModelError("EndValue", "End value must be larger than start value");
                    //TempData[$"{ResourceStrings.Error}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.EndValueError}")}";
                    //pSliderModelError = $"{ResourceStrings.EndValue}";
                    //pSliderModelErrorMessaage = $"{mLocalResourceManager.GetString($"{ResourceStrings.EndValueError}")}";
                    //TempData[$"{ResourceStrings.EndValue}"] = $"{mLocalResourceManager.GetString($"{ResourceStrings.EndValueError}")}";
                    pSliderModelError = $"{ResourceStrings.EndValue}";
                    pSliderModelErrorMessaage = $"{mLocalResourceManager.GetString($"{ResourceStrings.EndValueError}")}";
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
