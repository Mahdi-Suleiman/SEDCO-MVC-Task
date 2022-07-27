using Microsoft.Owin;
using Owin;
using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(SurveyQuestionsConfigurator.Web.Startup))]
namespace SurveyQuestionsConfigurator.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                // Any connection or hub wire up and configuration should go here
                app.MapSignalR();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}