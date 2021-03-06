﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using Newtonsoft.Json;

public partial class CPanel_DeliveryVehicleList : System.Web.UI.Page
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
        string sCols_NS = "VehicleID,VehicleNo,Brand,Model,ChasisNo,FuelType,GPSEnabled,City";
        StringBuilder sbGridData = new StringBuilder();
        string sContentID = "";
        if (sCols_NS != "")
        {
            string[] sColArr = sCols_NS.Split(',');

            if (clsCommon.GetSessionKeyValue("AccessLevel") == "2")
                StrWhereClause = string.Format(" AND DeliveryVehicles.CityID = (select CityID from AdminMast where AdminID = {0})", clsCommon.sQuote(clsCommon.GetSessionKeyValue("AdminID")));

            using (DataTable Dt = clsDatabase.GetDT(@"SELECT VehicleID,VehicleNo,Brand,Model,ChasisNo,FuelType, CASE GPSEnabled WHEN 'Y' THEN 'Yes' else 'No' end as GPSEnabled,
            C.CityName as City FROM DeliveryVehicles INNER JOIN City C on C.CityID = DeliveryVehicles.CityID WHERE 1 = 1 " + StrWhereClause, ref ErrMsgs))
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
                            if (item != "VehicleID")
                                sbGridData.Append("'" + item + "': \"" + clsCommon.ValidateJsonStringWithEscChars(r[item].ToString().Trim()) + "\", ");

                            if (item == "VehicleID")
                            {
                                sContentID = r[item].ToString();
                            }
                        }
                        sbGridData.Append("'VehicleID': \"" + sContentID + "\"");
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
        sbScript.Append("var Edit= \"<a href='\\DeliveryVehicle_AddEdit.aspx?ID=\" + cellVal + \"' title='Edit' ><i class='fa fa-pencil-square'></i></a>\";");
        sbScript.Append(" Edit +=  \"&nbsp;&nbsp;<a id='del\"+ rowObject.VehicleID +\"' onclick='javascript:DeleteVehicle(&quot;\"+cellVal+\"&quot;)' title='Delete' style='cursor:pointer;'><i class='fa fa-trash-o' ></i></a>\";");
        sbScript.Append("return Edit; ");
        sbScript.Append("};");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid({");
        sbScript.Append("contentType: 'application/json; charset=utf-8',");
        sbScript.Append("datatype: 'local',");
        sbScript.Append("data: gidData,");
        sbScript.Append("colNames: ['Vehicle No','Brand','Model','Chasis No','Fuel Type','City','GPS Enabled','Action'],");
        sbScript.Append("colModel: [");
        sbScript.Append("{name:'VehicleNo', index:'VehicleNo',width:90, cellattr: function () { return ' data-title=\"Vehicle No\"'; }},");
        sbScript.Append("{name:'Brand', index:'Brand',width:90, cellattr: function () { return ' data-title=\"Brand\"'; }},");
        sbScript.Append("{name:'Model', index:'Model',classes:'alignCenter', hidden:false,width:70, cellattr: function () { return ' data-title=\"Model\"'; }},");
        sbScript.Append("{name:'ChasisNo', index:'ChasisNo',classes:'alignCenter',width:50, cellattr: function () { return ' data-title=\"Chasis No\"'; }},");
        sbScript.Append("{name:'FuelType', index:'FuelType',width:50, cellattr: function () { return ' data-title=\"Fuel Type\"'; }},");
        sbScript.Append("{name:'City', index:'City',width:90, cellattr: function () { return ' data-title=\"City\"'; }, width:50},");
        sbScript.Append("{name:'GPSEnabled', index:'GPSEnabled', classes:'alignCenter', cellattr: function () { return ' data-title=\"GPS Enabled\"'; }, width:50},");
        sbScript.Append("{name:'VehicleID',index:'VehicleID',sortable:false,search : false, classes:'alignCenter', cellattr: function () { return ' data-title=\"Action\"'; },width:30, formatter: btnEdit}");
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

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'VehicleNo', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'Brand', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'FuelType', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'City', '', {'text-align':'left'});");
        sbScript.Append("});");

        sbScript.Append("</script>");
        ltrScript.Text = sbScript.ToString();
    }


    [WebMethod]
    public static string DeleteVehicle(string VehicleID)
    {
        string ErrMsg = "";
        if ((VehicleID != ""))
        {
            if (clsDatabase.ExecuteNonQuery(string.Format(@"  DELETE FROM DeliveryVehicles WHERE VehicleID = '{0}' ", VehicleID), ref ErrMsg))
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
            List<Vehicle> lstobjVehicle = new List<Vehicle>();
            {
                string strSql = "";
                strSql = @"SELECT VehicleID,VehicleNo,Brand,Model,ChasisNo,FuelType,CASE GPSEnabled WHEN 'Y' THEN 'Yes' else 'No' end as GPSEnabled FROM DeliveryVehicles";
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
                            Vehicle objManageVehicle = new Vehicle();

                            objManageVehicle.VehicleID = r["VehicleID"].ToString().Trim();
                            objManageVehicle.VehicleNo = r["VehicleNo"].ToString().Trim();
                            objManageVehicle.Brand = r["Brand"].ToString().Trim();
                            objManageVehicle.Model = r["Model"].ToString().Trim();
                            objManageVehicle.ChasisNo = r["ChasisNo"].ToString().Trim();
                            objManageVehicle.FuelType = r["FuelType"].ToString().Trim();
                            objManageVehicle.GPSEnabled = r["GPSEnabled"].ToString().Trim();
                            lstobjVehicle.Add(objManageVehicle);
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(lstobjVehicle);
        }
        catch { return ""; }
    }

    class Vehicle
    {
        public string VehicleID { get; set; }
        public string VehicleNo { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string ChasisNo { get; set; }
        public string FuelType { get; set; }
        public string GPSEnabled { get; set; }
    }
}