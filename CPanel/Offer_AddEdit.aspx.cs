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

public partial class CPanel_Offer_AddEdit : System.Web.UI.Page
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
            clsCommon.FillCombo(ref ddlItem, "SELECT * FROM Items ORDER BY ItemName ASC", "ItemName", "ItemID", "Select Item", ref strMsg);
            if (strMsg != "")
            {
                clsCommon.ErrorAlertBox(strMsg);
                return;
            }

            hfOfferID.Value = "";
            lblTitle.InnerHtml = "<strong> Offer - Add </strong>";

            if (Request.QueryString["ID"] != null)
            {
                if (Request.QueryString["ID"].ToString() != "")
                {
                    hfOfferID.Value = Request.QueryString["ID"].ToString();
                    lblTitle.InnerHtml = "<strong> Offer - Edit </strong>";

                    using (DataTable Dt = clsDatabase.GetDT(string.Format(@"SELECT * FROM offers where OfferID = '{0}'", Request.QueryString["ID"].ToString().Trim()), ref strMsg))
                    {
                        if (strMsg != "")
                        {
                            clsCommon.ErrorAlertBox(strMsg);
                            return;
                        }
                        if (Dt.Rows.Count > 0)
                        {
                            txtOfferCode.Text = Dt.Rows[0]["OfferCode"].ToString().Trim();
                            txtOfferName.Text = Dt.Rows[0]["OfferName"].ToString().Trim();
                            txtOffertype.Text = Dt.Rows[0]["Offertype"].ToString().Trim();
                            txtPrice.Text = Dt.Rows[0]["Price"].ToString().Trim();
                            CKeditor1.Text = Dt.Rows[0]["OfferDescription"].ToString().Trim();
                            ddlItem.Text = Dt.Rows[0]["ItemID"].ToString().Trim();
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

        string sQry = "";
        string sOfferID = "";

        if ((hfOfferID.Value.Trim().Length > 0) && (hfOfferID.Value != "0"))
        { sOfferID = hfOfferID.Value.Trim(); }
        else { sOfferID = Guid.NewGuid().ToString(); }

        if ((hfOfferID.Value.Trim().Length > 0) && (hfOfferID.Value != "0"))
        {
            sQry = string.Format(@"UPDATE offers SET OfferCode={0}, OfferName ={1}, OfferDescription={2}, Offertype ={3},ItemID={4},Price={5},
                        Active ={6}  WHERE OfferID = {7}; ", clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtOfferCode.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtOfferName.Text.Trim())), clsCommon.sQuote_N(CKeditor1.Text.Trim()),
                        clsCommon.sQuote(clsCommon.Remove_SQLInjection(txtOffertype.Text.Trim())), clsCommon.sQuote(ddlItem.SelectedValue), txtPrice.Text,
                        (rdbActive.Checked ? "'Y'" : "'N'"), clsCommon.sQuote(clsCommon.Remove_SQLInjection(hfOfferID.Value.Trim())));
        }
        else
        {
            sQry = string.Format(@"INSERT INTO offers(OfferID,OfferCode,OfferName,OfferDescription,Offertype,ItemID,Price,Active)
                        VALUES({0},{1},{2},{3},{4},{5},{6},{7}); ", clsCommon.sQuote(clsCommon.Remove_SQLInjection(sOfferID.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtOfferCode.Text)), clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(txtOfferName.Text.Trim())),
                        clsCommon.sQuote_N(clsCommon.Remove_SQLInjection(CKeditor1.Text.Trim())), clsCommon.sQuote(clsCommon.Remove_SQLInjection(txtOffertype.Text.Trim())),
                        clsCommon.sQuote_N(ddlItem.SelectedValue), txtPrice.Text, (rdbActive.Checked ? "'Y'" : "'N'"));
        }

        string ErrMsg = "";
        if (clsDatabase.ExecuteNonQuery(sQry, ref ErrMsg))
        {
            if ((hfOfferID.Value.Trim().Length > 0) && (hfOfferID.Value != "0"))
                clsCommon.SuccessAlertBox("Offer updated successfully.", "OfferList.aspx");
            else
                clsCommon.SuccessAlertBox("Offer added successfully.", "OfferList.aspx");
        }
        else
        {
            dvError.Style.Add("display", "");
            dvError.InnerHtml = "Error occured while adding/Updating Offer.&nbsp;" + ErrMsg;
        }
    }

    protected void custVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        dvError.Style.Add("display", "none");

        if (txtOfferCode.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter offer code.";
            txtOfferCode.Focus();
            return;
        }

        if (txtOfferName.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            dvError.Style.Add("display", "");
            dvError.InnerText = "Please enter offer name.";
            txtOfferName.Focus();
            return;
        }
    }
}