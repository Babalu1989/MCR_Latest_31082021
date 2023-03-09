using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.IO;
using System.Text;
using System.Globalization;

namespace SimpleTest
{
    public class SendmailDAL
    {
        public SendmailDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public bool sendmail(string sTo, string sFrom, string sSubject, string sBody)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            try
            {
                MailAddress fromAddress = new MailAddress("info@bookarity.com", "Bookarity.com");

                // You can specify the host name or ipaddress of your server
                // Default in IIS will be localhost 
                smtpClient.Host = "mail.bookarity.com";

                //Default port will be 25
                smtpClient.Port = 25;
                smtpClient.Host = "mail.bookarity.com";

                //From address will be given as a MailAddress Object
                message.From = fromAddress;

                // To address collection of MailAddress
                message.To.Add(sTo);
                message.Subject = sSubject;

                // CC and BCC optional
                // MailAddressCollection class is used to send the email to various users
                // You can specify Address as new MailAddress("admin1@yoursite.com")
                //message.CC.Add("shashik@xxxxx.com");
                //message.CC.Add("admin2@yoursite.com");

                //Body can be Html or text format
                //Specify true if it  is html message
                message.IsBodyHtml = false;

                // Message body content
                //string body = "Dear " + bo.RegistrationName + "\n";
                //body += "Your User Name: " + bo.Uni_UserName + "\n";
                //body += "Your Password: " + bo.Passward + "\n";
                message.Body = sBody;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("info@bookarity.com", "@bookarity12");// Send SMTP mail
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public static class RandomPassword
    {
        public static string GetRandomPassword(int length)
        {
            char[] chars = "ATQRUVBC2HISW34@GXYZ01DEF5JK678LMNOP9#".ToCharArray();
            string password = string.Empty;
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);
                //Don't Allow Repetation of Characters
                if (!password.Contains(chars.GetValue(x).ToString()))
                    password += chars.GetValue(x);
                else
                    i--;
            }
            return password;
        }
    }

    public static class Logger
    {
        public static void LogMessageToFile(string errorMessage, string errorDetails)
        {
            SendmailDAL sendMail = new SendmailDAL();
            //sendMail.sendmail("gouravgoutam6@gmail.com", "info@bookarity.com", "Bookarity Log", errorMessage + "<br/><br/>" + errorDetails);
            string directory = "~/Logs";
            if ((!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(directory))))
            {
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(directory));
            }

            string file = "~/Logs/" + "Exception_Log" + "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ".txt";
            if ((!File.Exists(System.Web.HttpContext.Current.Server.MapPath(file))))
            {
                File.Create(System.Web.HttpContext.Current.Server.MapPath(file)).Close();
            }

