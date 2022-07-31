using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;
using static SurveyQuestionsConfigurator.Entities.Resources.EnumResourceConstants;

namespace SurveyQuestionsConfigurator.Entities
{
    public class SliderQuestion : Question
    {
        #region Attributes

        [Display(Name = nameof(ResourceConstants.StartValue), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.StartValueRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [Range(1, 99)]
        public int StartValue { get; set; }

        [Display(Name = nameof(ResourceConstants.EndValue), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.EndValueRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [Range(1, 100)]
        public int EndValue { get; set; }

        [Display(Name = nameof(ResourceConstants.StartValueCaption), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.StartValueCaptionRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [MaxLength(100)]
        public string StartValueCaption { get; set; }

        [Display(Name = nameof(ResourceConstants.EndValueCaption), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.EndValueCaptionRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [MaxLength(100)]
        public string EndValueCaption { get; set; }
        #endregion

        public SliderQuestion(int pID, int pOrder, string pText, QuestionType pType, int pStartValue, int pEndValue, string pStartValueCaption, string pEndValueCaption) :
      base(pID, pOrder, pText, pType)
        {
            try
            {
                StartValue = pStartValue;
                EndValue = pEndValue;
                StartValueCaption = pStartValueCaption;
                EndValueCaption = pEndValueCaption;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public SliderQuestion(int id) :

           base(id)
        { }

        public SliderQuestion(SliderQuestion sliderQuestion) :
          this(sliderQuestion.ID, sliderQuestion.Order, sliderQuestion.Text, sliderQuestion.Type, sliderQuestion.StartValue, sliderQuestion.EndValue, sliderQuestion.StartValueCaption, sliderQuestion.EndValueCaption)
        { }
    }
}
