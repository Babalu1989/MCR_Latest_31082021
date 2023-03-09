using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Adapters;
using System.Collections;
using System.Text.RegularExpressions;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using iTextSharp.text.html.simpleparser;
using System.Data;
using System.Web.UI.HtmlControls;

using System.Drawing.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public partial class GenerateMCR : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string _sCANumber = Convert.ToString(Request.QueryString["CANo"]).Trim();

            retrieveData(_sCANumber);
        }        
    }

    const string msgFormat = "table[{0}], tr[{1}], td[{2}], a: {3}, b: {4}";
    const string table_pattern = "<table.*?>(.*?)</table>";
    const string tr_pattern = "<tr.*?>(.*?)</tr>";
    const string td_pattern = "<td.*?>(.*?)</td>";
    const string a_pattern = "<a href=\"(.*?)\"></a>";
    const string b_pattern = "<b>(.*?)</b>";

    private void retrieveData(string CANumber)
    {
        DataTable dtdata = new DataTable();       

        try
        {
            dtdata = objBL.GetMCR_OrderNoWise(CANumber);
       
            if (dtdata.Rows.Count > 0)
            {
                lblOrderNo.Text = dtdata.Rows[0]["ORDERID"].ToString().Trim();
                lblCA_Number.Text = CANumber.ToString().Trim();
                lblActType.Text = dtdata.Rows[0]["ILART_ACTIVITY_TYPE"].ToString().Trim();
                lblDivision.Text = dtdata.Rows[0]["DIVISION"].ToString().Trim();
                lblDateOfActivity.Text = dtdata.Rows[0]["ACT_DATE"].ToString().Trim();
                lblAccountClass.Text = dtdata.Rows[0]["ACCOUNT_CLASS"].ToString().Trim();
                lblCategory.Text = dtdata.Rows[0]["AUART"].ToString().Trim();
                lblSanctionLoad.Text = dtdata.Rows[0]["SANCTIONED_LOAD"].ToString().Trim();
                lblConsumerName.Text = dtdata.Rows[0]["NAME"].ToString().Trim();
                lblFatherHusbName.Text = dtdata.Rows[0]["FATHER_NAME"].ToString().Trim();
                lblAddress.Text = dtdata.Rows[0]["ADDRESS"].ToString().Trim();
                lblMobile.Text = dtdata.Rows[0]["TEL_NO"].ToString().Trim();
                lblNewMeterNo.Text = dtdata.Rows[0]["METER_NO"].ToString().Trim();

                //lblNewReading.Text = dtdata.Rows[0]["REG_DATE"].ToString().Trim();
                lblNewKWH.Text = dtdata.Rows[0]["MR_KWH"].ToString().Trim();
                lblNewKVAH.Text = dtdata.Rows[0]["MR_KVAH"].ToString().Trim();

                lblNewTermSeal1.Text = dtdata.Rows[0]["TERMINAL_SEAL"].ToString().Trim();
                lblNewTermSeal2.Text = dtdata.Rows[0]["OTHER_SEAL"].ToString().Trim();

                lblNewMeterBoxSeal1.Text = dtdata.Rows[0]["METERBOXSEAL1"].ToString().Trim();
                lblNewMeterBoxSeal2.Text = dtdata.Rows[0]["METERBOXSEAL2"].ToString().Trim();

                lblNewCableSize.Text = dtdata.Rows[0]["CABLESIZE2"].ToString().Trim();
                lblNewCableLen.Text = dtdata.Rows[0]["CABLELENGTH"].ToString().Trim();
                lblOPCable.Text = dtdata.Rows[0]["OUTPUTCABLELENGTH"].ToString().Trim();

                 lblOldMeterNo.Text = dtdata.Rows[0]["OLD_METERNO_SERNR"].ToString().Trim();
               // lblOldReading.Text = dtdata.Rows[0]["REG_DATE"].ToString().Trim();

                lblOldKWH.Text = dtdata.Rows[0]["OLD_MR_KWH"].ToString().Trim();
                lblOldKVAH.Text = dtdata.Rows[0]["MRKVAH_OLD"].ToString().Trim();

                lblOldTermSeal1.Text = dtdata.Rows[0]["REM_TERMINAL_SEAL"].ToString().Trim();
                lblOldTermSeal2.Text = dtdata.Rows[0]["REM_OTHER_SEAL"].ToString().Trim();

                lblOldMeterBoxSeal1.Text = dtdata.Rows[0]["REM_BOX_SEAL1"].ToString().Trim();
                lblOldMeterBoxSeal2.Text = dtdata.Rows[0]["REM_BOX_SEAL2"].ToString().Trim();

                lblOldCableSize.Text = dtdata.Rows[0]["CABLESIZE_OLD"].ToString().Trim();
                lblOldCableLen.Text = dtdata.Rows[0]["CABLELENGTH_OLD"].ToString().Trim();

                lblBagNo.Text = dtdata.Rows[0]["GUNNYBAG_OLD"].ToString().Trim();
                lblBagSealNo.Text = dtdata.Rows[0]["GUNNYBAGSEAL_OLD"].ToString().Trim();
                lblTestingDate.Text = dtdata.Rows[0]["LABTESTING_DATE_OLD"].ToString().Trim();

                lblEngID.Text = dtdata.Rows[0]["PUNCH_BY"].ToString().Trim();                

                //lblNoticeNo.Text = dtdata.Rows[0]["REG_DATE"].ToString().Trim();      

                 GetSignatureImage(lblOrderNo.Text);
            }
        }
        catch (Exception ex)
        {
           
        }
    }

    private void GetSignatureImage(string _sOrderID)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            DataTable _dtDetails = objBL.getBindImageUpload(_sOrderID);
             if (_dtDetails.Rows.Count > 0)
             {
                imgConsumerSign.ImageUrl = "ImageCSharp.aspx?FileName=" + MapImagePath(_dtDetails.Rows[0]["IMAGE_SIGNATURE"].ToString(), sl);
             }
        }
        catch (Exception ex)
        {

        }
    }

    private string MapImagePath(string _sPathFile, string _sS1Path)
    {
        try
        {            
            _sPathFile = _sPathFile.Replace("E:\\MobileServices\\UPLOADEDIMAGES", _sS1Path);
            _sPathFile = _sPathFile.Replace("C:\\MobileServices\\UPLOADEDIMAGES", _sS1Path);
            _sPathFile = _sPathFile.Replace("C:\\mobileservices_new\\UPLOADEDIMAGES", _sS1Path);

            _sPathFile = _sPathFile.Replace("\\", "//");

            // return "file:"+_sPathFile;
            return _sPathFile;
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            // oNetDrive.UnMapDrive();
        }
    }    

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.print();</script>");
    }   

    protected void btnSave_Click(object sender, EventArgs e)
    {       
        //Response.ClearContent();
        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=MCR.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter strWrite = new StringWriter();
        //HtmlTextWriter htmWrite = new HtmlTextWriter(strWrite);
        //HtmlForm frm = new HtmlForm();
        //PrintTable.Parent.Controls.Add(frm);
        //frm.Attributes["runat"] = "server";
        //frm.Controls.Add(PrintTable);
        //frm.RenderControl(htmWrite);
        //StringReader sr = new StringReader(strWrite.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 8f, 8f, 8f, 2f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();

        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.Flush();
        //Response.End();

    }
    protected void ImgPrintBtn_Click(object sender, ImageClickEventArgs e)
    {
        Response.Write("<script>window.print();</script>");
    }
    protected void ImgPdf_Click(object sender, ImageClickEventArgs e)
    {
        //Response.ClearContent();
        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=MCR.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter strWrite = new StringWriter();
        //HtmlTextWriter htmWrite = new HtmlTextWriter(strWrite);
        //HtmlForm frm = new HtmlForm();
        //PrintTable.Parent.Controls.Add(frm);
        //frm.Attributes["runat"] = "server";
        //frm.Controls.Add(PrintTable);
        //frm.RenderControl(htmWrite);
        //StringReader sr = new StringReader(strWrite.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 8f, 8f, 8f, 2f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();

        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.Flush();
        //Response.End();

        WebClient wc = new WebClient();
        string url = Request.Url.AbsoluteUri;
        string fileContent = wc.DownloadString(url);

        List<string> tableContents = GetContents(fileContent, table_pattern);
        string HTMLString = String.Join(" ", tableContents.ToArray());

        string text = HTMLString.Trim();
        Bitmap bitmap = new Bitmap(1, 1);
        Font font = new Font("Arial", 25, FontStyle.Regular, GraphicsUnit.Pixel);
        Graphics graphics = Graphics.FromImage(bitmap);
        int width = (int)graphics.MeasureString(text, font).Width;
        int height = (int)graphics.MeasureString(text, font).Height;
        bitmap = new Bitmap(bitmap, new Size(width, height));
        graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        graphics.DrawString(text, font, new SolidBrush(Color.FromArgb(255, 0, 0)), 0, 0);
        graphics.Flush();
        graphics.Dispose();
        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".jpg";
        bitmap.Save(Server.MapPath("~/images/") + fileName, ImageFormat.Jpeg);
       // imgText.ImageUrl = "~/images/" + fileName;
       // imgText.Visible = true;

    }

    private static List<string> GetContents(string input, string pattern)
    {
        MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.Singleline);
        List<string> contents = new List<string>();
        foreach (Match match in matches)
            contents.Add(match.Value);
        return contents;
    }

}