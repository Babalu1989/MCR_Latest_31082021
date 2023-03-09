using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.IO;
using System.Web.Hosting;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using aejw.Network;
using SimpleTest;


/// <summary>
/// Summary description for NewClassFile
/// </summary>
public class NewClassFile
{
	public NewClassFile()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    SimpleUtil objdbfun = SimpleUtil.Instance;

    

    #region "MCR Punching"

    public  DataTable CheckValid_UserID_Data(string strUserID, string strPassword)
    {
        string sql = string.Empty;

        sql = "  SELECT LOGIN_ID FROM MCR_LOGIN_MST WHERE UPPER(LOGIN_ID)=UPPER('" + strUserID + "') AND PASSWORD='" + strPassword + "' ";

        DataTable dt = new DataTable();
        
        dt = objdbfun.ExecuteReader(sql);

        return dt;
    }

    public  DataTable CheckValid_UserID_DTDetails(string strUserID, string strPassword)
    {
        string sql = string.Empty;

        sql = " SELECT EMP_NAME, EMP_ID, IMEI_NO, DIVISION, LOGIN_DATE, ROLE, MUD.ACTIVE_FLAG,LOGIN_TYPE FROM MCR_USER_DETAILS MUD, MCR_LOGIN_MST MLM WHERE MUD.EMP_ID= MLM.LOGIN_ID AND UPPER(EMP_ID)=UPPER('" + strUserID + "') AND PASSWORD='" + strPassword + "' ";

        DataTable dt = new DataTable();

        dt = objdbfun.ExecuteReader(sql);

        return dt;

    }

    public  DataTable Get_MCR_InputData_Details(string strLoginType, string strID, string stringLatitude, string stringLongitude)
    {
        string sql = string.Empty;

        if (strLoginType == "V")
        {
            sql = " SELECT AUART ORDER_TYPE, COMP_CODE, PSTING_DATE, ORDERID, METER_NO, DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, REQUEST_TYPE, POLE_NO, STICKER_NO, ACCOUNT_CLASS, SANCTIONED_LOAD, ZDSS,";
            sql += " PLANNER_GROUP, CABLE_SIZE, CABLE_LENGTH, MCR_PUNCH_FLAG, START_DATE, FINISH_DATE, ENTRY_DATE FROM MCR_INPUT_DETAILS WHERE VENDOR_CODE='" + strID + "' AND FLAG='N'";
        }
        else
        {
            sql = " SELECT AUART ORDER_TYPE, COMP_CODE, PSTING_DATE, ORDERID, METER_NO, DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, REQUEST_TYPE, POLE_NO, STICKER_NO, ACCOUNT_CLASS, SANCTIONED_LOAD, ZDSS, ";
            sql += " PLANNER_GROUP, CABLE_SIZE, CABLE_LENGTH, MCR_PUNCH_FLAG, START_DATE, FINISH_DATE, ENTRY_DATE FROM MCR_INPUT_DETAILS WHERE ORDERID IN (SELECT ORDER_NO FROM MCR_VEND_ORDER_INST_MAP WHERE INSTALLER_ID='" + strID + "')  AND FLAG='Y'";
        }

        bool isInserted = FillMCRMapDetails(strID, stringLatitude, stringLongitude);

        DataTable dt = new DataTable();

        dt = objdbfun.ExecuteReader(sql);

        return dt;

    }

