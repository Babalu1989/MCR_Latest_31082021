using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SimpleTest;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void btnLogin_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtUserName.Text.Trim() != "" && txtPassword.Text.Trim() != "" && txtNPassword.Text.Trim()!="")
            {
                DataTable _gdtDetails = objBL.getLoginDetails(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                if (_gdtDetails.Rows.Count > 0)
                {
                    int UpdateData = objBL.UpdatePassword(txtUserName.Text.Trim(), txtPassword.Text.Trim(), txtNPassword.Text.Trim());//Added by Babalu Kumar 28072020 Req. No. REQ28052020419205 And PCN No. 2606260603 for  linked with that IMEI
                    if (UpdateData == 1)
                    {
                        SimpleMethods.MsgBoxWithLocation("Password has been Changed Successfully.", "Default.aspx", this);
                    }                    
                }
                else
                    SimpleMethods.show("Kindly Enter Correct User Name or Password.");
            }
            else
                SimpleMethods.show("Kindly Enter User Name or Password.");
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}