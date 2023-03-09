
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SimpleTest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Report_MISCancelledCases : System.Web.UI.Page
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
           
           // btnSave.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSave, "") + ";this.value='Please wait....';this.disabled = true;");
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
            ddlDivision.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
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
            ddlVendorName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
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
            ddlOrderType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
        }
    }
    protected void grdCanecelledCases_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Main"];
        SetSortDirection(SortDireaction);
        if (_dtDetails != null)
        {
            //Sort the data.
            _dtDetails.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            grdCanecelledCases.DataSource = _dtDetails;
            grdCanecelledCases.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in grdCanecelledCases.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = grdCanecelledCases.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            //gvDetails.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

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
            //sortImage.ImageUrl = "view_sort_ascending.png";

        }
        else
        {
            _sortDirection = "ASC";
            //sortImage.ImageUrl = "view_sort_descending.png";
        }
    }
    private void DetailsData_Format()
    {
        for (int i = 0; i < grdCanecelledCases.Rows.Count; i++)
        {
            grdCanecelledCases.Rows[i].Cells[4].Text = grdCanecelledCases.Rows[i].Cells[4].Text.TrimStart('0');
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MISCancelledCases.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {        
            string _gFrom = "", _gTo = "", _gddlDivision = "";
            string _sOrdType = string.Empty, _sActType = string.Empty, _sVendorid = string.Empty ;

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
                    SimpleMethods.show("End Date should be greater than Start Date.");
                    return;
                }
            }

            if (ddlDivision.SelectedValue != "0")
                _gddlDivision = ddlDivision.SelectedValue;

            if (ddlVendorName.SelectedValue != "0")
                _sVendorid = ddlVendorName.SelectedValue;

            if (ddlOrderType.SelectedItem.Text != "-ALL-")
                _sOrdType = ddlOrderType.SelectedValue;



            DataTable _dtDetails = objBL.getMISCancelCases(_gddlDivision, _sOrdType, _gFrom, _gTo, _sVendorid
                                                               /*, _sActType, Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["ROLE"])*/);
            if (_dtDetails.Rows.Count > 0)
            {
                if (_dtDetails.Rows.Count > 150)
                {
                    
                    grdCanecelledCases.DataSource = null;
                    grdCanecelledCases.DataBind();
                    string _sfilename = "ExpectionalDetails" + DateTime.Now.ToString() + ".xls";
                    ExportToExcel(_dtDetails, _sfilename);
                    SimpleMethods.show("Data Has Been Successfully Download.");
                    
                }
                else
                {
                    btnExcel.Visible = true;
                    hdnTotalLBL.Visible = true;
                    hdnTotalCount.Visible = true;
                    hdnTotalCount.Text = _dtDetails.Rows.Count.ToString();
                    //_dtDetails = GetTotal_Data(_dtDetails);
                    grdCanecelledCases.DataSource = _dtDetails;
                    grdCanecelledCases.DataBind();

                    grdCanecelledCases.Rows[grdCanecelledCases.Rows.Count - 1].Font.Bold = true;
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
                grdCanecelledCases.DataSource = null;
                grdCanecelledCases.DataBind();
                ViewState["Main"] = null;

                grdCanecelledCases.DataSource = null;
                grdCanecelledCases.DataBind();
                // imgBtnExcel.Visible = false;

                SimpleMethods.show("No Data Found.");
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");            
            grdCanecelledCases.DataSource = null;
            grdCanecelledCases.DataBind();
        }
    }
    public void ExportToExcel(DataTable _dtDetails, string _ExportFile)
    {
        HtmlForm form = new HtmlForm();
        Response.Clear();
        Response.Buffer = true;

        if (_dtDetails.Rows.Count > 0)
        {
            GridView grddetails = new GridView();
            grddetails.DataSource = _dtDetails;
            grddetails.DataBind();
            string filename = "MISCancelledCases" + DateTime.Now.ToString() + ".xls";

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
            string filename = "MIS_Cancelled_Cases" + DateTime.Now.ToString() + ".xls";

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


    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}