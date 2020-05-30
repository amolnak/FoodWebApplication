using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections.Specialized;
using System.Collections;

/// <summary>
/// Summary description for clsDatabase
/// </summary>
public class clsDatabase
{
    private static string GetDBConn()
    {
        return ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
    }

    public static bool ExecuteNonQuery(string SQL, ref string ErrMsg, int TimeOut = 6120, Hashtable SP_Param = null, SqlConnection objconn = null, SqlTransaction objtrans = null)
    {
        ErrMsg = "";
        using (SqlConnection conn = new SqlConnection(GetDBConn()))
        {
            SqlTransaction otrans = null;
            bool blnRetVal = false;
            try
            {
                if (objconn == null)
                {
                    conn.Open();
                }

                if (objconn == null)
                {
                    if (objtrans != null)
                    {
                        objtrans = conn.BeginTransaction();
                    }
                    else
                    {
                        otrans = conn.BeginTransaction();
                    }
                }
                else
                {
                    if (objtrans == null)
                    {
                        otrans = objconn.BeginTransaction();
                    }
                }

                SqlCommand cmd;
                if (objconn == null)
                {
                    cmd = new SqlCommand(SQL, conn);
                }
                else
                {
                    cmd = new SqlCommand(SQL, objconn);
                }

                cmd.CommandType = CommandType.Text;

                if (SP_Param != null && SP_Param.Count > 0)
                {
                    foreach (DictionaryEntry keyValue in SP_Param)
                    {
                        cmd.Parameters.AddWithValue(keyValue.Key.ToString().Trim(), keyValue.Value);
                    }
                }

                if (objtrans != null)
                {
                    cmd.Transaction = objtrans;
                }
                else
                {
                    cmd.Transaction = otrans;
                }

                cmd.CommandTimeout = TimeOut;
                cmd.ExecuteNonQuery();

                if (otrans != null)
                {
                    otrans.Commit();
                }

                if (objconn == null)
                {
                    conn.Close();
                }

                blnRetVal = true;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["ExceptionMessage"] = ex.ToString();
                ErrMsg = ex.Message.ToString().Trim();
                if (otrans != null) otrans.Rollback();
            }
            return blnRetVal;
        }
    }

    public static double ExecuteScalarQuery(string SQL, ref string ErrMsg, int TimeOut = 5400, Hashtable SP_Param = null, SqlConnection objconn = null, SqlTransaction objtrans = null)
    {
        ErrMsg = "";
        using (SqlConnection conn = new SqlConnection(GetDBConn()))
        {
            SqlTransaction otrans = null;
            double dblRetVa = 0;
            try
            {
                if (objconn == null)
                {
                    conn.Open();
                }

                if (objconn == null)
                {
                    if (objtrans != null)
                    {
                        objtrans = conn.BeginTransaction();
                    }
                    else
                    {
                        otrans = conn.BeginTransaction();
                    }
                }
                else
                {
                    if (objtrans == null)
                    {
                        otrans = objconn.BeginTransaction();
                    }
                }

                SqlCommand cmd;
                if (objconn == null)
                {
                    cmd = new SqlCommand(SQL, conn);
                }
                else
                {
                    cmd = new SqlCommand(SQL, objconn);
                }
                cmd.CommandType = CommandType.Text;

                if (SP_Param != null && SP_Param.Count > 0)
                {
                    foreach (DictionaryEntry keyValue in SP_Param)
                    {
                        cmd.Parameters.AddWithValue(keyValue.Key.ToString().Trim(), keyValue.Value);
                    }
                }

                if (objtrans != null)
                {
                    cmd.Transaction = objtrans;
                }
                else
                {
                    cmd.Transaction = otrans;
                }

                cmd.CommandTimeout = TimeOut;
                dblRetVa = double.Parse(cmd.ExecuteScalar().ToString());

                if (otrans != null)
                {
                    otrans.Commit();
                }

                if (objconn == null)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["ExceptionMessage"] = ex.ToString();
                ErrMsg = ex.Message.ToString().Trim();
                if (otrans != null) otrans.Rollback();
            }
            return dblRetVa;
        }
    }

