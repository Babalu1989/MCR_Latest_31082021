
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

public partial class _Default : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    Image sortImage = new Image();

    /// <summary>
    /// Developed by Gourav Gouton on Date 14.11.2017 guide given by Piyush Sir
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                getDataMCRPunchingForSeriesAllocation();
                btnSubmitSealAllocation.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSubmitSealAllocation, "") + ";this.value='Please wait....';this.disabled = true;");
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

    #region Series Wise Allocation
    public void getDataMCRPunchingForSeriesAllocation()
    {
        try
        {
            //DataTable _gdtEmpDetail = objBL.getEmpDetails(Convert.ToString(Session["UserName"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));
            DataTable _gdtEmpDetail = objBL.getEmpDetailsNew(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));

            if (_gdtEmpDetail.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _gdtEmpDetail;
                gvSeriesWiseAllocation.DataBind();
            }

            gvSeriesWiseAllocation.Columns[3].Visible = true;
            gvSeriesWiseAllocation.Columns[4].Visible = false;
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
            hdfValue.Value = (row.FindControl("lblPSeal") as Label).Text;

            DataTable _gdtDetails = objBL.getLoginDetails(txtEmployeeID.Text.Trim(), "");
            if (_gdtDetails.Rows.Count > 0)
            {
                hdfDivision.Value = _gdtDetails.Rows[0]["DIVISION"].ToString();
            }

            tr1.Visible = true;
            tr2.Visible = true;
            tr3.Visible = true;
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void txtSealFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtSealFrom.Text.Trim() != "")
            {
                //DataTable _dtCheck = objBL.getSealDetails(Convert.ToString(HttpContext.Current.Session["UserName"]), txtSealFrom.Text.Trim(), "", hdfDivision.Value);

                DataTable _dtCheck = new DataTable();

                if (RbdSealType.SelectedValue.ToString() == "P")
                    _dtCheck = objBL.getSealDetails(Convert.ToString(Session["VENDOR_ID"]), txtSealFrom.Text.Trim(), "", hdfDivision.Value);
                else
                    _dtCheck = objBL.getSeal_GunnyDetails(Convert.ToString(Session["VENDOR_ID"]), txtSealFrom.Text.Trim(), "", hdfDivision.Value);

                if (_dtCheck.Rows.Count > 0)
                {
                    if (_dtCheck.Rows[0]["CONSUM_SEAL_FLAG"].ToString() == "Y")
                    {
                        SimpleMethods.show("Seal " + txtSealFrom.Text.Trim() + " is already alloted.");
                        txtSealFrom.Text = "";
                        txtSealFrom.Focus();
                    }
                    else if (_dtCheck.Rows[0]["CONSUM_SEAL_FLAG"].ToString() == "C")
                    {
                        SimpleMethods.show("Seal " + txtSealFrom.Text.Trim() + " is already consumed.");
                        txtSealFrom.Text = "";
                        txtSealFrom.Focus();
                    }

                    lblNOAllotSeal.Text = "";
                    chboxListSealNO.Items.Clear();
                }
                else
                {
                    lblNOAllotSeal.Text = "";
                    chboxListSealNO.Items.Clear();

                    SimpleMethods.show("Seal " + txtSealFrom.Text.Trim() + " is not available for you.");
                    txtSealFrom.Text = "";
                    txtSealFrom.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    public void BindSealDetails()
    {
        try
        {
            chboxListSealNO.Items.Clear();

            // DataTable _dtSeal = objBL.getSeal_Detail_CheckBox(Convert.ToString(HttpContext.Current.Session["UserName"]), txtSealFrom.Text.Trim(), txtSealTo.Text.Trim(), hdfDivision.Value);
            DataTable _dtSeal = objBL.getSeal_Detail_CheckBox(Convert.ToString(Session["VENDOR_ID"]), txtSealFrom.Text.Trim(), txtSealTo.Text.Trim(), hdfDivision.Value);
            {
                for (int i = 0; i < _dtSeal.Rows.Count; i++)
                {
                    chboxListSealNO.Items.Add(_dtSeal.Rows[i]["Seal"].ToString());
                    chboxListSealNO.Items[i].Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void txtSealTo_TextChanged(object sender, EventArgs e)
    {
        string _gMessage = string.Empty;
        try
        {
            if (txtSealTo.Text.Trim() != "")
            {
                //DataTable _dtCheck = objBL.getSealDetails(Convert.ToString(HttpContext.Current.Session["UserName"]), txtSealTo.Text.Trim(), "", hdfDivision.Value);
                DataTable _dtCheck = new DataTable();

                if (RbdSealType.SelectedValue.ToString() == "P")
                    _dtCheck = objBL.getSealDetails(Convert.ToString(Session["VENDOR_ID"]), txtSealTo.Text.Trim(), "", hdfDivision.Value);
                else
                    _dtCheck = objBL.getSeal_GunnyDetails(Convert.ToString(Session["VENDOR_ID"]), txtSealTo.Text.Trim(), "", hdfDivision.Value);

                if (_dtCheck.Rows.Count > 0)
                {
                    if (_dtCheck.Rows[0]["CONSUM_SEAL_FLAG"].ToString() == "N")
                    {
                        DataTable _dtCount = objBL.getSealDetailsCount(Convert.ToString(Session["VENDOR_ID"]), txtSealFrom.Text.Trim(), txtSealTo.Text.Trim(), hdfDivision.Value);
                        if (_dtCount.Rows.Count > 0)
                        {
                            BindSealDetails();
                            _gMessage = "No. of seals  allocate = " + _dtCount.Rows[0][0].ToString();
                            _gMessage += "<br/>";
                            _gMessage += "Previos seals available = " + hdfValue.Value;
                            _gMessage += "<br/>";
                            int Count = Convert.ToInt32(hdfValue.Value) + Convert.ToInt32(_dtCount.Rows[0][0].ToString());
                            _gMessage += "Total No of seals available = " + Count;

                            lblNOAllotSeal.Text = _gMessage;
                            btnSubmitSealAllocation.Visible = true;
                            btnCancelSealAllocation.Visible = true;

                            trSealDetail.Visible = true;
                        }
                    }
                    else
                    {
                        if (_dtCheck.Rows[0]["CONSUM_SEAL_FLAG"].ToString() == "Y")
                        {
                            SimpleMethods.show("Seal " + txtSealTo.Text.Trim() + " is already alloted.");
                            txtSealTo.Text = "";
                            txtSealTo.Focus();
                        }
                        else if (_dtCheck.Rows[0]["CONSUM_SEAL_FLAG"].ToString() == "X")
                        {
                            SimpleMethods.show("Seal " + txtSealTo.Text.Trim() + " is already consumed.");
                            txtSealTo.Text = "";
                            txtSealTo.Focus();
                        }
                        else
                        {
                            SimpleMethods.show("Seal " + txtSealTo.Text.Trim() + " is not available for you.");
                            txtSealTo.Text = "";
                            txtSealTo.Focus();
                        }

                        lblNOAllotSeal.Text = "";
                        chboxListSealNO.Items.Clear();
                    }
                }
                else
                {
                    lblNOAllotSeal.Text = "";
                    chboxListSealNO.Items.Clear();

                    SimpleMethods.show("Seal " + txtSealTo.Text.Trim() + " is not available for you.");
                    txtSealTo.Text = "";
                    txtSealTo.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void btnCancelSealAllocation_Click(object sender, EventArgs e)
    {
        Response.Redirect("SealAllotment.aspx");
    }

    protected void btnSubmitSealAllocation_Click(object sender, EventArgs e)
    {
        try
        {
            if (chboxListSealNO.Items.Count > 0)
            {
                for (int i = 0; i < chboxListSealNO.Items.Count; i++)
                {
                    if (chboxListSealNO.Items[i].Selected == true)
                    {
                        objBL.Assign_Seal_Allocation(txtEmployeeID.Text.Trim(), "", chboxListSealNO.Items[i].ToString(), Convert.ToString(Session["UserName"]));
                    }

                    if ((i == chboxListSealNO.Items.Count - 1) && ((RbdSealType.SelectedValue.ToString() == "P")))
                        SimpleMethods.MsgBoxWithLocation("Seals has been alloted to " + txtName.Text.Trim(), "SealAllotment.aspx", this);
                    else
                        SimpleMethods.MsgBoxWithLocation("Gunny Seals has been alloted to " + txtName.Text.Trim(), "SealAllotment.aspx", this);
                }
            }
            else
            {
                SimpleMethods.show("Kindly Select Any Seal.");
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }
    #endregion

    public void bindLineChart()
    {
        try
        {
            DataTable _gdtEmpDetail = objBL.getEmpDetailsNew(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));
            if (_gdtEmpDetail.Rows.Count > 0)
            {
                for (int i = 3; i < _gdtEmpDetail.Columns.Count; i++)
                {
                    Series series = new Series();
                    foreach (DataRow dr in _gdtEmpDetail.Rows)
                    {
                        int y = Convert.ToInt32(dr[i].ToString());
                        series.Points.AddXY(dr["EmpName"].ToString(), y);
                    }
                    Chart_Meter.Series.Add(series);
                }

                lblChart.Text = "Seal Reconciliation";
            }
        }
        catch (Exception ex)
        {
        }
    }


    protected void chboxListSealNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        string _gMessage = "";
        int CountValue = 0;
        try
        {
            for (int i = 0; i < chboxListSealNO.Items.Count; i++)
            {
                if (chboxListSealNO.Items[i].Selected == true)
                    CountValue = CountValue + 1;
            }
            _gMessage = "No. of seals  allocate = " + CountValue;
            _gMessage += "<br/>";
            _gMessage += "Previos seals available = " + hdfValue.Value;
            _gMessage += "<br/>";
            int Count = Convert.ToInt32(hdfValue.Value) + Convert.ToInt32(CountValue);
            _gMessage += "Total No of seals available = " + Count;

            lblNOAllotSeal.Text = _gMessage;
            btnSubmitSealAllocation.Visible = true;
            btnCancelSealAllocation.Visible = true;
        }
        catch (Exception ex)
        {

        }
    }

    #region"Gunny Bag"
    protected void RbdSealType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RbdSealType.SelectedValue.ToString() == "P")
        {
            getDataMCRPunchingForSeriesAllocation();
            // Chart_Gunny.Visible = false;
            // Chart_Meter.Visible = true;
            lblChart.Text = "Gunny Seal Reconciliation";
        }
        else
        {
            getDataMCRPunchingForSeriesAllocation_GunnyBag();
            //Chart_Gunny.Visible = true;
            // Chart_Meter.Visible = false;
            lblChart.Text = "Gunny Seal Reconciliation";
        }
    }

    public void getDataMCRPunchingForSeriesAllocation_GunnyBag()
    {
        try
        {
            DataTable _gdtEmpDetail = objBL.getEmpDetailsNew_Gunny(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]),
                                                                          Convert.ToString(Session["COMPANY"]));

            if (_gdtEmpDetail.Rows.Count > 0)
            {
                gvSeriesWiseAllocation.DataSource = _gdtEmpDetail;
                gvSeriesWiseAllocation.DataBind();

                gvSeriesWiseAllocation.Columns[3].Visible = false;
                gvSeriesWiseAllocation.Columns[4].Visible = true;
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }
    #endregion
}