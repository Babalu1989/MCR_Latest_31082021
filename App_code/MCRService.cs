using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Configuration;
using System.Data.OleDb;
using System.Reflection;
using System.Linq;
using System.Data;

/// <summary>
/// Summary description for MCRService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class MCRService : System.Web.Services.WebService {

    public MCRService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    NewClassFile _ObjClass = new NewClassFile();

    public UserCredentials consumer;

    #region "User Authontication"

    public bool checkConsumer(string _sFunName)
    {
        // In this method you can check the username and password 
        // with your database or something
        // You could also encrypt the password for more security

        //if (consumer != null)
        //{
        //    if (NewClassFile.getWebSericeAccess(consumer.userName, consumer.password, _sFunName))
        return true;
        //    else
        //        return false;
        //}
        //else
        //    return false;
    }

    private void InsertCallWidFunction(string strWebMthdId, string ipAddressAll, string strSessionId, string strUserName, string strMethodName)
    {

        //string strDt = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
        //strDt = "to_date('" + strDt + "','dd-Mon-yyyy hh24:mi:ss')";

        //NewClassFile.insertUserCallWidFunction(strWebMthdId, ipAddressAll, strSessionId, strDt, strUserName, strMethodName);

    }

    private void UpdateOutputCallWidFunction(string strSessionId, string strWebMthdId, string strWebMethodName)
    {
        //string strDt = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
        //strDt = "to_date('" + strDt + "','dd-Mon-yyyy hh24:mi:ss')";

        //NewClassFile.updateUserCallWidFunction(strDt, strSessionId, strWebMthdId, strWebMethodName);
    }

    private DataTable InvaildAuthontication()
    {
        DataTable dterror = new DataTable();
        dterror.TableName = "AuthonticationTable";
        dterror.Columns.Add("AUTHONTICATION");
        dterror.Rows.Add("INVAILD");
        return dterror;
    }

    private DataSet InvaildAuthonticationds()
    {
        DataSet ds = new DataSet("AUTHONTICATION");
        DataTable dterror = new DataTable();
        dterror.TableName = "AuthonticationTable";
        dterror.Columns.Add("AUTHONTICATION");
        dterror.Rows.Add("INVAILD");
        ds.Tables.Add(dterror);
        return ds;
    }

    private DataSet InvaildAppCodeU01()
    {
        DataSet ds = new DataSet("BAPI_RESULT");

        //outputFlagsTable
        DataTable dtOutputFlag = new DataTable();
        dtOutputFlag.TableName = "outputFlagsTable";
        dtOutputFlag.Columns.Add("E_Flag_Ap");
        dtOutputFlag.Columns.Add("E_Flag_Bp");
        dtOutputFlag.Columns.Add("E_Flag_So");
        dtOutputFlag.Columns.Add("E_Flag_Us");
        dtOutputFlag.Columns.Add("E_New_Partner");
        dtOutputFlag.Columns.Add("E_Service_Order");
        DataRow row = dtOutputFlag.NewRow();
        row["E_Flag_Ap"] = string.Empty;
        row["E_Flag_Bp"] = string.Empty;
        row["E_Flag_So"] = string.Empty;
        row["E_Flag_Us"] = string.Empty;
        row["E_New_Partner"] = string.Empty;
        row["E_Service_Order"] = "We have released new version 2.1 to avail services please download our latest app from website or play store.";
        dtOutputFlag.Rows.Add(row);

        //SAPDATA_ErrorDataTable
        DataTable dterror = new DataTable();
        dterror.TableName = "SAPDATA_ErrorDataTable";
        dterror.Columns.Add("Type");
        dterror.Columns.Add("Id");
        dterror.Columns.Add("Number");
        dterror.Columns.Add("Message");
        dterror.Columns.Add("Log_No");
        dterror.Columns.Add("Log_Msg_No");
        dterror.Columns.Add("Message_V1");
        dterror.Columns.Add("Message_V2");
        dterror.Columns.Add("Message_V3");
        dterror.Columns.Add("Message_V4");
        dterror.Columns.Add("Parameter");
        dterror.Columns.Add("Row");
        dterror.Columns.Add("Field");
        dterror.Columns.Add("System");

        DataRow rowError = dterror.NewRow();
        rowError["Type"] = "E";
        rowError["Id"] = string.Empty;
        rowError["Number"] = "000";
        rowError["Message"] = "We have released new version 2.1 to avail services please download our latest app from website or play store.";
        rowError["Log_No"] = string.Empty;
        rowError["Log_Msg_No"] = "000000";
        rowError["Message_V1"] = string.Empty;
        rowError["Message_V2"] = string.Empty;
        rowError["Message_V3"] = string.Empty;
        rowError["Message_V4"] = string.Empty;
        rowError["Parameter"] = string.Empty;
        rowError["Row"] = "0";
        rowError["Field"] = string.Empty;
        rowError["System"] = string.Empty;
        dterror.Rows.Add(rowError);

        ds.Tables.Add(dtOutputFlag);
        ds.Tables.Add(dterror);

        return ds;
    }

    private DataSet InvaildAppCode()
    {
        DataSet ds = new DataSet("BAPI_RESULT");

        //outputFlagsTable
        DataTable dtOutputFlag = new DataTable();
        dtOutputFlag.TableName = "outputFlagsTable";
        dtOutputFlag.Columns.Add("E_Flag_Ap");
        dtOutputFlag.Columns.Add("E_Flag_Bp");
        dtOutputFlag.Columns.Add("E_Flag_So");
        dtOutputFlag.Columns.Add("E_Flag_Us");
        dtOutputFlag.Columns.Add("E_New_Partner");
        dtOutputFlag.Columns.Add("E_Service_Order");
        DataRow row = dtOutputFlag.NewRow();
        row["E_Flag_Ap"] = string.Empty;
        row["E_Flag_Bp"] = string.Empty;
        row["E_Flag_So"] = string.Empty;
        row["E_Flag_Us"] = string.Empty;
        row["E_New_Partner"] = string.Empty;
        row["E_Service_Order"] = string.Empty;
        dtOutputFlag.Rows.Add(row);

        //SAPDATA_ErrorDataTable
        DataTable dterror = new DataTable();
        dterror.TableName = "SAPDATA_ErrorDataTable";
        dterror.Columns.Add("Type");
        dterror.Columns.Add("Id");
        dterror.Columns.Add("Number");
        dterror.Columns.Add("Message");
        dterror.Columns.Add("Log_No");
        dterror.Columns.Add("Log_Msg_No");
        dterror.Columns.Add("Message_V1");
        dterror.Columns.Add("Message_V2");
        dterror.Columns.Add("Message_V3");
        dterror.Columns.Add("Message_V4");
        dterror.Columns.Add("Parameter");
        dterror.Columns.Add("Row");
        dterror.Columns.Add("Field");
        dterror.Columns.Add("System");

        DataRow rowError = dterror.NewRow();
        rowError["Type"] = "E";
        rowError["Id"] = string.Empty;
        rowError["Number"] = "000";
        rowError["Message"] = "We have released new version 2.1 to avail services please download our latest app from website or play store.";
        rowError["Log_No"] = string.Empty;
        rowError["Log_Msg_No"] = "000000";
        rowError["Message_V1"] = string.Empty;
        rowError["Message_V2"] = string.Empty;
        rowError["Message_V3"] = string.Empty;
        rowError["Message_V4"] = string.Empty;
        rowError["Parameter"] = string.Empty;
        rowError["Row"] = "0";
        rowError["Field"] = string.Empty;
        rowError["System"] = string.Empty;
        dterror.Rows.Add(rowError);

        ds.Tables.Add(dtOutputFlag);
        ds.Tables.Add(dterror);

        return ds;
    }

    private DataSet NotAvail_ds()
    {
        DataSet ds = new DataSet("Service");
        DataTable dterror = new DataTable();
        dterror.TableName = "ServicesResult";
        dterror.Columns.Add("Result");
        dterror.Rows.Add("OOPs! Not Available");
        ds.Tables.Add(dterror);
        return ds;
    }

    #endregion

    #region MCR Punching


    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public DataTable MCR_GetUserLoginDetails(string strUser, string strPassword)
    {
        if (checkConsumer(MethodBase.GetCurrentMethod().Name))
        {
            DataTable dt = new DataTable();
            dt = _ObjClass.CheckValid_UserID_DTDetails(strUser, strPassword);
            dt.TableName = "MCR_USER_DETAILS";
            return dt;
        }
        else
            return (InvaildAuthontication());
    }

    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public DataTable MCR_GetUserMCR_INPUT_DT(string strUserType, string strUser, string stringLatitude, string stringLongitude)
    {
        if (checkConsumer(MethodBase.GetCurrentMethod().Name))
        {
            DataTable dt = new DataTable();
            dt = _ObjClass.Get_MCR_InputData_Details(strUserType, strUser, stringLatitude, stringLongitude);
            dt.TableName = "MCR_INPUT_DETAILS";
            return dt;
        }
        else
            return (InvaildAuthontication());
    }

    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public Boolean MCR_Insert_INPUT_Data(string ORDERNO, string DEVICENO, string OTHERSTICKER, string OTHER4, string INSTALLEDBUSBAR, string BUSBARSIZE,
                    string BUSBARNUMBER, string DRUMSIZE, string OTHER5, string CABLESIZE2, string CABLEINSTALLTYPE, string RUNNINGLENGTHFROM, string RUNNINGLENGTHTO,
                    string CABLELENGTH, string TERMINALSEAL1, string TERMINALSEAL2, string METERBOXSEAL1, string METERBOXSEAL2, string BUSBARSEAL1,
                    string BUSBARSEAL2, string INSTALLEDLOCATION, string POLENUMBER, string OTHER6, string OTHER7, string OTHER8, string OTHER9, string OTHER10, string OTHER11,
                    string TAKEPHOTOGRAPH, string METERDOWNLOAD, string DBLOCKED, string EARTHING, string HEIGHTOFMETER, string ANYJOINTS, string OVERHEADCABLE,
                    string OVERHEADCABLEPOLE, string FLOWMADE, string ADDITIONALACCESSORIES, string IMAGE1, string IMAGE2, string IMAGE3,
                    string IMEAGE_MCR, string IMAGE_METERTESTREPORT, string IMAGE_LABTESTINGREPORT, string IMAGE_SIGNATURE,
                    string OUTPUTBUSLENGTH, string OUTPUTCABLELENGTH, string EARTHINGPOLE, string IMAGE4, string PUNCHED,
                    string TAB_ID, string TAB_NAME, string GIS_LAT, string GIS_LONG, string GIS_STATUS, string IMEI_NO)
    {

        _ObjClass.Insert_MCR_InputData(ORDERNO, DEVICENO, OTHERSTICKER, OTHER4, INSTALLEDBUSBAR, BUSBARSIZE, BUSBARNUMBER, DRUMSIZE, OTHER5, CABLESIZE2, CABLEINSTALLTYPE, RUNNINGLENGTHFROM, RUNNINGLENGTHTO,
                                                CABLELENGTH, TERMINALSEAL1, TERMINALSEAL2, METERBOXSEAL1, METERBOXSEAL2, BUSBARSEAL1, BUSBARSEAL2, INSTALLEDLOCATION, POLENUMBER, OTHER6,
                                                OTHER7, OTHER8, OTHER9, OTHER10, TAKEPHOTOGRAPH, METERDOWNLOAD, DBLOCKED, EARTHING, HEIGHTOFMETER, ANYJOINTS, OVERHEADCABLE, OVERHEADCABLEPOLE, FLOWMADE,
                                                ADDITIONALACCESSORIES, OUTPUTBUSLENGTH, OUTPUTCABLELENGTH, EARTHINGPOLE, PUNCHED,
                                                TAB_ID, TAB_NAME, GIS_LAT, GIS_LONG, GIS_STATUS, IMEI_NO);

        _ObjClass.Update_MCRSealFlag(TERMINALSEAL1);

        if (TERMINALSEAL2 != "")
        {
            _ObjClass.Update_MCRSealFlag(TERMINALSEAL2);
        }
        if (METERBOXSEAL1 != "")
        {
            _ObjClass.Update_MCRSealFlag(METERBOXSEAL1);
        }
        if (METERBOXSEAL2 != "")
        {
            _ObjClass.Update_MCRSealFlag(METERBOXSEAL2);
        }
        if (BUSBARSEAL1 != "")
        {
            _ObjClass.Update_MCRSealFlag(BUSBARSEAL1);
        }
        if (BUSBARSEAL2 != "")
        {
            _ObjClass.Update_MCRSealFlag(BUSBARSEAL2);
        }

        string strCaNo = _ObjClass.GetCAOnOrderNo(ORDERNO);

        _ObjClass.Insert_MCR_IamgeList(ORDERNO, OTHERSTICKER, DEVICENO, IMAGE1, IMAGE2, IMAGE3,
                                           IMEAGE_MCR, IMAGE_METERTESTREPORT, IMAGE_LABTESTINGREPORT,
                                           IMAGE_SIGNATURE, IMAGE4, strCaNo);

        return _ObjClass.PunchOrder_Installer_InputData(ORDERNO, OTHER11, DEVICENO);

    }


    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public string MCR_ValidateSEAL(string SealNo)
    {
        if (checkConsumer(MethodBase.GetCurrentMethod().Name))
        {
            DataTable dt = new DataTable();
            dt = _ObjClass.CheckValid_SealDeails(SealNo);
            dt.TableName = "MCR_SEAL_DETAILS";

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0] != null)
                {
                    if (dt.Rows[0][0].ToString() == "Y")
                        return "T";
                    else
                        return "C";
                }
                else
                    return "F";
            }
            else
                return "F";
        }
        else
            return "F";
    }

    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public string MCR_Create_Installer_Data(string INSTALLER_NAME, string INSTALLER_ID, string IMEI, string DIVISION, string VENDOR_ID)
    {
        _ObjClass.Insert_InstallerData(INSTALLER_NAME, INSTALLER_ID, IMEI, DIVISION, VENDOR_ID);
        _ObjClass.Insert_InstallerLoginData(INSTALLER_ID);

        return "T";
    }

    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public DataTable MCR_GetInstaller_Data(string VENDOR_ID)
    {
        if (checkConsumer(MethodBase.GetCurrentMethod().Name))
        {
            DataTable dt = new DataTable();
            dt = _ObjClass.GetInstaller_DataList_VendorWise(VENDOR_ID);
            dt.TableName = "MCR_USER_DETAILS";
            return dt;
        }
        else
            return (InvaildAuthontication());
    }

    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public string MCR_OrderAssign(string ORDERNO, string METERNO, string VENDOR_ID, string INSTALLER_ID)
    {
        _ObjClass.Assign_OrderInstaller_InputData(ORDERNO, VENDOR_ID);
        _ObjClass.MapData_OrderInstaller_InputData(ORDERNO, METERNO, VENDOR_ID, INSTALLER_ID);

        return "T";
    }

    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public string MCR_Password_Update(string strEmpId, string strOldPass, string strNewPass)
    {
        _ObjClass.Update_PasswordData(strEmpId, strOldPass, strNewPass);
        return "T";
    }


    /// <summary>
    /// Developed by Gourav goutam on Date 15.11.2017 guide by swati kaushik,
    /// Developed for send Alloted Seal from Oracle Database to Mobile App
    /// </summary>
    /// <param name="strVendorID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public DataTable MCR_GetSeal_Details(string Installer_ID)
    {
        if (checkConsumer(MethodBase.GetCurrentMethod().Name))
        {
            DataTable dt = new DataTable();
            dt = _ObjClass.GetSeal_Details_AllotedFrom_Vendor(Installer_ID);
            dt.TableName = "MCR_SEAL_DETAILS";
            return dt;
        }
        else
            return (InvaildAuthontication());
    }

    [WebMethod(EnableSession = true)]
    [SoapHeader("consumer", Required = true)]
    public string MCR_OrderCancel(string Installer_ID, string Order_ID, string Reason, string Image, string Remarks)
    {
        _ObjClass.Update_OrderStatus(Order_ID, Installer_ID);
        _ObjClass.insert_OrderCancelDetails(Order_ID, Reason, Installer_ID, Remarks, Image);

        return "T";
    }





    #endregion
    
}
[Serializable]
public class UserCredentials : System.Web.Services.Protocols.SoapHeader
{
    public string userName;
    public string password;

}
