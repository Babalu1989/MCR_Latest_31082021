/*
 Developer Name:Babalu Kumar
 Req. No.:REQ2705202020141
 PCN No.:2107210708
 Purpose:Add Cable lenth reconcilation process
 */
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

public partial class frmCableLengthMST : System.Web.UI.Page
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
                //bindMainDataGrid("", "", "", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(), Session["ROLE"].ToString());
                bindMainDataGrid("", "", "", "", "", "", Session["ROLE"].ToString(),"","");
                BindDivisioin();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor                  
                //tr2.Visible = false;
                //tr2.Visible = false;
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
    private void bindMainDataGrid(string Materialno, string Fromdate, string Todate, string MaterialDoc,string Division,string Vendor,string Role,string Updatefrom,string  UpdateTo)
    {
        lblSelectedCase.Text = "0";
        // lblTotalCase.Text = "0";
        Div1.Visible = true;
        DataTable _gdtDetails = objBL.GetCable_Details(Materialno, Fromdate, Todate, MaterialDoc, Division, Vendor,Role,Updatefrom,UpdateTo);

        if (_gdtDetails.Rows.Count > 0)
        {
            lblSelectedCase.Text = _gdtDetails.Rows.Count.ToString();
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

        if (Session["ROLE"] != null)
        {
            if ((Session["ROLE"].ToString() == "A") || (Session["ROLE"].ToString() == "PV"))
            {
                ddlDivision.Items.Insert(0, new ListItem("-ALL-", "0"));

                if (_dtBindName.Rows.Count == 2)
                {
                    ddlDivision.SelectedIndex = 2;
                }
            }
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
            ddlVendorName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
            ddlVendorName.SelectedIndex = 1;
    }

    public void BindDivisioin1()
    {
        DataTable _dtBindName = objBL.getDivision1(Convert.ToString(Session["Divison"]));
        if (_dtBindName.Rows.Count > 0)
        {
            ddldiv.DataSource = _dtBindName;
            ddldiv.DataTextField = "DIVISION_NAME";
            ddldiv.DataValueField = "DIST_CD";
            ddldiv.DataBind();
            ddldiv.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
    }

    protected void lknbutton_Click(object sender, EventArgs e)
    {
        var strdata = (Control)sender;
        GridViewRow row = (GridViewRow)strdata.NamingContainer;
        string strmid = row.Cells[0].Text.ToString();
        Session["strid"] = strmid.ToString();
        string strmaterialid = row.Cells[1].Text.ToString();
        Session["materialid"] = strmaterialid.ToString();
        BindDivisioin1();
        Fill_Data(Convert.ToString(Session["strid"]), Convert.ToString(Session["materialid"]));
        divAttachment.Visible = true;
    }

    public void Fill_Data(string strId, string MaterialId)
    {
        DataTable _dtdetails = objBL.getDocDetails(strId, MaterialId);
        if (_dtdetails.Rows.Count > 0)
        {
            BindVendor(_dtdetails.Rows[0]["DIV_CODE"].ToString().ToUpper());
            for (int i = 0; i < ddldiv.Items.Count; i++)
            {
                if (ddldiv.Items[i].Text.ToUpper() == _dtdetails.Rows[0]["DIVISION"].ToString().ToUpper())
                {
                    ddldiv.SelectedIndex = i;
                }
            }
            for (int i = 0; i < ddlvendor.Items.Count; i++)
            {
                if (ddlvendor.Items[i].Text.ToUpper() == _dtdetails.Rows[0]["VENDOR_NAME"].ToString().ToUpper())
                {
                    ddlvendor.SelectedIndex = i;
                }
            }
            for (int i = 0; i < ddlcablesize.Items.Count; i++)
            {
                if (ddlcablesize.Items[i].Text.ToUpper() == _dtdetails.Rows[0]["CABLE_SIZE"].ToString().ToUpper())
                {
                    ddlcablesize.SelectedIndex = i;
                }
            }
            for (int i = 0; i <ddlcabletype.Items.Count; i++)
            {
                if (ddlcabletype.Items[i].Text.ToUpper() == _dtdetails.Rows[0]["CHARG_D_BATCH_NUMBER"].ToString().ToUpper())
                {
                    ddlcabletype.SelectedIndex = i;
                }
            }
            txtdateofissues.Text = _dtdetails.Rows[0]["DATE_OF_ISSUES"].ToString();
            txtQuantity.Text = _dtdetails.Rows[0]["MENGE_D_QUANTITY"].ToString();
            txtmsrno.Text = _dtdetails.Rows[0]["RSNUM_NUMBER_RESERVATION"].ToString();
            txtserialnofrom.Text = _dtdetails.Rows[0]["SERIAL_NO_FROM"].ToString();
            txtserialnoto.Text = _dtdetails.Rows[0]["SERIAL_NO_TO"].ToString();
            txtmake.Text = _dtdetails.Rows[0]["MAKE"].ToString();
            txtDrumno.Text = _dtdetails.Rows[0]["DRUM_NO"].ToString();
        }
    }

    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if ((!String.IsNullOrEmpty(txtPostingDateFrom.Text)) && (String.IsNullOrEmpty(txtPostingDateTo.Text)))
        {
            SimpleMethods.show("Please Enter Issues Date From and To");
            return;
        }
        if ((String.IsNullOrEmpty(txtPostingDateFrom.Text)) && (!String.IsNullOrEmpty(txtPostingDateTo.Text)))
        {
            SimpleMethods.show("Please Enter Date Issues Date From and To");
            return;
        }
        if ((!String.IsNullOrEmpty(txtPostingDateFrom.Text)) && (!String.IsNullOrEmpty(txtPostingDateTo.Text.Trim())))
        {
            DateTime postingDateFrom = Convert.ToDateTime(txtPostingDateFrom.Text.Trim());
            DateTime postingDateTo = Convert.ToDateTime(txtPostingDateTo.Text.Trim());

            int duration = postingDateTo.CompareTo(postingDateFrom);
            if (duration < 0)
            {
                SimpleMethods.show("To Date Must Be Greater Than Or Equal To From Date.");
                return;
            }
        }

        if ((!String.IsNullOrEmpty(txtupdatefrom.Text)) && (String.IsNullOrEmpty(txtupdateto.Text)))
        {
            SimpleMethods.show("Please Enter Updated Date From and To");
            return;
        }
        if ((String.IsNullOrEmpty(txtupdatefrom.Text)) && (!String.IsNullOrEmpty(txtupdateto.Text)))
        {
            SimpleMethods.show("Please Enter Updated Date From and To");
            return;
        }
        if ((!String.IsNullOrEmpty(txtupdatefrom.Text)) && (!String.IsNullOrEmpty(txtupdateto.Text)))
        {
            DateTime postingDateFrom = Convert.ToDateTime(txtupdatefrom.Text.Trim());
            DateTime postingDateTo = Convert.ToDateTime(txtupdateto.Text.Trim());

            int duration = postingDateTo.CompareTo(postingDateFrom);
            if (duration < 0)
            {
                SimpleMethods.show("To Date Must Be Greater Than Or Equal To From Date.");
                return;
            }
        }
        //if (ddlDivision.SelectedValue == "0" && ddlVendorName.SelectedValue == "0")
        //{
        //    bindMainDataGrid(txtmaterial.Text.ToString(), txtPostingDateFrom.Text.ToString(), txtPostingDateTo.Text.ToString(), txtMaterialDocNo.Text.ToString(), Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(), Session["ROLE"].ToString());
        //}
        //else if (ddlDivision.SelectedValue == "0" && ddlVendorName.SelectedValue != "0")
        //{
        //    bindMainDataGrid(txtmaterial.Text.ToString(), txtPostingDateFrom.Text.ToString(), txtPostingDateTo.Text.ToString(), txtMaterialDocNo.Text.ToString(), Session["Divison"].ToString(), ddlVendorName.SelectedValue, Session["ROLE"].ToString());
        //}
        //else if (ddlDivision.SelectedValue != "0" && ddlVendorName.SelectedValue == "0")
        //{
        //    bindMainDataGrid(txtmaterial.Text.ToString(), txtPostingDateFrom.Text.ToString(), txtPostingDateTo.Text.ToString(), txtMaterialDocNo.Text.ToString(), ddlDivision.SelectedValue, Session["VENDOR_ID"].ToString(), Session["ROLE"].ToString());
        //}
       // else
       // {
            bindMainDataGrid(txtmaterial.Text.ToString(), txtPostingDateFrom.Text.ToString(), txtPostingDateTo.Text.ToString(), txtMaterialDocNo.Text.ToString(), ddlDivision.SelectedValue, ddlVendorName.SelectedValue, Session["ROLE"].ToString(),txtupdatefrom.Text.ToString(),txtupdateto.Text.ToString());
       // }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmCableLengthMST.aspx");
    }

    protected void txtPostingDateTo_TextChanged(object sender, EventArgs e)
    {
        divAttachment.Visible = false;
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
        int i = 0;
        if (ddldiv.SelectedValue.Trim() == "0")
        {
            SimpleMethods.show("Please Select Division Option.");
            ddldiv.Focus();
            return;
        }
        if (txtDrumno.Text.Trim() == "" || txtDrumno.Text.Trim() == null)
        {
            SimpleMethods.show("Please Enter Drum Number");
            txtDrumno.Focus();
            return;
        }
        else if (ddlcablesize.SelectedItem.Text.ToString() == "Select")
        {
            SimpleMethods.show("Please Select Cable Size");
            ddlcablesize.Focus();
            return;
        }
        else if (txtQuantity.Text.Trim() == "" || txtQuantity.Text.Trim() == null)
        {
            SimpleMethods.show("Please Enter Cable Length");
            txtQuantity.Focus();
            return;
        }
        try
        {
            //i = objBL.Update_Cablelength(txtDrumno.Text.ToString().ToUpper(), txtCablesize.Text.ToString().ToUpper(), txtQuantity.Text.ToString().ToUpper(),
            //    Session["strid"].ToString(), Session["materialid"].ToString(), txtserialnofrom.Text.ToString(), txtserialnoto.Text.ToString(), ddldiv.SelectedValue,
            //    ddlvendor.SelectedItem.Text.ToString(), ddlvendor.SelectedValue, txtmsrno.Text.ToString(), ddlcabletype.SelectedValue, txtdateofissues.Text.ToString());

            i = objBL.Update_Cablelength(ddldiv.SelectedValue, ddldiv.SelectedItem.Text.ToString().ToUpper(), ddlvendor.SelectedItem.Text.ToString(), ddlvendor.SelectedValue, txtdateofissues.Text.ToString(),
                txtmsrno.Text.ToString().ToUpper(), txtmake.Text.ToString().ToUpper(), txtDrumno.Text.ToString().ToUpper(), ddlcablesize.SelectedItem.Text.ToString(), txtQuantity.Text.ToString(),
                ddlcabletype.SelectedValue, txtserialnofrom.Text.ToString(), txtserialnoto.Text.ToString(), Session["strid"].ToString(), Session["materialid"].ToString());
            if (i > 0)
            {
                bindMainDataGrid("", "", "", "","","","","","");
                txtDrumno.Text = "";
                ddlcablesize.SelectedIndex = 0;
                txtQuantity.Text = "";
                txtdateofissues.Text = "";
                ddlcabletype.SelectedIndex = 0;
                ddldiv.SelectedIndex = 0;
                ddlvendor.SelectedIndex = 0;
                divAttachment.Visible = false;
                bindMainDataGrid("", "", "", "", Session["Divison"].ToString(), Session["VENDOR_ID"].ToString(), Session["ROLE"].ToString(),"","");
                SimpleMethods.show("Record Updated Successfully.");
                return;
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show(ex.Message.ToString());
            return;
        }
    }

    protected void ddldiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor(ddldiv.SelectedValue);
    }

    public void BindVendor(string Vendor)
    {
        DataTable _dtVendor = objBL.getVendorId(Vendor);
        if (_dtVendor.Rows.Count > 0)
        {

            ddlvendor.DataSource = _dtVendor;
            ddlvendor.DataTextField = "VENDOR_NAME";//VENDOR_NAME
            ddlvendor.DataValueField = "VENDOR_ID";
            ddlvendor.DataBind();
            ddlvendor.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }

    protected void btnExcel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["DataTable"];
        //ViewState["DataTable"] = _gdtDetails;

        HtmlForm form = new HtmlForm();
        Response.Clear();
        Response.Buffer = true;
        if (_dtDetails.Rows.Count > 0)
        {
            GridView grddetails = new GridView();
            grddetails.DataSource = _dtDetails;
            grddetails.DataBind();
            string filename = "CableDetails" + DateTime.Now.ToString() + ".xls";

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