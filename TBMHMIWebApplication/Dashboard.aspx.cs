using System;
using System.Data;
using System.Web.Services;


namespace TBMHMIWebApplication
{
    public partial class Dashboard : System.Web.UI.Page
    {
       // OPCManager aa = new OPCManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            int count = 0;
            if (Session["OP1"] != null)
            { count = count + 1; }
            if (Session["OP2"] != null)
            { count = count + 1; }
            if (Session["OP3"] != null)
            { count = count + 1; }
            if (count < 2)
            {
               // Uitility.LogEvent("Operator 1 " + Session["UserId1"].ToString() + "Operator 2 " + Session["UserId2"].ToString() + "Login Sucessfully", Request.QueryString["HmiId"].ToString());
                Response.Redirect("Login.aspx?HmiId=" + Request.QueryString["HmiId"].ToString());
            }

            if (!IsPostBack)
            {

                getdetail();
                txtbarcode.Text = "";
                lblwcname.Text = Request.QueryString["HmiId"].ToString();



               


                //Timer1.Enabled = true;
            }

        }
        public void getdetail()
        {

            if (Session["OP1"] != null)
            {
               
                lblop1.Text = Session["UserId1"].ToString();
                opname1.Text = Session["OP1"].ToString();

                if (System.IO.File.Exists(Server.MapPath(Session["OP1img"].ToString())))
                {
                    imgop1.ImageUrl = Session["OP1img"].ToString();

                }
                else
                {

                    imgop1.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                }
            }
            else
            {
                imgop1.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                lblop1.Text = "Operator 1";
            }
            if (Session["OP2"] != null)
            {
                
                lblop2.Text = Session["UserId2"].ToString();
                opname2.Text = Session["OP2"].ToString();
                if (System.IO.File.Exists(Server.MapPath(Session["OP1img"].ToString())))
                {
                    imgop2.ImageUrl = Session["OP2img"].ToString();

                }
                else
                {

                    imgop2.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                }
            }
            else
            {
                imgop2.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                lblop2.Text = "Operator 2";
            }

            if (Session["OP3"] != null)
            {
                
                lblop3.Text = Session["UserId3"].ToString();
                opname3.Text = Session["OP3"].ToString();
                if (System.IO.File.Exists(Server.MapPath(Session["OP3img"].ToString())))
                {
                    imgop3.ImageUrl = Session["OP3img"].ToString();

                }
                else
                {

                    imgop3.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                }
            }
            else
            {
                imgop3.ImageUrl = ".//assets/images/tbm_app/default-user-img.jpg";
                lblop3.Text = "Operator 3";
            }


        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtuserdetail = new DataTable();

                if (Session["OP1"] == null)
                {
                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                    TbmreceipedatabaseObj.UserId = txtusername.Text;
                    TbmreceipedatabaseObj.Password = txtpassword.Text;
                    TbmreceipedatabaseObj.wcname = Request.QueryString["HmiId"].ToString();
                    dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                    if (dtuserdetail.Rows.Count > 0)
                    {
                        Session["OP1"] = dtuserdetail.Rows[0]["firstName"].ToString();
                        Session["UserId1"] = dtuserdetail.Rows[0]["UserId"].ToString();
                        Session["ManningID1"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                        Session["OP1img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();
                        Uitility.LogEvent("Operator 1 " + Session["UserId1"].ToString(), Request.QueryString["HmiId"].ToString());

                        if (Session["OP1"].ToString().Contains(Session["OP2"].ToString()) && Session["OP1"].ToString().Contains(Session["OP3"].ToString()))
                        {
                            Session.Remove("OP1");
                            Session.Remove("ManningID1");
                            Session.Remove("OP1img");
                            Session.Remove("UserId1");
                        }

                       
                    }


                }
                else if (Session["OP2"] == null && txtusername.Text != Session["UserId1"].ToString())
                {
                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                    TbmreceipedatabaseObj.UserId = txtusername.Text;
                    TbmreceipedatabaseObj.Password = txtpassword.Text;
                    TbmreceipedatabaseObj.wcname = Request.QueryString["HmiId"].ToString();
                    dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                    if (dtuserdetail.Rows.Count > 0)
                    {
                        Session["OP2"] = dtuserdetail.Rows[0]["firstName"].ToString();
                        Session["ManningID2"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                        Session["OP2img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();
                        Session["UserId2"] = dtuserdetail.Rows[0]["UserId"].ToString();
                        Uitility.LogEvent("Operator 2 " + Session["UserId1"].ToString(), Request.QueryString["HmiId"].ToString());
                        if (Session["OP2"].ToString().Contains(Session["OP1"].ToString()) && Session["OP2"].ToString().Contains(Session["OP3"].ToString()))
                        {
                            Session.Remove("OP2");
                            Session.Remove("ManningID2");
                            Session.Remove("OP2img");
                            Session.Remove("UserId2");
                        }


                    }
                }
                else if (Session["OP3"] == null && txtusername.Text != Session["UserId1"].ToString() && txtusername.Text != Session["UserId2"].ToString())
                {
                    Tbmreceipedatabase TbmreceipedatabaseObj = new Tbmreceipedatabase();
                    TbmreceipedatabaseObj.UserId = txtusername.Text;
                    TbmreceipedatabaseObj.Password = txtpassword.Text;
                    TbmreceipedatabaseObj.wcname = Request.QueryString["HmiId"].ToString();
                    dtuserdetail = TbmreceipedatabaseObj.SelectUserDetail();
                    if (dtuserdetail.Rows.Count > 0)
                    {
                        string hmid = dtuserdetail.Rows[0]["WcName"].ToString();

                        if (hmid == "0" || hmid.ToLower() == Request.QueryString["HmiId"].ToString().ToLower())
                        {
                            Session["OP3"] = dtuserdetail.Rows[0]["firstName"].ToString();
                            Session["ManningID3"] = dtuserdetail.Rows[0]["ManningID"].ToString();
                            Session["OP3img"] = dtuserdetail.Rows[0]["UserPhoto"].ToString();
                            Session["UserId3"] = dtuserdetail.Rows[0]["UserId"].ToString();
                            Uitility.LogEvent("Operator 3 " + Session["UserId1"].ToString(), Request.QueryString["HmiId"].ToString());
                            if (Session["OP3"].ToString().Contains(Session["OP2"].ToString()) && Session["OP3"].ToString().Contains(Session["OP1"].ToString()))
                            {
                                Session.Remove("OP3");
                                Session.Remove("ManningID3");
                                Session.Remove("OP3img");
                                Session.Remove("UserId3");
                            }
                        }


                    }
                }
                getdetail();
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                if (error.Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    lblop1.Text = "Data Base not in Network";

                }
                Uitility.LogError(ex, Request.QueryString["HmiId"].ToString());

            }
        }
     








    }
}