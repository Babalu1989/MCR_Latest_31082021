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

public partial class Report_MCRSummaryRpt : System.Web.UI.Page
{
    /// <summary>
    /// Developed by Sanjeev Ranjan on dt 30/01/2019
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
    public void getMaineData()
    {
        string _sDivision = string.Empty;

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

        DataTable _dtDetails = objBL.getMCR_SummaryRpt(txtFromDate.Text, txtToDate.Text, _sDivision);
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

            //SimpleMethods.show("No Data Found.");
        }
    }
    private DataTable GetTotal_Data(DataTable _dtData)
    {        
        DataRow dr;
        int _iTotCase = 0, _iPendCase = 0, _iAllCase = 0, _iCompCase=0,_iRevtCase=0;
        for (int i = 0; i < _dtData.Rows.Count; i++)
        {
            _iTotCase = _iTotCase + Convert.ToInt32(_dtData.Rows[i][2].ToString());
            _iPendCase = _iPendCase + Convert.ToInt32(_dtData.Rows[i][3].ToString());
            _iAllCase = _iAllCase + Convert.ToInt32(_dtData.Rows[i][4].ToString());
            _iCompCase = _iCompCase + Convert.ToInt32(_dtData.Rows[i][5].ToString());
            _iRevtCase = _iRevtCase + Convert.ToInt32(_dtData.Rows[i][6].ToString());
        }

        dr = _dtData.NewRow();

        dr[0] = " ";
        dr[1] = "Total";
        dr[2] = _iTotCase.ToString();
        dr[3] = _iPendCase.ToString();
        dr[4] = _iAllCase.ToString();
        dr[5] = _iCompCase.ToString();
        dr[6] = _iRevtCase.ToString();

        _dtData.Rows.Add(dr);

        return _dtData;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        getMaineData();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MCRSummaryRpt.aspx");
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
            string filename = "MCR_SummaryRpt" + DateTime.Now.ToString() + ".xls";

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
    protected void txtDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", txtDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}