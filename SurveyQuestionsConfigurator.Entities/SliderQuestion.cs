using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SurveyQuestionsConfigurator.Entities.Generic;

namespace SurveyQuestionsConfigurator.Entities
{
    public class SliderQuestion : Question
    {
        #region Attributes
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public string StartValueCaption { get; set; }
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
