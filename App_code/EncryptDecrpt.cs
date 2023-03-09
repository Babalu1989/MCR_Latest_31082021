using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for EncryptDecrpt
/// </summary>
public class EncryptDecrpt
{
	public EncryptDecrpt()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    // creat decrypt method//
    public string DecryptString(string inputString)
    {
        MemoryStream memStream = null;
        try
        {
            byte[] key = { };
            byte[] IV = { 12, 21, 43, 17, 57, 35, 67, 27 };
            string encryptKey = "aXb2uy4z";
            key = Encoding.UTF8.GetBytes(encryptKey);
            byte[] byteInput = new byte[inputString.Length];
            byteInput = Convert.FromBase64String(inputString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            memStream = new MemoryStream();
            ICryptoTransform transform = provider.CreateDecryptor(key, IV);
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(byteInput, 0, byteInput.Length);
            cryptoStream.FlushFinalBlock();
        }
        catch (Exception ex)
        {
           // Response.Write(ex.Message);
        }
        Encoding encoding1 = Encoding.UTF8;
        return encoding1.GetString(memStream.ToArray());
    }
    //------------//

    //--encrypt code--//
    public string EncryptString(string inputString)
    {
        MemoryStream memStream = null;
        try
        {
            byte[] key = { };
            byte[] IV = { 12, 21, 43, 17, 57, 35, 67, 27 };
            string encryptKey = "aXb2uy4z";
            key = Encoding.UTF8.GetBytes(encryptKey);
            byte[] byteInput = Encoding.UTF8.GetBytes(inputString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            memStream = new MemoryStream();
            ICryptoTransform transform = provider.CreateEncryptor(key, IV);
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(byteInput, 0, byteInput.Length);
            cryptoStream.FlushFinalBlock();

        }
        catch (Exception ex)
        {
           // Response.Write(ex.Message);
        }
        return Convert.ToBase64String(memStream.ToArray());
    }
    //---------//
    public string RandomNo()
    {
        int r = 0;
        Random rnd = new Random();
        r = rnd.Next();
        string val = r.ToString();
        return val;
    }
}