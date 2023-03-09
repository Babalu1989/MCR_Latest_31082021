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

public partial class Report_SchedularDetailsRpt : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                GetSchdularData();
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

    private void GetSchdularData()
    {
        DataTable _dtDetails = objBL.SehdularData1();
         if (_dtDetails.Rows.Count > 0)        
             lblMaxDateRpt1.Text = _dtDetails.Rows[0][0].ToString();
        else
             lblMaxDateRpt1.Text = "";

         _dtDetails = objBL.SehdularData2();
         if (_dtDetails.Rows.Count > 0)
             lblMaxDateRpt2.Text = _dtDetails.Rows[0][0].ToString();
         else
             lblMaxDateRpt2.Text = "";

         _dtDetails = objBL.SehdularData3();
         if (_dtDetails.Rows.Count > 0)
             lblMaxDateRpt3.Text = _dtDetails.Rows[0][0].ToString();
         else
             lblMaxDateRpt3.Text = "";

         _dtDetails = objBL.SehdularData4();
         if (_dtDetails.Rows.Count > 0)
             lblMaxDateRpt4.Text = _dtDetails.Rows[0][0].ToString();
         else
             lblMaxDateRpt4.Text = "";

         _dtDetails = objBL.SehdularData5();
         if (_dtDetails.Rows.Count > 0)
             lblMaxDateRpt5.Text = _dtDetails.Rows[0][0].ToString();
         else
             lblMaxDateRpt5.Text = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SchedularDetailsRpt.aspx");
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
    }
}