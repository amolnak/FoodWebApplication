using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class CPanel_DeliveryVehicle_AddEdit : System.Web.UI.Page
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
            hfVehicleID.Value = "";
            lblTitle.InnerHtml = "<strong> Delivery Vehicle - Add </strong>";

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfVehicleID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Delivery Vehicle - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM DeliveryVehicles where VehicleID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtVehicleNo.Text = Dt.Rows[0]["VehicleNo"].ToString().Trim();
                            txtBrand.Text = Dt.Rows[0]["Brand"].ToString().Trim();
                            txtModel.Text = Dt.Rows[0]["Model"].ToString().Trim();
                            txtChasisNo.Text = Dt.Rows[0]["ChasisNo"].ToString().Trim();
                            txtFuelType.Text = Dt.Rows[0]["FuelType"].ToString().Trim();
                            rdbYes.Checked = (Dt.Rows[0]["GPSEnabled"].ToString().Trim() == "Y" ? true : false);
                            rdbNo.Checked = (Dt.Rows[0]["GPSEnabled"].ToString().Trim() == "N" ? true : false);
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

        int iVehicleNoCnt = 0;

        value = clsDatabase.GetSingleValue("SELECT COUNT(0) FROM DeliveryVehicles WHERE VehicleNo=N'" + clsCommon.Remove_SQLInjection(txtVehicleNo.Text.Trim()) + "'" + (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" ? " and VehicleID <>'" + Request.QueryString["ID"].ToString() + "'" : "") + ";", ref strMsg);
        if (strMsg != "")
        {
            clsCommon.ErrorAlertBox(strMsg);
            return;
        }
        iVehicleNoCnt = clsCommon.ParseInteger(value);
        if (iVehicleNoCnt > 0)
        {
            dvError.Style.Add("display", "");
            dvError.InnerText = "Vehicle No already exists.";
            return;
        }

        int iChasisNoCnt = 0;
        value = clsDatabase.GetSingleValue("SELECT COUNT(0) FROM DeliveryVehicles WHERE ChasisNo=N'" + clsCommon.Remove_SQLInjection(txtChasisNo.Text.Trim()) + "'" + (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" ? " and VehicleID <>'" + Request.QueryString["ID"].ToString() + "'" : "") + ";", ref strMsg);
        if (strMsg != "")
        {
            clsCommon.ErrorAlertBox(strMsg);
            return;
        }
        iChasisNoCnt = clsCommon.ParseInteger(value);
        if (iChasisNoCnt > 0)
        {
            dvError.Style.Add("display", "");
            dvError.InnerText = "Chasis No already exists.";
            return;
        }

        #endregion CheckDuplicateValues

        string sQry = "";
        string sVehicleID = "";

        if ((hfVehicleID.Value.Trim().Length > 0) && (hfVehicleID.Value != "0"))
        { sVehicleID = hfVehicleID.Value.Trim(); }
        else { sVehicleID = Guid.NewGuid().ToString(); }

        if ((hfVehicleID.Value.Trim().Length > 0) && (hfVehicleID.Value != "0"))
        {
            sQry = string.Format("UPDATE DeliveryVehicles SET VehicleNo={0}, Brand={1}, Model={2}, ChasisNo={3}, FuelType={4}, GPSEnabled={5} WHERE VehicleID={6}; ",
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVehicleNo.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtBrand.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtModel.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtChasisNo.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtFuelType.Text.Trim())),
                        (rdbYes.Checked ? "'Y'" : "'N'"), clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfVehicleID.Value.Trim())));
        }
        else
        {
            sQry = string.Format("INSERT INTO DeliveryVehicles(VehicleID,VehicleNo,Brand,Model,ChasisNo,FuelType,GPSEnabled) VALUES({0},{1},{2},{3},{4},{5},{6}); ",
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(sVehicleID.Trim())), 
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtVehicleNo.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtBrand.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtModel.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtChasisNo.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtFuelType.Text.Trim())),
                        (rdbYes.Checked ? "'Y'" : "'N'"));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfVehicleID.Value.Trim().Length > 0) && (hfVehicleID.Value != "0"))
                clsCommon.SuccessAlertBox("Delivery Vehicle updated successfully.", "DeliveryVehicleList.aspx");
            else
                clsCommon.SuccessAlertBox("Delivery Vehicle added successfully.", "DeliveryVehicleList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occurred while adding/Updating Delivery Vehicles.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtVehicleNo.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter vehicle no.";
            txtVehicleNo.Focus();
            return;
        }

        if (txtChasisNo.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter chasis no.";
            txtChasisNo.Focus();
            return;
        }
    }
}