    /// <summary>
    /// Returns the primaryID of the record. Pass IsUpd as 'False' for Insert Statement and 'True' for Update Statement.
    /// </summary>
    /// <param name="strQry"></param>
    /// <param name="IsUpdate"></param>
    /// <returns></returns>
    public static double IExecuteNonQuery(string strQry, ref string ErrMsg, bool IsUpdate = false, Hashtable SP_Param = null, SqlConnection objconn = null, SqlTransaction objtrans = null)
    {
        double dblRetVa = 0;
        if (strQry.Length > 0)
        {
            SqlTransaction otrans = null;
            using (SqlConnection conn = new SqlConnection(GetDBConn()))
            {
                try
                {
                    if (objconn == null)
                    {
                        conn.Open();
                    }

                    if (objconn == null)
                    {
                        if (objtrans != null)
                        {
                            objtrans = conn.BeginTransaction();
                        }
                        else
                        {
                            otrans = conn.BeginTransaction();
                        }
                    }
                    else
                    {
                        if (objtrans == null)
                        {
                            otrans = objconn.BeginTransaction();
                        }
                    }

                    if (!IsUpdate)
                    {
                        SqlCommand cmd;
                        if (objconn == null)
                        {
                            cmd = new SqlCommand(strQry + "; SELECT CAST(scope_identity() AS numeric);--", conn);
                        }
                        else
                        {
                            cmd = new SqlCommand(strQry + "; SELECT CAST(scope_identity() AS numeric);--", objconn);
                        }

                        cmd.CommandType = CommandType.Text;

                        if (SP_Param != null && SP_Param.Count > 0)
                        {
                            foreach (DictionaryEntry keyValue in SP_Param)
                            {
                                cmd.Parameters.AddWithValue(keyValue.Key.ToString().Trim(), keyValue.Value);
                            }
                        }

                        if (objtrans != null)
                        {
                            cmd.Transaction = objtrans;
                        }
                        else
                        {
                            cmd.Transaction = otrans;
                        }
                        dblRetVa = double.Parse(cmd.ExecuteScalar().ToString());
                    }
                    else
                    {
                        SqlCommand cmd;
                        if (objconn == null)
                        {
                            cmd = new SqlCommand(strQry, conn);
                        }
                        else
                        {
                            cmd = new SqlCommand(strQry, objconn);
                        }
                        cmd.CommandType = CommandType.Text;

                        if (SP_Param != null && SP_Param.Count > 0)
                        {
                            foreach (DictionaryEntry keyValue in SP_Param)
                            {
                                cmd.Parameters.AddWithValue(keyValue.Key.ToString().Trim(), keyValue.Value);
                            }
                        }

                        if (objtrans != null)
                        {
                            cmd.Transaction = objtrans;
                        }
                        else
                        {
                            cmd.Transaction = otrans;
                        }
                        dblRetVa = double.Parse(cmd.ExecuteNonQuery().ToString());
                    }

                    if (otrans != null)
                    {
                        otrans.Commit();
                    }

                    if (objconn == null)
                    {
                        conn.Close();
                    }
                    return dblRetVa;
                }
                catch (Exception ex) { HttpContext.Current.Session["ExceptionMessage"] = ex.ToString(); ErrMsg = ex.Message.ToString().Trim(); if (otrans != null) otrans.Rollback(); return dblRetVa; }
            }
        }
        return dblRetVa;
    }

    public static string GetSingleValue(string SQL, ref string ErrMsg, Hashtable SP_Param = null, SqlConnection objconn = null, SqlTransaction objtrans = null)
    {
        ErrMsg = "";
        using (SqlConnection conn = new SqlConnection(GetDBConn()))
        {
            string result = "";
            try
            {
                if (objconn == null)
                {
                    conn.Open();
                }
                if (objconn == null)
                {
                    if (objtrans != null)
                    {
                        objtrans = conn.BeginTransaction();
                    }
                }
                SqlCommand cmd;
                if (objconn == null)
                {
                    cmd = new SqlCommand(SQL, conn);
                }
                else
                {
                    cmd = new SqlCommand(SQL, objconn);
                }
                cmd.CommandType = CommandType.Text;

                if (SP_Param != null && SP_Param.Count > 0)
                {
                    foreach (DictionaryEntry keyValue in SP_Param)
                    {
                        cmd.Parameters.AddWithValue(keyValue.Key.ToString().Trim(), keyValue.Value);
                    }
                }

                if (objtrans != null)
                {
                    cmd.Transaction = objtrans;
                }

                object resultObj = cmd.ExecuteScalar();

                result = (resultObj == null) ? "" : resultObj.ToString();

                if (objconn == null)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["ExceptionMessage"] = ex.ToString(); ErrMsg = ex.Message.ToString();              
            }

            return result;
        }
    }

    public static DataTable GetDT(string SQL, ref string ErrMsg, Hashtable SP_Param = null, SqlConnection objconn = null, SqlTransaction objtrans = null)
    {
        ErrMsg = "";
        using (SqlConnection conn = new SqlConnection(GetDBConn()))
        {
            DataTable result = new DataTable();
            try
            {
                if (objconn == null)
                {
                    conn.Open();
                }

                if (objconn == null)
                {
                    if (objtrans != null)
                    {
                        objtrans = conn.BeginTransaction();
                    }
                }

                SqlCommand cmd;
                if (objconn == null)
                {
                    cmd = new SqlCommand(SQL, conn);
                }
                else
                {
                    cmd = new SqlCommand(SQL, objconn);
                }
                cmd.CommandType = CommandType.Text;

                if (SP_Param != null && SP_Param.Count > 0)
                {
                    foreach (DictionaryEntry keyValue in SP_Param)
                    {
                        cmd.Parameters.AddWithValue(keyValue.Key.ToString().Trim(), keyValue.Value);
                    }
                }

                if (objtrans != null)
                {
                    cmd.Transaction = objtrans;
                }
                //cmd.ExecuteNonQuery();//Commented by Amol Naik to remove unwanted execution. 

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand = cmd;
                da.Fill(result);

                if (objconn == null)
                {
                    conn.Close();
                }
            }
            catch (Exception ex) { HttpContext.Current.Session["ExceptionMessage"] = ex.ToString(); ErrMsg = ex.Message.ToString(); }
            return result;
        }
    }

    public static IDataReader GetRS(string SQL)
    {
        SqlConnection conn = new SqlConnection(GetDBConn());
        conn.Open();
        SqlCommand cmd = new SqlCommand(SQL, conn);
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public clsDatabase()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}