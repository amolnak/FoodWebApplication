using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class CPanel_ItemAddOn_AddEdit : System.Web.UI.Page
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
            hfItemAddOnID.Value = "";
            lblTitle.InnerHtml = "<strong> Item AddOn - Add </strong>";

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfItemAddOnID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Item AddOn - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM ItemAddOns where ItemAddOnID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtAddOnName.Text = Dt.Rows[0]["AddOnName"].ToString().Trim();
                            txtAddOnPrice.Text = Dt.Rows[0]["AddOnPrice"].ToString().Trim();
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

        int iaddonCnt = 0;

        value = clsDatabase.GetSingleValue("SELECT COUNT(0) FROM ItemAddOns WHERE AddOnName=N'" + clsCommon.Remove_SQLInjection(txtAddOnName.Text.Trim()) + "'" + (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" ? " and ItemAddOnID <>'" + Request.QueryString["ID"].ToString() + "'" : "") + ";", ref strMsg);
        if (strMsg != "")
        {
            clsCommon.ErrorAlertBox(strMsg);
            return;
        }
        iaddonCnt = clsCommon.ParseInteger(value);
        if (iaddonCnt > 0)
        {
            dvError.Style.Add("display", "");
            dvError.InnerText = "Item AddOn already exists.";
            return;
        }

        #endregion CheckDuplicateValues

        string sQry = "";
        string sItemAddOnID = "";

        if ((hfItemAddOnID.Value.Trim().Length > 0) && (hfItemAddOnID.Value != "0"))
        { sItemAddOnID = hfItemAddOnID.Value.Trim(); }
        else { sItemAddOnID = Guid.NewGuid().ToString(); }

        if ((hfItemAddOnID.Value.Trim().Length > 0) && (hfItemAddOnID.Value != "0"))
        {
            sQry = string.Format("UPDATE ItemAddOns SET AddOnName={0}, AddOnPrice={1}  WHERE ItemAddOnID={2}; ",
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtAddOnName.Text.Trim())),
                         txtAddOnPrice.Text.Trim(), clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfItemAddOnID.Value.Trim())));
        }
        else
        {
            sQry = string.Format("INSERT INTO ItemAddOns(ItemAddOnID,AddOnName,AddOnPrice) VALUES({0},{1},{2}); ", clsCommon.sQuote(clsCommon.Remove_SQLInjection(sItemAddOnID.Trim())),
                clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtAddOnName.Text.Trim())), txtAddOnPrice.Text.Trim());
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfItemAddOnID.Value.Trim().Length > 0) && (hfItemAddOnID.Value != "0"))
                clsCommon.SuccessAlertBox("Item Addon updated successfully.", "ItemAddOnList.aspx");
            else
                clsCommon.SuccessAlertBox("Item Addon added successfully.", "ItemAddOnList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occurred while adding/Updating Item AddOn.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtAddOnName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter addon name.";
            txtAddOnName.Focus();
            return;
        }

        if (txtAddOnPrice.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter addon price.";
            txtAddOnPrice.Focus();
            return;
        }
    }
}