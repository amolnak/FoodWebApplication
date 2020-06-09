using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class CPanel_ItemType_AddEdit : System.Web.UI.Page
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
            hfItemTypeID.Value = "";
            lblTitle.InnerHtml = "<strong> Item Type - Add </strong>";

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfItemTypeID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Item Type - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM ItemTypes where ItemTypeID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtItemType.Text = Dt.Rows[0]["ItemType"].ToString().Trim();
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

        int iItemTypeCnt = 0;

        value = clsDatabase.GetSingleValue("SELECT COUNT(0) FROM ItemTypes WHERE ItemType=N'" + clsCommon.Remove_SQLInjection(txtItemType.Text.Trim()) + "'" + (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" ? " and ItemTypeID <>'" + Request.QueryString["ID"].ToString() + "'" : "") + ";", ref strMsg);
        if (strMsg != "")
        {
            clsCommon.ErrorAlertBox(strMsg);
            return;
        }
        iItemTypeCnt = clsCommon.ParseInteger(value);
        if (iItemTypeCnt > 0)
        {
            dvError.Style.Add("display", "");
            dvError.InnerText = "Item Type already exists.";
            return;
        }
        #endregion CheckDuplicateValues

        string sQry = "";
        string sItemTypeID = "";

        if ((hfItemTypeID.Value.Trim().Length > 0) && (hfItemTypeID.Value != "0"))
        { sItemTypeID = hfItemTypeID.Value.Trim(); }
        else { sItemTypeID = Guid.NewGuid().ToString(); }

        if ((hfItemTypeID.Value.Trim().Length > 0) && (hfItemTypeID.Value != "0"))
        {
            sQry = string.Format("UPDATE ItemTypes SET ItemType={0}  WHERE ItemTypeID = {1}; ", clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemType.Text.Trim())),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfItemTypeID.Value.Trim())));
        }
        else
        {
            sQry = string.Format("INSERT INTO ItemTypes(ItemTypeID,ItemType) VALUES({0},{1}); ",
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(sItemTypeID.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemType.Text.Trim())));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfItemTypeID.Value.Trim().Length > 0) && (hfItemTypeID.Value != "0"))
                clsCommon.SuccessAlertBox("Item type updated successfully.", "ItemTypeList.aspx");
            else
                clsCommon.SuccessAlertBox("Item type added successfully.", "ItemTypeList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occurred while adding/Updating item type.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtItemType.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter item type.";
            txtItemType.Focus();
            return;
        }
    }
}