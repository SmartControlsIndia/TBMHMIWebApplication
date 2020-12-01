using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TBMHMIWebApplication
{
    public class Uitility
    {

        public static void LogError(Exception ex,string buildingMachine)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
         
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;

            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string date = DateTime.Now.ToString("ddMMMyyyy");
            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Errorlogfiles\" + (buildingMachine));
         //   System.IO.Directory.CreateDirectory(buildingMachine);
            string path = HttpContext.Current.Server.MapPath(@"~/Errorlogfiles/" + buildingMachine +@"/"+ date + ".txt");

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
        public static void LogEvent(string logevent, string buildingMachine)
        {
            string message =DateTime.Now.ToString()+"   "+ logevent;


            // string path = Application.StartupPath+ @"\EventLog\ErrorLog.txt";
            string date = DateTime.Now.ToString("ddMMMyyyy");
            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"EventLog\"+(buildingMachine));
            string path = AppDomain.CurrentDomain.BaseDirectory + @"EventLog\"+ buildingMachine + @"\" + date + ".txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }

}