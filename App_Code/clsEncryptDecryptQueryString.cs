using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for clsEncryptDecryptQueryString
/// </summary>
public static class clsEncryptDecryptQueryString
{
    //public clsEncryptDecryptQueryString()
    //{		//
    //    // TODO: Add constructor logic here
    //    //
    //}

    private static byte[] key = { };
    private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
    public static string Decrypt(string stringToDecrypt)
    {
        //stringToDecrypt = HttpContext.Current.Server.UrlDecode(stringToDecrypt).Trim();
        stringToDecrypt = HttpUtility.UrlDecode(stringToDecrypt, HttpContext.Current.Response.ContentEncoding);

        byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
        try
        {
            string sEncryptionKey = ConfigurationManager.AppSettings["QueryStringEncryptionKey"].ToString();
            key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt.Replace(' ', '+'));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
              des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string Encrypt(string stringToEncrypt)
    {
        try
        {
            string sEncryptionKey = ConfigurationManager.AppSettings["QueryStringEncryptionKey"].ToString();
            key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
              des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
           // return HttpContext.Current.Server.UrlEncode(Convert.ToBase64String(ms.ToArray()));
            return HttpUtility.UrlEncode(Convert.ToBase64String(ms.ToArray()), HttpContext.Current.Response.ContentEncoding);
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

}