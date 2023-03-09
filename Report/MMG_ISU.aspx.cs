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
    public void BindVendor(string Vendorid, string Division, string Roleid)//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor  
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendorid, Division,Roleid);
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

    public void getMaineData()
    {
        string _sOrdType = string.Empty, _sActType = string.Empty;

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

        DataTable _dtDetails = objBL.getMMG_ISU(txtFromDate.Text, txtToDate.Text, Session["Divison"].ToString(), "", Session["COMPANY"].ToString(),
                                                txtActFromDate.Text, txtActToDate.Text, _sOrdType, _sActType, Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["ROLE"]));
        if (_dtDetails.Rows.Count > 0)
        {
            btnExcel.Visible = true;
            _dtDetails = GetTotal_Data(_dtDetails);
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();

            gvMainData.Rows[gvMainData.Rows.Count - 1].Font.Bold = true;

            for (int i = 0; i < gvMainData.Rows.Count - 1; i++)
            {
                gvMainData.Rows[i].Cells[0].Text = (i + 1).ToString();
            }

            if (ViewState["Main"] != null)
            {
                ViewState["Main"] = null;
            }
            ViewState["Main"] = _dtDetails;
        }
        else
        {
            btnExcel.Visible = false;
            gvMainData.DataSource = null;
            gvMainData.DataBind();
            ViewState["Main"] = null;

            gvDetails.DataSource = null;
            gvDetails.DataBind();
            imgBtnExcel.Visible = false;
        }
    }

    protected void gvMainData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "",_gPunchFrom = "", _gPunchTo = "", _gddlDivision = "";
            string _sOrdType = string.Empty, _sActType = string.Empty;

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

            if (txtActFromDate.Text != "" && txtActToDate.Text != "")
            {
                if (SimpleMethods.validateDate(txtActFromDate.Text, txtActToDate.Text) == true)
                {
                    _gPunchFrom = txtActFromDate.Text;
                    _gPunchTo = txtActToDate.Text;
                }
                else
                {
                    txtActFromDate.BackColor = System.Drawing.Color.Yellow;
                    txtActToDate.BackColor = System.Drawing.Color.Yellow;
                    SimpleMethods.show("From Activity Date should not be greater than To Date.");
                    return;
                }
            }

            _gddlDivision = e.CommandArgument.ToString();

            if (e.CommandName == "CASESCOUNT")
                lblReportHead.Text = "Cases Given to Vendor";
            else if (e.CommandName == "METERSCONSUMED")
                lblReportHead.Text = "Cases Executed";
            else if (e.CommandName == "METERPENDING")
                lblReportHead.Text = "Cases Pending";
            else if (e.CommandName == "METERSCANCEL")
                lblReportHead.Text = "Cases Required to Cancel";

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

            DataTable _dtDetails = objBL.getMMG_ISU_Details(_gFrom, _gTo, Session["Divison"].ToString(), _gddlDivision, e.CommandName, Session["COMPANY"].ToString(),
                                                           _gPunchFrom, _gPunchTo, _sOrdType, _sActType, Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["ROLE"]));
            if (_dtDetails.Rows.Count > 0)
            {
                imgBtnExcel.Visible = true;
                gvDetails.DataSource = _dtDetails;
                gvDetails.DataBind();
                if (ViewState["Details"] != null)
                {
                    ViewState["Details"] = null;
                }
                ViewState["Details"] = _dtDetails;

                DetailsData_Format();
            }
            else
            {
                imgBtnExcel.Visible = false;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                ViewState["Details"] = null;

                SimpleMethods.show("No Data Found.");
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "",_gPunchFrom = "", _gPunchTo = "", _gddlDivision = "";
            string _sOrdType = string.Empty, _sActType = string.Empty, _gddlVendorname=string.Empty;

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

            if (txtActFromDate.Text != "" && txtActToDate.Text != "")
            {
                if (SimpleMethods.validateDate(txtActFromDate.Text, txtActToDate.Text) == true)
                {
                    _gPunchFrom = txtActFromDate.Text;
                    _gPunchTo = txtActToDate.Text;
                }
                else
                {
                    txtActFromDate.BackColor = System.Drawing.Color.Yellow;
                    txtActToDate.BackColor = System.Drawing.Color.Yellow;
                    SimpleMethods.show("From Activity Date should not be greater than To Date.");
                    return;
                }
            }

            if (txtDivision.SelectedValue != "0")
                _gddlDivision = txtDivision.SelectedValue;

            if (ddlVendorName.SelectedValue != "0")
                _gddlVendorname = ddlVendorName.SelectedValue;

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

            DataTable _dtDetails = objBL.getMMG_ISU(_gFrom, _gTo, Session["Divison"].ToString(), _gddlDivision, Session["COMPANY"].ToString(),
                                                   _gPunchFrom, _gPunchTo, _sOrdType, _sActType, _gddlVendorname, Convert.ToString(Session["ROLE"]));
            if (_dtDetails.Rows.Count > 0)
            {
                btnExcel.Visible = true;
                _dtDetails = GetTotal_Data(_dtDetails);
                gvMainData.DataSource = _dtDetails;
                gvMainData.DataBind();

                gvMainData.Rows[gvMainData.Rows.Count - 1].Font.Bold = true;

                for (int i = 0; i < gvMainData.Rows.Count - 1; i++)
                {
                    gvMainData.Rows[i].Cells[0].Text = (i + 1).ToString();
                }

                if (ViewState["Main"] != null)
                {
                    ViewState["Main"] = null;
                }
                ViewState["Main"] = _dtDetails;
            }
            else
            {
                btnExcel.Visible = false;
                gvMainData.DataSource = null;
                gvMainData.DataBind();
                ViewState["Main"] = null;

                gvDetails.DataSource = null;
                gvDetails.DataBind();
                imgBtnExcel.Visible = false;

                SimpleMethods.show("No Data Found.");
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    private DataTable GetTotal_Data(DataTable _dtData)
    {
        DataRow dr;
        int _iCaseGinVendor = 0, _iCaseExcuted = 0, _iCasePending = 0, _iCaseReqCancel = 0;
        for (int i = 0; i < _dtData.Rows.Count; i++)
        {
            _iCaseGinVendor = _iCaseGinVendor + Convert.ToInt32(_dtData.Rows[i][4].ToString());
            _iCaseExcuted = _iCaseExcuted + Convert.ToInt32(_dtData.Rows[i][5].ToString());
            _iCasePending = _iCasePending + Convert.ToInt32(_dtData.Rows[i][6].ToString());
            _iCaseReqCancel = _iCaseReqCancel + Convert.ToInt32(_dtData.Rows[i][7].ToString());            
        }

        dr = _dtData.NewRow();

        dr[0] = " ";
        dr[1] = " ";
        dr[2] = "Total";
        dr[3] = " ";
        dr[4] = _iCaseGinVendor.ToString();
        dr[5] = _iCaseExcuted.ToString();
        dr[6] = _iCasePending.ToString();
        dr[7] = _iCaseReqCancel.ToString();        

        _dtData.Rows.Add(dr);

        return _dtData;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MeterReconciliation.aspx");
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
            string filename = "MMG_ISU_Details" + DateTime.Now.ToString() + ".xls";

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
    protected void imgBtnExcel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Details"];

        HtmlForm form = new HtmlForm();
        Response.Clear();
        Response.Buffer = true;
        if (_dtDetails.Rows.Count > 0)
        {
            GridView grddetails = new GridView();
            grddetails.DataSource = _dtDetails;
            grddetails.DataBind();
            string filename = "MMG_ISU_Details" + DateTime.Now.ToString() + ".xls";

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
    protected void gvDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Details"];
        SetSortDirection(SortDireaction);
        if (_dtDetails != null)
        {
            //Sort the data.
            _dtDetails.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            gvDetails.DataSource = _dtDetails;
            gvDetails.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvDetails.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvDetails.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
           // gvDetails.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

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
        for (int i = 0; i < gvDetails.Rows.Count; i++)
        {
            gvDetails.Rows[i].Cells[3].Text = gvDetails.Rows[i].Cells[3].Text.TrimStart('0');
            gvDetails.Rows[i].Cells[4].Text = gvDetails.Rows[i].Cells[4].Text.TrimStart('0');
            gvDetails.Rows[i].Cells[5].Text = gvDetails.Rows[i].Cells[5].Text.TrimStart('0');
        }
    }
    protected void txtDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", txtDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}