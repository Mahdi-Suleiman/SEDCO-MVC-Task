using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;


namespace SurveyQuestionsConfigurator.Entities
{
    public class SmileyQuestion : Question
    {
        [Required]
        [Range(2, 5)]
        [Display(Name = "Number Of Smiley Faces(2-5):")]
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
