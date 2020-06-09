using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class CPanel_DeliveryBoy_AddEdit : System.Web.UI.Page
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

            clsCommon.FillCombo(ref ddlVehicle, "SELECT VehicleID,VehicleNo FROM DeliveryVehicles ORDER BY VehicleNo ASC", "VehicleNo", "VehicleID", "Select vehicle", ref strMsg);
            if (strMsg != "")
            {
                clsCommon.ErrorAlertBox(strMsg);
                return;
            }

            FillRegion("");

            hfDeliveryBoyID.Value = "";
            lblTitle.InnerHtml = "<strong> Delivery Boy - Add </strong>";

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfDeliveryBoyID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Delivery Boy - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM DeliveryBoys where DeliveryBoyID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtFName.Text = Dt.Rows[0]["DBFName"].ToString().Trim();
                            txtLName.Text = Dt.Rows[0]["DBLName"].ToString().Trim();
                            txtUserName.Text = Dt.Rows[0]["DBUserName"].ToString().Trim();
                            txtPassword.Text = Dt.Rows[0]["DBPassword"].ToString().Trim();
                            txtAddress.Text = Dt.Rows[0]["DBAddress"].ToString().Trim();
                            txtEmail.Text = Dt.Rows[0]["DBEmail"].ToString().Trim();
                            txtPhone1.Text = Dt.Rows[0]["DBPhone1"].ToString().Trim();
                            txtPhone2.Text = Dt.Rows[0]["DBPhone2"].ToString().Trim();
                            ddlCity.SelectedValue = Dt.Rows[0]["DBCityID"].ToString().Trim();
                            FillRegion(Dt.Rows[0]["DBCityID"].ToString().Trim());
                            ddlRegion.SelectedValue = Dt.Rows[0]["DBRegionID"].ToString().Trim();
                            txtLicenseNo.Text = Dt.Rows[0]["LicenseNo"].ToString().Trim();
                            ddlVehicle.SelectedValue = Dt.Rows[0]["VehicleID"].ToString().Trim();
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
            strSql = string.Format(@"SELECT RegionID,RegionName FROM Region  where CityID={0} ORDER BY RegionName ASC", clsCommon.sQuote(CityID));

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

        if ((hfDeliveryBoyID.Value.Trim().Length > 0) && (hfDeliveryBoyID.Value != "0"))
        { sDeliveryBoyID = hfDeliveryBoyID.Value.Trim(); }
        else { sDeliveryBoyID = Guid.NewGuid().ToString(); }

        if ((hfDeliveryBoyID.Value.Trim().Length > 0) && (hfDeliveryBoyID.Value != "0"))
        {
            sQry = string.Format(@"UPDATE DeliveryBoys SET DBFName={0}, DBLName ={1}, DBUserName={2}, DBPassword ={3},DBAddress={4},DBEmail={5},
                    DBPhone1 ={6},DBPhone2={7},DBCityID={8},DBRegionID={9},LicenseNo={10},VehicleID ={11} WHERE DeliveryBoyID = {12}; ",
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtFName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtLName.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtUserName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPassword.Text.Trim())),
                          clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtAddress.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtEmail.Text.Trim())),
                            clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPhone1.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPhone2.Text.Trim())),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlCity.SelectedValue)), clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlRegion.SelectedValue)),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtLicenseNo.Text.Trim())), clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlVehicle.SelectedValue)),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfDeliveryBoyID.Value.Trim())));
        }
        else
        {
            sQry = string.Format(@"INSERT INTO DeliveryBoys(DeliveryBoyID,DBFName,DBLName,DBUserName,DBPassword,DBAddress,DBEmail,DBPhone1,DBPhone2,DBCityID,DBRegionID,LicenseNo,VehicleID)
                           VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}); ", clsCommon.sQuote(sDeliveryBoyID.Trim()),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtFName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtLName.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtUserName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPassword.Text.Trim())),
                          clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtAddress.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtEmail.Text.Trim())),
                            clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPhone1.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPhone2.Text.Trim())),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlCity.SelectedValue)), clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlRegion.SelectedValue)),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtLicenseNo.Text.Trim())), clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlVehicle.SelectedValue)));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfDeliveryBoyID.Value.Trim().Length > 0) && (hfDeliveryBoyID.Value != "0"))
                clsCommon.SuccessAlertBox("Delivery boy updated successfully.", "DeliveryBoysList.aspx");
            else
                clsCommon.SuccessAlertBox("Delivery boy added successfully.", "DeliveryBoysList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occurred while adding/Updating delivery boy.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtFName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter first name.";
            txtFName.Focus();
            return;
        }

        if (txtLName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter last name.";
            txtLName.Focus();
            return;
        }

        if (txtUserName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter user name.";
            txtUserName.Focus();
            return;
        }

        if (txtPassword.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter password.";
            txtPassword.Focus();
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

        if (txtEmail.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter email.";
            txtEmail.Focus();
            return;
        }

        if (txtPhone1.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter phone number.";
            txtPhone1.Focus();
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

        if (ddlRegion.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter region.";
            ddlRegion.Focus();
            return;
        }

        if (txtLicenseNo.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter license no.";
            txtLicenseNo.Focus();
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