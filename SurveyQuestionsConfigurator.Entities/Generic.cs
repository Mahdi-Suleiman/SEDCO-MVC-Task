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
        public enum KeyConstants
        {
            ServerName,
            DatabaseName,
            UserId,
            Password,
            Checkconnectivity
        }

        public enum ActionNameConstants
        {
            Index,
            Edit,
            DisabledEdit,
            Create,
            _CreateSmileyQuestion,
            _CreateSliderQuestion,
            _CreateStarQuestion
        }


        public enum ControllerNameConstants
        {
            SmileyQuestion,
            SliderQuestion,
            StarQuestion
        }

        public enum ControllerConstants
        {
            Questions,
        }
    }
}