            using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(file)))
            {
                w.WriteLine("Log Entry : ");
                w.WriteLine("Team Name: " + "Gourav Gourav");
                w.WriteLine("Error Time: " + DateTime.Now);
                w.WriteLine("Error Message: " + errorMessage);
                w.WriteLine("Error Details: " + errorDetails);
                w.WriteLine("_____________________________________________________________________");
                w.Flush();
                w.Close();
            }

        }
    }

    public static class SimpleMethods
    {

        public static bool validateDate(string firstDate, string secondDate)
        {
            int compareResult = DateTime.Compare(Convert.ToDateTime(firstDate),Convert.ToDateTime(secondDate));
            bool result=false;
            if (compareResult <= 0)
            {
                result = true;
            }
            return result;
        }

        public static string convertDate(string date)
        {
            string formatedDate = string.Empty;
            if (date != "")
            {
                formatedDate=Convert.ToDateTime(date).ToShortDateString();
            }
            return formatedDate;
        }

        public static List<int> getCheckedItems(CheckBoxList chklistItems)
        {
            List<int> itemInfo = new List<int>();
            foreach (ListItem item in chklistItems.Items)
            {
                if (item.Selected == true)
                {
                    itemInfo.Add(Convert.ToInt32(item.Value));
                }
            }
            return itemInfo;
        }

        public static void show(string _msg)
        {
            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
            object obj = new object();
            page.ClientScript.RegisterStartupScript(obj.GetType(), "key2", "<script>alert('" + _msg + "');</script>");
        }

        public static void MsgBoxWithLocation(string msg, string Location, Page pageof)
        {
            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
            Label lbl = new Label();
            string message = msg;
            string url = Location;
            string script = "window.onload = function(){ alert('";
            script += message;
            script += "');";
            script += "window.location = '";
            script += url;
            script += "'; }";
            ScriptManager.RegisterClientScriptBlock(pageof, page.GetType(), "UniqueKey", script, true);
            pageof.Controls.Add(lbl);
        }

        public static long securePage(HttpContext context)
        {
            Random random=new Random();
            long randomNumber = random.Next(100000);
            context.Session["number"] = randomNumber;
            return randomNumber;
        }

        public static void ExportToXls(DataTable dataTable,HttpResponse Response,string fileName)
        {
            if (dataTable.Rows.Count > 0)
            {
                string filename = string.Format("{0}{1}.xls",fileName,DateTime.Now);
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dataTable;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                Response.Write(tw.ToString());
                Response.End();
            }
        }

        public static string[] SplitItemNumber(string itemNumber)
        {
            string[] itemNo = new string[2];
            int length = 4;
            string comNo = itemNumber;
            if (comNo.Length == 6)
            {
                length = 3;
            }
            itemNo[0] = comNo.Substring(0, 3);
            itemNo[1] = comNo.Substring(3, length);
            itemNo[0] = itemNo[0].ToUpper();
            itemNo[1] = itemNo[1].PadLeft(4, '0');
            return itemNo;
        }

        public static void ExportGridViewToExcel(GridView gridview, HttpResponse Response, string filename)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", filename));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridview.AllowPaging = false;
            //Change the Header Row back to white color
            gridview.HeaderRow.Style.Add("background-color", "#FFFFFF");
            //Applying stlye to gridview header cells
            for (int i = 0; i < gridview.HeaderRow.Cells.Count; i++)
            {
                gridview.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
            }
            int j = 1;
            //This loop is used to apply stlye to cells based on particular row
            foreach (GridViewRow gvrow in gridview.Rows)
            {
                gvrow.BackColor = System.Drawing.Color.White;
                if (j <= gridview.Rows.Count)
                {
                    if (j % 2 != 0)
                    {
                        for (int k = 0; k < gvrow.Cells.Count; k++)
                        {
                            gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
                        }
                    }
                }
                j++;
            }
            gridview.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
            
        }

        public static GridView RenameHeaders(GridView Headers,string GroupCols)
        {
            string[] columnNames = GroupCols.Split(',');
            for (int i = 1; i <= columnNames.Length; i++)
            {
                Headers.HeaderRow.Cells[i].Text = columnNames[i - 1];
            }
            return Headers;
        }

        public static void ExportGridViewToExcelWithData(GridView gridview, HttpResponse Response, string filename,DataTable dt,string GroupColumns)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", filename));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gridview.AllowPaging = false;
            gridview.DataSource = dt;
            gridview.DataBind();
            if (GroupColumns.Length > 0)
            {
                gridview = RenameHeaders(gridview, GroupColumns);
            }
            //Change the Header Row back to white color
            gridview.HeaderRow.Style.Add("background-color", "#FFFFFF");
            //Applying stlye to gridview header cells
            for (int i = 0; i < gridview.HeaderRow.Cells.Count; i++)
            {
                gridview.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
            }
            int j = 1;
            //This loop is used to apply stlye to cells based on particular row
            foreach (GridViewRow gvrow in gridview.Rows)
            {
                gvrow.BackColor = System.Drawing.Color.White;
                if (j <= gridview.Rows.Count)
                {
                    if (j % 2 != 0)
                    {
                        for (int k = 0; k < gvrow.Cells.Count; k++)
                        {
                            gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
                        }
                    }
                }
                j++;
            }
            gridview.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

        }
    }
}
