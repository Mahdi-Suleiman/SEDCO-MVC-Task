using Microsoft.AspNet.SignalR;
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
            Clients.All.RefreshQuestionsList(pQuestionList);
        }

        //    public void Send(string name, string message)
        //    {
        //        //Clients.All.hello();
        //        Clients.All.addNewMessageToPage();
        //    }

        //    public void Send(Object pObject)
        //    {
        //        //Clients.All.hello();
        //        Clients.All.addNewMessageToPage(pObject);
        //    }
        //}
    }
}