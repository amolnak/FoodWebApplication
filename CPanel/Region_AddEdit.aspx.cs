using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class CPanel_Region_AddEdit : System.Web.UI.Page
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
            hfRegionID.Value = "";
            lblTitle.InnerHtml = "<strong> Region - Add </strong>";

            clsCommon.FillCombo(ref ddlCity, "SELECT CityID,CityName FROM City where Active <>'N' ORDER BY CityName ASC", "CityName", "CityID", "Select city", ref strMsg);
            if (strMsg != "")
            {
                clsCommon.ErrorAlertBox(strMsg);
                return;
            }

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfRegionID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Region - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM Region where RegionID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtRegionName.Text = Dt.Rows[0]["RegionName"].ToString().Trim();
                            txtPincode.Text = Dt.Rows[0]["Pincode"].ToString().Trim();
                            ddlCity.SelectedValue = Dt.Rows[0]["CityID"].ToString().Trim();
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
         
        int iRegionNameCnt = 0;
        value = clsDatabase.GetSingleValue("SELECT COUNT(0) FROM Region WHERE RegionName=N'" + clsCommon.Remove_SQLInjection(txtRegionName.Text.Trim()) + "'" + (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" ? " and RegionID <>'" + Request.QueryString["ID"].ToString() + "'" : "") + ";", ref strMsg);
        if (strMsg != "")
        {
            clsCommon.ErrorAlertBox(strMsg);
            return;
        }
        iRegionNameCnt = clsCommon.ParseInteger(value);
        if (iRegionNameCnt > 0)
        {
            dvError.Style.Add("display", "");
            dvError.InnerText = "Region Name already exists.";
            return;
        }

        #endregion CheckDuplicateValues

        string sQry = "";
        string sRegionID = "";

        if ((hfRegionID.Value.Trim().Length > 0) && (hfRegionID.Value != "0"))
        { sRegionID = hfRegionID.Value.Trim(); }
        else { sRegionID = Guid.NewGuid().ToString(); }

        if ((hfRegionID.Value.Trim().Length > 0) && (hfRegionID.Value != "0"))
        {
            sQry = string.Format("UPDATE Region SET RegionName={0}, Pincode={1}, Active={2}, CityID={3} WHERE RegionID={4}; ",
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtRegionName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPincode.Text.Trim())),
                   (rdbActive.Checked ? "'Y'" : "'N'"), clsCommon.sQuote(ddlCity.SelectedValue), clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfRegionID.Value.Trim())));
        }
        else
        {
            sQry = string.Format("INSERT INTO Region(RegionID,RegionName,Pincode,CityID,Active) VALUES({0},{1},{2},{3},{4}); ",
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(sRegionID.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtRegionName.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPincode.Text.Trim())), clsCommon.sQuote(ddlCity.SelectedValue), (rdbActive.Checked ? "'Y'" : "'N'"));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfRegionID.Value.Trim().Length > 0) && (hfRegionID.Value != "0"))
                clsCommon.SuccessAlertBox("Region updated successfully.", "RegionList.aspx");
            else
                clsCommon.SuccessAlertBox("Region added successfully.", "RegionList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occurred while adding/Updating Region.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtRegionName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter region name.";
            txtRegionName.Focus();
            return;
        }

        if (txtPincode.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter pincode.";
            txtPincode.Focus();
            return;
        }

        if (ddlCity.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter city.";
            ddlCity.Focus();
            return;
        }
    }
}