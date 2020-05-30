using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPanel_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (this.IsValid)
        {
            using (IDataReader iDr = clsDatabase.GetRS("SELECT AdminID,Name, AccessLevel FROM AdminMast WHERE Active='Y' AND UserName=N'" + txtUserName.Text.Trim().Replace("'", "''") + "' and Password=N'" + txtPassword.Text.Trim().Replace("'", "''") + "'"))
            {
                if (iDr.Read())
                {
                    //Hashtable hsAdminLogin = new Hashtable();
                    //hsAdminLogin.Add("AdminID", iDr["AdminID"].ToString());
                    //hsAdminLogin.Add("AccessLevel", iDr["AccessLevel"].ToString());
                    //hsAdminLogin.Add("AName", iDr["Name"].ToString());
                    //HttpContext.Current.Session["ADMIN"] = hsAdminLogin;
                    clsCommon.AdminLogin(iDr, true);

                    if (chkRemember.Checked == true)
                    {
                        Response.Cookies["username"].Value = txtUserName.Text.Trim();
                        Response.Cookies["password"].Value = txtPassword.Text.Trim();
                        Response.Cookies["username"].Expires = DateTime.Now.AddDays(15);
                        Response.Cookies["password"].Expires = DateTime.Now.AddDays(15);
                    }
                   // HttpContext.Current.Response.Redirect("/CPanel/Dashboard1.aspx");
                }
                else
                {
                    txtUserName.Focus();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "$.notify({ title: \"Login Failed : \", message: \"Username or Password is incorrect please try again.\", icon: 'fa fa-close' }, { type: \"danger\", newest_on_top: true });", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "$.notify({ title: \"Login Failed : \", message: \"Username or Password is incorrect please try again.\", icon: 'fa fa-close' }, { type: \"danger\", newest_on_top: true });", true);
            txtUserName.Focus();
        }
    }
}