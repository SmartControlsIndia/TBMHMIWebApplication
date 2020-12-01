using System;
using System.Collections.Generic;
using System.Data;
 
using System.Web;

namespace TBMHMIWebApplication
{
    public class Tbmreceipedatabase
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }

        public string ReceipeName { get; set; }


        public string wcID { get; set; }
        public string gtbarCode { get; set; }
       
        public string recipeCode { get; set; }
        public string gtWeight { get; set; }
        public string manningID { get; set; }
        public string ResionId { get; set; }
        public string manningID2 { get; set; }
        public string manningID3 { get; set; }
        public int StatusId { get; set; }

        public string wcname { get; set; }
        public string ok { get; set; }

        public string defectID { get; set; }
        public DataTable selectWcid()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@WcName", wcname);
           return db.ExecuteDataTable("selectWcid");
        }

        public DataTable SelectUserDetail()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@userid", UserId);
            db.AddParameter("@password", Password);
            db.AddParameter("@wcname", wcname);
            return db.ExecuteDataTable("SelectUserDetail");
        }
        public DataTable selecttagreceipe()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@HMINAME", wcname);
           
            return db.ExecuteDataTable("selecttagreceipe");
        }
        
        public string selectHMIDisplayInfo()
        {
            DBConnection db = new DBConnection();
           
            return Convert.ToString(db.ExecuteScalar("selectHMIDisplayInfo"));

        }
        public string getHMIDownTimeStart()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@wcname", wcname);
            return Convert.ToString(db.ExecuteScalar("getHMIDownTimeStart"));

        }
        public string AddUpdateChangePassword()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@userid", UserId);
            db.AddParameter("@password", Password);
            db.AddParameter("@NewPassword", NewPassword);
            db.AddParameter("@wcname", wcname);
            return Convert.ToString(db.ExecuteScalar("AddUpdateChangePassword"));
           
        }
        public DataTable selectuserdetailLogOut()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@userid", UserId);
            db.AddParameter("@password", Password);
            db.AddParameter("@wcname", wcname);
            return db.ExecuteDataTable("selectuserdetailLogOut");
        }
        
        public DataTable getoem()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@name", ReceipeName);
            return db.ExecuteDataTable("getoem");
        }

        public string Addtbmpcr()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@wcID", wcID);
            db.AddParameter("@gtbarCode", gtbarCode);
          
            db.AddParameter("@recipeCode", recipeCode);
            db.AddParameter("@gtWeight", gtWeight);
            db.AddParameter("@manningID", manningID);
          
            db.AddParameter("@manningID2", manningID2);
            db.AddParameter("@manningID3", manningID3);
            db.AddParameter("@defectID", defectID);
            db.AddParameter("@StatusId", StatusId);
            
            return Convert.ToString(db.ExecuteScalar("Addtbmpcr"));
        }
        public string Logoutuser()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@UserId", UserId);
            return Convert.ToString(db.ExecuteScalar("Logoutuser"));
        }

        public string AddRecipe()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@RecipeName", ReceipeName);
            return Convert.ToString(db.ExecuteScalar("AddRecipe"));
        }
        public string SaveDownTime()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@save", ok);
            db.AddParameter("@wcname", wcname);
            db.AddParameter("@resionid ", ResionId);
            db.AddParameter("@manningId", manningID);
            return Convert.ToString(db.ExecuteScalar("AddUpdateSaveDownTime"));
        }


        public DataTable CheckBarcode()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@Barcode", gtbarCode);
            return db.ExecuteDataTable("CheckBarcode");
        }

        public DataTable sp_TBMGetShiftWiseCount()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@Wcname", wcname);
            return db.ExecuteDataTable("sp_TBMGetShiftWiseCount");
        }
        public DataTable selecttbmpcr()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@wcname", wcname);
            return db.ExecuteDataTable("selecttbmpcr");
        }
        public DataTable selectReasion()
        {
            DBConnection db = new DBConnection();
            return db.ExecuteDataTable("selectReasion");


        }
    }
}