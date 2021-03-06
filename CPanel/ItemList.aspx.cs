﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.Data;
using System.Web.Services;
using Newtonsoft.Json;

public partial class CPanel_ItemList : System.Web.UI.Page
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
        string sCols_NS = "ItemID,ItemType,ItemCode,ItemName,ItemPrice,VegNonVeg,AddOnName,StarReceipe";
        StringBuilder sbGridData = new StringBuilder();
        string sContentID = "";
        if (sCols_NS != "")
        {
            string[] sColArr = sCols_NS.Split(',');

            using (DataTable Dt = clsDatabase.GetDT(@"select ItemID,ItemType,ItemCode,ItemName,ItemPrice,CASE VegNonVeg WHEN 'V' then 'Veg' else 'Non-Veg' end as VegNonVeg,
            AddOnName,CASE StarReceipe when 'Y' then 'Yes' else 'No' end as StarReceipe from Items I inner join ItemTypes IT on IT.ItemTypeID = I.ItemTypeID inner join 
            ItemAddOns IA on IA.ItemAddOnID = I.ItemAddOnID ", ref ErrMsgs))
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
                            if (item != "ItemID")
                                sbGridData.Append("'" + item + "': \"" + clsCommon.ValidateJsonStringWithEscChars(r[item].ToString().Trim()) + "\", ");

                            if (item == "ItemID")
                            {
                                sContentID = r[item].ToString();
                            }
                        }
                        sbGridData.Append("'ItemID': \"" + sContentID + "\"");
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
        sbScript.Append("var Edit= \"<a href='\\Item_AddEdit.aspx?ID=\" + cellVal + \"' title='Edit' ><i class='fa fa-pencil-square'></i></a>\";");
        sbScript.Append(" Edit +=  \"&nbsp;&nbsp;<a id='del\"+ rowObject.ItemID +\"' onclick='javascript:DeleteItem(&quot;\"+cellVal+\"&quot;)' title='Delete' style='cursor:pointer;'><i class='fa fa-trash-o' ></i></a>\";");
        sbScript.Append("return Edit; ");
        sbScript.Append("};");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid({");
        sbScript.Append("contentType: 'application/json; charset=utf-8',");
        sbScript.Append("datatype: 'local',");
        sbScript.Append("data: gidData,");
        sbScript.Append("colNames: ['Item Code','Item Name','Price','Veg / NonVeg','AddOn Name','Star Receipe','Action'],");
        sbScript.Append("colModel: [");
        sbScript.Append("{name:'ItemCode', index:'ItemCode',width:60, cellattr: function () { return ' data-title=\"Item Code\"'; }},");
        sbScript.Append("{name:'ItemName', index:'ItemName',width:120, cellattr: function () { return ' data-title=\"Item Name\"'; }},");
        sbScript.Append("{name:'ItemPrice', index:'ItemPrice',classes:'alignCenter', hidden:false,width:50, cellattr: function () { return ' data-title=\"Price\"'; }},");
        sbScript.Append("{name:'VegNonVeg', index:'VegNonVeg',classes:'alignCenter',width:60, cellattr: function () { return ' data-title=\"Veg / NonVeg\"'; }},");
        sbScript.Append("{name:'AddOnName', index:'AddOnName',width:90, cellattr: function () { return ' data-title=\"AddOn Name\"'; }},");
        sbScript.Append("{name:'StarReceipe', index:'StarReceipe',classes:'alignCenter', cellattr: function () { return ' data-title=\"Star Receipe\"'; }, width:50},");
        sbScript.Append("{name:'ItemID', index:'ItemID',sortable:false,search : false, classes:'alignCenter', cellattr: function () { return ' data-title=\"Action\"'; },width:30, formatter: btnEdit}");
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

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'ItemCode', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'ItemName', '', {'text-align':'left'});");
        //  sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'ItemPrice', '', {'text-align':'right'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'AddOnName', '', {'text-align':'left'});");
        sbScript.Append("});");

        sbScript.Append("</script>");
        ltrScript.Text = sbScript.ToString();
    }


    [WebMethod]
    public static string DeleteItem(string ItemID)
    {
        string ErrMsg = "";
        if ((ItemID != ""))
        {
            if (clsDatabase.ExecuteNonQuery(string.Format(@" DELETE FROM Items WHERE ItemID = '{0}' ", ItemID), ref ErrMsg))
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
            List<Item> lstobjItem = new List<Item>();
            {
                string strSql = "";
                strSql = @"select ItemID,ItemType,ItemCode,ItemName,ItemPrice,CASE VegNonVeg WHEN 'V' then 'Veg' else 'Non-Veg' end as VegNonVeg, AddOnName,
                CASE StarReceipe when 'Y' then 'Yes' else 'No' end as StarReceipe from Items I inner join ItemTypes IT on IT.ItemTypeID = I.ItemTypeID inner join 
                ItemAddOns IA on IA.ItemAddOnID = I.ItemAddOnID";

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
                            Item objManageItem = new Item();
                            objManageItem.ItemID = r["ItemID"].ToString().Trim();
                            objManageItem.ItemType = r["ItemType"].ToString().Trim();
                            objManageItem.ItemCode = r["ItemCode"].ToString().Trim();
                            objManageItem.ItemName = r["ItemName"].ToString().Trim();
                            objManageItem.ItemPrice = r["ItemPrice"].ToString().Trim();
                            objManageItem.VegNonVeg = r["VegNonVeg"].ToString().Trim();
                            objManageItem.AddOnName = r["AddOnName"].ToString().Trim();
                            objManageItem.StarReceipe = r["StarReceipe"].ToString().Trim();
                            lstobjItem.Add(objManageItem);
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(lstobjItem);
        }
        catch { return ""; }
    }

    class Item
    {
        public string ItemID { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemPrice { get; set; }
        public string VegNonVeg { get; set; }
        public string AddOnName { get; set; }
        public string StarReceipe { get; set; }
    }
}