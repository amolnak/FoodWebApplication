using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class CPanel_Admin_AddEdit : System.Web.UI.Page
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

            hfAdminID.Value = "";
            lblTitle.InnerHtml = "<strong> Admin - Add </strong>";
            dvResetPwd.Style.Add("display", "none");

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    dvResetPwd.Style.Add("display", "");
                    hrefResetPwd.HRef = "ResetPassword.aspx?ID=" + Request.QueryString["ID"].ToString();
                    hfAdminID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Admin - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM ADMINMast where AdminID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtName.Text = Dt.Rows[0]["Name"].ToString().Trim();
                            txtEmailID.Text = Dt.Rows[0]["EmailID"].ToString().Trim();
                            txtUserName.Text = Dt.Rows[0]["UserName"].ToString().Trim();

                            txtPassword.Attributes.Add("value", Dt.Rows[0]["Password"].ToString().Trim());
                            txtCPassword.Attributes.Add("value", Dt.Rows[0]["Password"].ToString().Trim());
                            ddlCity.SelectedValue = Dt.Rows[0]["CityID"].ToString().Trim();

                            if (ddlAdminType.Items.FindByValue(Dt.Rows[0]["AccessLevel"].ToString().Trim()) != null)
                            {
                                ddlAdminType.SelectedValue = Dt.Rows[0]["AccessLevel"].ToString().Trim();
                            }

                            ddlAdminType.SelectedValue = Dt.Rows[0]["AccessLevel"].ToString().Trim();
                            rdbActive.Checked = (Dt.Rows[0]["Active"].ToString().Trim() == "Y" ? true : false);
                            rdbInActive.Checked = (Dt.Rows[0]["Active"].ToString().Trim() == "N" ? true : false);

                            dvPwd.Style.Add("display", "none");
                            dvCPwd.Style.Add("display", "none");
                            dvAdminType.Style.Add("display", "");
                            regexp_NewPwd.Enabled = false;
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
        int iUserNameCnt = 0;
        string strMsg = "";
        string value = clsDatabase.GetSingleValue("SELECT COUNT(0) FROM AdminMast WHERE UserName=N'" + clsCommon.Remove_SQLInjection(txtUserName.Text.Trim()) + "'" + (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" ? " and AdminId<>'" + Request.QueryString["ID"].ToString() + "'" : "") + ";", ref strMsg);
        if (strMsg != "")
        {
            clsCommon.ErrorAlertBox(strMsg);
            return;
        }
        iUserNameCnt = clsCommon.ParseInteger(value);
        if (iUserNameCnt > 0)
        {
            dvError.Style.Add("display", "");
            dvError.InnerText = "Username already exists.";
            return;
        }

        string sQry = "";
        string sAdminID = "";
        string sAdminType = "";

        if ((hfAdminID.Value.Trim().Length > 0) && (hfAdminID.Value != "0"))
        { sAdminID = hfAdminID.Value.Trim(); }
        else { sAdminID = Guid.NewGuid().ToString(); }

        sAdminType = ddlAdminType.SelectedValue;

        if ((hfAdminID.Value.Trim().Length > 0) && (hfAdminID.Value != "0"))
        {
            sQry = string.Format("UPDATE AdminMast SET Name={0}, EmailID={1}, UserName={2}, Active={3}, CityID={5}, AccessLevel={6}  WHERE AdminId={4}; ",
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtName.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtEmailID.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtUserName.Text.Trim())),
                        (rdbActive.Checked ? "'Y'" : "'N'"),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfAdminID.Value.Trim())),
                        clsCommon.sQuote(ddlCity.SelectedValue.Trim()), sAdminType.Trim());
        }
        else
        {
            sQry = string.Format("INSERT INTO AdminMast(AdminId,Cityid,Name,EmailID,UserName,Password,AccessLevel,Active) VALUES({0},{1},{2},{3},{4},{5},{6},{7}); ",
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(sAdminID.Trim())), clsCommon.sQuote(ddlCity.SelectedValue.Trim()), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtName.Text.Trim())),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(txtEmailID.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtUserName.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtPassword.Text.Trim())), sAdminType.Trim(), (rdbActive.Checked ? "'Y'" : "'N'"));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfAdminID.Value.Trim().Length > 0) && (hfAdminID.Value != "0"))
                clsCommon.SuccessAlertBox("Admin updated successfully.", "AdminList.aspx");
            else
                clsCommon.SuccessAlertBox("Admin added successfully.", "AdminList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occured while adding/Updating admin.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter name.";
            txtName.Focus();
            return;
        }

        if (txtEmailID.Text.Trim() == string.Empty || (!Regex.IsMatch(txtEmailID.Text.Trim(), @"/\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/")))
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter valid email.";
            txtEmailID.Focus();
            return;
        }

        if (txtUserName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter username.";
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
    }
}