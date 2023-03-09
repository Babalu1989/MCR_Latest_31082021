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

    /// <summary>
    /// Developed by Gourav Gouton on Date 14.11.2017 guide given by Swati Kaushik
    /// Developed for Add and Modified Users
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                getRoleDetails();
                getData(); 
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

    public void getRoleDetails()
    {
        DataTable Dt = objBL.getRoleMstDetails("Y");
        if (Dt.Rows.Count > 0)
        {
            ddlRoleID.DataSource = Dt;
            ddlRoleID.DataTextField = "role_name";
            ddlRoleID.DataValueField = "Role_id";
            ddlRoleID.DataBind();
        }
    }

    public void getData()
    {
        DataTable _dtData = objBL.getRoleRightMst();
        if (_dtData.Rows.Count > 0)
        {
            gvMainData.DataSource = _dtData;
            gvMainData.DataBind();
        }
    }

    protected void ddlRoleID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedValue == "0")
        {
            ddlCompany.BackColor = System.Drawing.Color.Yellow;
            ddlCompany.Focus();
            SimpleMethods.show("Please Select Company Name.");
            return;
        }

        if (ddlRoleID.SelectedItem.Text != "--Select One--")
        {
            DataTable _dtDDTable = new DataTable();
            _dtDDTable = objBL.GetRoleRightData_RoleWise(ddlRoleID.SelectedValue, ddlCompany.SelectedValue);

            if (_dtDDTable.Rows.Count > 0)
            {
                getData();

                for (int i = 0; i < _dtDDTable.Rows.Count; i++)
                {
                    for (int j = 0; j < gvMainData.Rows.Count; j++)
                    {
                        Label lblFormID = (Label)gvMainData.Rows[j].FindControl("lblFormCode");
                        if (lblFormID.Text == _dtDDTable.Rows[i]["USER_MODULE_CODE"].ToString())
                        {
                            RadioButton rdbtnAllow = (RadioButton)gvMainData.Rows[j].FindControl("rdbtnAllow");
                            RadioButton rdbtnNone = (RadioButton)gvMainData.Rows[j].FindControl("rdbtnNone");
                            if (_dtDDTable.Rows[i]["ROLE_AMQ"].ToString() == "Y")
                            {
                                gvMainData.Rows[j].Cells[2].BackColor = System.Drawing.Color.SkyBlue;
                                rdbtnAllow.Checked = true;
                                rdbtnNone.Checked = false;
                            }
                            if (_dtDDTable.Rows[i]["ROLE_AMQ"].ToString() == "N")
                            {
                                gvMainData.Rows[j].Cells[3].BackColor = System.Drawing.Color.SkyBlue;
                                rdbtnAllow.Checked = false;
                                rdbtnNone.Checked = true;
                            }
                            continue;
                        }
                    }
                }
            }
            else
            {
                getData();
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlRoleID.SelectedItem.Text != "--Select One--")
            {
                DeleteRoleModule(ddlRoleID.SelectedValue, ddlCompany.SelectedValue);
                InsertRolMod_Details();

                SimpleMethods.show("Role Right Has Been Successfully Updated.");

                ddlRoleID.SelectedIndex = -1;
                ddlCompany.SelectedIndex = -1;
                getData();
            }
            else
                SimpleMethods.show("Kindly Select Company & Role Name.");
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again");
        }
    }

    private void InsertRolMod_Details()
    {
        for (int i = 0; i < gvMainData.Rows.Count; i++)
        {
            string _sRdbRight = string.Empty;
            RadioButton rdbtnAllow = (RadioButton)gvMainData.Rows[i].FindControl("rdbtnAllow");
            RadioButton rdbtnNone = (RadioButton)gvMainData.Rows[i].FindControl("rdbtnNone");

            if (rdbtnAllow.Checked == true)
                _sRdbRight = "Y";
            else
                _sRdbRight = "N";

            if (_sRdbRight != "")
            {
                Label lblFormID = (Label)gvMainData.Rows[i].FindControl("lblFormCode");
                objBL.Insert_RoleAssign(ddlRoleID.SelectedValue, lblFormID.Text, _sRdbRight, ddlCompany.SelectedValue);
            }
        }


    }
    
    private int DeleteRoleModule(string _sRoleID, string _sComapany)
    {
        return objBL.DeleteRole_RightAssinMst(_sRoleID, _sComapany);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
    }

    
}