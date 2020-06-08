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

public partial class CPanel_VendorList : System.Web.UI.Page
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
        string sCols_NS = "VendorID,VendorCode,Name,VendorEmail,VendorPhone1,VendorPhone2,Region,City,DeliveryProvision,Active";
        StringBuilder sbGridData = new StringBuilder();
        string sContentID = "";
        if (sCols_NS != "")
        {
            string[] sColArr = sCols_NS.Split(',');

            using (DataTable Dt = clsDatabase.GetDT(@"select VendorID,VendorCode,VendorFName + ' ' + VendorLName as Name,VendorEmail,VendorPhone1,VendorPhone2,
            R.RegionName as Region, C.CityName as City,Case DeliveryProvision WHEN 'Y' then 'Yes' else 'No' end as DeliveryProvision, CASE vendors.Active WHEN 'Y' then 'Active'
            else 'In-active' end as Active from vendors INNER JOIN Region R ON vendors.RegionID = R.RegionID INNER JOIN City C on C.CityID = vendors.CityID", ref ErrMsgs))
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
                            if (item != "VendorID")
                                sbGridData.Append("'" + item + "': \"" + clsCommon.ValidateJsonStringWithEscChars(r[item].ToString().Trim()) + "\", ");

                            if (item == "VendorID")
                            {
                                sContentID = r[item].ToString();
                            }
                        }
                        sbGridData.Append("'VendorID': \"" + sContentID + "\"");
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
        sbScript.Append("var Edit= \"<a href='\\Vendor_AddEdit.aspx?ID=\" + cellVal + \"' title='Edit' ><i class='fa fa-pencil-square'></i></a>\";");
        sbScript.Append("return Edit; ");
        sbScript.Append("};");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid({");
        sbScript.Append("contentType: 'application/json; charset=utf-8',");
        sbScript.Append("datatype: 'local',");
        sbScript.Append("data: gidData,");
        sbScript.Append("colNames: ['Vendor Code','Name','Email','Phone No.','Alternate Phone No.','Region','City','Delivery Provision','Status','Action'],");
        sbScript.Append("colModel: [");
        sbScript.Append("{name:'VendorCode', index:'VendorCode', cellattr: function () { return ' data-title=\"Vendor Code\"'; }},");
        sbScript.Append("{name:'Name', index:'Name',cellattr: function () { return ' data-title=\"Name\"'; }},");
        sbScript.Append("{name:'VendorEmail', index:'VendorEmail', cellattr: function () { return ' data-title=\"Email\"'; }},");
        sbScript.Append("{name:'VendorPhone1', index:'VendorPhone1',classes:'alignCenter', hidden:false, cellattr: function () { return ' data-title=\"Phone No\"'; }},");
        sbScript.Append("{name:'VendorPhone2', index:'VendorPhone2',classes:'alignCenter', cellattr: function () { return ' data-title=\"Alternate Phone No\"'; }},");
        sbScript.Append("{name:'Region', index:'Region', cellattr: function () { return ' data-title=\"Region\"'; }},");
        sbScript.Append("{name:'City', index:'City', cellattr: function () { return ' data-title=\"City\"'; }},");
        sbScript.Append("{name:'DeliveryProvision', index:'DeliveryProvision', classes:'alignCenter', cellattr: function () { return ' data-title=\"Delivery Provision\"'; }, width:90},");
        sbScript.Append("{name:'Active', index:'Active', classes:'alignCenter', cellattr: function () { return ' data-title=\"Status\"'; }, width:70},");
        sbScript.Append("{name:'VendorID', index:'VendorID',sortable:false,search : false, classes:'alignCenter', cellattr: function () { return ' data-title=\"Action\"'; },width:60, formatter: btnEdit}");
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

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'VendorCode', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'Name', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'VendorEmail', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'Region', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'City', '', {'text-align':'left'});");
        sbScript.Append("});");

        sbScript.Append("</script>");
        ltrScript.Text = sbScript.ToString();
    }
}