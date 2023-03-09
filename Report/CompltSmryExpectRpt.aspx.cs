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

public partial class Report_CompltSmryExpectRpt : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    Image sortImage = new Image();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                BindDivisioin();
                BindOrderType();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
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
    public void BindOrderType()
    {
        DataTable _dtBindName = objBL.getOrderTypeDetails(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"])); //16032018
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

    /// <summary>
    /// Developed by Sanjeev Ranjan on dt 24/01/2019
    /// </summary>
    /// 
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "", _gddlDivision = "", _gddlVendorName = "", _gActivityType = "";
            string _sOrdType = string.Empty, _sActType = string.Empty;
            DataTable _dtDetails = new DataTable();
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
            if (txtDivision.SelectedValue != "0")
                _gddlDivision = txtDivision.SelectedValue;
            else
                _gddlDivision = Convert.ToString(Session["Divison"]);

            if (ddlVendorName.SelectedValue != "0")
                _gddlVendorName = ddlVendorName.SelectedValue;

            if (ddlOrderType.SelectedItem.Text != "-ALL-")
                _sOrdType = ddlOrderType.SelectedValue;
            else
                _sOrdType = ddlOrderType.SelectedItem.Text;

            if (ddlPMActivity.SelectedItem.Text != "-ALL-")
                _sActType = ddlPMActivity.SelectedValue;
            else
                _sActType = ddlPMActivity.SelectedItem.Text;
           
             _dtDetails = objBL.getCompleteExceptRpt(Session["COMPANY"].ToString(), _gFrom, _gTo, _gddlDivision, _gddlVendorName, _sOrdType, _sActType);
            if (_dtDetails.Rows.Count > 0)
            {
                {
                    btnExcel.Visible = false;
                    gvMainData.DataSource = null;
                    gvMainData.DataBind();
                    ViewState["Main"] = null;
                    string _sfilename = "ExpectionalDetails" + DateTime.Now.ToString() + ".xls";
                    ExportToExcel(_dtDetails, _sfilename);
                    SimpleMethods.show("Data Has Been Successfully Download.");
                }
            }
            else
            {
                btnExcel.Visible = false;
                gvMainData.DataSource = null;
                gvMainData.DataBind();
                ViewState["Main"] = null;

                SimpleMethods.show("No Data Found.");
            }
            if (gvMainData.Rows.Count > 0)
            {
                lblTotCunt.Text = "Total Count : " + gvMainData.Rows.Count.ToString();
                lblTotCunt.Visible = true;
                btnExcel.Visible = true;
            }
            else
            {
                lblTotCunt.Visible = false;
                btnExcel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompltSmryRpt.aspx");
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
            string filename = "Deails" + DateTime.Now.ToString() + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            form.Controls.Add(grddetails);
            this.Controls.Add(form);
            form.RenderControl(hw);
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
            string filename = "Details" + DateTime.Now.ToString() + ".xls";

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
            sortImage.ImageUrl = "view_sort_ascending.png";

        }
        else
        {
            _sortDirection = "ASC";
            sortImage.ImageUrl = "view_sort_descending.png";
        }
    }
    public string _sortDirection { get; set; }
    protected void txtDivision_SelectedIndexChanged(object sender, EventArgs e)//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        BindVendor("", txtDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
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
            string filename = "ExpectionalDetails" + DateTime.Now.ToString() + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            form.Controls.Add(grddetails);
            this.Controls.Add(form);
            form.RenderControl(hw);
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}