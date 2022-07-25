using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyQuestionsConfigurator.Web
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            //Clients.All.hello();
            Clients.All.addNewMessageToPage();
        }

        public void Send(Object pObject)
        {
            //Clients.All.hello();
            Clients.All.addNewMessageToPage(pObject);
        }
    }
}