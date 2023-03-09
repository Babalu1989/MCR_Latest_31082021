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
                txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                BindDivisioin();
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
            ddlVendorName.SelectedIndex = 1;
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
        //DataTable _dtDetails = objBL.getSealReconciliation(txtFromDate.Text, txtToDate.Text, Session["Divison"].ToString(), "", Session["COMPANY"].ToString());
        DataTable _dtDetails = new DataTable();

        if (RbdSealType.SelectedValue.ToString() == "P")
            _dtDetails = objBL.getSealReconciliation("", txtFromDate.Text, txtToDate.Text, "", txtDivision.SelectedValue, Session["COMPANY"].ToString());
        else
            _dtDetails = objBL.getGunnySealReconciliation("", txtFromDate.Text, txtToDate.Text, "", txtDivision.SelectedValue, Session["COMPANY"].ToString());

        if (_dtDetails.Rows.Count > 0)
        {
            btnExcel.Visible = true;
            _dtDetails = GetTotal_Data(_dtDetails);
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();

            gvMainData.Rows[gvMainData.Rows.Count - 1].Font.Bold = true;

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

            //SimpleMethods.show("No Data Found.");
        }
    }

    protected void gvMainData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "", _gddlDivision = "", _sVendorId = "";
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

            string[] CommandArgument = e.CommandArgument.ToString().Split(',');
            _gddlDivision = CommandArgument[0];
            _sVendorId = CommandArgument[1];

            // _gddlDivision = e.CommandArgument.ToString();

            if (e.CommandName == "CASESCOUNT")
                lblReportHead.Text = "Seal Issued To Vendor";
            else if (e.CommandName == "SEALCONSUMED")
                lblReportHead.Text = "Seal Consumed";
            else if (e.CommandName == "SEALPENDING")
                lblReportHead.Text = "Seal with Installer";
            else if (e.CommandName == "SEALNOTASSIGN")
                lblReportHead.Text = "Seal Pendding";

            DataTable _dtDetails = new DataTable();

            if (RbdSealType.SelectedValue.ToString() == "P")
                _dtDetails = objBL.getSealReconciliationDetails(_gFrom, _gTo, Session["Divison"].ToString(), _gddlDivision, e.CommandName,
                                                                                    Session["COMPANY"].ToString(), _sVendorId);
            else
                _dtDetails = objBL.getSealGunnyReconciliationDetails(_gFrom, _gTo, Session["Divison"].ToString(), _gddlDivision, e.CommandName,
                                                                                    Session["COMPANY"].ToString(), _sVendorId);

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
            string _gFrom = "", _gTo = "", _gddlDivision = "", _sVendorId = string.Empty;

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
            {
                _gddlDivision = txtDivision.SelectedValue;
                _sVendorId = ddlVendorName.SelectedValue;//Added by Babalu Kumar 31122020 PCN REQ04122020121216 
                //_sVendorId = GetVendorIDList_DivisionWise(txtDivision.SelectedValue);//commented by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
            }
            else
            {
                _sVendorId = ddlVendorName.SelectedValue;//Added by Babalu Kumar 31122020 PCN REQ04122020121216 
                //_sVendorId = GetVendorIDList_DivisionWise(Session["Divison"].ToString());//commented by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor  
            }

            DataTable _dtDetails = new DataTable();
            if (RbdSealType.SelectedValue.ToString() == "P")
                _dtDetails = objBL.getSealReconciliation(_sVendorId, _gFrom, _gTo, Session["Divison"].ToString(), _gddlDivision, Session["COMPANY"].ToString());
            else
                _dtDetails = objBL.getGunnySealReconciliation(_sVendorId, _gFrom, _gTo, Session["Divison"].ToString(), _gddlDivision, Session["COMPANY"].ToString());

            if (_dtDetails.Rows.Count > 0)
            {
                btnExcel.Visible = true;

                _dtDetails = GetTotal_Data(_dtDetails);
                gvMainData.DataSource = _dtDetails;
                gvMainData.DataBind();

                gvMainData.Rows[gvMainData.Rows.Count - 1].Font.Bold = true;

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
        int _iSealIssueVendor = 0, _iSealConsumed = 0, _iSealWithInst = 0, _iSealPend = 0;
        for (int i = 0; i < _dtData.Rows.Count; i++)
        {
            _iSealIssueVendor = _iSealIssueVendor + Convert.ToInt32(_dtData.Rows[i][3].ToString());
            _iSealConsumed = _iSealConsumed + Convert.ToInt32(_dtData.Rows[i][4].ToString());
            _iSealWithInst = _iSealWithInst + Convert.ToInt32(_dtData.Rows[i][5].ToString());
            _iSealPend = _iSealPend + Convert.ToInt32(_dtData.Rows[i][6].ToString());
        }

        dr = _dtData.NewRow();

        dr[0] = " ";
        dr[1] = " ";
        dr[2] = "Total";
        dr[3] = _iSealIssueVendor.ToString();
        dr[4] = _iSealConsumed.ToString();
        dr[5] = _iSealWithInst.ToString();
        dr[6] = _iSealPend.ToString();

        _dtData.Rows.Add(dr);

        return _dtData;
    }

    private string GetVendorIDList_DivisionWise(string _sDivision)
    {
        DataTable _dtVendor = new DataTable();
        string _sVendorID = string.Empty;
        _dtVendor = objBL.GetVendorID_DivisionWise(_sDivision);
        if (_dtVendor.Rows.Count > 0)
        {
            for (int i = 0; i < _dtVendor.Rows.Count; i++)
            {
                _sVendorID += _dtVendor.Rows[i][0].ToString();
                _sVendorID += ",";
            }
        }

        if (_sVendorID.Length > 1)
            _sVendorID.Substring(0, _sVendorID.Length - 1);

        if (_sVendorID != "")
            return _sVendorID.Replace(",", "','");
        else
            return "";
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SealReconciliation.aspx");
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
            string filename = "Seal_Deails" + DateTime.Now.ToString() + ".xls";

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
            string filename = "Seal_Details" + DateTime.Now.ToString() + ".xls";

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
        }
        else
        {
            _sortDirection = "ASC";
        }
    }

    public string _sortDirection { get; set; }

    private void DetailsData_Format()
    {
        for (int i = 0; i < gvDetails.Rows.Count; i++)
        {
            gvDetails.Rows[i].Cells[4].Text = gvDetails.Rows[i].Cells[4].Text.TrimStart('0');
        }
    }
    protected void txtDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", txtDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}