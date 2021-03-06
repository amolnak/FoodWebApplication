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

public partial class CPanel_DeliveryBoysList : System.Web.UI.Page
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
        string ErrMsgs = "", StrWhereClause = "";
        string sCols_NS = "DeliveryBoyID,DBName,DBEmail,DBPhone1,DBPhone2,DBRegion,DBCity,VehicleNo";
        StringBuilder sbGridData = new StringBuilder();
        string sContentID = "";
        {
            string[] sColArr = sCols_NS.Split(',');

            if (clsCommon.GetSessionKeyValue("AccessLevel") == "2")
                StrWhereClause = string.Format(" AND DBCityID = (select CityID from AdminMast where AdminID = {0})", clsCommon.sQuote(clsCommon.GetSessionKeyValue("AdminID")));

            using (DataTable Dt = clsDatabase.GetDT(@"select DeliveryBoyID,DBFName + ' ' + DBLName as DBName,DBEmail,DBPhone1,DBPhone2,R.RegionName as DBRegion,
            C.CityName as DBCity,VehicleNo from DeliveryBoys DB INNER JOIN DeliveryVehicles DV on DV.VehicleID=DB.VehicleID INNER JOIN Region R ON 
            DB.DBRegionID = R.RegionID INNER JOIN City C on C.CityID = DB.DBCityID WHERE 1 = 1 " + StrWhereClause, ref ErrMsgs))
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
                            if (item != "DeliveryBoyID")
                                sbGridData.Append("'" + item + "': \"" + clsCommon.ValidateJsonStringWithEscChars(r[item].ToString().Trim()) + "\", ");

                            if (item == "DeliveryBoyID")
                            {
                                sContentID = r[item].ToString();
                            }
                        }
                        sbGridData.Append("'DeliveryBoyID': \"" + sContentID + "\"");
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
        sbScript.Append("var Edit= \"<a href='\\DeliveryBoy_AddEdit.aspx?ID=\" + cellVal + \"' title='Edit' ><i class='fa fa-pencil-square'></i></a>\";");
        sbScript.Append(" Edit +=  \"&nbsp;&nbsp;<a id='del\"+ rowObject.DeliveryBoyID +\"' onclick='javascript:DeleteDeliveryBoy(&quot;\"+cellVal+\"&quot;)' title='Delete' style='cursor:pointer;'><i class='fa fa-trash-o' ></i></a>\";");
        sbScript.Append("return Edit; ");
        sbScript.Append("};");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid({");
        sbScript.Append("contentType: 'application/json; charset=utf-8',");
        sbScript.Append("datatype: 'local',");
        sbScript.Append("data: gidData,");
        sbScript.Append("colNames: ['Name','Email','Phone No.','Alternate Phone No.','Region','City','Vehicle No','Action'],");
        sbScript.Append("colModel: [");
        sbScript.Append("{name:'DBName', index:'DBName',width:100, cellattr: function () { return ' data-title=\"Name\"'; }},");
        sbScript.Append("{name:'DBEmail', index:'DBEmail',width:120, cellattr: function () { return ' data-title=\"Email\"'; }},");
        sbScript.Append("{name:'DBPhone1', index:'DBPhone1',classes:'alignCenter', hidden:false,width:70, cellattr: function () { return ' data-title=\"Phone No\"'; }},");
        sbScript.Append("{name:'DBPhone2', index:'DBPhone2',classes:'alignCenter',width:90, cellattr: function () { return ' data-title=\"Alternate Phone No\"'; }},");
        sbScript.Append("{name:'DBRegion', index:'DBRegion',width:50, cellattr: function () { return ' data-title=\"Region\"'; }},");
        sbScript.Append("{name:'DBCity', index:'DBCity', cellattr: function () { return ' data-title=\"City\"'; }, width:50},");
        sbScript.Append("{name:'VehicleNo', index:'VehicleNo', classes:'alignCenter', cellattr: function () { return ' data-title=\"Vehicle No\"'; }, width:50},");
        sbScript.Append("{name:'DeliveryBoyID', index:'DeliveryBoyID',sortable:false,search : false, classes:'alignCenter', cellattr: function () { return ' data-title=\"Action\"'; },width:30, formatter: btnEdit}");
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

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'DBName', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'DBEmail', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'DBRegion', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'DBCity', '', {'text-align':'left'});");
        sbScript.Append("});");

        sbScript.Append("</script>");
        ltrScript.Text = sbScript.ToString();
    }

    [WebMethod]
    public static string DeleteDeliveryBoy(string DeliveryBoyID)
    {
        string ErrMsg = "";
        if ((DeliveryBoyID != ""))
        {
            if (clsDatabase.ExecuteNonQuery(string.Format(@"  DELETE FROM DeliveryBoys WHERE DeliveryBoyID = '{0}' ", DeliveryBoyID), ref ErrMsg))
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
            List<DeliveryBoys> lstobjDB = new List<DeliveryBoys>();
            {
                string strSql = "";
                strSql = @"select DeliveryBoyID,DBFName + ' ' + DBLName as DBName,DBEmail,DBPhone1,DBPhone2,R.RegionName as DBRegion, C.CityName as DBCity, VehicleNo 
                        from DeliveryBoys DB INNER JOIN DeliveryVehicles DV on DV.VehicleID = DB.VehicleID INNER JOIN Region R ON
                        DB.DBRegionID = R.RegionID INNER JOIN City C on C.CityID = DB.DBCityID";
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
                            DeliveryBoys objManageDB = new DeliveryBoys();

                            objManageDB.DeliveryBoyID = r["DeliveryBoyID"].ToString().Trim();
                            objManageDB.DBName = r["DBName"].ToString().Trim();
                            objManageDB.DBEmail = r["DBEmail"].ToString().Trim();
                            objManageDB.DBPhone1 = r["DBPhone1"].ToString().Trim();
                            objManageDB.DBPhone2 = r["DBPhone2"].ToString().Trim();
                            objManageDB.DBRegion = r["DBRegion"].ToString().Trim();
                            objManageDB.DBCity = r["DBCity"].ToString().Trim();
                            objManageDB.VehicleNo = r["VehicleNo"].ToString().Trim();
                            lstobjDB.Add(objManageDB);
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(lstobjDB);
        }
        catch { return ""; }
    }

    class DeliveryBoys
    {
        public string DeliveryBoyID { get; set; }
        public string DBName { get; set; }
        public string DBEmail { get; set; }
        public string DBPhone1 { get; set; }
        public string DBPhone2 { get; set; }
        public string DBRegion { get; set; }
        public string DBCity { get; set; }
        public string VehicleNo { get; set; }
    }

}