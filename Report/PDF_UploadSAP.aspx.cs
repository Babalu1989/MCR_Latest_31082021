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

public partial class Report_PDF_UploadSAP : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                BindDivisioin();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor                 
                lblRoleCheck.Text = objBL.checkRole(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"])).ToString().Trim();
                if (Convert.ToInt32(lblRoleCheck.Text.Trim()) > 1 || Convert.ToString(Session["ROLE"]) == "PV")
                {
                    trOrderType.Visible = true;
                    BindOrderType(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));
                }
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

    public void BindOrderType(string _sVendorID, string _sDivision, string _sRole) 
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
    public void BindVendor(string Vendorid, string Division, string Roleid)//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendorid, Division, Roleid);
        if (_dtEmpName.Rows.Count > 0)
        {
            ddlVendorName.DataSource = _dtEmpName;
            ddlVendorName.DataTextField = "VENDOR_NAME";
            ddlVendorName.DataValueField = "VENDOR_ID";
            ddlVendorName.DataBind();
            ddlVendorName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
            ddlVendorName.SelectedIndex = 1;
    }
   protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrderType.SelectedItem.Text != "-ALL-")
        {
            DataTable _dtBindName = objBL.getPM_Activity_OrderWise(ddlOrderType.SelectedValue, Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));    //16032018
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
        string _sOrdType = string.Empty, _sActType = string.Empty, _sDivision=string.Empty;

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

            if (txtDivision.SelectedItem.Text != "-ALL-")
                _sDivision = txtDivision.SelectedValue;
            else
                _sDivision = "";
        }

        DataTable _dtDetails = objBL.getMCR_PDF_SAP_Details(txtFromDate.Text, txtToDate.Text, "", "", Session["COMPANY"].ToString(),
                                                    Session["Divison"].ToString(), _sOrdType, _sActType, Convert.ToString(Session["VENDOR_ID"]),
                                                    Convert.ToString(Session["ROLE"]), _sDivision,rbdAction.SelectedValue);
        if (_dtDetails.Rows.Count > 0)
        {
            btnExcel.Visible = true;
            btnSAPUpload.Visible = true;
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();
            if (ViewState["Main"] != null)
            {
                ViewState["Main"] = null;
            }
            ViewState["Main"] = _dtDetails;
        }
        else
        {
            btnExcel.Visible = false;
            btnSAPUpload.Visible = false;
            gvMainData.DataSource = null;
            gvMainData.DataBind();
            ViewState["Main"] = null;

            //SimpleMethods.show("No Data Found.");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "", _gCANO = "", _gOrderNO = "";
            string _sOrdType = string.Empty, _sActType = string.Empty, _sDivision = string.Empty, _sVendor = string.Empty;

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

            if (txtCANO.Text.Trim() != "")
                _gCANO = txtCANO.Text.Trim();

            if (txtOrderNo.Text.Trim() != "")
                _gOrderNO = txtOrderNo.Text.Trim();

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

            if (ddlVendorName.SelectedItem.Text != "-ALL-")
                _sVendor = ddlVendorName.SelectedValue;
            else
                _sVendor = "";
            if (txtDivision.SelectedItem.Text != "-ALL-")
                _sDivision = txtDivision.SelectedValue;
            else
                _sDivision = "";

            //showdata(_gFrom, _gTo, _gCANO, _gOrderNO, Session["COMPANY"].ToString(),
            //                                            Session["Divison"].ToString(), _sOrdType, _sActType, Convert.ToString(Session["VENDOR_ID"]),
            //                                            Convert.ToString(Session["ROLE"]), _sDivision, rbdAction.SelectedValue);

            DataTable _dtDetails = objBL.getMCR_PDF_SAP_Details(_gFrom, _gTo, _gCANO, _gOrderNO, Session["COMPANY"].ToString(),
                                                            Session["Divison"].ToString(), _sOrdType, _sActType, _sVendor,
                                                            Convert.ToString(Session["ROLE"]), _sDivision, rbdAction.SelectedValue);
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

                if (rbdAction.SelectedValue == "U")
                {
                    btnSAPUpload.Visible = true;
                    gvMainData.Columns[12].Visible = true;
                }
                else
                {
                    btnSAPUpload.Visible = false;
                    gvMainData.Columns[12].Visible = false;
                }
            }
            else
            {
                btnExcel.Visible = false;
                btnSAPUpload.Visible = false;
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

    /////Babalu Kumar
    //public void showdata(string _From, string _To, string _CANO, string _OrderNO, string _Company,
    //                                                       string _division,string _sOrdType, string _sActType, string _vendor,
    //                                                       string _srole, string _sDivision,string _rbdAction)
    //{
    //    DataTable _dtDetails = objBL.getMCR_PDF_SAP_Details(_From, _To, _CANO, _OrderNO, Session["COMPANY"].ToString(),
    //                                                    Session["Divison"].ToString(), _sOrdType, _sActType, Convert.ToString(Session["VENDOR_ID"]),
    //                                                    Convert.ToString(Session["ROLE"]), _sDivision, rbdAction.SelectedValue);
    //    if (_dtDetails.Rows.Count > 0)
    //    {
    //        btnExcel.Visible = true;

    //        gvMainData.DataSource = _dtDetails;
    //        gvMainData.DataBind();



    //        if (ViewState["Main"] != null)
    //        {
    //            ViewState["Main"] = null;
    //        }
    //        ViewState["Main"] = _dtDetails;

    //        DetailsData_Format();

    //        if (rbdAction.SelectedValue == "U")
    //        {
    //            btnSAPUpload.Visible = true;
    //            gvMainData.Columns[12].Visible = true;
    //        }
    //        else
    //        {
    //            btnSAPUpload.Visible = false;
    //            gvMainData.Columns[12].Visible = false;
    //        }
    //    }
    //    else
    //    {
    //        btnExcel.Visible = false;
    //        btnSAPUpload.Visible = false;
    //        gvMainData.DataSource = null;
    //        gvMainData.DataBind();
    //        ViewState["Main"] = null;

    //        SimpleMethods.show("No Data Found.");
    //    }

    //}
    ///////

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PDF_UploadSAP.aspx");
    }

    protected void btnExcel_Click(object sender, EventArgs e)
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
            string filename = "MCR_PDF_SAP" + DateTime.Now.ToString() + ".xls";

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

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
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
            //gvMainData.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
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
           // sortImage.ImageUrl = "view_sort_ascending.png";

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

    public void BindDivisioin()
    {
        DataTable _dtBindName = objBL.getDivisionDetails(Convert.ToString(Session["Divison"]));
        if (_dtBindName.Rows.Count > 0)
        {
            txtDivision.DataSource = _dtBindName;
            txtDivision.DataTextField = "DIVISION_NAME";
            txtDivision.DataValueField = "DIST_CD";
            txtDivision.DataBind();
            txtDivision.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
        if (_dtBindName.Rows.Count == 1)
            txtDivision.SelectedIndex = 1;
    }

    protected void ImageMCRPDF_Command(object sender, CommandEventArgs e)    
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;
        string _sPdfName = "000" + e.CommandName.ToString()+".pdf";
        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string s2 = e.CommandArgument.ToString();

            FileStream livestream = new FileStream(s2, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)livestream.Length];
            livestream.Read(buffer, 0, (int)livestream.Length);
            livestream.Close();
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("content-disposition",
                     "attachment;filename=" + _sPdfName.ToString());
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


    private void TransferData_SAP(string _sOrdNo)
    {
        DataTable _dt = new DataTable();
        _dt = objBL.GetData_OrderSendSAP(_sOrdNo);
        if (_dt.Rows.Count > 0)
        {
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                if (_dt.Rows[i]["MCR_PDF"].ToString() != "")
                {
                    TranferImageSap_MCR_PDF(MapImagePath(_dt.Rows[i]["MCR_PDF"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "0");
                }

                if (_dt.Rows[i]["IMAGE1"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE1"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "1");
                }
                if (_dt.Rows[i]["IMAGE2"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE2"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "2");
                }
                if (_dt.Rows[i]["IMAGE3"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE3"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "3");
                }
                if (_dt.Rows[i]["IMEAGE_MCR"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMEAGE_MCR"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "4");
                }
                if (_dt.Rows[i]["IMAGE_METERTESTREPORT"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE_METERTESTREPORT"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "5");
                }
                if (_dt.Rows[i]["IMAGE_LABTESTINGREPORT"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE_LABTESTINGREPORT"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "6");
                }
                if (_dt.Rows[i]["IMAGE_SIGNATURE"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE_SIGNATURE"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "7");
                }
                if (_dt.Rows[i]["IMAGE4"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE4"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "8");
                }

                if (_dt.Rows[i]["IMAGE1_OLD"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE1_OLD"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "9");
                }
                if (_dt.Rows[i]["IMAGE2_OLD"].ToString() != "")
                {
                    TranferImageSap(MapImagePath(_dt.Rows[i]["IMAGE2_OLD"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["ORDERID"].ToString(), "10");
                }


                Transfer_ImagePDF_In_SAP(_dt.Rows[i]["ORDERID"].ToString());                

            }
        }
    }

    public string MapImagePath(string _sPathFile)
    {
        try
        {
            _sPathFile = _sPathFile.Replace("C:\\MobileServices\\UPLOADEDIMAGES\\", "D:\\MobileServices\\UPLOADEDIMAGES\\");
            _sPathFile = _sPathFile.Replace("C:\\mobileservices_new\\UPLOADEDIMAGES\\", "D:\\MobileServices\\UPLOADEDIMAGES\\");
            _sPathFile = _sPathFile.Replace("E:\\MobileServices\\UPLOADEDIMAGES\\", "D:\\MobileServices\\UPLOADEDIMAGES\\");

            return _sPathFile;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    public void TranferImageSap(string _gImagePath, string _gCompanyCode, string _gCANO, string _gOrderNO, string _gCount)
    {
        string pth = "";        

        if ((_gImagePath.Substring(0, 1) != "C") && (_gImagePath.Substring(0, 1) != "E") && (_gImagePath.Substring(0, 1) != "D"))
        {
            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
        }       

        if (_gCompanyCode == "BRPL")
        {
            pth = @"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB";

            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB";
            nd.MapNetworkDrive(@"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB", "Z:", "DSSUSER", "dssuser@123");
        }        

        try
        {
           
            string sl = pth;
            string _sDir = sl;

            DirectoryInfo _DirInfo = new DirectoryInfo(_sDir);
            if (_DirInfo.Exists == false)
                _DirInfo.Create();

            if (File.Exists(_gImagePath) == true)
            {
                if (File.Exists(_sDir + "\\" + _gCANO + "_" + _gCount + ".jpeg") == false)
                    File.Copy(_gImagePath, _sDir + "\\" + _gCANO + "_" + _gCount + ".jpeg");

                if (File.Exists(_sDir + "\\" + _gCANO + "_" + _gCount + ".jpeg") == true)
                {
                    objBL.Insert_MCR_jpeg_Log(_gOrderNO, _gCANO,_gCount, _sDir, _gImagePath);                                        
                }
            }
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            
        }
    }

    public void TranferImageSap_MCR_PDF(string _gImagePath, string _gCompanyCode, string _gCANO, string _gOrderNO, string _gCount)
    {
        string pth = "";        

        if ((_gImagePath.Substring(0, 1) != "C") && (_gImagePath.Substring(0, 1) != "E") && (_gImagePath.Substring(0, 1) != "D"))
        {
            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
        }

        if (_gCompanyCode == "BRPL")
        {
            pth = @"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB";
            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB";
            nd.MapNetworkDrive(@"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB", "Z:", "DSSUSER", "dssuser@123");
        }        
        try
        {

            string sl = pth;
            string _sDir = sl;

            DirectoryInfo _DirInfo = new DirectoryInfo(_sDir);
            if (_DirInfo.Exists == false)
                _DirInfo.Create();

            if (File.Exists(_gImagePath) == true)
            {

                if (File.Exists(_sDir + "\\" + _gOrderNO + "_" + _gCount + ".pdf") == false)
                    File.Copy(_gImagePath, _sDir + "\\" + _gOrderNO + "_" + _gCount + ".pdf");

                if (File.Exists(_sDir + "\\" + _gOrderNO + "_" + _gCount + ".pdf") == true)
                {
                    objBL.Insert_MCR_PDF_Log(_gOrderNO, _gCANO, _sDir + "\\" + _gOrderNO + "_" + _gCount + ".pdf", _gImagePath);
                    objBL.Update_MCR_PDF(_gOrderNO);                   
                }
            }
            else
            {
                objBL.Update_MCR_PDF(_gOrderNO);                 
            }
        }
        catch (Exception ex)
        {
           
        }
        finally
        {
            
        }
    }

    public void Transfer_ImagePDF_In_SAP(string _gOrderNO)
    {
        try
        {
            DataTable _dt = objBL.GetData_PDFOrderSAP(_gOrderNO);
            if (_dt.Rows.Count > 0)
            {
                if (_dt.Rows[0]["PDF_FLAG"].ToString() == "Y")
                {
                    objBL.Update_TansferSAP_Flag(_gOrderNO);
                }                
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    protected void btnSAPUpload_Click(object sender, ImageClickEventArgs e)
    {
        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSAP_Flag");
            if (ChkBoxRows.Checked == true)
            {
                if(row.Cells[10].Text.Trim()=="Order")
                    TransferData_SAP("00"+row.Cells[6].Text);
                else
                    TransferLooseData_SAP(row.Cells[5].Text);
            }
            if (ChkBoxRows.Checked == true)
            {
                ChkBoxRows.Checked = false;
            }
        }
        Response.Write("<script>alert('PDF Uploaded Successfully!!')</script>");
    }

    private void TransferLooseData_SAP(string _sMeterNo)
    {
        DataTable _dt = new DataTable();
        _dt = objBL.GetData_LooseSendSAP(_sMeterNo);
        if (_dt.Rows.Count > 0)
        {
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                if (_dt.Rows[i]["MCR_PDF"].ToString() != "")
                {                    
                    TranferLooseImageSap_MCR_PDF(MapImagePath(_dt.Rows[i]["MCR_PDF"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "0");
                }

                if (_dt.Rows[i]["IMAGE1"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE1"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "1");
                }
                if (_dt.Rows[i]["IMAGE2"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE2"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "2");
                }
                if (_dt.Rows[i]["IMAGE3"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE3"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "3");
                }
                if (_dt.Rows[i]["IMEAGE_MCR"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMEAGE_MCR"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "4");
                }
                if (_dt.Rows[i]["IMAGE_METERTESTREPORT"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE_METERTESTREPORT"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "5");
                }
                if (_dt.Rows[i]["IMAGE_LABTESTINGREPORT"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE_LABTESTINGREPORT"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "6");
                }
                if (_dt.Rows[i]["IMAGE_SIGNATURE"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE_SIGNATURE"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "7");
                }
                if (_dt.Rows[i]["IMAGE4"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE4"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "8");
                }

                if (_dt.Rows[i]["IMAGE1_OLD"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE1_OLD"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "9");
                }
                if (_dt.Rows[i]["IMAGE2_OLD"].ToString() != "")
                {
                    TranferLooseImageSap(MapImagePath(_dt.Rows[i]["IMAGE2_OLD"].ToString()), _dt.Rows[i]["COMP_CODE"].ToString(), _dt.Rows[i]["CA_NO"].ToString(), _dt.Rows[i]["deviceno"].ToString(), "10");
                }


                TransferLoose_ImagePDF_In_SAP(_dt.Rows[i]["deviceno"].ToString());

            }
        }
    }

    public void TranferLooseImageSap(string _gImagePath, string _gCompanyCode, string _gCANO, string _gDeviceNo, string _gCount)
    {
        string pth = "";

        if ((_gImagePath.Substring(0, 1) != "C") && (_gImagePath.Substring(0, 1) != "E") && (_gImagePath.Substring(0, 1) != "D"))
        {
            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
        }
        if (_gCompanyCode == "BRPL")
        {
            pth = @"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB";
            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB";
            nd.MapNetworkDrive(@"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB", "Z:", "DSSUSER", "dssuser@123");
        }
        try
        {
            string sl = pth;
            string _sDir = sl;
            DirectoryInfo _DirInfo = new DirectoryInfo(_sDir);
            if (_DirInfo.Exists == false)
                _DirInfo.Create();
            if (File.Exists(_gImagePath) == true)
            {
                if (File.Exists(_sDir + "\\" + _gCANO + "_" + _gCount + ".jpeg") == false)
                    File.Copy(_gImagePath, _sDir + "\\" + _gCANO + "_" + _gCount + ".jpeg");

                if (File.Exists(_sDir + "\\" + _gCANO + "_" + _gCount + ".jpeg") == true)
                {
                    objBL.Insert_LooseMCR_jpeg_Log(_gDeviceNo, _gCANO, _gCount, _sDir, _gImagePath);
                }
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    public void TranferLooseImageSap_MCR_PDF(string _gImagePath, string _gCompanyCode, string _gCANO, string _gDeviceNo, string _gCount)
    {
        string pth = "";
        string username = "";
        string password = "";

        if ((_gImagePath.Substring(0, 1) != "C") && (_gImagePath.Substring(0, 1) != "E") && (_gImagePath.Substring(0, 1) != "D"))
        {
            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
        }

        if (_gCompanyCode == "BRPL")
        {
            pth = @"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB";

            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB";
            nd.MapNetworkDrive(@"\\10.125.64.228\DSS\IN_BRPL\MMG_TAB", "Z:", "DSSUSER", "dssuser@123");
        }

        if (_gCompanyCode == "BYPL")
        {
            pth = @"\\10.125.64.228\DSS\IN_BYPL\MMG_TAB";

            Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
            string Extpth = string.Empty;
            Extpth = @"\\10.125.64.228\DSS\IN_BYPL\MMG_TAB";
            nd.MapNetworkDrive(@"\\10.125.64.228\DSS\IN_BYPL\MMG_TAB", "Z:", "DSSUSER", "dssuser@123");
        }


        try
        {

            string sl = pth;

            string _sDir = sl;

            DirectoryInfo _DirInfo = new DirectoryInfo(_sDir);
            if (_DirInfo.Exists == false)
                _DirInfo.Create();

            if (File.Exists(_gImagePath) == true)
            {
                if (File.Exists(_sDir + "\\" + _gCANO + "_" + _gCount + ".pdf") == false)
                    File.Copy(_gImagePath, _sDir + "\\" + _gCANO + "_" + _gCount + ".pdf");

                if (File.Exists(_sDir + "\\" + _gCANO + "_" + _gCount + ".pdf") == true)
                {
                    objBL.Insert_LooseMCR_PDF_Log(_gDeviceNo, _gCANO, _sDir + "\\" + _gCANO + "_" + _gCount + ".pdf", _gImagePath);
                    objBL.Update_LooseMCR_PDF(_gDeviceNo);                                         
                }
            }
            else
            {
                objBL.Update_LooseMCR_PDF(_gDeviceNo);   
            }
        }
        catch (Exception ex)
        {
           
        }
        finally
        {
            
        }
    }
    
    public void TransferLoose_ImagePDF_In_SAP(string _gDeviceNo)
    {
        try
        {
            DataTable _dt = objBL.GetData_LoosePDFOrderSAP(_gDeviceNo);
            if (_dt.Rows.Count > 0)
            {
                if (_dt.Rows[0]["PDF_FLAG"].ToString() == "Y")
                {
                    objBL.Update_LooseTansferSAP_Flag(_gDeviceNo);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void txtDivision_SelectedIndexChanged(object sender, EventArgs e)//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        BindVendor("", txtDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}
