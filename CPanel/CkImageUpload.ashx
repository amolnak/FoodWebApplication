<%@ WebHandler Language="C#" Class="CkImageUpload" %>

using System;
using System.Web;

public class CkImageUpload : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        HttpPostedFile uploads = context.Request.Files["upload"];
        string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
        string file = System.IO.Path.GetFileName(uploads.FileName);
        string FormName = context.Request.QueryString["Form"].ToString();
        string url = "";
        HttpContext.Current.Response.Write(CKEditorFuncNum);

        if (FormName == "Notif")
        {
            uploads.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/ImageFiles/Notifications/") + file);
            url = "/ImageFiles/Notifications/" + file;
        }
        //else if (FormName == "Faq")
        //{
        //    uploads.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/DyFiles/FaqFls/") + file);
        //    url = "/DyFiles/FaqFls/" + file;
        //}
        //else if (FormName == "MLib")
        //{
        //    uploads.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/DyFiles/MediaLibFls/") + file);
        //    url = "/DyFiles/MediaLibFls/" + file;
        //}
        //else if (FormName == "RefDoc")
        //{
        //    uploads.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/DyFiles/RefDocFls/") + file);
        //    url = "/DyFiles/RefDocFls/" + file;
        //}         
        //else if (FormName == "EmailTmp")
        //{
        //    uploads.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/DyFiles/EmailTmpFls/") + file);
        //    url = "/DyFiles/EmailTmpFls/" + file;
        //}
        //   else if (FormName == "CEP")
        //{
        //    uploads.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/DyFiles/CEPFls/") + file);
        //    url = "/DyFiles/CEPFls/" + file;
        //}
        //   else if (FormName == "CEPTitle")
        //{
        //    uploads.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/DyFiles/CEPTitleFls/") + file);
        //    url = "/DyFiles/CEPTitleFls/" + file;
        //}
        //  else if (FormName == "Tools")
        //{
        //    uploads.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/DyFiles/ToolsFls/") + file);
        //    url = "/DyFiles/ToolsFls/" + file;
        //}

        context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}