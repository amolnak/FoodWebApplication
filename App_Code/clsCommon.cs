using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for clsCommon
/// </summary>
public class clsCommon
{
    /// <summary>
    /// Put a Single Quote to parameter field
    /// </summary>
    /// <param name="sFieldNM"></param>
    /// <returns></returns>
    public static string sQuote(string sFieldNM)
    {
        return "'" + sFieldNM + "'";
    }

    /// <summary>
    ///  Put a Single Quote to unicode field
    /// </summary>
    /// <param name="sFieldNM"></param>
    /// <returns></returns>
    public static string sQuote_N(string sFieldNM)
    {
        return "N'" + sFieldNM + "'";
    }

    /// <summary>
    /// Handles SQLInjection
    /// </summary>
    /// <param name="sFieldNM"></param>
    /// <param name="iFieldLength"></param>
    /// <returns></returns>
    public static string Remove_SQLInjection(string sFieldNM, int iFieldLength = 0)
    {
        if (!(string.IsNullOrWhiteSpace(sFieldNM)))
        {
            sFieldNM = sFieldNM.Replace("--", "").Replace(";", "");


            if ((iFieldLength > 0) && (sFieldNM.Length > iFieldLength))
                sFieldNM = sFieldNM.Substring(0, iFieldLength);

            sFieldNM = sFieldNM.Replace("'", "''");
        }
        return sFieldNM;
    }

    public static string Remove_SQLInjectionWithDQuote(string sFieldNM, int iFieldLength = 0)
    {
        if (sFieldNM != "")
        {
            sFieldNM = sFieldNM.Replace("--", "").Replace(";", "").Replace("\"", "");


            if ((iFieldLength > 0) && (sFieldNM.Length > iFieldLength))
                sFieldNM = sFieldNM.Substring(0, iFieldLength);

            sFieldNM = sFieldNM.Replace("'", "''");
        }
        return sFieldNM;
    }

