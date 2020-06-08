using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.Data;

public partial class CPanel_Item_AddEdit : System.Web.UI.Page
{
    public static string Path;
    protected void Page_Load(object sender, EventArgs e)
    {
        clsCommon.AdminAuthentication();
        #region Set the side menu in collapsed mode
        HtmlGenericControl Mstrbody = (HtmlGenericControl)Master.FindControl("Mstrbody");  /* body tag of CPanel Master Page */
        HtmlGenericControl smenuUI = (HtmlGenericControl)Master.FindControl("smenuUI");  /* ul of side menu in Master Page */

        Mstrbody.Attributes.Add("class", "sidebar-mini fixed animated fadeIn sidebar-collapse");
        smenuUI.Attributes.Add("class", "sidebar-menu");
        #endregion

        Path = Request.ServerVariables["server_name"].ToString().Trim();
        string strMsg = "";
        if (!Page.IsPostBack)
        {
            dvError.Style.Add("display", "none");
            clsCommon.FillCombo(ref ddlItemAddOn, "SELECT * FROM ItemAddOns ORDER BY AddOnName ASC", "AddOnName", "ItemAddOnID", "Select AddOn", ref strMsg);
            if (strMsg != "")
            {
                clsCommon.ErrorAlertBox(strMsg);
                return;
            }

            clsCommon.FillCombo(ref ddlItemType, "SELECT * FROM ItemTypes ORDER BY ItemType ASC", "ItemType", "ItemTypeID", "Select Item Type", ref strMsg);
            if (strMsg != "")
            {
                clsCommon.ErrorAlertBox(strMsg);
                return;
            }

            hfItemID.Value = "";
            lblTitle.InnerHtml = "<strong> Item - Add </strong>";
            //a_view.Visible = false;

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                   // a_view.Visible = true;
                    hfItemID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Item - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM Items where ItemID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtItemCode.Text = Dt.Rows[0]["ItemCode"].ToString().Trim();
                            txtItemDesc.Text = Dt.Rows[0]["ItemDescription"].ToString().Trim();
                            txtItemName.Text = Dt.Rows[0]["ItemName"].ToString().Trim();
                            txtItemPrice.Text = Dt.Rows[0]["ItemPrice"].ToString().Trim();
                            ddlItemAddOn.Text = Dt.Rows[0]["ItemAddOnID"].ToString().Trim();
                            ddlItemType.Text = Dt.Rows[0]["ItemTypeID"].ToString().Trim();
                            rdbVeg.Checked = (Dt.Rows[0]["VegNonVeg"].ToString().Trim() == "V" ? true : false);
                            rdbNonVeg.Checked = (Dt.Rows[0]["VegNonVeg"].ToString().Trim() == "N" ? true : false);
                            rdbYes.Checked = (Dt.Rows[0]["StarReceipe"].ToString().Trim() == "Y" ? true : false);
                            rdbNo.Checked = (Dt.Rows[0]["StarReceipe"].ToString().Trim() == "N" ? true : false);

                            // a_view.HRef = ConfigurationManager.AppSettings["Protocol"].ToString() + Path + "/ImageFiles/Items/" + Dt.Rows[0]["ItemID"].ToString().Trim() + "." + Dt.Rows[0]["ImageExt"].ToString().Trim();
                            // ViewImg.Text = Dt.Rows[0]["ItemID"].ToString().Trim() + "." + Dt.Rows[0]["ImageExt"].ToString().Trim();
                            //imgAlbumArtThumb.Src = ConfigurationManager.AppSettings["Protocol"].ToString() + Path + "/ImageFiles/Items/" + Dt.Rows[0]["ItemID"].ToString().Trim() + "." + Dt.Rows[0]["ImageExt"].ToString().Trim();
                            //imgAlbumArtThumb.Style.Add("background", "url('" + ConfigurationManager.AppSettings["Protocol"].ToString() + Path + "/ImageFiles/Items/" + Dt.Rows[0]["ItemID"].ToString().Trim() + "." + Dt.Rows[0]["ImageExt"].ToString().Trim() + "') no-repeat center");
                            //imgAlbumArtThumb.Attributes.Add("src", ConfigurationManager.AppSettings["Protocol"].ToString() + Path + "/ImageFiles/Items/" + Dt.Rows[0]["ItemID"].ToString().Trim() + "." + Dt.Rows[0]["ImageExt"].ToString().Trim());
                            //if (ViewImg.Text != string.Empty)
                            //{
                            //    cstmvld_Img.Enabled = false;
                            //    spnUpdImg.Visible = false;
                            //}
                        }
                    }
                }
            }
        }
    }


    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtItemCode.Text == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter item code.";
            txtItemCode.Focus();
            return;
        }

        if (txtItemName.Text == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter item name.";
            txtItemName.Focus();
            return;
        }

        if (ddlItemType.SelectedIndex == 0)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter item type.";
            ddlItemType.Focus();
            return;
        }

        if (txtItemPrice.Text == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter item price.";
            txtItemPrice.Focus();
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
        string sItemID = "";

        if ((hfItemID.Value.Trim().Length > 0) && (hfItemID.Value != "0"))
        { sItemID = hfItemID.Value.Trim(); }
        else { sItemID = Guid.NewGuid().ToString(); }

        if ((hfItemID.Value.Trim().Length > 0) && (hfItemID.Value != "0"))
        {
            sQry = string.Format(@"UPDATE Items SET ItemTypeID={0}, ItemCode ={1}, ItemName={2}, ItemDescription ={3},ItemPrice={4},VegNonVeg={5},
                        ItemAddOnID ={6},StarReceipe={7} WHERE ItemID = {8}; ",
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlItemType.SelectedValue.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemCode.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemDesc.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemPrice.Text.Trim())), (rdbVeg.Checked ? "'V'" : "'N'"),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlItemType.SelectedValue.Trim())), (rdbYes.Checked ? "'Y'" : "'N'"),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfItemID.Value.Trim())));
        }
        else
        {
            sQry = string.Format(@"INSERT INTO Items(ItemID,ItemTypeID,ItemCode,ItemName,ItemDescription,ItemPrice,VegNonVeg,ItemAddOnID,StarReceipe)
                        VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}); ", clsCommon.sQuote(clsCommon.Remove_SQLInjection(sItemID.Trim())),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlItemType.SelectedValue.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemCode.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemName.Text.Trim())), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemDesc.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtItemPrice.Text.Trim())), (rdbVeg.Checked ? "'V'" : "'N'"),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(ddlItemAddOn.SelectedValue.Trim())), (rdbYes.Checked ? "'Y'" : "'N'"));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfItemID.Value.Trim().Length > 0) && (hfItemID.Value != "0"))
                clsCommon.SuccessAlertBox("Item updated successfully.", "ItemList.aspx");
            else
                clsCommon.SuccessAlertBox("Item added successfully.", "ItemList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occured while adding/Updating Item.&nbsp;" + ErrMsg;
        }
    }
}