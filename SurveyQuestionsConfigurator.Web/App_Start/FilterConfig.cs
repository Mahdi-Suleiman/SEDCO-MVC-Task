using SurveyQuestionsConfigurator.CommonHelpers;
using System;
using System.Web;
using System.Web.Mvc;

namespace SurveyQuestionsConfigurator.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            try
            {
                filters.Add(new HandleErrorAttribute());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}
