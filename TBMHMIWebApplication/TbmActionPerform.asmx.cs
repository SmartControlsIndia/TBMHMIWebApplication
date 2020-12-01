using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
 

namespace TBMHMIWebApplication
{
    /// <summary>
    /// Summary description for TbmActionPerform
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class TbmActionPerform :  System.Web.Services.WebService
    {

        static OPCManager myopc;//= new OPCManager();

        public class TBMinfoupdate
        {
            public string CyclestartStopTBM { get; set; }
            public string ReceipeNameTBM { get; set; }
            public string ReceipeImageTBM { get; set; }
            public string okTBM { get; set; }
            public string nokTBM { get; set; }
            public string InterlockTBM { get; set; }
            
            public string messageTBM { get; set; }
            public string ErrorMessageTBM { get; set; }
            public string AutoLogoutTBM { get; set; }
            public string Dission { get; set; }
           public string ScanningAllow { get; set; }
            public string TBMcyclestart { get; set; }
            
            public string Datetimenow { get; set; }
            public string sapcode { get; set; }
        }

        public  class Receipe
        {
            public string Receipename { get; set; }
            public string logopath { get; set; }

            public string Sapcode { get; set; }

        }
        public class OemImagePath
        {
            
            public string logopath { get; set; }



        }

        public class TBMShiftWiseCount
        {
            public string Shift { get; set; }
            public string count { get; set; }
            public string status { get; set; }

        }


        //static  List<Receipe> Receipemaster = new List<Receipe>(); 
        List<Receipe> Receipemaster = new List<Receipe>();  // remove static so that all workcenters get new instance
        int recipecount;
        int callgettimeCount = 0;


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string okfunction(string HmiId, string Barcode)
        {


            SmartLogic.SmartOPC AllTagStatus;

            SmartLogic.loginformation inf;
            AllTagStatus = myopc.TBMHMI();
            inf = myopc.TBMinf();
            string ok = "";

            try
            {
                string Receipe = AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Recipe").ToLower())).ToString();
                if (Barcode.Length == 10)
                {
                    //Uitility.LogEvent("Barcode is " + Barcode.ToString() + " " + "Receipe " + Receipe, HmiId);
                    string op1 = "0", op2 = "0", op3 = "0";
                    if (Session["ManningID1"] != null)
                    { op1 = Session["ManningID1"].ToString(); }
                    if (Session["ManningID2"] != null)
                    { op2 = Session["ManningID2"].ToString(); }
                    if (Session["ManningID3"] != null)
                    { op3 = Session["ManningID3"].ToString(); }

                    int count = 0;
                    if (Session["ManningID1"] != null)
                    { count = count + 1; }
                    if (Session["ManningID2"] != null)
                    { count = count + 1; }
                    if (Session["ManningID3"] != null)
                    { count = count + 1; }

                    if (count >= 2)
                    {
                        int statusid = 1;

                        Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                        TbmreceipedatabaseObj.wcID = Session["Wcid"].ToString();
                        TbmreceipedatabaseObj.gtbarCode = Barcode;
                        TbmreceipedatabaseObj.recipeCode = Receipe;
                        TbmreceipedatabaseObj.gtWeight = "0";
                        TbmreceipedatabaseObj.manningID = op1;
                        TbmreceipedatabaseObj.manningID2 = op2;
                        TbmreceipedatabaseObj.manningID3 = op3;
                        TbmreceipedatabaseObj.defectID = "0";
                        TbmreceipedatabaseObj.StatusId = statusid;

                        string result = TbmreceipedatabaseObj.Addtbmpcr();



                        if (result.IndexOf("Successfully") != -1)
                        {
                            Session["Dission"] = "1";
                           // AllTagStatus.WriteData(0, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);
                            ok = "OK";
                          //  Session["ScanningAllow"] = "1";
                            Uitility.LogEvent(" \t Data Saved Sucessfully \t " + "Barcode " + Barcode + " Receipe " + Receipe + "Operator " + op1 + " " + op2 + " " + op3, HmiId);
                        }
                        else
                        {
                            Session["Dission"] = "0";
                           // AllTagStatus.WriteData(0, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);
                            Uitility.LogEvent(" \t Barcode All Ready exist \t " + "Barcode " + Barcode + " Receipe " + Receipe + "Operator " + op1 + " " + op2 + " " + op3 + " " + result, HmiId);
                         
                          //  Session["ScanningAllow"] = "1";
                            ok = "Nok";
                        }
                    }
                    else
                    {
                        ok = "Logout";
                        Uitility.LogEvent("Two Operator Are Not Login ", HmiId);
                    }

                }
            }
            catch (Exception ex)
            {
                Uitility.LogError(ex, HmiId);
            }
            var jsonSerialiser = new JavaScriptSerializer();
         
            
            var json = jsonSerialiser.Serialize(ok);
            return json;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string nokfunction(string HmiId,string Barcode)
        {
            SmartLogic.SmartOPC AllTagStatus;

            SmartLogic.loginformation inf;
            AllTagStatus = myopc.TBMHMI();
            inf = myopc.TBMinf();

            string ok = "";

            try
            {
                string Receipe = AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Recipe").ToLower())).ToString();

                //AllTagStatus.WriteData(1, inf.opcItemID[inf.opcItemID.IndexOf(HmiId + "_Nok")]);

                //AllTagStatus.WriteData(1, inf.opcItemID[1]);
                //AllTagStatus.WriteData(1, inf.opcItemID[3]);

                if (Barcode.Length == 10)
                {
                    //Uitility.LogEvent("Barcode is " + Barcode.ToString() + " " + "Receipe " + Receipe, HmiId);
                    string op1 = "0", op2 = "0", op3 = "0";
                    if (Session["ManningID1"] != null)
                    { op1 = Session["ManningID1"].ToString(); }
                    if (Session["ManningID2"] != null)
                    { op2 = Session["ManningID2"].ToString(); }
                    if (Session["ManningID3"] != null)
                    { op3 = Session["ManningID3"].ToString(); }

                    int count = 0;
                    if (Session["ManningID1"] != null)
                    { count = count + 1; }
                    if (Session["ManningID2"] != null)
                    { count = count + 1; }
                    if (Session["ManningID3"] != null)
                    { count = count + 1; }

                    if (count >= 2)
                    {
                        int statusid = 2;

                        Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                        TbmreceipedatabaseObj.wcID = Session["Wcid"].ToString(); ;
                        TbmreceipedatabaseObj.gtbarCode = Barcode;
                        TbmreceipedatabaseObj.recipeCode = Receipe;
                        TbmreceipedatabaseObj.gtWeight = "0";
                        TbmreceipedatabaseObj.manningID = op1;
                        TbmreceipedatabaseObj.manningID2 = op2;
                        TbmreceipedatabaseObj.manningID3 = op3;
                        TbmreceipedatabaseObj.defectID = "0";
                        TbmreceipedatabaseObj.StatusId = statusid;

                        string result = TbmreceipedatabaseObj.Addtbmpcr();
                        if (result.IndexOf("Successfully") != -1)
                        {
                            Session["Dission"] = "1";
                           // AllTagStatus.WriteData(0, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);
                            ok = "OK";
                          // Session["ScanningAllow"] = "1";
                            Uitility.LogEvent(" \t Data Saved Sucessfully \t " + "Barcode " + Barcode + " Receipe " + Receipe + "Operator " + op1 + " " + op2 + " " + op3, HmiId);
                        }
                        else
                        {
                            Session["Dission"] = "0";
                            //AllTagStatus.WriteData(0, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);
                            Uitility.LogEvent(" \t Barcode All Ready exist \t " + "Barcode " + Barcode + " Receipe " + Receipe + "Operator " + op1 + " " + op2 + " " + op3 + " " + result, HmiId);
                         //   Session["ScanningAllow"] = "1";
                           
                            ok = "Nok";
                        }
                    }
                    else
                    {
                        ok = "Logout";
                        Uitility.LogEvent("Two Operator Are Not Login ", HmiId);
                    }


                }


            }
            catch (Exception ex)
            {

                Uitility.LogError(ex, HmiId);
            }
            var jsonSerialiser = new JavaScriptSerializer();

           
            var json = jsonSerialiser.Serialize(ok);
            return json;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string InterLockEnable(string HmiId, string Barcode)
        {
            SmartLogic.SmartOPC AllTagStatus;

            SmartLogic.loginformation inf;
            AllTagStatus = myopc.TBMHMI();
            inf = myopc.TBMinf();

            string ok = "";

            try
            {
               


                if (Barcode.Length == 10)
                {
                    Tbmreceipedatabase TbmCheckBarcodeObj = new Tbmreceipedatabase();
                    TbmCheckBarcodeObj.gtbarCode = Barcode;
                    DataTable dt = new DataTable();
                    dt = TbmCheckBarcodeObj.CheckBarcode();
                    if (dt.Rows.Count > 0)
                    {
                         //Session["ScanningAllow"] = "1";
                        Session["Dission"] = "0";
                        ok = "Nok";
                        AllTagStatus.WriteData(0, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);
                    }
                    else
                    {
                        Session["Dission"] = "1";
                        ok = "ok";
                        Session["ScanningAllow"] = "1";
                        Session["cyclestartCondition"] = "0";
                        AllTagStatus.WriteData(1, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);
                    }

                }
                else

                {
                    
                }





            }
            catch (Exception ex)
            {

                Uitility.LogError(ex, HmiId);
            }
            var jsonSerialiser = new JavaScriptSerializer();


            var json = jsonSerialiser.Serialize(ok);
            return json;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTime(string HmiId,string Barcode)
        {
            Uitility.LogEvent("\t GetTimecalled\t", HmiId+""+Barcode);
            List<TBMinfoupdate> Tagreturn = new List<TBMinfoupdate>();
            SmartLogic.SmartOPC AllTagStatus;
            SmartLogic.loginformation inf;
            if (HttpContext.Current.Session == null) // send response for logout when no session available
            {
                Tagreturn.Add(new TBMinfoupdate
                {
                    CyclestartStopTBM = "",
                    ReceipeNameTBM = "",
                    ReceipeImageTBM = "",
                    okTBM = "",
                    nokTBM = "",
                    InterlockTBM = "",
                    messageTBM = "no session available",
                    ErrorMessageTBM = "001",
                    AutoLogoutTBM = "AU1000",
                    Dission = "",
                    ScanningAllow = "",
                    Datetimenow = DateTime.Now.ToString("dd/MMM/yyyy HH:MM:sss"),
                    sapcode = ""
                });
            }
            else
            {
                if (HmiId!= "")
                {
                    Uitility.LogEvent("\t checking OPC Connection\t", HmiId);
                    if (myopc == null || myopc.OPCStatus() != 1)
                    {

                        Uitility.LogEvent("\t ConfirmToPLC OPC Connection Starting \t", HmiId);
                        try
                        {
                            if (myopc != null)
                            {
                                myopc.stoptopc();
                                Uitility.LogEvent("\t ConfirmToPLC OPC Connection stopped \t", HmiId);
                            }


                            myopc = new OPCManager();

                            System.Threading.Thread.Sleep(3000);
                            // myopc.startopc();


                            Uitility.LogEvent("\t Kepware Connection build Sucessfully \t", HmiId);


                            Tagreturn.Add(new TBMinfoupdate
                            {
                                CyclestartStopTBM = "",
                                ReceipeNameTBM = "",
                                ReceipeImageTBM = "",
                                okTBM = "",
                                nokTBM = "",
                                InterlockTBM = "",
                                messageTBM = " Kebware Connection done",
                                ErrorMessageTBM = "001",
                                AutoLogoutTBM = "",
                                Dission = "",
                                ScanningAllow = "",
                                Datetimenow = DateTime.Now.ToString("dd/MMM/yyyy HH:MM:sss"),
                                sapcode = ""
                            });
                        }
                        catch (Exception ex)
                        {
                            Uitility.LogError(ex, HmiId);
                        }


                    }
                }
                    
                else
                {
                    int count1 = 0;

                    try
                    {
                        if (Session["ManningID1"] != null)
                        {
                            count1 = count1 + 1;
                        }
                        if (Session["ManningID2"] != null)
                        {
                            count1 = count1 + 1;
                        }
                        if (Session["ManningID3"] != null)
                        {
                            count1 = count1 + 1;
                        }
                    }
                    catch(Exception ex)
                    {
                        Uitility.LogEvent("\t count1 fire error 2 login not catching \t", HmiId);
                    }
                   

                    if (count1 >= 2)
                    {
                        try
                        {
                            AllTagStatus = myopc.TBMHMI();
                            inf = myopc.TBMinf();
                            string CycleStartStop = "", ok = "", Receipe = "", Interlock = "", nok = "";
                            if (AllTagStatus != null && inf != null)
                            {
                                if (AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_CycleStart").ToLower())) != null)
                                {
                                    CycleStartStop = AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_CycleStart").ToLower())).ToString();
                                }
                                else
                                {
                                    Uitility.LogEvent("\t CycleStartStop Tag Not Communicate  \t", HmiId);
                                }
                                if (AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Recipe").ToLower())) != null)
                                {
                                    Receipe = AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Recipe").ToLower())).ToString();
                                }
                                else
                                { Uitility.LogEvent("\t Receipe Tag Not Communicate  \t", HmiId); }
                                if (AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Ok").ToLower())) != null)
                                {
                                    ok = AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Ok").ToLower())).ToString();
                                }
                                else
                                { Uitility.LogEvent("\t OK Tag Not Communicating  \t", HmiId); }
                                if (AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Nok").ToLower())) != null)
                                {
                                    nok = AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Nok").ToLower())).ToString();
                                }
                                else
                                {
                                    Uitility.LogEvent("\t Not Ok Tag Not Communicating  \t", HmiId);
                                }
                                if (AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())) != null)
                                {
                                    Interlock = AllTagStatus.opcValue.GetValue(inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())).ToString();
                                }
                                else
                                {

                                    Uitility.LogEvent("\t Interlock Tag Not Communicating  \t", HmiId);
                                }
                            }

                                /// Finding OEM Logo Detection Condition
                                /// 
                                string receipeImage = "Logo/logo.png";
                                string saprecipecode = "";
                                string message = "";
                                try
                                {
                                    if (Receipemaster.Count == 0 || Receipemaster.Count < recipecount)
                                    {
                                        Receipemaster.Clear();
                                    Uitility.LogEvent("\t recipecount \t", HmiId);
                                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                                        TbmreceipedatabaseObj.ReceipeName = Receipe;

                                        DataTable dt = new DataTable();
                                        DataSet ds = new DataSet();

                                        try
                                        {
                                            dt = TbmreceipedatabaseObj.getoem();
                                        }
                                        catch (Exception ex)
                                        {
                                            Uitility.LogEvent("\t Error  \t" + ex.Message, HmiId);
                                        }

                                        ds.Tables.Add(dt);

                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {
                                            Receipemaster.Add(new Receipe { Receipename = dr["name"].ToString(), logopath = dr["logoName"].ToString(), Sapcode = dr["description"].ToString() });

                                        }

                                    }

                                    // error line 437


                                    var receipeImage2 = Receipemaster.Find(x => x.Receipename == Receipe);

                                    if (receipeImage2 == null)
                                    {
                                        if (Receipe != "" || Receipe.Contains("String"))
                                        {
                                            try
                                            {
                                                Tbmreceipedatabase objreceipe = new Tbmreceipedatabase();
                                                objreceipe.wcname = HmiId;
                                                DataTable dttagreceipe = objreceipe.selecttagreceipe();

                                                if (dttagreceipe.Rows.Count > 0)
                                                {
                                                    AllTagStatus.WriteData(dttagreceipe.Rows[0][0], inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Recipe").ToLower())]);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Uitility.LogEvent("\t Error Finding OEM Logo Detection Condition  \t" + ex.Message, HmiId);
                                            }
                                        }

                                        Receipemaster.Clear();
                                        receipeImage = "Logo/logo.png";
                                        saprecipecode = "No Sap Code";

                                    }
                                    else
                                    {
                                        receipeImage = receipeImage2.logopath.ToString();
                                        saprecipecode = receipeImage2.Sapcode.ToString();
                                    }
                                }
                                catch (Exception ex)
                                {
                                  Uitility.LogEvent("\t Err  \t" + ex.Message, HmiId);
                                }                            /// End Recipe Detection Condition
                                DateTime CT = DateTime.Now;
                                // DateTime CTcom = DateTime.Now;
                                string AutoLogout = "";
                                string ErrorCode = "001";

                                if (CT < Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy") + " 15:00:15")
                                    && CT > Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy") + " 15:00:00")
                                    || CT < Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy") + " 23:00:15")
                                    && CT > Convert.ToDateTime("23:00:00")
                                    || CT < Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy") + " 07:00:15")
                                    && CT > Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy") + " 07:00:00"))
                                {
                                    if (Barcode.Length == 10)
                                    {
                                        savebarcode(HmiId, Barcode, Receipe);
                                    }
                                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();

                                    if (Session["UserId1"] != null)
                                    {

                                        TbmreceipedatabaseObj.UserId = Session["UserId1"].ToString();
                                        string dtuserdetail = TbmreceipedatabaseObj.Logoutuser();
                                        Uitility.LogEvent("\t Auto logout done Successfully\t" + Session["UserId1"].ToString(), HmiId);
                                        Session.Remove("OP1");
                                        Session.Remove("ManningID1");
                                        Session.Remove("OP1img");
                                        Session.Remove("UserId1");
                                    }
                                    if (Session["UserId2"] != null)
                                    {

                                        TbmreceipedatabaseObj = new Tbmreceipedatabase();
                                        TbmreceipedatabaseObj.UserId = Session["UserId2"].ToString();
                                        string dtuserdetail = TbmreceipedatabaseObj.Logoutuser();
                                        Uitility.LogEvent("\t Auto logout done  Successfully\t" + Session["UserId2"].ToString(), HmiId);
                                        Session.Remove("OP2");
                                        Session.Remove("ManningID2");
                                        Session.Remove("OP2img");
                                        Session.Remove("UserId2");
                                        Session.RemoveAll();
                                        Session.Abandon();

                                    }
                                    if (Session["UserId3"] != null)
                                    {
                                        TbmreceipedatabaseObj = new Tbmreceipedatabase();
                                        TbmreceipedatabaseObj.UserId = Session["UserId3"].ToString();
                                        string dtuserdetail = TbmreceipedatabaseObj.Logoutuser();
                                        Uitility.LogEvent("\t Auto logout done Successfully \t" + Session["UserId3"].ToString(), HmiId);

                                        Session.Remove("OP3");
                                        Session.Remove("ManningID3");
                                        Session.Remove("OP3img");
                                        Session.Remove("UserId3");
                                    }
                                    AutoLogout = "AU1000";
                                }

                                /// Detecting Cycle Stop Condition
                                if (CycleStartStop == "1" || CycleStartStop == "True")
                                {
                                    Session["CycleStartStopTrue"] = CycleStartStop;

                                }
                                if ((CycleStartStop == "0" || CycleStartStop == "False") && Session["CycleStartStopTrue"] != null)
                                {
                                    Session.Remove("CycleStartStopTrue");
                                    if (Barcode.Length == 10)
                                    {
                                        Session["ScanningAllow"] = "1";
                                    }
                                    else
                                    { Session["ScanningAllow"] = "0"; }
                                    Session["cyclestartCondition"] = "2";
                                    AllTagStatus.WriteData(0, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);
                                    Uitility.LogEvent("\t Cycle Stop  done Successfully\t", HmiId);

                                }
                                ///End Cycle Stop Detect Condition
                                ///
                                /// Detecting Cycle Start Condition
                                if (CycleStartStop == "0" || CycleStartStop == "False")
                                {
                                    Session["CycleStartStopFalse"] = CycleStartStop;

                                }

                                if ((CycleStartStop == "1" || CycleStartStop == "True") && Session["CycleStartStopFalse"] != null)
                                {
                                    Session.Remove("CycleStartStopFalse");
                                    Session["cyclestart"] = "1";
                                    Session["cyclestartCondition"] = "1";

                                    Uitility.LogEvent("\t Cycle Start done  Successfully \t", HmiId);

                                }
                                /// end Detecting Cycle Start Condition


                                ///Detecting New Barcode Scan
                                if (Session["barcode"] != null)
                                {
                                    if (Session["barcode"].ToString() != Barcode && Barcode != "")
                                    {
                                        if (Barcode.Length == 10)
                                        {
                                            Uitility.LogEvent("\t Barcode Scanned Detected \t" + Barcode, HmiId);
                                            Session.Remove("barcode");
                                        }
                                    }
                                }
                                else if (Barcode != "")
                                {
                                    Session["barcode"] = Barcode;
                                }
                                ///End Detecting New Barcode Scan


                                ///Detecting Pushbutton input
                                if (Session["cyclestart"] != null)
                                {
                                    if ((ok == "0" || ok == "False") && (nok == "0" || nok == "False"))
                                    {
                                        Session["ok"] = ok;
                                        Session["oksave"] = "0";

                                    }

                                    if ((ok == "1" || ok == "True") && Session["ok"] != null)
                                    {
                                        Session["oksave"] = "1";
                                        Session.Remove("ok");
                                        Session.Remove("cyclestart");
                                    }
                                    else if ((nok == "1" || nok == "True") && Session["ok"] != null)
                                    {
                                        Session["oksave"] = "1";
                                        Session.Remove("ok");
                                        Session.Remove("cyclestart");
                                    }
                                }
                                /// End Detecting Pushbutton input



                                if (Session["oksave"] != null)
                                {
                                    if (Session["oksave"].ToString() == "1")
                                    {
                                        if (ok == "1" || nok == "1" || ok == "True" || nok == "True")
                                        {

                                            if (Barcode.Length == 10)
                                            {

                                                string op1 = "0", op2 = "0", op3 = "0";
                                                if (Session["ManningID1"] != null)
                                                { op1 = Session["ManningID1"].ToString(); }
                                                if (Session["ManningID2"] != null)
                                                { op2 = Session["ManningID2"].ToString(); }
                                                if (Session["ManningID3"] != null)
                                                { op3 = Session["ManningID3"].ToString(); }

                                                int count = 0;
                                                if (Session["ManningID1"] != null)
                                                { count = count + 1; }
                                                if (Session["ManningID2"] != null)
                                                { count = count + 1; }
                                                if (Session["ManningID3"] != null)
                                                { count = count + 1; }

                                                if (count >= 2)
                                                {
                                                    int statusid = 0;
                                                    if (ok == "1" || ok == "True")
                                                    {
                                                        statusid = 1;
                                                        Uitility.LogEvent("\t Push button Pressed Successfully\t" + "OK", HmiId);
                                                    }
                                                    else if (nok == "1" || nok == "True")
                                                    {
                                                        statusid = 2;
                                                        Uitility.LogEvent("\t Push button Pressed Successfully \t" + "NOK", HmiId);
                                                    }



                                                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                                                    TbmreceipedatabaseObj.wcID = Session["Wcid"].ToString();
                                                    TbmreceipedatabaseObj.gtbarCode = Barcode;
                                                    TbmreceipedatabaseObj.recipeCode = Receipe;
                                                    TbmreceipedatabaseObj.gtWeight = "0";
                                                    TbmreceipedatabaseObj.manningID = op1;
                                                    TbmreceipedatabaseObj.manningID2 = op2;
                                                    TbmreceipedatabaseObj.manningID3 = op3;
                                                    TbmreceipedatabaseObj.defectID = "0";
                                                    TbmreceipedatabaseObj.StatusId = statusid;

                                                    string result = TbmreceipedatabaseObj.Addtbmpcr();
                                                    if (result.IndexOf("Successfully") != -1)
                                                    {
                                                        Session["oksave"] = "0";
                                                        Session["Dission"] = "1";
                                                        //AllTagStatus.WriteData(0, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);
                                                        // Session["ScanningAllow"] = "1";
                                                        message = result;

                                                        Uitility.LogEvent(" \t Data Saved Sucessfully \t " + "Barcode " + Barcode + " Receipe " + Receipe + "Operator " + op1 + " " + op2 + " " + op3, HmiId);
                                                    }
                                                    else
                                                    {
                                                        Uitility.LogEvent(" \t Barcode AlReady exist \t " + "Barcode " + Barcode + " Receipe " + Receipe + "Operator " + op1 + " " + op2 + " " + op3 + " " + result, HmiId);
                                                        //  Session["ScanningAllow"] = "1";
                                                        Session["Dission"] = "0";
                                                        //  AllTagStatus.WriteData(0, inf.opcItemID[inf.opcItemID.IndexOf((HmiId + "_Interlock").ToLower())]);


                                                        message = result;
                                                    }
                                                }
                                                else
                                                {
                                                    Uitility.LogEvent("Two Operator Are Not Login ", HmiId);
                                                    ErrorCode = "101";
                                                }


                                            }
                                        }

                                    }
                                }


                                if (Session["Dission"] == null)
                                {
                                    Session["Dission"] = "1";

                                }
                                if (Session["cyclestartCondition"] == null)
                                {
                                    Session["cyclestartCondition"] = "0";

                                }

                                if (Session["ScanningAllow"] == null)
                                { Session["ScanningAllow"] = ""; }

                                if ((Session["ScanningAllow"].ToString() == "0" || Session["ScanningAllow"].ToString() == "False") && (Session["Dission"].ToString() == "1"))
                                {
                                    // if ((CycleStartStop == "1" || CycleStartStop == "True"))
                                    // {
                                    //     Session["ScanningAllow"] = "1";

                                    // }
                                    //else if(Barcode.Length==10)
                                    // {
                                    //     Session["ScanningAllow"] = "1";
                                    // }
                                    // else
                                    // {
                                    Session["ScanningAllow"] = "0";
                                    //}

                                }

                                Tagreturn.Add(new TBMinfoupdate
                                {
                                    CyclestartStopTBM = CycleStartStop,
                                    ReceipeNameTBM = Receipe,
                                    ReceipeImageTBM = receipeImage,
                                    okTBM = ok,
                                    nokTBM = nok,
                                    InterlockTBM = Interlock,
                                    messageTBM = message,
                                    ErrorMessageTBM = ErrorCode,
                                    AutoLogoutTBM = AutoLogout,
                                    Dission = Session["Dission"].ToString(),
                                    ScanningAllow = Session["ScanningAllow"].ToString(),
                                    TBMcyclestart = Session["cyclestartCondition"].ToString(),
                                    Datetimenow = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:sss"),
                                    sapcode = saprecipecode
                                });
                            
                        }
                        catch (Exception ex)
                        {
                            string error = ex.ToString();

                            if (error.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                            {
                                error = "Data Base not in Network";

                            }
                            Uitility.LogError(ex, HmiId);
                            Tagreturn.Add(new TBMinfoupdate
                            {
                                CyclestartStopTBM = "",
                                ReceipeNameTBM = "",
                                ReceipeImageTBM = "",
                                okTBM = "",
                                nokTBM = "",
                                InterlockTBM = "",
                                messageTBM = "",
                                ErrorMessageTBM = "401",
                                AutoLogoutTBM = "",
                                Dission = "",
                                ScanningAllow = "",
                                Datetimenow = DateTime.Now.ToString("dd/MMM/yyyy HH:MM:sss"),
                                sapcode = ""
                            });
                        }
                    }
                    else
                    {
                        Uitility.LogEvent("Two Operator Are Not Login auto logout perform", HmiId);
                        Tagreturn.Add(new TBMinfoupdate
                        {
                            CyclestartStopTBM = "",
                            ReceipeNameTBM = "",
                            ReceipeImageTBM = "",
                            okTBM = "",
                            nokTBM = "",
                            InterlockTBM = "",
                            messageTBM = "",
                            ErrorMessageTBM = "",
                            AutoLogoutTBM = "AU1000",
                            Dission = "",
                            ScanningAllow = "",
                            Datetimenow = DateTime.Now.ToString("dd/MMM/yyyy HH:MM:sss"),
                            sapcode = ""
                        });
                    }
                }
            }
            
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Tagreturn);
            return json;
            
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Logout(string UserId ,string Password,string TBMName)
        {
            Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
            TbmreceipedatabaseObj.UserId = UserId;
            TbmreceipedatabaseObj.Password = Password;
            TbmreceipedatabaseObj.wcname = TBMName;
            DataTable   dtuserdetail = TbmreceipedatabaseObj.selectuserdetailLogOut();
            if (dtuserdetail.Rows.Count > 0)
            {
                if (Session["UserId1"] != null)
                {
                    if (UserId == Session["UserId1"].ToString())
                    {
                        Uitility.LogEvent("Operator 1 Logout Successfully" + Session["UserId1"].ToString(), TBMName);
                        Session.Remove("OP1");
                        Session.Remove("ManningID1");
                        Session.Remove("OP1img");
                        Session.Remove("UserId1");

                    }
                }
                if (Session["UserId2"] != null)
                {
                    if (UserId == Session["UserId2"].ToString())
                    {
                        Uitility.LogEvent("Operator 2 Logout Successfully " + Session["UserId2"].ToString(), TBMName);
                        Session.Remove("OP2");
                        Session.Remove("ManningID2");
                        Session.Remove("OP2img");
                        Session.Remove("UserId2");
                    }
                }
                if (Session["UserId3"] != null)
                {
                    if (UserId == Session["UserId3"].ToString())
                    {
                        Uitility.LogEvent("Operator 3 Logout Successfully" + Session["UserId3"].ToString(), TBMName);
                        Session.Remove("OP3");
                        Session.Remove("ManningID3");
                        Session.Remove("OP3img");
                        Session.Remove("UserId3");
                    }
                }
            }
            var jsonSerialiser = new JavaScriptSerializer();

            string Logout = "Logout";
            var json = jsonSerialiser.Serialize(Logout);
            return json;
        }

      
        public void savebarcode(string HmiId,string Barcode, string Receipe)
        {
            string op1 = "0", op2 = "0", op3 = "0";
            if (Session["ManningID1"] != null)
            { op1 = Session["ManningID1"].ToString(); }
            if (Session["ManningID2"] != null)
            { op2 = Session["ManningID2"].ToString(); }
            if (Session["ManningID3"] != null)
            { op3 = Session["ManningID3"].ToString(); }

            int count = 0;
            if (Session["ManningID1"] != null)
            { count = count + 1; }
            if (Session["ManningID2"] != null)
            { count = count + 1; }
            if (Session["ManningID3"] != null)
            { count = count + 1; }

            if (count >= 2)
            {
                int statusid = 0;
                
                    statusid = 1;
                    Uitility.LogEvent("\t Default Ok Due to AutoLogout done Successfully\t" + "OK", HmiId);
                
                Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                TbmreceipedatabaseObj.wcID = Session["Wcid"].ToString();
                TbmreceipedatabaseObj.gtbarCode = Barcode;
                TbmreceipedatabaseObj.recipeCode = Receipe;
                TbmreceipedatabaseObj.gtWeight = "0";
                TbmreceipedatabaseObj.manningID = op1;
                TbmreceipedatabaseObj.manningID2 = op2;
                TbmreceipedatabaseObj.manningID3 = op3;
                TbmreceipedatabaseObj.defectID = "0";
                TbmreceipedatabaseObj.StatusId = statusid;

                string result = TbmreceipedatabaseObj.Addtbmpcr();
                if (result.IndexOf("Successfully") != -1)
                {
                    Session["oksave"] = "0";
                    Session["Dission"] = "1";
                   
                 

                    Uitility.LogEvent(" \t Default   Data Saved Sucessfully  Due to AutoLogout \t " + "Barcode " + Barcode + " Receipe " + Receipe + "Operator " + op1 + " " + op2 + " " + op3, HmiId);
                }
                else
                {
                    Uitility.LogEvent(" \t Default Barcode All Ready exist Due to AutoLogout \t " + "Barcode " + Barcode + " Receipe " + Receipe + "Operator " + op1 + " " + op2 + " " + op3 + " " + result, HmiId);
                   
                    Session["Dission"] = "0";
                    

                    
                }
            }
            else
            {
                Uitility.LogEvent("Two Operator Are Not Login ", HmiId);
                
            }

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Login(string UserId, string Password, string TBMName)
        {
            DataTable dtuserdetail = new DataTable();

            if (Session["OP1"] == null)
            {
                Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                TbmreceipedatabaseObj.UserId = UserId;
                TbmreceipedatabaseObj.Password = Password;
                TbmreceipedatabaseObj.wcname = TBMName;
                dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                if (dtuserdetail.Rows.Count > 0)
                {
                    string hmid = dtuserdetail.Rows[0]["WcName"].ToString();

                    if (hmid == "0" || hmid == TBMName)
                    {
                        
                        Session["OP1"] = dtuserdetail.Rows[0]["firstName"].ToString();
                        Session["UserId1"] = dtuserdetail.Rows[0]["UserId"].ToString();
                        Session["ManningID1"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                        Session["OP1img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();

                        string a = Session["OP1"].ToString();
                        string b = "";
                        if (Session["OP2"] != null)
                        {
                            b = Session["OP2"].ToString();
                        }
                        string c = "";
                        if (Session["OP3"] != null)
                        {
                            c = Session["OP3"].ToString();
                        }



                        if (a == b && a == c)
                        {
                            Session.Remove("OP1");
                            Session.Remove("ManningID1");
                            Session.Remove("OP1img");
                            Session.Remove("UserId1");
                        }
                       
                        
                    }
                    else
                    {
                        //lblinformation.Text = hmid;
                    }
                }


            }
            else if (Session["OP2"] == null && (UserId ?? "").ToLower() != (Session["UserId1"].ToString() ?? "").ToLower() )
            {
                Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                TbmreceipedatabaseObj.UserId = UserId;
                TbmreceipedatabaseObj.Password = Password;
                TbmreceipedatabaseObj.wcname = TBMName;
                dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                if (dtuserdetail.Rows.Count > 0)
                {
                    string hmid = dtuserdetail.Rows[0]["WcName"].ToString();

                    if (hmid == "0" || hmid == TBMName)
                    {
                         
                        Session["OP2"] = dtuserdetail.Rows[0]["firstName"].ToString();
                        Session["ManningID2"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                        Session["OP2img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();
                        Session["UserId2"] = dtuserdetail.Rows[0]["UserId"].ToString();


                       
                        string a = Session["OP1"].ToString();
                        string b = "";
                        if (Session["OP2"] != null)
                        {
                            b = Session["OP2"].ToString();
                        }
                        string c = "";
                        if (Session["OP3"] != null)
                        {
                            c = Session["OP3"].ToString();
                        }




                        if (b == a && b == c)
                        {


                            Session.Remove("OP2");
                            Session.Remove("ManningID2");
                            Session.Remove("OP2img");
                            Session.Remove("UserId2");
                        }
                         
                    }
                    else
                    {
                      //  lblinformation.Text = hmid;
                    }

                }
            }
            else if (Session["OP3"] == null && (UserId ?? "").ToLower() != (Session["UserId1"].ToString() ?? "").ToLower() && (UserId ?? "").ToLower() != (Session["UserId2"].ToString() ?? "").ToLower())
            {
                Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                TbmreceipedatabaseObj.UserId = UserId;
                TbmreceipedatabaseObj.Password = Password;
                TbmreceipedatabaseObj.wcname = TBMName;
                dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                if (dtuserdetail.Rows.Count > 0)
                {
                    string hmid = dtuserdetail.Rows[0]["WcName"].ToString();

                    if (hmid == "0" || hmid == TBMName)
                    {
                        
                        Session["OP3"] = dtuserdetail.Rows[0]["firstName"].ToString();
                        Session["ManningID3"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                        Session["OP3img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();
                        Session["UserId3"] = dtuserdetail.Rows[0]["UserId"].ToString();
                        string a = Session["OP1"].ToString();
                        string b = "";
                        if (Session["OP2"] != null)
                        {
                            b = Session["OP2"].ToString();
                        }
                        string c = "";
                        if (Session["OP3"] != null)
                        {
                            c = Session["OP3"].ToString();
                        }


                        if (c == b && c == a)
                        {


                            Session.Remove("OP3");
                            Session.Remove("ManningID3");
                            Session.Remove("OP3img");
                            Session.Remove("UserId3");
                        }
                        
                    }
                    else
                    {
                       // lblinformation.Text = hmid;
                    }

                }
            }
            var jsonSerialiser = new JavaScriptSerializer();

            string Logout = "Login";
            var json = jsonSerialiser.Serialize(Logout);
            return json;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ChangePassword(string UserId, string Password,string NewPassword, string TBMName)
        {
            DataTable dtuserdetail = new DataTable();
            string Logout = "";

            Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                TbmreceipedatabaseObj.UserId = UserId;
                TbmreceipedatabaseObj.Password = Password;
            TbmreceipedatabaseObj.NewPassword = NewPassword;
            TbmreceipedatabaseObj.wcname = TBMName;
            Logout = TbmreceipedatabaseObj.AddUpdateChangePassword();
            
            var jsonSerialiser = new JavaScriptSerializer();

           
            var json = jsonSerialiser.Serialize(Logout);
            return json;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getHMIDisplayInfo()
        {
          Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
          string dtuserdetail = TbmreceipedatabaseObj.selectHMIDisplayInfo();
          var jsonSerialiser = new JavaScriptSerializer();
          var json = jsonSerialiser.Serialize(dtuserdetail);
          return json;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getHMIDownTimeStart(string Hmiid)
        {
            Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
            TbmreceipedatabaseObj.wcname = Hmiid;
            string dtuserdetail = TbmreceipedatabaseObj.getHMIDownTimeStart();
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(dtuserdetail);
            return json;
        }
        

      [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetShiftWiseCount(string wcname)
        {
            DataTable dt = new DataTable();
            try
            {

                Tbmreceipedatabase objTbmreceipedatabase = new Tbmreceipedatabase();
                objTbmreceipedatabase.wcname = wcname.ToString();
                dt = objTbmreceipedatabase.sp_TBMGetShiftWiseCount();
                

            }
            catch (Exception exp)
            {
                
            }
            
            List<TBMShiftWiseCount> obj = new List<TBMShiftWiseCount>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj.Add(new TBMShiftWiseCount
                {
                    Shift = dt.Rows[i][0].ToString(),

                    count = dt.Rows[i][1].ToString(),
                    status = dt.Rows[i][2].ToString(),


                });
            }

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(obj);
            return json;



        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OemImagePathFunction()
        {
            List<OemImagePath> obj = new List<OemImagePath>();

            DirectoryInfo d = new DirectoryInfo(Server.MapPath(@"~/OEMLogo/"));//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*"); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                obj.Add(new OemImagePath
                {
                    logopath = file.Name
                });
               // str = str + ", " + file.Name;
            }

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    obj.Add(new TBMShiftWiseCount
            //    {
            //        Shift = dt.Rows[i][0].ToString(),

            //        count = dt.Rows[i][1].ToString(),
            //        status = dt.Rows[i][2].ToString(),


            //    });
            //}
            var jsonSerialiser = new JavaScriptSerializer();

           // string Logout = "Logout";
            var json = jsonSerialiser.Serialize(obj);
            return json;
        }

        [WebMethod]
        public List<Dropdown> LoadselectselectSimulationWcName()
        {
            List<Dropdown> WcNamereturn = new List<Dropdown>();
            SimulationTagWc objSimulationTagWc = new SimulationTagWc();
            DataTable dt = objSimulationTagWc.selectselectSimulationWcNameName();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        WcNamereturn.Add(new Dropdown()
                        {
                            id = Convert.ToInt32(dt.Rows[i]["id"].ToString()),
                            Name = dt.Rows[i]["Name"].ToString()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
               // Uitility.LogError(ex);
                throw ex;
            }
            return WcNamereturn;
        }
     
        [WebMethod]
        public List<Dropdown> LoadReceipe()
        {
            List<Dropdown> ReceipeNamereturn = new List<Dropdown>();
            SimulationTagWc objSimulationTagWc = new SimulationTagWc();
            DataTable dt = objSimulationTagWc.selectReceipeName();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ReceipeNamereturn.Add(new Dropdown()
                        {
                            id = Convert.ToInt32(dt.Rows[i]["id"].ToString()),
                            Name = dt.Rows[i]["Name"].ToString()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                //Uitility.LogError(ex);
                throw ex;
            }
            recipecount = ReceipeNamereturn.Count;
            return ReceipeNamereturn;
        }

        [WebMethod]
        public List<Dropdown> LoadReasion()
        {
            List<Dropdown> ReceipeNamereturn = new List<Dropdown>();
            Tbmreceipedatabase objSimulationTagWc = new Tbmreceipedatabase();
            DataTable dt = objSimulationTagWc.selectReasion();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ReceipeNamereturn.Add(new Dropdown()
                        {
                            id = Convert.ToInt32(dt.Rows[i]["id"].ToString()),
                            Name = dt.Rows[i]["Name"].ToString()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                //Uitility.LogError(ex);
                throw ex;
            }
            return ReceipeNamereturn;
        }

        public class Dropdown
        {
            public int id { get; set; }
            public string Name { get; set; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateTag(string ReceipeName, string Wcid, string WcName)
        {

            SmartLogic.SmartOPC AllTagStatus;
            SmartLogic.loginformation inf;
            string ok = "";
            try
            {
                if (Session["UserId1"] != null)
                {
                    AllTagStatus = myopc.TBMHMI();
                    inf = myopc.TBMinf();
                    SimulationTagWc objreceipe = new SimulationTagWc();
                    objreceipe.ReceipeName = ReceipeName;
                    objreceipe.Wcid = Wcid;
                    objreceipe.UpdateReceipe();
                    AllTagStatus.WriteData(ReceipeName, inf.opcItemID[inf.opcItemID.IndexOf((WcName + "_Recipe").ToLower())]);
                    //Uitility.LogEvent("\t Update Receipe \t" + "User Id \t" + Session["UserId"].ToString() + "#" + "Update Receipe  on Machine #" + WcName + "Receipe#" + ReceipeName);
                    ok = "Update";
                }
                else
                {
                    ok = "Logout";
                }

            }
            catch (Exception ex)
            {
                string error = ex.ToString();

                if (error.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    error = "Data Base not in Network";
                }
                //Uitility.LogError(ex);

            }

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(ok);
            return json;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddReceipe(string ReceipeName)
        {


            string ok = "";
            try
            {
                if (ReceipeName.Length > 0)
                {
                    SimulationTagWc objreceipe = new SimulationTagWc();
                    objreceipe.ReceipeName = ReceipeName;
                    objreceipe.AddRecipe();
                   // Uitility.LogEvent("\t Add New Receipe \t" + Session["UserId"].ToString() + "#" + "Add New Recipe #" + ReceipeName);
                    ok = "OK";
                    //bindReceipeMachine();

                }

            }
            catch (Exception ex)
            {
                string error = ex.ToString();

                if (error.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    error = "Data Base not in Network";
                }
               // Uitility.LogError(ex);
            }

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(ok);
            return json;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string LoadLastTyre(string wcname)
        {


            Int64 ok = 0;
            try
            {
               
                    Tbmreceipedatabase obj = new Tbmreceipedatabase();
                obj.wcname = wcname;
             DataTable  dt=   obj.selecttbmpcr();
                  if(dt.Rows.Count>0)
                {
                    
                    DateTime d1 = DateTime.Now;
                    DateTime d2 = Convert.ToDateTime(dt.Rows[0]["dtandtime"].ToString());

                    TimeSpan t = d1 - d2;
                    double nrofsec = t.TotalSeconds;
                    
                    ok = (Int64)Math.Round(nrofsec);
                    //Convert.ToInt64(nrofsec.ToString("N0"));
                }
                    
                  

                

            }
            catch (Exception ex)
            {
                string error = ex.ToString();

                if (error.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    error = "Data Base not in Network";
                }
                // Uitility.LogError(ex);
            }

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(ok);
            return json;
        }      

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveDownTimeOK(string wcname, string resionid,string save)
        {

            string ok = "";
            try
            {

                Tbmreceipedatabase obj = new Tbmreceipedatabase();
                obj.ok = save;
                obj.wcname = wcname;
                obj.manningID = Session["ManningID1"].ToString();
                obj.ResionId = resionid;
                ok = obj.SaveDownTime();
                Uitility.LogEvent(" \t Save DownTime With Resion \t " +DateTime.Now.ToString() + "Operator " + Session["ManningID1"].ToString() + " " , wcname);





            }
            catch (Exception ex)
            {
                string error = ex.ToString();

                if (error.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    error = "Data Base not in Network";
                }
                Uitility.LogError(ex, wcname);
            }

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(ok);
            return json;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveDownTime(string wcname, string resionid)
        {


            string ok = "";
            try
            {

                Tbmreceipedatabase obj = new Tbmreceipedatabase();
                obj.ok = "";
                obj.wcname = wcname;
                obj.manningID = Session["ManningID1"].ToString();
                obj.ResionId = resionid;
                ok = obj.SaveDownTime();
                Uitility.LogEvent(" \t DownTime Start \t " + DateTime.Now.ToString() + "Operator " + Session["ManningID1"].ToString() + " ", wcname);





            }
            catch (Exception ex)
            {
                string error = ex.ToString();

                if (error.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    error = "Data Base not in Network";
                }
                 Uitility.LogError(ex , wcname);
            }

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(ok);
            return json;
        }


    }

    

}
