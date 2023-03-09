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
using System.Web.UI.DataVisualization.Charting;
using System.Web.Services;

public partial class SealAllotmentTransfer : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                BindDivisioin();
                getDataMCRPunchingForSeriesAllocation();
                btnSubmitSealAllocation.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSubmitSealAllocation, "") + ";this.value='Please wait....';this.disabled = true;");
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
            }
            bindLineChart();
        }
        else
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("Default.aspx");
        }
    }

    public void bindLineChart()
    {
        try
        {
            DataTable _gdtEmpDetail = new DataTable();


            if ((ddlDivision.SelectedValue == "0") && (ddlVendor.SelectedValue == "0"))
                _gdtEmpDetail = objBL.getEmpDetails(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]),Convert.ToString(Session["COMPANY"]));
            else if ((ddlDivision.SelectedValue != "0") && (ddlVendor.SelectedValue != "0"))
                _gdtEmpDetail = objBL.getEmpDetails(ddlVendor.SelectedValue, ddlDivision.SelectedValue, Convert.ToString(Session["COMPANY"]));
            else if ((ddlDivision.SelectedValue != "0") && (ddlVendor.SelectedValue == "0"))
                _gdtEmpDetail = objBL.getEmpDetails(Convert.ToString(Session["VENDOR_ID"]), ddlDivision.SelectedValue,Convert.ToString(Session["COMPANY"]));
            else if ((ddlDivision.SelectedValue == "0") && (ddlVendor.SelectedValue != "0"))
                _gdtEmpDetail = objBL.getEmpDetails(ddlVendor.SelectedValue, Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));

            if (_gdtEmpDetail.Rows.Count > 0)
            {
                for (int i = 4; i < _gdtEmpDetail.Columns.Count; i++)
                {
                    Series series = new Series();
                    foreach (DataRow dr in _gdtEmpDetail.Rows)
                    {
                        int y = Convert.ToInt32(dr[i].ToString());
                        series.Points.AddXY(dr["EmpName"].ToString(), y);
                    }
                    Chart_Meter.Series.Add(series);
                     lblMapTitle.Visible = true;
                }
            }           
            else
                lblMapTitle.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    public void getDataMCRPunchingForSeriesAllocation()
    {
        try
        {    
            DataTable _gdtEmpDetail = objBL.getEmpDetails(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), 
                                                                        Convert.ToString(Session["COMPANY"]));

            if (_gdtEmpDetail.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _gdtEmpDetail;
                gvSeriesWiseAllocation.DataBind();

                BlankHeader.Visible = false;
            }
            else
            {
                gvSeriesWiseAllocation.DataSource = _gdtEmpDetail;
                gvSeriesWiseAllocation.DataBind();
                BlankHeader.Visible = true;

                tr1.Visible = false;
                tr3.Visible = false;
                btnSubmitSealAllocation.Visible = false;
                btnCancelSealAllocation.Visible = false;
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void rdbdiscontinued_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            RadioButton rd = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rd.NamingContainer;

            txtName.Text = (row.FindControl("lblEMPNAME") as Label).Text;
            txtEmployeeID.Text = (row.FindControl("lblInstallerID") as Label).Text;

            if((ddlDivision.SelectedValue=="0") && (ddlVendor.SelectedValue=="0"))
                BindSealDetails(txtEmployeeID.Text, Session["Divison"].ToString(), Convert.ToString(Session["VENDOR_ID"]));//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
            else if((ddlDivision.SelectedValue !="0") && (ddlVendor.SelectedValue!="0"))
                BindSealDetails(txtEmployeeID.Text, ddlDivision.SelectedValue, ddlVendor.SelectedValue);
            else if((ddlDivision.SelectedValue !="0") && (ddlVendor.SelectedValue=="0"))
                BindSealDetails(txtEmployeeID.Text, ddlDivision.SelectedValue, Convert.ToString(Session["VENDOR_ID"]));//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
            else
                BindSealDetails(txtEmployeeID.Text, ddlDivision.SelectedValue, Convert.ToString(Session["VENDOR_ID"]));//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 

            BindInstaller_Data_VendorWise(txtEmployeeID.Text);

            tr1.Visible = true;            
            tr3.Visible = true;
            btnSubmitSealAllocation.Visible = true;
            btnCancelSealAllocation.Visible = true;
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    public void BindSealDetails(string _sIstallerID, string _sDivision, string _sVendorID)
    {
        try
        {
            chboxListSealNO.Items.Clear();
            DataTable _dtSeal = objBL.getSealDetails_InstallerWise(_sVendorID, _sIstallerID, _sDivision);
            {
                for (int i = 0; i < _dtSeal.Rows.Count; i++)
                {
                    chboxListSealNO.Items.Add(_dtSeal.Rows[i]["Seal"].ToString());                    
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    private void BindInstaller_Data_VendorWise(string _sEmpID)
    {
        DataTable _dtInstaller = new DataTable();
        if (ddlVendor.SelectedValue == "0")
            _dtInstaller = objBL.getInstaller_EmpDetails(Convert.ToString(Session["VENDOR_ID"]), _sEmpID, Session["Divison"].ToString());
        else
            _dtInstaller = objBL.getInstaller_EmpDetails(ddlVendor.SelectedValue, _sEmpID, Session["Divison"].ToString());      

        if (_dtInstaller.Rows.Count > 0)
        {
            ddlInstaller.Items.Clear();
            ddlInstaller.DataSource = _dtInstaller;
            ddlInstaller.DataTextField = "EMPNAME";
            ddlInstaller.DataValueField = "EMP_ID";
            ddlInstaller.DataBind();
            ddlInstaller.Items.Insert(0, new ListItem("-SELECT-", "0"));
        }
        else
        {
            ddlInstaller.Items.Clear();
            ddlInstaller.Items.Insert(0, new ListItem("-SELECT-", "0"));
        }

    }

    protected void btnSubmitSealAllocation_Click(object sender, EventArgs e)
    {
        int _iCount = 0;
        if (ddlInstaller.SelectedValue != "0")
        {
            for (int i = 0; i < chboxListSealNO.Items.Count; i++)
            {
                if (chboxListSealNO.Items[i].Selected == true)
                {
                    _iCount++;
                    objBL.Assign_Seal_Allocation(lblInstallerID.Text.Trim(), "", chboxListSealNO.Items[i].ToString(), Convert.ToString(Session["UserName"]));
                }
            }

            if(_iCount==0)
                SimpleMethods.show("Kindly Select Any Seal and Try Again..");
            else
                SimpleMethods.MsgBoxWithLocation("Seals has been Transfer to " + txtName.Text.Trim(), "SealAllotmentTransfer.aspx", this);
        }
        else
        {
            SimpleMethods.show("Please Select Installer Name and Try Again..");
        }
    }

    protected void btnCancelSealAllocation_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
    }

    protected void ddlInstaller_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInstaller.SelectedValue != "0")
        {
            lblInstallerID.Text = ddlInstaller.SelectedValue;
        }
        else
        {
            SimpleMethods.show("Please Try Again.");
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

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable _dtData = new DataTable();

        if (ddlDivision.SelectedValue == "0")
        {
            _dtData = objBL.getSealEmpDetails(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));

            if (_dtData.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _dtData;
                gvSeriesWiseAllocation.DataBind();

                BlankHeader.Visible = false;
            }
            else
            {
                gvSeriesWiseAllocation.DataSource = _dtData;
                gvSeriesWiseAllocation.DataBind();
                BlankHeader.Visible = true;

                tr1.Visible = false;
                tr3.Visible = false;
                btnSubmitSealAllocation.Visible = false;
                btnCancelSealAllocation.Visible = false;
            }
        }
        else
        {
            // BindVendor_DivWise(ddlDivision.SelectedValue);//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 

            BindVendor("", ddlDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
            _dtData = objBL.getSealEmpDetails(Convert.ToString(Session["VENDOR_ID"]), ddlDivision.SelectedValue, Convert.ToString(Session["COMPANY"]));

            if (_dtData.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _dtData;
                gvSeriesWiseAllocation.DataBind();

                BlankHeader.Visible = false;
            }
            else
            {
                gvSeriesWiseAllocation.DataSource = _dtData;
                gvSeriesWiseAllocation.DataBind();
                BlankHeader.Visible = true;

                tr1.Visible = false;
                tr3.Visible = false;
                btnSubmitSealAllocation.Visible = false;
                btnCancelSealAllocation.Visible = false;
            }
        }

        bindLineChart();
    }

    public void BindVendor_DivWise(string _sDivID)
    {
        DataTable _dtBindName = objBL.GetVendorList_DivWise(_sDivID);

        if (_dtBindName.Rows.Count > 0)
        {
            ddlVendor.DataSource = _dtBindName;
            ddlVendor.DataTextField = "VENDOR_NAME";
            ddlVendor.DataValueField = "VENDOR_ID";
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, new ListItem("ALL", "0"));

            btnViewData.Visible = true;
        }
        else
        {
            btnViewData.Visible = false;
        }


        if (ddlVendor.Items.Count == 2)
            ddlVendor.SelectedIndex = 1;

    }

    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Show_Data();
    }

    private void Show_Data()
    {
        DataTable _dtData = new DataTable();
        string _sDivision = string.Empty;

        if (ddlVendor.SelectedValue == "0")
        {
            if (ddlDivision.SelectedValue == "0")
                _sDivision = Convert.ToString(Session["Divison"]);
            else
                _sDivision = ddlDivision.SelectedValue;

            _dtData = objBL.getSealEmpDetails(Convert.ToString(Session["VENDOR_ID"]), _sDivision, Convert.ToString(Session["COMPANY"]));

            if (_dtData.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _dtData;
                gvSeriesWiseAllocation.DataBind();

                BlankHeader.Visible = false;
            }
            else
            {
                gvSeriesWiseAllocation.DataSource = _dtData;
                gvSeriesWiseAllocation.DataBind();
                BlankHeader.Visible = true;

                tr1.Visible = false;
                tr3.Visible = false;
                btnSubmitSealAllocation.Visible = false;
                btnCancelSealAllocation.Visible = false;
            }
        }
        else
        {
            _dtData = objBL.getSealEmpDetails(ddlVendor.SelectedValue, ddlDivision.SelectedValue, Convert.ToString(Session["COMPANY"]));

            if (_dtData.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _dtData;
                gvSeriesWiseAllocation.DataBind();

                BlankHeader.Visible = false;
            }
            else
            {
                gvSeriesWiseAllocation.DataSource = _dtData;
                gvSeriesWiseAllocation.DataBind();
                BlankHeader.Visible = true;

                tr1.Visible = false;
                tr3.Visible = false;
                btnSubmitSealAllocation.Visible = false;
                btnCancelSealAllocation.Visible = false;
            }
        }

        bindLineChart();
    }

    protected void btnViewData_Click(object sender, EventArgs e)
    {
        Show_Data();
    }
    
    protected void btnChecked_Click(object sender, EventArgs e)
    {
        if (btnChecked.Text == "Checked ALL")
        {
            for (int i = 0; i < chboxListSealNO.Items.Count; i++)
            {
                chboxListSealNO.Items[i].Selected = true;
            }
            btnChecked.Text = "Unchecked ALL";
        }
        else
        {
            for (int i = 0; i < chboxListSealNO.Items.Count; i++)
            {
                chboxListSealNO.Items[i].Selected = false;
            }
            btnChecked.Text = "Checked ALL";
        }
    }

    public void BindVendor(string Vendorid, string Division,string Roleid)
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendorid, Division,Roleid);
        if (_dtEmpName.Rows.Count > 0)
        {
            ddlVendor.DataSource = _dtEmpName;
            ddlVendor.DataTextField = "VENDOR_NAME";
            ddlVendor.DataValueField = "VENDOR_ID";
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
            ddlVendor.SelectedIndex = 1;
    }
}