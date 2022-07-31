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
using static SurveyQuestionsConfigurator.Entities.Resources.EnumResourceConstants;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class SliderQuestionController : Controller
    {
        #region Properties & Attributes

        #endregion
        #region Constructor

        #endregion
        #region Actions

        #endregion
        private readonly QuestionManager mQuestionManager;
        private readonly string mOfflineViewName;
        private readonly ResourceManager mLocalResourceManager;


        public SliderQuestionController()
        {
            try
            {
                mQuestionManager = new QuestionManager();
                mOfflineViewName = "~/Views/Shared/Offline.cshtml";
                mLocalResourceManager = new ResourceManager("SurveyQuestionsConfigurator.Entities.Resources.LanguageStrings", typeof(LanguageStrings).Assembly);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// GET: SliderQuestion/Create
        /// Returns the create view
        /// </summary>
        /// <returns>
        /// View
        /// </returns>
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
        /// POST: SliderQuestion/Create
        /// 1) Validate the form the comes with the POST request
        /// 2) Get the corresponding question tType from the form
        /// 3) Add the corresponding question tType
        /// 4) Based on the result, redirect to the index action or return the same view with a validation error on the order field
        /// </summary>
        /// <returns>
        /// View
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                SliderQuestion tSliderQuestion = null;
                if (ModelState.IsValid)
                {
                    int tType = Convert.ToInt32(collection[nameof(ResourceConstants.Type)]);
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
                int tOrder = Convert.ToInt32(collection[nameof(ResourceConstants.Order)]);
                string tText = collection[nameof(ResourceConstants.Text)];
                QuestionType tType = QuestionType.SLIDER;
                int tStartValue = Convert.ToInt32(collection[nameof(ResourceConstants.StartValue)]);
                int tEndValue = Convert.ToInt32(collection[nameof(ResourceConstants.EndValue)]);
                string tStartValueCaption = collection[nameof(ResourceConstants.StartValueCaption)];
                string tEndValueCaption = collection[nameof(ResourceConstants.EndValueCaption)];


                SliderQuestion tSliderQuestion = new SliderQuestion(tId, tOrder, tText, tType, tStartValue, tEndValue, tStartValueCaption, tEndValueCaption);
                pSliderQuestion = tSliderQuestion;
                ErrorCode tResult = mQuestionManager.InsertSliderQuestion(tSliderQuestion);

                if (tResult == ErrorCode.VALIDATION)
                {
                    pSliderModelError = $"{ResourceConstants.Order}";
                    pSliderModelErrorMessaage = $"{mLocalResourceManager.GetString($"{ResourceConstants.OrderAlreadyInUse}")}";
                }

                if (tEndValue < tStartValue)
                {
                    pSliderModelError = $"{ResourceConstants.EndValue}";
                    pSliderModelErrorMessaage = $"{mLocalResourceManager.GetString($"{ResourceConstants.EndValueError}")}";
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
