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
    public class SmileyQuestion : Question
    {
        [Display(Name = nameof(ResourceConstants.NumberOfSmileyFaces), ResourceType = typeof(LanguageStrings))]
        [Required(ErrorMessageResourceName = nameof(ResourceConstants.NumberOfSmileyFacesRequired), ErrorMessageResourceType = typeof(LanguageStrings))]
        [Range(2, 5)]
        public int NumberOfSmileyFaces { get; set; }
        public SmileyQuestion(int pID, int pOrder, string pText, QuestionType pType, int pNumberOfSmileyFaces) :
            base(pID, pOrder, pText, pType)
        {
            try
            {
                NumberOfSmileyFaces = pNumberOfSmileyFaces;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

        }

        public SmileyQuestion(int pID) :

            base(pID)
        { }

        public SmileyQuestion(SmileyQuestion smileyQuestion) :
            this(smileyQuestion.ID, smileyQuestion.Order, smileyQuestion.Text, smileyQuestion.Type, smileyQuestion.NumberOfSmileyFaces)
        { }
    }
}
