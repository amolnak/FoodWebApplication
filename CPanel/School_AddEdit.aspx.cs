using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class CPanel_School_AddEdit : System.Web.UI.Page
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
            clsCommon.FillCombo(ref ddlCity, "SELECT CityID,CityName FROM City where Active <>'N' ORDER BY CityName ASC", "CityName", "CityID", "Select city", ref strMsg);
            if (strMsg != "")
            {
                clsCommon.ErrorAlertBox(strMsg);
                return;
            }

            FillRegion("");

            hfSchoolID.Value = "";
            lblTitle.InnerHtml = "<strong> School - Add </strong>";

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfSchoolID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> School - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM Schools where SchoolID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtSchoolName.Text = Dt.Rows[0]["SchoolName"].ToString().Trim();
                            txtAddress.Text = Dt.Rows[0]["Address"].ToString().Trim();
                            txtEmailID.Text = Dt.Rows[0]["EmailID"].ToString().Trim();
                            txtPhone1.Text = Dt.Rows[0]["Phone1"].ToString().Trim();
                            txtPhone2.Text = Dt.Rows[0]["Phone2"].ToString().Trim();
                            ddlCity.SelectedValue = Dt.Rows[0]["CityID"].ToString().Trim();
                            FillRegion(Dt.Rows[0]["CityID"].ToString().Trim());
                            ddlRegion.SelectedValue = Dt.Rows[0]["RegionID"].ToString().Trim();
                            txtNoofSubscribers.Text = Dt.Rows[0]["NoofSubscribers"].ToString().Trim();
                            rdbActive.Checked = (Dt.Rows[0]["Active"].ToString().Trim() == "Y" ? true : false);
                            rdbInActive.Checked = (Dt.Rows[0]["Active"].ToString().Trim() == "N" ? true : false);
                        }
                    }
                }
            }
        }
    }

    public void FillRegion(string CityID)
    {
        DataTable dtRegion = new DataTable();
        string strSql = "", ErrMsg = "";

        if (CityID != string.Empty)
            strSql = string.Format(@"SELECT RegionID, RegionName FROM Region where CityID = {0} ORDER BY RegionName ASC", clsCommon.sQuote(CityID));

        if (strSql != string.Empty)
        {
            dtRegion = clsDatabase.GetDT(strSql, ref ErrMsg);
            if (dtRegion != null)
            {
                if (dtRegion.Rows.Count > 0)
                {
                    dvRegion.Visible = true;
                    ddlRegion.DataSource = dtRegion;
                    ddlRegion.DataTextField = "RegionName";
                    ddlRegion.DataValueField = "RegionID";
                    ddlRegion.DataBind();
                    ddlRegion.Items.Insert(0, new ListItem("Select Region", ""));
                }
                else
                    dvRegion.Visible = false;
            }
            else
                dvRegion.Visible = false;
        }
        else
            dvRegion.Visible = false;

        if (ErrMsg != "")
        {
            clsCommon.ErrorAlertBox(ErrMsg);
            return;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        string sQry = "";
        string sDeliveryBoyID = "";

        if ((hfSchoolID.Value.Trim().Length > 0) && (hfSchoolID.Value != "0"))
        { sDeliveryBoyID = hfSchoolID.Value.Trim(); }
        else { sDeliveryBoyID = Guid.NewGuid().ToString(); }

        if ((hfSchoolID.Value.Trim().Length > 0) && (hfSchoolID.Value != "0"))
        {
            sQry = string.Format(@"UPDATE Schools SET SchoolName = {0}, Address = {1}, EmailID = {2}, Phone1 = {3}, Phone2 = {4}, CityID = {5}, RegionID = {6},
                    Active = {7} WHERE SchoolID = {8}; ", clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtSchoolName.Text.Trim())),
                    clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtAddress.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtEmailID.Text.Trim())),
                    clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPhone1.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPhone2.Text.Trim())),
                    clsCommon.sQuote(ddlCity.SelectedValue), clsCommon.sQuote(ddlRegion.SelectedValue), (rdbActive.Checked ? "'Y'" : "'N'"),
                    clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfSchoolID.Value.Trim())));
        }
        else
        {
            sQry = string.Format(@"INSERT INTO Schools(SchoolID,SchoolName,Address,EmailID,Phone1,Phone2,CityID,RegionID,Active,LocationMAP)
                   VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},''); ", clsCommon.sQuote_N(sDeliveryBoyID), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtSchoolName.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtAddress.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtEmailID.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPhone1.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPhone2.Text.Trim())),
                   clsCommon.sQuote(ddlCity.SelectedValue), clsCommon.sQuote(ddlRegion.SelectedValue), (rdbActive.Checked ? "'Y'" : "'N'"));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfSchoolID.Value.Trim().Length > 0) && (hfSchoolID.Value != "0"))
                clsCommon.SuccessAlertBox("School updated successfully.", "SchoolList.aspx");
            else
                clsCommon.SuccessAlertBox("School added successfully.", "SchoolList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occurred while adding/Updating school.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtSchoolName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter school name.";
            txtSchoolName.Focus();
            return;
        }

        if (txtAddress.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter address.";
            txtAddress.Focus();
            return;
        }

        if (txtEmailID.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter email id.";
            txtEmailID.Focus();
            return;
        }
        if (txtPhone1.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter phone.";
            txtPhone1.Focus();
            return;
        }
        if (ddlCity.SelectedValue == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please select city name.";
            ddlCity.Focus();
            return;
        }
        if (ddlRegion.SelectedValue == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter region name.";
            ddlRegion.Focus();
            return;
        }
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity.SelectedIndex > 0)
        {
            dvRegion.Visible = true;
            FillRegion(ddlCity.SelectedValue);
        }
    }
}