    private  bool FillMCRMapDetails(string strUserId, string stringLatitude, string stringLongitude)
    {
        string sql = string.Empty;
        sql = "  INSERT INTO MCR_MAP_DTLS (USER_ID, LATITUDE, LONGITUDE) VALUES (?, ?, ?) ";

        NDS ndsCon = new NDS();
        OleDbConnection ocon_MobApp = new OleDbConnection(ndsCon.DcrepCon());

        try
        {
            if (ocon_MobApp.State == ConnectionState.Closed)
            {
                ocon_MobApp.Open();
            }
            OleDbCommand oleDbCommand = new OleDbCommand(sql, ocon_MobApp);
            OleDbParameter USER_ID = oleDbCommand.Parameters.Add("@USER_ID", OleDbType.VarChar, 20);
            OleDbParameter LATITUDE = oleDbCommand.Parameters.Add("@LATITUDE", OleDbType.VarChar, 60);
            OleDbParameter LONGITUDE = oleDbCommand.Parameters.Add("@LONGITUDE", OleDbType.VarChar, 60);

            USER_ID.Value = strUserId;
            LATITUDE.Value = stringLatitude;
            LONGITUDE.Value = stringLongitude;

            int intOut = oleDbCommand.ExecuteNonQuery();

            if (intOut != 0)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString() + sql);
            return true;
        }
        finally
        {
            if (ocon_MobApp.State == ConnectionState.Open)
            {
                ocon_MobApp.Close();
            }
        }

