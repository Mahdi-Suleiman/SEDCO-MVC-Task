using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.Entities
{
    public class StarQuestion : Question
    {
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
