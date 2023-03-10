/*
 Developer Name:Babalu Kumar
 Req. No.:REQ2705202020141
 PCN No.:2107210708
 Purpose:Add Cable lenth reconcilation process
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using SimpleTest;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class frmCableAllocation : System.Web.UI.Page
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
                //BindReturnCable(Session["Divison"].ToString());
                // divAttachment.Visible = true;
                divVendorList.Visible = false;
                getDataMCRPunchingForSeriesAllocation();
                bindMainDataGrid(rdbtnList.SelectedValue, "", "", "", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(), "", "");
                getInstallerDetails();
                BindDivisioin();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                tr2.Visible = false;
                tr2.Visible = false;
                btnSave.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSave, "") + ";this.value='Please wait....';this.disabled = true;");
                gvMainData.Columns[17].Visible = false;
                //gvMainData.Columns[18].Visible = false;
                //gvMainData.Columns[19].Visible = false;
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

    private void bindMainDataGrid(string strValue, string Materialno, string Fromdate, string Todate, string MaterialDoc, string Div, string Vendor, string UpdateFrom, string UpdateTo)
    {
        lblSelectedCase.Text = "0";
        lblTotalCase.Text = "0";
        Div1.Visible = true;
        DataTable _gdtDetails = objBL.GetCable_Allocation_Details(strValue, Materialno, Fromdate, Todate, MaterialDoc, Div, Vendor, UpdateFrom, UpdateTo);

        if (_gdtDetails.Rows.Count > 0)
        {
            lblTotalCase.Text = _gdtDetails.Rows.Count.ToString();
            gvMainData.DataSource = _gdtDetails;
            gvMainData.DataBind();

            if (rdbtnList.SelectedValue == "1")
            {
                tr8.Visible = false;
            }
            else if (rdbtnList.SelectedValue == "2")
            {
                //tr8.Visible = true;
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
        }
        else
        {
            btnExcel.Visible = false;
            gvMainData.DataSource = null;
            gvMainData.DataBind();
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
            ddlDivision.SelectedIndex = 1;
        if (Session["ROLE"] != null)
        {
            if ((Session["ROLE"].ToString() == "A") || (Session["ROLE"].ToString() == "PV"))
            {
                // ddlDivision.Items.Insert(0, new ListItem("-ALL-", "0"));

                if (_dtBindName.Rows.Count == 2)
                {
                    ddlDivision.SelectedIndex = 2;
                }
            }
        }
    }

    protected void sellectOne(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gv = (GridViewRow)chk.NamingContainer;
        int rownumber = gv.RowIndex;

        if (chk.Checked)
        {
            int i;
            for (i = 0; i <= gvMainData.Rows.Count - 1; i++)
            {
                if (i != rownumber)
                {
                    CheckBox chkcheckbox = ((CheckBox)(gvMainData.Rows[i].FindControl("chkRow")));
                    chkcheckbox.Checked = false;
                }
            }
            if (rdbtnList.SelectedValue == "1")
            {
                divVendorList.Visible = true;
                TabDropData.Visible = true;
            }
            else if (rdbtnList.SelectedValue == "2")
            {
                divVendorList.Visible = false;
                TabDropData.Visible = false;
                tr8.Visible = true;
            }
        }
        else
        {
            divVendorList.Visible = false;
            TabDropData.Visible = false;
        }
    }

    public void getDataMCRPunchingForSeriesAllocation()
    {
        try
        {
            DataTable _gdtEmpDetail = objBL.getEmpDetailsNew1(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));
            if (_gdtEmpDetail.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _gdtEmpDetail;
                gvSeriesWiseAllocation.DataBind();
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void rdbdiscontinued_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            RadioButton rd = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rd.NamingContainer;
            txtName.Text = (row.FindControl("lblEMPNAME") as Label).Text;
            txtEmployeeID.Text = (row.FindControl("lblInstallerID") as Label).Text;
            DataTable _gdtDetails = objBL.getLoginDetails(Session["UserName"].ToString(), "");
            if (_gdtDetails.Rows.Count > 0)
            {
                // hdfDivision.Value = _gdtDetails.Rows[0]["DIVISION"].ToString();
            }

            tr1.Visible = true;
            tr2.Visible = true;
            tr3.Visible = true;
            tr7.Visible = true;
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int Cablelength = 0;
        int strTotallength = 0;
        int count = 0;
        string _gddlEmpName = string.Empty, _sVendorID = string.Empty;
        try
        {
            if (txtCablelength.Text == "" && txtCablelength.Text == null)
            {
                SimpleMethods.show("Please Enter Cable Length");
                return;
            }
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
                        Label txtbox = (Label)row.FindControl("txtcable");
                        Label materialdoc = (Label)row.FindControl("lblmaterialdoc");
                        Label division = (Label)row.FindControl("lblDivcode");
                        Label materialno = (Label)row.FindControl("lblmaterialno");
                        Label vendorid = (Label)row.FindControl("lblVendorId");
                        Label drumno = (Label)row.FindControl("lbldrumno");
                        Label cablesize = (Label)row.FindControl("lblcablesize");
                        Label MRSno = (Label)row.FindControl("lblMRSNo");
                        Label SerialNofrom = (Label)row.FindControl("lblsrfrom");
                        Label SerialnoTo = (Label)row.FindControl("lblSrto");
                        Label lblMake = (Label)row.FindControl("lblMake");
                        int cablelength = Convert.ToInt32(txtbox.Text.ToString());
                        if (cablelength >= Convert.ToInt32(txtCablelength.Text.ToString()))
                        {
                            strTotallength = cablelength - Convert.ToInt32(txtCablelength.Text.ToString());
                        }
                        else
                        {
                            SimpleMethods.show("Allocated cable length should be less than actual cable length");
                            return;
                        }
                        if (ChkBoxRows.Checked == true)
                        {
                            DataTable _dt = objBL.GetRemaining_Cable(materialdoc.Text, materialno.Text);
                            if (_dt.Rows.Count > 0)
                            {
                                Cablelength = (Convert.ToInt32(_dt.Rows[0]["ALLOCATED_CABLE_LENGTH"]) + Convert.ToInt32(txtCablelength.Text.ToString()));
                            }
                            else
                            {
                                Cablelength = Convert.ToInt32(txtCablelength.Text.ToString());
                            }
                            objBL.Assign_CableToInstaller(materialdoc.Text, materialno.Text, _sVendorID, _gddlEmpName, Convert.ToString(strTotallength), Cablelength.ToString());
                            count = count + 1;
                            DataTable _dt1 = objBL.GetCable_Length(materialdoc.Text, materialno.Text, _gddlEmpName);
                            if (_dt1.Rows.Count > 0)
                            {
                                int tsrcablelength = (Convert.ToInt32(_dt1.Rows[0]["CABLE_LENGTH"]) + Convert.ToInt32(txtCablelength.Text.ToString()));
                                objBL.UpdateCablelength(materialdoc.Text, materialno.Text, _gddlEmpName, tsrcablelength.ToString());
                            }
                            else
                            {
                                objBL.Assign_CableAllocateToInstaller(materialdoc.Text, materialno.Text, division.Text, vendorid.Text, MRSno.Text, drumno.Text, cablesize.Text, txtCablelength.Text.ToString(), _gddlEmpName, Session["UserName"].ToString(), SerialNofrom.Text, SerialnoTo.Text, lblMake.Text);
                            }
                        }
                    }
                }

                if (count == 0)
                    SimpleMethods.show("Kindly Select Order.");
                else
                {
                    pCount = pCount + count;
                    SimpleMethods.MsgBoxWithLocation("Total Cable Length Allocated To " + _gIntallerName + " is " + txtCablelength.Text.ToString() + ".", "frmCableAllocation.aspx", this);
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
            foreach (GridViewRow row in gvSeriesWiseAllocation.Rows)
            {
                RadioButton rb = (RadioButton)row.FindControl("rdbtnseriesSelect");
                if (rb.Checked)
                {
                    //  Label _gMeterAlloted = (Label)row.FindControl("lblMeterAlloted");
                    Label _gEMPNAME = (Label)row.FindControl("lblEMPNAME");
                    _gIntallerName = Convert.ToString(_gEMPNAME.Text);
                    // pCount = Convert.ToInt32(_gMeterAlloted.Text);
                    Label rdbtnSelect = (Label)gvSeriesWiseAllocation.Rows[row.RowIndex].FindControl("lblInstallerID");
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

    protected void rdbtnList_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvMainData.DataSource = null;
        gvMainData.DataBind();
        Div1.Visible = false;
        lblSelectedCase.Text = "0";
        lblTotalCase.Text = "0";
        divVendorList.Visible = true;
        if (rdbtnList.SelectedValue == "1") //pending For Allocation
        {
            tr2.Visible = false;
            tr8.Visible = false;
            tr7.Visible = false;
            divVendorList.Visible = false;
            gvMainData.Columns[17].Visible = false;
            //gvMainData.Columns[18].Visible = false;
            //gvMainData.Columns[19].Visible = false;
        }
        else if (rdbtnList.SelectedValue == "2")
        {
            tr2.Visible = false;
            divVendorList.Visible = false;
            TabDropData.Visible = false;
            tr8.Visible = false;
            tr7.Visible = false;
            gvMainData.Columns[17].Visible = true;
            // gvMainData.Columns[18].Visible = true;
            //gvMainData.Columns[19].Visible = true;
            //divAttachment.Visible = false;
        }
        //else if (rdbtnList.SelectedValue == "3")    //Return Cable Length
        //{
        //    bindMainDataGrid(rdbtnList.SelectedValue, "", "", "", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(),"","");
        //    tr2.Visible = false;
        //    tr8.Visible = false;
        //    tr7.Visible = false;
        //    divVendorList.Visible = false;
        //    //BindReturnCable(Session["Divison"].ToString());
        //    divAttachment.Visible = true;
        //}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string _gPostingDateFrom = "", _gPostingDateTo = "";
        string _sMeterNoFrom = "", _sMaterialDocNo = "";

        if ((txtMeterNo.Text.Trim() != "" && txtMeterNo.Text.Trim() == ""))
        {
            SimpleMethods.show("Please Enter Issues Date From and To");
            return;
        }

        if ((txtPostingDateFrom.Text.Trim() != "" && txtPostingDateTo.Text.Trim() == "") || (txtPostingDateFrom.Text.Trim() == "" && txtPostingDateTo.Text.Trim() != ""))
        {
            SimpleMethods.show("Please Enter Issues Date From and To");
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

        if ((txtupdatefrom.Text.Trim() != "" && txtupdateto.Text.Trim() == "") || (txtupdatefrom.Text.Trim() == "" && txtupdateto.Text.Trim() != ""))
        {
            SimpleMethods.show("Please Enter Updated Date From and To");
            return;
        }

        if (txtupdatefrom.Text.Trim() != "" && txtupdateto.Text.Trim() != "")
        {
            DateTime postingDateFrom = Convert.ToDateTime(txtupdatefrom.Text.Trim());
            DateTime postingDateTo = Convert.ToDateTime(txtupdateto.Text.Trim());

            int duration = postingDateTo.CompareTo(postingDateFrom);
            if (duration < 0)
            {
                SimpleMethods.show("To Date Must Be Greater Than Or Equal To From Date.");
                return;
            }
        }
        try
        {
            if (txtMaterialDocNo.Text != "")
                _sMaterialDocNo = txtMaterialDocNo.Text.Trim();

            if (txtMeterNo.Text != "")
                _sMeterNoFrom = txtMeterNo.Text.Trim();

            if (txtPostingDateFrom.Text.Trim() != "")
                _gPostingDateFrom = txtPostingDateFrom.Text.Trim();

            if (txtPostingDateTo.Text.Trim() != "")
                _gPostingDateTo = txtPostingDateTo.Text.Trim();
            bindMainDataGrid(rdbtnList.SelectedValue, txtMeterNo.Text.ToString(), txtPostingDateFrom.Text.ToString(), txtPostingDateTo.Text.ToString(), txtMaterialDocNo.Text.ToString(), Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(), txtupdatefrom.Text.ToString(), txtupdateto.Text.ToString());
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    public void getInstallerDetails()
    {
        try
        {
            string _sVendorID = string.Empty;

            if (Convert.ToString(Session["ROLE"]) != "PV")
                _sVendorID = Convert.ToString(Session["VENDOR_ID"]).Trim();
            DataTable _gdtEmpDetail = objBL.getEmpDetailsNew1(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));
            // DataTable _gdtEmpDetail = objBL.getEmpDetails_LosseMeter(_sVendorID, Convert.ToString(Session["COMPANY"]), Convert.ToString(Session["Divison"]));//Added By Babalu Kumar 29092020 Division Check
            if (_gdtEmpDetail.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _gdtEmpDetail;
                gvSeriesWiseAllocation.DataBind();
                ddlUpdateInstaller.DataSource = _gdtEmpDetail;
                ddlUpdateInstaller.DataTextField = "EMPLOYEE_NAME";
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
                    Label txtbox = (Label)row.FindControl("txtcable");
                    Label materialdoc = (Label)row.FindControl("lblmaterialdoc");
                    Label division = (Label)row.FindControl("lblDivcode");
                    Label materialno = (Label)row.FindControl("lblmaterialno");
                    Label vendorid = (Label)row.FindControl("lblVendorId");
                    Label drumno = (Label)row.FindControl("lbldrumno");
                    Label cablesize = (Label)row.FindControl("lblcablesize");
                    Label MRSno = (Label)row.FindControl("lblMRSNo");
                    Label SerialNofrom = (Label)row.FindControl("lblsrfrom");
                    Label SerialnoTo = (Label)row.FindControl("lblSrto");

                    objBL.Re_Assign_CableToInstaller(materialdoc.Text, materialno.Text, _VendorID, ddlUpdateInstaller.SelectedValue);
                    Count = Count + 1;
                    DataTable _dt1 = objBL.GetRe_Cable_Length(materialdoc.Text, materialno.Text, ddlUpdateInstaller.SelectedValue);
                    if (_dt1.Rows.Count > 0)
                    {
                        //int tsrcablelength = (Convert.ToInt32(_dt1.Rows[0]["CABLE_LENGTH"]) + Convert.ToInt32(txtbox.Text.ToString()));
                        objBL.Update_ReassignCablelength(materialdoc.Text, materialno.Text, ddlUpdateInstaller.SelectedValue, vendorid.Text);
                    }
                    //else
                    //{
                    //    objBL.Assign_CableAllocateToInstaller(materialdoc.Text, materialno.Text, division.Text, vendorid.Text, vendorid.Text, drumno.Text, cablesize.Text, txtbox.Text.ToString(), ddlUpdateInstaller.SelectedValue, Session["UserName"].ToString(), SerialNofrom.Text, SerialnoTo.Text);
                    //}
                }
            }
            if (Count == 0)
            {
                SimpleMethods.show("Kindly Select Loose Meter Details");
                return;
            }
            SimpleMethods.MsgBoxWithLocation("Case is Alloted to " + ddlUpdateInstaller.SelectedItem.Text, "frmCableAllocation.aspx", this);
        }
        else
        {
            ddlUpdateInstaller.BackColor = System.Drawing.Color.Yellow;
            SimpleMethods.show("Kindly Select Installer Name.");
        }
    }

    public string _sortDirection { get; set; }

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
        }
        else
        {
            _sortDirection = "ASC";
        }
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmCableAllocation.aspx");
    }

    protected void btnExcel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["DataTable"];
        //ViewState["DataTable"] = _gdtDetails;

        HtmlForm form = new HtmlForm();
        Response.Clear();
        Response.Buffer = true;
        if (_dtDetails.Rows.Count > 0)
        {
            GridView grddetails = new GridView();
            grddetails.DataSource = _dtDetails;
            grddetails.DataBind();
            string filename = "CableDetails" + DateTime.Now.ToString() + ".xls";

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

    public void BindVendor(string Vendorid, string Division, string Roleid)//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendorid, Division, Roleid);
        if (_dtEmpName.Rows.Count > 0)
        {
            ddlVendor.DataSource = _dtEmpName;
            ddlVendor.DataTextField = "VENDOR_NAME";
            ddlVendor.DataValueField = "VENDOR_ID";
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
            ddlVendor.SelectedIndex = 1;
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        divAttachment.Visible = false;
    }
    protected void btnDocClose_Click(object sender, ImageClickEventArgs e)
    {
        divAttachment.Visible = false;
    }

    private void BindReturnCable(string MateriaDoc, string MaterialNo)
    {

        DataTable _gdt = objBL.GetCable_Return(MateriaDoc, MaterialNo);
        if (_gdt.Rows.Count > 0)
        {
            lblTotalCase.Text = _gdt.Rows.Count.ToString();
            GridView1.DataSource = _gdt;
            GridView1.DataBind();
            divAttachment.Visible = true;
        }
        else
        {
            divAttachment.Visible = false;
            btnExcel.Visible = false;
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
    } //Return Cable Length
    protected void sellectrow(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gv = (GridViewRow)chk.NamingContainer;
        int rownumber = gv.RowIndex;

        if (chk.Checked)
        {
            int i;
            for (i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                if (i != rownumber)
                {
                    CheckBox chkcheckbox = ((CheckBox)(GridView1.Rows[i].FindControl("chkRow1")));
                    chkcheckbox.Checked = false;
                }
                if (chk.Checked)
                {
                    btnaccept.Visible = true;
                    //btnreject.Visible = true;
                }
            }
        }
        else
        {
            btnaccept.Visible = false;
            //btnreject.Visible = false;
        }
    } //Return Cable Length
    protected void btnaccept_Click(object sender, EventArgs e)
    {
        int result = 0;
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow1");
            if (ChkBoxRows.Checked == true)
            {
                Label lblallocate = (Label)row.FindControl("lblallocate");
                Label lblreturncable = (Label)row.FindControl("lblreturncable");
                Label lblmateraildoc = (Label)row.FindControl("lblmateraildoc");
                Label lblmaterialno = (Label)row.FindControl("lblmaterialno");
                if (!String.IsNullOrEmpty(lblreturncable.Text))
                {
                    string[] straraycable = lblreturncable.Text.Split('.');
                    DataTable _dtnew = objBL.GetCable_ReturnLength(lblmateraildoc.Text, lblmaterialno.Text, lblallocate.Text);
                    if (_dtnew.Rows.Count > 0)
                    {
                        int tsrcablelength = (Convert.ToInt32(_dtnew.Rows[0]["MENGE_D_QUANTITY"]) + Convert.ToInt32(straraycable[0].ToString()));
                        int Totalallocatedlength = (Convert.ToInt32(_dtnew.Rows[0]["ALLOCATED_CABLE_LENGTH"]) - Convert.ToInt32(straraycable[0].ToString()));
                        result = objBL.UpdateReturnCablelengthMain(lblmateraildoc.Text, lblmaterialno.Text, tsrcablelength.ToString(), Totalallocatedlength.ToString());
                        objBL.UpdateReturnCablelengthNew(lblmateraildoc.Text, lblmaterialno.Text, tsrcablelength.ToString(), Totalallocatedlength.ToString(), lblallocate.Text);
                        if (result > 0)
                        {
                            result = objBL.UpdateReturnCablelength(lblmateraildoc.Text, lblmaterialno.Text, straraycable[0].ToString());
                            bindMainDataGrid(rdbtnList.SelectedValue, "", "", "", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(), "", "");
                            BindReturnCable(lblmateraildoc.Text, lblmaterialno.Text);
                            SimpleMethods.show("Return Remaining Cable Accepted By Vendor.");
                            return;
                        }
                    }
                }
                else
                {
                    SimpleMethods.show("Return cable not available on select record please try another.");
                    return;
                }
            }
        }
    } //Return Cable Length
    //protected void btnreject_Click(object sender, EventArgs e)
    //{
    //    int result = 0;
    //    foreach (GridViewRow row in GridView1.Rows)
    //    {

    //        CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow1");
    //        if (ChkBoxRows.Checked == true)
    //        {
    //            Label lblallocate = (Label)row.FindControl("lblallocate");
    //            Label lbldrumno = (Label)row.FindControl("lbldrumno");
    //            Label lblcablesize = (Label)row.FindControl("lblcablesize");
    //            Label lblreturncable = (Label)row.FindControl("lblreturncable");
    //            Label lblmateraildoc = (Label)row.FindControl("lblmateraildoc");
    //            Label lblmaterialno = (Label)row.FindControl("lblmaterialno");
    //            string[] straraycable = lblreturncable.Text.Split('.');
    //            // DataTable _dtnew = objBL.GetCable_ReturnLength(lblmateraildoc.Text, lblmaterialno.Text, lblallocate.Text);
    //            //if (_dtnew.Rows.Count > 0)
    //            //  {
    //            // int tsrcablelength = (Convert.ToInt32(_dtnew.Rows[0]["RETURN_UNUSED_CABLE"]) + Convert.ToInt32(lblreturncable.Text.ToString()));
    //            //result = objBL.UpdateRejectedCablelength(lblmateraildoc.Text, lblmaterialno.Text, tsrcablelength.ToString());
    //            result = objBL.UpdateReturnCablelength_Reject(lblmateraildoc.Text, lblmaterialno.Text, straraycable[0].ToString());
    //            if (result > 0)
    //            {
    //                objBL.UpdateRejectedCablelength(lblmateraildoc.Text, lblmaterialno.Text);
    //                bindMainDataGrid(rdbtnList.SelectedValue, "", "", "", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(),"","");
    //                //BindReturnCable(Session["Divison"].ToString());
    //                SimpleMethods.show("Return Remaining Cable Rejected By Vendor.");
    //                return;
    //            }
    //            // }
    //        }
    //    }
    //} //Return Cable Length

    //protected void lknbutton_Click(object sender, EventArgs e)
    //{
    //    int result = 0;
    //    var strdata = (Control)sender;
    //    GridViewRow row = (GridViewRow)strdata.NamingContainer;
    //    Label lblmaterialdoc = (Label)row.FindControl("lblmaterialdoc");
    //    Label lblmaterialno = (Label)row.FindControl("lblmaterialno");
    //    Label txtunusedcable = (Label)row.FindControl("txtunusedcable");
    //    Label txtallocatedto = (Label)row.FindControl("txtallocatedto");
    //    if (!String.IsNullOrEmpty(txtunusedcable.Text))
    //    {
    //        string[] straraycable = txtunusedcable.Text.Split('.');
    //        DataTable _dtnew = objBL.GetCable_ReturnLength(lblmaterialdoc.Text, lblmaterialno.Text, txtallocatedto.Text);
    //        if (_dtnew.Rows.Count > 0)
    //        {
    //            int tsrcablelength = (Convert.ToInt32(_dtnew.Rows[0]["MENGE_D_QUANTITY"]) + Convert.ToInt32(straraycable[0].ToString()));
    //            int Totalallocatedlength = (Convert.ToInt32(_dtnew.Rows[0]["ALLOCATED_CABLE_LENGTH"]) - Convert.ToInt32(straraycable[0].ToString()));
    //            result = objBL.UpdateReturnCablelengthMain(lblmaterialdoc.Text, lblmaterialno.Text, tsrcablelength.ToString(), Totalallocatedlength.ToString());
    //            if (result > 0)
    //            {
    //                result = objBL.UpdateReturnCablelength(lblmaterialdoc.Text, lblmaterialno.Text, straraycable[0].ToString());
    //                bindMainDataGrid(rdbtnList.SelectedValue, "", "", "", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(), "", "");
    //                SimpleMethods.show("Return Remaining Cable Accepted By Vendor.");
    //                return;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        SimpleMethods.show("Return cable not available on select record please try another.");
    //        return;
    //    }
    //}

    protected void lknbutton_Click(object sender, EventArgs e)
    {
        int result = 0;
        var strdata = (Control)sender;
        GridViewRow row = (GridViewRow)strdata.NamingContainer;
        Label lblmaterialdoc = (Label)row.FindControl("lblmaterialdoc");
        Label lblmaterialno = (Label)row.FindControl("lblmaterialno");
        Label txtunusedcable = (Label)row.FindControl("txtunusedcable");
        Label txtallocatedto = (Label)row.FindControl("txtallocatedto");
        //if (!String.IsNullOrEmpty(txtunusedcable.Text))
        //{
        BindReturnCable(lblmaterialdoc.Text, lblmaterialno.Text);

        //string[] straraycable = txtunusedcable.Text.Split('.');
        //DataTable _dtnew = objBL.GetCable_ReturnLength(lblmaterialdoc.Text, lblmaterialno.Text, txtallocatedto.Text);
        //if (_dtnew.Rows.Count > 0)
        //{
        //    int tsrcablelength = (Convert.ToInt32(_dtnew.Rows[0]["MENGE_D_QUANTITY"]) + Convert.ToInt32(straraycable[0].ToString()));
        //    int Totalallocatedlength = (Convert.ToInt32(_dtnew.Rows[0]["ALLOCATED_CABLE_LENGTH"]) - Convert.ToInt32(straraycable[0].ToString()));
        //  result = objBL.UpdateReturnCablelengthMain(lblmaterialdoc.Text, lblmaterialno.Text, tsrcablelength.ToString(), Totalallocatedlength.ToString());
        //    if (result > 0)
        //    {
        //      result = objBL.UpdateReturnCablelength(lblmaterialdoc.Text, lblmaterialno.Text, straraycable[0].ToString());
        //        bindMainDataGrid(rdbtnList.SelectedValue, "", "", "", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(), "", "");
        //        SimpleMethods.show("Return Remaining Cable Accepted By Vendor.");
        //        return;
        //    }
        //}
        // }
        //  else
        // {
        //   SimpleMethods.show("Return cable not available on select record please try another.");
        //    return;
        // }
    }
}