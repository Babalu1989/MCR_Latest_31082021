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

public partial class AppLogin : System.Web.UI.Page
{
    /// <summary>
    /// Developed by Sanjeev Ranjan on dt 18/01/2019
    /// </summary>
    /// 
    SimpleBL objBL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["LOGIN_TYPE"] != null)
                Response.Redirect("HomePage.aspx");
        }
    }

    public string convertDivisionName(string DivisionName)
    {
        string result = string.Empty;
        if (DivisionName != "")
        {
            result = DivisionName.Replace(",", "','");
        }
        return result;
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string _sCANo = string.Empty;
            string _sOrdNO = string.Empty;

            if (txtPassword.Text.Trim() == "Password")
                txtPassword.Text = "";

            if ((txtOrdNo.Text.Trim() != "") && (txtOrdNo.Text.Trim() != "Order Number"))
                _sOrdNO = "00" + txtOrdNo.Text;

            if (txtUserName.Text.Trim() != "UserID" && txtPassword.Text.Trim() != "Password")
            {
                DataTable _gdtDetails = objBL.GetMCRLogin_CANoWise("000" + txtUserName.Text.Trim(), "0000000000" + txtPassword.Text.Trim(), _sOrdNO);

                if (_gdtDetails.Rows.Count > 0)
                {
                    if (_gdtDetails.Rows[0][0].ToString() == "-1")
                    {
                        SimpleMethods.show("Unable to Connect with Database Server. Please Try Again Later...");
                    }
                    else
                    {
                        _sCANo = "000" + txtUserName.Text.Trim();


                        DataTable _dtDetails = objBL.getBindImageUpload(_sOrdNO);
                        if (_dtDetails.Rows.Count > 0)
                        {
                            LnkPDFDownload.CommandArgument = _dtDetails.Rows[0]["MCR_PDF"].ToString();

                            if (LnkPDFDownload.CommandArgument == "")
                            {
                                LnkPDFDownload.Visible = false;
                                btnLogin.Visible = true;
                            }
                            else
                            {
                                LnkPDFDownload.Visible = true;
                                btnLogin.Visible = false;
                            }
                        }
                        else
                        {
                            if (objBL.GetMCRLooseLogin_CANoWise("0000000000" + txtPassword.Text.Trim()).Rows.Count > 0)
                            {
                                _dtDetails = objBL.getBindImageUpload("0000000000" + txtPassword.Text.Trim());
                                if (_dtDetails.Rows.Count > 0)
                                {
                                    LnkPDFDownload.CommandArgument = _dtDetails.Rows[0]["MCR_PDF"].ToString();

                                    if (LnkPDFDownload.CommandArgument == "")
                                    {
                                        LnkPDFDownload.Visible = false;
                                        btnLogin.Visible = true;
                                    }
                                    else
                                    {
                                        LnkPDFDownload.Visible = true;
                                        btnLogin.Visible = false;
                                    }
                                }
                            }
                            else
                            {
                                _dtDetails = objBL.getBindImageUpload_MeterWise(txtPassword.Text.Trim());
                                if (_dtDetails.Rows.Count > 0)
                                {
                                    LnkPDFDownload.CommandArgument = _dtDetails.Rows[0]["MCR_PDF"].ToString();

                                    if (LnkPDFDownload.CommandArgument == "")
                                    {
                                        LnkPDFDownload.Visible = false;
                                        btnLogin.Visible = true;
                                    }
                                    else
                                    {
                                        LnkPDFDownload.Visible = true;
                                        btnLogin.Visible = false;
                                    }
                                }
                                else
                                {
                                    SimpleMethods.show("Invalid CA Number or Order Number or Meter No.");
                                }
                            }
                        }

                        // Response.Redirect("GenerateMCR.aspx?CANo=" + _sCANo.Trim());
                        //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>window.open('GenerateMCR.aspx?CANo=" + _sCANo.Trim() + "','winnew','height=1200,width=1200,left=0, top=0,resizable=yes,scrollbars=yes,status=yes');</script>");            
                    }
                }
                else
                {
                    if (objBL.GetMCRLooseLogin_CANoWise("0000000000" + txtPassword.Text.Trim()).Rows.Count > 0)
                    {
                        DataTable _dtDetails = objBL.getBindImageUpload("0000000000" + txtPassword.Text.Trim());
                        if (_dtDetails.Rows.Count > 0)
                        {
                            LnkPDFDownload.CommandArgument = _dtDetails.Rows[0]["MCR_PDF"].ToString();

                            if (LnkPDFDownload.CommandArgument == "")
                            {
                                LnkPDFDownload.Visible = false;
                                btnLogin.Visible = true;
                            }
                            else
                            {
                                LnkPDFDownload.Visible = true;
                                btnLogin.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        DataTable _dtDetails = objBL.getBindImageUpload_MeterWise(txtPassword.Text.Trim());
                        if (_dtDetails.Rows.Count > 0)
                        {
                            LnkPDFDownload.CommandArgument = _dtDetails.Rows[0]["MCR_PDF"].ToString();

                            if (LnkPDFDownload.CommandArgument == "")
                            {
                                LnkPDFDownload.Visible = false;
                                btnLogin.Visible = true;
                            }
                            else
                            {
                                LnkPDFDownload.Visible = true;
                                btnLogin.Visible = false;
                            }
                        }
                        else
                        {
                            SimpleMethods.show("Invalid CA Number or Order Number or Meter No.");
                        }

                    }
                }
            }
            else
                SimpleMethods.show("Kindly Enter Valid CA Number or Order Number or Meter No.");
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void LnkPDFDownload_Click(object sender, EventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = LnkPDFDownload.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=MCR.pdf");
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.SuppressContent = true;
            Response.End();
        }
        catch (Exception Ex)
        {
            if (Ex.Message.Contains("Could not find file") == true)
            {
                SimpleMethods.show("Could not find file, Please Try Again...");
            }
        }
    }
}