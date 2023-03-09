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

public partial class _Default : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    // Image sortImage = new Image();

    /// <summary>
    /// Developed by Gourav Gouton on Date 20.11.2017 guide given by Piyush Sir
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                txtActFrmDT.Text = DateTime.Now.AddDays(-2).ToString("dd-MMM-yyyy");
                txtActToDT.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                lblRoleCheck.Text = objBL.checkRole(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"])).ToString().Trim();
                if (Convert.ToInt32(lblRoleCheck.Text.Trim()) > 1 || Convert.ToString(Session["ROLE"]) == "PV")
                {
                    trOrderType.Visible = true;
                    BindOrderType(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));
                }

                BindDivisioin();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                getMaineData();
                btnSave.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSave, "") + ";this.value='Please wait....';this.disabled = true;");
            }
        }
        else
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
    }

    public void BindOrderType(string _sVendorID, string _sDivision, string _sRole)  //16032018
    {
        DataTable _dtBindName = objBL.getOrderTypeDetails(_sVendorID, _sDivision, _sRole);
        if (_dtBindName.Rows.Count > 0)
        {
            ddlOrderType.DataSource = _dtBindName;
            ddlOrderType.DataTextField = "ORDER_DESCRIPTION";
            ddlOrderType.DataValueField = "ORDER_TYPE";
            ddlOrderType.DataBind();
            ddlOrderType.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
    }

    protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrderType.SelectedItem.Text != "-ALL-")
        {
            DataTable _dtBindName = objBL.getPM_Activity_OrderWise(ddlOrderType.SelectedValue, ddlVendorName.SelectedValue, Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));    //16032018
            if (_dtBindName.Rows.Count > 0)
            {
                ddlPMActivity.DataSource = _dtBindName;
                ddlPMActivity.DataTextField = "PM_DESCRIPTION";
                ddlPMActivity.DataValueField = "PM_ACTIVTY";
                ddlPMActivity.DataBind();
                ddlPMActivity.Items.Insert(0, new ListItem("-ALL-", "0"));
            }
        }
        else
        {
            ddlPMActivity.Items.Clear();
            ddlPMActivity.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
    }


    public void getMaineData()
    {
        string _sOrdType = string.Empty, _sActType = string.Empty, _sDivision = string.Empty;

        if (Convert.ToInt32(lblRoleCheck.Text.Trim()) > 1 || Convert.ToString(Session["ROLE"]) == "PV")
        {
            if (ddlOrderType.SelectedItem.Text != "-ALL-")
                _sOrdType = ddlOrderType.SelectedValue;
            else
                _sOrdType = ddlOrderType.SelectedItem.Text;

            if (ddlPMActivity.SelectedItem.Text != "-ALL-")
                _sActType = ddlPMActivity.SelectedValue;
            else
                _sActType = ddlPMActivity.SelectedItem.Text;
        }

        if (ddlDivision.SelectedValue != "0")
            _sDivision = ddlDivision.SelectedValue;

        DataTable _dtDetails = objBL.getMCR_Upload(txtFromDate.Text, txtToDate.Text, "", "", _sDivision, Session["Divison"].ToString(), Session["COMPANY"].ToString(),
                                    _sOrdType, _sActType, Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["ROLE"]),
                                    txtActFrmDT.Text, txtActToDT.Text);
        if (_dtDetails.Rows.Count > 0)
        {
            btnExcel.Visible = true;
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();
            if (ViewState["Main"] != null)
            {
                ViewState["Main"] = null;
            }
            ViewState["Main"] = _dtDetails;

            DetailsData_Format();
            SetViewDoc_Image();
        }
        else
        {
            btnExcel.Visible = false;
            gvMainData.DataSource = null;
            gvMainData.DataBind();
            ViewState["Main"] = null;

            //SimpleMethods.show("No Data Found.");
        }
    }

    private void SetViewDoc_Image()
    {
        for (int i = 0; i < gvMainData.Rows.Count; i++)
        {

            ImageButton GrdbtnID = (ImageButton)gvMainData.Rows[i].FindControl("imgComplaintID");

            // if ((gvMainData.Rows[i].Cells[7].Text == "N") && (gvMainData.Rows[i].Cells[8].Text == "N") && (gvMainData.Rows[i].Cells[9].Text == "N"))
            //   GrdbtnID.Visible = false;
            //else
            GrdbtnID.Visible = true;

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "", _gCANO = "", _gMeterNO = "";
            string _gActFrom = "", _gActTo = "", _sDivision = "";
            string _sOrdType = string.Empty, _sActType = string.Empty, _gddlVendorname = string.Empty;

            if (Convert.ToInt32(lblRoleCheck.Text.Trim()) > 1 || Convert.ToString(Session["ROLE"]) == "PV")
            {
                if (ddlOrderType.SelectedItem.Text != "-ALL-")
                    _sOrdType = ddlOrderType.SelectedValue;
                else
                    _sOrdType = ddlOrderType.SelectedItem.Text;

                if (ddlPMActivity.SelectedItem.Text != "-ALL-")
                    _sActType = ddlPMActivity.SelectedValue;
                else
                    _sActType = ddlPMActivity.SelectedItem.Text;
            }

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                if (SimpleMethods.validateDate(txtFromDate.Text, txtToDate.Text) == true)
                {
                    _gFrom = txtFromDate.Text;
                    _gTo = txtToDate.Text;
                }
                else
                {
                    txtFromDate.BackColor = System.Drawing.Color.Yellow;
                    txtToDate.BackColor = System.Drawing.Color.Yellow;
                    SimpleMethods.show("From Date should not be greater than To Date.");
                    return;
                }
            }

            if (txtActFrmDT.Text != "" && txtActToDT.Text != "")
            {
                if (SimpleMethods.validateDate(txtActFrmDT.Text, txtActToDT.Text) == true)
                {
                    _gActFrom = txtActFrmDT.Text;
                    _gActTo = txtActToDT.Text;
                }
                else
                {
                    txtActFrmDT.BackColor = System.Drawing.Color.Yellow;
                    txtActToDT.BackColor = System.Drawing.Color.Yellow;
                    SimpleMethods.show("From Date should not be greater than To Date.");
                    return;
                }
            }

            if (txtCANO.Text.Trim() != "")
                _gCANO = txtCANO.Text.Trim();

            if (txtMeterNo.Text.Trim() != "")
                _gMeterNO = txtMeterNo.Text.Trim();

            if (ddlDivision.SelectedValue != "0")
                _sDivision = ddlDivision.SelectedValue;

            if (ddlVendorName.SelectedValue != "0")
                _gddlVendorname = ddlVendorName.SelectedValue;


            DataTable _dtDetails = objBL.getMCR_Upload(_gFrom, _gTo, _gCANO, _gMeterNO, _sDivision, Session["Divison"].ToString(), Session["COMPANY"].ToString(),
                                                            _sOrdType, _sActType, _gddlVendorname, Convert.ToString(Session["ROLE"]),
                                                            _gActFrom, _gActTo);
            if (_dtDetails.Rows.Count > 0)
            {
                btnExcel.Visible = true;
                gvMainData.DataSource = _dtDetails;
                gvMainData.DataBind();
                if (ViewState["Main"] != null)
                {
                    ViewState["Main"] = null;
                }
                ViewState["Main"] = _dtDetails;

                DetailsData_Format();
                SetViewDoc_Image();
            }
            else
            {
                btnExcel.Visible = false;
                gvMainData.DataSource = null;
                gvMainData.DataBind();
                ViewState["Main"] = null;

                SimpleMethods.show("No Data Found.");
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MCR_Upload.aspx");
    }

    public void BindUploadImages(string OrderID, string OrderType)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;
        string[] strArray = OrderType.Split('-');
        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            // OrderID = "001020178738";

            DataTable _dtDetails = objBL.getBindOrder_ImageUpload(OrderID);
            if (_dtDetails.Rows.Count > 0)
            {
                //Image1.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMAGE1"].ToString());
                ImageButton1.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE1"].ToString(), sl);

                if (ImageButton1.CommandArgument == "")
                {
                    tr1.Visible = false;
                }
                else
                {
                    if (strArray[0].ToString() == "ZMSO" && strArray[2].ToString() == "J06")
                    {
                        lblMtrPhotograph.Text = "Busbar Photo1";
                        tr1.Visible = true;
                        lblMtrPhotograph.Visible = true;
                    }
                    else
                    {
                        lblMtrPhotograph.Text = "Meter Photograph";
                        tr1.Visible = true;
                        lblMtrPhotograph.Visible = true;
                    }

                }

                //Image2.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMAGE2"].ToString());
                ImageButton2.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE2"].ToString(), sl);
                if (ImageButton2.CommandArgument == "")
                {
                    tr2.Visible = false;
                }
                else
                {
                    if (strArray[0].ToString() == "ZMSO" && strArray[2].ToString() == "J06")
                    {
                        lblcompmtrPhotograph.Text = "Busbar Photo2";
                        tr2.Visible = true;
                        lblcompmtrPhotograph.Visible = true;
                    }
                    else
                    {
                        lblcompmtrPhotograph.Text = "Complete Meter Photograph with Background";
                        tr2.Visible = true;
                        lblcompmtrPhotograph.Visible = true;
                    }
                }

                //Image3.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMAGE3"].ToString());
                ImageButton3.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE3"].ToString(), sl);
                if (ImageButton3.CommandArgument == "")
                    tr3.Visible = false;
                else
                    tr3.Visible = true;

                //Image4.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMAGE4"].ToString());
                ImageButton4.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE4"].ToString(), sl);
                if (ImageButton4.CommandArgument == "")
                    tr4.Visible = false;
                else
                    tr4.Visible = true;

                //imgMeterTest.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMAGE_METERTESTREPORT"].ToString());
                ImageMeterTest.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE_METERTESTREPORT"].ToString(), sl);
                if (ImageMeterTest.CommandArgument == "")
                {
                    tr6.Visible = false;
                }
                else
                {
                    if (strArray[0].ToString() == "ZDRP")
                    {
                        lblMtrTest.Text = "Pole end Photograph";
                        tr6.Visible = true;
                        lblMtrTest.Visible = true;
                    }
                    else
                    {
                        lblMtrTest.Text = "Meter Test Report";
                        tr6.Visible = true;
                        lblMtrTest.Visible = true;
                    }
                }

                ////imgLabTest.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMAGE_LABTESTINGREPORT"].ToString());
                ImageLabTest.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE_LABTESTINGREPORT"].ToString(), sl);
                if (ImageLabTest.CommandArgument == "")
                    tr7.Visible = false;
                else
                    tr7.Visible = true;

                //imgSign.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMAGE_SIGNATURE"].ToString());
                ImageSign.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE_SIGNATURE"].ToString(), sl);
                if (ImageSign.CommandArgument == "")
                    tr8.Visible = false;
                else
                    tr8.Visible = true;

                //imgMCR.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMEAGE_MCR"].ToString());
                ImageMCR.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMEAGE_MCR"].ToString(), sl);
                if (ImageMCR.CommandArgument == "")
                {

                    tr5.Visible = false;
                }
                else
                {
                    if (strArray[0].ToString() == "ZDIN")
                    {
                        lblMCR.Text = "Group Photo";
                        tr5.Visible = true;
                        lblMCR.Visible = true;
                    }
                    else if (strArray[0].ToString() == "ZDRP")
                    {
                        lblMCR.Text = "Busbar/Photo3";
                        tr5.Visible = true;
                        lblMCR.Visible = true;
                    }
                    else
                    {
                        lblMCR.Text = "MCR";
                        tr5.Visible = true;
                        lblMCR.Visible = true;
                    }

                }

                ImageOldMeter1.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE1_OLD"].ToString(), sl);
                if (ImageOldMeter1.CommandArgument == "")
                    tr11.Visible = false;
                else
                    tr11.Visible = true;

                ImageOldMeter2.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE2_OLD"].ToString(), sl);
                if (ImageOldMeter2.CommandArgument == "")
                    tr12.Visible = false;
                else
                    tr12.Visible = true;

                ImageAutoMCR.CommandArgument = MapImagePath(_dtDetails.Rows[0]["MCR_PDF"].ToString(), sl);
                //if (ImageAutoMCR.CommandArgument == "")
                //{
                //    if (Session["ROLE"].ToString() != "V")//Added by Babalu Kumar 31082020
                //    {
                //        tr13.Visible = false;
                //    }
                //    else
                //    {
                //        tr13.Visible = false;
                //    }
                //}
                //else
                //{
                //    if (Session["ROLE"].ToString() != "V")
                //    {
                //        tr13.Visible = true;
                //    }
                //    else
                //    {
                //        tr13.Visible = false;
                //    }
                //}
                if (ImageAutoMCR.CommandArgument == "")
                {
                    tr13.Visible = false;
                }
                else
                {
                    tr13.Visible = true;
                }

            }
            else
            {
                tr1.Visible = false;
                tr2.Visible = false;
                tr3.Visible = false;
                tr4.Visible = false;
                tr5.Visible = false;
                tr6.Visible = false;
                tr7.Visible = false;
                tr8.Visible = false;

                tr11.Visible = false;
                tr12.Visible = false;
                tr13.Visible = false;
            }

            _dtDetails = objBL.getBindCancel_ImageUpload(OrderID);
            if (_dtDetails.Rows.Count > 0)
            {
                // imgCancel.ImageUrl = MapImagePath(_dtDetails.Rows[0]["IMAGE_PATH"].ToString());
                ImageCancel.CommandArgument = MapImagePath(_dtDetails.Rows[0]["IMAGE_PATH"].ToString(), sl);
                if (ImageCancel.CommandArgument == "")
                    tr9.Visible = false;
                else
                    tr9.Visible = true;

                ImageCancelSignature.CommandArgument = MapImagePath(_dtDetails.Rows[0]["SIGNATURE_PATH"].ToString(), sl);
                if (ImageCancelSignature.CommandArgument == "")
                    tr14.Visible = false;
                else
                    tr14.Visible = true;

                ImageCancelPDF.CommandArgument = MapImagePath(_dtDetails.Rows[0]["PDF_PATH"].ToString(), sl);
                if (ImageCancelPDF.CommandArgument == "")
                    tr15.Visible = false;
                else
                    tr15.Visible = true;
            }
            else
            {
                tr9.Visible = false;
                tr14.Visible = false;
                tr15.Visible = false;
            }

        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }

    }

    private string MapImagePath(string _sPathFile, string _sS1Path)
    {

        try
        {

            //_sPathFile = _sPathFile.Replace("C:\\MobileServices\\UPLOADEDIMAGES\\", "http://10.125.66.15//MobileServices//UPLOADEDIMAGES//");
            //  _sPathFile = _sPathFile.Replace("C:\\mobileservices_new\\UPLOADEDIMAGES\\", "http://10.125.66.15//MobileServices//UPLOADEDIMAGES//");

            //_sPathFile = _sPathFile.Replace("C:\\MobileServices\\UPLOADEDIMAGES\\", "http://10.125.64.216:7850//MobileServices//UPLOADEDIMAGES//");
            //_sPathFile = _sPathFile.Replace("C:\\mobileservices_new\\UPLOADEDIMAGES\\", "http://10.125.64.216:7890//MobileServices_New//UPLOADEDIMAGES//");

            //_sPathFile = _sPathFile.Replace("E:\\MobileServices\\UPLOADEDIMAGES\\", "http://10.125.66.15//MobileServices//UPLOADEDIMAGES//");
            //_sPathFile = _sPathFile.Replace("C:\\MobileServices\\UPLOADEDIMAGES\\", "http://10.125.66.15//MobileServices//UPLOADEDIMAGES//");
            //_sPathFile = _sPathFile.Replace("C:\\mobileservices_new\\UPLOADEDIMAGES\\", "http://10.125.66.15//MobileServices//UPLOADEDIMAGES//");

            //_sPathFile = _sPathFile.Replace("E:\\MobileServices\\UPLOADEDIMAGES\\", "http://10.125.64.215:7850//MobileServices//UPLOADEDIMAGES//");
            //_sPathFile = _sPathFile.Replace("C:\\MobileServices\\UPLOADEDIMAGES\\", "http://10.125.64.216:7850//MobileServices//UPLOADEDIMAGES//");
            //_sPathFile = _sPathFile.Replace("C:\\mobileservices_new\\UPLOADEDIMAGES\\", "http://10.125.64.216:7890//MobileServices_New//UPLOADEDIMAGES//");



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

    protected void imgEmpID_Command(object sender, CommandEventArgs e)
    {
        BindUploadImages(e.CommandArgument.ToString(), e.CommandName.ToString());
        string Ordertype = e.CommandName.ToString();
        //int i = Convert.ToInt32(e.CommandArgument.ToString());
        //GridViewRow row = gvMainData.Rows[i];
        //string Ordertype = row.Cells[9].Text;
        mp1.Show();
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
    }

    protected void btnExcel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Main"];

        HtmlForm form = new HtmlForm();
        Response.Clear();
        Response.Buffer = true;
        if (_dtDetails.Rows.Count > 0)
        {
            GridView grddetails = new GridView();
            grddetails.DataSource = _dtDetails;
            grddetails.DataBind();
            string filename = "MCR_Upload_Image_Report" + DateTime.Now.ToString() + ".xls";

            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            form.Controls.Add(grddetails);
            this.Controls.Add(form);

            form.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    protected void gvMainData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Main"];
        SetSortDirection(SortDireaction);
        if (_dtDetails != null)
        {
            //Sort the data.
            _dtDetails.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvMainData.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvMainData.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            // gvMainData.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

            DetailsData_Format();
        }
    }
    public string SortDireaction
    {
        get
        {
            if (ViewState["SortDireaction"] == null)
                return string.Empty;
            else
                return ViewState["SortDireaction"].ToString();
        }
        set
        {
            ViewState["SortDireaction"] = value;
        }
    }

    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "ASC")
        {
            _sortDirection = "DESC";
            //sortImage.ImageUrl = "view_sort_ascending.png";

        }
        else
        {
            _sortDirection = "ASC";
            // sortImage.ImageUrl = "view_sort_descending.png";
        }
    }

    public string _sortDirection { get; set; }

    private void DetailsData_Format()
    {
        for (int i = 0; i < gvMainData.Rows.Count; i++)
        {
            gvMainData.Rows[i].Cells[4].Text = gvMainData.Rows[i].Cells[4].Text.TrimStart('0');
            gvMainData.Rows[i].Cells[5].Text = gvMainData.Rows[i].Cells[5].Text.TrimStart('0');
            gvMainData.Rows[i].Cells[6].Text = gvMainData.Rows[i].Cells[6].Text.TrimStart('0');
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageButton1.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Meter_Photograph.jpeg");
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

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageButton2.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Comp_Meter_Photograph.jpeg");
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

    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageButton3.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Meter_Photo_ELCB.jpeg");
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

    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageButton4.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Pole_End_Photograph.jpeg");
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

    protected void ImageMCR_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageMCR.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=MCR_Photograph.jpeg");
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

    protected void ImageMeterTest_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageMeterTest.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Meter_Test_Photograph.jpeg");
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

    protected void ImageLabTest_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageLabTest.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Meter_Lab_Photograph.jpeg");
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

    protected void ImageSign_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageSign.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Sign_Photograph.jpeg");
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

    protected void ImageCancel_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageCancel.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Cancel_Photograph.jpeg");
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

    protected void ImageCancelSign_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageCancelSignature.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Cancel_Sign.jpeg");
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
    protected void ImageCancelPDF_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageCancelPDF.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=Cancel_MCR.pdf");
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

    public void BindDivisioin()
    {
        DataTable _dtBindName = objBL.getDivisionDetails(Convert.ToString(Session["Divison"]));
        if (_dtBindName.Rows.Count > 0)
        {
            ddlDivision.DataSource = _dtBindName;
            ddlDivision.DataTextField = "DIVISION_NAME";
            ddlDivision.DataValueField = "DIST_CD";
            ddlDivision.DataBind();
        }

        ddlDivision.Items.Insert(0, new ListItem("-ALL-", "0"));
        if (_dtBindName.Rows.Count == 1)
        {
            ddlDivision.SelectedIndex = 1;
        }
    }

    public void BindVendor(string Vendorid, string Division, string Roleid)//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendorid, Division, Roleid);
        if (_dtEmpName.Rows.Count > 0)
        {
            ddlVendorName.DataSource = _dtEmpName;
            ddlVendorName.DataTextField = "VENDOR_NAME";
            ddlVendorName.DataValueField = "VENDOR_ID";
            ddlVendorName.DataBind();
            ddlVendorName.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
        {
            ddlVendorName.SelectedIndex = 1;
        }
    }
    protected void ImageOldMeter1_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageOldMeter1.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=OldMeter1.jpeg");
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
    protected void ImageOldMeter2_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageOldMeter2.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=OldMeter2.jpeg");
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
    protected void ImageAutoMCR_Click(object sender, ImageClickEventArgs e)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = ImageAutoMCR.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=AutoMCR.pdf");
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
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}