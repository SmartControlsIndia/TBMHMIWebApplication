using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TBMHMIWebApplication
{
    public class SimulationTagWc
    {
        public string ReceipeName { get; set; }
        public string Wcid { get; set; }
        public DataTable selectSimulationWcName()
        {
            DBConnection db = new DBConnection();

            return db.ExecuteDataTable("selectSimulationWcName");
        }
        public string UpdateReceipe()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@wcid", Wcid);
            db.AddParameter("@ReceipeName", ReceipeName);
            return Convert.ToString(db.ExecuteScalar("UpdateReceipe"));
        }
        public void selectReceipe(DropDownList ddl)
        {
            DBConnection db = new DBConnection();
            DataTable dt = db.ExecuteDataTable("selectReceipe");
            ddl.DataTextField = "name";
            ddl.DataValueField = "id";
            ddl.DataSource = dt;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        public DataTable selectReceipeName()
        {
            DBConnection db = new DBConnection();
            return db.ExecuteDataTable("selectReceipe");
            

        }
        public DataTable selectselectSimulationWcNameName()
        {
            DBConnection db = new DBConnection();
            return db.ExecuteDataTable("selectSimulationWcName");


        }
        public void selectSimulationWcName(DropDownList ddl)
        {
            DBConnection db = new DBConnection();
            DataTable dt = db.ExecuteDataTable("selectSimulationWcName");
            ddl.DataTextField = "name";
            ddl.DataValueField = "id";
            ddl.DataSource = dt;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        public string AddRecipe()
        {
            DBConnection db = new DBConnection();
            db.AddParameter("@RecipeName", ReceipeName);
            return Convert.ToString(db.ExecuteScalar("AddRecipe"));
        }
    }
}