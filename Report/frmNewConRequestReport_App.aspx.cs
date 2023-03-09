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

public partial class Report_frmNewConRequestReport_App : System.Web.UI.Page
{
    /// <summary>
    /// Developed by Arvinder on dt 02/04/2019
    /// </summary>
    ///

    SimpleBL objBL = new SimpleBL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                BindDivisioin();
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
            Page.MaintainScrollPositionOnPostBack = true;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        getMaineData();
    }

    public void getMaineData()
    {
        string _sDivision = string.Empty;
        Div1.Visible = false;
        DIV2.Visible = false;
        lblDivisionDrill1.Text = "";
        btnExcel.Visible = false;
        gvMainData.DataSource = null;
        gvMainData.DataBind();
        gdvDrill1.DataSource = null;
        gdvDrill1.DataBind();

        lblDivisionDrill2.Text = "";
        btnExcelDrill2.Visible = false;
        DIV3.Visible = false;
        gdvDrill2.DataSource = null;
        gdvDrill2.DataBind();
        ViewState["Data_Drill2"] = null;

        ViewState["Main"] = null;

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if (SimpleMethods.validateDate(txtFromDate.Text, txtToDate.Text) == true)
            {

            }
            else
            {
                txtFromDate.BackColor = System.Drawing.Color.Yellow;
                txtToDate.BackColor = System.Drawing.Color.Yellow;
                SimpleMethods.show("From Activity Date should not be greater than To Date.");
                return;
            }
        }

        if (txtDivision.SelectedItem.Text != "-ALL-")
            _sDivision = txtDivision.SelectedValue;
        else
            _sDivision = Session["Divison"].ToString();

        DataTable _dtDetails = objBL.getNewConAppReport_Main(txtFromDate.Text, txtToDate.Text, _sDivision, Session["COMPANY"].ToString());   
        if (_dtDetails.Rows.Count > 0)
        {
            Div1.Visible = true;
            btnExcel.Visible = true;            
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();           

            if (ViewState["Main"] != null)
            {
                ViewState["Main"] = null;
            }
            ViewState["Main"] = _dtDetails;
        }
        else        
            SimpleMethods.show("No Data Found.");        
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmNewConRequestReport_App.aspx");
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
            string filename = "MCR_SummaryRpt_AppCases" + DateTime.Now.ToString("yyyyMMdd") + ".xls";

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
    protected void gvMainData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "", _gDivision = "";            
            lblDivisionDrill1.Text = "";
            DIV2.Visible = false;
            gdvDrill1.DataSource = null;
            gdvDrill1.DataBind();

            lblDivisionDrill2.Text = "";
            btnExcelDrill2.Visible = false;
            DIV3.Visible = false;
            gdvDrill2.DataSource = null;
            gdvDrill2.DataBind();
            ViewState["Data_Drill2"] = null;

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

            _gDivision = e.CommandArgument.ToString().Trim();
            lblDivisionDrill1.Text = e.CommandName.Trim();

            DataTable _dtDetails = objBL.getNewConAppReport_Drill1(_gFrom, _gTo, _gDivision, Session["COMPANY"].ToString());     
            if (_dtDetails.Rows.Count > 0)
            {
                DIV2.Visible = true;
                gdvDrill1.DataSource = _dtDetails;
                gdvDrill1.DataBind();                
            }            
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }
    protected void gdvDrill1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string _gFrom = "", _gTo = "", _gDivision = "", _sFlag = "";
            lblDivisionDrill2.Text = "";
            btnExcelDrill2.Visible = false;
            DIV3.Visible = false;
            gdvDrill2.DataSource = null;
            gdvDrill2.DataBind();
            ViewState["Data_Drill2"] = null;

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

            _gDivision = e.CommandArgument.ToString().Trim();
            lblDivisionDrill2.Text = lblDivisionDrill1.Text.Trim();
            _sFlag = e.CommandName.Trim();

            DataTable _dtDetails = objBL.getNewConAppReport_Drill2(_gFrom, _gTo, _gDivision, Session["COMPANY"].ToString(), _sFlag, lblDivisionDrill2.Text.Trim());     
            if (_dtDetails.Rows.Count > 0)
            {
                DIV3.Visible = true;
                btnExcelDrill2.Visible = true;
                gdvDrill2.DataSource = _dtDetails;
                gdvDrill2.DataBind();

                if (ViewState["Data_Drill2"] != null)
                {
                    ViewState["Data_Drill2"] = null;
                }

                ViewState["Data_Drill2"] = _dtDetails;
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }
    protected void btnExcelDrill2_Click(object sender, ImageClickEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Data_Drill2"];

        HtmlForm form = new HtmlForm();
        Response.Clear();
        Response.Buffer = true;
        if (_dtDetails.Rows.Count > 0)
        {
            GridView grddetails = new GridView();
            grddetails.DataSource = _dtDetails;
            grddetails.DataBind();
            string filename = "MCR_AppCases_Details" + DateTime.Now.ToString("yyyyMMdd") + ".xls";

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
}