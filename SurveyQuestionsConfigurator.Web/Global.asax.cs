using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SurveyQuestionsConfigurator.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string tDefaultCulture = ConfigurationManager.AppSettings["DefaultCulture"];
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(tDefaultCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(tDefaultCulture);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string tDefaultCulture = ConfigurationManager.AppSettings["DefaultCulture"];
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(tDefaultCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(tDefaultCulture);
        }
    }
}
