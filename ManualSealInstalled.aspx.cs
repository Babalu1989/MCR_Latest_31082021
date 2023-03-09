using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SimpleTest;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using System.Web.Services;
using System.Data.OleDb;

public partial class ManualSealInstalled : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();    
    DataTable _dtDetails = new DataTable();
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {                
                Display_UploadFileDoc();
            }            
        }
        else
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("Default.aspx");
        }
    }

    public void Display_UploadFileDoc()
    {
        DataTable _dtFileData = new DataTable();
        _dtFileData = objBL.GetUpload_SealFile();

        gvUploadFile.DataSource = _dtFileData;
        gvUploadFile.DataBind();

        for (int i = 0; i < gvUploadFile.Rows.Count; i++)
        {
            gvUploadFile.Rows[i].Cells[0].Text = (i + 1).ToString();
        }

        if (gvUploadFile.Rows.Count > 0)
            BlankHeader.Visible = false;
        else
            BlankHeader.Visible = true;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if ((FileUpload1.PostedFile.ContentLength != 0))
        {
            DirectoryInfo _DirInfo = new DirectoryInfo(Server.MapPath("~/" + "SEAL_Doc"));
            if (_DirInfo.Exists == false)
                _DirInfo.Create();

            string fn = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            fn = fn.Replace("'", "");
            FileUpload1.PostedFile.SaveAs(_DirInfo + "//" + fn);

            GetData_ExcelManager(_DirInfo);
            SimpleMethods.show("Seals has been successfully updated.");
        }
        else
        {         
            SimpleMethods.show("Please Add Excel File for Upload and Try Again..");
        }
    }

    private void GetData_ExcelManager(DirectoryInfo _DirInfo)
    {
        try
        {            
            string _cmp = string.Empty;
          
            string fn = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            string FileExt = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
            string str = FileUpload1.PostedFile.FileName;
            string file = System.IO.Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
          
            DataTable _dtSheetName = new DataTable();

            if (FileExt == ".xls")
            {
                string path = _DirInfo + "\\" + fn;

                OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + path + ";Extended Properties=Excel 8.0;");
                connection.Open();

                _dtSheetName = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                String[] excelSheets = new String[_dtSheetName.Rows.Count];
                int r = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in _dtSheetName.Rows)
                {
                    excelSheets[r] = row["TABLE_NAME"].ToString();
                    r++;
                }

                OleDbCommand command = new OleDbCommand("SELECT * FROM [" + excelSheets[0].ToString() + "]", connection);
                OleDbDataReader dr;
               
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable excelData = new DataTable("ExcelData");
                excelData.Load(dr);
                dr.Close();
                connection.Close();

                for (int i = 0; i < excelData.Rows.Count; i++)
                {
                    if(excelData.Rows[i][0].ToString().Trim() !="")
                        objBL.Update_MCRSeal_Manual(excelData.Rows[i][0].ToString().ToUpper(), Session["UserName"].ToString(), fn, path);
                }
            }
            else if (FileExt == ".xlsx")
            {
                string path = _DirInfo + "\\" + fn;

                OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0 Xml;");
                connection.Open();

                _dtSheetName = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                String[] excelSheets = new String[_dtSheetName.Rows.Count];
                int r = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in _dtSheetName.Rows)
                {
                    excelSheets[r] = row["TABLE_NAME"].ToString();
                    r++;
                }

                OleDbCommand command = new OleDbCommand("SELECT * FROM [" + excelSheets[0].ToString() + "]", connection);
                OleDbDataReader dr;

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable excelData = new DataTable("ExcelData");
                excelData.Load(dr);
                dr.Close();
                connection.Close();

                for (int i = 0; i < excelData.Rows.Count; i++)
                {
                    if (excelData.Rows[i][0].ToString().Trim() != "")
                        objBL.Update_MCRSeal_Manual(excelData.Rows[i][0].ToString().ToUpper(), Session["UserName"].ToString(), fn, path);
                }
            }

        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please check Excel File Formate for Upload and Try Again..");
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
    }

    protected void lnkUploadedFileName_Command(object sender, CommandEventArgs e)
    {       
        try{
            string s2 = e.CommandArgument.ToString();
            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition","attachment;filename=" + e.CommandName.ToString());
            Response.BinaryWrite(buffer);
            Response.End();
        }
        catch (Exception ex)
        {
            SimpleMethods.show("There is problem in downloading file and Please Try Again..");
        }
        
    }
}