        return true;
    }

    public  string GetCAOnOrderNo(string oRDERNO)
    {
        string strSql = "SELECT CA_NO FROM MCR_INPUT_DETAILS WHERE orderid= '" + oRDERNO + "' ";
        DataTable dt = new DataTable();
        dt = objdbfun.ExecuteReader(strSql);

        if (dt.Rows.Count > 0)
            return dt.Rows[0]["CA_NO"].ToString();
        else
            return "";
    }

    public  bool Insert_MCR_IamgeList(string strOrderNo, string strStickerNo, string strDeviceNo, string strImage1, string strImage2, string strImage3,
                                            string strImageMCR, string strMtrRptImg, string strLabelRptImg, string strImgSignature, string strImage4, string strCaNo)
    {
        try
        {
            byte[] _byImg = null;
            string _strImage1 = string.Empty, _strImage2 = string.Empty, _strImage3 = string.Empty, _strImage4 = string.Empty, _strImageMCR = string.Empty;
            string _strMtrRptImg = string.Empty, _strLabelRptImg = string.Empty, _strImgSignature = string.Empty;

            string strCA = string.Empty;
            strCA = strCaNo;

            string strDate = DateTime.Now.ToString("ddMMyyyy");

            if (strImage1.Trim() != "")
            {
                try
                {
                    _byImg = Convert.FromBase64String(strImage1);
                    _strImage1 = byteArrayToImage(_byImg, strCA + strDate + "A");
                }
                catch (Exception ex)
                {
                    WriteIntoFile(DateTime.Now.ToString() + "Error in MCR image 1" + ex.ToString());
                }
            }

            if (strImage2.Trim() != "")
            {
                try
                {
                    _byImg = Convert.FromBase64String(strImage2);
                    _strImage2 = byteArrayToImage(_byImg, strCA + strDate + "B");
                }
                catch (Exception ex)
                {
                    WriteIntoFile(DateTime.Now.ToString() + "Error in MCR image 2" + ex.ToString());
                }
            }

            if (strImage3.Trim() != "")
            {
                try
                {
                    _byImg = Convert.FromBase64String(strImage3);
                    _strImage3 = byteArrayToImage(_byImg, strCA + strDate + "C");
                }
                catch (Exception ex)
                {
                    WriteIntoFile(DateTime.Now.ToString() + "Error in MCR image 3" + ex.ToString());
                }
            }

            if (strImageMCR.Trim() != "")
            {
                try
                {
                    _byImg = Convert.FromBase64String(strImageMCR);
                    _strImageMCR = byteArrayToImage(_byImg, strCA + strDate + "D");
                }
                catch (Exception ex)
                {
                    WriteIntoFile(DateTime.Now.ToString() + "Error in MCR image" + ex.ToString());
                }
            }

            if (strMtrRptImg.Trim() != "")
            {
                try
                {
                    _byImg = Convert.FromBase64String(strMtrRptImg);
                    _strMtrRptImg = byteArrayToImage(_byImg, strCA + strDate + "E");
                }
                catch (Exception ex)
                {
                    WriteIntoFile(DateTime.Now.ToString() + "Error in MCR Meter" + ex.ToString());
                }
            }

            if (strLabelRptImg.Trim() != "")
            {
                try
                {
                    _byImg = Convert.FromBase64String(strLabelRptImg);
                    _strLabelRptImg = byteArrayToImage(_byImg, strCA + strDate + "F");
                }
                catch (Exception ex)
                {
                    WriteIntoFile(DateTime.Now.ToString() + "Error in MCR Label" + ex.ToString());
                }
            }
            if (strImgSignature.Trim() != "")
            {
                try
                {
                    _byImg = Convert.FromBase64String(strImgSignature);
                    _strImgSignature = byteArrayToImage(_byImg, strCA + strDate + "G");
                }
                catch (Exception ex)
                {
                    WriteIntoFile(DateTime.Now.ToString() + "Error in MCR Signature" + ex.ToString());
                }
            }
            if (strImage4.Trim() != "")
            {
                try
                {
                    _byImg = Convert.FromBase64String(strImage4);
                    _strImage4 = byteArrayToImage(_byImg, strCA + strDate + "H");
                }
                catch (Exception ex)
                {
                    WriteIntoFile(DateTime.Now.ToString() + "Error in MCR image 4" + ex.ToString());
                }
            }

            try
            {
                string sqlinsert = "INSERT INTO MCR_IMAGE_DETAILS(ORDERID, STICKERNO, DEVICENO, IMAGE1, IMAGE2, IMAGE3, IMEAGE_MCR, IMAGE_METERTESTREPORT, IMAGE_LABTESTINGREPORT, IMAGE_SIGNATURE,IMAGE4 )";
                sqlinsert = sqlinsert + " values('" + strOrderNo.ToString().Trim() + "','" + strStickerNo.ToString().Trim() + "','" + strDeviceNo.ToString().Trim() + "',";
                sqlinsert = sqlinsert + " '" + _strImage1.ToString().Trim() + "','" + _strImage2.ToString().Trim() + "','" + _strImage3.ToString().Trim() + "',";
                sqlinsert = sqlinsert + " '" + _strImageMCR.ToString().Trim() + "','" + _strMtrRptImg.ToString().Trim() + "','" + _strLabelRptImg.ToString().Trim() + "','" + _strImgSignature.ToString().Trim() + "','" + _strImage4 + "')";

                if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
                return false;
            }
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    public bool Insert_MCR_InputData(string strOrderNo, string strDeviceNo, string strStickerNo, string strELCB_Inst, string strInstBusBar, string strBusBarSize, string strBusBarNo,
                                            string strDurmSize, string strActivityReason, string strCableSize, string strCableIntType, string strRunLenFrm, string strRunLenTo, string strCableLen,
                                            string strTerSeal1, string strTerSeal2, string strMtrBoxSeal1, string strMtrBoxSeal2, string strBusBarSeal1, string strBusBarSeal2,
                                            string strInstallcation, string strPoleNo, string strActDate, string strMR_KWH, string strMR_KW, string strMR_KVAH,
                                            string strMR_KVA, string strTakePhotGraph, string strMeterDownload, string strDBLock, string strEarthing,
                                            string strMeterHeight, string strAnyJoint, string strOverHeadCable, string strOverHeadCablePole, string strFlowmade,
                                            string strAdditionAccess, string strOutPutBusLen, string strOutPutCableLen, string strEarthingPole, string _gB_BAR_CABLE_SIZE,
                                            string strTAB_ID, string strTAB_NAME, string strGIS_LAT, string strGIS_LONG, string strGIS_STATUS, string strIMEI_NO)
    {
        try
        {
            string sqlinsert = "INSERT INTO MCR_DETAILS (ORDERID,DEVICENO,OTHERSTICKER,ELCB_INSTALLED,INSTALLEDBUSBAR,BUSBARSIZE,BUS_BAR_NO,DRUMSIZE,ACTIVITY_REASON, ";
            sqlinsert = sqlinsert + " CABLESIZE2,CABLEINSTALLTYPE,RUNNINGLENGTHFROM,RUNNINGLENGTHTO,CABLELENGTH,TERMINAL_SEAL,REM_TERMINAL_SEA, ";
            sqlinsert = sqlinsert + " METERBOXSEAL1,METERBOXSEAL2,BUSBARSEAL1,BUSBARSEAL2,INSTALLATION,POLENUMBER, ";
            sqlinsert = sqlinsert + " ACTIVITY_DATE,MR_KWH,MR_KW,MR_KVAH,MR_KVA,TAKEPHOTOGRAPH,METERDOWNLOAD,DBLOCKED,EARTHING,HEIGHTOFMETER, ";
            sqlinsert = sqlinsert + " ANYJOINTS,OVERHEADCABLE,OVERHEADCABLEPOLE,FLOWMADE,ADDITIONALACCESSORIES,OUTPUTBUSLENGTH,OUTPUTCABLELENGTH,EARTHINGPOLE, B_BAR_CABLE_SIZE,TAB_LOGIN_ID, TAB_LN_ID_NAME,GIS_LAT,GIS_LONG,GIS_STATUS,IMEI_NO) ";
            sqlinsert = sqlinsert + " VALUES('" + strOrderNo.ToString().Trim() + "','" + strDeviceNo.ToString().Trim() + "','" + strStickerNo.ToString().Trim() + "','" + strELCB_Inst.ToString().Trim() + "','" + strInstBusBar.ToString().Trim() + "',";
            sqlinsert = sqlinsert + " '" + strBusBarSize.ToString().Trim() + "', '" + strBusBarNo.ToString().Trim() + "','" + strDurmSize.ToString().Trim() + "','" + strActivityReason.ToString().Trim() + "','" + strCableSize.ToString().Trim() + "','" + strCableIntType.ToString().Trim() + "',";
            sqlinsert = sqlinsert + " '" + strRunLenFrm.ToString().Trim() + "','" + strRunLenTo.ToString().Trim() + "','" + strCableLen.ToString().Trim() + "','" + strTerSeal1.ToString().Trim() + "',";
            sqlinsert = sqlinsert + " '" + strTerSeal2.ToString().Trim() + "','" + strMtrBoxSeal1.ToString().Trim() + "','" + strMtrBoxSeal2.ToString().Trim() + "','" + strBusBarSeal1.ToString().Trim() + "','" + strBusBarSeal2.ToString().Trim() + "','" + strInstallcation.ToString().Trim() + "',";
            sqlinsert = sqlinsert + " '" + strPoleNo.ToString().Trim() + "',TO_DATE('" + strActDate + "','dd/MM/yyyy'),";
            sqlinsert = sqlinsert + " '" + strMR_KWH.ToString().Trim() + "','" + strMR_KW.ToString().Trim() + "','" + strMR_KVAH.ToString().Trim() + "','" + strMR_KVA.ToString().Trim() + "','" + strTakePhotGraph.ToString().Trim() + "','" + strMeterDownload.ToString().Trim() + "','" + strDBLock.ToString().Trim() + "','" + strEarthing.ToString().Trim() + "',";
            sqlinsert = sqlinsert + " '" + strMeterHeight.ToString().Trim() + "','" + strAnyJoint.ToString().Trim() + "','" + strOverHeadCable.ToString().Trim() + "','" + strOverHeadCablePole.ToString().Trim() + "',";
            sqlinsert = sqlinsert + " '" + strFlowmade.ToString().Trim() + "','" + strAdditionAccess.ToString().Trim() + "','" + strOutPutBusLen + "','" + strOutPutCableLen + "','" + strEarthingPole + "', '" + _gB_BAR_CABLE_SIZE + "',";
            sqlinsert = sqlinsert + " '" + strTAB_ID.ToString().Trim() + "','" + strTAB_NAME.ToString().Trim() + "','" + strGIS_LAT.ToString().Trim() + "','" + strGIS_LONG.ToString().Trim() + "','" + strGIS_STATUS.ToString().Trim() + "','" + strIMEI_NO.ToString().Trim() + "')";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;

        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }


    public DataTable CheckValid_SealDeails(string strSealNo)
    {
        string sql = string.Empty;

        sql = " SELECT DISTINCT CONSUM_SEAL_FLAG FROM MCR_SEAL_DETAILS WHERE SEAL='" + strSealNo + "' AND CONSUM_SEAL_FLAG='Y' ";

        DataTable dt = new DataTable();
        dt = objdbfun.ExecuteReader(sql);

        return dt;

    }

    public bool Update_MCRSealFlag(string strSealNo)
    {
        try
        {
            string sqlinsert = " UPDATE MCR_SEAL_DETAILS SET CONSUM_SEAL_FLAG='C', PUNCH_DATE=sysdate WHERE SEAL='" + strSealNo + "' ";
            
            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    public bool Insert_InstallerData(string strInstallerName, string strInstID, string strIMEI, string strDivision, string strVendorID)
    {
        try
        {
            string sqlinsert = " INSERT INTO MCR_USER_DETAILS (EMP_NAME, EMP_ID, IMEI_NO, DIVISION, EMP_TYPE, ROLE, VENDOR_ID, ACTIVE_FLAG) ";
            sqlinsert = sqlinsert + "  VALUES   ('" + strInstallerName + "', '" + strInstID + "', '" + strIMEI + "', '" + strDivision + "', 'I', 'USER', '" + strVendorID + "', 'Y') ";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    public bool Insert_InstallerLoginData(string strInstallerID)
    {
        try
        {
            string sqlinsert = "  INSERT INTO MCR_LOGIN_MST (LOGIN_ID, PASSWORD, OLD_PASSWORD, NEW_PASSWORD, LOGIN_TYPE) ";
            sqlinsert = sqlinsert + "       VALUES   ('" + strInstallerID + "', '1234', '1234', '1234', 'I') ";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    public DataTable GetInstaller_DataList_VendorWise(string strVendorID)
    {
        string sql = string.Empty;
        sql = " SELECT EMP_NAME,EMP_ID FROM MCR_USER_DETAILS WHERE EMP_TYPE='I' AND VENDOR_ID='" + strVendorID + "' ORDER BY EMP_NAME ";

        DataTable dt = new DataTable();

        dt = objdbfun.ExecuteReader(sql);

        return dt;

    }

    public bool Assign_OrderInstaller_InputData(string strOrder, string strVendorID)
    {
        try
        {
            string sqlinsert = " UPDATE MCR_INPUT_DETAILS SET FLAG='Y', ALLOCATE_DATE = SYSDATE,ALLOCATE_BY='" + strVendorID + "' WHERE ORDERID='" + strOrder + "' ";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    public bool PunchOrder_Installer_InputData(string strOrder, string strInstallerID, string strDeviceNo)
    {
        try
        {
            string sqlinsert = " UPDATE MCR_INPUT_DETAILS SET FLAG='C', PUNCH_DATE = SYSDATE,PUNCH_BY='" + strInstallerID + "' WHERE ORDERID='" + strOrder + "' AND METER_NO='" + strDeviceNo + "' ";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    public bool MapData_OrderInstaller_InputData(string strOrder, string strMeterNo, string strVendorID, string strInstallerID)
    {
        try
        {
            string sqlinsert = " INSERT INTO MCR_VEND_ORDER_INST_MAP (ORDER_NO, METER_NO, VENDOR_CODE, INSTALLER_ID, ORDER_TYPE) ";
            sqlinsert = sqlinsert + "    VALUES  ('" + strOrder + "', '" + strMeterNo + "', '" + strVendorID + "', '" + strInstallerID + "', 'ZDIN') ";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }   

    public bool Update_PasswordData(string UserID, string OldPass, string NewPass)
    {
        try
        {
            string sqlinsert = " UPDATE MCR_LOGIN_MST SET PASSWORD='" + NewPass + "' WHERE LOGIN_ID ='" + UserID + "'";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    /// <summary>
    /// Developed by Gourav goutam on Date 15.11.2017 guide by swati kaushik,
    /// Developed for send Alloted Seal from Oracle Database to Mobile App
    /// </summary>
    /// <param name="strVendorID"></param>
    /// <returns></returns>
    /// 
    public DataTable GetSeal_Details_AllotedFrom_Vendor(string Installer_ID)
    {
        string sql = string.Empty;
        sql = " SELECT seal, SERIAL_NO FROM MCR_SEAL_DETAILS WHERE  CONSUM_SEAL_FLAG='Y' AND ALLOTED_TO='" + Installer_ID + "' ORDER BY seal ";

        DataTable dt = new DataTable();
       
        dt = objdbfun.ExecuteReader(sql);

        return dt;

    }

    public bool Update_OrderStatus(string Order_ID, string _gInstallerID)
    {
        try
        {
            string sqlinsert = " UPDATE MCR_INPUT_DETAILS SET FLAG='E', PUNCH_BY='" + _gInstallerID + "', PUNCH_DATE=sysdate WHERE ORDERID='" + Order_ID + "'";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    public bool insert_OrderCancelDetails(string _gORDERID, string _gREASON, string _gINSTALLER_ID, string _gREMARKS, string _gImagePath)
    {
        byte[] _byImg = null;
        if (_gImagePath.Trim() != "")
        {
            try
            {
                _byImg = Convert.FromBase64String(_gImagePath);
                _gImagePath = byteArrayToImage(_byImg, _gORDERID + "_Img_Cancel_order");
            }
            catch (Exception ex)
            {
                WriteIntoFile(DateTime.Now.ToString() + "Error in MCR Cancel Order" + ex.ToString());
            }
        }

        try
        {
            string sqlinsert = " INSERT INTO mcr_order_cancel(ORDERID, REASON, INSTALLER_ID, REMARKS, IMAGE_PATH) VALUES('" + _gORDERID + "','" + _gREASON + "','" + _gINSTALLER_ID + "','" + _gREMARKS + "', '" + _gImagePath + "') ";

            if (objdbfun.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return false;
        }
    }

    public void WriteIntoFile(string _smsg)
    {
        using (StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath(@"~\App_Data\ApplicationLog.txt")))
        {
            sw.WriteLine(_smsg);
            sw.Close();
        }
    }

    public string byteArrayToImage(byte[] byteArrayIn, string filename) //22012015
    {
        try
        {
            DirectoryInfo _DirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/UPLOADEDIMAGES") + "\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString());
            if (_DirInfo.Exists == false)
                _DirInfo.Create();

            string _sPath = _DirInfo.FullName + "\\" + filename + ".jpeg";
            string _sFileNameWithoutExt = _DirInfo.FullName + "\\" + filename;
            int _iFileIndex = 1;
            while (File.Exists(_sPath))
            {
                _sPath = _sFileNameWithoutExt + "_" + _iFileIndex.ToString() + ".jpeg";
                _iFileIndex++;
            }

            BinaryWriter fs = new BinaryWriter(new FileStream(_sPath, FileMode.Append, FileAccess.Write));
            fs.Write(byteArrayIn);
            fs.Close();
            return _sPath;
        }
        catch (Exception ex)
        {
            WriteIntoFile(DateTime.Now.ToString() + ex.ToString());
            return "";
        }
    }
   

    #endregion
}