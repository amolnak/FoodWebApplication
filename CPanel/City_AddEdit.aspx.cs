using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class CPanel_City_AddEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsCommon.AdminAuthentication();
        #region Set the side menu in collapsed mode
        HtmlGenericControl Mstrbody = (HtmlGenericControl)Master.FindControl("Mstrbody");  /* body tag of CPanel Master Page */
        HtmlGenericControl smenuUI = (HtmlGenericControl)Master.FindControl("smenuUI");  /* ul of side menu in Master Page */

        Mstrbody.Attributes.Add("class", "sidebar-mini fixed animated fadeIn sidebar-collapse");
        smenuUI.Attributes.Add("class", "sidebar-menu");
        #endregion

        string strMsg = "";
        if (!Page.IsPostBack)
        {
            dvError.Style.Add("display", "none");
            hfCityID.Value = "";
            lblTitle.InnerHtml = "<strong> City - Add </strong>";

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfCityID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> City - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM City where CityID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtCityCode.Text = Dt.Rows[0]["CityCode"].ToString().Trim();
                            txtCityName.Text = Dt.Rows[0]["CityName"].ToString().Trim();
                            rdbActive.Checked = (Dt.Rows[0]["Active"].ToString().Trim() == "Y" ? true : false);
                            rdbInActive.Checked = (Dt.Rows[0]["Active"].ToString().Trim() == "N" ? true : false);
                        }
                    }
                }
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        string strMsg = "", value = "";

        #region CheckDuplicateValues

        int iCityCodeCnt = 0;

        value = clsDatabase.GetSingleValue("SELECT COUNT(0) FROM City WHERE CityCode=N'" + clsCommon.Remove_SQLInjection(txtCityCode.Text.Trim()) + "'" + (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" ? " and CityID <>'" + Request.QueryString["ID"].ToString() + "'" : "") + ";", ref strMsg);
        if (strMsg != "")
        {
            clsCommon.ErrorAlertBox(strMsg);
            return;
        }
        iCityCodeCnt = clsCommon.ParseInteger(value);
        if (iCityCodeCnt > 0)
        {
            dvError.Style.Add("display", "");
            dvError.InnerText = "City Code already exists.";
            return;
        }

        int iCityNameCnt = 0;
        value = clsDatabase.GetSingleValue("SELECT COUNT(0) FROM City WHERE CityName=N'" + clsCommon.Remove_SQLInjection(txtCityName.Text.Trim()) + "'" + (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" ? " and CityID <>'" + Request.QueryString["ID"].ToString() + "'" : "") + ";", ref strMsg);
        if (strMsg != "")
        {
            clsCommon.ErrorAlertBox(strMsg);
            return;
        }
        iCityNameCnt = clsCommon.ParseInteger(value);
        if (iCityNameCnt > 0)
        {
            dvError.Style.Add("display", "");
            dvError.InnerText = "City Name already exists.";
            return;
        }

        #endregion CheckDuplicateValues

        string sQry = "";
        string sCityID = "";

        if ((hfCityID.Value.Trim().Length > 0) && (hfCityID.Value != "0"))
        { sCityID = hfCityID.Value.Trim(); }
        else { sCityID = Guid.NewGuid().ToString(); }

        if ((hfCityID.Value.Trim().Length > 0) && (hfCityID.Value != "0"))
        {
            sQry = string.Format("UPDATE City SET CityName={0}, CityCode={1}, Active={2} WHERE CityID={3}; ",
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtCityName.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtCityCode.Text.Trim())),
                        (rdbActive.Checked ? "'Y'" : "'N'"), clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfCityID.Value.Trim())));
        }
        else
        {
            sQry = string.Format("INSERT INTO City(CityID,CityName,CityCode,Active) VALUES({0},{1},{2},{3}); ",
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(sCityID.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtCityName.Text.Trim())),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(txtCityCode.Text.Trim())), (rdbActive.Checked ? "'Y'" : "'N'"));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfCityID.Value.Trim().Length > 0) && (hfCityID.Value != "0"))
                clsCommon.SuccessAlertBox("City updated successfully.", "CityList.aspx");
            else
                clsCommon.SuccessAlertBox("City added successfully.", "CityList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occured while adding/Updaing City.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtCityCode.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter city code.";
            txtCityCode.Focus();
            return;
        }
             
        if (txtCityName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter city name.";
            txtCityName.Focus();
            return;
        }         
    }
}