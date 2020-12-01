using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TBMHMIWebApplication
{
    public partial class Login : System.Web.UI.Page
    {
        //static OPCManager = new OPCManager();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                try { 

                imgop1.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                imgop2.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                imgop3.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                getdetail();
                }
                catch (Exception ex)
                {
                    Uitility.LogError(ex, Request.QueryString["HmiId"].ToString());
                }
            }
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtuserdetail = new DataTable();

                DataTable dtwcid = new DataTable();
                Tbmreceipedatabase TbmWcnameObj = new Tbmreceipedatabase();
                TbmWcnameObj.wcname= Request.QueryString["HmiId"].ToString();
                dtwcid = TbmWcnameObj.selectWcid();
                if (dtwcid.Rows.Count > 0)
                {
                    Session["Wcid"] = dtwcid.Rows[0]["id"].ToString();
                }
                else
                {
                    Session["Wcid"] = "0";
                }
                if (Session["OP1"] == null)
                {
                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                    TbmreceipedatabaseObj.UserId = txtusername.Text;
                    TbmreceipedatabaseObj.Password = txtpassword.Text;
                    TbmreceipedatabaseObj.wcname = Request.QueryString["HmiId"].ToString();
                    dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                    if (dtuserdetail.Rows.Count > 0)
                    {
                        string hmid= dtuserdetail.Rows[0]["WcName"].ToString();

                        if (hmid == "0" || hmid.ToLower() == Request.QueryString["HmiId"].ToString().ToLower())
                        {
                            lblinformation.Text = hmid;
                            Session["OP1"] = dtuserdetail.Rows[0]["firstName"].ToString();
                            Session["UserId1"] = dtuserdetail.Rows[0]["UserId"].ToString();
                            Session["ManningID1"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                          
                            if (System.IO.File.Exists(Server.MapPath(dtuserdetail.Rows[0]["UserPhoto"].ToString())))
                            {
                                Session["OP1img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();
                                
                            }
                            else
                            {

                                Session["OP1img"] = ".//assets/images/tbm_app/default-user-img.jpg";
                            }
                            Uitility.LogEvent("Operator 1 Login " + Session["UserId1"].ToString(), hmid);
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



                            if (a==b && a==c)
                            {
                                Session.Remove("OP1");
                                Session.Remove("ManningID1");
                                Session.Remove("OP1img");
                                Session.Remove("UserId1");
                            }
                            oplogin1.Visible = false;
                            btnloginop1.Visible = true;
                            lblop1.Text = dtuserdetail.Rows[0]["firstName"].ToString();
                            lblinformation.Text = "";
                        }
                        else
                        { lblinformation.Text = hmid; }
                    }


                }
                else if (Session["OP2"] == null &&  (txtusername.Text ?? "").ToLower() != (Session["UserId1"].ToString() ?? "").ToLower())
                {
                    
                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                    TbmreceipedatabaseObj.UserId = txtusername.Text;
                    TbmreceipedatabaseObj.Password = txtpassword.Text;
                    TbmreceipedatabaseObj.wcname = Request.QueryString["HmiId"].ToString();
                    dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                    if (dtuserdetail.Rows.Count > 0)
                    { string hmid = dtuserdetail.Rows[0]["WcName"].ToString();

                        if (hmid == "0" || hmid.ToLower() == Request.QueryString["HmiId"].ToString().ToLower())
                        {
                            lblinformation.Text = hmid;
                            Session["OP2"] = dtuserdetail.Rows[0]["firstName"].ToString();
                            Session["ManningID2"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                          //  Session["OP2img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();
                            Session["UserId2"] = dtuserdetail.Rows[0]["UserId"].ToString();
                            if (System.IO.File.Exists(Server.MapPath(dtuserdetail.Rows[0]["UserPhoto"].ToString())))
                            {
                                Session["OP2img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();

                            }
                            else
                            {

                                Session["OP2img"] = ".//assets/images/tbm_app/default-user-img.jpg";
                            }

                            Uitility.LogEvent("Operator 2 Login " + Session["UserId2"].ToString(), hmid);

                            oplogin1.Visible = false;
                            btnloginop1.Visible = true;
                            lblop1.Text = dtuserdetail.Rows[0]["firstName"].ToString();
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
                            lblinformation.Text = "";
                        }
                        else
                        { lblinformation.Text = hmid; }

                    }
                }
                else if (Session["OP3"] != null && Session["OP3"].ToString() != "")
                {
                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                    TbmreceipedatabaseObj.UserId = txtusername.Text;
                    TbmreceipedatabaseObj.Password = txtpassword.Text;
                    TbmreceipedatabaseObj.wcname = Request.QueryString["HmiId"].ToString();
                    dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                    if (dtuserdetail.Rows.Count > 0)
                    { string hmid = dtuserdetail.Rows[0]["WcName"].ToString();

                        if (hmid == "0" || hmid.ToLower() == Request.QueryString["HmiId"].ToString().ToLower())
                        {
                            lblinformation.Text = hmid;
                            Session["OP3"] = dtuserdetail.Rows[0]["firstName"].ToString();
                            Session["ManningID3"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                          //  Session["OP3img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();
                            if (System.IO.File.Exists(dtuserdetail.Rows[0]["UserPhoto"].ToString()))
                            {
                                Session["OP3img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();

                            }
                            else
                            {

                                Session["OP3img"] = ".//assets/images/tbm_app/default-user-img.jpg";
                            }
                            Session["UserId3"] = dtuserdetail.Rows[0]["UserId"].ToString();
                            Uitility.LogEvent("Operator 3 Login " + Session["UserId3"].ToString(), hmid);
                            string a = Session["OP1"].ToString();
                            string b = "";
                            if (Session["OP2"] != null)
                            {
                               b= Session["OP2"].ToString();
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
                            lblinformation.Text = "";
                        }
                        else
                        { lblinformation.Text = hmid; }

                    }
                }
                getdetail();
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
              if( error.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    lblop1.Text = "Data Base not in Network";

                }
                Uitility.LogError(ex, Request.QueryString["HmiId"].ToString());

            }
        }
        public void getdetail()
        {
            try
            {
                if (Session["OP1"] != null)
                {


                    imgop1.ImageUrl = Session["OP1img"].ToString();
                    oplogin1.Visible = false;
                    btnloginop1.Visible = true;
                    lblop1.Text = Session["OP1"].ToString();
                }
                else
                {
                    imgop1.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";

                    oplogin1.Visible = true;
                    btnloginop1.Visible = false;
                    Session["OP1"] = null;
                    Session["UserId1"] = null;
                    lblop1.Text = "Operator 1";
                }

                if (Session["OP2"] != null)
                {


                    imgop2.ImageUrl = Session["OP2img"].ToString();
                    oplogin2.Visible = false;
                    btnloginop2.Visible = true;
                    lblop2.Text = Session["OP2"].ToString();
                }
                else
                {
                    imgop2.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";

                    oplogin2.Visible = true;
                    btnloginop2.Visible = false;
                    Session["OP2"] = null;
                    Session["UserId2"] = null;
                    lblop2.Text = "Operator 2";
                }

                if (Session["OP3"] != null)
                {


                    imgop3.ImageUrl = Session["OP3img"].ToString();
                    oplogin3.Visible = false;
                    btnloginop3.Visible = true;
                    lblop3.Text = Session["OP3"].ToString();
                }
                else
                {
                    imgop3.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";

                    oplogin3.Visible = true;
                    btnloginop3.Visible = false;
                    Session["OP3"] = null;
                    Session["UserId3"] = null;
                    lblop3.Text = "Operator 3";
                }
                int count = 0;
                if (Session["OP1"] != null)
                { count = count + 1; }
                if (Session["OP2"] != null)
                { count = count + 1; }
                if (Session["OP3"] != null)
                { count = count + 1; }
                if (count >= 2)
                {
                   


                    string id = Request.QueryString["HmiId"].ToString();
                    Response.Redirect("Dashboard.aspx?HmiId=" + id, false);



                }
            }
            catch(Exception ex)
            {
                Uitility.LogError(ex, Request.QueryString["HmiId"].ToString());
            }
        }
       
        protected void btnloginop1_Click(object sender, EventArgs e)
        {
            try
            {
                imgop1.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";

                oplogin1.Visible = true;
                btnloginop1.Visible = false;
                Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                TbmreceipedatabaseObj.UserId = Session["UserId1"].ToString();
                TbmreceipedatabaseObj.Logoutuser();
                Session.Remove("OP1");
                Session.Remove("ManningID1");
                Session.Remove("OP1img");
                Session.Remove("UserId1");

                lblop1.Text = "Operator 1";

            }
            catch (Exception ex)
            {
                Uitility.LogError(ex, Request.QueryString["HmiId"].ToString());
            }
}

        protected void btnloginop2_Click(object sender, EventArgs e)
        {
            try { 
            imgop2.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
            Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
            TbmreceipedatabaseObj.UserId = Session["UserId2"].ToString();
            TbmreceipedatabaseObj.Logoutuser();
            oplogin2.Visible = true;
            btnloginop2.Visible = false;
            Session["OP2"] = null;
            Session["UserId2"] = null;
            lblop2.Text = "Operator 2";
            }
            catch (Exception ex)
            {
                Uitility.LogError(ex, Request.QueryString["HmiId"].ToString());
            }
        }

        protected void btnloginop3_Click(object sender, EventArgs e)
        {
            try { 
            imgop3.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
            Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
            TbmreceipedatabaseObj.UserId = Session["UserId3"].ToString();
            TbmreceipedatabaseObj.Logoutuser();
            oplogin3.Visible = true;
            btnloginop3.Visible = false;
            Session["OP3"] = null;
            Session["UserId3"] = null;
            lblop3.Text = "Operator 3";
            }
            catch (Exception ex)
            {
                Uitility.LogError(ex, Request.QueryString["HmiId"].ToString());
            }
        }
    }
}