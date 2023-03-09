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

public partial class OrderTypePMActMgm : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    Image sortImage = new Image();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {               
                BindOrderType();
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

    public void getData(string _sOrdType, string _sActType)
    {
        DataTable _dtData = objBL.GetOrderType_PMActivityData(_sOrdType, _sActType);
        if (_dtData.Rows.Count > 0)
        {
            gvMainData.DataSource = _dtData;
            gvMainData.DataBind();

            GetDataFormate();
        }
    }

    public void BindOrderType()
    {
        DataTable _dtBindName = objBL.getOrderTypeDetails(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"])); //16032018
        if (_dtBindName.Rows.Count > 0)
        {
            ddlOrderType.DataSource = _dtBindName;
            ddlOrderType.DataTextField = "ORDER_DESCRIPTION";
            ddlOrderType.DataValueField = "ORDER_TYPE";
            ddlOrderType.DataBind();
            ddlOrderType.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
    }

    private void GetDataFormate()
    {
        for (int j = 0; j < gvMainData.Rows.Count; j++)
        {
            Label lblActiveFlag = (Label)gvMainData.Rows[j].FindControl("lblActive");
            Label lblSAPFLAG = (Label)gvMainData.Rows[j].FindControl("lblSAP_FLAG");

            RadioButton rdbtnAllow = (RadioButton)gvMainData.Rows[j].FindControl("rdbtnAllow");
            RadioButton rdbtnNone = (RadioButton)gvMainData.Rows[j].FindControl("rdbtnNone");

            if (lblActiveFlag.Text.ToString() == "Y")
            {
                gvMainData.Rows[j].Cells[5].BackColor = System.Drawing.Color.Green;
                rdbtnAllow.Checked = true;
                rdbtnNone.Checked = false;
            }
            if (lblActiveFlag.Text.ToString() == "N")
            {
                gvMainData.Rows[j].Cells[6].BackColor = System.Drawing.Color.LightGray;
                rdbtnAllow.Checked = false;
                rdbtnNone.Checked = true;
            }

            RadioButton rdbtnSAPAllow = (RadioButton)gvMainData.Rows[j].FindControl("rdbtnSAPAllow");
            RadioButton rdbtnSAPNone = (RadioButton)gvMainData.Rows[j].FindControl("rdbtnSAPNone");

            if (lblSAPFLAG.Text.ToString() == "Y")
            {
                gvMainData.Rows[j].Cells[7].BackColor = System.Drawing.Color.Green;
                rdbtnSAPAllow.Checked = true;
                rdbtnSAPNone.Checked = false;
            }
            if (lblSAPFLAG.Text.ToString() == "N")
            {
                gvMainData.Rows[j].Cells[8].BackColor = System.Drawing.Color.LightGray;
                rdbtnSAPAllow.Checked = false;
                rdbtnSAPNone.Checked = true;
            }

        }
    }

    protected void imgUpdate_Command(object sender, CommandEventArgs e)
    {
        objBL.Update_OrderType_PMActivity( "", "",e.CommandArgument.ToString(), e.CommandName.ToString());
        //getData();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("HomePage.aspx");
    }
    protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrderType.SelectedItem.Text != "-ALL-")
        {
            DataTable _dtBindName = objBL.getPM_Activity_OrderWise(ddlOrderType.SelectedValue, Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"])); //16032018
            if (_dtBindName.Rows.Count > 0)
            {
                ddlPMActivity.DataSource = _dtBindName;
                ddlPMActivity.DataTextField = "PM_DESCRIPTION";
                ddlPMActivity.DataValueField = "PM_ACTIVTY";
                ddlPMActivity.DataBind();
                ddlPMActivity.Items.Insert(0, new ListItem("-ALL-", "0"));
            }
        }
        else
        {
            ddlPMActivity.Items.Clear();
            ddlPMActivity.Items.Insert(0, new ListItem("-ALL-", "0"));
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string _sOrdType = string.Empty, _sActType = string.Empty;

        if (ddlOrderType.SelectedItem.Text != "-ALL-")
            _sOrdType = ddlOrderType.SelectedValue;
        else
            _sOrdType = "";

        if (ddlPMActivity.SelectedItem.Text != "-ALL-")
            _sActType = ddlPMActivity.SelectedValue;
        else
            _sActType = "";

        getData(_sOrdType, _sActType);
    }
}