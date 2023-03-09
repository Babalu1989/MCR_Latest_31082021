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

public partial class frmMeterShiftingDetails : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    string _gIntallerName = string.Empty;
    int pCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                bindMainDataGrid("", "", "", "", "");
                BindVendorDropDown();
               // BindDivisioin();
                tr2.Visible = false;
                tr2.Visible = false;
                btnSave.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSave, "") + ";this.value='Please wait....';this.disabled = true;");
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
        ddlVendorName.Items.Insert(0, new ListItem("-Select One-", ""));
    }
    private void bindMainDataGrid(string Orderid, string Status, string Division, string Fromdate, string Todate)
    {
        lblSelectedCase.Text = "0";
        lblTotalCase.Text = "0";
        Div1.Visible = true;
        DataTable _gdtDetails = objBL.GetMeterShifting_Details(Orderid, Status, Session["Divison"].ToString(), Fromdate, Todate);

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
    private void DetailsData_Format()
    {
        for (int i = 0; i < gvMainData.Rows.Count; i++)
        {
            gvMainData.Rows[i].Cells[2].Text = gvMainData.Rows[i].Cells[2].Text.TrimStart('0');
            gvMainData.Rows[i].Cells[4].Text = gvMainData.Rows[i].Cells[4].Text.TrimStart('0');
        }
    }
    //public void BindDivisioin()
    //{
    //    DataTable _dtBindName = objBL.getDivisionDetails(Convert.ToString(Session["Divison"]));
    //    if (_dtBindName.Rows.Count > 0)
    //    {
    //        ddlDivision.DataSource = _dtBindName;
    //        ddlDivision.DataTextField = "DIVISION_NAME";
    //        ddlDivision.DataValueField = "DIST_CD";
    //        ddlDivision.DataBind();
    //    }

    //    if (Session["ROLE"] != null)
    //    {
    //        if ((Session["ROLE"].ToString() == "A") || (Session["ROLE"].ToString() == "PV"))
    //        {
    //            ddlDivision.Items.Insert(0, new ListItem("-ALL-", "0"));

    //            if (_dtBindName.Rows.Count == 2)
    //            {
    //                ddlDivision.SelectedIndex = 2;
    //            }
    //        }
    //    }
    //}
    protected void lknbutton_Click(object sender, EventArgs e)
    {
        var strdata = (Control)sender;
        GridViewRow row = (GridViewRow)strdata.NamingContainer;
        lblorderid.Text = row.Cells[0].Text.ToString();
        lbldivision.Text = row.Cells[1].Text.ToString();
        lblactivityold.Text = row.Cells[7].Text.ToString();
        lblactivitynew.Text = row.Cells[8].Text.ToString();
        txtrmk.Text = row.Cells[9].Text.ToString();
        lblprevamt.Text = "Rs. " + row.Cells[11].Text.ToString();
        string[] ActivityCode = lblactivitynew.Text.Split('-');
        DataTable ActivityAmt = objBL.getGetActivityamt(ActivityCode[0].ToString());
        if (ActivityAmt.Rows.Count > 0)
        {
            lblcurramt.Text = "Rs. " + ActivityAmt.Rows[0]["AMOUNT"].ToString();
            if (ActivityAmt.Rows[0]["AMOUNT"].ToString() != "" && row.Cells[11].Text.ToString() != "" && row.Cells[11].Text.ToString() != "&nbsp;")
            {
                lblamtdiff.Text = "Rs. " + Convert.ToString(Convert.ToInt32(row.Cells[11].Text.ToString()) - Convert.ToInt32(ActivityAmt.Rows[0]["AMOUNT"].ToString()));
            }
        }
        divAttachment.Visible = true;
        txtrmk.ReadOnly = true;
    }
    //protected void btnUpdate_Click(object sender, EventArgs e)
    //{
    //    int i = 0;
    //    if (txtremarks.Text.Trim() == "" || txtremarks.Text.Trim() == null)
    //    {
    //        SimpleMethods.show("Please Enter Drum Number");
    //        txtremarks.Focus();
    //        return;
    //    }
    //    try
    //    {
    //        i = objBL.Update_Cablelength(txtremarks.Text.ToString().ToUpper(), txtremarks.Text.ToString().ToUpper(), txtremarks.Text.ToString().ToUpper(), Session["strid"].ToString(), Session["materialid"].ToString(), txtremarks.Text.ToString(), txtremarks.Text.ToString());
    //        if (i > 0)
    //        {
    //            bindMainDataGrid("", "", "", "");
    //            txtremarks.Text = "";
    //            SimpleMethods.show("Record Updated Successfully.");
    //            return;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        SimpleMethods.show(ex.Message.ToString());
    //        return;
    //    }
    //}
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bindMainDataGrid(txtorderid.Text.ToString(), ddlactivitytype.SelectedItem.Text.ToString(), Session["Divison"].ToString(), txtPostingDateFrom.Text.ToString(), txtPostingDateTo.Text.ToString());
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmMeterShiftingDetails.aspx");
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        divAttachment.Visible = false;
    }
    protected void btnDocClose_Click(object sender, ImageClickEventArgs e)
    {
        divAttachment.Visible = false;
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (ddlstatus.SelectedItem.Text == "-Select-")
        {
            SimpleMethods.show("Please select action type option.");
            ddlstatus.Focus();
            return;
        }
        if (txtmmgrmk.Text == "")
        {
            SimpleMethods.show("Please enter mmg coordinator remark.");
            txtmmgrmk.Focus();
            return;
        }
        try
        {
            int i = objBL.Update_Shifting_Details(lblorderid.Text, lbldivision.Text, ddlstatus.SelectedItem.Text.ToUpper(), txtmmgrmk.Text.ToString(), lblamtdiff.Text.Replace("Rs.", "").Replace("-", ""));
            if (i > 0)
            {
                SimpleMethods.show("Record Updated Successfully.");
                bindMainDataGrid("", "", "", "", "");
                divAttachment.Visible = false;
                return;
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show(ex.Message.ToString());
            return;
        }
    }
    protected void btnExcel_Click(object sender, ImageClickEventArgs e)
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
            string filename = "Meter_Shifting" + DateTime.Now.ToString() + ".xls";

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