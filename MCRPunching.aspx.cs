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
using System.Net;

public partial class _Default : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    Image sortImage = new Image();

    string _gIntallerName = string.Empty;
    int pCount = 0;


    /// <summary>
    /// Developed by Gourav Gouton on Date 14.11.2017 guide given by Piyush Sir
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {               
                BindDivisioin();
                BindEmployeeDropDown();
                GetOrderDropType();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                lblRoleCheck.Text = objBL.checkRole(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"])).ToString().Trim();
                if (Convert.ToInt32(lblRoleCheck.Text.Trim()) > 1 || Convert.ToString(Session["ROLE"]) == "PV")
                {
                    trOrderType.Visible = true;
                    BindOrderType(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));
                }
                btnSave.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSave, "") + ";this.value='Please wait....';this.disabled = true;");
                btnDropOrder.Attributes.Add("onclick", "return confirm('Are you sure, you want to Cancel this Order?');");
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

    public void getDataMCRPunching()
    {
        string _sOrdType = string.Empty, _sActType = string.Empty;
        try
        {
            if (ddlOrderType.SelectedItem.Text != "-ALL-")
                _sOrdType = ddlOrderType.SelectedValue;
            else
                _sOrdType = ddlOrderType.SelectedItem.Text;

            if (ddlPMActivity.SelectedItem.Text != "-ALL-")
                _sActType = ddlPMActivity.SelectedValue;
            else
                _sActType = ddlPMActivity.SelectedItem.Text;

            //DataTable _gdtDetails = objBL.Get_MCR_InputData_Details(Convert.ToString(Session["UserName"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]), "N", "", "", "", "");
            DataTable _gdtDetails = objBL.Get_MCR_InputData_Details(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]),
                                                    Convert.ToString(Session["COMPANY"]), "N", "", "", "", "", "", "", "", "", _sOrdType, _sActType, Convert.ToString(Session["ROLE"]));

            if (_gdtDetails.Rows.Count > 0)
            {
                lblTotalCase.Text = _gdtDetails.Rows.Count.ToString();
                gvMainData.DataSource = _gdtDetails;
                gvMainData.DataBind();

                if (ViewState["DataTable"] != null)
                {
                    btnExcel.Visible = true;
                    ViewState["DataTable"] = null;
                    ViewState["DataTable"] = _gdtDetails;
                }
                else
                {
                    btnExcel.Visible = true;
                    ViewState["DataTable"] = _gdtDetails;
                }

                DetailsData_Format();
            }
            else
            {
                btnSubmit.Visible = false;
                gvEmpDetails.DataSource = null;
                gvEmpDetails.DataBind();
                //SimpleMethods.show("No Record Found.");
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    #region Check Box Selection

    //protected void sellectAll(object sender, EventArgs e)
    //{
    //    int Count = 0;
    //    CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
    //    foreach (GridViewRow row in gvMainData.Rows)
    //    {
    //        if (rdbtnList.SelectedValue == "1")
    //        {
    //            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
    //            if (ChkBoxHeader.Checked == true)
    //            {
    //                Count = Count + 1;
    //                ChkBoxRows.Checked = true;
    //                lblSelectedCase.Text = Count.ToString();


    //            }
    //            else
    //            {
    //                Count = Convert.ToInt32(lblSelectedCase.Text);
    //                Count = Count - 1;
    //                ChkBoxRows.Checked = false;
    //                lblSelectedCase.Text = Count.ToString();
    //            }

    //            if (Count == 0)     //12032018
    //            {
    //                divInstallerList.Visible = false;
    //                TabDropData.Visible = false;
    //            }               

    //            if (Count > 10)
    //            {
    //                ChkBoxRows.Checked = false;
    //                SimpleMethods.show("You can not select more than 10 records");
    //                return;
    //            }
    //        }
    //        if (rdbtnList.SelectedValue == "2" || rdbtnList.SelectedValue == "4")
    //        {
    //            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
    //            if (ChkBoxHeader.Checked == true)
    //            {
    //                Count = Count + 1;
    //                ChkBoxRows.Checked = true;
    //                lblSelectedCase.Text = Count.ToString();
    //            }
    //            else
    //            {
    //                Count = Convert.ToInt32(lblSelectedCase.Text);
    //                Count = Count - 1;
    //                ChkBoxRows.Checked = false;
    //                lblSelectedCase.Text = Count.ToString();
    //            }


    //            TabDropData.Visible = false;
    //        }
    //    }
    //}

    protected void sellectOne(object sender, EventArgs e)
    {
        int Count = 0;
        LinkButton lkbtnHeader = (LinkButton)gvMainData.HeaderRow.FindControl("lkSelectAll");
        //CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
        foreach (GridViewRow row in gvMainData.Rows)
        {
            if (rdbtnList.SelectedValue == "1")
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                if (ChkBoxRows.Checked == true)
                {
                    Count = Count + 1;

                    lblSelectedCase.Text = Count.ToString();
                    if (Count == Convert.ToInt32(lblTotalCase.Text.ToString()))
                        lkbtnHeader.Text = "DeSelect All";

                    //if (Count == 1)
                    //getEmpDetails();    //03042018

                    divInstallerList.Visible = true;
                    TabDropData.Visible = true;
                }
                else
                    lkbtnHeader.Text = "Select All";

                lblSelectedCase.Text = Count.ToString();

                if (Count == 0)     //12032018
                {
                    divInstallerList.Visible = false;
                    TabDropData.Visible = false;
                }

                if (Count > 10)
                {
                    ChkBoxRows.Checked = false;
                    SimpleMethods.show("You can not select more than 10 records");
                    return;
                }
            }
            if (rdbtnList.SelectedValue == "2" || rdbtnList.SelectedValue == "4")
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                if (ChkBoxRows.Checked == true)
                {
                    Count = Count + 1;
                    lblSelectedCase.Text = Count.ToString();
                    if (Count == Convert.ToInt32(lblTotalCase.Text.ToString()))
                        lkbtnHeader.Text = "DeSelect All";
                }
                else
                    lkbtnHeader.Text = "Select All";

                lblSelectedCase.Text = Count.ToString();

                divInstallerList.Visible = false;
                TabDropData.Visible = false;
            }
        }
    }
    #endregion

    //public void getEmpDetails() //03042018
    //{
    //    try
    //    {
    //        //DataTable _gdtEmpDetail = objBL.getEmpDetails(Convert.ToString(Session["UserName"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));
    //        string _sVendorID = "";
    //        if (Convert.ToString(Session["ROLE"]) != "PV")
    //            _sVendorID = Convert.ToString(Session["VENDOR_ID"]).Trim();

    //        DataTable _gdtEmpDetail = objBL.getEmpDetails(_sVendorID, Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));

    //        if (_gdtEmpDetail.Rows.Count > 0)
    //        {
    //            gvEmpDetails.DataSource = _gdtEmpDetail;
    //            gvEmpDetails.DataBind();
    //        }
    //        else
    //        {
    //            gvEmpDetails.DataSource = null;
    //            gvEmpDetails.DataBind();
    //           // SimpleMethods.show("No Record Found.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        SimpleMethods.show("Please Try Again.");
    //    }
    //}

    private int ShowAllocatedMember()
    {
        int _iResult = 1;
        int Count = 0;

        foreach (GridViewRow row in gvMainData.Rows)
        {
            if (rdbtnList.SelectedValue == "1")
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                if (ChkBoxRows.Checked == true)
                {
                    Count = Count + 1;
                }
            }
            if (rdbtnList.SelectedValue == "2" || rdbtnList.SelectedValue == "4")
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                if (ChkBoxRows.Checked == true)
                {
                    Count = Count + 1;
                }
            }
        }

        lblSelectedCase.Text = Count.ToString();

        if (Count == 0)
        {
            _iResult = 0;
        }

        if (Count > 10)
        {
            _iResult = 10;
        }

        return _iResult;
    }


    #region Radio Button Selection
    protected void rdbtnList_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSelectedCase.Text = "0";
        lblTotalCase.Text = "0";
        string _gFlag = "", _gPostingDate = "", _gPostingToDate = "", _gDivision = "";
        string _sOrdType = string.Empty, _sActType = string.Empty;
        divInstallerList.Visible = false;   //12032018
        TabDropData.Visible = false;    //12032018

        if (rdbtnList.SelectedValue != "1")
        {
            tr2.Visible = true;
            tr1.Visible = false;
            tr3.Visible = false;
            tr5.Visible = false;
            tr4.Visible = false;
            tr6.Visible = false;

            if (rdbtnList.SelectedValue == "2")
            {
                _gFlag = "Y";
                gvMainData.Columns[13].Visible = false;
                gvMainData.Columns[0].Visible = true;
                gvMainData.Columns[11].Visible = true;
                gvMainData.Columns[12].Visible = true;
                tr8.Visible = true;

                lblFromDate.Text = "Kitting From Date";
                lblToDate.Text = "Kitting To Date";
                txtPostingDate.Text = "";
                txtPostingToDate.Text = "";
            }

            if (rdbtnList.SelectedValue == "3")
            {
                _gFlag = "C";
                gvMainData.Columns[13].Visible = false;
                gvMainData.Columns[0].Visible = false;
                gvMainData.Columns[11].Visible = true;
                gvMainData.Columns[12].Visible = true;
                tr8.Visible = false;

                lblFromDate.Text = "Activity From Date";
                lblToDate.Text = "Activity To Date";
                txtPostingDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                txtPostingToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }

            if (rdbtnList.SelectedValue == "4")
            {
                _gFlag = "E";
                gvMainData.Columns[13].Visible = true;
                gvMainData.Columns[0].Visible = false;
                gvMainData.Columns[11].Visible = true;
                gvMainData.Columns[12].Visible = true;
                tr8.Visible = false;

                lblFromDate.Text = "Activity From Date";
                lblToDate.Text = "Activity To Date";
                txtPostingDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                txtPostingToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }

            if (txtDivision.SelectedValue != "0")
                _gDivision = txtDivision.SelectedValue;

            if (txtPostingDate.Text.Trim() != "")
                _gPostingDate = txtPostingDate.Text.Trim();

            if (txtPostingToDate.Text.Trim() != "")
                _gPostingToDate = txtPostingToDate.Text.Trim();

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

            DataTable _gdtDetails = objBL.Get_MCR_InputData_Details(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]),
                Convert.ToString(Session["COMPANY"]), _gFlag, _gDivision, "", _gPostingDate, "", "", "", "", _gPostingToDate, _sOrdType, _sActType,
                Convert.ToString(Session["ROLE"])); //16032018

            if (_gdtDetails.Rows.Count > 0)
            {
                lblTotalCase.Text = _gdtDetails.Rows.Count.ToString();
                gvMainData.DataSource = _gdtDetails;
                gvMainData.DataBind();

                if (ViewState["DataTable"] != null)
                {
                    btnExcel.Visible = true;
                    ViewState["`"] = null;
                    ViewState["DataTable"] = _gdtDetails;
                }
                else
                {
                    btnExcel.Visible = true;
                    ViewState["DataTable"] = _gdtDetails;
                }

                DetailsData_Format();
            }
            else
            {
                tr8.Visible = false;
                btnExcel.Visible = false;
                gvMainData.DataSource = null;
                gvMainData.DataBind();

                lblTotalCase.Text = "0";
                //SimpleMethods.show("No Data Found.");
            }
        }
        else
            Response.Redirect("MCRPunching.aspx");
    }
    #endregion

    #region Bind Data

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
        {
            txtDivision.SelectedIndex = 1;
        }
    }

    public void BindVendor(string Vendorid,string Division,string Roleid)
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendorid, Division, Roleid);
        if (_dtEmpName.Rows.Count > 0)
        {
            ddlVendorName.DataSource = _dtEmpName;
            ddlVendorName.DataTextField = "VENDOR_NAME";
            ddlVendorName.DataValueField = "VENDOR_ID";
            ddlVendorName.DataBind();
            ddlVendorName.Items.Insert(0, new ListItem("-All-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
        {
            ddlVendorName.SelectedIndex = 1;
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

    #endregion

    #region Button Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblSelectedCase.Text = "0";
        lblTotalCase.Text = "0";
        string _gDivision = "", _gAddress = "", _gPostingDate = "", _gPostingToDate = "", _gInstallerName = "", _gFlag = "";
        string _sMeterNo = string.Empty, _sOrderNo = string.Empty, _BasicFindate = string.Empty;
        string _sOrdType = string.Empty, _sActType = string.Empty, _sVendorid = string.Empty;
        divInstallerList.Visible = false;   //12032018
        TabDropData.Visible = false;    //12032018

        try
        {
            if (txtMeterNO.Text != "")
                _sMeterNo = txtMeterNO.Text;

            if (txtServiceOrdNo.Text != "")
                _sOrderNo = txtServiceOrdNo.Text;

            if (txtDivision.SelectedValue != "0")
                _gDivision = txtDivision.SelectedValue;

            if (txtAddess.Text.Trim() != "")
                _gAddress = txtAddess.Text.Trim();

            if (txtPostingDate.Text.Trim() != "")
                _gPostingDate = txtPostingDate.Text.Trim();

            if (txtPostingToDate.Text.Trim() != "")
                _gPostingToDate = txtPostingToDate.Text.Trim();

            if (txtBasicFinishDate.Text.Trim() != "")
                _BasicFindate = txtBasicFinishDate.Text.Trim();

            if (ddlInstallerName.SelectedValue != "0")
                _gInstallerName = ddlInstallerName.SelectedItem.Text;

            if (ddlVendorName.SelectedValue == "0")//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
            {
                if (Convert.ToString(Session["ROLE"]) == "V")
                {
                    SimpleMethods.show("Please select vendor name option.");
                    ddlVendorName.Focus();
                    return;
                }
            }
            else
            {
                _sVendorid = ddlVendorName.SelectedValue;//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
            }

            _gFlag = "N";

            if (rdbtnList.SelectedValue == "2")
            {
                _gFlag = "Y";
                gvMainData.Columns[13].Visible = false;
                gvMainData.Columns[0].Visible = true;
                gvMainData.Columns[11].Visible = true;
                gvMainData.Columns[12].Visible = true;
            }

            if (rdbtnList.SelectedValue == "3")
            {
                _gFlag = "C";
                gvMainData.Columns[13].Visible = false;
                gvMainData.Columns[0].Visible = false;
                gvMainData.Columns[11].Visible = true;
                gvMainData.Columns[12].Visible = true;
            }

            if (rdbtnList.SelectedValue == "4")
            {
                _gFlag = "E";
                gvMainData.Columns[13].Visible = true;
                gvMainData.Columns[0].Visible = false;
                gvMainData.Columns[11].Visible = true;
                gvMainData.Columns[12].Visible = true;
            }

            //16032018
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

            //DataTable _gdtDetails = objBL.Get_MCR_InputData_Details(Convert.ToString(Session["UserName"]), Convert.ToString(Session["Divison"]),
            //    Convert.ToString(Session["COMPANY"]), _gFlag, _gDivision, _gAddress, _gPostingDate, _gInstallerName, _sMeterNo, _sOrderNo,
            //    _BasicFindate, _gPostingToDate);

            DataTable _gdtDetails = objBL.Get_MCR_InputData_Details(_sVendorid, Convert.ToString(Session["Divison"]),
                Convert.ToString(Session["COMPANY"]), _gFlag, _gDivision, _gAddress, _gPostingDate, _gInstallerName, _sMeterNo, _sOrderNo,
                _BasicFindate, _gPostingToDate, _sOrdType, _sActType, Convert.ToString(Session["ROLE"]));   //Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 

            if (_gdtDetails.Rows.Count > 0)
            {
                lblTotalCase.Text = _gdtDetails.Rows.Count.ToString();
                gvMainData.DataSource = _gdtDetails;
                gvMainData.DataBind();

                if (ViewState["DataTable"] != null)
                {
                    btnExcel.Visible = true;
                    ViewState["DataTable"] = null;
                    ViewState["DataTable"] = _gdtDetails;
                }
                else
                {
                    btnExcel.Visible = true;
                    ViewState["DataTable"] = _gdtDetails;
                }

                // DetailsData_Format();
            }
            else
            {
                btnExcel.Visible = false;
                gvMainData.DataSource = null;
                gvMainData.DataBind();

                SimpleMethods.show("No Record Found.");
            }


            if (rdbtnList.SelectedValue == "1")
            {
                if (gvMainData.Rows.Count > 0)
                {
                    divInstallerList.Visible = true;
                    TabDropData.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            SimpleMethods.show(ex.Message.ToString());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MCRPunching.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ShowAllocatedMember() == 10)
        {
            SimpleMethods.show("You can not select more than 10 records");
        }
        else
        {
            int count = 0;
            string _gddlEmpName = string.Empty;
            try
            {
                _gddlEmpName = getIntallerID();
                if (_gddlEmpName != "")
                {
                    CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
                    foreach (GridViewRow row in gvMainData.Rows)
                    {
                        CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                        if (ChkBoxRows.Checked == true)
                        {
                            Label _gOrder_id = (Label)row.FindControl("lblOrderID");
                            Label _gMeterNo = (Label)row.FindControl("lblMeterNO");
                            Label _gMobNo = (Label)row.FindControl("lblMobNO");

                            Label _glblAUART = (Label)row.FindControl("lbl_AUART");
                            Label _glblILART_TYPE = (Label)row.FindControl("lbl_ILART_ACTIVITY_TYPE");
                            {
                                if (objBL.MapData_OrderInstaller_InputData(_gOrder_id.Text, _gMeterNo.Text, Convert.ToString(Session["UserName"]),
                                                                            _gddlEmpName, _glblAUART.Text) == 1)
                                {
                                    count = count + 1;
                                    Get_SMSMessage_Text_OrderWise(_gOrder_id.Text, _gddlEmpName, _gMobNo.Text, _glblAUART.Text, row.Cells[2].Text, _glblILART_TYPE.Text);
                                    GetOrderDetails(_gOrder_id.Text);
                                }
                                else
                                {
                                    count = 1;
                                }
                            }
                        }
                    }

                    if (count == 0)
                        SimpleMethods.show("Kindly Select Order.");
                    else
                    {
                        pCount = pCount + count;
                        SimpleMethods.MsgBoxWithLocation("Total Case Pending with " + _gIntallerName + " is " + pCount + ".", "MCRPunching.aspx", this);
                        _gIntallerName = "";
                        pCount = 0;
                    }
                }
                else
                {
                    SimpleMethods.show("Kindly Select Installer.");
                }
            }
            catch (Exception ex)
            {
                SimpleMethods.show(ex.Message.ToString());
            }
        }
    }

    private void Get_SMSMessage_Text_OrderWise(string _sOrderID, string _sIntallerID, string _sMobileNo, string _sOrderType, string _sDivision, string _sOrdSubType)
    {
        string _sMessage = string.Empty, _sHappyCode = string.Empty, _sUnsatisfiedCode = string.Empty, _sAppDate = string.Empty, SMSId = string.Empty;
        int Result = 0;
        int _iDay = 0;
        DataTable _dtSMSMsg = new DataTable();
        DataTable _dtSMSInfo = new DataTable();

        //Random generator = new Random();
        //int r = generator.Next(1000, 1000000);
        //var chars = "0123456789";
        //var stringChars = new char[4];
        //var random = new Random();

        //for (int i = 0; i < stringChars.Length; i++)
        //    stringChars[i] = chars[random.Next(chars.Length)];

        //_sHappyCode = new String(stringChars);

        //for (int i = 0; i < stringChars.Length; i++)
        //    stringChars[i] = chars[random.Next(chars.Length)];

        //_sUnsatisfiedCode = new String(stringChars);

        Random r = new Random(); //Changed By Babalu Kumar Req No. REQ23072020713112 PCN No.1108110802
        _sHappyCode =r.Next(1000, 9999).ToString();
        SMSId = r.Next(1000, 1000000).ToString();
        if (_sHappyCode == null || _sHappyCode == "")
        {
            _sHappyCode = "9999";
        }
        _dtSMSInfo = objBL.GetInstaller_Details(_sIntallerID);
        _dtSMSMsg = objBL.getSMS_Text_OrderWise(_sOrderType, Session["COMPANY"].ToString(), _sDivision, "1", _sOrdSubType); // S1ALN,S1KHP,S1NHP,S1NZD,S1SVR,S2HKS,S2RKP,S2SKT,S2VKJ        
        if ((_dtSMSMsg.Rows.Count > 0) && (_dtSMSInfo.Rows.Count > 0))
        {
            _sMessage = _dtSMSMsg.Rows[0][0].ToString();
            _sAppDate = System.DateTime.Now.AddDays(_iDay).ToString("dd/MM");
            _sMessage = _sMessage.Replace("R1", _sOrderID.Substring(2, _sOrderID.Length - 2));
            _sMessage = _sMessage.Replace("R2", _dtSMSInfo.Rows[0][0].ToString());
            _sMessage = _sMessage.Replace("R3", _dtSMSInfo.Rows[0][1].ToString());
            _sMessage = _sMessage.Replace("R4", _sAppDate);
            _sMessage = _sMessage.Replace("R5", _sHappyCode);
            _sMessage = _sMessage.Replace("R6", _sUnsatisfiedCode);
            Result = objBL.Assign_OrderInstHappCode_InputData(_sOrderID, _sHappyCode);//Changed By Babalu Kumar Req No. REQ23072020713112 PCN No.1108110802
            if (Result > 0)
            {
                if (_sHappyCode != null && _sHappyCode != "")
                {
                    objBL.Insert_SMS_Details(SMSId, _sOrderID, _sMobileNo, _sMessage, _sDivision, Session["COMPANY"].ToString());
                }
            }
        }
    }

    public string getIntallerID()
    {
        string result = string.Empty;
        try
        {
            foreach (GridViewRow row in gvEmpDetails.Rows)
            {
                RadioButton rb = (RadioButton)row.FindControl("rdbtnSelect");
                if (rb.Checked)
                {
                    Label _gMeterAlloted = (Label)row.FindControl("lblMeterAlloted");
                    Label _gEMPNAME = (Label)row.FindControl("lblEMPNAME");
                    _gIntallerName = Convert.ToString(_gEMPNAME.Text);
                    pCount = Convert.ToInt32(_gMeterAlloted.Text);

                    Label rdbtnSelect = (Label)gvEmpDetails.Rows[row.RowIndex].FindControl("lblInstallerID");
                    result = rdbtnSelect.Text;
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show(ex.Message.ToString());
        }
        return result;
    }

    #endregion

    #region Sorting
    protected void gvMainData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["DataTable"];
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

            lblSelectedCase.Text = "0";
            TabDropData.Visible = false;
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
            //  sortImage.ImageUrl = "view_sort_descending.png";
        }
    }

    public string _sortDirection { get; set; }
    #endregion

    #region Bind Chart
    private void BindChart()
    {
        StringBuilder strScript = new StringBuilder();

        DataTable _gdtEmpDetail = objBL.getEmpDetails(Convert.ToString(Session["UserName"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));

        if (_gdtEmpDetail != null)
        {
            try
            {
                strScript.Append(@"<script type='text/javascript'>
                    google.load('visualization', '1', {packages: ['corechart']});</script>

                    <script type='text/javascript'>
                    function drawVisualization() {       
                    var data = google.visualization.arrayToDataTable([
                    ['EMPNAME', 'MeterAlloted', 'SealAlloted'],");

                foreach (DataRow row in _gdtEmpDetail.Rows)
                {
                    strScript.Append("['" + row["EMPNAME"] + "'," + row["MeterAlloted"] + "," +
                        row["SealAlloted"] + "],");
                }

                strScript.Remove(strScript.Length - 1, 1);
                strScript.Append("]);");

                strScript.Append("var options = { title : 'Installer wise Allotements Details', vAxis: {title: 'Count'},  hAxis: {title: 'Installer Name'}, seriesType: 'bars', series: {2: {type: 'area'}} };");
                strScript.Append(" var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));  chart.draw(data, options); } google.setOnLoadCallback(drawVisualization);");
                strScript.Append(" </script>");

                ltScripts.Text = strScript.ToString();
            }
            catch
            {
            }
            finally
            {
                _gdtEmpDetail.Dispose();
                strScript.Clear();
            }
        }
    }
    #endregion

    protected void gvMainData_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gvMainData.SelectedRow;
        lblOrderID.Text = ((Label)row.Cells[1].FindControl("lblOrderID")).Text;
        lblDivision.Text = row.Cells[2].Text.ToString();
        lblCANo.Text = row.Cells[3].Text.ToString();
        lblMeterNo.Text = ((Label)row.Cells[4].FindControl("lblMeterNO")).Text;
        lblAllotedTo.Text = row.Cells[12].Text.ToString();
        mp1.Show();
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
    }


    public void BindEmployeeDropDown()  //03042018
    {
        string _sVendorID = "";
        gvEmpDetails.DataSource = null;
        gvEmpDetails.DataBind();
        ddlEmpName.DataSource = null;
        ddlEmpName.DataBind();
        ddlInstallerName.DataSource = null;
        ddlInstallerName.DataBind();
        ddlUpdateInstaller.DataSource = null;
        ddlUpdateInstaller.DataBind();

        if (Convert.ToString(Session["ROLE"]) != "PV")
            _sVendorID = Convert.ToString(Session["VENDOR_ID"]).Trim();

        DataTable _gdtEmpDetail = objBL.getEmpDetails(_sVendorID, Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));

        if (_gdtEmpDetail.Rows.Count > 0)
        {
            ddlEmpName.DataSource = _gdtEmpDetail;
            ddlEmpName.DataTextField = "EMPNAME";
            ddlEmpName.DataValueField = "EMP_ID";
            ddlEmpName.DataBind();
            ddlEmpName.Items.Insert(0, new ListItem("-Select One-", "0"));

            ddlInstallerName.DataSource = _gdtEmpDetail;
            ddlInstallerName.DataTextField = "EMPNAME";
            ddlInstallerName.DataValueField = "EMP_ID";
            ddlInstallerName.DataBind();
            ddlInstallerName.Items.Insert(0, new ListItem("-Select One-", "0"));

            ddlUpdateInstaller.DataSource = _gdtEmpDetail;
            ddlUpdateInstaller.DataTextField = "EMPLOYEE_NAME";
            ddlUpdateInstaller.DataValueField = "EMP_ID";
            ddlUpdateInstaller.DataBind();
            ddlUpdateInstaller.Items.Insert(0, new ListItem("-Select One-", "0"));

            gvEmpDetails.DataSource = _gdtEmpDetail;
            gvEmpDetails.DataBind();
        }
    }

    protected void btnExcel_Click1(object sender, ImageClickEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["DataTable"];

        HtmlForm form = new HtmlForm();
        Response.Clear();
        Response.Buffer = true;
        if (_dtDetails.Rows.Count > 0)
        {
            GridView grddetails = new GridView();
            grddetails.DataSource = _dtDetails;
            grddetails.DataBind();
            string filename = "Meter_Allocation" + rdbtnList.SelectedItem.Text + DateTime.Now.ToString() + ".xls";

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

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (ddlEmpName.SelectedValue != "0" && Remarks.Text != "")
        {
            objBL.Insert_RevertProcess(lblOrderID.Text, lblAllotedTo.Text, ddlEmpName.SelectedValue, Remarks.Text);
            objBL.Update_Revert_Process(ddlEmpName.SelectedValue, lblOrderID.Text);
            SimpleMethods.MsgBoxWithLocation("Now Order " + lblOrderID.Text + " is Allot to " + ddlEmpName.SelectedItem.Text, "MCRPunching.aspx", this);
        }
        else
        {
            mp1.Show();
            SimpleMethods.show("Kindly Select Installer Name or Enter Some Remarks.");
        }
    }

    protected void btnUpdateInstaller_Click(object sender, EventArgs e)
    {
        string _sMessage = string.Empty, _sHappyCode = string.Empty, _sUnsatisfiedCode = string.Empty, _Id=string.Empty;
        try
        {
            //Random generator = new Random();
            //int r = generator.Next(1000, 1000000);
            //var chars = "0123456789";
            //var stringChars = new char[4];
            //var random = new Random();
            //for (int i = 0; i < stringChars.Length; i++)
            //    stringChars[i] = chars[random.Next(chars.Length)];
            //_sHappyCode = new String(stringChars);
            //for (int i = 0; i < stringChars.Length; i++)
            //    stringChars[i] = chars[random.Next(chars.Length)];
            //_sUnsatisfiedCode = new String(stringChars);


            Random r = new Random(); //Changed By Babalu Kumar Req No. REQ23072020713112 PCN No.1108110802
            _Id = r.Next(1000, 1000000).ToString();

            int _Day = 0;
            if (ShowAllocatedMember() == 10)
            {
                SimpleMethods.show("You can not select more than 10 records");
            }
            else
            {
                int Count = 0;
                if (ddlUpdateInstaller.SelectedValue != "0")
                {
                    CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
                    foreach (GridViewRow row in gvMainData.Rows)
                    {
                        CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                        if (ChkBoxRows.Checked == true)
                        {
                            string OrderNO = string.Empty;
                            string PInstallerName = string.Empty;
                            OrderNO = ((Label)row.Cells[1].FindControl("lblOrderID")).Text;
                            Label _gMobNo = (Label)row.FindControl("lblMobNO");//Babalu Kumar 
                            Label _glblAUART = (Label)row.FindControl("lbl_AUART");// Babalu Kumar 
                            Label _glblILART_TYPE = (Label)row.FindControl("lbl_ILART_ACTIVITY_TYPE");// Babalu Kumar 
                            Label strDiv = (Label)row.FindControl("lblDiv");// Babalu Kumar 
                            PInstallerName = row.Cells[12].Text.ToString();
                            Count = Count + 1;
                            objBL.Insert_RevertProcess(OrderNO, PInstallerName, ddlUpdateInstaller.SelectedValue, "");
                            objBL.Update_Revert_Process(ddlUpdateInstaller.SelectedValue, OrderNO);
                            DataTable _dtHappycode = objBL.GetHappyCode_Details(OrderNO);//03072020 by Babalu Kumar add Promocode
                            objBL.Update_Input_Details(ddlUpdateInstaller.SelectedValue, OrderNO);//29062020 by Babalu Kumar add Promocode
                            DataTable _dtSMSInfo = objBL.GetInstaller_Details(ddlUpdateInstaller.SelectedValue);// Babalu Kumar 
                            DataTable _dtSMSMsg = objBL.getSMS_Text_OrderWise(_glblAUART.Text, Session["COMPANY"].ToString(), strDiv.Text, "1", _glblILART_TYPE.Text);// Babalu Kumar 
                            if ((_dtSMSMsg.Rows.Count > 0) && (_dtSMSInfo.Rows.Count > 0))
                            {
                                string _sAppDate = System.DateTime.Now.AddDays(_Day).ToString("dd/MM");
                                _sMessage = _dtSMSMsg.Rows[0][0].ToString();
                                _sMessage = _sMessage.Replace("R1", OrderNO.Substring(2, OrderNO.Length - 2));
                                _sMessage = _sMessage.Replace("R2", _dtSMSInfo.Rows[0][0].ToString());
                                _sMessage = _sMessage.Replace("R3", _dtSMSInfo.Rows[0][1].ToString());
                                _sMessage = _sMessage.Replace("R4", _sAppDate);
                                _sMessage = _sMessage.Replace("R5", _dtHappycode.Rows[0]["HAPPY_CODE_GEN"].ToString());
                                _sMessage = _sMessage.Replace("R6", _sUnsatisfiedCode);
                                if (_dtHappycode.Rows[0]["HAPPY_CODE_GEN"].ToString() != null && _dtHappycode.Rows[0]["HAPPY_CODE_GEN"].ToString() != "")
                                {
                                    objBL.Update_SMS_Details(_Id, OrderNO, _gMobNo.Text, _sMessage, strDiv.Text, Session["COMPANY"].ToString());
                                }
                            }
                        }
                    }
                    if (Count == 0)
                    {
                        SimpleMethods.show("Kindly Select Order No");
                        return;
                    }
                    SimpleMethods.MsgBoxWithLocation("Order is Alloted to " + ddlUpdateInstaller.SelectedItem.Text, "MCRPunching.aspx", this);
                }
                else
                {
                    ddlUpdateInstaller.BackColor = System.Drawing.Color.Yellow;
                    SimpleMethods.show("Kindly Select Installer Name.");
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show(ex.Message.ToString());
        }
    }

    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt2 = new DataTable();
            int _Day = 0;
            int Result = 0;
            string _sAppDate = System.DateTime.Now.AddDays(_Day).ToString("dd/MM");
            string _sSMSID = string.Empty, _sMobNo = string.Empty, _sMessage = string.Empty,
                _sHappycode = string.Empty, _sInstaller = string.Empty, _sInstallername = string.Empty, OrderType = string.Empty, Telno = string.Empty;
            Random generator = new Random();
            int r = generator.Next(1000, 1000000);
            _sSMSID = r.ToString();
            if (ShowAllocatedMember() == 10)
            {
                SimpleMethods.show("You can not select more than 10 records");
            }
            else
            {
                DataTable _dtMessage = new DataTable();
                CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
                foreach (GridViewRow row in gvMainData.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                    if (ChkBoxRows.Checked == true)
                    {
                        string OrderNO = string.Empty;
                        OrderNO = ((Label)row.Cells[1].FindControl("lblOrderID")).Text;
                        _dtMessage = objBL.getResend_HappyCode(OrderNO);
                        if (_dtMessage.Rows.Count > 0)
                        {
                            _sHappycode = _dtMessage.Rows[0]["HAPPY_CODE_GEN"].ToString();
                            _sInstaller = _dtMessage.Rows[0]["ALLOCATE_TO"].ToString();
                            OrderType = _dtMessage.Rows[0]["AUART"].ToString();
                            Telno = _dtMessage.Rows[0]["TEL_NO"].ToString();
                        }
                        if (_sHappycode == null || _sHappycode == "")
                        {
                            Random random = new Random(); //Changed By Babalu Kumar Req No. REQ23072020713112 PCN No.1108110802
                            _sHappycode = random.Next(1000, 9999).ToString();
                            if (_sHappycode == "" || _sHappycode == null)
                            {
                                _sHappycode = "9999";
                            }
                            Result = objBL.Assign_OrderInstHappCode_InputData(OrderNO, _sHappycode);//Changed By Babalu Kumar Req No. REQ23072020713112 PCN No.1108110802
                        }
                        if (_sInstaller != "" && _sInstaller != null)
                        {
                            dt2 = objBL.GetInstaller(_sInstaller);
                            _sInstallername = dt2.Rows[0]["EMP_NAME"].ToString();
                            _sMobNo = dt2.Rows[0]["MOBILE_NO"].ToString();
                        }
                        if (OrderType == "ZDRP")
                        {
                            _sMessage = "As per BSES representative your Meter needs replacement " + _sInstallername + " No. " + _sMobNo + " will visit you on " + _sAppDate + ".Share Code " + _sHappycode + " if satisfied after work. Team BRPL";
                        }
                        else
                        {
                            _sMessage = "As per your request " + OrderNO.Substring(2, OrderNO.Length - 2) + ", " + _sInstallername + " No. " + _sMobNo + " will visit you on " + _sAppDate + ".Share Code " + _sHappycode + " if satisfied after work. Team BRPL";
                        }
                        if (_sHappycode != null && _sHappycode != "")
                        {
                            objBL.Insert_MobSMSData(_sSMSID, OrderNO, "BRPL", Telno, _sMessage);
                        }
                        {
                            // SendSMS_HelpDesk(_dtMessage.Rows[0][0].ToString(), _dtMessage.Rows[0][1].ToString());
                        }
                        SimpleMethods.show("SMS Resend Successfully");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show(ex.Message.ToString());
        }
    }

    public bool SendSMS_HelpDesk(string _sMobile, string _sMess)
    {
        bool isTrue = false;
        try
        {
            string strUrl = "http://sms6.routesms.com:8080/bulksms/bulksms?username=bsestrans1&password=bsestr12&type=0&dlr=1&destination=" + _sMobile + "&source=BSESRP&message=" + _sMess;

            HttpWebResponse hwres = (HttpWebResponse)WebRequest.Create(strUrl.ToString()).GetResponse();
            string ResponseText;
            while (hwres.StatusCode.ToString().Equals("OK") != true) { }
            if (hwres.StatusCode.ToString().Equals("OK"))
            {
                ResponseText = ParseHttpWebResponse(hwres);
            }
            hwres.Close();
        }
        catch (Exception ex)
        {
            //MessageBox.show("Please Try Again.");
        }

        return isTrue;
    }

    private string ParseHttpWebResponse(HttpWebResponse httpRep)
    {
        string rep;
        StreamReader sr = new StreamReader(httpRep.GetResponseStream());
        rep = sr.ReadToEnd();
        return rep;
    }

    /// <summary>
    /// Developed by Sanjeev Ranjan on dt 23/01/2019
    /// </summary>
    /// 

    private void DetailsData_Format()
    {
        for (int i = 0; i < gvMainData.Rows.Count; i++)
        {
            gvMainData.Rows[i].Cells[3].Text = gvMainData.Rows[i].Cells[3].Text.TrimStart('0');
            ((Label)gvMainData.Rows[i].Cells[1].FindControl("ORDERID")).Text = ((Label)gvMainData.Rows[i].Cells[1].FindControl("ORDERID")).Text.TrimStart('0');
            ((Label)gvMainData.Rows[i].Cells[1].FindControl("METER_NO")).Text = ((Label)gvMainData.Rows[i].Cells[1].FindControl("METER_NO")).Text.TrimStart('0');
        }
    }

    protected void btnDropOrder_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
            Label lblGvOrderID = (Label)row.FindControl("lblOrderID");
            Label lblGvMeterNo = (Label)row.FindControl("lblMeterNO");

            if (ChkBoxRows.Checked == true)
            {
                Update_Order_Status("E", lblGvOrderID.Text, lblGvMeterNo.Text);
            }
        }

        getDataMCRPunching();
        TabDropData.Visible = false;
    }

    private void GetOrderDropType()
    {
        DataTable _dtDorpType = new DataTable();
        _dtDorpType = objBL.getOrderCancel_Type();
        if (_dtDorpType.Rows.Count > 0)
        {
            ddlDropReason.Items.Clear();
            for (int i = 0; i < _dtDorpType.Rows.Count; i++)
            {
                ddlDropReason.Items.Add(_dtDorpType.Rows[i]["NAME"].ToString());
                ddlDropReason.Items[i].Value = _dtDorpType.Rows[i]["ID"].ToString();
            }
        }
    }

    private void Update_Order_Status(string _sFlag, string _sOrderNo, string _sMeterNo)
    {
        objBL.Update_Order_Status(Session["UserName"].ToString(), ddlDropReason.SelectedItem.Text.Trim(), _sFlag, _sOrderNo, _sMeterNo);
        TabDropData.Visible = false;
    }

    protected void LnkGrapgh_Click(object sender, EventArgs e)
    {
        BindChart();
    }

    protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)    //16032018
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

    protected void lkSelectAll_Click(object sender, EventArgs e)
    {
        int Count = 0;
        LinkButton lkbtnHeader = (LinkButton)gvMainData.HeaderRow.FindControl("lkSelectAll");
        //CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
        foreach (GridViewRow row in gvMainData.Rows)
        {
            if (rdbtnList.SelectedValue == "1")
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                //if (ChkBoxHeader.Checked == true)
                if (lkbtnHeader.Text == "Select All")
                {
                    Count = Count + 1;
                    ChkBoxRows.Checked = true;
                    lblSelectedCase.Text = Count.ToString();

                    if (Count == 1)
                        //getEmpDetails();    //03042018

                        divInstallerList.Visible = true;
                    TabDropData.Visible = true;
                }
                else
                {
                    Count = Convert.ToInt32(lblSelectedCase.Text);
                    Count = Count - 1;
                    ChkBoxRows.Checked = false;
                    lblSelectedCase.Text = Count.ToString();
                }

                if (Count == 0)     //12032018
                {
                    divInstallerList.Visible = false;
                    TabDropData.Visible = false;
                }

                if (Count > 10)
                {
                    ChkBoxRows.Checked = false;
                    SimpleMethods.show("You can not select more than 10 records");
                    return;
                }
            }
            if (rdbtnList.SelectedValue == "2" || rdbtnList.SelectedValue == "4")
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                if (lkbtnHeader.Text == "Select All")
                {
                    Count = Count + 1;
                    ChkBoxRows.Checked = true;
                    lblSelectedCase.Text = Count.ToString();
                }
                else
                {
                    Count = Convert.ToInt32(lblSelectedCase.Text);
                    Count = Count - 1;
                    ChkBoxRows.Checked = false;
                    lblSelectedCase.Text = Count.ToString();
                }

                divInstallerList.Visible = false;
                TabDropData.Visible = false;
            }
        }

        if (lkbtnHeader.Text == "Select All")
            lkbtnHeader.Text = "DeSelect All";
        else if (lkbtnHeader.Text == "DeSelect All")
            lkbtnHeader.Text = "Select All";
    }

    protected void txtDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", txtDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }

    public void GetOrderDetails(string strOrder)
    {
        DataTable _dtOrder = objBL.GetOrderData(strOrder);
        if (_dtOrder.Rows.Count > 0)
        {
            objBL.OrderUpdateCode(strOrder, _dtOrder.Rows[0]["ZZ_CONNTYPE"].ToString());
        }
    }
}