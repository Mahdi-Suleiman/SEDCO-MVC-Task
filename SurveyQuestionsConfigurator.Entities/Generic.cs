using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Entities
{
    public class Generic
    {
        public enum ErrorCode
        {
            ERROR = -1,
            SUCCESS = 1,
            VALIDATION = 2,
            EMPTY = 3
        }

        public enum QuestionType
        {
            SMILEY = 0,
            SLIDER = 1,
            STAR = 2
        }
        public enum QuestionColumn
        {
            ID,
            Order,
            Text,
            Type,
            NumberOfSmileyFaces,
            NumberOfStars,
            StartValue,
            EndValue,
            StartValueCaption,
            EndValueCaption,
            ReturnValue
        }
    }
}
