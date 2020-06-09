using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using Newtonsoft.Json;

public partial class CPanel_ItemAddOnList : System.Web.UI.Page
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
        string sCols_NS = "ItemAddOnID,AddOnName,AddOnPrice";
        StringBuilder sbGridData = new StringBuilder();
        string sContentID = "";

        if (sCols_NS != "")
        {
            string[] sColArr = sCols_NS.Split(',');

            using (DataTable Dt = clsDatabase.GetDT(@"SELECT ItemAddOnID,AddOnName,AddOnPrice FROM ItemAddOns", ref ErrMsgs))
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
                            if (item != "ItemAddOnID")
                                sbGridData.Append("'" + item + "': \"" + clsCommon.ValidateJsonStringWithEscChars(r[item].ToString().Trim()) + "\", ");

                            if (item == "ItemAddOnID")
                            {
                                sContentID = r[item].ToString();
                            }
                        }
                        sbGridData.Append("'ItemAddOnID': \"" + sContentID + "\"");
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
        sbScript.Append("var Edit= \"<a href='\\ItemAddOn_AddEdit.aspx?ID=\" + cellVal + \"' title='Edit' ><i class='fa fa-pencil-square'></i></a>\";");
        sbScript.Append(" Edit +=  \"&nbsp;&nbsp;<a id='del\"+ rowObject.ItemAddOnID +\"' onclick='javascript:DeleteItemAddOn(&quot;\"+cellVal+\"&quot;)' title='Delete' style='cursor:pointer;'><i class='fa fa-trash-o' ></i></a>\";");
        sbScript.Append("return Edit; ");
        sbScript.Append("};");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid({");
        sbScript.Append("contentType: 'application/json; charset=utf-8',");
        sbScript.Append("datatype: 'local',");
        sbScript.Append("data: gidData,");
        sbScript.Append("colNames: ['AddOn Name','AddOn Price','Action'],");
        sbScript.Append("colModel: [");
        sbScript.Append("{name:'AddOnName', index:'AddOnName',  cellattr: function () { return ' data-title=\"AddOn Name\"'; }},");
        sbScript.Append("{name:'AddOnPrice', index:'AddOnPrice', hidden:false, classes:'alignCenter', cellattr: function () { return ' data-title=\"AddOn Price\"'; }},");
        sbScript.Append("{name:'ItemAddOnID',index:'ItemAddOnID',sortable:false,search : false, classes:'alignCenter', cellattr: function () { return ' data-title=\"Action\"'; },width:30, formatter: btnEdit}");
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
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'AddOnName', '', {'text-align':'left'});");
        sbScript.Append("});");

        sbScript.Append("</script>");
        ltrScript.Text = sbScript.ToString();
    }


    [WebMethod]
    public static string DeleteitemAddon(string ItemAddOnID)
    {
        string ErrMsg = "";
        if ((ItemAddOnID != ""))
        {
            if (clsDatabase.ExecuteNonQuery(string.Format(@"  DELETE FROM ItemAddons WHERE ItemAddOnID = '{0}' ", ItemAddOnID), ref ErrMsg))
            {
                if (ErrMsg != string.Empty)
                {
                    clsCommon.ErrorAlertBox(ErrMsg);
                    return "";
                }
                else
                    return GetJSon_Obj();
            }
            else
                return "";
        }
        else
            return "";
    }

    public static string GetJSon_Obj()
    {
        string ErrMsg = "";
        try
        {
            List<ItemAddon> lstobjItemAddon = new List<ItemAddon>();
            {
                string strSql = "";
                strSql = @"SELECT ItemAddOnID,AddOnName,AddOnPrice FROM ItemAddOns";
                if (ErrMsg != string.Empty)
                {
                    clsCommon.ErrorAlertBox(ErrMsg);
                    return "";
                }
                using (DataTable dtList = clsDatabase.GetDT(strSql, ref ErrMsg))
                {
                    if (ErrMsg != string.Empty)
                    {
                        clsCommon.ErrorAlertBox(ErrMsg);
                        return "";
                    }
                    if ((dtList.Rows.Count > 0))
                    {
                        rowcount = dtList.Rows.Count;
                        foreach (DataRow r in dtList.Rows)
                        {
                            ItemAddon objManageItemAddon = new ItemAddon();
                            objManageItemAddon.AddOnName = r["AddOnName"].ToString().Trim();
                            objManageItemAddon.ItemAddOnID = r["ItemAddOnID"].ToString().Trim();
                            objManageItemAddon.AddOnPrice = r["AddOnPrice"].ToString().Trim();                           
                            lstobjItemAddon.Add(objManageItemAddon);
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(lstobjItemAddon);
        }
        catch { return ""; }
    }

    class ItemAddon
    {
        public string AddOnName { get; set; }
        public string ItemAddOnID { get; set; }
        public string AddOnPrice { get; set; }
    }
}