using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Report_frmCableSummaryRpt : System.Web.UI.Page
{
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
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                btnSave.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSave, "") + ";this.value='Please wait....';this.disabled = true;");
                BindDetails("", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString());
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
    public void BindDetails(string strFromdate,string strtodate,string strdiv,string strVendorid)
    {
        DataTable _dtBindName = objBL.getDetail(strFromdate, strtodate, strdiv, strVendorid);
        if (_dtBindName.Rows.Count > 0)
        {
            gvMainCable.DataSource = _dtBindName;
            gvMainCable.DataBind();
            btnExcel.Visible = true;
            ViewState["Main"] = _dtBindName;
        }
        else
        {
            gvMainCable.DataSource = null;
            gvMainCable.DataBind();
        }
        //if (_dtBindName.Rows.Count == 1)
        //    ddlDivision.SelectedIndex = 1;
       
    }

    public void BindVendor(string Vendorid, string Division, string Roleid)//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendorid, Division, Roleid);
        if (_dtEmpName.Rows.Count > 0)
        {
            txtVendor.DataSource = _dtEmpName;
            txtVendor.DataTextField = "VENDOR_NAME";
            txtVendor.DataValueField = "VENDOR_ID";
            txtVendor.DataBind();
            txtVendor.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
        {
            txtVendor.SelectedIndex = 1;
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
            string filename = "ExpectionalDetails" + DateTime.Now.ToString() + ".xls";

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
        Response.Redirect("frmCableSummaryRpt.aspx");
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
            string filename = "Cable_Report" + DateTime.Now.ToString() + ".xls";
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
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/HomePage.aspx");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BindDetails(txtFromDate.Text.ToString(), txtToDate.Text.ToString(), ddlDivision.SelectedValue,txtVendor.Text.ToString());
    }
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}