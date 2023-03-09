using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.OleDb;

public partial class frmInstallerOnMap : System.Web.UI.Page
{    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    #region lal long user details

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static UserLatLongDetails[] BindLatLongDetails(string strUserId, string strDate)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<UserLatLongDetails> details = new List<UserLatLongDetails>();
                
        dt = FindLatLongDetails(strUserId, strDate);

        foreach (DataRow dtrow in dt.Rows)
        {
            UserLatLongDetails userDetails = new UserLatLongDetails();

            userDetails.UserID = dtrow["USER_ID"].ToString();
            userDetails.Latitude = dtrow["LATITUDE"].ToString();
            userDetails.Longitude = dtrow["LONGITUDE"].ToString();
            userDetails.DateTime = dtrow["Date_Time"].ToString();

            if (userDetails.UserID != "")
                details.Add(userDetails);
        }
        return details.ToArray();
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static UserDetails[] BindUserDetails()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<UserDetails> details = new List<UserDetails>();

        dt = FindUserDetails();

        foreach (DataRow dtrow in dt.Rows)
        {
            UserDetails userDetails = new UserDetails();

            userDetails.UserID = dtrow["EMP_ID"].ToString();
            userDetails.UserName = dtrow["EMP_NAME"].ToString();

            if (userDetails.UserID != "")
                details.Add(userDetails);
        }
        return details.ToArray();
    }


    private static DataTable FindLatLongDetails(string strUserId, string strDate)
    {
        DataTable dt = new DataTable();
        List<DataColumn> listCols = new List<DataColumn>();

        NDS ndsCon = new NDS();
        OleDbConnection ocon = new OleDbConnection(ndsCon.DcrepCon());

        string sql = "SELECT  USER_ID, LATITUDE, LONGITUDE, TO_CHAR(ENTRY_DATE,'dd-Mon-yyyy hh:mm:ss AM') Date_Time ";
                sql = sql  +" FROM MCR_MAP_DTLS WHERE TO_CHAR(ENTRY_DATE) = TO_CHAR(SYSDATE) ";
                sql = sql  + " AND USER_ID = ? and latitude != '0.0' and longitude != '0.0' ";
                sql = sql  +" ORDER BY ENTRY_DATE DESC"; 
        try
        {
            if (ocon.State == ConnectionState.Closed)
            {
                ocon.Open();
            }
            OleDbCommand oleDbCommand = new OleDbCommand(sql, ocon);

            OleDbParameter USER_ID = oleDbCommand.Parameters.Add("@USER_ID", OleDbType.VarChar, 20);
            //OleDbParameter ENTRY_DATE = oleDbCommand.Parameters.Add("@ENTRY_DATE", OleDbType.VarChar, 16);

            USER_ID.Value = strUserId;
            //ENTRY_DATE.Value = strDate;

            OleDbDataReader rdr = oleDbCommand.ExecuteReader();

            DataTable dtSchema = rdr.GetSchemaTable();

            if (dtSchema != null)
            {
                foreach (DataRow drow in dtSchema.Rows)
                {
                    string columnName = System.Convert.ToString(drow["ColumnName"]);
                    DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                    column.Unique = (bool)drow["IsUnique"];
                    column.AllowDBNull = (bool)drow["AllowDBNull"];
                    column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                    listCols.Add(column);
                    dt.Columns.Add(column);
                }
            }

            while (rdr.Read())
            {
                string str = rdr.GetString(0);
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[((DataColumn)listCols[i])] = rdr[i];
                }
                dt.Rows.Add(dataRow);
            }

        }
        catch (Exception ex)
        {           
            //WriteIntoFile(DateTime.Now.ToString() + ex.ToString() + sql);
            return dt;
        }
        finally
        {
            if (ocon.State == ConnectionState.Open)
            {
                ocon.Close();
            }
        }
        return dt;
    }

    private static DataTable FindUserDetails()
    {
        DataTable dt = new DataTable();
        List<DataColumn> listCols = new List<DataColumn>();

        NDS ndsCon = new NDS();
        OleDbConnection ocon = new OleDbConnection(ndsCon.DcrepCon());

        string sql = "SELECT EMP_NAME, EMP_ID FROM MCR_USER_DETAILS WHERE ACTIVE_FLAG='Y'";
        try
        {
            if (ocon.State == ConnectionState.Closed)
            {
                ocon.Open();
            }
            OleDbCommand oleDbCommand = new OleDbCommand(sql, ocon);                        
            OleDbDataReader rdr = oleDbCommand.ExecuteReader();

            DataTable dtSchema = rdr.GetSchemaTable();

            if (dtSchema != null)
            {
                foreach (DataRow drow in dtSchema.Rows)
                {
                    string columnName = System.Convert.ToString(drow["ColumnName"]);
                    DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                    column.Unique = (bool)drow["IsUnique"];
                    column.AllowDBNull = (bool)drow["AllowDBNull"];
                    column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                    listCols.Add(column);
                    dt.Columns.Add(column);
                }
            }

            while (rdr.Read())
            {
                string str = rdr.GetString(0);
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[((DataColumn)listCols[i])] = rdr[i];
                }
                dt.Rows.Add(dataRow);
            }

        }
        catch (Exception ex)
        {
            //WriteIntoFile(DateTime.Now.ToString() + ex.ToString() + sql);
            return dt;
        }
        finally
        {
            if (ocon.State == ConnectionState.Open)
            {
                ocon.Close();
            }
        }
        return dt;
    }


    public class UserLatLongDetails
    {
        public string UserID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DateTime { get; set; }
    }

    public class UserDetails
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
    }
    #endregion

}

