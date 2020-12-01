using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartLogic;
using System.Windows.Forms;
using System.IO;

namespace TBMHMIWebApplication 
{
    public class OPCManager
    {
        loginformation inf = new loginformation();

        static string FilePath = HttpContext.Current.Server.MapPath(@"~/Configuration/config.ini");
        INIFile INIFile = new INIFile(FilePath);
        public AutoCompleteStringCollection WcName;
        SmartLog smartLog = new SmartLog(INIFile.Read("path", "EventLogPath"), INIFile.Read("Path", "ErrorLogPath"));
              
        SmartOPC opcManager;
        string configFile = "";
       
        public bool opcstarted = false;

        public string OK = "";

        public string NOTOK = "";
        public string Start = "";
        public string Stop = "";
        public string Receipe = "";
        public SmartOPC TBMHMI()
        {
            return opcManager;
        }

        public loginformation TBMinf()
        {
            return inf;
        }
        public OPCManager()
        {
            
           
            //maintmr = new System.Timers.Timer(1000);
            //maintmr.Enabled = false;
            opcManager = new SmartOPC(INIFile.Read("OPC Server", "opcServer"),  smartLog);
           
              WcName = new AutoCompleteStringCollection();
            inf.opcItemID = new AutoCompleteStringCollection();
            inf.opcItem = new AutoCompleteStringCollection();
            inf.opcvalue = new AutoCompleteStringCollection();
            configFile = INIFile.Read("Path", "TagConfigPath");
          

            try
            {
                LoadConfiguration();

                opcManager.Initialize(inf.opcItemID, inf.opcItem);

                //opcManager.StartData();

                //opcstarted = true;

                startopc();


                //this.maintmr.Elapsed += new System.Timers.ElapsedEventHandler(maintmr_Elapsed);
                //maintmr.Enabled = true;

            }
            catch (Exception exc)
            {

            }

        }
      
        public void startopc()
        {
            bool flag = false;

            opcstarted = false;
            
             flag=  opcManager.StartData();

            if (flag)
            {

                opcstarted = true;
            }

             
        }
        public void stoptopc()
        {

            opcManager.StopData();

            opcstarted = false;

        }


        public int OPCStatus()
        {
            int x = 0;
            try
            {

                if (opcManager.opcRunningState() == true)
                {

                    x = 1;
                }
            }
            catch (Exception exp)
            {

            }
            //if (opcstarted == true)
            //{
            //    x = 1;
            //}
            return x;
        }

        public void   OKCode()
        {
            opcManager.WriteData(1, inf.opcItemID[inf.opcItemID.IndexOf("Ok")]);
        }
        public void NOKCode()
        {
            opcManager.WriteData(1, inf.opcItemID[inf.opcItemID.IndexOf("Nok")]);
        }

        private void LoadConfiguration()
        {
            try
            {

                inf.csvReader = new StreamReader(configFile);

                inf.csvReader.ReadLine();


                inf.content = inf.csvReader.ReadLine();

                int i = 0;

                while (inf.content != null)
                {

                    setFilePath(inf.content);

                    inf.content = inf.csvReader.ReadLine();

                    i++;
                }

                inf.csvReader.Close();



                inf.metadata = EventID.Load_Configuration + "#" + DateTime.Now.ToString() + "#" + configFile + "#" + "Loaded successfully" + "#" + "";
                inf.args = inf.metadata.Split(new char[] { '#' });
                smartLog.WriteEventMessage(inf.args);

            }
            catch (Exception exc)
            {

                // Uitility.LogError(exc);

                //inf.metadata = "Error" + "#" + DateTime.Now.ToString() + "#" + "Load Configuration" + "#" + exc.Message + "#" + "";
                //inf.args = inf.metadata.Split(new char[] { '#' });
                //smartLog.WriteErrorMessage(inf.args);

            }

        }
        private void setFilePath(String args)
        {
            try
            {
                string[] metadata = args.Split(new char[] { ',' });

                for (int i = 1; i < metadata.Length; i = i + 2)
                {


                    inf.opcvalue.Add("0");
                    WcName.Add(metadata[0]);
                    inf.opcItemID.Add(metadata[i].ToLower());
                    inf.opcItem.Add(metadata[i + 1].ToLower());

                    //  inf.opcItem.Add(metadata[i + 1].Split(new char[] { '\\' })[1]);



                }
            }
            catch (Exception ex)
            {
                //Uitility.LogError(ex);
            }
        }


    }
}