using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO; 

public partial class ImageCSharp : System.Web.UI.Page
{
    string pth = string.Empty, username = string.Empty, password = string.Empty, sl = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            if (Request.QueryString["FileName"] != null)
            {
                string _sFolder = string.Empty;
                string filename = string.Empty;
                string _sFileDT = Request.QueryString["FileName"].ToString();
                string[] arr = _sFileDT.ToString().Split('/');

                _sFolder = arr[arr.Length - 3].ToString();
                filename = arr[arr.Length - 1].ToString();

                pth = @"\\10.125.64.236\UploadedImages\MCR\";
                sl = pth;
                string _sDir = sl + _sFolder;
                pth = _sDir;

                Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
                nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");

                string filePath = pth;
                string contenttype = "image/" + Path.GetExtension(filename).Replace(".", "");
                FileStream fs = new FileStream(filePath + "\\" + filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                br.Close();
                fs.Close();

                //Write the file to response Stream
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = contenttype;
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();

            }
        }
        catch (Exception er)
        {

        }
    }
}