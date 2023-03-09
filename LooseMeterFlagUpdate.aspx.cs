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

public partial class LooseMeterFlagUpdate : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                BindDivisioin();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 REQ04122020121216                
               // BindVendorDropDown();
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

    public void BindVendorDropDown()
    {
        DataTable _gdtEmpDetail = objBL.getVendorDetails(Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));
        {
            ddlVendorName.DataSource = _gdtEmpDetail;
            ddlVendorName.DataTextField = "VENDOR_NAME";
            ddlVendorName.DataValueField = "VENDOR_ID";
            ddlVendorName.DataBind();
            ddlVendorName.Items.Insert(0, new ListItem("-Select One-", ""));
        }
    }
    public void BindVendor(string Vendorid, string Division, string Roleid)//Added By Babalu Kumar 30122020 REQ04122020121216
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string _gPostingDateFrom = "", _gPostingDateTo = "";
        string _sMeterNoFrom = "", _sMeterNoTo = "", _sMaterialDocNo = "", _sSehemeNO="";

        if ((txtMeterNoFrom.Text.Trim() != "" && txtMeterNoTo.Text.Trim() == "") || (txtMeterNoFrom.Text.Trim() == "" && txtMeterNoTo.Text.Trim() != ""))
        {
            SimpleMethods.show("Please Enter Meter Series From & To");
            return;
        }

        if ((txtPostingDateFrom.Text.Trim() != "" && txtPostingDateTo.Text.Trim() == "") || (txtPostingDateFrom.Text.Trim() == "" && txtPostingDateTo.Text.Trim() != ""))
        {
            SimpleMethods.show("Please Enter Activity Date From & To");
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

            bindMainDataGrid(Convert.ToString(Session["COMPANY"]), _sMaterialDocNo,_sSehemeNO, _sMeterNoFrom, _sMeterNoTo, _gPostingDateFrom, _gPostingDateTo,
                "4", ddlVendorName.SelectedItem.Value.Trim(), Convert.ToString(Session["ROLE"]), Convert.ToString(Session["Divison"]));
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    private void bindMainDataGrid(string _sCompany, string _sMaterialDocNo, string _sSehemeNo, string _sMeterNoFrom, string _sMeterNoTo, string _gPostingDateFrom, string _gPostingDateTo, string _sSelect, string _sVendor, string RoleId, string SessionDiv)
    {
        lblSelectedCase.Text = "0";
        lblTotalCase.Text = "0";
        Div1.Visible = true;

        string _sDivCode = string.Empty;

        if (ddlDivision.SelectedItem.Text != "-ALL-")
            _sDivCode = ddlDivision.SelectedValue;

        DataTable _gdtDetails = objBL.Get_LooseMeter_KittingOrderDetails(_sCompany, _sMaterialDocNo, _sSehemeNo, _sMeterNoFrom, _sMeterNoTo,
                                                                                        _gPostingDateFrom, _gPostingDateTo, _sSelect, _sVendor, _sDivCode, RoleId, SessionDiv);

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
            btnExcel.Visible = false;
            gvMainData.DataSource = null;
            gvMainData.DataBind();
        }
    }    

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("LooseMeterFlagUpdate.aspx");
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
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



            if (Count == 0)
            {

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

            if (Count == 0)
            {
                TabDropData.Visible = false;
            }
            else
            {
                TabDropData.Visible = true;
            }
        }
    }
    #endregion

    private void DetailsData_Format()
    {
        for (int i = 0; i < gvMainData.Rows.Count; i++)
        {
            gvMainData.Rows[i].Cells[1].Text = gvMainData.Rows[i].Cells[1].Text.TrimStart('0');
            gvMainData.Rows[i].Cells[5].Text = gvMainData.Rows[i].Cells[5].Text.TrimStart('0');
            gvMainData.Rows[i].Cells[7].Text = gvMainData.Rows[i].Cells[7].Text.TrimStart('0');
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
        }
        else
        {
            _sortDirection = "ASC";
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

    protected void imgAction_Command(object sender, CommandEventArgs e)
    {
        DataTable _dtData = new DataTable();
        _dtData = objBL.GetLosseMeter_DetailsData_CaseWise(e.CommandArgument.ToString(), e.CommandName.ToString());

        if (_dtData.Rows.Count > 0)
        {
            lblDivision.Text = _dtData.Rows[0]["DIVISION"].ToString();
            lblDeviceNo.Text = _dtData.Rows[0]["DEVICENO"].ToString();
            lblOrderID.Text = _dtData.Rows[0]["ORDERID"].ToString();

            lblINSTALLEDBUSBAR.Text = _dtData.Rows[0]["INSTALLEDBUSBAR"].ToString();
            lblINSTALLATION.Text = _dtData.Rows[0]["INSTALLATION"].ToString();
            lblORDER_TYPE.Text = _dtData.Rows[0]["ORDER_TYPE"].ToString();

            lblELCB_INSTALLED.Text = _dtData.Rows[0]["ELCB_INSTALLED"].ToString();
            lblACTIVITY_DATE.Text = _dtData.Rows[0]["ACTIVITY_DATE"].ToString();
            lblBUS_BAR_NO.Text = _dtData.Rows[0]["BUS_BAR_NO"].ToString();

            lblCustName.Text = _dtData.Rows[0]["Cust_Name"].ToString();
            lblPunchName.Text = _dtData.Rows[0]["PUNCH_NAME"].ToString();
            lblPunchedBy.Text = _dtData.Rows[0]["PUNCH_BY"].ToString();

            divAttachment.Visible = true;
            pagedimmer.Visible = true;
        }
    }

    protected void btnDocClose_Click(object sender, ImageClickEventArgs e)
    {
        divAttachment.Visible = false;
        pagedimmer.Visible = false;
    }

    protected void btnDocCancel_Click(object sender, EventArgs e)
    {
        divAttachment.Visible = false;
        pagedimmer.Visible = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string _sOrderNumber = string.Empty;
        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
            if (ChkBoxRows.Checked == true)
            {
               objBL.UpdateLoose_MCR_Details(row.Cells[2].Text,"0000000000" + row.Cells[1].Text);
               objBL.UpdateLoose_MCR_ImageDetails(row.Cells[2].Text, "0000000000" + row.Cells[1].Text);
               objBL.UpdateLoose_Ketting_Status(row.Cells[2].Text, "0000000000" + row.Cells[1].Text);
            }
        }

        SimpleMethods.show("Selected cases has been successfully Kitting");
    }


    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindScheme_DivisioinWise(Convert.ToString(Session["VENDOR_ID"]));
        BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 REQ04122020121216
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
        }

        ddlDivision.Items.Insert(0, new ListItem("-ALL-", "0"));

        if (_dtBindName.Rows.Count == 2)
        {
            ddlDivision.SelectedIndex = 2;
        }

        BindScheme_DivisioinWise(Convert.ToString(Session["VENDOR_ID"]));
    }

    public void BindScheme_DivisioinWise(string _sDivID)
    {
        if (_sDivID == "0")
        {
            _sDivID = "";
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