    /// <summary>
    /// Shows Javascript Alert Box. If Second Parameter is supplied, then it redirects to the page mentioned in second parameter.
    /// </summary>
    /// <param name="sMsg"></param>
    /// <param name="sRedirectUrl"></param>
    public static void AlertBox(string sMsg, string sRedirectUrl = "")
    {
        //HttpContext.Current.Response.Write(sRedirectUrl + "<br/>");
        //HttpContext.Current.Response.End();

        HttpContext.Current.Response.Write("<script> alert(\"" + sMsg + "\");" + (sRedirectUrl.Length > 0 ? "window.location='" + sRedirectUrl + "';" : "") + " </script>");
    }
    public static void SuccessAlertBox(string sMsg, string sRedirectUrl = "")
    {
        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "alert_success", "<script>swal({html:true, title: '', text: '" + sMsg + "', type: 'success'}, function() {window.location = '" + sRedirectUrl + "';});</script>", false);
    }
    public static void ErrorAlertBox(string sMsg, string sRedirectUrl = "")
    {
        string sMsgNew = sMsg.Replace("'", "");
        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "error_success", "<script>swal({html:true,title: 'Sorry...', text: '" + sMsgNew + "', type: 'error'}, function() {window.location = '" + sRedirectUrl + "';});</script>", false);
    }
    public static void ErrorAlertBoxWithoutPageReload(string sMsg, string sRedirectUrl = "")
    {
        string sMsgNew = sMsg.Replace("'", "");
        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "error_success", "<script>swal({html:true,title: 'Sorry...', text: '" + sMsgNew + "', type: 'error'});</script>", false);
    }
    public static void WarningAlertBoxWithoutPageReload(string sMsg, string sRedirectUrl = "")
    {
        string sMsgNew = sMsg.Replace("'", "");
        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.CurrentHandler, typeof(Page), "warning", "<script>swal({html:true,title: '', text: '" + sMsgNew + "', type: 'warning'});</script>", false);
    }
    public static void FillCombo(ref DropDownList ddlCombo, string sQry, string sDataMember, string sValueMember, string sDefaultText, ref string ErrMsgs)
    {
        string ErrMsg = ""; ErrMsgs = "";
        using (DataTable Dt = clsDatabase.GetDT(sQry, ref ErrMsg))
        {
            if (ErrMsg != "")
            {
                ErrMsgs = ErrMsg;
            }
            if (Dt.Rows.Count > 0)
            {
                ddlCombo.Items.Clear();
                ddlCombo.DataSource = Dt;
                ddlCombo.DataTextField = sDataMember;
                ddlCombo.DataValueField = sValueMember;
                ddlCombo.DataBind();
            }
        }
        if (sDefaultText.Trim().Length > 0)
        {
            ddlCombo.Items.Insert(0, new ListItem(sDefaultText.Trim(), ""));
            ddlCombo.SelectedValue = "";
        }
    }

    /// <summary>
    /// Converts string to integer
    /// </summary>
    /// <param name="_Val"></param>
    /// <returns></returns>
    public static int ParseInteger(string _Val)
    {
        int iRetVal = 0;
        try { Int32.TryParse(_Val, out iRetVal); }
        catch { iRetVal = 0; }
        return iRetVal;
    }

    /// <summary>
    /// Converts string to double
    /// </summary>
    /// <param name="_Val"></param>
    /// <returns></returns>
    public static double ParseDouble(string _Val)
    {
        double iRetVal = 0;
        try { double.TryParse(_Val, out iRetVal); }
        catch { iRetVal = 0; }
        return iRetVal;
    }

    /// <summary>
    /// Convert a value to 2 decimal without applying round off.
    /// </summary>
    /// <param name="sValue">Value to convert</param>
    /// <returns>Two decimal string value</returns>
    public static string ConvertTo2Decimal(string sValue)
    {
        string sRetVal = "";
        try
        {
            if (sValue.IndexOf('.') > 0)
            {
                sValue = ParseDouble(sValue).ToString("N3"); /* First convert in to 3 decimal to handle index out of range error. */
                sRetVal = sValue.Substring(0, sValue.IndexOf('.')) + "." + sValue.Substring(sValue.IndexOf('.') + 1, 2);
            }
            else
            {
                sRetVal = ParseDouble(sValue).ToString("N2");
            }
        }
        catch { sRetVal = sRetVal = ParseDouble(sValue).ToString("N2"); }
        return sRetVal;
    }

    /// <summary>
    /// This method can be used at the time of manual creating Json object to bind JQGrid using stringbuilder. 
    /// No need to use this method at the time of creating Json object using SerializeObject method 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ValidateJsonStringWithEscChars(string value)
    {
        value = value.Replace("\r", "\\r");
        value = value.Replace("\n", "\\n");
        value = value.Replace("\\", "\\\\");
        value = value.Replace("\"", "\\\"");
        value = value.Replace("'", "\'");
        value = value.Replace("\t", "\\t");
        value = value.Replace("\b", "\\b");
        value = value.Replace("\f", "\\f");
        // value = value.Replace(":", "\\:");
        value = value.Replace("\\u", "\\\\u");

        return value;
    }
    public static string ValidateJsonString(string value)
    {
        value = value.Replace("\\", "");
        value = value.Replace("\r", "");
        value = value.Replace("\n", "");
        value = value.Replace("\"", "");
        value = value.Replace("'", "");
        value = value.Replace("\t", "");
        value = value.Replace("\b", "");
        value = value.Replace("\f", "");
        value = value.Replace(":", "");
        value = value.Replace("\\u", "");

        return value;
    }

    public static string GetGridPaging_ArrayList(int RowCount = 0)
    {
        string strArray = "";
        string strDefaultPagesize_Value = "";
        string strViewAllLimit_Value = "";
        try
        {
            string CPanelGridPaging = ConfigurationManager.AppSettings.Get("CPanelGridPaging");
            strArray = CPanelGridPaging.Split('|')[0];
            strDefaultPagesize_Value = CPanelGridPaging.Split('|')[1];
            strViewAllLimit_Value = CPanelGridPaging.Split('|')[2];
            if (RowCount != 0)
            {
                if (RowCount <= Convert.ToInt32(strViewAllLimit_Value) && RowCount > Convert.ToInt32(strDefaultPagesize_Value))
                    strArray = "[" + strArray + ",'ALL']";
                else
                    strArray = "[" + strArray + "]";
            }
            else
                strArray = "[" + strArray + "]";
        }
        catch (Exception ex)
        {

            throw ex;
        }
        return strArray;
    }

    public static string GetGridPaging_DefaultPagesize()
    {
        string strDefaultPagesize_Value = "";
        try
        {
            string CPanelGridPaging = ConfigurationManager.AppSettings.Get("CPanelGridPaging");
            strDefaultPagesize_Value = CPanelGridPaging.Split('|')[1];
        }
        catch (Exception ex)
        {

            throw ex;
        }
        return strDefaultPagesize_Value;
    }

    public static string GetGridPaging_ViewAllLimit()
    {
        string strViewAllLimit_Value = "";
        try
        {
            string CPanelGridPaging = ConfigurationManager.AppSettings.Get("CPanelGridPaging");
            strViewAllLimit_Value = CPanelGridPaging.Split('|')[2];
        }
        catch (Exception ex)
        {

            throw ex;
        }
        return strViewAllLimit_Value;
    }

    public static string GetSessionKeyValue(string key)
    {
        string KeyValue = "";
        Hashtable ht;

        if (HttpContext.Current.Request.QueryString["DEVELOPER"] != null && HttpContext.Current.Request.QueryString["DEVELOPER"] == "Y")
        {
            ht = (Hashtable)HttpContext.Current.Session["DEVELOPER"];
        }
        else
        {
            ht = (Hashtable)HttpContext.Current.Session["ADMIN"];
        }

        if (ht != null)
        {
            foreach (DictionaryEntry dEntry in ht)
            {
                if (dEntry.Key.ToString() == key)
                {
                    KeyValue = dEntry.Value.ToString();
                }
            }
        }

        return KeyValue;
    }

    public static string SiteURLSub(string url)
    {
        if (HttpContext.Current.Request.QueryString["DEVELOPER"] != null && HttpContext.Current.Request.QueryString["DEVELOPER"] == "Y")
        {
            if (url.Contains("?"))
            {
                url = url + "&DEVP=Y";
            }
            else
            {
                url = url + "?DEVP=Y";
            }
        }

        return url;
    }

    public static void AdminAuthentication()
    {
        //if (clsCommon.IsPortalUnderMaintenance() == "Y" && HttpContext.Current.Request.QueryString["WTIL"] == null)
        //{
        //    HttpContext.Current.Session.RemoveAll();
        //    HttpContext.Current.Response.Redirect("/CPanel/Secure/SiteUnderMaintenance.aspx");
        //}

        if (HttpContext.Current.Request.QueryString["DEVELOPER"] != null && HttpContext.Current.Request.QueryString["DEVELOPER"] == "Y")
        {
            if (!(HttpContext.Current.Session["DEVELOPER"] != null && GetSessionKeyValue("AccessLevel") == "0"))
            {
                HttpContext.Current.Response.Redirect("/CPanel/Logout.aspx");
            }
        }
        else
        {
            if (!(HttpContext.Current.Session["ADMIN"] != null && Convert.ToInt16(GetSessionKeyValue("AccessLevel")) > 0))
            {
                HttpContext.Current.Response.Redirect("/CPanel/Logout.aspx");
            }
        }
    }

    public static string IsPortalUnderMaintenance()
    {
        string UnderMaintenance = "N";
        string ErrMsg = "";
        DataTable dt = clsDatabase.GetDT(" select * from SiteConfig ", ref ErrMsg);
        if (ErrMsg != "")
        {
            return "";
        }
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["PortalUnderMain"].ToString().Trim() == "Y")
            {
                UnderMaintenance = "Y";
            }
        }
        // UnderMaintenance = "Y";
        return UnderMaintenance;
    }

    public static void AdminLogout(string MainLogOut = "")
    {
        if (HttpContext.Current.Request.QueryString["DEVELOPER"] != null && HttpContext.Current.Request.QueryString["DEVELOPER"] == "Y")
        {
            HttpContext.Current.Session["DEVELOPER"] = null;
            HttpContext.Current.Session["ADMIN"] = null;
            if ((!string.IsNullOrWhiteSpace(MainLogOut)) && MainLogOut == "Y") { HttpContext.Current.Session["PreviousLoggedInAdminId"] = null; }
        }
        else
        {
            HttpContext.Current.Session["ADMIN"] = null;
            if ((!string.IsNullOrWhiteSpace(MainLogOut)) && MainLogOut == "Y") { HttpContext.Current.Session["PreviousLoggedInAdminId"] = null; }
        }
    }

    public static void AdminLogin(IDataReader iDr, Boolean Redirect = true, string AdminID = "", string CurrentLoggedInAdminId = "", string CurrentAdminAccessLevel = "")
    {
        Hashtable hsAdminLogin = new Hashtable();
        hsAdminLogin.Add("AdminID", iDr["AdminID"].ToString());
        hsAdminLogin.Add("AccessLevel", iDr["AccessLevel"].ToString());
        hsAdminLogin.Add("AName", iDr["Name"].ToString());

        if (iDr["AccessLevel"].ToString() == "0")
        {
            HttpContext.Current.Session["DEVELOPER"] = hsAdminLogin;
            //HttpContext.Current.Session["ADMIN_GSCOUNID"] = null;           
            if (Redirect)
            {
                HttpContext.Current.Response.Redirect("/CPanel/Dashboard1.aspx?DEVELOPER=Y");
            }
        }
        else
        {
            if (clsCommon.IsPortalUnderMaintenance() == "Y")
            {
                HttpContext.Current.Response.Redirect("/CPanel/SiteUnderMaintenance.aspx");
            }

            HttpContext.Current.Session["ADMIN"] = hsAdminLogin;
            //if (GsCounAdminID != "")
            //{
            //    HttpContext.Current.Session["ADMIN_GSCOUNID"] = GsCounAdminID;
            //}
            //else
            //{
            //    HttpContext.Current.Session["ADMIN_GSCOUNID"] = null;
            //}

            if (CurrentLoggedInAdminId != "")
            {
                HttpContext.Current.Session["PreviousLoggedInAdminId"] = CurrentLoggedInAdminId;
            }
            if (CurrentAdminAccessLevel != "")
            {
                HttpContext.Current.Session["PreviousAdminAccessLevel"] = CurrentAdminAccessLevel;
            }

            if (Redirect)
            {
                HttpContext.Current.Response.Redirect("/CPanel/ChangeAdminLogin.aspx");
            }
        }
    }

    public static bool IsSuperORUpperAdmin(string AdminID)
    {
        int cnt = 0;
        string ErrMsg = "";

        cnt = Convert.ToInt16(clsDatabase.GetSingleValue(string.Format(@" SELECT  count(AdminID) Cnt FROM AdminMast WHERE AdminId = N'{0}' and Accesslevel in (1,2)", AdminID), ref ErrMsg));

        if (cnt > 0)
            return true;
        else
            return false;
    }

    public clsCommon()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}