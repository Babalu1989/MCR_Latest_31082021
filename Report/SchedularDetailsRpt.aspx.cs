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
    Image sortImage = new Image();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                getMaineData();               
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
    

    public void getMaineData()
    {
        DataTable _dtDetails = objBL.SehdularData();
        string _sSchType = string.Empty;
        int _iCount = 0;

        if (_dtDetails.Rows.Count > 0)
        {
            //btnExcel.Visible = true;
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();

            foreach (GridViewRow row in gvMainData.Rows)
            {
                LinkButton lnkCount = (LinkButton)row.FindControl("lnkSchCount");

                row.Cells[0].Text = (_iCount + 1).ToString();
                _sSchType = (_iCount + 1).ToString();
                lnkCount.Text= GetSchdularCount_TypeWise(_sSchType);
                _iCount++;
            }
        }
        else
        {
            btnExcel.Visible = false;
            gvMainData.DataSource = null;
            gvMainData.DataBind();                    
            imgBtnExcel.Visible = false;            
        }
    }

    private string GetSchdularCount_TypeWise(string _sType)
    {
        DataTable _dtSch = new DataTable();

        if (_sType == "1")
            _dtSch = objBL.SehdularDataCount1();
        else if (_sType == "2")
            _dtSch = objBL.SehdularDataCount2();
        else if (_sType == "3")
            _dtSch = objBL.SehdularDataCount3();
        else if (_sType == "4")
            _dtSch = objBL.SehdularDataCount4();
        else if (_sType == "5")
            _dtSch = objBL.SehdularDataCount5();

        if (_dtSch.Rows.Count > 0)
            return _dtSch.Rows[0][0].ToString();
        else
            return "0";
    }

    protected void gvMainData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataTable _dtDetails = new DataTable();                    
            
            if (e.CommandArgument.ToString() == "Allocated Cases to Installer")
            {
                lblReportHead.Text = "Allocated Cases to Installer Details Data";
                _dtDetails = objBL.SehdularDataCount1_Details();

                if (_dtDetails.Rows.Count > 0)
                {
                    //imgBtnExcel.Visible = true;
                    gvInputData.DataSource = _dtDetails;
                    gvInputData.DataBind();
                    if (ViewState["Details"] != null)
                    {
                        ViewState["Details"] = null;
                    }
                    ViewState["Details"] = _dtDetails;

                    gvInputData.Visible = true;                    
                    gvSealDetails.Visible = false;
                }
                else
                {
                    imgBtnExcel.Visible = false;
                    gvInputData.DataSource = null;
                    gvInputData.DataBind();
                    ViewState["Details"] = null;

                    SimpleMethods.show("No Data Found.");
                }
            }
            else if (e.CommandArgument.ToString() == "Kitting Details")
            {
                lblReportHead.Text = "Kitting Details Data";
                _dtDetails = objBL.SehdularDataCount2_Details();

                if (_dtDetails.Rows.Count > 0)
                {
                    //imgBtnExcel.Visible = true;
                    gvSealDetails.DataSource = _dtDetails;
                    gvSealDetails.DataBind();
                    if (ViewState["Details"] != null)
                    {
                        ViewState["Details"] = null;
                    }
                    ViewState["Details"] = _dtDetails;

                    gvInputData.Visible = false;                    
                    gvSealDetails.Visible = true;
                }
                else
                {
                    imgBtnExcel.Visible = false;
                    gvSealDetails.DataSource = null;
                    gvSealDetails.DataBind();
                    ViewState["Details"] = null;

                    SimpleMethods.show("No Data Found.");
                }
            }
            else if (e.CommandArgument.ToString() == "Punch Details")
            {
                lblReportHead.Text = "Punch Details Data";
                _dtDetails = objBL.SehdularDataCount3_Details();

                if (_dtDetails.Rows.Count > 0)
                {
                    //imgBtnExcel.Visible = true;
                    gvInputData.DataSource = _dtDetails;
                    gvInputData.DataBind();
                    if (ViewState["Details"] != null)
                    {
                        ViewState["Details"] = null;
                    }
                    ViewState["Details"] = _dtDetails;

                    gvInputData.Visible = true;                    
                    gvSealDetails.Visible = false;
                }
                else
                {
                    imgBtnExcel.Visible = false;
                    gvInputData.DataSource = null;
                    gvInputData.DataBind();
                    ViewState["Details"] = null;

                    SimpleMethods.show("No Data Found.");
                }
            }
            else if (e.CommandArgument.ToString() == "Punched Cases through TAB")
            {
                lblReportHead.Text = "Punched Cases through TAB Details Data";
                _dtDetails = objBL.SehdularDataCount4_Details();

                if (_dtDetails.Rows.Count > 0)
                {
                    //imgBtnExcel.Visible = true;
                    gvInputData.DataSource = _dtDetails;
                    gvInputData.DataBind();
                    if (ViewState["Details"] != null)
                    {
                        ViewState["Details"] = null;
                    }
                    ViewState["Details"] = _dtDetails;

                    gvInputData.Visible = true;                    
                    gvSealDetails.Visible = false;
                }
                else
                {
                    imgBtnExcel.Visible = false;
                    gvInputData.DataSource = null;
                    gvInputData.DataBind();
                    ViewState["Details"] = null;

                    SimpleMethods.show("No Data Found.");
                }
            }
            else
            {
                lblReportHead.Text = "Seal Details Data";
                _dtDetails = objBL.SehdularDataCount5_Details();

                if (_dtDetails.Rows.Count > 0)
                {
                    //imgBtnExcel.Visible = true;
                    gvInputData.DataSource = _dtDetails;
                    gvInputData.DataBind();
                    if (ViewState["Details"] != null)
                    {
                        ViewState["Details"] = null;
                    }
                    ViewState["Details"] = _dtDetails;

                    gvInputData.Visible = true;                    
                    gvSealDetails.Visible = false;
                }
                else
                {
                    imgBtnExcel.Visible = false;
                    gvInputData.DataSource = null;
                    gvInputData.DataBind();
                    ViewState["Details"] = null;

                    SimpleMethods.show("No Data Found.");
                }
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    //private void DetailsData_Format()
    //{
    //    for (int i = 0; i < gvDetails.Rows.Count; i++)
    //    {
    //        gvDetails.Rows[i].Cells[4].Text = gvDetails.Rows[i].Cells[4].Text.TrimStart('0');
    //    }
    //}
    
    protected void gvInputData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Details"];
        SetSortDirection(SortDireaction);

        if (_dtDetails != null)
        {
            //Sort the data.
            _dtDetails.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            gvInputData.DataSource = _dtDetails;
            gvInputData.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvInputData.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvInputData.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            gvInputData.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            //DetailsData_Format();
        }
    }

    protected void gvSealDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Details"];
        SetSortDirection(SortDireaction);
        if (_dtDetails != null)
        {
            //Sort the data.
            _dtDetails.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            gvSealDetails.DataSource = _dtDetails;
            gvSealDetails.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvSealDetails.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvSealDetails.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            gvSealDetails.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);            
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


    protected void btnExcel_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void imgBtnExcel_Click(object sender, ImageClickEventArgs e)
    {
       //SELECT DIV.DIVISION_NAME, COUNT(1)
 	   // FROM MCR_INPUT_DETAILS DT,MCR_DIVISION DIV WHERE DT.ENTRY_DATE= (SELECT MAX(ENTRY_DATE) FROM MCR_INPUT_DETAILS)
	   // AND DT.DIVISION=DIV.DIST_CD GROUP BY DIV.DIVISION_NAME ORDER BY DIV.DIVISION_NAME
    }

    protected void BtnSyncData_Click(object sender, EventArgs e)
    {        
        objBL.SchMCR_Data(ddlCircle.SelectedValue, txtFrmDate.Text);       
    }
}