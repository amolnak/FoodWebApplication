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

public partial class CPanel_NotificationList : System.Web.UI.Page
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
        string sCols_NS = "NotificationId,NotifCreatedDate,NotifTitle,NotificationType,ScheduleUTCTime,NotificationStatus,DateCreated_Unx,DateScheduled_Unx";
        StringBuilder sbGridData = new StringBuilder();
        string sContentID = "", SCol_DateCreated_Unx = "", SCol_DateScheduled_Unx = "", SCol_CreatedDate = "", SCol_ScheduleDate = "";
        if (sCols_NS != "")
        {
            string[] sColArr = sCols_NS.Split(',');

            using (DataTable Dt = clsDatabase.GetDT(@"select NotificationId,NotifCreatedDate,NotifTitle,CASE WHEN NotificationType = 'SN' then 'Send Now' else 
            'Schedduled' end as NotificationType,ScheduleUTCTime,CASE NotificationStatus WHEN 'C' then 'Complete' else 'In-Complete' end as NotificationStatus,
            dbo.fn_GetUnixTimestamp(NotifCreatedDate) DateCreated_Unx,dbo.fn_GetUnixTimestamp(ScheduleUTCTime) DateScheduled_Unx from Notifications ", ref ErrMsgs))
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
                            if (item != "NotificationId")
                                sbGridData.Append("'" + item + "': \"" + clsCommon.ValidateJsonStringWithEscChars(r[item].ToString().Trim()) + "\", ");

                            if (item == "NotificationId")
                            {
                                sContentID = r[item].ToString();
                            }
                            if (item == "NotifCreatedDate")
                            {
                                SCol_CreatedDate = r[item].ToString();
                            }
                            if (item == "ScheduleUTCTime")
                            {
                                SCol_ScheduleDate = r[item].ToString();
                            }
                            if (item == "DateCreated_Unx")
                            {
                                SCol_DateCreated_Unx = r[item].ToString();
                            }
                            if (item == "DateScheduled_Unx")
                            {
                                SCol_DateScheduled_Unx = r[item].ToString();
                            }
                        }
                        sbGridData.Append("'NotificationId': \"" + sContentID + "\"");
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
        sbScript.Append("var Edit= \"<a href='\\Notification_AddEdit.aspx?ID=\" + cellVal + \"' title='Edit' ><i class='fa fa-pencil-square'></i></a>\";");
        sbScript.Append("return Edit; ");
        sbScript.Append("};");

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid({");
        sbScript.Append("contentType: 'application/json; charset=utf-8',");
        sbScript.Append("datatype: 'local',");
        sbScript.Append("data: gidData,");
        sbScript.Append("colNames: ['Notification Title','Notification Type','Date Created','Scheduled Date','Status','Action'],");
        sbScript.Append("colModel: [");
        sbScript.Append("{name:'NotifTitle', index:'NotifTitle',width:100, cellattr: function () { return ' data-title=\"Notification Title\"'; }},");
        sbScript.Append("{name:'NotificationType', index:'NotificationType',width:120, cellattr: function () { return ' data-title=\"Notification Type\"'; }},");
        sbScript.Append("{name:'NotifCreatedDate', index:'DateCreated_Unx',classes:'alignCenter', hidden:false,width:50, cellattr: function () { return ' data-title=\"Date Created\"'; }},");
        sbScript.Append("{name:'ScheduleUTCTime', index:'DateScheduled_Unx',classes:'alignCenter',width:60, cellattr: function () { return ' data-title=\"Scheduled Date\"'; }},");
        sbScript.Append("{name:'NotificationStatus', index:'NotificationStatus',width:90, cellattr: function () { return ' data-title=\"Status\"'; }},");
        sbScript.Append("{name:'NotificationId', index:'NotificationId',sortable:false,search : false, classes:'alignCenter', cellattr: function () { return ' data-title=\"Action\"'; },width:30, formatter: btnEdit}");
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

        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'NotifTitle', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'NotificationType', '', {'text-align':'left'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'NotifCreatedDate', '', {'text-align':'center'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'ScheduleUTCTime', '', {'text-align':'center'});");
        sbScript.Append("jQuery('#" + theGrid.ClientID + "').jqGrid ('setLabel', 'NotificationStatus', '', {'text-align':'left'});");
        sbScript.Append("});");

        sbScript.Append("</script>");
        ltrScript.Text = sbScript.ToString();
    }

}