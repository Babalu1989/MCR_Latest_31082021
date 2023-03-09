<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using System.IO;
using System.Data;
public class ImageHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string _sImagePath = string.Empty;
            if (context.Request.QueryString["IMPPATH"] != null)
                _sImagePath = context.Request.QueryString["IMPPATH"].ToString();
            FileStream livestream = new FileStream(_sImagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            context.Response.Buffer = true;
            context.Response.Charset = "";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "image/jpg";
            context.Response.AddHeader("content-disposition", "attachment;filename=1.JPEG");
            context.Response.BinaryWrite(buffer);
            context.Response.Flush();
            context.Response.End();

        }
        catch (Exception ex)
        {
        }
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}