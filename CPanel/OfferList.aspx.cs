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

public partial class CPanel_OfferList : System.Web.UI.Page
{
    public static int rowcount = 0;
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
            GetJSon(ref strMsg);
            if (strMsg != "")
            {
                clsCommon.ErrorAlertBox(strMsg);
                return;
            }
        }
    }

    private void GetJSon(ref string ErrMsg)
    {
        string ErrMsgs = ""; ErrMsg = "";
        string sCols_NS = "OfferID,OfferCode,OfferName,OfferDescription,Offertype,ItemName,Price,Active";
        StringBuilder sbGridData = new StringBuilder();
        string sContentID = "";
        if (sCols_NS != "")
        {
            string[] sColArr = sCols_NS.Split(',');

            using (DataTable Dt = clsDatabase.GetDT(@"select OfferID,OfferCode,OfferName,OfferDescription,Offertype,I.ItemName,Price, CASE Active when 'Y' then 'Yes' 
            else 'No' end as Active from offers O, Items I where O.ItemID = I.ItemID ", ref ErrMsgs))
            {
                if (ErrMsgs != "")
                {
                    ErrMsg = ErrMsgs;
                    return;
                }
                if ((Dt.Rows.Count > 0))
                {
                    rowcount = Dt.Rows.Count;
                    foreach (DataRow r in Dt.Rows)
                    {
                        sbGridData.Append("{");
                        sContentID = "";
                        foreach (string item in sColArr)
                        {
                            if (item != "OfferID")
                                sbGridData.Append("'" + item + "': \"" + clsCommon.ValidateJsonStringWithEscChars(r[item].ToString().Trim()) + "\", ");

                            if (item == "OfferID")
                            {
                                sContentID = r[item].ToString();
                            }
                        }
                        sbGridData.Append("'OfferID': \"" + sContentID + "\"");
                        sbGridData.Append("},");
                    }
                }
            }
        }

        if (sbGridData.Length > 0)
            sbGridData = sbGridData.Remove(sbGridData.Length - 1, 1);

        StringBuilder sbScript = new StringBuilder();
        sbScript.Append("<script type='text/javascript'>");
        sbScript.Append("$(document).ready(function () {");
        sbScript.Append("'use strict'; var gidData = [" + sbGridData.ToString() + "], theGrid = $('#" + theGrid.ClientID + "'), numberTemplate = { formatter: 'number', align: 'right', sorttype: 'number' }, horizontalScrollPosition = 0, selectedRow = null;");
        sbScript.Append("var btnEdit = function(cellVal,options,rowObject) {");
        sbScript.Append("var Edit= \"<a href='\\Offer_AddEdit.aspx?ID=\" + cellVal + \"' title='Edit' ><i class='fa fa-pencil-square'></i></a>\";");
        sbScript.Append("return Edit; ");
        sbScript.Append("};");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid({");
        sbScript.Append("contentType: 'application/json; charset=utf-8',");
        sbScript.Append("datatype: 'local',");
        sbScript.Append("data: gidData,");
        sbScript.Append("colNames: ['Offer Code','Offer Name','Item Name','Price','Active','Action'],");
        sbScript.Append("colModel: [");
        sbScript.Append("{name:'OfferCode', index:'OfferCode',width:60, cellattr: function () { return ' data-title=\"Offer Code\"'; }},");
        sbScript.Append("{name:'OfferName', index:'OfferName',width:120, cellattr: function () { return ' data-title=\"Offer Name\"'; }},");
        sbScript.Append("{name:'ItemName', index:'ItemName', width:100, cellattr: function () { return ' data-title=\"ItemName\"'; }},");
        sbScript.Append("{name:'Price', index:'Price',classes:'alignCenter',width:60, cellattr: function () { return ' data-title=\"Price\"'; }},");
        sbScript.Append("{name:'Active', index:'Active',width:50,classes:'alignCenter',  cellattr: function () { return ' data-title=\"Active\"'; }},");
        sbScript.Append("{name:'OfferID', index:'OfferID',sortable:false,search : false, classes:'alignCenter', cellattr: function () { return ' data-title=\"Action\"'; },width:30, formatter: btnEdit}");
        sbScript.Append("],");
        sbScript.Append("gridview: true,");
        sbScript.Append("rownumbers: false,");
        sbScript.Append("rowNum: " + ConfigurationManager.AppSettings["CPPageSize"].ToString().Trim() + ",");
        // sbScript.Append("rowList: [10, 25, 50, 100, 250],");
        sbScript.Append("rowList: " + clsCommon.GetGridPaging_ArrayList(rowcount) + ",");

        sbScript.Append("pager: '#" + gridPager.ClientID + "',");
        sbScript.Append("viewrecords: true,");
        sbScript.Append("multiSort: false,");
        sbScript.Append("autowidth: true,");
        sbScript.Append("sortname: 'Name',");
        sbScript.Append("sortorder: 'asc',");
        sbScript.Append("caption: '',");
        sbScript.Append("height: 'auto',");
        sbScript.Append("autoencode: true,");
        sbScript.Append("loadonce: true,");
        sbScript.Append("hidegrid: false,");    /* TO HIDE EXPAND AND COLLAPSE*/
        sbScript.Append("ignoreCase: true,");
        // sbScript.Append("rownumbers:true,"); /* TO SHOW ROW NUMBER FOR EVERY RECORD*/
        sbScript.Append("toppager:true,");      /* TO SHOW ONE PAGER AT THE TOP ALSO*/
        sbScript.Append("shrinkToFit : true,");
        sbScript.Append("loadComplete: function () {");
        sbScript.Append("$(this).find('>tbody>tr.jqgrow:odd').addClass('rowOdd');");
        sbScript.Append("$(this).find('>tbody>tr.jqgrow:even').addClass('rowEven');");
        sbScript.Append("$(\".ui-pg-selbox option[value='ALL']\").val(" + clsCommon.GetGridPaging_ViewAllLimit() + ");");
        sbScript.Append("}");
        sbScript.Append("});");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid('navGrid','#" + gridPager.ClientID + "',{cloneToTop:true,edit:false,add:false,del:false,search:true},{/* edit options */ },{ /* add options */},{ /* delete options */},{ /* search options */ sopt: ['cn','nc','eq','ne','lt','le','gt','ge','bw','bn','ew','en']},{});");
        sbScript.Append("$(window).resize(function () {");
        sbScript.Append("var newWidth = $('#" + theGrid.ClientID + "').closest('.ui-jqgrid').parent().width();");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid('setGridWidth', newWidth, true);");
        sbScript.Append("});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid('navGrid', '#" + gridPager.ClientID + "', { edit: false, add: false, del: false },{/* edit options */ },{ /* add options */},{ /* delete options */},{ /* search options */ sopt: ['cn','nc','eq','ne','lt','le','gt','ge','bw','bn','ew','en']},{});");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'OfferCode', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'OfferName', '', {'text-align':'left'});");
        //  sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'ItemPrice', '', {'text-align':'right'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'ItemName', '', {'text-align':'left'});");
        sbScript.Append("});");

        sbScript.Append("</script>");
        ltrScript.Text = sbScript.ToString();
    }
}