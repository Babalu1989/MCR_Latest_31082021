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
    Image sortImage = new Image();
    string strVendorid = string.Empty;

    /// <summary>
    /// Developed by Gourav Gouton on Date 14.11.2017 guide given by Swati Kaushik
    /// Developed for Add and Modified Users
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null && Session["Divison"] != null)
        {
            if (!IsPostBack)
            {
                txtCompany.Text = Session["COMPANY"].ToString();
                BindDivisioin();
                BindRoleDetails();
                if ((Session["ROLE"] != null) && (ddlRole.Items.Count > 1))
                {
                    if (Session["ROLE"].ToString() == "V")
                    {
                        //ddlRole.SelectedIndex = 1;
                        ddlRole.SelectedIndex = 0;//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                        ddlRole.Enabled = false;

                        getEmployeeDetails("", Session["Divison"].ToString(), "");
                    }
                    else if (Session["ROLE"].ToString() == "A")
                    {
                        ddlRole.SelectedIndex = 0;//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                        ddlRole.Items.RemoveAt(4);
                        ddlRole.Items.RemoveAt(4);
                        getEmployeeDetails(ddlRole.SelectedValue, Session["Divison"].ToString(), Session["ROLE"].ToString());
                    }
                    else if (Session["ROLE"].ToString() == "R")
                    {
                        ddlRole.Items.RemoveAt(2);
                        ddlRole.SelectedIndex = 0;//Added by Babalu Kumar 31122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                        getEmployeeDetails(ddlRole.SelectedValue, Session["Divison"].ToString(), Session["ROLE"].ToString());

                    }
                }
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

    #region Sorting
    protected void gvMainData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Main"];
        SetSortDirection(SortDireaction);
        if (_dtDetails != null)
        {
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
            gvMainData.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
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
            sortImage.ImageUrl = "view_sort_ascending.png";

        }
        else
        {
            _sortDirection = "ASC";
            sortImage.ImageUrl = "view_sort_descending.png";
        }
    }

    public string _sortDirection { get; set; }
    #endregion

    #region Bind Data
    public void BindDivisioin()
    {
        DataTable _dtBindName = objBL.getDivisionDetails(Convert.ToString(Session["Divison"]));
        if (_dtBindName.Rows.Count > 0)
        {
            ddlDivision.DataSource = _dtBindName;
            ddlDivision.DataTextField = "DIVISION_NAME";
            ddlDivision.DataValueField = "DIST_CD";
            ddlDivision.DataBind();
            ddlDivision.Items.Insert(0, new ListItem("-Select One-", "0"));
        }
    }

    public void BindRoleDetails()
    {
        DataTable _dtBindName = objBL.getRoleDetails(Convert.ToString(Session["LOGIN_TYPE"]));
        if (_dtBindName.Rows.Count > 0)
        {
            ddlRole.DataSource = _dtBindName;
            ddlRole.DataTextField = "ROLE_NAME";
            ddlRole.DataValueField = "ROLE_ID";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("-Select One-", "0"));
        }
    }

    public void getEmployeeDetails(string _sLoginType, string Div, string roleid)
    {
        DataTable _dtDetails = new DataTable();

        if (Session["ROLE"].ToString() == "V")
            _dtDetails = objBL.getEmployeeDetails(Convert.ToString(Session["VENDOR_ID"]), "", _sLoginType, Convert.ToString(Session["Divison"]), roleid);
        else
            _dtDetails = objBL.getEmployeeDetails(Convert.ToString(Session["UserName"]), "", _sLoginType, Convert.ToString(Session["Divison"]), roleid);

        if (_dtDetails.Rows.Count > 0)
        {
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();

            if (ViewState["Main"] != null)
            {
                ViewState["Main"] = null;
            }
            ViewState["Main"] = _dtDetails;
        }
    }
    #endregion

    protected void imgEmpID_Command(object sender, CommandEventArgs e)//Added By Babalu Kumar 28072020
    {
        try
        {
            DataTable _dtDetails = new DataTable();
            string _sRole = string.Empty;
            _sRole = e.CommandName.ToString();
            if (_sRole.ToString().ToUpper() == "VENDOR")
                _sRole = "V";
            else if (_sRole.ToString().ToUpper() == "ADMIN")
                _sRole = "A";
            else if (_sRole.ToString().ToUpper() == "INSTALLER") //Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
                _sRole = "I";
            else if (_sRole.ToString().ToUpper() == "POWER ADMIN")
                _sRole = "R";
            else if (_sRole.ToString().ToUpper() == "VIEWER")
                _sRole = "U";//Added By Babalu Kumar 29072020
            else if (_sRole.ToString().ToUpper() == "POWER VENDOR")
                _sRole = "PV";
            _dtDetails = objBL.getEmployeeDetails(Convert.ToString(Session["UserName"]), e.CommandArgument.ToString(), _sRole, Convert.ToString(Session["Divison"]), "");

            if (_dtDetails.Rows.Count > 0)
            {
                txtName.Text = Convert.ToString(_dtDetails.Rows[0]["EMP_NAME"]);
                txtPassword.Text = Convert.ToString(_dtDetails.Rows[0]["PASSWORD"]);
                txtCompany.Text = Convert.ToString(_dtDetails.Rows[0]["COMPANY"]);
                txtIMEINo.Text = Convert.ToString(_dtDetails.Rows[0]["IMEI_NO"]);
                txtUserID.Text = Convert.ToString(_dtDetails.Rows[0]["EMP_ID"]);
                txtMobNo.Text = Convert.ToString(_dtDetails.Rows[0]["MOBILE_NO"]);
                txtDesgnation.Text = Convert.ToString(_dtDetails.Rows[0]["DESIGNATION"]);
                txtIMEINo1.Text = Convert.ToString(_dtDetails.Rows[0]["IMEI_NO2"]);
                txtUserID.ReadOnly = true;
                txtCompany.ReadOnly = true;

                for (int i = 0; i < ddlDivision.Items.Count; i++)
                {
                    if (ddlDivision.Items[i].Value == Convert.ToString(_dtDetails.Rows[0]["DIVISION"]))
                    {
                        ddlDivision.SelectedIndex = i;
                        BindVendor(ddlDivision.Items[i].Value);//Added By Babalu Kumar 30122020
                        break;
                    }
                }

                for (int i = 0; i < ddlRole.Items.Count; i++)
                {
                    if (ddlRole.Items[i].Text == Convert.ToString(_dtDetails.Rows[0]["EMP_TYPE"]))
                    {
                        ddlRole.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < ddlActivation.Items.Count; i++)
                {
                    if (ddlActivation.Items[i].Value == Convert.ToString(_dtDetails.Rows[0]["ACTIVE_FLAG"]))
                    {
                        ddlActivation.SelectedIndex = i;
                        break;
                    }
                }
                if (_dtDetails.Rows[0]["ROLE"].ToString() == "V")
                {
                    txtvendor.Text = _dtDetails.Rows[0]["VENDOR_ID"].ToString();
                    tabv.Visible = true;
                }
                else if (_dtDetails.Rows[0]["ROLE"].ToString() == "I") //Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
                {
                    for (int i = 0; i < ddlVendorname.Items.Count; i++)
                    {
                        if (ddlVendorname.Items[i].Value == Convert.ToString(_dtDetails.Rows[0]["VENDOR_ID"]))
                        {
                            ddlVendorname.SelectedIndex = i;
                            trid.Visible = true;
                            break;
                        }
                    }
                }
                else if (_dtDetails.Rows[0]["ROLE"].ToString() == "A")
                {
                    for (int i = 0; i < ddlVendorname.Items.Count; i++)
                    {
                        if (ddlVendorname.Items[i].Value == Convert.ToString(_dtDetails.Rows[0]["VENDOR_ID"]))
                        {
                            ddlVendorname.SelectedIndex = i;
                            trid.Visible = true;
                            break;
                        }
                    }
                }
                btnSave.Text = "Update";
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddUser.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text == "")
            {
                SimpleMethods.show("Please enter name.");
                txtName.Focus();
                return;
            }
            if (txtUserID.Text == "")
            {
                SimpleMethods.show("Please enter user Id");
                txtUserID.Focus();
                return;
            }
            if (ddlRole.SelectedValue == "I") //Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
            {
                if (txtIMEINo.Text == "")
                {
                    SimpleMethods.show("Please Enter Your IMEI Number");
                    txtIMEINo.Focus();
                    return;
                }
                else
                {

                }
            }
            if (ddlRole.SelectedValue == "I") //Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
            {
                if (txtMobNo.Text == "")
                {
                    SimpleMethods.show("Please enter mobile number");
                    txtMobNo.Focus();
                    return;
                }
                else
                {

                }
            }
            if (txtDesgnation.Text == "")
            {
                SimpleMethods.show("Please enter designation");
                txtDesgnation.Focus();
                return;
            }
            if (ddlDivision.SelectedItem.Text == "-Select One-")
            {
                SimpleMethods.show("Please select division option");
                ddlDivision.Focus();
                return;
            }
            if (ddlActivation.SelectedItem.Text == "-Select One-")
            {
                SimpleMethods.show("Please select status option");
                ddlActivation.Focus();
                return;
            }
            if (ddlRole.SelectedItem.Text == "-Select One-")
            {
                SimpleMethods.show("Please select role option");
                ddlRole.Focus();
                return;
            }
            if (ddlRole.SelectedValue == "I") //Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
            {
                if (ddlVendorname.SelectedItem.Text == "-Select One-")
                {
                    SimpleMethods.show("Please select vendor option");
                    ddlVendorname.Focus();
                    return;
                }
              
            }
            if (ddlRole.SelectedValue == "V")
            {
                if (txtvendor.Text == "")
                {
                    SimpleMethods.show("Please enter vendor  Id");
                    ddlVendorname.Focus();
                    return;
                }
            }
            if (ddlRole.SelectedValue == "A")
            {
                if (ddlVendorname.SelectedItem.Text == "-Select One-")
                {
                    SimpleMethods.show("Please select vendor  option");
                    ddlVendorname.Focus();
                    return;
                }
            }
            if (btnSave.Text == "Confirm")
                Confirm_User(ddlRole.SelectedValue);

            if (btnSave.Text == "Update")
                Update_User(ddlRole.SelectedValue);
        }
        catch (Exception ex)
        {
            SimpleMethods.show(ex.Message.ToString());
        }
    }

    public void Confirm_User(string strtype)
    {
        DataTable _dtCheck = objBL.Check_USER_DETAILS(txtUserID.Text.Trim());
        if (_dtCheck.Rows.Count > 0)
        {
            if (Convert.ToInt32(_dtCheck.Rows[0]["count"].ToString()) > 0)
            {
                SimpleMethods.show("User Already Exists.");
                return;
            }
            else
            {
                txtPassword.Text = "12345678";

                if (Session["ROLE"].ToString() == "A")
                {
                    if (ddlRole.SelectedValue == "V")
                    {
                        strVendorid = txtvendor.Text.ToString();
                    }
                    else if (ddlRole.SelectedValue == "I")//Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
                    {
                        strVendorid = ddlVendorname.SelectedValue;
                    }
                    else if (ddlRole.SelectedValue == "A")
                    {
                        foreach (ListItem lst in ddlVendorname.Items)
                        {
                            if (lst.Selected == true)
                            {
                                strVendorid += lst.Value + ",";
                            }

                        }  
                       // strVendorid = ddlVendorname.SelectedValue;
                    }
                    objBL.insert_USER_DETAILS(txtName.Text.Trim(), txtUserID.Text.Trim(), txtIMEINo.Text.Trim(), ddlDivision.SelectedValue, strtype, strtype,//Added By Babalu Kumar 26072020 
                                                   strVendorid, "BRPL", txtMobNo.Text, txtDesgnation.Text,txtIMEINo1.Text.Trim());

                    objBL.insert_LOGIN_MST(txtUserID.Text.Trim(), txtPassword.Text.Trim(), ddlRole.SelectedValue, txtName.Text.Trim(), ddlDivision.SelectedValue);
                    if (strtype == "V")
                    {
                        objBL.insert_Vendor_MST(txtName.Text.Trim(), strVendorid, ddlDivision.SelectedValue, txtMobNo.Text, txtDesgnation.Text, "BRPL", txtUserID.Text.ToString());
                        DataTable _dtcheck = objBL.Check_Vendor(ddlDivision.SelectedValue, strVendorid);
                        if (_dtcheck.Rows.Count > 0)
                        {
                            objBL.Insert_MCR_V_D_OTYPE_PMACTMAP(strVendorid, ddlDivision.SelectedValue);//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                        }

                    }
                    else if (strtype == "I")
                    {
                        objBL.insert_Insallter_DETAILS(txtUserID.Text.Trim(), txtName.Text.Trim(), ddlDivision.SelectedValue, txtMobNo.Text,
                      strVendorid, txtDesgnation.Text, txtIMEINo.Text, txtIMEINo.Text);
                    }
                }
                else if (Session["ROLE"].ToString() == "R")
                {
                    if (ddlRole.SelectedValue == "V")
                    {
                        strVendorid = txtvendor.Text.ToString();
                    }
                    else if (ddlRole.SelectedValue == "I")//Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
                    {
                        strVendorid = ddlVendorname.SelectedValue;
                    }
                    else if (ddlRole.SelectedValue == "A")
                    {
                        foreach (ListItem lst in ddlVendorname.Items)
                        {
                            if (lst.Selected == true)
                            {
                                strVendorid += lst.Value + ",";
                            }

                        } 
                        //strVendorid = ddlVendorname.SelectedValue;
                    }
                    objBL.insert_USER_DETAILS(txtName.Text.Trim(), txtUserID.Text.Trim(), txtIMEINo.Text.Trim(), ddlDivision.SelectedValue, ddlRole.SelectedValue, ddlRole.SelectedValue,
                                                   strVendorid, "BRPL", txtMobNo.Text, txtDesgnation.Text, txtIMEINo1.Text.Trim());

                    objBL.insert_LOGIN_MST(txtUserID.Text.Trim(), txtPassword.Text.Trim(), ddlRole.SelectedValue, txtName.Text.Trim(), ddlDivision.SelectedValue);
                    if (strtype == "V")
                    {
                        objBL.insert_Vendor_MST(txtName.Text.Trim(), strVendorid, ddlDivision.SelectedValue, txtMobNo.Text, txtDesgnation.Text, "BRPL", txtUserID.Text.ToString());
                        //objBL.Insert_MCR_V_D_OTYPE_PMACTMAP(strVendorid, ddlDivision.SelectedValue);//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                        DataTable _dtcheck = objBL.Check_Vendor(ddlDivision.SelectedValue, strVendorid);
                        if (_dtcheck.Rows.Count > 0)
                        {
                            objBL.Insert_MCR_V_D_OTYPE_PMACTMAP(strVendorid, ddlDivision.SelectedValue);//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                        }
                    }
                    else if (strtype == "I")
                    {
                        objBL.insert_Insallter_DETAILS(txtUserID.Text.Trim(), txtName.Text.Trim(), ddlDivision.SelectedValue, txtMobNo.Text,
                        strVendorid, txtDesgnation.Text, txtIMEINo.Text, txtIMEINo.Text);
                    }
                }
                else
                {
                    if (ddlRole.SelectedValue == "V")
                    {
                        strVendorid = txtvendor.Text.ToString();
                    }
                    else if (ddlRole.SelectedValue == "I")//Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
                    {
                        strVendorid = ddlVendorname.SelectedValue;
                    }
                    else if (ddlRole.SelectedValue == "A")
                    {
                        foreach (ListItem lst in ddlVendorname.Items)
                        {
                            if (lst.Selected == true)
                            {
                                strVendorid += lst.Value + ",";
                            }

                        } 
                        //strVendorid = ddlVendorname.SelectedValue;
                    }
                    objBL.insert_USER_DETAILS(txtName.Text.Trim(), txtUserID.Text.Trim(), txtIMEINo.Text.Trim(), ddlDivision.SelectedValue, ddlRole.SelectedValue, ddlRole.SelectedValue,
                                                strVendorid, "BRPL", txtMobNo.Text, txtDesgnation.Text, txtIMEINo1.Text.Trim());

                    objBL.insert_LOGIN_MST(txtUserID.Text.Trim(), txtPassword.Text.Trim(), ddlRole.SelectedValue, txtName.Text.Trim(), ddlDivision.SelectedValue);

                    objBL.insert_Insallter_DETAILS(txtUserID.Text.Trim(), txtName.Text.Trim(), ddlDivision.SelectedValue, txtMobNo.Text,
                       strVendorid, txtDesgnation.Text, txtIMEINo.Text, txtIMEINo.Text);
                }

                SimpleMethods.MsgBoxWithLocation("User add successfully.", "AddUser.aspx", this);
            }
        }
    }

    private string Get_VendorID_DivWise(string _sDivisionID)
    {
        DataTable _dtVendor = new DataTable();
        _dtVendor = objBL.GetVendorID_DIVWise(_sDivisionID);

        if (_dtVendor.Rows.Count > 0)
            return _dtVendor.Rows[0][0].ToString();
        else
            return "";

    }

    private string Get_Vendor_DivWise(string _sDivisionID)//Added By Babalu Kumar 23122020
    {
        DataTable _dtVendordetails = new DataTable();
        _dtVendordetails = objBL.GetVendor_Divwise(_sDivisionID);

        if (_dtVendordetails.Rows.Count > 0)
            return _dtVendordetails.Rows[0][0].ToString();
        else
            return "";

    }

    public void BindVendor(string Div)
    {
        DataTable _dtVendorname = objBL.GetVendorDetails(Div);//Added By Babalu Kumar 23122020
        if (_dtVendorname.Rows.Count > 0)
        {
            ddlVendorname.DataSource = _dtVendorname;
            ddlVendorname.DataTextField = "VENDOR_NAME";
            ddlVendorname.DataValueField = "VENDOR_ID";
            ddlVendorname.DataBind();
            ddlVendorname.Items.Insert(0, new ListItem("-Select One-", "0"));
        }
    }

    public void Update_User(string strtype1)
    {
        txtPassword.Text = "12345678";
        if (Session["ROLE"].ToString() == "A")//Added By Babalu Kumar  28072020
        {
            if (ddlRole.SelectedValue == "V")
            {
                strVendorid = txtvendor.Text.ToString();
            }
            else if (ddlRole.SelectedValue == "I")
            {
                strVendorid = ddlVendorname.SelectedValue;
            }
            else if (ddlRole.SelectedValue == "A")
            {
                strVendorid = ddlVendorname.SelectedValue;
            }
            objBL.Update_USER_DETAILS(ddlActivation.SelectedValue, txtName.Text.Trim(), txtUserID.Text.Trim(), txtIMEINo.Text.Trim(), ddlDivision.SelectedValue,
                                        ddlRole.SelectedValue, ddlRole.SelectedValue, strVendorid, "BRPL", txtMobNo.Text, txtDesgnation.Text, txtIMEINo1.Text.Trim());

            objBL.update_LOGIN_MST(txtUserID.Text.Trim(), txtPassword.Text.Trim(), ddlRole.SelectedValue, ddlActivation.SelectedValue, ddlDivision.SelectedValue);//Change By Babalu Kumar 27052020

            if (strtype1 == "V")
            {
                objBL.update_Vendor_MST(txtName.Text.Trim(), txtUserID.Text.Trim(), ddlDivision.SelectedValue, txtMobNo.Text, txtDesgnation.Text, txtCompany.Text.Trim(), ddlActivation.SelectedValue, strVendorid);//Change By Babalu Kumar 27052020
                //objBL.Insert_MCR_V_D_OTYPE_PMACTMAP(strVendorid, ddlDivision.SelectedValue);//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                DataTable _dtcheck = objBL.Check_Vendor(ddlDivision.SelectedValue, strVendorid);
                if (_dtcheck.Rows.Count > 0)
                {
                    objBL.Insert_MCR_V_D_OTYPE_PMACTMAP(strVendorid, ddlDivision.SelectedValue);//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                }

            }
            else if (strtype1 == "I")
            {
                objBL.Update_Insallter_DETAILS(txtName.Text.Trim(), txtUserID.Text.Trim(), ddlDivision.SelectedValue, txtMobNo.Text, txtDesgnation.Text, "BRPL", ddlActivation.SelectedValue,
                ddlVendorname.SelectedValue, txtIMEINo.Text);
            }
        }
        else if (Session["ROLE"].ToString() == "R")//Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
        {

            if (ddlRole.SelectedValue == "V")
            {
                strVendorid = txtvendor.Text.ToString();
            }
            else if (ddlRole.SelectedValue == "I")
            {
                strVendorid = ddlVendorname.SelectedValue;
            }
            else if (ddlRole.SelectedValue == "A")
            {
                strVendorid = ddlVendorname.SelectedValue;
            }
            objBL.Update_USER_DETAILS(ddlActivation.SelectedValue, txtName.Text.Trim(), txtUserID.Text.Trim(), txtIMEINo.Text.Trim(), ddlDivision.SelectedValue,
                                       ddlRole.SelectedValue, ddlRole.SelectedValue, strVendorid, "BRPL", txtMobNo.Text, txtDesgnation.Text, txtIMEINo1.Text.Trim());

            objBL.update_LOGIN_MST(txtUserID.Text.Trim(), txtPassword.Text.Trim(), ddlRole.SelectedValue, ddlActivation.SelectedValue, ddlDivision.SelectedValue);//Change By Babalu Kumar 27052020

            if (strtype1 == "V")
            {
                objBL.update_Vendor_MST(txtName.Text.Trim(), txtUserID.Text.Trim(), ddlDivision.SelectedValue, txtMobNo.Text, txtDesgnation.Text, txtCompany.Text.Trim(), ddlActivation.SelectedValue, strVendorid);//Change By Babalu Kumar 27052020
                //objBL.Insert_MCR_V_D_OTYPE_PMACTMAP(strVendorid, ddlDivision.SelectedValue);//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                DataTable _dtcheck = objBL.Check_Vendor(ddlDivision.SelectedValue, strVendorid);
                if (_dtcheck.Rows.Count > 0)
                {
                    objBL.Insert_MCR_V_D_OTYPE_PMACTMAP(strVendorid, ddlDivision.SelectedValue);//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                }
            }
            else if (strtype1 == "I")
            {
                objBL.Update_Insallter_DETAILS(txtName.Text.Trim(), txtUserID.Text.Trim(), ddlDivision.SelectedValue, txtMobNo.Text, txtDesgnation.Text, "BRPL", ddlActivation.SelectedValue,
                ddlVendorname.SelectedValue, txtIMEINo.Text);
            }
        }
        SimpleMethods.MsgBoxWithLocation("Update successfully.", "AddUser.aspx", this);
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
    }

    protected void imgResetPass_Command(object sender, CommandEventArgs e)
    {
        string _sCode = e.CommandName.ToString();
        objBL.Reset_Password(_sCode);

        SimpleMethods.MsgBoxWithLocation("Password has been successfully Reset.", "AddUser.aspx", this);
    }

    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        getEmployeeDetails(ddlRole.SelectedValue, Session["Divison"].ToString(), Session["ROLE"].ToString());
        if (ddlRole.SelectedValue == "V")
        {
            trid.Visible = false;
            tabv.Visible = true;
        }
        else if (ddlRole.SelectedValue == "I")//Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
        {
            trid.Visible = true;
            tabv.Visible = false;
        }
        else if (ddlRole.SelectedValue == "A")
        {
            trid.Visible = true;
            tabv.Visible = false;
        }
        else
        {
            trid.Visible = false;
            tabv.Visible = false;
        }
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)//Added By Babalu Kumar  28122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        BindVendor(ddlDivision.SelectedValue);
        if (ddlRole.SelectedValue == "V")
        {
            trid.Visible = false;
            tabv.Visible = true;
        }
        else if (ddlRole.SelectedValue == "I")
        {
            trid.Visible = true;
            tabv.Visible = false;
        }
        else if (ddlRole.SelectedValue == "A")
        {
            trid.Visible = true;
            tabv.Visible = false;
        }
        else 
        {
            trid.Visible = false;
            tabv.Visible = false;
        }
    }
}