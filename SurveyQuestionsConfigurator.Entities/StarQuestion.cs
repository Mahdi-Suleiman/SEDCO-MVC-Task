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
    public class StarQuestion : Question
    {
        [Display(Name = nameof(ResourceConstants.NumberOfStars), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.NumberOfStarsRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [Range(1, 10)]
        public int NumberOfStars { get; set; }

        public StarQuestion(int pID, int pOrder, string pText, QuestionType pType, int pNumberOfStars) :
            base(pID, pOrder, pText, pType)
        {
            try
            {
                NumberOfStars = pNumberOfStars;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public StarQuestion(int pID) :

            base(pID)
        { }

        public StarQuestion(StarQuestion starQuestion) :
         this(starQuestion.ID, starQuestion.Order, starQuestion.Text, starQuestion.Type, starQuestion.NumberOfStars)
        { }
    }
}
