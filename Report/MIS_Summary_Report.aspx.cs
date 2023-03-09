using SimpleTest;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class MMG_New_Summary : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session.Keys.Count != 0)
            {
                BindDivisioin();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                BindOrderType(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));
            }
            else
            {
                Response.Redirect("/Default.aspx");
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
            ddlDivision.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
        if (_dtBindName.Rows.Count == 1)
            ddlDivision.SelectedIndex = 1;
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
            ddlVendorName.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
            ddlVendorName.SelectedIndex = 1;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "", _gddlDivision = "";
            string _sOrdType = string.Empty, _sActType = string.Empty, _gddlVendorName=string.Empty;
            if (txtStartDate.Text != "" && txtEndDate.Text != "")
            {
                if (SimpleMethods.validateDate(txtStartDate.Text, txtEndDate.Text) == true)
                {
                    _gFrom = txtStartDate.Text;
                    _gTo = txtEndDate.Text;
                }
                else
                {
                    txtStartDate.BackColor = System.Drawing.Color.Yellow;
                    txtEndDate.BackColor = System.Drawing.Color.Yellow;
                    SimpleMethods.show("From Date should not be greater than To Date.");
                    return;
                }
            }
            if (ddlDivision.SelectedValue != "0")
                _gddlDivision = ddlDivision.SelectedValue;

            if (ddlVendorName.SelectedValue != "0")//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                _gddlVendorName = ddlVendorName.SelectedValue;

            if (Convert.ToString(Session["ROLE"]) == "PV")
            {
                if (ddlOrderType.SelectedValue != "0")
                    _sOrdType = ddlOrderType.SelectedValue;

            }
            if (ddlOrderType.SelectedValue != "0")
                _sOrdType = ddlOrderType.SelectedValue;
            DataTable _dtDetails = objBL.GetMISummaryReport(_gddlDivision, _sOrdType, _gFrom, _gTo, _gddlVendorName);
            if (_dtDetails.Rows.Count > 0)
            {
                totals.Visible = true;
                btnExcel.Visible = true;
                grdMcrSummary.DataSource = _dtDetails;
                grdMcrSummary.DataBind();

                if (_dtDetails.Rows.Count > 1)
                {
                    grdMcrSummary.Rows[grdMcrSummary.Rows.Count - 1].Font.Bold = true;
                    grdMcrSummary.FooterRow.Cells[0].Text = "Total";
                    grdMcrSummary.FooterRow.Cells[0].Font.Bold = true;
                    grdMcrSummary.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdMcrSummary.Rows[grdMcrSummary.Rows.Count - 1].Font.Bold = true;
                    grdMcrSummary.FooterRow.Cells[3].Text = lbl_ttl_OrdrCancelled.Text;
                    grdMcrSummary.FooterRow.Cells[3].Font.Bold = true;
                    grdMcrSummary.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                    grdMcrSummary.Rows[grdMcrSummary.Rows.Count - 1].Font.Bold = true;
                    grdMcrSummary.FooterRow.Cells[4].Text = lbl_ttl_OrdrComplete.Text;
                    grdMcrSummary.FooterRow.Cells[4].Font.Bold = true;
                    grdMcrSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                    grdMcrSummary.Rows[grdMcrSummary.Rows.Count - 1].Font.Bold = true;
                    grdMcrSummary.FooterRow.Cells[5].Text = lbl_ttl_LooseComplete.Text;
                    grdMcrSummary.FooterRow.Cells[5].Font.Bold = true;
                    grdMcrSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
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
                grdMcrSummary.DataSource = null;
                grdMcrSummary.DataBind();
                ViewState["Main"] = null;
                totals.Visible = false;
                grdMcrSummary.DataSource = null;
                grdMcrSummary.DataBind();
                SimpleMethods.show("No Data Found.");
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('"+ex.Message.ToString()+"')</script>");
            SimpleMethods.show(ex.Message.ToString());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MIS_Summary_Report.aspx");
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
    }
    protected void grdMcrSummary_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Main"];
        SetSortDirection(SortDireaction);
        if (_dtDetails != null)
        {
            _dtDetails.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            grdMcrSummary.DataSource = _dtDetails;
            grdMcrSummary.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in grdMcrSummary.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = grdMcrSummary.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            grdMcrSummary.FooterRow.Cells[0].Text = "Total";
            grdMcrSummary.FooterRow.Cells[1].Font.Bold = true;
            grdMcrSummary.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            DetailsData_Format();
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
    private void DetailsData_Format()
    {
        for (int i = 0; i < grdMcrSummary.Rows.Count; i++)
        {
            grdMcrSummary.Rows[i].Cells[4].Text = grdMcrSummary.Rows[i].Cells[4].Text.TrimStart('0');
        }
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
            grddetails.Rows[grddetails.Rows.Count - 1].Font.Bold = true;
            grddetails.FooterRow.Cells[0].Text = "Total";
            grddetails.FooterRow.Cells[0].Font.Bold = true;
            grddetails.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            grddetails.Rows[grddetails.Rows.Count - 1].Font.Bold = true;
            grddetails.FooterRow.Cells[3].Text = lbl_ttl_OrdrCancelled.Text;
            grddetails.FooterRow.Cells[3].Font.Bold = true;
            grddetails.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;

            grddetails.Rows[grddetails.Rows.Count - 1].Font.Bold = true;
            grddetails.FooterRow.Cells[4].Text = lbl_ttl_OrdrComplete.Text;
            grddetails.FooterRow.Cells[4].Font.Bold = true;
            grddetails.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            string filename = "MIS_Summary_Report" + DateTime.Now.ToString() + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            form.Controls.Add(grdMcrSummary);
            this.Controls.Add(form);
            form.RenderControl(hw);
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    int totalOrdererdCompleted = 0;
    int totalCancelled = 0;
    int totalLooseCompleted = 0;
    protected void grdMcrSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            totalCancelled = totalCancelled + Convert.ToInt32(e.Row.Cells[3].Text);
            lbl_ttl_OrdrCancelled.Text = totalCancelled.ToString();

            totalOrdererdCompleted = totalOrdererdCompleted + Convert.ToInt32(e.Row.Cells[4].Text);
            lbl_ttl_OrdrComplete.Text = totalOrdererdCompleted.ToString();

            totalLooseCompleted = totalLooseCompleted + Convert.ToInt32(e.Row.Cells[5].Text);
            lbl_ttl_LooseComplete.Text = totalLooseCompleted.ToString();
        }
    }
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}