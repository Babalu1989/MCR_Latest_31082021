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

public partial class ReallocationCancelOrder : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                txtPostingDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                txtPostingToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                BindDivisioin();
                BindOrderType();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
            }
        }
        else
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("Default.aspx");
        }
        btnDelete.Attributes.Add("onclick", "return  ex();");
        BtnReallocate.Attributes.Add("onclick", "return confirm('Are you sure, you want to Reallocate this Order to Vendor Portal?');");

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
            txtDivision.Items.Insert(0, new ListItem("ALL", "0"));
        }
        if (_dtBindName.Rows.Count == 1)
            txtDivision.SelectedIndex = 1;
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
        }

        if (Count > 0)
        {            
            TABUpdate.Visible = true;
        }
        else
        {
            TABUpdate.Visible = false;           
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
        }

        if (Count > 0)
        {
            TABUpdate.Visible = true;           
        }
        else
        {
            TABUpdate.Visible = false;           
        }
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
            //gvMainData.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

            lblSelectedCase.Text = "0";
            TABUpdate.Visible = false;

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
          //  sortImage.ImageUrl = "view_sort_ascending.png";

        }
        else
        {
            _sortDirection = "ASC";
          //  sortImage.ImageUrl = "view_sort_descending.png";
        }
    }

    public string _sortDirection { get; set; }
    #endregion

    public void getMaineData()
    {
        string _gDivision = "", _gPostingDate = "", _gPostingToDate = "";
        string _sMeterNo = string.Empty, _sOrderNo = string.Empty, _BasicFindate = string.Empty;
        string _sOrdType = string.Empty, _sActType = string.Empty, _sVendorid = string.Empty;

        if (txtMeterNO.Text != "")
            _sMeterNo = txtMeterNO.Text;

        if (txtServiceOrdNo.Text != "")
            _sOrderNo = txtServiceOrdNo.Text;

        if (txtDivision.SelectedValue != "0")
            _gDivision = txtDivision.SelectedValue;

        if (ddlVendorName.SelectedValue != "0")//Added by Babalu Kumar 04012021 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
               _sVendorid =ddlVendorName.SelectedValue;
        
   
        if (txtPostingDate.Text.Trim() != "")
            _gPostingDate = txtPostingDate.Text.Trim();

        if (txtPostingToDate.Text.Trim() != "")
            _gPostingToDate = txtPostingToDate.Text.Trim();

        if (txtBasicFinishDate.Text.Trim() != "")
            _BasicFindate = txtBasicFinishDate.Text.Trim();

        if (ddlOrderType.SelectedItem.Text != "-ALL-")
            _sOrdType = ddlOrderType.SelectedValue;
        else
            _sOrdType = ddlOrderType.SelectedItem.Text;

        if (ddlPMActivity.SelectedItem.Text != "-ALL-")
            _sActType = ddlPMActivity.SelectedValue;
        else
            _sActType = ddlPMActivity.SelectedItem.Text;


        DataTable _gdtDetails = objBL.Get_MCR_InputData_CancelDetails( Convert.ToString(Session["Divison"]),
               Convert.ToString(Session["COMPANY"]), "E", _gDivision, "", _gPostingDate, "", _sMeterNo, _sOrderNo,
               _BasicFindate, _gPostingToDate, _sOrdType, _sActType, _sVendorid);

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

            lblSelectedCase.Text = "0";
            TABUpdate.Visible = false;
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
            gvMainData.Rows[i].Cells[3].Text = gvMainData.Rows[i].Cells[3].Text.TrimStart('0');
            ((Label)gvMainData.Rows[i].Cells[1].FindControl("ORDERID")).Text = ((Label)gvMainData.Rows[i].Cells[1].FindControl("ORDERID")).Text.TrimStart('0');
            ((Label)gvMainData.Rows[i].Cells[1].FindControl("METER_NO")).Text = ((Label)gvMainData.Rows[i].Cells[1].FindControl("METER_NO")).Text.TrimStart('0');

        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {        
        getMaineData();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReallocationCancelOrder.aspx");
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
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
            string filename = "Cancel_Order_"+ DateTime.Now.ToString() + ".xls";

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
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
            Label lblGvOrderID = (Label)row.FindControl("lblOrderID");
            Label lblGvMeterNo = (Label)row.FindControl("lblMeterNO");

            if (ChkBoxRows.Checked == true)
            {
              //  Update_Order_Status("PC", lblGvOrderID.Text, lblGvMeterNo.Text);
                Update_Order_Status("N", lblGvOrderID.Text, lblGvMeterNo.Text);
            }
        }

        getMaineData();
    }
    protected void BtnReallocate_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
            Label lblGvOrderID = (Label)row.FindControl("lblOrderID");
            Label lblGvMeterNo = (Label)row.FindControl("lblMeterNO");

            if (ChkBoxRows.Checked == true)
            {
                Update_Order_Status("N", lblGvOrderID.Text, lblGvMeterNo.Text);
            }
        }

        getMaineData();
    }
    private void Update_Order_Status(string _sFlag,string _sOrderNo, string _sMeterNo)
    {
        //objBL.Update_Order_Status(Session["UserName"].ToString(), txtRemarks.Text.Trim().Replace("'", "''"), _sFlag, _sOrderNo, _sMeterNo);

        if (objBL.Update_Order_Status(Session["UserName"].ToString(), txtRemarks.Text.Trim().Replace("'", "''"), _sFlag, _sOrderNo, _sMeterNo) == 1)
            objBL.Delete_Order_Status(_sOrderNo, _sMeterNo);

        txtRemarks.Text = "";
        TABUpdate.Visible = false;
    }
    public void BindVendor(string Vendorid, string Division, string Roleid)//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
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
            ddlVendorName.SelectedIndex = 1;
    }
    protected void txtDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", txtDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}