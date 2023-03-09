using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RnDPage : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();

    protected void Page_Load(object sender, EventArgs e)
    {
        Button1.Attributes["onclick"] = "this.disabled=true;this.value='Please wait..';" + Page.GetPostBackEventReference(Button1).ToString();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        lblResult.Text = "";
        Label1.Text = "";
        if (txtpass.Text == "dxo191")
        {
            if (txtQueries.Text != "")
            {
                lblResult.Text = (objBL.InsertBySQL(txtQueries.Text.Trim())).ToString();
            }
        }
        else
        {
            Label1.Text = "You damm don't use this page...'";
        }

        //for (int i = 0; i < 1000000000; i++)
        //{

        //}
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
}