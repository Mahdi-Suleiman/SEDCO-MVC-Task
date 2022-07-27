using Microsoft.AspNet.SignalR;
using SurveyQuestionsConfigurator.CommonHelpers;
using SurveyQuestionsConfigurator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyQuestionsConfigurator.Web.Hubs
{
    //public interface IQuestionsHub
    //{
    //    void RefreshQuestionsList(List<Question> pQuestionList);
    //}
    public class QuestionsHub : Hub<IQuestionsHub>
    {
        public void Refresh(List<Question> pQuestionList)
        {
            try
            {
                Clients.All.RefreshQuestionsList(pQuestionList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}