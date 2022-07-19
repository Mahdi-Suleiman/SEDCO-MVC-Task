using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace SurveyQuestionsConfigurator.CommonHelpers
{
    public class Logger
    {
        //private static string mExeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); ///Gets the directory name of the current exe
        //private static string mExeFolder = System.Web.Hosting.HostingEnvironment.MapPath("~/bin"); ///Gets the directory name of the current exe
        private static string mExeFolder = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin"); ///Gets the directory name of the current exe
        private static object mBalanceLock = new object(); /// used to lock writing on file
        private static object mBalanceLock2 = null; /// used to lock writing on file for the backup function

        public static void LogError(Exception pEx)
        {
            try
            {
                /// lock file writing to prevent exception System.IO.IOException : 
                /// The process cannot access the file 'LogFile.txt' because it is being used by another process
                lock (mBalanceLock)
                {
                    using (StreamWriter sw = new StreamWriter($"{mExeFolder}/logs/LogFile.txt", append: true))
                    {
                        sw.WriteLine($@"
 -------------- ({DateTime.Now}) --------------

-Exception Type: {pEx.GetType()}
-Exception Call Site: {pEx.TargetSite}
-Exception Short Message: {pEx.Message}
-Exception Long Message: {pEx}
-Exception Stack Trace: {pEx.StackTrace}
");
                    }
                }
            }
            catch (Exception ex2)
            {
                BackUpLogger(ex2);
            }
        }

        /// Log error in case main "LogError" faced an exception
        public static void BackUpLogger(Exception pEx)
        {
            try
            {
                mBalanceLock2 = new object();
                lock (mBalanceLock2)
                {
                    using (StreamWriter sw = new StreamWriter($"{mExeFolder}/logs/LogFile.txt", append: true))
                    {
                        sw.WriteLine($@"
 -------------- ({DateTime.Now}) --------------

-Exception Type: {pEx.GetType()}
-Exception Call Site: {pEx.TargetSite}
-Exception Short Message: {pEx.Message}
-Exception Long Message: {pEx}
-Exception Stack Trace: {pEx.StackTrace}
");
                    }
                }
            }
            catch
            {

            }
        }
    }
}
