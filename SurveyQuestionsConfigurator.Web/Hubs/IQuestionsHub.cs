using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyQuestionsConfigurator.Web.Hubs
{
    public interface IQuestionsHub
    {
        void RefreshQuestionsList(List<Question> pQuestionList);
    }
}
