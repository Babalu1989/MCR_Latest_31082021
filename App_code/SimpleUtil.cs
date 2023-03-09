using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.OleDb;


namespace SimpleTest
{
    /// <summary>
    /// Summary description for SimpleTest
    /// </summary>
    public  class SimpleUtil
    {
        private static SimpleUtil instance;
        int result = 0;
        
            
        private SimpleUtil()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static SimpleUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SimpleUtil();
                }
                return instance;
            }

        }

        private static string getConnectionString()
        {
            NDS ObjNDS = new NDS();
            return ObjNDS.DcrepCon();
        }

        private static string getConnectionStringMIS()
        {
            NDS ObjNDS = new NDS();
            return ObjNDS.MISCon();
        }
        

        public  DataTable ExecuteReader(string sqlQuery)
        {
            DataTable dt = new DataTable();
            OleDbConnection OleDbConnection = new OleDbConnection();
            OleDbCommand OleDbCommand = new OleDbCommand();
            try
            {
                OleDbConnection.ConnectionString = getConnectionString();
                OleDbConnection.Open();
                OleDbCommand.Connection = OleDbConnection;
                OleDbCommand.CommandText = sqlQuery;
                OleDbDataAdapter da = new OleDbDataAdapter(OleDbCommand);
                da.Fill(dt);
            }
            catch (OleDbException oraExp)
            {
                dt.TableName = "OutPutTable";
                dt.Columns.Add("ExData");
                DataRow dr = dt.NewRow();
                dt.Rows.Add("-1");
                dt.AcceptChanges();

                Logger.LogMessageToFile(oraExp.Message, oraExp.StackTrace);
            }
            finally
            {
                if (OleDbConnection != null)
                {
                    OleDbConnection.Close();
                    OleDbConnection.Dispose();
                }
            }
            return dt;
        }

        public DataTable ExecuteReaderMIS(string sqlQuery)
        {
            DataTable dt = new DataTable();
            OleDbConnection OleDbConnection = new OleDbConnection();
            OleDbCommand OleDbCommand = new OleDbCommand();
            try
            {
                OleDbConnection.ConnectionString = getConnectionStringMIS();
                OleDbConnection.Open();
                OleDbCommand.Connection = OleDbConnection;
                OleDbCommand.CommandText = sqlQuery;
                OleDbDataAdapter da = new OleDbDataAdapter(OleDbCommand);
                da.Fill(dt);
            }
            catch (OleDbException oraExp)
            {
                dt.TableName = "OutPutTable";
                dt.Columns.Add("ExData");
                DataRow dr = dt.NewRow();
                dt.Rows.Add("-1");
                dt.AcceptChanges();

                Logger.LogMessageToFile(oraExp.Message, oraExp.StackTrace);
            }
            finally
            {
                if (OleDbConnection != null)
                {
                    OleDbConnection.Close();
                    OleDbConnection.Dispose();
                }
            }
            return dt;
        }

        public int ExecuteScalar(string sqlQuery)
        {
            result = 0;
            OleDbConnection OleDbConnection = new OleDbConnection();
            OleDbCommand OleDbCommand = new OleDbCommand();
            try
            {
                OleDbConnection.ConnectionString = getConnectionString();
                OleDbConnection.Open();
                OleDbCommand.Connection = OleDbConnection;
                OleDbCommand.CommandText = sqlQuery;
                result = Convert.ToInt32(OleDbCommand.ExecuteScalar());                
            }
            catch (OleDbException oraExp)
            {
                result = -1;
                Logger.LogMessageToFile(oraExp.Message, oraExp.StackTrace);
            }
            finally
            {
                if (OleDbConnection != null)
                {
                    OleDbConnection.Close();
                    OleDbConnection.Dispose();
                }
            }
            return result;
        }

        public int ExecuteNonQuery(string sqlQuery)
        {
            result = 0;
            OleDbConnection OleDbConnection = new OleDbConnection();
            OleDbCommand OleDbCommand = new OleDbCommand();
            OleDbTransaction ot = null;

            try
            {
                OleDbConnection.ConnectionString = getConnectionString();
                OleDbConnection.Open();
                ot = OleDbConnection.BeginTransaction();
                OleDbCommand.Connection = OleDbConnection;
                OleDbCommand.CommandText = sqlQuery;
                OleDbCommand.Transaction = ot;
                result = OleDbCommand.ExecuteNonQuery();
                ot.Commit();
            }
            catch (OleDbException oraExp)
            {
                result = -1;
                ot.Rollback();
                Logger.LogMessageToFile(oraExp.Message, oraExp.StackTrace);
            }
            finally
            {
                if (OleDbConnection != null)
                {
                    OleDbConnection.Close();
                   OleDbConnection.Dispose();
                }
            }
            return result;
        }

        public int ExecuteNonQuerywithCMD(OleDbCommand OleDbCommand)
        {
            result = 0;
            OleDbConnection OleDbConnection = new OleDbConnection();
            OleDbTransaction ot = null;
            try
            {
                OleDbConnection.ConnectionString = getConnectionString();
                OleDbConnection.Open();
                ot = OleDbConnection.BeginTransaction();
                OleDbCommand.Connection = OleDbConnection;
                OleDbCommand.Transaction = ot;
                result = OleDbCommand.ExecuteNonQuery();
                ot.Commit();
            }
            catch (OleDbException oraExp)
            {
                result = -1;
                ot.Rollback();
                Logger.LogMessageToFile(oraExp.Message, oraExp.StackTrace);
            }
            finally
            {             
                if (OleDbConnection != null)
                {
                    OleDbConnection.Close();
                    OleDbConnection.Dispose();
                }
            }
            return result;
        }

        public List<string> ExecuteListReader(string sqlQuery)
        {
            List<string> list = new List<string>();
            OleDbConnection OleDbConnection = new OleDbConnection();
            OleDbCommand OleDbCommand = new OleDbCommand();
            try
            {
                OleDbConnection.ConnectionString = getConnectionString();
                OleDbConnection.Open();
                OleDbCommand.Connection = OleDbConnection;
                OleDbCommand.CommandText = sqlQuery;
                OleDbDataReader dr=OleDbCommand.ExecuteReader();
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        list.Add(dr[i].ToString());
                    }
                }
                dr.Close();
            }
            catch (OleDbException oraExp)
            {
                Logger.LogMessageToFile(oraExp.Message, oraExp.StackTrace);
            }
            finally
            {
                if (OleDbConnection != null)
                {
                    OleDbConnection.Close();
                    OleDbConnection.Dispose();
                }
            }
            return list;
        }

        public DataTable executesql(string query)
        {
            return ExecuteReader(query);
        }

        public void ExcuteProceudre(string _sCircle, string _sFrmDate)
        {
            OleDbConnection OleDbConnection = new OleDbConnection();
            OleDbCommand OleDbCommand = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();

            try
            {
                OleDbConnection.ConnectionString = getConnectionString();                
                if (OleDbConnection.State == ConnectionState.Closed)
                {
                    DataSet ds = new DataSet();
                    OleDbConnection.Open();
                    OleDbCommand.CommandType = CommandType.StoredProcedure;
                    OleDbCommand = new OleDbCommand("AUTO_CASE_REMOVAL", OleDbConnection);
                    OleDbCommand.CommandTimeout = 0;

                    OleDbCommand.Parameters.Add("@P_CIRCLE", OleDbType.VarChar).Value = _sCircle;
                    OleDbCommand.Parameters.Add("@P_DATE", OleDbType.VarChar).Value = _sFrmDate;                    
                                   
                    OleDbCommand.ExecuteNonQuery();                                       
                    //da = new OleDbDataAdapter(OleDbCommand);
                    //da.Fill(ds);
                }                
            }
            finally
            {
                if (OleDbConnection.State == ConnectionState.Open)
                {
                    OleDbConnection.Close();
                    OleDbConnection.Dispose();
                }
            }
        }

        public DataTable ExecuteReaderMobapp(string sqlQuery)
        {
            DataTable dt = new DataTable();
            OleDbConnection OleDbConnection = new OleDbConnection();
            OleDbCommand OleDbCommand = new OleDbCommand();
            try
            {
                OleDbConnection.ConnectionString = getConnectionStringmobapp();
                OleDbConnection.Open();
                OleDbCommand.Connection = OleDbConnection;
                OleDbCommand.CommandText = sqlQuery;
                OleDbDataAdapter da = new OleDbDataAdapter(OleDbCommand);
                da.Fill(dt);
            }
            catch (OleDbException oraExp)
            {
                dt.TableName = "OutPutTable";
                dt.Columns.Add("ExData");
                DataRow dr = dt.NewRow();
                dt.Rows.Add("-1");
                dt.AcceptChanges();

                Logger.LogMessageToFile(oraExp.Message, oraExp.StackTrace);
            }
            finally
            {
                if (OleDbConnection != null)
                {
                    OleDbConnection.Close();
                    OleDbConnection.Dispose();
                }
            }
            return dt;
        }

        private static string getConnectionStringmobapp()
        {
            NDS ObjNDS = new NDS();
            return ObjNDS.ConMobapp();
        }
    }
}
