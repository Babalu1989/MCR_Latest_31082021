using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SimpleTest;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

public partial class frmLooseMeterPunching : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    string _gIntallerName = string.Empty;
    int pCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                BindVendorDropDown();
                BindDivisioin();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));

                if (rdbtnList.SelectedValue == "1") //pending For Allocation
                {
                    tr2.Visible = false;
                    gvMainData.Columns[0].Visible = true;
                    gvMainData.Columns[12].Visible = false;
                    gvMainData.Columns[13].Visible = false;
                    getInstallerDetails();
                }
                else if (rdbtnList.SelectedValue == "2")    //Alloted Cases
                {
                    tr2.Visible = false;
                    gvMainData.Columns[0].Visible = false;
                    gvMainData.Columns[12].Visible = true;
                    gvMainData.Columns[13].Visible = true;
                    gvEmpDetails.DataSource = null;
                    gvEmpDetails.DataBind();
                    divVendorList.Visible = false;  //09032018
                }

                btnSave.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSave, "") + ";this.value='Please wait....';this.disabled = true;");
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

    public void getInstallerDetails()
    {
        try
        {
            string _sVendorID = string.Empty;

            if (Convert.ToString(Session["ROLE"]) != "PV")
                _sVendorID = Convert.ToString(Session["VENDOR_ID"]).Trim();
            DataTable _gdtEmpDetail = objBL.getEmpDetails_LosseMeter(_sVendorID, Convert.ToString(Session["COMPANY"]), Convert.ToString(Session["Divison"]));//Added By Babalu Kumar 29092020 Division Check
            if (_gdtEmpDetail.Rows.Count > 0)
            {
                gvEmpDetails.DataSource = _gdtEmpDetail;
                gvEmpDetails.DataBind();
                ddlUpdateInstaller.DataSource = _gdtEmpDetail;
                ddlUpdateInstaller.DataTextField = "EMPNAME";
                ddlUpdateInstaller.DataValueField = "EMP_ID";
                ddlUpdateInstaller.DataBind();
                ddlUpdateInstaller.Items.Insert(0, new ListItem("-Select One-", "0"));
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    public void BindVendorDropDown()
    {
        // DataTable _gdtEmpDetail = objBL.getVendorDetails(Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));
        //{
        //    ddlVendorName.DataSource = _gdtEmpDetail;
        //    ddlVendorName.DataTextField = "VENDOR_NAME";
        //    ddlVendorName.DataValueField = "VENDOR_ID";
        //    ddlVendorName.DataBind();
        //    ddlVendorName.Items.Insert(0, new ListItem("-Select One-", ""));            
        //}

        // ddlVendorName.Items.Insert(0, new ListItem("-Select One-", "")); 
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string _gPostingDateFrom = "", _gPostingDateTo = "";
        string _sMeterNoFrom = "", _sMeterNoTo = "", _sMaterialDocNo = "", _sSehemeNO = "", _sVendorid = "";

        if ((txtMeterNoFrom.Text.Trim() != "" && txtMeterNoTo.Text.Trim() == "") || (txtMeterNoFrom.Text.Trim() == "" && txtMeterNoTo.Text.Trim() != ""))
        {
            SimpleMethods.show("Please Enter Meter Series From & To");
            return;
        }

        if ((txtPostingDateFrom.Text.Trim() != "" && txtPostingDateTo.Text.Trim() == "") || (txtPostingDateFrom.Text.Trim() == "" && txtPostingDateTo.Text.Trim() != ""))
        {
            SimpleMethods.show("Please Enter Posting Date From & To");
            return;
        }

        if (txtPostingDateFrom.Text.Trim() != "" && txtPostingDateTo.Text.Trim() != "")
        {
            DateTime postingDateFrom = Convert.ToDateTime(txtPostingDateFrom.Text.Trim());
            DateTime postingDateTo = Convert.ToDateTime(txtPostingDateTo.Text.Trim());

            int duration = postingDateTo.CompareTo(postingDateFrom);
            if (duration < 0)
            {
                SimpleMethods.show("To Date Must Be Greater Than Or Equal To From Date.");
                return;
            }
        }

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
        try
        {
            if (txtMaterialDocNo.Text != "")
                _sMaterialDocNo = txtMaterialDocNo.Text.Trim();

            if (ddlScehme.SelectedItem.Text != "-ALL-")
                _sSehemeNO = ddlScehme.Text.Trim();
            else
            {
                for (int i = 1; i < ddlScehme.Items.Count; i++)
                {
                    _sSehemeNO += ddlScehme.Items[i].Text;
                    _sSehemeNO += ",";
                }

                if (_sSehemeNO.Length > 1)
                {
                    _sSehemeNO = _sSehemeNO.Substring(0, _sSehemeNO.Length - 1);
                    _sSehemeNO = _sSehemeNO.Replace(",", "','");
                }
            }

            if (txtMeterNoFrom.Text != "")
                _sMeterNoFrom = txtMeterNoFrom.Text.Trim();

            if (txtMeterNoTo.Text != "")
                _sMeterNoTo = txtMeterNoTo.Text.Trim();

            if (txtPostingDateFrom.Text.Trim() != "")
                _gPostingDateFrom = txtPostingDateFrom.Text.Trim();

            if (txtPostingDateTo.Text.Trim() != "")
                _gPostingDateTo = txtPostingDateTo.Text.Trim();

            bindMainDataGrid(Convert.ToString(Session["COMPANY"]), _sMaterialDocNo, _sSehemeNO, _sMeterNoFrom, _sMeterNoTo, _gPostingDateFrom, _gPostingDateTo,
                rdbtnList.SelectedItem.Value.Trim(), _sVendorid);  //Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor         
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    private void bindMainDataGrid(string _sCompany, string _sMaterialDocNo, string _sSchemeNo, string _sMeterNoFrom, string _sMeterNoTo,
                                                            string _gPostingDateFrom, string _gPostingDateTo, string _sSelect, string _sVendor)
    {
        lblSelectedCase.Text = "0";
        lblTotalCase.Text = "0";
        Div1.Visible = true;
        divVendorList.Visible = false;

        DataTable _gdtDetails = objBL.Get_LooseMeter_Details(_sCompany, _sMaterialDocNo, _sSchemeNo, _sMeterNoFrom, _sMeterNoTo, _gPostingDateFrom,
                                                            _gPostingDateTo, _sSelect, _sVendor);

        if (_gdtDetails.Rows.Count > 0)
        {
            lblTotalCase.Text = _gdtDetails.Rows.Count.ToString();
            gvMainData.DataSource = _gdtDetails;
            gvMainData.DataBind();

            if (rdbtnList.SelectedValue == "1") //pending For Allocation
            {
                gvMainData.Columns[0].Visible = true;
                gvMainData.Columns[12].Visible = false;
                gvMainData.Columns[13].Visible = false;

                tr8.Visible = false;
            }
            else if (rdbtnList.SelectedValue == "2")    //Alloted Cases
            {
                gvMainData.Columns[0].Visible = true;
                gvMainData.Columns[12].Visible = true;
                gvMainData.Columns[13].Visible = true;

                tr8.Visible = true;
            }

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
            btnExcel.Visible = false;
            gvMainData.DataSource = null;
            gvMainData.DataBind();

            //SimpleMethods.show("No Record Found.");
        }
    }

    private void DetailsData_Format()
    {
        for (int i = 0; i < gvMainData.Rows.Count; i++)
        {
            gvMainData.Rows[i].Cells[2].Text = gvMainData.Rows[i].Cells[2].Text.TrimStart('0');
            gvMainData.Rows[i].Cells[4].Text = gvMainData.Rows[i].Cells[4].Text.TrimStart('0');
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
            string filename = "Meter_Allocation" + DateTime.Now.ToString("yyyyMMdd") + ".xls";

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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmLooseMeterPunching.aspx");
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
    }

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
            divVendorList.Visible = false;  //09032018
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

    public string _sortDirection { get; set; }

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
    protected void gvMainData_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridViewRow row = gvMainData.SelectedRow;
        //lblOrderID.Text = ((Label)row.Cells[1].FindControl("lblOrderID")).Text;
        //lblDivision.Text = row.Cells[2].Text.ToString();
        //lblCANo.Text = row.Cells[3].Text.ToString();
        //lblMeterNo.Text = ((Label)row.Cells[4].FindControl("lblMeterNO")).Text;
        //lblAllotedTo.Text = row.Cells[12].Text.ToString();
        //mp1.Show();
    }

    #region Check Box Selection
    protected void sellectAll(object sender, EventArgs e)
    {
        int Count = 0;
        CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");

        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");

            if (ChkBoxHeader.Checked == true)
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

            //TabDropData.Visible = true;
            divVendorList.Visible = true;   //09032018

            if (Count == 0)
            {
                //TabDropData.Visible = false;
                divVendorList.Visible = false;
            }

        }
    }

    protected void sellectOne(object sender, EventArgs e)
    {
        int Count = 0;
        CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
            if (ChkBoxRows.Checked == true)
            {
                Count = Count + 1;
                lblSelectedCase.Text = Count.ToString();
                if (Count == Convert.ToInt32(lblTotalCase.Text.ToString()))
                    ChkBoxHeader.Checked = true;
            }
            else
                ChkBoxHeader.Checked = false;

            lblSelectedCase.Text = Count.ToString();

            if (rdbtnList.SelectedValue == "1")
            {
                if (Count == 0)
                {
                    divVendorList.Visible = false;
                    TabDropData.Visible = false;
                }
                else
                {
                    divVendorList.Visible = true;
                    TabDropData.Visible = true;
                }
            }
        }
    }
    #endregion


    protected void rdbtnList_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvMainData.DataSource = null;
        gvMainData.DataBind();
        Div1.Visible = false;
        BindVendorDropDown();
        lblSelectedCase.Text = "0";
        lblTotalCase.Text = "0";

        if (rdbtnList.SelectedValue == "1") //pending For Allocation
        {
            tr2.Visible = false;
            tr8.Visible = false;
        }
        else if (rdbtnList.SelectedValue == "2")    //Alloted Cases
        {
            tr2.Visible = false;
            divVendorList.Visible = false;  //09032018
            TabDropData.Visible = false;
            tr8.Visible = false;
        }
    }

    protected void rdbtnSelect_CheckedChanged(object sender, EventArgs e)
    {
        //foreach (GridViewRow row in gvVendorDetails.Rows)
        //{
        //    RadioButton rdb = (RadioButton)row.FindControl("rdbtnSelect");
        //    if (rdb.Checked == true)            
        //        ((Button)row.FindControl("btnSubmit")).Visible = true;            
        //    else
        //        ((Button)row.FindControl("btnSubmit")).Visible = false;            
        //}
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int count = 0;
        string _gddlEmpName = string.Empty, _sVendorID = string.Empty;

        try
        {
            _sVendorID = Convert.ToString(Session["VENDOR_ID"]).Trim();
            _gddlEmpName = getIntallerID();
            if (_gddlEmpName != "")
            {
                CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
                foreach (GridViewRow row in gvMainData.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                    if (ChkBoxRows.Checked == true)
                    {
                        if (objBL.Assign_LossMeter_OrdInstaller_InputData(_gddlEmpName, _sVendorID, "0000000000" + row.Cells[4].Text, row.Cells[1].Text) == 1)
                        {
                            objBL.MapData_OrderInst_LosseMeter_InputData("0000000000" + row.Cells[4].Text, row.Cells[1].Text, _sVendorID, _gddlEmpName);
                            count = count + 1;
                        }
                    }
                }

                if (count == 0)
                    SimpleMethods.show("Kindly Select Order.");
                else
                {
                    pCount = pCount + count;
                    SimpleMethods.MsgBoxWithLocation("Total Case Pending with " + _gIntallerName + " is " + pCount + ".", "frmLooseMeterPunching.aspx", this);
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
            SimpleMethods.show("Please Try Again.");
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
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
        return result;
    }

    protected void btnUpdateInstaller_Click(object sender, EventArgs e)
    {
        int Count = 0;
        string _VendorID = Convert.ToString(Session["VENDOR_ID"]).Trim();

        if (ddlUpdateInstaller.SelectedValue != "0")
        {
            CheckBox ChkBoxHeader = (CheckBox)gvMainData.HeaderRow.FindControl("chkb1");
            foreach (GridViewRow row in gvMainData.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                if (ChkBoxRows.Checked == true)
                {
                    string PInstallerName = string.Empty;
                    PInstallerName = row.Cells[12].Text.ToString();
                    Count = Count + 1;

                    objBL.Insert_LooseMtr_RevertProcess(row.Cells[4].Text, row.Cells[1].Text, PInstallerName, ddlUpdateInstaller.SelectedValue, "Installer Update");
                    objBL.Assign_LossMeter_OrdInstaller_InputData(ddlUpdateInstaller.SelectedValue, _VendorID, "0000000000" + row.Cells[4].Text, row.Cells[1].Text);

                    objBL.Update_Revert_LooseMeter_Process(ddlUpdateInstaller.SelectedValue, "0000000000" + row.Cells[4].Text, row.Cells[1].Text);
                }
            }
            if (Count == 0)
            {
                SimpleMethods.show("Kindly Select Loose Meter Details");
                return;
            }
            SimpleMethods.MsgBoxWithLocation("Case is Alloted to " + ddlUpdateInstaller.SelectedItem.Text, "frmLooseMeterPunching.aspx", this);
        }
        else
        {
            ddlUpdateInstaller.BackColor = System.Drawing.Color.Yellow;
            SimpleMethods.show("Kindly Select Installer Name.");
        }
    }


    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        // BindScheme_DivisioinWise(Convert.ToString(Session["VENDOR_ID"]));
        BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
        BindScheme_Vendor();
        if (Convert.ToString(Session["ROLE"]) == "A")
        {
            BindScheme_DivisioinWise(BindScheme_Vendor());
        }
       else if (Convert.ToString(Session["ROLE"]) == "R")
        {
            BindScheme_DivisioinWise(BindScheme_Vendor());
        }
        else
        {
            BindScheme_DivisioinWise(Convert.ToString(Session["VENDOR_ID"]));
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
            ddlDivision.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
        if (_dtBindName.Rows.Count == 1)
        {
            ddlDivision.SelectedIndex = 1;
        }
        if (Session["ROLE"] != null)
        {
            if ((Session["ROLE"].ToString() == "A") || (Session["ROLE"].ToString() == "PV"))
            {
                //ddlDivision.Items.Insert(0, new ListItem("-ALL-", "0"));

                if (_dtBindName.Rows.Count == 2)
                {
                    ddlDivision.SelectedIndex = 2;
                }
            }
        }

        BindScheme_DivisioinWise(Convert.ToString(Session["VENDOR_ID"]));
    }

    //public void BindScheme_DivisioinWise(string _sDivID)
    //{
    //    if (_sDivID == "0")
    //    {
    //        for(int i=1; i<ddlDivision.Items.Count; i++)
    //        {
    //            _sDivID += ddlDivision.Items[i].Value;
    //            _sDivID += ",";
    //        }

    //        if (_sDivID.Length > 1)
    //        {
    //            _sDivID = _sDivID.Substring(0, _sDivID.Length - 1);
    //            _sDivID = _sDivID.Replace(",", "','");
    //        }
    //    }

    //    DataTable _dtBindName = objBL.getScheme_DivWise(_sDivID);
    //    if (_dtBindName.Rows.Count > 0)
    //    {
    //        ddlScehme.DataSource = _dtBindName;
    //        ddlScehme.DataTextField = "WBS_SCHEME_NO";
    //        ddlScehme.DataValueField = "WBS_SCHEME_NO";
    //        ddlScehme.DataBind();
    //    }                
    //}

    public void BindScheme_DivisioinWise(string _sDivID)
    {
        if (_sDivID == "0")
        {
            for (int i = 1; i < ddlDivision.Items.Count; i++)
            {
                _sDivID += ddlDivision.Items[i].Value;
                _sDivID += ",";
            }

            if (_sDivID.Length > 1)
            {
                _sDivID = _sDivID.Substring(0, _sDivID.Length - 1);
                _sDivID = _sDivID.Replace(",", "','");
            }
        }
        DataTable _dtBindName = objBL.getScheme_DivWise(_sDivID);
        if (_dtBindName.Rows.Count > 0)
        {
            ddlScehme.DataSource = _dtBindName;
            ddlScehme.DataTextField = "WBS_SCHEME_NO";
            ddlScehme.DataValueField = "WBS_SCHEME_NO";
            ddlScehme.DataBind();
        }
    }

    public void BindVendor(string Vendor, string Division, string Roleid)//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendor, Division, Roleid);
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

    public string BindScheme_Vendor()
    {
        string strV = string.Empty;
        for (int i = 1; i < ddlVendorName.Items.Count; i++)
        {
            strV += ddlVendorName.Items[i].Value;
            strV += ",";
        }
        if (strV.Length > 1)
        {
            strV = strV.Substring(0, strV.Length - 1);
            strV = strV.Replace(",", "','");
        }
        return strV;
    }
    protected void ddlVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["ROLE"]) == "R")
        {
            if (ddlVendorName.SelectedValue == "0")
            {
                BindScheme_DivisioinWise("");
            }
            else
            {
                BindScheme_DivisioinWise(ddlVendorName.SelectedValue);
            }
        }
        else if (Convert.ToString(Session["ROLE"]) == "A")
        {
            if (ddlVendorName.SelectedValue == "0")
            {
                BindScheme_DivisioinWise("");
            }
            else
            {
                BindScheme_DivisioinWise(ddlVendorName.SelectedValue);
            }
        }
        else if (Convert.ToString(Session["ROLE"]) == "V")
        {
            if (ddlVendorName.SelectedValue == "0")
            {
                BindScheme_DivisioinWise("");
            }
            else
            {
                BindScheme_DivisioinWise(ddlVendorName.SelectedValue);
            }
        }
    }
}