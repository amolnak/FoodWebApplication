using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class CPanel_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsCommon.AdminAuthentication();

        if (!Page.IsPostBack)
        {
            if (HttpContext.Current.Request.QueryString["WTIL"] != null && HttpContext.Current.Request.QueryString["WTIL"] == "Y")
            {
                hreflogout.HRef = "/CPanel/Logout.aspx?WTIL=Y";
            }
            LoadMenu(clsCommon.GetSessionKeyValue("AccessLevel"));
            spnAdminName.InnerHtml = clsCommon.GetSessionKeyValue("AName");
            hrefDashboard.HRef = clsCommon.SiteURLSub("/CPanel/Dashboard1.aspx");
            hrefDBGulLogo.HRef = clsCommon.SiteURLSub("/CPanel/Dashboard1.aspx");
        }

        string ErrMsg = "", strSQL = "", strAdminID = "";

        if (Convert.ToInt16(clsCommon.GetSessionKeyValue("AccessLevel")) < 3)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["PreviousLoggedInAdminId"])))
            {
                strSQL = string.Format(@"SELECT * FROM ADMINMast WHERE AdminID = {0} UNION  SELECT * FROM ADMINMast WHERE UpperAdminID ={0} OR AdminID = {0}
                    Order by Accesslevel", clsCommon.sQuote(clsCommon.GetSessionKeyValue("AdminID")));
            }
            else
            {
                strSQL = string.Format(@"SELECT * FROM ADMINMast WHERE AdminID = {0} UNION SELECT * FROM ADMINMast WHERE UpperAdminID = {0} OR AdminID = {1}
                    Order by Accesslevel", clsCommon.sQuote(clsCommon.GetSessionKeyValue("AdminID")), clsCommon.sQuote(Convert.ToString(Session["PreviousLoggedInAdminId"])));
            }
        }
        else
        {
            strSQL = string.Format(@"SELECT * FROM ADMINMast WHERE AdminID = {0} UNION SELECT * FROM ADMINMast WHERE  AdminID = (select UpperAdminID from ADMINMast where 
                    AdminID =  {0}) OR AdminID = {1}", clsCommon.sQuote(clsCommon.GetSessionKeyValue("AdminID")), clsCommon.sQuote(Convert.ToString(Session["PreviousLoggedInAdminId"])));

        }

        DataTable DtGS = clsDatabase.GetDT(strSQL, ref ErrMsg);

        if (DtGS != null && DtGS.Rows.Count > 0 && Int32.Parse(clsCommon.GetSessionKeyValue("AccessLevel")) > 0)
        {
            StringBuilder sbAdmins = new StringBuilder();

            foreach (DataRow drgs in DtGS.Rows)
            {
                if (drgs["AdminID"].ToString() != clsCommon.GetSessionKeyValue("AdminID"))
                    sbAdmins.Append("<li><a href='ChangeAdminLogin.aspx?AdminID=" + drgs["AdminID"].ToString() + "'>" + drgs["Name"].ToString() + "</a></li>");
            };
            ltrAdmins.Text = sbAdmins.ToString();
        }
    }

    private void LoadMenu(string sAccessLevel)
    {
        StringBuilder sbMenu = new StringBuilder();
        string ErrMsg = "";
        if (sAccessLevel == "0")
        {
            #region Load menu for AccessLevel 0 - Developer
            using (DataTable Dt = clsDatabase.GetDT("SELECT C.AccessLevel, C.CPPageId, C.Title, C.Section,C.ParentPage,C.PageURL,C.PageType FROM CPanelPages AS C WHERE AccessLevel=0 and C.ParentPage is null ORDER BY Section,ParentPage,Title", ref ErrMsg))
            {
                if (Dt.Rows.Count > 0)
                {
                    sbMenu.Append(LoadMenuParent(Dt, sAccessLevel));
                }
            }
            #endregion
        }
        else if (sAccessLevel == "1" || sAccessLevel == "2")
        {
            #region Load Menu for for AccessLevel 1 or 2
            // using (DataTable Dt = clsDatabase.GetDT("SELECT distinct	C.AccessLevel, A.AdminId,A.CCode,C.CPPageId, C.Title, C.Section,C.ParentPage,C.PageURL,C.PageType FROM AdminMast AS A left JOIN CPanelPages AS C ON C.AccessLevel = A.AccessLevel WHERE A.AdminId=" + clsCommon.sQuote(ManageLoginSession.GetSessionKeyValue("AdminID").ToString()) + " ORDER BY Section,ParentPage,Title"))            
            using (DataTable Dt = clsDatabase.GetDT("Select DISTINCT * from (SELECT distinct AccessLevel, " + clsCommon.sQuote(clsCommon.GetSessionKeyValue("AdminID").ToString()) + " as  AdminID, CPPageId, Title, Section,ParentPage,PageURL,PageType from CPanelPages WHERE  PageType = 'G' AND AccessLevel >= " + clsCommon.sQuote(clsCommon.GetSessionKeyValue("AccessLevel").ToString()) + " UNION  SELECT distinct C.AccessLevel, A.AdminId,C.CPPageId, C.Title, C.Section,C.ParentPage,C.PageURL,C.PageType from CPanelPages as C JOIN AdminPages as AP ON(AP.CPPageId = C.CPPageId) JOIN AdminMast as A ON(A.AdminId = AP.AdminId) WHERE PageType='C' AND C.AccessLevel >=" + clsCommon.sQuote(clsCommon.GetSessionKeyValue("AccessLevel").ToString()) + " AND A.AdminId = " + clsCommon.sQuote(clsCommon.GetSessionKeyValue("AdminID").ToString()) + ") MenuRecords ORDER BY MenuRecords.Section,MenuRecords.ParentPage,MenuRecords.Title", ref ErrMsg))
            {
                if (Dt.Rows.Count > 0)
                {
                    sbMenu.Append(LoadMenuParent(Dt, sAccessLevel));
                }
            }
            #endregion
        }
        else
        {
            #region Load Menu for for AccessLevel>=3
            using (DataTable Dt = clsDatabase.GetDT("SELECT distinct C.AccessLevel, A.AdminId,C.CPPageId, C.Title, C.Section,C.ParentPage,C.PageURL,C.PageType from CPanelPages as C JOIN AdminPages as AP ON(AP.CPPageId = C.CPPageId) JOIN AdminMast as A ON(A.AdminId = AP.AdminId) WHERE  C.AccessLevel >=" + clsCommon.sQuote(clsCommon.GetSessionKeyValue("AccessLevel").ToString()) + " AND A.AdminId = " + clsCommon.sQuote(clsCommon.GetSessionKeyValue("AdminID").ToString()) + " ORDER BY C.Section,C.ParentPage,C.Title", ref ErrMsg))
            {
                if (Dt.Rows.Count > 0)
                {
                    sbMenu.Append(LoadMenuParent(Dt, sAccessLevel));
                }
            }
            #endregion
        }
        if (ErrMsg != "")
            return;

        ltrMenu.Text = sbMenu.ToString();
    }

    private string LoadMenuParent(DataTable Dt, string sAccessLevel)
    {
        try
        {
            string ErrMsg = "";
            StringBuilder sbMenu = new StringBuilder();

            DataView view = new DataView(Dt);
            DataTable dtSections = view.ToTable(true, "Section");

            //string sFormNM = HttpContext.Current.Request.RawUrl;
            string sFormNM = HttpContext.Current.Request.FilePath;
            sFormNM = Regex.Replace(sFormNM, "/CPanel", "", RegexOptions.IgnoreCase);
            // sFormNM = Regex.Replace(sFormNM, "\\?" + Request.QueryString.ToString(), "", RegexOptions.IgnoreCase);

            string sSection = "";
            DataRow[] rst = Dt.Select("PageURL=" + clsCommon.sQuote(sFormNM.Trim()));
            if (rst.Length > 0)
            {
                sSection = rst[0]["Section"].ToString();
            }

            string sActiveFormID = clsDatabase.GetSingleValue("SELECT CPPageID from CPanelPages WHERE PageURL=" + clsCommon.sQuote(sFormNM.Trim()) + (sAccessLevel == "0" ? " and AccessLevel=0" : ""), ref ErrMsg);

            foreach (DataRow r in dtSections.Rows)
            {

                if (sSection.Trim().ToUpper() == r["Section"].ToString().Trim().ToUpper())
                    sbMenu.Append("<li id='mnu" + r["Section"].ToString().Trim().Replace(" ", "") + "' class='treeview active Activated'>");
                else
                    sbMenu.Append("<li id='mnu" + r["Section"].ToString().Trim().Replace(" ", "") + "' class='treeview'>");

                sbMenu.Append("<a href='#'><i class='fa " + GetMenuIcons(r["Section"].ToString().Trim()) + "'></i><span>" + r["Section"].ToString().Trim() + "</span><i class='fa fa-angle-down'></i></a>");

                sbMenu.Append(LoadSubMenu(Dt, r["Section"].ToString().Trim(), "", sActiveFormID, sSection));

                sbMenu.Append("</li>");
            }

            if (ErrMsg != "")
                return "";

            return sbMenu.ToString();
        }
        catch (Exception ex)
        {

            return "";
        }
    }

    private string LoadSubMenu(DataTable Dt, string sSection, string sParentPage, string sActiveForm, string sActiveSection)
    {
        DataRow[] rst;

        if (sParentPage == "")
            rst = Dt.Select("Section=" + clsCommon.sQuote(sSection.Trim()) + " and ParentPage is null ");
        else
            rst = Dt.Select("Section=" + clsCommon.sQuote(sSection.Trim()) + " and ParentPage=" + clsCommon.sQuote(sParentPage.Trim()));

        StringBuilder sbMenu = new StringBuilder();

        if (rst.Length > 0)
        {
            sbMenu.Append("<ul class='treeview-menu'>");

            foreach (DataRow r in rst)
            {
                DataRow[] rstSub = Dt.Select("Section=" + clsCommon.sQuote(sSection.Trim()) + " and ParentPage=" + clsCommon.sQuote(r["CPPageId"].ToString().Trim()));

                if (rstSub.Length > 0)
                    sbMenu.Append("<li id='li" + r["CPPageId"].ToString().Trim() + "' class='treeview' >");
                else
                {
                    if ((sActiveSection == sSection) && (sActiveForm == r["CPPageId"].ToString().Trim()))
                        sbMenu.Append("<li id='li" + r["CPPageId"].ToString().Trim() + "' class='Activated' >");
                    else
                        sbMenu.Append("<li id='li" + r["CPPageId"].ToString().Trim() + "' >");
                }

                if (sParentPage.Trim().Length > 0)
                    sbMenu.Append("<a href='" + clsCommon.SiteURLSub(r["PageURL"].ToString().Trim() != "" && r["PageURL"].ToString().Trim() != "#" ? "/CPanel" + r["PageURL"].ToString().Trim() : "#") + "?fid=" + clsEncryptDecryptQueryString.Encrypt(sParentPage) + "'><i class='fa fa-dot-circle-o'></i>" + r["Title"].ToString().Trim() + (rstSub.Length > 0 ? "<i class='fa fa-angle-down'></i>" : "") + "</a>");
                else
                    sbMenu.Append("<a href='" + clsCommon.SiteURLSub(r["PageURL"].ToString().Trim() != "" && r["PageURL"].ToString().Trim() != "#" ? "/CPanel" + r["PageURL"].ToString().Trim() : "#") + "'><i class='fa fa-dot-circle-o'></i><span>" + r["Title"].ToString().Trim() + (rstSub.Length > 0 ? "</span><i class='fa fa-angle-down updnlink'></i>" : "") + "</a>");

                if (rstSub.Length > 0)
                {
                    sbMenu.Append(LoadSubMenu(Dt, r["Section"].ToString().Trim(), r["CPPageId"].ToString().Trim(), sActiveForm, sActiveSection));
                }

                sbMenu.Append("</li>");
            }

            sbMenu.Append("</ul>");
        }

        return sbMenu.ToString();
    }

    private string GetMenuIcons(string sMenuSection)
    {
        string rtnIcon = "";
        switch (sMenuSection.ToUpper())
        {
            case "SUPPORT":
                rtnIcon = "fa-ticket";
                break;
            case "ADMIN TASKS":
                rtnIcon = "fa-tasks";
                break;
            case "BSM":
                rtnIcon = "fa-book";
                break;

            case "CEP MANAGEMENT":
                rtnIcon = "fa-file-o";
                break;

            case "REPORTS":
                rtnIcon = "fa-file-text";
                break;

            case "SETTINGS":
                rtnIcon = "fa-cog";
                break;

            case "USER MANAGEMENT":
                rtnIcon = "fa-user";
                break;

            case "SCRATCH CARDS":
                rtnIcon = "fa-credit-card";
                break;

            case "MEMBERSHIP":
                rtnIcon = "fa-star";
                break;

            default:
                rtnIcon = "fa-database";
                break;

        }

        return rtnIcon;
    }
}
