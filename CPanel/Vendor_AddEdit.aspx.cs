using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class CPanel_Vendor_AddEdit : System.Web.UI.Page
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

            hfVendorID.Value = "";
            lblTitle.InnerHtml = "<strong> Vendor - Add </strong>";

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfVendorID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Vendor - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM Vendors where VendorID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtVendorCode.Text = Dt.Rows[0]["VendorCode"].ToString().Trim();
                            txtVendorFName.Text = Dt.Rows[0]["VendorFName"].ToString().Trim();
                            txtVendorLName.Text = Dt.Rows[0]["VendorLName"].ToString().Trim();
                            txtUserName.Text = Dt.Rows[0]["UserName"].ToString().Trim();
                            txtPassword.Text = Dt.Rows[0]["Password"].ToString().Trim();
                            txtVendorAddress.Text = Dt.Rows[0]["VendorAddress"].ToString().Trim();
                            txtVendorEmail.Text = Dt.Rows[0]["VendorEmail"].ToString().Trim();
                            txtVendorPhone1.Text = Dt.Rows[0]["VendorPhone1"].ToString().Trim();
                            txtVendorPhone2.Text = Dt.Rows[0]["VendorPhone2"].ToString().Trim();
                            ddlCity.SelectedValue = Dt.Rows[0]["CityID"].ToString().Trim();
                            FillRegion(Dt.Rows[0]["CityID"].ToString().Trim());
                            ddlRegion.SelectedValue = Dt.Rows[0]["RegionID"].ToString().Trim();
                            rdbYes.Checked = (Dt.Rows[0]["DeliveryProvision"].ToString().Trim() == "Y" ? true : false);
                            rdbNo.Checked = (Dt.Rows[0]["DeliveryProvision"].ToString().Trim() == "N" ? true : false);
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
        string sVendorID = "";

        if ((hfVendorID.Value.Trim().Length > 0) && (hfVendorID.Value != "0"))
        { sVendorID = hfVendorID.Value.Trim(); }
        else { sVendorID = Guid.NewGuid().ToString(); }

        if ((hfVendorID.Value.Trim().Length > 0) && (hfVendorID.Value != "0"))
        {
            sQry = string.Format(@"UPDATE Vendors SET VendorFName={0}, VendorLName ={1}, UserName={2}, Password ={3},VendorAddress={4},VendorEmail={5},VendorPhone1 ={6},
                   VendorPhone2={7},CityID={8},RegionID={9},DeliveryProvision={10},Active ={11},VendorCode= {12}, LocationMap = {13} WHERE DeliveryBoyID = {14}; ",
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorFName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorLName.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtUserName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPassword.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorAddress.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorEmail.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorPhone1.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorPhone2.Text.Trim())),
                   clsCommon.sQuote(ddlCity.SelectedValue), clsCommon.sQuote(ddlRegion.SelectedValue), (rdbYes.Checked ? "'Y'" : "'N'"), (rdbActive.Checked ? "'Y'" : "'N'"),
                   clsCommon.sQuote(clsCommon.Remove_SQLInjection(txtVendorCode.Text.Trim())), clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfVendorID.Value.Trim())));
        }
        else
        {
            sQry = string.Format(@"INSERT INTO Vendors(VendorID,VendorCode,UserName,Password,VendorFName,VendorLName,VendorEmail,VendorPhone1,VendorPhone2,VendorAddress
                   DeliveryProvision,CityID,RegionID,Active,LocationMAP) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},''); ", clsCommon.sQuote(sVendorID.Trim()),
                   clsCommon.sQuote(clsCommon.Remove_SQLInjection(txtVendorCode.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtUserName.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPassword.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorFName.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorLName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorEmail.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorPhone1.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorPhone2.Text.Trim())),
                   clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVendorAddress.Text.Trim())), (rdbYes.Checked ? "'Y'" : "'N'"), clsCommon.sQuote(ddlCity.SelectedValue),
                   clsCommon.sQuote(ddlRegion.SelectedValue), (rdbActive.Checked ? "'Y'" : "'N'"));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfVendorID.Value.Trim().Length > 0) && (hfVendorID.Value != "0"))
                clsCommon.SuccessAlertBox("Vendor updated successfully.", "VendorList.aspx");
            else
                clsCommon.SuccessAlertBox("Vendor added successfully.", "VendorList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occured while adding/Updating Vendor.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtVendorCode.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter vendor code.";
            txtVendorCode.Focus();
            return;
        }

        if (txtVendorFName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter first name.";
            txtVendorFName.Focus();
            return;
        }

        if (txtVendorLName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter last name.";
            txtVendorLName.Focus();
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

        if (txtVendorAddress.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter address.";
            txtVendorAddress.Focus();
            return;
        }

        if (txtVendorEmail.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter email.";
            txtVendorEmail.Focus();
            return;
        }

        if (txtVendorPhone1.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter phone number.";
            txtVendorPhone1.Focus();
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