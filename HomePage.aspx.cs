using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SimpleTest;
using System.Data;
using System.Text;
using System.Web.UI.DataVisualization.Charting;

public partial class _Default : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                //if (Session["ROLE"].ToString() == "V")
                  //  BindChart();

                //bindBarChart_Meter();
                //bindBarChart_Seal();

                lblCompany.Text = Session["COMPANY"].ToString();
                lblEmpCode.Text = Session["UserName"].ToString();
                lblName.Text = Session["EMP_NAME"].ToString();
                GetDivisionName();

                GetRole_Description();

                if (Session["ROLE"].ToString() == "S")
                    TRDiv_Row.Visible = false;
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


    private void GetDivisionName()
    {
        DataTable _dtDiv= new DataTable();
        _dtDiv= objBL.getDivisionDetails(Session["Divison"].ToString());

        if(_dtDiv.Rows.Count>0)
         lblDivision.Text = _dtDiv.Rows[0][1].ToString();
        else
          lblDivision.Text = Session["Divison"].ToString();
    }

    private void GetRole_Description()
    {
        DataTable _dtRole = new DataTable();
        _dtRole = objBL.getRoleRightName_IDWise(Session["LOGIN_TYPE"].ToString());

        if (_dtRole.Rows.Count > 0)
        {           
                lblEmployeeType.Text = _dtRole.Rows[0][0].ToString();
        }
    }


    public void bindBarChart_Meter()
    {
        StringBuilder strScript = new StringBuilder();
        DataTable _dtDetails = objBL.getMeterReconciliation_graph("", "", Session["Divison"].ToString(), "", Session["COMPANY"].ToString(), Session["UserName"].ToString());
        if (_dtDetails.Rows.Count > 0)
        {
            try
            {
                strScript.Append(@"<script type='text/javascript'>
                    google.load('visualization', '1', {packages: ['corechart']});</script>

                    <script type='text/javascript'>
                    function drawVisualization() {       
                    var data = google.visualization.arrayToDataTable([
                    ['VENDER_NAME', 'Alloted_Order', 'Installed_Order', 'Pending_order_at_Installer', 'Cancel_Order_From_Field', 'Not_Assigned_Order'],");

                foreach (DataRow row in _dtDetails.Rows)
                {
                    strScript.Append("['" + row["VENDER_NAME"] + "'," + row["Alloted_Order"] + "," +
                        row["Installed_Order"] + "," + row["Pending_order_at_Installer"] + "," + row["Cancel_Order_From_Field"] + "," + row["Not_Assigned_Order"] + "],");
                }

                strScript.Remove(strScript.Length - 1, 1);
                strScript.Append("]);");

                strScript.Append("var options = { title : 'Meter Details', vAxis: {title: 'Count'},  hAxis: {title: 'Meter Reconciliation'}, seriesType: 'bars', series: {2: {type: 'area'}} };");
                strScript.Append(" var chart = new google.visualization.ComboChart(document.getElementById('chart_Meter'));  chart.draw(data, options); } google.setOnLoadCallback(drawVisualization);");
                strScript.Append(" </script>");

                ltScripts_meter.Text = strScript.ToString();
            }
            catch
            {
            }
            finally
            {

            }
        }
    }

    public void bindBarChart_Seal()
    {
        StringBuilder strScript = new StringBuilder();
        DataTable _dtDetails = objBL.getSealReconciliation_Graph("", "", Session["Divison"].ToString(), "", Session["COMPANY"].ToString(), Session["UserName"].ToString());
        if (_dtDetails.Rows.Count > 0)
        {
            for (int i = 2; i < _dtDetails.Columns.Count; i++)
            {
                strScript.Append(@"<script type='text/javascript'>
                    google.load('visualization', '1', {packages: ['corechart']});</script>

                    <script type='text/javascript'>
                    function drawVisualization() {       
                    var data = google.visualization.arrayToDataTable([
                    ['VENDER_NAME', 'Alloted_Seals', 'Installed_Seals', 'Pending_Seals_at_Installer', 'SEALNOTASSIGN'],");

                foreach (DataRow row in _dtDetails.Rows)
                {
                    strScript.Append("['" + row["VENDER_NAME"] + "'," + row["Alloted_Seals"] + "," +
                        row["Installed_Seals"] + "," + row["Pending_Seals_at_Installer"] + "," + row["Not_Assigned_Seals"] + "],");
                }

                strScript.Remove(strScript.Length - 1, 1);
                strScript.Append("]);");

                strScript.Append("var options = { title : 'Seal Details', vAxis: {title: 'Count'},  hAxis: {title: 'Seal Reconciliation'}, seriesType: 'bars', series: {2: {type: 'area'}} };");
                strScript.Append(" var chart = new google.visualization.ComboChart(document.getElementById('chart_Seal'));  chart.draw(data, options); } google.setOnLoadCallback(drawVisualization);");
                strScript.Append(" </script>");

                ltScripts_Seal.Text = strScript.ToString();
            }
        }
    }

    private void BindChart()
    {
        StringBuilder strScript = new StringBuilder();

        DataTable _gdtEmpDetail = objBL.getEmpDetails(Convert.ToString(Session["UserName"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["COMPANY"]));

        if (_gdtEmpDetail != null)
        {
            try
            {
                strScript.Append(@"<script type='text/javascript'>
                    google.load('visualization', '1', {packages: ['corechart']});</script>

                    <script type='text/javascript'>
                    function drawVisualization() {       
                    var data = google.visualization.arrayToDataTable([
                    ['EMPNAME', 'MeterAlloted', 'SealAlloted'],");

                foreach (DataRow row in _gdtEmpDetail.Rows)
                {
                    strScript.Append("['" + row["EMPNAME"] + "'," + row["MeterAlloted"] + "," +
                        row["SealAlloted"] + "],");
                }

                strScript.Remove(strScript.Length - 1, 1);
                strScript.Append("]);");

                strScript.Append("var options = { title : 'Installer wise Allotements Details', vAxis: {title: 'Count'},  hAxis: {title: 'Installer Name'}, seriesType: 'bars', series: {2: {type: 'area'}} };");
                strScript.Append(" var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));  chart.draw(data, options); } google.setOnLoadCallback(drawVisualization);");
                strScript.Append(" </script>");

                ltScripts.Text = strScript.ToString();
            }
            catch
            {
            }
            finally
            {
                _gdtEmpDetail.Dispose();
                strScript.Clear();
            }
        }
    }

    protected void LnkGrapgh_Click(object sender, EventArgs e)
    {
        if (Session["ROLE"].ToString() == "V")
          BindChart();

        bindBarChart_Meter();
        bindBarChart_Seal();
    }
}