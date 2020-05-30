using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPanel_ChangeAdminLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsCommon.AdminAuthentication();
        if (!Page.IsPostBack)
        {
            //START: Handle Super and Upper Addmin autologin
            string AdminID = Request.QueryString["AdminID"];
            string strErr = "";
            if (!String.IsNullOrWhiteSpace(AdminID))
            {
                using (IDataReader iDr = clsDatabase.GetRS("SELECT AdminID,Name,AccessLevel FROM AdminMast WHERE AdminId = " + clsCommon.sQuote_N(AdminID.Trim().Replace("'", "''"))))
                {
                    if (iDr.Read())
                    {
                        string _CurrentLoggedInAdminId = "", _CurrentAdminAccessLevel="";
                       // bool _IsHeadCounGroupShare = clsCommon.IsSuperORUpperAdmin(clsCommon.GetSessionKeyValue("AdminID"));

                        if (string.IsNullOrWhiteSpace(Convert.ToString(Session["PreviousLoggedInAdminId"])))
                        {
                           _CurrentLoggedInAdminId = clsCommon.GetSessionKeyValue("AdminID"); 
                        }
                        if (string.IsNullOrWhiteSpace(Convert.ToString(Session["PreviousAdminAccessLevel"])))
                        {
                            _CurrentAdminAccessLevel = clsCommon.GetSessionKeyValue("AccessLevel");
                        }

                        clsCommon.AdminLogout();
                        clsCommon.AdminLogin(iDr, false, "", _CurrentLoggedInAdminId, _CurrentAdminAccessLevel);
                        Response.Redirect("Dashboard1.aspx");
                    }
                }
            }
            //END: Handle Super and Upper Addmin autologin
        }


    }
}