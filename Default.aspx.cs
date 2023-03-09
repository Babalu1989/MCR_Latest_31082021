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
            if (Session["LOGIN_TYPE"] != null)
                Response.Redirect("HomePage.aspx");
        }
    }

    public string convertDivisionName(string DivisionName)
    {
        string result = string.Empty;
        if (DivisionName != "")
        {
            result = DivisionName.Replace(",", "','");
        }
        return result;
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUserName.Text.Trim() != "UserID" && txtPassword.Text.Trim() != "Password")
            {
                DataTable _gdtDetails = objBL.getLoginDetails(txtUserName.Text.Trim(), txtPassword.Text.Trim());

                if (_gdtDetails.Rows.Count > 0)
                {
                    if (_gdtDetails.Rows[0][0].ToString() == "-1")
                    {
                        SimpleMethods.show("Unable to Connect with Database Server. Please Try Again Later...");
                        return;
                    }
                    else
                    {
                        Session["UserName"] = Convert.ToString(_gdtDetails.Rows[0]["LOGIN_ID"]);
                        Session["EMP_NAME"] = Convert.ToString(_gdtDetails.Rows[0]["EMP_NAME"]);
                        Session["COMPANY"] = Convert.ToString(_gdtDetails.Rows[0]["COMPANY"]);
                        Session["ROLE"] = Convert.ToString(_gdtDetails.Rows[0]["ROLE"]);
                        if (_gdtDetails.Rows[0]["VENDOR_ID"].ToString() != "")
                            Session["VENDOR_ID"] = Convert.ToString(_gdtDetails.Rows[0]["VENDOR_ID"]);
                        else
                            Session["VENDOR_ID"] = "";
                        Session["Divison"] = convertDivisionName(Convert.ToString(_gdtDetails.Rows[0]["division"]));
                        Session["LOGIN_TYPE"] = Convert.ToString(_gdtDetails.Rows[0]["LOGIN_TYPE"]);
                        Response.Redirect("HomePage.aspx");
                    }
                }
                else
                   SimpleMethods.show("Invalid User ID & Password.");           
            }
            else
                SimpleMethods.show("Kindly Enter User Name or Password.");
            return;
        }
        catch (Exception ex)
        {
            SimpleMethods.show(ex.Message.ToString());
        }
    }
}